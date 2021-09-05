using Newtonsoft.Json;
using SubstrateNetApi.Model.Meta;
using System;
using System.Collections.Generic;
using System.Text;
using static SubstrateNetApi.Model.Meta.Storage;

namespace SubstrateNetApi.Model.Types.Struct
{

    public class RuntimeMetadataV14 : StructType
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

            Types = new PortableRegistry();
            Types.Decode(byteArray, ref p);

 //           Modules = new Vec<ModuleMetadata>();
 //           Modules.Decode(byteArray, ref p);

            Extrinsic = new ExtrinsicMetadata();
            Extrinsic.Decode(byteArray, ref p);

            _size = p - start;
        }
        public PortableRegistry Types { get; private set; }
 //       public Vec<PalletMetadata> Modules { get; private set; }
        public ExtrinsicMetadata Extrinsic { get; private set; }
    }

}
