using System;

namespace SubstrateNetApi.Model.Types.Base
{
    public class U32 : BaseType<uint>
    {
        public override string Name() => "u32";

        public override int Size() => 4;

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
            var result = new byte[Size()];
            bytes.CopyTo(result, 0);
            Create(result);
        }

        public override void Create(byte[] byteArray)
        {
            if (byteArray.Length < Size())
            {
                var newByteArray = new byte[Size()];
                byteArray.CopyTo(newByteArray, 0);
                byteArray = newByteArray;
            }

            Bytes = byteArray;
            Value = BitConverter.ToUInt32(byteArray, 0);
        }

        public void Create(uint value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }
    }
}