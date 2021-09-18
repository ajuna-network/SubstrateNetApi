using System;

namespace SubstrateNetApi.Model.Types.TypeDefPrimitive
{
    public class PrimI16 : BaseType<short>
    {
        public override string TypeName() => "i16";

        public override int TypeSize() => 2;

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void CreateFromJson(string str)
        {
            var bytes = Utils.HexToByteArray(str, true);
            Array.Reverse(bytes);
            var result = new byte[TypeSize()];
            bytes.CopyTo(result, 0);
            Create(result);
        }

        public override void Create(byte[] byteArray)
        {
            if (byteArray.Length < TypeSize())
            {
                var newByteArray = new byte[TypeSize()];
                byteArray.CopyTo(newByteArray, 0);
                byteArray = newByteArray;
            }

            Bytes = byteArray;
            Value = BitConverter.ToInt16(byteArray, 0);
        }

        public void Create(short value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }
    }
}