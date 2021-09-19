using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SubstrateNetApi.Model.Types.Metadata.V14;

namespace NodeLibraryGen
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
}
