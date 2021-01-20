using SubstrateNetApi.MetaDataModel.Types;
using System.Collections.Generic;

namespace SubstrateNetApi.MetaDataModel.Calls
{
    public class GenericExtrinsicCall : IEncodable
    {
        public string ModuleName { get; }

        public string CallName { get; }

        public List<IEncodable> _callArguments;

        public GenericExtrinsicCall(string moduleName, string callName, params IEncodable[] list)
        {
            ModuleName = moduleName;

            CallName = callName;

            _callArguments = new List<IEncodable>();

            foreach (var element in list)
            {
                _callArguments.Add(element);
            }
        }

        public byte[] Encode()
        {
            var byteList = new List<byte>();
            foreach (var callArgument in _callArguments)
            {
                byteList.AddRange(callArgument.Encode());
            }
            return byteList.ToArray();
        }
    }

    public class ExtrinsicCall
    {
        public static GenericExtrinsicCall BalanceTransfer(string address, int balance)
        {
            return BalanceTransfer(AccountId.CreateFromAddress(address), new Balance(balance));
        }

        public static GenericExtrinsicCall BalanceTransfer(AccountId dest, Balance value)
        {
            return new GenericExtrinsicCall("Balances", "transfer", dest, value);
        }

        public static GenericExtrinsicCall BalanceSetBalance(AccountId who, Balance new_free, Balance new_reserved)
        {
            return new GenericExtrinsicCall("Balances", "set_balance", who, new_free, new_reserved);
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
        public static GenericExtrinsicCall BidAuction(Hash mogwai_id, Balance bid)
        {
            return new GenericExtrinsicCall("DotMogModule", "bid_auction", mogwai_id, bid);
        }

        public static GenericExtrinsicCall BreedMogwai(Hash mogwai_id_1, Hash mogwai_id_2)
        {
            return new GenericExtrinsicCall("DotMogModule", "breed_mogwai", mogwai_id_1, mogwai_id_2);
        }

        public static GenericExtrinsicCall BuyMogwai(Hash mogwai_id, Balance max_price)
        {
            return new GenericExtrinsicCall("DotMogModule", "buy_mogwai", mogwai_id, max_price);
        }

        public static GenericExtrinsicCall CreateAuction(Hash mogwai_id, Balance min_bid, BlockNumber expiry)
        {
            return new GenericExtrinsicCall("DotMogModule", "create_auction", mogwai_id, min_bid, expiry);
        }

        public static GenericExtrinsicCall CreateMogwai()
        {
            return new GenericExtrinsicCall("DotMogModule", "create_mogwai");
        }

        public static GenericExtrinsicCall RemoveMogwai(Hash mogwai_id)
        {
            return new GenericExtrinsicCall("DotMogModule", "remove_mogwai", mogwai_id);
        }

        public static GenericExtrinsicCall SetPrice(Hash mogwai_id, Balance new_price)
        {
            return new GenericExtrinsicCall("DotMogModule", "set_price", mogwai_id, new_price);
        }

        public static GenericExtrinsicCall Transfer(AccountId to, Hash mogwai_id)
        {
            return new GenericExtrinsicCall("DotMogModule", "transfer", to, mogwai_id);
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
