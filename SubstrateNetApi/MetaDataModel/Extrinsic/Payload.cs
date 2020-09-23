using System.Linq;

namespace SubstrateNetApi.MetaDataModel
{
    public class Payload
    {
        private Method _call;
        private SignedExtensions _signedExtension;

        public Payload(Method call, SignedExtensions signedExtensions)
        {
            _call = call;
            _signedExtension = signedExtensions;
        }

        public byte[] Encode()
        {
            byte[] bytes = _call.Encode().Concat(_signedExtension.Encode()).ToArray();
            if (bytes.Length > 256)
            {
                bytes = HashExtension.Blake2(bytes, 256);
            }
            return bytes;
        }
    }
}
