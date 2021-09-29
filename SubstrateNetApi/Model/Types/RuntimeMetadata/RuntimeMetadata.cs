using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Metadata.V14;
using SubstrateNetApi.Model.Types.Primitive;
using System;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class RuntimeMetadata : BaseType
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

            RuntimeMetadataData = new RuntimeMetadataV14();
            RuntimeMetadataData.Decode(byteArray, ref p);

            TypeSize = p - start;
        }
        public MetaDataInfo MetaDataInfo { get; private set; }
        public RuntimeMetadataV14 RuntimeMetadataData { get; private set; }
    }

}
