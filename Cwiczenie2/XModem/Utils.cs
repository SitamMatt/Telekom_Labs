using System;
using System.Collections.Generic;
using System.Text;

namespace Telekom
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
        
        public static ushort CalculateCRC(ReadOnlySpan<byte> data)
        {
            const ushort generator = 0x1021; // wielomian
            ushort crc = 0; // wynik CRC

            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ generator);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            return crc;
        }
    }
}
