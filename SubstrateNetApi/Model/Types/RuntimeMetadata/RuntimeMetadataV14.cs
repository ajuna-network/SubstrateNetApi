using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;
using static SubstrateNetApi.Model.Meta.Storage;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{

    public class RuntimeMetadataV14 : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Types = new PortableRegistry();
            Types.Decode(byteArray, ref p);

            Modules = new BaseVec<PalletMetadata>();
            Modules.Decode(byteArray, ref p);

            Extrinsic = new ExtrinsicMetadataStruct();
            Extrinsic.Decode(byteArray, ref p);

            TypeId = new TType();
            TypeId.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public PortableRegistry Types { get; private set; }
 
        public BaseVec<PalletMetadata> Modules { get; private set; }
        
        public ExtrinsicMetadataStruct Extrinsic { get; private set; }

        public TType TypeId { get; private set; }
    }

    public class MetaDataInfo : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Magic = new U32();
            Magic.Decode(byteArray, ref p);

            Version = new U8();
            Version.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public U32 Magic { get; private set; }
        public U8 Version { get; private set; }

    }

    public class PalletMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            PalletName = new Str();
            PalletName.Decode(byteArray, ref p);

            PalletStorage = new BaseOpt<StorageMetadata>();
            PalletStorage.Decode(byteArray, ref p);

            PalletCalls = new BaseOpt<PalletCallMetadata>();
            PalletCalls.Decode(byteArray, ref p);

            PalletEvents = new BaseOpt<PalletEventMetadata>();
            PalletEvents.Decode(byteArray, ref p);

            PalletConstants = new BaseVec<PalletConstantMetadata>();
            PalletConstants.Decode(byteArray, ref p);

            PalletErrors = new BaseOpt<ErrorMetadata>();
            PalletErrors.Decode(byteArray, ref p);

            Index = new U8();
            Index.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Str PalletName { get; private set; }
        public BaseOpt<StorageMetadata> PalletStorage { get; private set; }
        public BaseOpt<PalletCallMetadata> PalletCalls { get; private set; }
        public BaseOpt<PalletEventMetadata> PalletEvents { get; private set; }
        public BaseVec<PalletConstantMetadata> PalletConstants { get; private set; }
        public BaseOpt<ErrorMetadata> PalletErrors { get; private set; }
        public U8 Index { get; private set; }
    }

    public class StorageMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Prefix = new Str();
            Prefix.Decode(byteArray, ref p);

            Entries = new BaseVec<StorageEntryMetadata>();
            Entries.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Str Prefix { get; private set; }
        public BaseVec<StorageEntryMetadata> Entries { get; private set; }
    }

    public class StorageEntryMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            StorageName = new Str();
            StorageName.Decode(byteArray, ref p);

            StorageModifier = new BaseEnum<Modifier>();
            StorageModifier.Decode(byteArray, ref p);

            StorageType = new BaseEnumExt<Storage.Type, TType, StorageEntryTypeMap>();
            StorageType.Decode(byteArray, ref p);

            StorageDefault = new ByteGetter();
            StorageDefault.Decode(byteArray, ref p);

            Documentation = new BaseVec<Str>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Str StorageName { get; private set; }
        public BaseEnum<Modifier> StorageModifier { get; private set; }
        public BaseEnumExt<Storage.Type, TType, StorageEntryTypeMap> StorageType { get; private set; }
        public ByteGetter StorageDefault { get; private set; }
        public BaseVec<Str> Documentation { get; private set; }
    }

    public class ByteGetter : BaseVec<U8>
    {
    }

    public class StorageEntryTypeMap : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Hashers = new BaseVec<BaseEnum<Hasher>>();
            Hashers.Decode(byteArray, ref p);

            Key = new TType();
            Key.Decode(byteArray, ref p);

            Value = new TType();
            Value.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public BaseVec<BaseEnum<Hasher>> Hashers { get; private set; }
        public TType Key { get; private set; }
        public TType Value { get; private set; }
    }

    public class PalletCallMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            CallType = new TType();
            CallType.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public TType CallType { get; private set; }
    }

    public class PalletEventMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            EventType = new TType();
            EventType.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public TType EventType { get; private set; }
    }
    public class PalletConstantMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ConstantName = new Str();
            ConstantName.Decode(byteArray, ref p);

            ConstantType = new TType();
            ConstantType.Decode(byteArray, ref p);

            ConstantValue = new ByteGetter();
            ConstantValue.Decode(byteArray, ref p);

            Documentation = new BaseVec<Str>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Str ConstantName { get; private set; }
        public TType ConstantType { get; private set; }
        public ByteGetter ConstantValue { get; private set; }
        public BaseVec<Str> Documentation { get; private set; }
    }

    public class ErrorMetadata : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ErrorType = new TType();
            ErrorType.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public TType ErrorType { get; private set; }

    }

    public class ExtrinsicMetadataStruct : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ExtrinsicType = new TType();
            ExtrinsicType.Decode(byteArray, ref p);

            Version = new U8();
            Version.Decode(byteArray, ref p);

            SignedExtensions = new BaseVec<SignedExtensionMetadataStruct>();
            SignedExtensions.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public TType ExtrinsicType { get; private set; }
        public U8 Version { get; private set; }
        public BaseVec<SignedExtensionMetadataStruct> SignedExtensions { get; private set; }
    }

    public class SignedExtensionMetadataStruct : BaseType
    {
        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            SignedIdentifier = new Str();
            SignedIdentifier.Decode(byteArray, ref p);

            SignedExtType = new TType();
            SignedExtType.Decode(byteArray, ref p);

            AddSignedExtType = new TType();
            AddSignedExtType.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public Str SignedIdentifier { get; private set; }
        public TType SignedExtType { get; private set; }
        public TType AddSignedExtType { get; private set; }
    }

}
