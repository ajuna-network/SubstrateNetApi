using Chaos.NaCl;
using NLog;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;
using System;
using System.Net.Sockets;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SubstrateNetWallet
{
    public class Wallet
    {
        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private const string WEBSOCKETURL = "wss://node01.dotmog.com";

        private const string DefaultWalletFile = "wallet.dat";

        private readonly string _path;

        private WalletFile _walletFile;

        private Random _random = new Random();

        private SubstrateClient _client;

        private CancellationTokenSource _connectTokenSource;

        public bool IsUnlocked => Account != null;

        public bool IsCreated => _walletFile != null;

        public Account Account { get; private set; }

        public ChainInfo ChainInfo { get; private set; }

        public bool IsConnected => _client.IsConnected;

        public bool IsOnline => IsConnected && _subscriptionIdNewHead != string.Empty && _subscriptionIdFinalizedHeads != string.Empty;

        private string _subscriptionIdNewHead, _subscriptionIdFinalizedHeads;

        public Wallet(string path = DefaultWalletFile) : this(null, path)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="password"></param>
        /// <param name="path"></param>
        public Wallet(string password, string path)
        {
            _path = path;

            if (!Caching.TryReadFile(path, out _walletFile) && password != null)
            {
                Create(password);
            }
            else if (password != null)
            {
                Unlock(password);
            }

            _connectTokenSource = new CancellationTokenSource();
        }

        public async Task StartAsync(string webSocketUrl = WEBSOCKETURL)
        {
            // disconnect from node if we are already connected to one.
            if (IsConnected)
            {
                Logger.Warn($"Wallet already connected, disconnecting from {ChainInfo} now");
                await StopAsync();
            }

            // connect wallet
            await ConnectAsync(webSocketUrl);

            if (IsConnected)
            {
                await RefreshSubscriptionsAsync();
            }
        }

        private async Task RefreshSubscriptionsAsync()
        {
            Logger.Info($"Refreshing all subscriptions");

            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            // subscribe to new heads
            _subscriptionIdNewHead = await _client.Chain.SubscribeNewHeadsAsync((header) => CallBackNewHeads(header), _connectTokenSource.Token);

            // subscribe to finalized heads
            _subscriptionIdFinalizedHeads = await _client.Chain.SubscribeFinalizedHeadsAsync((header) => CallBackFinalizedHeads(header), _connectTokenSource.Token);
        }

        private async Task UnsubscribeAllAsync()
        {
            if (_subscriptionIdNewHead != string.Empty)
            {
                // unsubscribe from new heads
                if (!await _client.Chain.UnsubscribeNewHeadsAsync(_subscriptionIdNewHead, _connectTokenSource.Token))
                {
                    Logger.Warn($"Couldn't unsubscribe new heads {_subscriptionIdNewHead} id.");
                }
                _subscriptionIdNewHead = string.Empty;
            }

            if (_subscriptionIdFinalizedHeads != string.Empty)
            {
                // unsubscribe from finalized heads
                if (!await _client.Chain.UnsubscribeFinalizedHeadsAsync(_subscriptionIdFinalizedHeads, _connectTokenSource.Token))
                {
                    Logger.Warn($"Couldn't unsubscribe finalized heads {_subscriptionIdFinalizedHeads} id.");
                }
                _subscriptionIdFinalizedHeads = string.Empty;
            }
        }

        public async Task StopAsync()
        {
            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            // disconnect wallet
            await DisconnectAsync();
        }

        public virtual void CallBackNewHeads(Header header)
        {

        }

        public virtual void CallBackFinalizedHeads(Header header)
        {

        }

        /// <summary>
        /// Create a new wallet with mnemonic
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Create(string password)
        {
            if (IsCreated)
            {
                Logger.Warn($"Wallet already created.");
                return true;
            }

            Logger.Info($"Creating new wallet.");

            byte[] randomBytes = new byte[48];

            _random.NextBytes(randomBytes);

            var memoryBytes = randomBytes.AsMemory();

            var pswBytes = Encoding.UTF8.GetBytes(password);

            var salt = memoryBytes.Slice(0, 16).ToArray();
            
            var seed = memoryBytes.Slice(16, 32).ToArray();

            pswBytes = SHA256.Create().ComputeHash(pswBytes);

            var encryptedSeed = ManagedAes.EncryptStringToBytes_Aes(Utils.Bytes2HexString(seed, Utils.HexStringFormat.PURE), pswBytes, salt);

            _walletFile = new WalletFile(encryptedSeed, salt);
           
            Caching.Persist(_path, _walletFile);

            Chaos.NaCl.Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] privateKey, seed);

            Account = new Account(KeyType.ED25519, privateKey, publicKey);

            return true;
        }

        /// <summary>
        /// Unlock a locked wallet.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Unlock(string password)
        {
            if (IsUnlocked || !IsCreated)
            {
                Logger.Warn($"Wallet is already unlocked or doesn't exist.");
                return IsUnlocked && IsCreated;
            }

            Logger.Info($"Unlock new wallet.");

            var pswBytes = Encoding.UTF8.GetBytes(password);

            pswBytes = SHA256.Create().ComputeHash(pswBytes);

            var seed = ManagedAes.DecryptStringFromBytes_Aes(_walletFile.encryptedSeed, pswBytes, _walletFile.salt);

            Chaos.NaCl.Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] privateKey, Utils.HexToByteArray(seed));

            Account = new Account(KeyType.ED25519, privateKey, publicKey);

            return true;
        }
    
        /// <summary>
        /// Try signing a message
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool TrySignMessage(Account signer, byte[] data, out byte[] signature)
        {
            signature = null;

            if (signer?.PrivateKey == null)
            {
                Logger.Warn($"Account or private key doesn't exists.");
                return false;
            }

            switch (signer.KeyType)
            {
                case KeyType.ED25519:
                    signature = Ed25519.Sign(data, signer.PrivateKey);
                    break;
                default:
                    throw new NotImplementedException($"Keytype {signer.KeyType} is currently not implemented for signing.");
            }

            
            return true;
        }

        /// <summary>
        /// Verify a signature of a message
        /// </summary>
        /// <param name="signer"></param>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool VerifySignature(Account signer, byte[] data, byte[] signature)
        {
            switch (signer.KeyType)
            {
                case KeyType.ED25519:
                    return Ed25519.Verify(signature, data, signer.PublicKey);
                case KeyType.SR25519:
                    return Sr25519v091.Verify(signature, signer.PublicKey, data);
                default:
                    throw new NotImplementedException($"Keytype {signer.KeyType} is currently not implemented for verifying signatures.");
            }
        }

        public bool HashData(string data)
        {
            throw new NotImplementedException();
        }

        public bool TryTransfer(Account recipient, int amount)
        {
            throw new NotImplementedException();
        }

        public async Task ConnectAsync(string webSocketUrl)
        {
            Logger.Info($"Connecting to {webSocketUrl}");

            _client = new SubstrateClient(new Uri(webSocketUrl));

            _client.RegisterTypeConverter(new MogwaiStructTypeConverter());

            await _client.ConnectAsync(_connectTokenSource.Token);

            if (!IsConnected)
            {
                Logger.Error($"Connection couldn't be established!");
                return;
            }

            var systemName = await _client.System.NameAsync(_connectTokenSource.Token);

            var systemVersion = await _client.System.VersionAsync(_connectTokenSource.Token);

            var systemChain = await _client.System.ChainAsync(_connectTokenSource.Token);

            ChainInfo = new ChainInfo(systemName, systemVersion, systemChain);

            Logger.Info($"Connection established to {ChainInfo}");
        }

        public async Task DisconnectAsync()
        {
            await _client.CloseAsync(_connectTokenSource.Token);
        }
    }
}
