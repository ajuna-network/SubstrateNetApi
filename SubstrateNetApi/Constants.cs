namespace SubstrateNetApi
{
    public class Constants
    {

        public static uint SPEC_VERSION = 259;

        public const uint TX_VERSION = 1;

        public const ulong EXTRINSIC_ERA_PERIOD_DEFAULT = 64;

        public const byte EXTRINSIC_VERSION = 4; // aka the 2. TRANSACTION_VERSION
        // https://github.com/paritytech/substrate/blob/c0cb70419798eb7fd38806da668bec05f8cfd7f1/primitives/runtime/src/generic/unchecked_extrinsic.rs#L33

        public static uint ADDRESS_VERSION = 1; // 0 = AccountId (no prefix), 1 = IndicesLookup (0xFF), 2 = MutiAddress (https://github.com/paritytech/substrate/pull/7380)
    }
}
