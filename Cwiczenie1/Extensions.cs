using System;
using System.Collections.Generic;
using System.Linq;

namespace Cwiczenie1
{
    public static class Extensions
    {
        public static byte ToByte(this IEnumerable<byte> bits)
        {
            if (bits.Count() != 8)
            {
                throw new Exception("Wrong size");
            }

            byte result = 0;
            for (int i = 0; i < bits.Count(); i++)
            {
                result <<= 1;
                result += bits.ElementAt(i);
            }

            return result;
        }

        public static byte ReadBinaryByte(this ReadOnlySpan<char> bits)
        {
            if (bits.Length != 8)
            {
                throw new Exception("wrong size");
            }

            byte result = 0;
            for (int i = 0; i < bits.Length; i++)
            {
                result <<= 1;
                byte t = (byte) (bits[i] - 48);
                if (t != 0 && t != 1)
                {
                    throw new Exception("wrong format");
                }

                result += t;
            }

            return result;
        }

        public static byte ReadBinaryByte(this string bits)
        {
            return bits.AsSpan().ReadBinaryByte();
        }

        public static bool TryReadBinaryByte(this string bits, out byte result)
        {
            try
            {
                result = bits.ReadBinaryByte();
                return true;
            }
            catch (Exception e)
            {
                result = 0;
                return false;
            }
        }
    }
}