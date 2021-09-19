using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class TypePortableForm : BaseType
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

            TypeDef = new BaseEnumExt<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, BaseEnum<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType>();
            TypeDef.Decode(byteArray, ref p);

            Docs = new Vec<Str>();
            Docs.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public Path Path { get; private set; }
        public Vec<TypeParameter> TypeParams { get; private set; }
        public BaseEnumExt<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, BaseEnum<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType> TypeDef { get; private set; }
        public Vec<Str> Docs { get; private set; }
    }

    public class Path : Vec<Str>
    {
        public override string TypeName() => "Path<T: Form = MetaForm>";
    }

    public class TypeParameter : BaseType
    {
        public override string TypeName() => "TypeParameter<T: Form = MetaForm>";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParameterName = new Str();
            TypeParameterName.Decode(byteArray, ref p);

            TypeParameterType = new BaseOpt<TType>();
            TypeParameterType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public Str TypeParameterName { get; private set; }
        public BaseOpt<TType> TypeParameterType { get; private set; }
    }

}
