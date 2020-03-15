using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public ref struct BitSpan
    {
        public Span<byte> Span;


        public BitSpan(Span<byte> span)
        {
            Span = span;
            BitLength = span.Length * 8;
        }

        public static byte[] masks =
        {
            128,64,32,16,8,4,2,1
        };

        public int BitLength { get; private set; }
        public int ByteLenght { get => Span.Length; }

        public bool this[int i]
        {
            get
            {
                int byteIndex = i / 8;
                int bitIndex = i % 8;
                byte e = Span[byteIndex];
                byte masked = (byte)(e & masks[bitIndex]);
                return masked != 0;
            }
            set
            {
                int byteIndex = i / 8;
                int bitIndex = i % 8;
                byte e = Span[byteIndex];
                if (value)
                {
                    Span[byteIndex] = (byte)(e | masks[bitIndex]);
                }
                else
                {
                    byte mask = (byte)(~masks[bitIndex]);
                    Span[byteIndex] = (byte)(e & mask);
                }

            }
        }

        public byte ByteFromRange(int start, int length)
        {
            byte result = 0;
            for (int i = length - 1; i >= 0; i--)
            {
                if (this[start+i])
                {
                    result |= masks[8 - (length - i)];
                }
            }
            return result;
        }

        internal BitSet BitSetFromRange(int start, int length)
        {
            List<int> vs = new List<int>();
            for (int i = 0; i < length; i++)
            {
                vs.Add(this[start + i] ? 1 : 0);
            }
            return new BitSet(vs);
        }
    }
}
