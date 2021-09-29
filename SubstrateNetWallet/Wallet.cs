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
using Schnorrkel.Keys;
//using Schnorrkel;
using SubstrateNetApi;
using SubstrateNetApi.Model.Calls;
using SubstrateNetApi.Model.FrameSystem;
using SubstrateNetApi.Model.Rpc;
using SubstrateNetApi.Model.SpCore;
using SubstrateNetApi.Model.Types;
using SubstrateNetApi.Model.Types.Custom;
using SubstrateNetApi.Model.Types.Struct;
using SubstrateNetApi.TypeConverters;
using static SubstrateNetApi.Mnemonic;

[assembly: InternalsVisibleTo("SubstrateNetWalletTests")]

namespace SubstrateNetWallet
{
    /// <summary>
    /// Basic Wallet implementation
    /// TODO: Make sure that a live runtime change is handled correctly.
    /// </summary>
    public class Wallet
    {
        //private const string Websocketurl = "wss://mogiway-01.dotmog.com";
        private const string WebSocketUrl = "ws://127.0.0.1:9944";

        private const string FileType = "dat";

        private const string DefaultWalletName = "wallet";

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

        /// <summary>
        /// Gets a value indicating whether this instance is unlocked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is unlocked; otherwise, <c>false</c>.
        /// </value>
        public bool IsUnlocked => Account != null;

        /// <summary>
        /// Gets a value indicating whether this instance is created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is created; otherwise, <c>false</c>.
        /// </value>
        public bool IsCreated => _walletFile != null;

        public Account Account { get; private set; }

        public SubstrateNetApi.Model.Types.Struct.AccountInfo AccountInfo { get; private set; }

        public ChainInfo ChainInfo { get; private set; }

        public SubstrateClientExt Client { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is connected; otherwise, <c>false</c>.
        /// </value>
        public bool IsConnected => Client != null && Client.IsConnected;

        /// <summary>
        /// Gets a value indicating whether this instance is online.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is online; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnline => IsConnected && _subscriptionIdNewHead != string.Empty &&
                                _subscriptionIdFinalizedHeads != string.Empty;

        /// <summary>
        /// Determines whether [is valid wallet name] [the specified wallet name].
        /// </summary>
        /// <param name="walletName">Name of the wallet.</param>
        /// <returns>
        ///   <c>true</c> if [is valid wallet name] [the specified wallet name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidWalletName(string walletName)
        {
            return walletName.Length > 4 && walletName.Length < 21 &&
                   walletName.All(c => char.IsLetterOrDigit(c) || c.Equals('_'));
        }

        /// <summary>
        /// Determines whether [is valid password] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <c>true</c> if [is valid password] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsValidPassword(string password)
        {
            return password.Length > 7 && password.Length < 21 && password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) && password.Any(char.IsDigit);
        }

        /// <summary>
        /// Adds the type of the wallet file.
        /// </summary>
        /// <param name="walletName">Name of the wallet.</param>
        /// <returns></returns>
        public string AddWalletFileType(string walletName)
        {
            return $"{walletName}.{FileType}";
        }

        /// <summary>
        /// Occurs when [chain information updated].
        /// </summary>
        public event EventHandler<ChainInfo> ChainInfoUpdated;

        /// <summary>
        /// Occurs when [account information updated].
        /// </summary>
        public event EventHandler<SubstrateNetApi.Model.Types.Struct.AccountInfo> AccountInfoUpdated;

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
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="mnemonic">The mnemonic.</param>
        /// <param name="walletName">Name of the wallet.</param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(string password, string mnemonic, string walletName = DefaultWalletName)
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

            Logger.Info("Creating new wallet from mnemonic.");

            var seed = Mnemonic.GetSecretKeyFromMnemonic(mnemonic, "Substrate", BIP39Wordlist.English);

            var randomBytes = new byte[48];

            _random.NextBytes(randomBytes);

            var memoryBytes = randomBytes.AsMemory();

            var pswBytes = Encoding.UTF8.GetBytes(password);

            var salt = memoryBytes.Slice(0, 16).ToArray();

            pswBytes = SHA256.Create().ComputeHash(pswBytes);

            var encryptedSeed =
                ManagedAes.EncryptStringToBytes_Aes(Utils.Bytes2HexString(seed, Utils.HexStringFormat.Pure), pswBytes, salt);

            var miniSecret = new MiniSecret(seed, ExpandMode.Ed25519);
            var getPair = miniSecret.GetPair();

            var keyType = KeyType.Sr25519;
            _walletFile = new WalletFile(keyType, getPair.Public.Key, encryptedSeed, salt);

            Caching.Persist(AddWalletFileType(walletName), _walletFile);

            Account = Account.Build(keyType, getPair.Secret.ToBytes(), getPair.Public.Key);

            if (IsOnline) _subscriptionAccountInfo = await SubscribeAccountInfoAsync();

            return true;
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="walletName">Name of the wallet.</param>
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

            var keyType = KeyType.Ed25519;
            _walletFile = new WalletFile(keyType, publicKey, encryptedSeed, salt);

            Caching.Persist(AddWalletFileType(walletName), _walletFile);

            Account = Account.Build(keyType, privateKey, publicKey);

            if (IsOnline) _subscriptionAccountInfo = await SubscribeAccountInfoAsync();

            return true;
        }

        /// <summary>
        /// Unlocks the asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="noCheck">if set to <c>true</c> [no check].</param>
        /// <returns></returns>
        /// <exception cref="Exception">Public key check failed!</exception>
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

                byte[] publicKey = null;
                byte[] privateKey = null;
                switch (_walletFile.KeyType)
                {
                    case KeyType.Ed25519:
                        Ed25519.KeyPairFromSeed(out publicKey, out privateKey, Utils.HexToByteArray(seed));
                        break;
                    case KeyType.Sr25519:
                        var miniSecret = new MiniSecret(Utils.HexToByteArray(seed), ExpandMode.Ed25519);
                        var getPair = miniSecret.GetPair();
                        privateKey = getPair.Secret.ToBytes();
                        publicKey = getPair.Public.Key;
                        break;
                }

                if (noCheck || !publicKey.SequenceEqual(_walletFile.PublicKey))
                    throw new Exception("Public key check failed!");

                Account = Account.Build(_walletFile.KeyType, privateKey, publicKey);
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
        /// Tries the sign message.
        /// </summary>
        /// <param name="signer">The signer.</param>
        /// <param name="data">The data.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">KeyType {signer.KeyType} is currently not implemented for signing.</exception>
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
                case KeyType.Sr25519:
                    signature = Sr25519v091.SignSimple(signer.Bytes, signer.PrivateKey, data);
                    break;
                default:
                    throw new NotImplementedException(
                        $"KeyType {signer.KeyType} is currently not implemented for signing.");
            }

            return true;
        }

        /// <summary>
        /// Verifies the signature.
        /// </summary>
        /// <param name="signer">The signer.</param>
        /// <param name="data">The data.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">KeyType {signer.KeyType} is currently not implemented for verifying signatures.</exception>
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
            var accountId32 = new AccountId32();
            accountId32.Create(Account.Bytes);

            return await Client.SubscribeStorageKeyAsync(
                SystemStorage.AccountParams(accountId32), 
                CallBackAccountChange, 
                CancellationToken.None);
        }

        ///// <summary>
        ///// Submits the generic extrinsic asynchronous.
        ///// </summary>
        ///// <param name="genericExtrinsicCall">The generic extrinsic call.</param>
        ///// <returns></returns>
        //public async Task<string> SubmitGenericExtrinsicAsync(GenericExtrinsicCall genericExtrinsicCall)
        //{
        //    return await Client.Author.SubmitAndWatchExtrinsicAsync(CallBackExtrinsic, genericExtrinsicCall, Account, 0, 64,
        //            _connectTokenSource.Token);
        //}

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="webSocketUrl">The web socket URL.</param>
        private async Task ConnectAsync(string webSocketUrl)
        {
            Logger.Info($"Connecting to {webSocketUrl}");

            var client = new SubstrateClientExt(new Uri(webSocketUrl));

            await ConnectAsync(client);
        }

        /// <summary>
        /// Connects the asynchronous.
        /// </summary>
        /// <param name="webSocketUrl">The web socket URL.</param>
        private async Task ConnectAsync(SubstrateClientExt client)
        {
            Client = client;

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
        /// Starts the asynchronous.
        /// </summary>
        /// <param name="webSocketUrl">The web socket URL.</param>
        public async Task StartAsync(string webSocketUrl = WebSocketUrl)
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
        /// Starts the asynchronous.
        /// </summary>
        /// <param name="webSocketUrl">The web socket URL.</param>
        public async Task StartAsync(SubstrateClientExt client)
        {
            // disconnect from node if we are already connected to one.
            if (IsConnected)
            {
                Logger.Warn($"Wallet already connected, disconnecting from {ChainInfo} now");
                await StopAsync();
            }

            // connect wallet
            await ConnectAsync(client);

            if (IsConnected)
            {
                Logger.Warn("Starting subscriptions now.");
                await RefreshSubscriptionsAsync();
            }
        }

        /// <summary>
        /// Refreshes the subscriptions asynchronous.
        /// </summary>
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
        /// Unsubscribes all asynchronous.
        /// </summary>
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
        /// Stops the asynchronous.
        /// </summary>
        public async Task StopAsync()
        {
            // unsubscribe all subscriptions
            await UnsubscribeAllAsync();

            //ChainInfoUpdated -= Wallet_ChainInfoUpdated;

            // disconnect wallet
            await Client.CloseAsync(_connectTokenSource.Token);
        }

        /// <summary>
        /// Calls the back new heads.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="header">The header.</param>
        public virtual void CallBackNewHeads(string subscriptionId, Header header)
        {
        }

        /// <summary>
        /// Calls the back finalized heads.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="header">The header.</param>
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
        /// Calls the back account change.
        /// </summary>
        /// <param name="subscriptionId">The subscription identifier.</param>
        /// <param name="storageChangeSet">The storage change set.</param>
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

            if (string.IsNullOrEmpty(accountInfoStr))
            {
                Logger.Warn("Couldn't update account informations. Account doesn't exists, please check 'CallBackAccountChange'");
                return;
            }

            var accountInfo = new SubstrateNetApi.Model.Types.Struct.AccountInfo();
            accountInfo.Create(accountInfoStr);
            AccountInfo = accountInfo;

            AccountInfoUpdated?.Invoke(this, AccountInfo);
        }
    }
}