using System;
using Cwiczenie1;

namespace Telekom.Bits
{
    public class BitMatrix
    {
        private bool[,] data;

        public int M => data.GetLength(0);
        public int N => data.GetLength(1);

        public BitMatrix(bool[,] bits) => data = bits;

        public BitMatrix(int[,] bits)
        {
            data = new bool[bits.GetLength(0),bits.GetLength(1)];
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    data[i, j] = bits[i,j] != 0;
        }

        public static BitMatrix Identity(int diagonal)
        {
            bool[,] bits = new bool[diagonal,diagonal];
            for (int i = 0; i < diagonal; i++)
                bits[i, i] = true;
            return new BitMatrix(bits);
        }

        public bool this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }
        
        public BitMatrix AppendHorizontally(BitMatrix matrix)
        {
            if(M != matrix.M)
                throw new Exception("Invalid Matrix Size");
            int n = N + matrix.N;
            bool[,] bits = new bool[M,n];
            ArrayUtils.Copy(data, bits, 0, 0);
            ArrayUtils.Copy(matrix.data, bits, 0, N);
            return new BitMatrix(bits);
        }

        public BitMatrix Transpose()
        {
            bool[,] bits = new bool[N, M];
            for (int i = 0; i < M; i++)
                for (int j = 0; j < N; j++)
                    bits[j, i] = this[i, j];
            return new BitMatrix(bits);
        }

        public bool HasColumn(BitVector column, out int columnNumber)
        {
            for (int i = 0; i < N; i++)
            {
                bool check = true;
                for (int j = 0; j < M; j++)
                {
                    check &= (column[j] == this[j, i]);
                }

                if (check)
                {
                    columnNumber = i;
                    return true;
                }
                
            }

            columnNumber = 0;
            return false;
        }

        public bool HasColumnSum(BitVector column, out int number1, out int number2)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = i+1; j < N; j++)
                {
                    bool check = true;
                    for (int k = 0; k < M; k++)
                    {
                        check &= column[k] == (this[k, i] ^ this[k, j]);
                    }

                    if (check)
                    {
                        number1 = i;
                        number2 = j;
                        return true;
                    }
                }
            }

            number1 = 0;
            number2 = 0;
            return false;
        }
    }
}