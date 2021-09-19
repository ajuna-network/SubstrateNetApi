using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using System.Text;
using static SubstrateNetApi.Model.Meta.Storage;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{

    public class RuntimeMetadataV14 : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Types = new PortableRegistry();
            Types.Decode(byteArray, ref p);

            Modules = new Vec<PalletMetadata>();
            Modules.Decode(byteArray, ref p);

            Extrinsic = new ExtrinsicMetadata();
            Extrinsic.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public PortableRegistry Types { get; private set; }
 
        public Vec<PalletMetadata> Modules { get; private set; }
        
        public ExtrinsicMetadata Extrinsic { get; private set; }
    }

    public class MetaDataInfo : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Magic = new PrimU32();
            Magic.Decode(byteArray, ref p);

            Version = new PrimU8();
            Version.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public PrimU32 Magic { get; private set; }
        public PrimU8 Version { get; private set; }

    }

    public class PalletMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            PalletName = new BaseString();
            PalletName.Decode(byteArray, ref p);

            PalletStorage = new Option<StorageMetadata>();
            PalletStorage.Decode(byteArray, ref p);

            PalletCalls = new Option<PalletCallMetadata>();
            PalletCalls.Decode(byteArray, ref p);

            PalletEvent = new Option<PalletEventMetadata>();
            PalletEvent.Decode(byteArray, ref p);

            PalletConstants = new Vec<PalletConstantMetadata>();
            PalletConstants.Decode(byteArray, ref p);

            PalletErrors = new Option<ErrorMetadata>();
            PalletErrors.Decode(byteArray, ref p);

            Index = new PrimU8();
            Index.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString PalletName { get; private set; }
        public Option<StorageMetadata> PalletStorage { get; private set; }
        public Option<PalletCallMetadata> PalletCalls { get; private set; }
        public Option<PalletEventMetadata> PalletEvent { get; private set; }
        public Vec<PalletConstantMetadata> PalletConstants { get; private set; }
        public Option<ErrorMetadata> PalletErrors { get; private set; }
        public PrimU8 Index { get; private set; }
    }

    public class StorageMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Prefix = new BaseString();
            Prefix.Decode(byteArray, ref p);

            Entries = new Vec<StorageEntryMetadata>();
            Entries.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString Prefix { get; private set; }
        public Vec<StorageEntryMetadata> Entries { get; private set; }
    }

    public class StorageEntryMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            StorageName = new BaseString();
            StorageName.Decode(byteArray, ref p);

            StorageModifier = new BaseEnum<Modifier>();
            StorageModifier.Decode(byteArray, ref p);

            StorageType = new BaseEnumExt<Storage.Type, TType, StorageEntryTypeMap, NullType, NullType, NullType, NullType, NullType, NullType, NullType>();
            StorageType.Decode(byteArray, ref p);

            StorageDefault = new ByteGetter();
            StorageDefault.Decode(byteArray, ref p);

            Documentation = new Vec<BaseString>();
            Documentation.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString StorageName { get; private set; }
        public BaseEnum<Modifier> StorageModifier { get; private set; }
        public BaseEnumExt<Storage.Type, TType, StorageEntryTypeMap, NullType, NullType, NullType, NullType, NullType, NullType, NullType> StorageType { get; private set; }
        public ByteGetter StorageDefault { get; private set; }
        public Vec<BaseString> Documentation { get; private set; }
    }

    public class ByteGetter : Vec<PrimU8>
    {
        public override string TypeName() => "unknown";
    }

    public class StorageEntryTypeMap : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Hashers = new Vec<BaseEnum<Hasher>>();
            Hashers.Decode(byteArray, ref p);

            Key = new TType();
            Key.Decode(byteArray, ref p);

            Value = new TType();
            Value.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public Vec<BaseEnum<Hasher>> Hashers { get; private set; }
        public TType Key { get; private set; }
        public TType Value { get; private set; }
    }

    public class PalletCallMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            CallType = new TType();
            CallType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public TType CallType { get; private set; }
    }

    //public class CallMetadata : StructType
    //{
    //    public override string Name() => "unknown";

    //    private int _size;
    //    public override int Size() => _size;

    //    public override byte[] Encode()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Decode(byte[] byteArray, ref int p)
    //    {
    //        var start = p;

    //        FunctionName = new DecodeDifferentStr();
    //        FunctionName.Decode(byteArray, ref p);

    //        FunctionArguments = new DecodeDifferent<FunctionArgumentMetadata>();
    //        FunctionArguments.Decode(byteArray, ref p);

    //        Documentation = new DecodeDifferent<DecodeDifferentStr>();
    //        Documentation.Decode(byteArray, ref p);

    //        _size = p - start;
    //    }
    //    public DecodeDifferentStr FunctionName { get; private set; }
    //    public DecodeDifferent<FunctionArgumentMetadata> FunctionArguments { get; private set; }
    //    public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
    //}

    //public class FunctionArgumentMetadata : StructType
    //{
    //    public override string Name() => "unknown";

    //    private int _size;
    //    public override int Size() => _size;

    //    public override byte[] Encode()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Decode(byte[] byteArray, ref int p)
    //    {
    //        var start = p;

    //        FunctionArgumentName = new DecodeDifferentStr();
    //        FunctionArgumentName.Decode(byteArray, ref p);

    //        FunctionArgumentType = new DecodeDifferentStr();
    //        FunctionArgumentType.Decode(byteArray, ref p);

    //        _size = p - start;
    //    }
    //    public DecodeDifferentStr FunctionArgumentName { get; private set; }
    //    public DecodeDifferentStr FunctionArgumentType { get; private set; }
    //}

    public class PalletEventMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            EventType = new TType();
            EventType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public TType EventType { get; private set; }
    }

    //public class EventMetadata : StructType
    //{
    //    public override string Name() => "unknown";

    //    private int _size;
    //    public override int Size() => _size;

    //    public override byte[] Encode()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void Decode(byte[] byteArray, ref int p)
    //    {
    //        var start = p;

    //        EventName = new DecodeDifferentStr();
    //        EventName.Decode(byteArray, ref p);

    //        EventArguments = new DecodeDifferent<DecodeDifferentStr>();
    //        EventArguments.Decode(byteArray, ref p);

    //        Documentation = new DecodeDifferent<DecodeDifferentStr>();
    //        Documentation.Decode(byteArray, ref p);

    //        _size = p - start;
    //    }
    //    public DecodeDifferentStr EventName { get; private set; }
    //    public DecodeDifferent<DecodeDifferentStr> EventArguments { get; private set; }
    //    public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
    //}

    public class PalletConstantMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ConstantName = new BaseString();
            ConstantName.Decode(byteArray, ref p);

            ConstantType = new TType();
            ConstantType.Decode(byteArray, ref p);

            ConstantValue = new ByteGetter();
            ConstantValue.Decode(byteArray, ref p);

            Documentation = new Vec<BaseString>();
            Documentation.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString ConstantName { get; private set; }
        public TType ConstantType { get; private set; }
        public ByteGetter ConstantValue { get; private set; }
        public Vec<BaseString> Documentation { get; private set; }
    }

    public class ErrorMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ErrorType = new TType();
            ErrorType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public TType ErrorType { get; private set; }

    }

    public class ExtrinsicMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ExtrinsicType = new TType();
            ExtrinsicType.Decode(byteArray, ref p);

            Version = new PrimU8();
            Version.Decode(byteArray, ref p);

            SignedExtensions = new Vec<SignedExtensionMetadata>();
            SignedExtensions.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public TType ExtrinsicType { get; private set; }
        public PrimU8 Version { get; private set; }
        public Vec<SignedExtensionMetadata> SignedExtensions { get; private set; }
    }

    public class SignedExtensionMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            SignedIdentifier = new BaseString();
            SignedIdentifier.Decode(byteArray, ref p);

            SignedExtType = new TType();
            SignedExtType.Decode(byteArray, ref p);

            AddSignedExtType = new TType();
            AddSignedExtType.Decode(byteArray, ref p);

            _typeSize = p - start;
        }
        public BaseString SignedIdentifier { get; private set; }
        public TType SignedExtType { get; private set; }
        public TType AddSignedExtType { get; private set; }
    }

}
