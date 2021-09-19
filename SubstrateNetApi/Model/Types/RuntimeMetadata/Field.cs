using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class Field : BaseType
    {
        public override string TypeName() => "Field<T: Form = MetaForm>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            FieldName = new BaseOpt<Str>();
            FieldName.Decode(byteArray, ref p);

            FieldTy = new TType();
            FieldTy.Decode(byteArray, ref p);

            FieldTypeName = new BaseOpt<Str>();
            FieldTypeName.Decode(byteArray, ref p);

            Docs = new Vec<Str>();
            Docs.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public BaseOpt<Str> FieldName { get; private set; }
        public TType FieldTy { get; private set; }
        public BaseOpt<Str> FieldTypeName { get; private set; }
        public Vec<Str> Docs { get; private set; }
    }

}
