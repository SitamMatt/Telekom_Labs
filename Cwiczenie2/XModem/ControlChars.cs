using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenie2.Xmodem
{
    public static class ControlChars
    {
        public const byte SOH = 0x01;
        public const byte EOT = 0x04;
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;
        public const byte CAN = 0x18;
        public const byte SUB = 26;
        public const byte C = 0x43;
    }
}
