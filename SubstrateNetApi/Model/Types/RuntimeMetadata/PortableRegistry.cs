using SubstrateNetApi.Model.Types.Struct;

namespace SubstrateNetApi.Model.Types.Metadata.V14
{
    public class PortableRegistry : Vec<PortableType>
    {
        public override string Name() => "PortableRegistry";
    }

}
