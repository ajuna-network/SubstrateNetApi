using System;
using System.Collections.Generic;
using System.Text;
using SubstrateNetApi.Model.Types.Base;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Timepoint : StructType
    {
        public override string Name() => "Timepoint<T::BlockNumber>";

        private int _size;
        public override int Size() => _size;

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

            _size = p - start;
        }

        public BlockNumber Height { get; private set; }
        public U32 Index { get; private set; }
    }
}
