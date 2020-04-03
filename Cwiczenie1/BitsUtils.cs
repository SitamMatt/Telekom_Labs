using System;
using System.IO;
using Cwiczenie1;

namespace Telekom
{
    public static class BitsUtils
    {

        public static int GetBytesSize(int bitsCount)
        {
            int size = bitsCount / 8;
            if (bitsCount % 8 != 0)
                size++;
            return size;
        }
        public static byte[] ToBinary(ReadOnlySpan<char> str)
        {
            int len = str.Length / 8;
            int rest = str.Length % 8;
            
            byte[] array = new byte[rest == 0 ? len : len+1];

            for (int i = 0; i < len; i++)
            {
                var span = str.Slice(i * 8, 8);
                byte res = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (span[j] == '1')
                    {
                        res |= (byte) (1 << (7 - j));
                    }else if (span[j] != '0')
                    {
                        throw new Exception("Wrong format, expected 0 or 1");
                    }
                }

                array[i] = res;
            }

            var s = str.Slice(str.Length - rest);
            for (int i = 0; i < rest; i++)
            {
                if (s[i] == '1')
                {
                    array[^1] |= (byte) (1 << (7 - i));
                }else if (s[i] != '0')
                {
                    throw new Exception("Wrong format, expected 0 or 1");
                }
            }
            return array;
        }
    }
}