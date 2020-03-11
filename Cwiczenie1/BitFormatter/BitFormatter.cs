using System;
using System.IO;
using System.Text;

namespace Cwiczenie1.BitFormatting
{
    public class BitFormatter
    {
        public virtual void Write(Stream output, byte[] encoded)
        {
            StringBuilder sb = new StringBuilder(encoded.Length*8);
            for (int i = 0; i < encoded.Length; i++)
            {
                sb.Append(Convert.ToString(encoded[i], 2));
            }
            var sw = new StreamWriter(output);
            sw.Write(sb.ToString());
            sw.Flush();
        }
    }
}