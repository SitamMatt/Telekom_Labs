using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public ref struct BitStack
    {
        public BitSpan span;

        public int Cursor { get; set; }

        public BitStack(Span<byte> span)
        {
            this.span = new BitSpan(span);
            this.Cursor = 0;
        }

        public BitStack(Span<byte> span, int startingCursor)
        {
            Cursor = startingCursor;
            this.span = new BitSpan(span);
        }

        public void Append(bool value)
        {
            span[Cursor] = value;
            Cursor++;
        }

        public void Append(List<int> values)
        {
            foreach (var item in values)
            {
                Append(item != 0);
            }
        }

        public void Append(string values)
        {
            Convert.ToByte(values, 2);
            foreach(var c in values)
            {
                Append(c != '0');
            }
        }

        public bool this[int i]
        {
            get
            {
                return span[i];
            }
            set
            {
                span[i] = value;
            }
        }
    }
}
