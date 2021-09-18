using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AssetBalance : StructBase
    {
        public override string TypeName() => "AssetBalance<T::Balance>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Balance = new U32();
            Balance.Decode(byteArray, ref p);

            IsFrozen = new Bool();
            IsFrozen.Decode(byteArray, ref p);

            Sufficient = new Bool();
            Sufficient.Decode(byteArray, ref p);

            //Extra = new Extra();
            //Extra.Decode(byteArray, ref p);

            _typeSize = p - start;
        }

        public U32 Balance { get; private set; }
        public Bool IsFrozen { get; private set; }
        public Bool Sufficient { get; private set; }
        //public Extra Extra { get; private set; }
    }

}
