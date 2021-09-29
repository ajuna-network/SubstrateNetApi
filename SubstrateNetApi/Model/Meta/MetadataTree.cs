using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SubstrateNetApi.Model.Meta;
using SubstrateNetApi.Model.Types.Metadata.V14;
using System.Collections.Generic;

namespace SubstrateNetApi.Model.Meta
{
    public class NodeType
    {
        [JsonIgnore]
        public uint Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Path { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NodeTypeParam[] TypeParams { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TypeDefEnum TypeDef { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Docs { get; set; }
    }

    public class NodeTypeParam
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? TypeId { get; set; }
    }

    public class NodeTypePrimitive : NodeType
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public TypeDefPrimitive Primitive { get; set; }
    }

    public class NodeTypeComposite : NodeType
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NodeTypeField[] TypeFields { get; set; }
    }

    public class NodeTypeField
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName { get; set; }

        public uint TypeId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Docs { get; set; }
    }

    public class NodeTypeArray : NodeType
    {
        public uint Length { get; set; }

        public uint TypeId { get; set; }
    }

    public class NodeTypeSequence : NodeType
    {
        public uint TypeId { get; set; }
    }

    public class NodeTypeCompact : NodeType
    {
        public uint TypeId { get; set; }
    }

    public class NodeTypeTuple : NodeType
    {
        public uint[] TypeIds { get; set; }
    }

    public class NodeTypeBitSequence : NodeType
    {
        public uint TypeIdStore { get; set; }
        public uint TypeIdOrder { get; set; }
    }

    public class NodeTypeVariant : NodeType
    {
        public TypeVariant[] Variants { get; set; }
    }

    public class TypeVariant
    {
        public string Name { get; set; }
        public NodeTypeField[] TypeFields { get; set; }

        public int Index { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Docs { get; set; }
    }

    public class NodeMetadataV14
    {
        public Dictionary<uint, NodeType> Types { get; set; }
        public Dictionary<uint, PalletModule> Modules { get; set; }
        public ExtrinsicMetadata Extrinsic { get; set; }
        public uint TypeId { get; set; }
    }

    public class SignedExtensionMetadata
    {
        public string SignedIdentifier { get; set; }
        public uint SignedExtType { get; set; }
        public uint AddSignedExtType { get; set; }
    }

    public class ExtrinsicMetadata
    {
        public uint TypeId { get; set; }
        public int Version { get; set; }
        public SignedExtensionMetadata[] SignedExtensions { get; set; }
    }

    public class PalletConstant
    {
        public string Name { get; set; }
        public uint TypeId { get; set; }
        public byte[] Value { get; set; }
        public string[] Docs { get; set; }
    }

    public class PalletStorage
    {
        public string Prefix { get; set; }
        public Entry[] Entries { get; set; }
    }

    public class Entry
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Storage.Modifier Modifier { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Storage.Type StorageType { get; set; }
        public (uint, TypeMap) TypeMap { get; set; }
        public byte[] Default { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Docs { get; set; }
    }

    public class TypeMap
    {
        [JsonProperty("Hashers", ItemConverterType = typeof(StringEnumConverter))]
        public Storage.Hasher[] Hashers { get; set; }
        public uint Key { get; set; }
        public uint Value { get; set; }
    }

    public class PalletModule
    {
        public string Name { get; set; }
        public PalletStorage Storage { get; set; }
        public PalletCalls Calls { get; set; }
        public PalletEvents Events { get; set; }
        public PalletConstant[] Constants { get; set; }
        public PalletErrors Errors { get; set; }
        public uint Index { get; set; }
    }

    public class PalletCalls
    {
        public uint TypeId { get; set; }
    }

    public class PalletEvents
    {
        public uint TypeId { get; set; }
    }

    public class PalletErrors
    {
        public uint TypeId { get; set; }
    }

}
