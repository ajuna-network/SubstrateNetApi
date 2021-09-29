using System;
using System.Text;
using SubstrateNetApi.Model.Meta;

namespace SubstrateNetApi
{
    public class MetaDataParser
    {
        public MetaDataParser(string origin, string metaData)
        {
            Parse(origin, Utils.HexToByteArray(metaData));
        }

        public MetaData MetaData { get; private set; }

        internal MetaData Parse(string origin, byte[] m)
        {
            var p = 0;

            return MetaData;
        }

        private byte[] ExtractBytes(byte[] m, ref int p)
        {
            var value = CompactInteger.Decode(m, ref p);
            var bytes = new byte[value];
            for (var i = 0; i < value; i++)
            {
                bytes[i] = m[p++];
            }

            return bytes;
        }

        private string ExtractString(byte[] m, ref int p)
        {
            var value = CompactInteger.Decode(m, ref p);

            var s = string.Empty;
            for (var i = 0; i < value; i++)
            {
                s += (char) m[p++];
            }

            return s;
        }
    }
}