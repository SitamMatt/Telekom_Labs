using System.IO;
using Cwiczenie1.BitFormatting;

namespace Cwiczenie1.CodingStream
{
    public abstract class CodingStream
    {
        public Stream Input { get; set; }
        public Stream Output { get; set; }
        public BitFormatter Formatter { get; set; }

        public abstract void StartFlow();
    }
}