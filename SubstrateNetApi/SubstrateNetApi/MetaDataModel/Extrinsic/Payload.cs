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

        public byte[] Serialize()
        {
            return _call.Serialize().Concat(_signedExtension.Serialize()).ToArray();
        }
    }
}
