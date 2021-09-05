using SubstrateNetApi.Model.Types.Base;
using System;

namespace SubstrateNetApi.Model.Types.Struct
{
    public enum TypeDefEnum
    {
        /// A composite type (e.g. a struct or a tuple)
        Composite,
        /// A variant type (e.g. an enum)
        Variant,
        /// A sequence type with runtime known length.
        Sequence,
        /// An array type with compile-time known length.
        Array,
        /// A tuple type.
        Tuple,
        /// A Rust primitive type.
        Primitive,
        /// A type using the [`Compact`] encoding
        Compact,
        /// A type representing a sequence of bits.
        BitSequence
    }

    public class TypeDefComposite : StructType
    {
        public override string Name() => "TypeDefComposite<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Fields = new Vec<Field>();
            Fields.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Vec<Field> Fields { get; private set; }

    }

    public class TypeDefVariant : StructType
    {
        public override string Name() => "TypeDefVariant<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParam = new Vec<Variant>();
            TypeParam.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Vec<Variant> TypeParam { get; private set; }
    }

    public class TypeDefSequence : StructType
    {
        public override string Name() => "TypeDefSequence<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParam = new TypePortableForm();
            TypeParam.Decode(byteArray, ref p);

            _size = p - start;
        }
        public TypePortableForm TypeParam { get; private set; }
    }

    public class TypeDefArray : StructType
    {
        public override string Name() => "TypeDefArray<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Len = new U32();
            Len.Decode(byteArray, ref p);

            TypeParam = new TypePortableForm();
            TypeParam.Decode(byteArray, ref p);

            _size = p - start;
        }
        public U32 Len { get; private set; }
        public TypePortableForm TypeParam { get; private set; }
    }

    public class TypeDefTuple : StructType
    {
        public override string Name() => "TypeDefTuple<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Fields = new Vec<Field>();
            Fields.Decode(byteArray, ref p);

            _size = p - start;
        }
        public Vec<Field> Fields { get; private set; }
    }

    public enum TypeDefPrimitive
    {
        /// `bool` type
        Bool,
        /// `char` type
        Char,
        /// `str` type
        Str,
        /// `u8`
        U8,
        /// `u16`
        U16,
        /// `u32`
        U32,
        /// `u64`
        U64,
        /// `u128`
        U128,
        /// 256 bits unsigned int (no rust equivalent)
        U256,
        /// `i8`
        I8,
        /// `i16`
        I16,
        /// `i32`
        I32,
        /// `i64`
        I64,
        /// `i128`
        I128,
        /// 256 bits signed int (no rust equivalent)
        I256,
    }

    public class TypeDefCompact : StructType
    {
        public override string Name() => "TypeDefCompact<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            TypeParam = new TypePortableForm();
            TypeParam.Decode(byteArray, ref p);

            _size = p - start;
        }
        public TypePortableForm TypeParam { get; private set; }
    }

    public class TypeDefBitSequence : StructType
    {
        public override string Name() => "TypeDefBitSequence<T: Form = MetaForm>";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            BitStoreType = new TypePortableForm();
            BitStoreType.Decode(byteArray, ref p);

            BitOrderType = new TypePortableForm();
            BitOrderType.Decode(byteArray, ref p);

            _size = p - start;
        }
        public TypePortableForm BitStoreType { get; private set; }
        public TypePortableForm BitOrderType { get; private set; }
    }
}
