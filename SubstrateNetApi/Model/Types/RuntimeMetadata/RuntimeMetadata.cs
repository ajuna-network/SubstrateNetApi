using SubstrateNetApi.Model.Types.Base;
using SubstrateNetApi.Model.Types.Metadata.V14;
using System;
using System.Text;

namespace SubstrateNetApi.Model.Types.Struct
{
    public class RuntimeMetadata : StructType
    {
        public override string Name() => "unknown";

        private int _size;
        public override int Size() => _size;

        public override byte[] Encode()
        {
            throw new NotImplementedException();
        }

        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;

            MetaDataInfo = new MetaDataInfo();
            MetaDataInfo.Decode(byteArray, ref p);

            string str = Encoding.Default.GetString(byteArray.AsMemory().Slice(p).ToArray());

            RuntimeMetadataData = new RuntimeMetadataV14();
            RuntimeMetadataData.Decode(byteArray, ref p);

            _size = p - start;
        }
        public MetaDataInfo MetaDataInfo { get; private set; }
        public U32 MetaReserved { get; private set; }
        public RuntimeMetadataV14 RuntimeMetadataData { get; private set; }
    }

}
