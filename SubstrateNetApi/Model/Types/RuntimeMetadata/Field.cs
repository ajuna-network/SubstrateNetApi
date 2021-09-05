using SubstrateNetApi.Model.Types.Base;
using System;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class Field : StructType
    {
        public override string Name() => "Type<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeDefFieldName = new Option<Vec<BaseChar>>();
            TypeDefFieldName.Decode(byteArray, ref p);

            TypeDefFieldType = new TypePortableForm();
            TypeDefFieldType.Decode(byteArray, ref p);

            TypeDefFieldTypeName = new Option<Vec<BaseChar>>();
            TypeDefFieldTypeName.Decode(byteArray, ref p);

            Docs = new Vec<BaseChar>();
            Docs.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Option<Vec<BaseChar>> TypeDefFieldName { get; private set; }
        public TypePortableForm TypeDefFieldType { get; private set; }
        public Option<Vec<BaseChar>> TypeDefFieldTypeName { get; private set; }
        public Vec<BaseChar> Docs { get; private set; }
    }

}
