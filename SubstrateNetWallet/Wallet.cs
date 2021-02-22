using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chaos.NaCl;
using NLog;
using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;

[assembly: InternalsVisibleTo("SubstrateNetWalletTests")]

namespace SubstrateNetWallet
{
    /// <summary>
    /// Basic Wallet implementation
    /// TODO: Make sure that a live runtime change is handled correctly.
    /// </summary>
    public class Wallet
    {
        private const string Websocketurl = "wss://mogiway-01.dotmog.com";

        private const string FileType = "dat";

        private const string DefaultWalletName = "wallet";

        /// <summary> The logger. </summary>
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly CancellationTokenSource _connectTokenSource;

        private readonly Random _random = new Random();

        private string _subscriptionIdNewHead, _subscriptionIdFinalizedHeads, _subscriptionAccountInfo;

        private WalletFile _walletFile;

        /// <summary>
        /// Constructor
        /// </summary>
        public Wallet()
        {
            _connectTokenSource = new CancellationTokenSource();
        }

        public bool IsUnlocked => Account != null;

        public bool IsCreated => _walletFile != null;

        public Account Account { get; private set; }

        public AccountInfo AccountInfo { get; private set; }

        public ChainInfo ChainInfo { get; private set; }

        public SubstrateClient Client { get; private set; }

        public bool IsConnected => Client != null && Client.IsConnected;

        public bool IsOnline => IsConnected && _subscriptionIdNewHead != string.Empty &&
                                _subscriptionIdFinalizedHeads != string.Empty;

        public bool IsValidWalletName(string walletName)
        {
            return walletName.Length > 4 && walletName.Length < 21 &&
                   walletName.All(c => char.IsLetterOrDigit(c) || c.Equals('_'));
        }

        public bool IsValidPassword(string password)
        {
            return password.Length > 7 && password.Length < 21 && password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) && password.Any(char.IsDigit);
        }

        public string AddWalletFileType(string walletName)
        {
            return $"{walletName}.{FileType}";
        }

        public event EventHandler<ChainInfo> ChainInfoUpdated;

        public event EventHandler<AccountInfo> AccountInfoUpdated;

        /// <summary>
        /// Load an existing wallet
        /// </summary>
        /// <param name="walletName"></param>
        /// <returns></returns>
        public bool Load(string walletName = DefaultWalletName)
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
        /// <param name="walletName"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(string password, string walletName = DefaultWalletName)
        {
            if (IsCreated)
            {
                Logger.Warn("Wallet already created.");
                return true;
            }

            if (!IsValidPassword(password))
            {
                Logger.Warn(
                    "Password isn't is invalid, please provide a proper password. Minmimu eight size and must have upper, lower and digits.");
                return false;
            }

            Logger.Info("Creating new wallet.");

            var randomBytes = new byte[48];

            _random.NextBytes(randomBytes);

            var memoryBytes = randomBytes.AsMemory();

            var pswBytes = Encoding.UTF8.GetBytes(password);

            var salt = memoryBytes.Slice(0, 16).ToArray();

            var seed = memoryBytes.Slice(16, 32).ToArray();

            pswBytes = SHA256.Create().ComputeHash(pswBytes);

            var encryptedSeed =
                ManagedAes.EncryptStringToBytes_Aes(Utils.Bytes2HexString(seed, Utils.HexStringFormat.Pure), pswBytes,
                    salt);

            Ed25519.KeyPairFromSeed(out var publicKey, out var privateKey, seed);

            _walletFile = new WalletFile(publicKey, encryptedSeed, salt);

            Caching.Persist(AddWalletFileType(walletName), _walletFile);

            Account = Account.Build(KeyType.Ed25519, privateKey, publicKey);

            if (IsOnline) _subscriptionAccountInfo = await SubscribeAccountInfoAsync();

            return true;
        }

        /// <summary>
        /// Unlock a locked wallet.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="noCheck"></param>
        /// <returns></returns>
        public async Task<bool> UnlockAsync(string password, bool noCheck = false)
        {
            if (IsUnlocked || !IsCreated)
            {
                Logger.Warn("Wallet is already unlocked or doesn't exist.");
                return IsUnlocked && IsCreated;
            }

            Logger.Info("Unlock new wallet.");

            try
            {
                var pswBytes = Encoding.UTF8.GetBytes(password);

                pswBytes = SHA256.Create().ComputeHash(pswBytes);

                var seed = ManagedAes.DecryptStringFromBytes_Aes(_walletFile.EncryptedSeed, pswBytes, _walletFile.Salt);

                Ed25519.KeyPairFromSeed(out var publicKey, out var privateKey, Utils.HexToByteArray(seed));

                if (noCheck || !publicKey.SequenceEqual(_walletFile.PublicKey))
                    throw new Exception("Public key check failed!");

                Account = Account.Build(KeyType.Ed25519, privateKey, publicKey);
            }
            catch (Exception exception)
            {
                Logger.Warn($"Couldn't unlock the wallet with this password. {exception}");
                return false;
            }


            if (IsOnline) _subscriptionAccountInfo = await SubscribeAccountInfoAsync();

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
                Logger.Warn("Account or private key doesn't exists.");
                return false;
            }

            switch (signer.KeyType)
            {
                case KeyType.Ed25519:
                    signature = Ed25519.Sign(data, signer.PrivateKey);
                    break;
                default:
                    throw new NotImplementedException(
                        $"KeyType {signer.KeyType} is currently not implemented for signing.");
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
                case KeyType.Ed25519:
                    return Ed25519.Verify(signature, data, signer.Bytes);
                case KeyType.Sr25519:
                    return Sr25519v091.Verify(signature, signer.Bytes, data);
                default:
                    throw new NotImplementedException(
                        $"KeyType {signer.KeyType} is currently not implemented for verifying signatures.");
            }
        }

        /// <summary>
        /// Subscribe to AccountInfo asynchronous
        /// </summary>
        /// <returns></returns>
        public async Task<string> SubscribeAccountInfoAsync()
        {
            return await Client.SubscribeStorageKeyAsync("System", "Account",
                new[] {Utils.Bytes2HexString(Utils.GetPublicKeyFrom(Account.Value))},
                CallBackAccountChange, _connectTokenSource.Token);
        }

        /// <summary>
        /// Submit generic Extrinsic asynchronous
        /// </summary>
        /// <param name="genericExtrinsicCall"></param>
        /// <returns></returns>
        public async Task<string> SubmitGenericExtrinsicAsync(GenericExtrinsicCall genericExtrinsicCall)
        {
            return await Client.Author
                .SubmitAndWatchExtrinsicAsync(CallBackExtrinsic, genericExtrinsicCall, Account, 0, 64,
                    _connectTokenSource.Token);
        }

        private async Task ConnectAsync(string webSocketUrl)
        {
            Logger.Info($"Connecting to {webSocketUrl}");

            Client = new SubstrateClient(new Uri(webSocketUrl));

            //TODO check if that can be made generic as parameter
            Client.RegisterTypeConverter(new GenericTypeConverter<MogwaiStruct>());
            Client.RegisterTypeConverter(new GenericTypeConverter<MogwaiBios>());

            await Client.ConnectAsync(_connectTokenSource.Token);

            if (!IsConnected)
            {
                Logger.Error("Connection couldn't be established!");
                return;
            }

            var systemName = await Client.System.NameAsync(_connectTokenSource.Token);

            var systemVersion = await Client.System.VersionAsync(_connectTokenSource.Token);

            var systemChain = await Client.System.ChainAsync(_connectTokenSource.Token);

            ChainInfo = new ChainInfo(systemName, systemVersion, systemChain, Client.RuntimeVersion);

            Logger.Info($"Connection established to {ChainInfo}");
        }

        /// <summary>
        /// Start connection and refresh subscriptions.
        /// </summary>
        /// <param name="webSocketUrl"></param>
        /// <returns></returns>
        public async Task StartAsync(string webSocketUrl = Websocketurl)
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
                Logger.Warn("Starting subscriptions now.");
                await RefreshSubscriptionsAsync();
            }
        }

        /// <summary>
        /// Refresh subscriptions asynchronous
        /// </summary>
        /// <returns></returns>
        public async Task RefreshSubscriptionsAsync()
        {
            Logger.Info("Refreshing all subscriptions");

            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            // subscribe to new heads
            _subscriptionIdNewHead =
                await Client.Chain.SubscribeNewHeadsAsync(CallBackNewHeads, _connectTokenSource.Token);

            // subscribe to finalized heads
            _subscriptionIdFinalizedHeads =
                await Client.Chain.SubscribeFinalizedHeadsAsync(CallBackFinalizedHeads, _connectTokenSource.Token);

            if (IsUnlocked)
                // subscribe to account info
                _subscriptionAccountInfo = await SubscribeAccountInfoAsync();
        }

        /// <summary>
        /// Unsubscribe all asynchronous
        /// </summary>
        /// <returns></returns>
        public async Task UnsubscribeAllAsync()
        {
            if (!string.IsNullOrEmpty(_subscriptionIdNewHead))
            {
                // unsubscribe from new heads
                if (!await Client.Chain.UnsubscribeNewHeadsAsync(_subscriptionIdNewHead, _connectTokenSource.Token))
                    Logger.Warn($"Couldn't unsubscribe new heads {_subscriptionIdNewHead} id.");
                _subscriptionIdNewHead = string.Empty;
            }

            if (!string.IsNullOrEmpty(_subscriptionIdNewHead))
            {
                // unsubscribe from finalized heads
                if (!await Client.Chain.UnsubscribeFinalizedHeadsAsync(_subscriptionIdFinalizedHeads,
                    _connectTokenSource.Token))
                    Logger.Warn($"Couldn't unsubscribe finalized heads {_subscriptionIdFinalizedHeads} id.");
                _subscriptionIdFinalizedHeads = string.Empty;
            }

            if (!string.IsNullOrEmpty(_subscriptionAccountInfo))
            {
                // unsubscribe from finalized heads
                if (!await Client.State.UnsubscribeStorageAsync(_subscriptionAccountInfo, _connectTokenSource.Token))
                    Logger.Warn($"Couldn't unsubscribe storage subscription {_subscriptionAccountInfo} id.");
                _subscriptionAccountInfo = string.Empty;
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

            //ChainInfoUpdated -= Wallet_ChainInfoUpdated;

            // disconnect wallet
            await Client.CloseAsync(_connectTokenSource.Token);
        }

        /// <summary>
        /// Call back for new heads.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="header"></param>
        public virtual void CallBackNewHeads(string subscriptionId, Header header)
        {
        }

        /// <summary>
        /// Call back for finalized heads.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="header"></param>
        public virtual void CallBackFinalizedHeads(string subscriptionId, Header header)
        {
            ChainInfo.UpdateFinalizedHeader(header);

            ChainInfoUpdated?.Invoke(this, ChainInfo);
        }

        /// <summary>
        /// Call back for extrinsic.
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="extrinsicStatus"></param>
        public virtual void CallBackExtrinsic(string subscriptionId, ExtrinsicStatus extrinsicStatus)
        {
        }

        /// <summary>
        /// Call back account change.
        /// TODO: Implement type StorageChangeSet
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="storageChangeSet"></param>
        public virtual void CallBackAccountChange(string subscriptionId, StorageChangeSet storageChangeSet)
        {
            if (storageChangeSet.Changes == null 
                || storageChangeSet.Changes.Length == 0 
                || storageChangeSet.Changes[0].Length < 2)
            {
                Logger.Warn("Couldn't update account informations. Please check 'CallBackAccountChange'");
                return;
            }

            var accountInfoStr = storageChangeSet.Changes[0][1];

            if (accountInfoStr is null)
            {
                Logger.Warn("Couldn't update account informations. Account doesn't exists, please check 'CallBackAccountChange'");
                return;
            }

            Logger.Debug("Updated account successfully.");

            var accountInfo = new AccountInfo();
            accountInfo.Create(accountInfoStr);
            AccountInfo = accountInfo;

            AccountInfoUpdated?.Invoke(this, AccountInfo);
        }
    }
}