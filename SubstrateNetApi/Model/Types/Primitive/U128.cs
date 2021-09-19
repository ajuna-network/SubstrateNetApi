using System;
using System.Numerics;

namespace SubstrateNetApi.Model.Types.Primitive
{
    public class U128 : BasePrim<BigInteger>
    {
        public override string TypeName() => "u128";

        public override int TypeSize() => 16;

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
            // make sure it is unsigned we add 00 at the end
            if (byteArray.Length < TypeSize())
            {
                var newByteArray = new byte[TypeSize()];
                byteArray.CopyTo(newByteArray, 0);
                byteArray = newByteArray;
            } 
            else if (byteArray.Length == TypeSize())
            {
                byte[] newArray = new byte[byteArray.Length + 2];
                byteArray.CopyTo(newArray, 0);
                newArray[byteArray.Length - 1] = 0x00;
            } else
            {
                throw new Exception($"Wrong byte array size for {TypeName()}, max. {TypeSize()} bytes!");
            }

            Bytes = byteArray;
            Value = new BigInteger(byteArray);
        }

        public void Create(BigInteger value)
        {
            var byteArray = value.ToByteArray();

            if (byteArray.Length > 16)
            {
                throw new Exception($"Wrong byte array size for {TypeName()}, max. {TypeSize()} bytes!");
            }

            Bytes = value.ToByteArray();
            Value = value;
        }
    }
}