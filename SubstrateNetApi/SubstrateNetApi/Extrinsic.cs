using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi
{
    class Extrinsic
    {
        public string SubmitExtrinsic(byte[] encodedMethodBytes, string module, string method, byte[] pubKey, string privateKey)
        {
            string teStr = ExtrinsicQueryString(encodedMethodBytes, module, method, pubKey, privateKey);

            var query = new JObject { { "method", "author_submitExtrinsic" }, { "params", new JArray { teStr } } };
            //var response = _jsonRpc.Request(query);

            //return response.ToString();
            return null;
        }

        internal string ExtrinsicQueryString(byte[] encodedMethodBytes, string module, string method, byte[] pubKey, string privateKey)
        {

            //var publicKey = _protocolParams.Metadata.GetPublicKeyFromAddr(sender);
            //var address = new ExtrinsicAddress(publicKey);

            //var nonce = GetAccountNonce(sender);
            //_logger.Info($"sender nonce: {nonce}");
            //var extra = new SignedExtra(DefaultEra(), nonce, BigInteger.Zero);

            //var absoluteIndex = _protocolParams.Metadata.GetModuleIndex(module, false);
            //var moduleIndex = (byte)_protocolParams.Metadata.GetModuleIndex(module, true);
            //var methodIndex = (byte)_protocolParams.Metadata.GetCallMethodIndex(absoluteIndex, method);
            //var call = new ExtrinsicCallRaw<byte[]>(moduleIndex, methodIndex, encodedMethodBytes);
            //var extrinsic = new UncheckedExtrinsic<ExtrinsicAddress, ExtrinsicMultiSignature, SignedExtra, ExtrinsicCallRaw<byte[]>>(true, address, null, extra, call);

            //Signer.SignUncheckedExtrinsic(extrinsic, publicKey.Bytes, privateKey.HexToByteArray());

            //return CreateSerializer()
            //    .Serialize(AsByteVec.FromValue(extrinsic))
            //    .ToPrefixedHexString();
            return null;
        }

    }
}
