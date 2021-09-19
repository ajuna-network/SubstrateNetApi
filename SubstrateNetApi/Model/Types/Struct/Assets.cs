using SubstrateNetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.Model.Types.Base
{
    public class AssetBalance : BaseType
    {
        public override string TypeName() => "AssetBalance<T::Balance>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Balance = new PrimU32();
            Balance.Decode(byteArray, ref p);

            IsFrozen = new PrimBool();
            IsFrozen.Decode(byteArray, ref p);

            Sufficient = new PrimBool();
            Sufficient.Decode(byteArray, ref p);

            //Extra = new Extra();
            //Extra.Decode(byteArray, ref p);

            _typeSize = p - start;
        }

        public PrimU32 Balance { get; private set; }
        public PrimBool IsFrozen { get; private set; }
        public PrimBool Sufficient { get; private set; }
        //public Extra Extra { get; private set; }
    }

}
