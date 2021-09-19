using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class Variant : BaseType
    {
        public override string TypeName() => "Variant<T: Form = MetaForm>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            VariantName = new BaseString();
            VariantName.Decode(byteArray, ref p);

            VariantFields = new Vec<Field>();
            VariantFields.Decode(byteArray, ref p);

            Index = new U8();
            Index.Decode(byteArray, ref p);

            Docs = new Vec<BaseString>();
            Docs.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString VariantName { get; private set; }
        public Vec<Field> VariantFields { get; private set; }
        public U8 Index { get; private set; }
        public Vec<BaseString> Docs { get; private set; }
    }

}
