using System;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Timepoint : BaseType
    {
        public override string TypeName() => "Timepoint<T::BlockNumber>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Height = new BlockNumber();
            Height.Decode(byteArray, ref p);

            Index = new U32();
            Index.Decode(byteArray, ref p);

            TypeSize = p - start;
        }

        public BlockNumber Height { get; private set; }
        public U32 Index { get; private set; }
    }
}
