using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie2.XModem
{
    public static class Utils
    {
        public static int CalculateChecksum(ReadOnlySpan<byte> data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum % 256;
        }
    }
}
