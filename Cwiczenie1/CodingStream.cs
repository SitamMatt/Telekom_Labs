using System.IO;

namespace Cwiczenie1
{
    public abstract class CodingStream
    {
        public Stream Input { get; set; }
        public Stream Output { get; set; }
        public BitFormatter Formatter { get; set; }

        public abstract void StartFlow();
    }
}