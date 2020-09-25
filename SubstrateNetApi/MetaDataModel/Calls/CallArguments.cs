using Newtonsoft.Json;
using SubstrateNetApi.MetaDataModel.Values;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Calls
{
    public class CallArguments : IEncodable
    {
        public List<IEncodable> _callArguments;

        public CallArguments(params IEncodable[] list)
        {
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

    public class CallArgs
    {
        public static Func<AccountId, Balance, CallArguments> BalanceTransfer = CallArgumentsMethode;

        private static CallArguments CallArgumentsMethode(IEncodable a, IEncodable b)
        {
            return new CallArguments(a, b);
        }
    }
}
