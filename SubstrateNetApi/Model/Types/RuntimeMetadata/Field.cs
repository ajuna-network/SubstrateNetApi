using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class Field : StructType
    {
        public override string Name() => "Field<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            FieldName = new Option<Vec<BaseChar>>();
            FieldName.Decode(byteArray, ref p);

            FieldTy = new TType();
            FieldTy.Decode(byteArray, ref p);

            FieldTypeName = new Option<Vec<BaseChar>>();
            FieldTypeName.Decode(byteArray, ref p);

            Docs = new Vec<Vec<BaseChar>>();
            Docs.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Option<Vec<BaseChar>> FieldName { get; private set; }
        public TType FieldTy { get; private set; }
        public Option<Vec<BaseChar>> FieldTypeName { get; private set; }
        public Vec<Vec<BaseChar>> Docs { get; private set; }
    }

}
