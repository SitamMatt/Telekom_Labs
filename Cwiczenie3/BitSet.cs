using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public struct BitSet
    {
        private byte buffer;

        public static byte[] masks =
        {
            128,64,32,16,8,4,2,1
        };

        public int Length { get; private set; }

        public BitSet(List<int> list)
        {
            buffer = 0;
            Length = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                this[i] = list[i] != 0;
            }
        }

        private bool this[int i]
        {
            get
            {
                byte masked = (byte)(buffer & masks[i]);
                return masked != 0;
            }
            set
            {
                if (value)
                {
                    buffer = (byte)(buffer | masks[i]);
                }
                else
                {
                    byte mask = (byte)(~masks[i]);
                    buffer = (byte)(buffer & mask);
                }

            }
        }

        public static bool operator==(BitSet rgh, BitSet lgh)
        {
            if (rgh.Length != lgh.Length)
                return false;

            return rgh == lgh;
        }

        public static bool operator!=(BitSet rgh, BitSet lgh)
        {
            return !(rgh == lgh);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Length);
            for (int i = 0; i < Length; i++)
            {
                sb.Append(this[i] ? "1" : "0");
            }
            return sb.ToString();
        }
    }
}
