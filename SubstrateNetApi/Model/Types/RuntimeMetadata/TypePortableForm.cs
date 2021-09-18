using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Struct;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class TypePortableForm : StructBase
    {
        public override string TypeName() => "Type<T: Form = MetaForm>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Path = new Path();
            Path.Decode(byteArray, ref p);

            TypeParams = new Vec<TypeParameter>();
            TypeParams.Decode(byteArray, ref p);

            TypeDef = new ExtEnumType<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, EnumType<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType>();
            TypeDef.Decode(byteArray, ref p);

            Docs = new Vec<BaseString>();
            Docs.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public Path Path { get; private set; }
        public Vec<TypeParameter> TypeParams { get; private set; }
        public ExtEnumType<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, EnumType<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType> TypeDef { get; private set; }
        public Vec<BaseString> Docs { get; private set; }
    }

    public class Path : Vec<BaseString>
    {
        public override string TypeName() => "Path<T: Form = MetaForm>";
    }

    public class TypeParameter : StructBase
    {
        public override string TypeName() => "TypeParameter<T: Form = MetaForm>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParameterName = new BaseString();
            TypeParameterName.Decode(byteArray, ref p);

            TypeParameterType = new Option<TType>();
            TypeParameterType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString TypeParameterName { get; private set; }
        public Option<TType> TypeParameterType { get; private set; }
    }

}
