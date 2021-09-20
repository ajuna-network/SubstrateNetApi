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

            TypeParams = new BaseVec<TypeParameter>();
            TypeParams.Decode(byteArray, ref p);

            TypeDef = new BaseEnumExt<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, BaseEnum<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, BaseVoid>();
            TypeDef.Decode(byteArray, ref p);

            Docs = new BaseVec<Str>();
            Docs.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Path Path { get; private set; }
        public BaseVec<TypeParameter> TypeParams { get; private set; }
        public BaseEnumExt<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, BaseEnum<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, BaseVoid> TypeDef { get; private set; }
        public BaseVec<Str> Docs { get; private set; }
    }

    public class Path : BaseVec<Str>
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

            TypeSize = p - start;
        }
        public Str TypeParameterName { get; private set; }
        public BaseOpt<TType> TypeParameterType { get; private set; }
    }

}
