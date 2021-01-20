using System.Linq;

namespace SubstrateNetApi.Model.Extrinsics
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
            return bytes;
        }
    }
}
