using Newtonsoft.Json;
using StreamJsonRpc;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

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

        public static GenericExtrinsicCall BalanceTransfer(AccountId accountId, Balance balance)
        {
            return new GenericExtrinsicCall("Balances", "transfer", accountId, balance);
        }

        public static GenericExtrinsicCall DmogCreateMogwai()
        {
            return new GenericExtrinsicCall("Dmog", "create_mogwai");
        }
    }
}
