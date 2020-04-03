using System;
using System.Collections.Generic;
using System.Linq;
using Telekom.Bits;

namespace Cwiczenie1
{
    public static class Extensions
    {
        public static BitVector ToBitVector(this byte bits, bool direction = false)
        {
            return new BitVector(bits, direction);
        }

        public static void Print(this BitVector vector)
        {
            for (int i = 0; i < vector.Length; i++)
                Console.WriteLine(vector[i] ? '1' : '0');
        }
    }
}