using Chaos.NaCl;
using NLog;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.MetaDataModel.Values;
using SubstrateNetApi.TypeConverters;
using System;
using System.Linq;
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

        private const string _fileType = "dat";

        private const string _defaultWalletName = "wallet";

        private WalletFile _walletFile;

        private Random _random = new Random();

        private SubstrateClient _client;

        private CancellationTokenSource _connectTokenSource;

        private string _subscriptionIdNewHead, _subscriptionIdFinalizedHeads;

        public bool IsUnlocked => Account != null;

        public bool IsCreated => _walletFile != null;

        public Account Account { get; private set; }

        public AccountInfo AccountInfo { get; private set; }

        public ChainInfo ChainInfo { get; private set; }

        public SubstrateClient Client => _client;

        public bool IsConnected => _client != null && _client.IsConnected;

        public bool IsOnline => IsConnected && _subscriptionIdNewHead != string.Empty && _subscriptionIdFinalizedHeads != string.Empty;

        public bool IsValidWalletName(string walletName) => walletName.Length > 4 && walletName.Length < 21 && walletName.All(c => Char.IsLetterOrDigit(c) || c.Equals('_'));

        public bool IsValidPassword(string password) => password.Length > 7 && password.Length < 21 && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);

        public string AddWalletFileType(string walletName) => $"{walletName}.{_fileType}";

        public event EventHandler<ChainInfo> ChainInfoUpdated;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="password"></param>
        /// <param name="walletName"></param>
        public Wallet()
        {
            _connectTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Load an existing wallet
        /// </summary>
        /// <param name="walletName"></param>
        /// <returns></returns>
        public bool Load(string walletName = _defaultWalletName)
        {
            if (!IsValidWalletName(walletName))
            {
                Logger.Warn("Wallet name is invalid, please provide a proper wallet name. [A-Za-Z_]{20}.");
                return false;
            }

            var walletFileName = AddWalletFileType(walletName);
            if (!Caching.TryReadFile(walletFileName, out _walletFile))
            {
                Logger.Warn($"Failed to load wallet file '{walletFileName}'!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Create a new wallet which is encrypted with a password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Create(string password, string walletName = _defaultWalletName)
        {
            if (IsCreated)
            {
                Logger.Warn($"Wallet already created.");
                return true;
            }

            if (!IsValidPassword(password))
            {
                Logger.Warn("Password isn't is invalid, please provide a proper password. Minmimu eight size and must have upper, lower and digits.");
                return false;
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

            Chaos.NaCl.Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] privateKey, seed);

            _walletFile = new WalletFile(publicKey, encryptedSeed, salt);

            Caching.Persist(AddWalletFileType(walletName), _walletFile);

            Account = new Account(KeyType.ED25519, privateKey, publicKey);

            Logger.Warn($"Listning on call back event of chain info update.");
            ChainInfoUpdated += Wallet_ChainInfoUpdated;

            return true;
        }

        /// <summary>
        /// Unlock a locked wallet.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Unlock(string password, bool noCheck = false)
        {
            if (IsUnlocked || !IsCreated)
            {
                Logger.Warn($"Wallet is already unlocked or doesn't exist.");
                return IsUnlocked && IsCreated;
            }

            Logger.Info($"Unlock new wallet.");

            try
            {
                var pswBytes = Encoding.UTF8.GetBytes(password);

                pswBytes = SHA256.Create().ComputeHash(pswBytes);

                var seed = ManagedAes.DecryptStringFromBytes_Aes(_walletFile.EncryptedSeed, pswBytes, _walletFile.Salt);

                Chaos.NaCl.Ed25519.KeyPairFromSeed(out byte[] publicKey, out byte[] privateKey, Utils.HexToByteArray(seed));

                if (noCheck || !publicKey.SequenceEqual(_walletFile.PublicKey))
                {
                    throw new Exception("Public key check failed!");
                }

                Account = new Account(KeyType.ED25519, privateKey, publicKey);
            }
            catch (Exception exception)
            {
                Logger.Warn($"Couldn't unlock the wallet with this password. {exception}");
                return false;
            }

            Logger.Warn($"Listning on call back event of chain info update.");
            ChainInfoUpdated += Wallet_ChainInfoUpdated;

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

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool HashData(string data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="recipient"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool TryTransfer(Account recipient, int amount)
        {
            throw new NotImplementedException();
        }

        private async Task ConnectAsync(string webSocketUrl)
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

        /// <summary>
        /// Start connection and refresh subscriptions.
        /// </summary>
        /// <param name="webSocketUrl"></param>
        /// <returns></returns>
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
                Logger.Warn($"Starting subscriptions now.");
                await RefreshSubscriptionsAsync();
            }
        }

        private void Wallet_ChainInfoUpdated(object sender, ChainInfo e)
        {
            if (IsCreated && IsUnlocked && Account != null)
            {
                _ = UpdateAccountInfoAsync(Account);
            }
        }

        private async Task RefreshSubscriptionsAsync()
        {
            Logger.Info($"Refreshing all subscriptions");

            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            // subscribe to new heads
            _subscriptionIdNewHead = await _client.Chain.SubscribeNewHeadsAsync((subscriptionId, header) => CallBackNewHeads(subscriptionId, header), _connectTokenSource.Token);

            // subscribe to finalized heads
            _subscriptionIdFinalizedHeads = await _client.Chain.SubscribeFinalizedHeadsAsync((subscriptionId, header) => CallBackFinalizedHeads(subscriptionId, header), _connectTokenSource.Token);
        }

        private async Task UnsubscribeAllAsync()
        {
            if (_subscriptionIdNewHead != null
             && _subscriptionIdNewHead != string.Empty)
            {
                // unsubscribe from new heads
                if (!await _client.Chain.UnsubscribeNewHeadsAsync(_subscriptionIdNewHead, _connectTokenSource.Token))
                {
                    Logger.Warn($"Couldn't unsubscribe new heads {_subscriptionIdNewHead} id.");
                }
                _subscriptionIdNewHead = string.Empty;
            }

            if (_subscriptionIdNewHead != null
             && _subscriptionIdNewHead != string.Empty)
            {
                // unsubscribe from finalized heads
                if (!await _client.Chain.UnsubscribeFinalizedHeadsAsync(_subscriptionIdFinalizedHeads, _connectTokenSource.Token))
                {
                    Logger.Warn($"Couldn't unsubscribe finalized heads {_subscriptionIdFinalizedHeads} id.");
                }
                _subscriptionIdFinalizedHeads = string.Empty;
            }
        }

        /// <summary>
        /// Stop the current connection and unsubscribe all.
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            ChainInfoUpdated -= Wallet_ChainInfoUpdated;

            // disconnect wallet
            await _client.CloseAsync(_connectTokenSource.Token);
        }

        /// <summary>
        /// Update account info for the current wallet.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task UpdateAccountInfoAsync(Account account)
        {
            var reqResult = await _client.GetStorageAsync("System", "Account", new string[] { Utils.Bytes2HexString(Utils.GetPublicKeyFrom(account.Address)) }, _connectTokenSource.Token);

            if (!(reqResult is AccountInfo))
            {
                Logger.Warn($"Couldn't update account informations. Please check '{reqResult}'");
                return;
            }

            Logger.Debug($"Updated account successfully.");
            AccountInfo = reqResult as AccountInfo;
        }

        /// <summary>
        /// Call back for new heads.
        /// </summary>
        /// <param name="header"></param>
        public virtual void CallBackNewHeads(string subscriptionId, Header header)
        {

        }

        /// <summary>
        /// Call back for finalized heads.
        /// </summary>
        /// <param name="header"></param>
        public virtual void CallBackFinalizedHeads(string subscriptionId, Header header)
        {
            ChainInfo.UpdateFinalizedHeader(header);

            ChainInfoUpdated?.Invoke(this, ChainInfo);
        }

    }
}
