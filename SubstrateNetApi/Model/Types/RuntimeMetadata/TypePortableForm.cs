﻿using SubstrateNetApi.Model.Types.Base;
using System;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class TypePortableForm : StructType
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

            Path = new Path();
            Path.Decode(byteArray, ref p);

            TypeParams = new Vec<TypeParameter>();
            TypeParams.Decode(byteArray, ref p);

            TypeDef = new ExtEnumType<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, EnumType<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType>();
            TypeDef.Decode(byteArray, ref p);

            Docs = new Vec<BaseChar>();
            Docs.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Path Path { get; private set; }
        public Vec<TypeParameter> TypeParams { get; private set; }
        public ExtEnumType<TypeDefEnum, TypeDefComposite, TypeDefVariant, TypeDefSequence, TypeDefArray, TypeDefTuple, EnumType<TypeDefPrimitive>, TypeDefCompact, TypeDefBitSequence, NullType> TypeDef { get; private set; }
        public Vec<BaseChar> Docs { get; private set; }
    }

    public class Path : Vec<Vec<BaseChar>>
    {
        public override string Name() => "Path<T: Form = MetaForm>";
    }

    public class TypeParameter : StructType
    {
        public override string Name() => "TypeParameter<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParameterName = new Vec<BaseChar>();
            TypeParameterName.Decode(byteArray, ref p);

            TypeParameterType = new Option<TypePortableForm>();
            TypeParameterType.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Vec<BaseChar> TypeParameterName { get; private set; }
        public Option<TypePortableForm> TypeParameterType { get; private set; }
    }

}
