using SubstrateNetApi.Model.Types.Primitive;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class PortableType : BaseType
    {
        public override string TypeName() => "PortableType";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            // #[codec(compact)]
            Id = new U32();
            Id.Create(CompactInteger.Decode(byteArray, ref p));

            Ty = new TypePortableForm();
            Ty.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public U32 Id { get; private set; }
        public TypePortableForm Ty { get; private set; }
    }

}
