using System.Collections.Generic;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Enum;
using SubstrateNetApi.Model.Types.Primitive;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class DispatchInfo : BaseType
    {
        public override string TypeName() => "DispatchInfo";

        public override byte[] Encode()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Weight.Encode());

            result.AddRange(DispatchClass.Encode());

            result.AddRange(Pays.Encode());

            return result.ToArray();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Weight = new U64();
            Weight.Decode(byteArray, ref p);


            DispatchClass = new BaseEnum<DispatchClass>();
            DispatchClass.Decode(byteArray, ref p);

            Pays = new BaseEnum<Pays>();
            Pays.Decode(byteArray, ref p);

            TypeSize = p - start;
        }

        public U64 Weight { get; set; }
        public BaseEnum<DispatchClass> DispatchClass { get; set; }
        public BaseEnum<Pays> Pays { get; set; }
    }
}