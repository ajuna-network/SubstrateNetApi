using System.Text;

namespace Schnorrkel
{
    public static class ByteArrExt
    {
        public static bool Equal(this byte[] self, byte[] other)
        {
            if (self.Length != other.Length)
                return false;

            for (var i = 0; i < self.Length; i++)
            {
                if (self[i] != other[i])
                    return false;
            }

            return true;
        }

        public static string PrintByteArray(this byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string PrintHexByteArray(this byte[] bytes)
        {
            var sb = new StringBuilder("new byte[] { ");
            foreach (var b in bytes)
            {
                sb.Append($"0x{b:X2}, ");
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
