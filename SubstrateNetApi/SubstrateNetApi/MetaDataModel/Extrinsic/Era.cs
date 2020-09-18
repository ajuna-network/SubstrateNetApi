using System;
using System.Collections.Generic;
using System.Text;

namespace SubstrateNetApi.MetaDataModel.Extrinsic
{
    public class Era
    {
        private ulong _period;

        private ulong _phase;

        private bool _isImmortal;

        public ulong Period => _period;

        public ulong Phase => _phase;

        public Era(ulong period, ulong phase) : this(period, phase, false)
        {

        }

        public Era(ulong period, ulong phase, bool isImmortal)
        {
            _period = period;
            _phase = phase;
            _isImmortal = isImmortal;
        }

        public byte[] Serialize()
        {
            if (_isImmortal)
            {
                return new byte[] { 0x00 };
            }
            var quantizeFactor = Math.Max(1,  _period / 4096);
            var lastBit = _period & (ulong)-(long)_period;
            var logOf2 = (lastBit != 0) ? Math.Log(lastBit, 2) : 64;
            var low = (ushort)Math.Min(15, Math.Max(1, logOf2 - 1));
            var high = (ushort)((_phase / quantizeFactor) << 4);
            var encoded = (ushort)(low | high);

            return BitConverter.GetBytes(encoded);
        }

        public static Era Deserialize(byte[] bytes)
        {
            if (bytes.Length == 1 && bytes[0] == 0x00)
            {
                return new Era(0, 0, true);

            } 
            else if (bytes.Length == 2)
            {
                var ul0 = (ulong)bytes[0];
                var ul1 = (ulong)bytes[1];
                var encoded = ul0 + (ul1 << 8);
                var period = 2UL << (int)(encoded % (1 << 4));
                var quantizeFactor = Math.Max(1, period >> 12);
                var phase = (encoded >> 4) * quantizeFactor;
                if (period < 4 || phase >= period)
                {
                    throw new ArgumentException($"{Utils.Bytes2HexString(new byte[] { bytes[0], bytes[1] })} is not a valid representation of Era.");
                }
                return new Era(period, phase);
            } 
            else
            {
                throw new Exception("Invalid byte array to get era.");
            }
        }
    }
}
