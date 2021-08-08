namespace SubstrateNetApi.Model.Meta
{
    public class Types
    {
        public enum TypeDef
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
            BitSequence,
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
    }
}