using System;

namespace Telekom.Bits
{
    public ref struct BitVector
    {
        private Span<byte> data;

        public Span<byte> Bytes => data;
        public int Length => data.Length*8;

        public BitVector(byte bits)
        {
            data = new byte[]{bits};
        }

        public BitVector(Span<byte> bits)
        {
            data = bits;
        }

        public BitVector(byte[] bits, bool direction = false)
        {
            data = bits;
        }
        
        public BitVector(byte bits, bool direction)
        {
            data = new[] {bits};
        }

        public bool IsZero()
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != 0)
                    return false;
            }

            return true;
        }

        public static BitVector operator *(BitMatrix matrix, BitVector vector)
        {
            int len = matrix.M;    
            // if(matrix.N != vector.Length)
            //     throw new Exception("Invalid Matrix Size");
            
            BitVector result = new BitVector(new byte[BitsUtils.GetBytesSize(len)]);
            for (int i = 0; i < len; i++)
            {
                bool res = false;
                for (int j = 0; j < matrix.N; j++)
                {
                    res ^= matrix[i, j] & vector[j];
                }

                result[i] = res;
            }

            return result;
        }
        
        public bool this[int index]
        {
            get
            {
                int byteIndex = index / 8;
                byte bitIndex = (byte) (index % 8);
                byte mask = (byte)(1 << (7 - bitIndex));
                byte bit = (byte) (data[byteIndex] & mask);
                return bit != 0;
            }
            set
            {
                if (value == this[index]) return;
                int byteIndex = index / 8;
                byte bitIndex = (byte) (index % 8);
                byte mask = (byte)(1 << (7 - bitIndex));
                if (value)
                    data[byteIndex] |= mask;
                else
                    data[byteIndex] &= (byte) (~mask);
            }
        }

        public void SwitchBit(in int index)
        {
            this[index] = !this[index];
        }
    }
}