using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Primitive;
using SubstrateNetApi.Model.Types.Struct;
using System;
using System.Collections.Generic;
using static SubstrateNetApi.Model.Meta.Storage;

namespace SubstrateNetApi.Model.Types.Metadata.V13
{
    public class RuntimeMetadataV13 : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            MetaDataInfo = new MetaDataInfo();
            MetaDataInfo.Decode(byteArray, ref p);

            Modules = new DecodeDifferent<ModuleMetadata>();
            Modules.Decode(byteArray, ref p);

            Extrinsic = new ExtrinsicMetadata();
            Extrinsic.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public MetaDataInfo MetaDataInfo { get; private set; }
        public DecodeDifferent<ModuleMetadata> Modules { get; private set; }
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

            Magic = new U32();
            Magic.Decode(byteArray, ref p);

            Version = new U8();
            Version.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public U32 Magic { get; private set; }
        public U8 Version { get; private set; }

    }

    public class ModuleMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ModuleName = new DecodeDifferentStr();
            ModuleName.Decode(byteArray, ref p);

            ModuleStorage = new BaseOpt<StorageMetadata>();
            ModuleStorage.Decode(byteArray, ref p);

            ModuleCalls = new BaseOpt<DecodeDifferent<FunctionMetadata>>();
            ModuleCalls.Decode(byteArray, ref p);

            ModuleEvent = new BaseOpt<DecodeDifferent<EventMetadata>>();
            ModuleEvent.Decode(byteArray, ref p);

            ModuleConstants = new DecodeDifferent<ModuleConstantMetadata>();
            ModuleConstants.Decode(byteArray, ref p);

            ModuleErrors = new DecodeDifferent<ErrorMetadata>();
            ModuleErrors.Decode(byteArray, ref p);

            Index = new U8();
            Index.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr ModuleName { get; private set; }
        public BaseOpt<StorageMetadata> ModuleStorage { get; private set; }
        public BaseOpt<DecodeDifferent<FunctionMetadata>> ModuleCalls { get; private set; }
        public BaseOpt<DecodeDifferent<EventMetadata>> ModuleEvent { get; private set; }
        public DecodeDifferent<ModuleConstantMetadata> ModuleConstants { get; private set; }
        public DecodeDifferent<ErrorMetadata> ModuleErrors { get; private set; }
        public U8 Index { get; private set; }
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

            Prefix = new DecodeDifferent<PrimChar>();
            Prefix.Decode(byteArray, ref p);

            Entries = new DecodeDifferent<StorageEntryMetadata>();
            Entries.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferent<PrimChar> Prefix { get; private set; }
        public DecodeDifferent<StorageEntryMetadata> Entries { get; private set; }
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

            StorageName = new DecodeDifferentStr();
            StorageName.Decode(byteArray, ref p);

            StorageModifier = new BaseEnum<Modifier>();
            StorageModifier.Decode(byteArray, ref p);

            StorageType = new BaseEnumExt<Storage.Type, DecodeDifferentStr, StorageEntryTypeMap, StorageEntryTypeDoubleMap, StorageEntryTypeNMap, Void, Void, Void, Void, Void>();
            StorageType.Decode(byteArray, ref p);

            StorageDefault = new ByteGetter();
            StorageDefault.Decode(byteArray, ref p);

            Documentation = new DecodeDifferent<DecodeDifferentStr>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr StorageName { get; private set; }
        public BaseEnum<Modifier> StorageModifier { get; private set; }
        public BaseEnumExt<Storage.Type, DecodeDifferentStr, StorageEntryTypeMap, StorageEntryTypeDoubleMap, StorageEntryTypeNMap, Void, Void, Void, Void, Void> StorageType { get; private set; }
        public ByteGetter StorageDefault { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
    }

    public class ByteGetter : Vec<U8>
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

            Hasher = new BaseEnum<Hasher>();
            Hasher.Decode(byteArray, ref p);

            Key = new DecodeDifferentStr();
            Key.Decode(byteArray, ref p);

            Value = new DecodeDifferentStr();
            Value.Decode(byteArray, ref p);

            Unused = new Bool();
            Unused.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public BaseEnum<Hasher> Hasher { get; private set; }
        public DecodeDifferentStr Key { get; private set; }
        public DecodeDifferentStr Value { get; private set; }
        public Bool Unused { get; private set; }
    }

    public class StorageEntryTypeDoubleMap : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Hasher = new BaseEnum<Hasher>();
            Hasher.Decode(byteArray, ref p);

            Key1 = new DecodeDifferentStr();
            Key1.Decode(byteArray, ref p);

            Key2 = new DecodeDifferentStr();
            Key2.Decode(byteArray, ref p);

            Value = new DecodeDifferentStr();
            Value.Decode(byteArray, ref p);

            Key2Hasher = new BaseEnum<Hasher>();
            Key2Hasher.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public BaseEnum<Hasher> Hasher { get; private set; }
        public DecodeDifferentStr Key1 { get; private set; }
        public DecodeDifferentStr Key2 { get; private set; }
        public DecodeDifferentStr Value { get; private set; }
        public BaseEnum<Hasher> Key2Hasher { get; private set; }
    }

    public class StorageEntryTypeNMap : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            Keys = new DecodeDifferent<DecodeDifferentStr>();
            Keys.Decode(byteArray, ref p);

            Hashers = new DecodeDifferent<BaseEnum<Hasher>>();
            Hashers.Decode(byteArray, ref p);

            Value = new DecodeDifferentStr();
            Value.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferent<DecodeDifferentStr> Keys { get; private set; }
        public DecodeDifferent<BaseEnum<Hasher>> Hashers { get; private set; }
        public DecodeDifferentStr Value { get; private set; }
    }

    public class FunctionMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            FunctionName = new DecodeDifferentStr();
            FunctionName.Decode(byteArray, ref p);

            FunctionArguments = new DecodeDifferent<FunctionArgumentMetadata>();
            FunctionArguments.Decode(byteArray, ref p);

            Documentation = new DecodeDifferent<DecodeDifferentStr>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr FunctionName { get; private set; }
        public DecodeDifferent<FunctionArgumentMetadata> FunctionArguments { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
    }

    public class FunctionArgumentMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            FunctionArgumentName = new DecodeDifferentStr();
            FunctionArgumentName.Decode(byteArray, ref p);

            FunctionArgumentType = new DecodeDifferentStr();
            FunctionArgumentType.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr FunctionArgumentName { get; private set; }
        public DecodeDifferentStr FunctionArgumentType { get; private set; }
    }

    public class EventMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            EventName = new DecodeDifferentStr();
            EventName.Decode(byteArray, ref p);

            EventArguments = new DecodeDifferent<DecodeDifferentStr>();
            EventArguments.Decode(byteArray, ref p);

            Documentation = new DecodeDifferent<DecodeDifferentStr>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr EventName { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> EventArguments { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
    }

    public class ModuleConstantMetadata : BaseType
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            ConstantName = new DecodeDifferentStr();
            ConstantName.Decode(byteArray, ref p);

            ConstantType = new DecodeDifferentStr();
            ConstantType.Decode(byteArray, ref p);

            ConstantValue = new ByteGetter();
            ConstantValue.Decode(byteArray, ref p);

            Documentation = new DecodeDifferent<DecodeDifferentStr>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr ConstantName { get; private set; }
        public DecodeDifferentStr ConstantType { get; private set; }
        public ByteGetter ConstantValue { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
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

            ErrorName = new DecodeDifferentStr();
            ErrorName.Decode(byteArray, ref p);

            Documentation = new DecodeDifferent<DecodeDifferentStr>();
            Documentation.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public DecodeDifferentStr ErrorName { get; private set; }
        public DecodeDifferentStr ConstantType { get; private set; }
        public ByteGetter ConstantValue { get; private set; }
        public DecodeDifferent<DecodeDifferentStr> Documentation { get; private set; }
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

            Version = new U8();
            Version.Decode(byteArray, ref p);

            SignedExtensions = new Vec<DecodeDifferentStr>();
            SignedExtensions.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public U8 Version { get; private set; }
        public Vec<DecodeDifferentStr> SignedExtensions { get; private set; }
    }

    public class DecodeDifferentStr : DecodeDifferent<PrimChar>
    {
        public override string TypeName() => "unknown";
    }

    public class DecodeDifferent<T> : BaseType where T : IType, new()
    {
        public override string TypeName() => "unknown";

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            var list = new List<T>();

            var length = CompactInteger.Decode(byteArray, ref p);
            for (var i = 0; i < length; i++)
            {
                var t = new T();
                t.Decode(byteArray, ref p);
                list.Add(t);
            }

            TypeSize = p - start;

            var bytes = new byte[TypeSize];
            Array.Copy(byteArray, start, bytes, 0, TypeSize);

            Bytes = bytes;
            Value = list;
        }

        public override void CreateFromJson(string str)
        {
            Create(Utils.HexToByteArray(str));
        }

        public List<T> Value { get; internal set; }

        public void Create(List<T> list)
        {
            Value = list;
            Bytes = Encode();
        }

    }
}
