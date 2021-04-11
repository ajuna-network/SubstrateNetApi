using System;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class MogwaicoinAddress : StructType
    {
        public override string Name() => "MogwaicoinAddress<T::AccountId, ClaimState, BalanceOf<T>>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Address = new Vec<U8>();
            Address.Decode(byteArray, ref p);

            Account = new AccountId();
            Account.Decode(byteArray, ref p);

            Signature = new Vec<U8>();
            Signature.Decode(byteArray, ref p);

            State = new EnumType<ClaimState>();
            State.Decode(byteArray, ref p);

            Balance = new Balance();
            Balance.Decode(byteArray, ref p);

            _size = p - start;
        }

        public Vec<U8> Address { get; private set; }
        public AccountId Account { get; private set; }
        public Vec<U8> Signature { get; private set; }
        public EnumType<ClaimState> State { get; private set; }
        public Balance Balance { get; private set; }
      
    }
}