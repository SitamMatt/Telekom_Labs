using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cwiczenie1
{
    public class BitVector
    {
        private bool[] _vector;
        public int Length => _vector.Length;

        public BitVector(bool[] vector)
        {
            this._vector = vector;
        }

        public BitVector(int length)
        {
            this._vector = new bool[length];
        }

        public bool this[int i]
        {
            get => _vector[i];
            set => _vector[i] = value;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(_vector.Length * 2);
            for (int i = 0; i < _vector.Length; i++)
            {
                builder.Append(_vector[i] == false ? 0 : 1).Append(" ");
            }

            return builder.ToString();
        }

        public byte[] ToByteArray(bool offset = false)
        {
            if (!offset && (Length % 8 != 0))
            {
                throw new Exception("Wrong size");
            }

            int flen = Length % 8 == 0 ? Length / 8 : Length / 8 + 1;
            int len = Length / 8;
            byte[] arr = new byte[flen];
            for (int i = 0; i < len; i++)
            {
                byte b = 0;
                for (int j = 0; j < 8; j++)
                {
                    b <<= 1;
                    byte c = (byte)(this[i*8 + j] == false ? 0 : 1);
                    b += c;
                }
                arr[i] = b;
            }

            if (offset)
            {
                byte b1 = 0;
                for (int i = 0; i < Length % 8; i++)
                {
                    b1 <<= 1;
                    byte c = (byte) (this[len * 8 + i] == false ? 0 : 1);
                    b1 += c;
                }

                b1 <<= 8 - (Length % 8);
                arr[flen - 1] = b1;
            }

            return arr;
        }

        public static BitVector operator +(BitVector lgh, BitVector rgh)
        {
            if (lgh.Length != rgh.Length)
            {
                throw new Exception("Wrong size");
            }
            var result = new BitVector(lgh.Length);
            for (int i = 0; i < lgh.Length; i++)
            {
                result[i] = lgh[i] ^ rgh[i];
            }

            return result;
        }
        
        public static bool operator !=(BitVector lgh, BitVector rgh)
        {
            return !(lgh == rgh);
        }

        public static bool operator ==(BitVector lgh, BitVector rgh)
        {
            if (lgh.Length != rgh.Length)
            {
                return false;
            }

            for (int i = 0; i < lgh.Length; i++)
            {
                if (lgh[i] != rgh[i])
                {
                    return false;
                }
            }

            return true;
        }

    }
}