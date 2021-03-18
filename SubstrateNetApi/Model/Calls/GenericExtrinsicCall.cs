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
}
