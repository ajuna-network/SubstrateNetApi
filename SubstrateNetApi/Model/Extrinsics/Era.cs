using Newtonsoft.Json;
using SubstrateNetApi.Model.Types;
using System;

namespace SubstrateNetApi.Model.Extrinsics
{
    public class Era : IEncodable
    {
        public bool IsImmortal { get; }

        public ulong Period { get; }

        public ulong Phase { get; }

        public ulong EraStart(ulong currentBlockNumber) => IsImmortal ? 0 : (Math.Max(currentBlockNumber, Phase) - Phase) / Period * Period + Phase;

        public Era(ulong period, ulong phase, bool isImmortal)
        {
            Period = period;
            Phase = phase;
            IsImmortal = isImmortal;

        }

        override
        public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Era Create(uint lifeTime, ulong finalizedHeaderBlockNumber)
        {
            if (lifeTime == 0)
            {
                return new Era(0, 0, true);
            }

            // NODE: { "IsImmortal":false,"Period":64,"Phase":49}
            // API: { "IsImmortal":false,"Period":64,"Phase":61}

            // RUST Implementation
            //let period = period.checked_next_power_of_two()
            //    .unwrap_or(1 << 16)
            //    .max(4)
            //    .min(1 << 16);
            //let phase = current % period;
            //let quantize_factor = (period >> 12).max(1);
            //let quantized_phase = phase / quantize_factor * quantize_factor;
            //Era::Mortal(period, quantized_phase)

            ulong period = (ulong)Math.Pow(2, Math.Round(Math.Log(lifeTime, 2)));
            period = Math.Max(period, 4);
            period = Math.Min(period, 65536);
            ulong phase = finalizedHeaderBlockNumber % period;
            var quantize_factor = Math.Max(period >> 12, 1);
            var quantized_phase = phase / quantize_factor * quantize_factor;

            return new Era(period, quantized_phase, false);
        }

        public byte[] Encode()
        {
            if (IsImmortal)
            {
                return new byte[] { 0x00 };
            }
            var quantizeFactor = Math.Max(1, Period / 4096);
            var lastBit = Period & (ulong)-(long)Period;
            //var rest = _period;
            //var lastBit = 1;
            //while (rest % 2 == 0 && rest != 0)
            //{
            //    rest /= 2;
            //    lastBit *= 2;
            //}
            var logOf2 = lastBit != 0 ? Math.Log(lastBit, 2) : 64;
            var low = (ushort)Math.Min(15, Math.Max(1, logOf2 - 1));
            var high = (ushort)(Phase / quantizeFactor << 4);
            var encoded = (ushort)(low | high);

            return BitConverter.GetBytes(encoded);
        }

        public static Era Decode(byte[] bytes)
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
                return new Era(period, phase, false);
            }
            else
            {
                throw new Exception("Invalid byte array to get era.");
            }
        }
    }
}
