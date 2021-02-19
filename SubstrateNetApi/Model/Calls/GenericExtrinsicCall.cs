using SubstrateNetApi.Model.Types;
using System.Collections.Generic;
using System.Numerics;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Calls
{
    public class GenericExtrinsicCall : IEncodable
    {
        public string ModuleName { get; }

        public string CallName { get; }

        public List<IType> CallArguments;

        public GenericExtrinsicCall(string moduleName, string callName, params IType[] list)
        {
            ModuleName = moduleName;

            CallName = callName;

            CallArguments = new List<IType>();

            foreach (var element in list)
            {
                CallArguments.Add(element);
            }
        }

        public byte[] Encode()
        {
            var byteList = new List<byte>();
            foreach (var callArgument in CallArguments)
            {
                byteList.AddRange(callArgument.Encode());
            }
            return byteList.ToArray();
        }
    }

    public class ExtrinsicCall
    {
        public static GenericExtrinsicCall BalanceTransfer(string address, BigInteger balanceAmount)
        {
            var accountId = new AccountId();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            var balance = new Balance();
            balance.Create(balanceAmount);

            return BalanceTransfer(accountId, balance);
        }

        public static GenericExtrinsicCall BalanceTransfer(AccountId dest, Balance value)
        {
            return new GenericExtrinsicCall("Balances", "transfer", dest, value);
        }

        public static GenericExtrinsicCall BalanceSetBalance(AccountId who, Balance newFree, Balance newReserved)
        {
            return new GenericExtrinsicCall("Balances", "set_balance", who, newFree, newReserved);
        }

        public static GenericExtrinsicCall BalanceForceTransfer(AccountId source, AccountId dest, Balance value)
        {
            return new GenericExtrinsicCall("Balances", "force_transfer", source, dest, value);
        }

        public static GenericExtrinsicCall BalanceTransferKeepAlive(AccountId dest, Balance value)
        {
            return new GenericExtrinsicCall("Balances", "transfer_keep_alive", dest, value);
        }

        public static GenericExtrinsicCall SystemSuicide()
        {
            return new GenericExtrinsicCall("System", "suicide");
        }

        public static GenericExtrinsicCall TimestampSet(Moment moment)
        {
            return new GenericExtrinsicCall("Timestamp", "set");
        }


    }

    public class DotMogCall
    {
        public static GenericExtrinsicCall BidAuction(Hash mogwaiId, Balance bid)
        {
            return new GenericExtrinsicCall("DotMogModule", "bid_auction", mogwaiId, bid);
        }

        public static GenericExtrinsicCall BreedMogwai(Hash mogwaiId1, Hash mogwaiId2)
        {
            return new GenericExtrinsicCall("DotMogModule", "breed_mogwai", mogwaiId1, mogwaiId2);
        }

        public static GenericExtrinsicCall BuyMogwai(Hash mogwaiId, Balance maxPrice)
        {
            return new GenericExtrinsicCall("DotMogModule", "buy_mogwai", mogwaiId, maxPrice);
        }

        public static GenericExtrinsicCall CreateAuction(Hash mogwaiId, Balance minBid, BlockNumber expiry)
        {
            return new GenericExtrinsicCall("DotMogModule", "create_auction", mogwaiId, minBid, expiry);
        }

        public static GenericExtrinsicCall CreateMogwai()
        {
            return new GenericExtrinsicCall("DotMogModule", "create_mogwai");
        }

        public static GenericExtrinsicCall RemoveMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "remove_mogwai", mogwaiId);
        }

        public static GenericExtrinsicCall MorphMogwai(Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "morph_mogwai", mogwaiId);
        }

        public static GenericExtrinsicCall SetPrice(Hash mogwaiId, Balance newPrice)
        {
            return new GenericExtrinsicCall("DotMogModule", "set_price", mogwaiId, newPrice);
        }

        public static GenericExtrinsicCall Transfer(AccountId to, Hash mogwaiId)
        {
            return new GenericExtrinsicCall("DotMogModule", "transfer", to, mogwaiId);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index, U8 valueOpt)
        {
            return new GenericExtrinsicCall("DotMogModule", "update_config", index, valueOpt);
        }

        public static GenericExtrinsicCall UpdateConfig(U8 index)
        {
            return new GenericExtrinsicCall("DotMogModule", "update_config", index);
        }
    }
}
