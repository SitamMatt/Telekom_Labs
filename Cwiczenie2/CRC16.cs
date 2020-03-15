using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie2
{
    public static class CRC16
    {
        private const int poly = 0x11021; // wielomian do dzielenia

        private static byte[] masks = // maska to wyodrębniania poszczególnych bitów
        {
            1,2,4,8,16,32,64,128
        };

        public static byte[] Calculate(ReadOnlySpan<byte> data)
        {
            const ushort generator = 0x1021; /* divisor is 16bit */
            ushort crc = 0; /* CRC value is 16bit */

            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8); /* move byte into MSB of 16bit CRC */

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0) /* test for MSB = bit 15 */
                    {
                        crc = (ushort)((crc << 1) ^ generator);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            Console.WriteLine(Convert.ToString(crc, 16));
            //Span<byte> span = stackalloc byte[data.Length+2];
            //data.CopyTo(span); // kopiowanie na stos danych wejściowych

            //int len = span.Length * 8; // licznik aktualnego bitu

            //while (--len >= 16) // dopóki licznik bitów jest większy od 0 (większa lub równa 4)
            //{
            //    int i = len % 8;
            //    int j = span.Length - 1 - len / 8;
            //    if((span[j] & masks[i]) == 0)
            //    {
            //        continue;
            //    }
            //    else
            //    {
            //        Print(span);
            //        for (int l = 0; l < j; l++)
            //        {
            //            Console.Write(Convert.ToString((byte)0, 2).PadLeft(8, '0') + " ");
            //        }
            //        Console.Write(Convert.ToString((byte)(poly >> (16-i)), 2).PadLeft(8, '0') + " ");
            //        Console.Write(Convert.ToString((byte)(poly >> (8 - i)), 2).PadLeft(8, '0')+ " ");
            //        Console.Write(Convert.ToString(unchecked((byte)(poly << i)), 2).PadLeft(8, '0')+ " ");

            //        Console.WriteLine();
            //        Console.WriteLine();

            //        span[j] ^= (byte)(poly >> (16 - i));
            //        span[j + 1] ^= (byte)(poly >> (8 - i));
            //        span[j + 2] ^= (byte)(poly << i);
            //    }
            //}
            //Print(span);

            return new byte[4]; 
        }


        public static bool Check(ReadOnlySpan<byte> data)
        {
            return true;
        }

        public static void Print(ReadOnlySpan<byte> data)
        {
            foreach (var item in data)
            {
                Console.Write(Convert.ToString(item, 2).PadLeft(8, '0') + " ");
            }
            Console.WriteLine();
        }
    }
}
