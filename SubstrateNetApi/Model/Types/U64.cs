using System;

namespace SubstrateNetApi.Model.Types
{
    public class U64 : BaseType
    {
        public override string Name() => "u64";

        public override int Size() => 8;

        public ulong Value { get; internal set; }

        public override byte[] Encode()
        {
            var reversed = Bytes;
            Array.Reverse(reversed);
            return reversed;
        }

        public override void Create(string str)
        {
            byte[] bytes = Utils.HexToByteArray(str, true);
            Array.Reverse(bytes);
            byte[] result = new byte[Size()];
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
            Value = BitConverter.ToUInt64(byteArray, 0);
        }
    }
}