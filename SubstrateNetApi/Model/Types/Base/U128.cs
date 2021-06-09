using System;

namespace SubstrateNetApi.Model.Types.Base
{
    public class U128 : BaseType<ulong>
    {
        public override string Name() => "u128";

        public override int Size() => 16;

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
            Create(bytes);
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
            Value = BitConverter.ToUInt64(byteArray, 0);
        }

        public void Create(ulong value)
        {
            Bytes = BitConverter.GetBytes(value);
            Value = value;
        }
    }
}