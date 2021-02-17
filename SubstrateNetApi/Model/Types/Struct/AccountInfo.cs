using System;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class AccountInfo : StructType
    {
        private int _size;

        public U32 Nonce { get; private set; }
        public RefCount Consumers { get; private set; }
        public RefCount Providers { get; private set; }
        public AccountData AccountData { get; private set; }

        public override string Name()
        {
            return "AccountInfo<T::Index, T::AccountData>";
        }

        public override int Size()
        {
            return _size;
        }

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Nonce = new U32();
            Nonce.Decode(byteArray, ref p);

            Consumers = new RefCount();
            Consumers.Decode(byteArray, ref p);

            Providers = new RefCount();
            Providers.Decode(byteArray, ref p);

            AccountData = new AccountData();
            AccountData.Decode(byteArray, ref p);

            _size = p - start;
        }
    }
}