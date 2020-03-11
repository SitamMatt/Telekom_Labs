using System.IO;

namespace Cwiczenie1.BitFormatting
{
    public class RawBinaryFormatter : BitFormatter
    {
        public override void Write(Stream output, byte[] encoded)
        {
            output.Write(encoded);
        }
    }
}