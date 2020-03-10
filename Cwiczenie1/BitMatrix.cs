using System;
using System.Collections.Specialized;

namespace Cwiczenie1
{
    public class BitMatrix
    {
        public int M { get; private set; }
        public int N { get; private set; }

        private bool[,] matrix;

        public BitMatrix(int M, int N)
        {
            this.M = M;
            this.N = N;
            this.matrix = new bool[M, N];
        }

        public BitMatrix(bool[,] data)
        {
            this.matrix = data;
            this.M = data.GetLength(0);
            this.N = data.GetLength(1);
        }

        public static BitMatrix Identity(int k)
        {
            var result = new BitMatrix(k,k);
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (i == j)
                    {
                        result[i, j] = true;
                    }
                    else
                    {
                        result[i, j] = false;
                    }
                }
            }

            return result;
        }

        public BitMatrix(bool[] data)
        {
            matrix = new bool[data.Length,1];
            for (int i = 0; i < data.Length; i++)
            {
                matrix[i,0] = data[i];
            }

            N = 1;
            M = data.Length;
        }

        public BitMatrix(int[,] data)
        {
            this.matrix = new bool[data.GetLength(0),data.GetLength(1)];
            this.M = data.GetLength(0);
            this.N = data.GetLength(1);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    matrix[i, j] = data[i, j] != 0;
                }
            }
        }

        public BitMatrix(in byte b)
        {
            matrix = new bool[1,8];
            M = 1;
            N = 8;
            byte t = 128;
            for (int i = 0; i < N; i++)
            {
                matrix[0, i] = (t & b) != 0;
                t /= 2;
            }
        }

        public BitMatrix(byte[] buffer)
        {
            
            matrix = new bool[1,buffer.Length*8];
            M = 1;
            N = buffer.Length * 8;
            for (int i = 0; i < buffer.Length; i++)
            {
                byte t = 128;
                for (int j = 0; j < 8; j++)
                {
                    matrix[0, i * 8 + j] = (t & buffer[i]) != 0;
                    t /= 2;
                }
            }
        }

        public BitMatrix(byte[] buffer, int len)
        {
            matrix = new bool[1,len];
            M = 1;
            N = len;
            for (int i = 0; i < buffer.Length; i++)
            {
                byte t = 128;
                int l = len < 8 ? len % 8 : 8;
                for (int j = 0; j < l; j++)
                {
                    matrix[0, i * 8 + j] = (t & buffer[i]) != 0;
                    t /= 2;
                }

                len -= 8;
            }
        }

        public bool this[int i, int j] {
            get => matrix[i, j];
            set => matrix[i, j] = value;
        }

        public BitVector this[int i]
        {
            get
            {
                var arr = new bool[M];
                for (int j = 0; j < M; j++)
                {
                    arr[j] = matrix[j, i];
                }
                return new BitVector(arr);
            }
        }

        public bool IsZero()
        {
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] == true)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public BitMatrix Transpose()
        {
            var result = new BitMatrix(N, M);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[j, i] = matrix[i, j];
                }
            }

            return result;
        }

        public static void Print(BitMatrix matrix)
        {
            for (int i = 0; i < matrix.M; i++)
            {
                String line = "";
                for (int j = 0; j < matrix.N; j++)
                {
                    line += Convert.ToInt32(matrix.matrix[i, j]) + " ";
                }
                Console.WriteLine(line);
            }
        }

        public static BitMatrix MergeHorizontally(BitMatrix lgh, BitMatrix rgh)
        {
            return lgh.MergeHorizontally(rgh);
        }

        public BitMatrix MergeHorizontally(BitMatrix rgh)
        {
            if (this.M != rgh.M)
            {
                throw new Exception("Wrong matrix size");
            }
            var result = new BitMatrix(M,N + rgh.N);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i, j] = this[i, j];
                }

                for (int j = 0; j < rgh.N; j++)
                {
                    result[i, N + j] = rgh[i, j];
                }
            }

            return result;
        }

        public static BitMatrix MergeVertically(BitMatrix lgh, BitMatrix rgh)
        {
            return lgh.MergeVertically(rgh);
        }

        public BitMatrix MergeVertically(BitMatrix rgh)
        {
            if (this.N != rgh.N)
            {
                throw new Exception("Wrong matrix size");
            }
            var result = new BitMatrix(M + rgh.M,N);
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    result[i, j] = this[i, j];
                }
            }

            for (int i = 0; i < rgh.M; i++)
            {
                for (int j = 0; j < rgh.N; j++)
                {
                    result[i + M, j] = rgh[i, j];
                }
            }

            return result;
        }

        public static BitMatrix operator *(BitMatrix lgh, BitMatrix rgh)
        {
            if (lgh.N != rgh.M)
            {
                throw new Exception("Wrong matrix size");
            }
            
            var result = new BitMatrix(lgh.M, rgh.N);
            for (int i = 0; i < result.M; i++)
            {
                for (int j = 0; j < result.N; j++)
                {
                    result.matrix[i, j] = false;
                    for (int k = 0; k < lgh.N; k++)
                    {
                        result.matrix[i, j] ^= lgh.matrix[i, k] & rgh.matrix[k, j];
                    }
                }
            }

            return result;
        }

        public BitVector ToVector()
        {
            if (M == 1)
            {
                var arr = new bool[N];
                for (int i = 0; i < N; i++)
                {
                    arr[i] = this[M-1, i];
                }
                return new BitVector(arr);
            }

            if (N == 1)
            {
                var arr = new bool[M];
                for (int i = 0; i < M; i++)
                {
                    arr[i] = this[i, N-1];
                }
                return new BitVector(arr);
            }
            throw new Exception("BitMatrix is not a vector");
        }

        public override string ToString()
        {
            String result = "";
            for (int i = 0; i < M; i++)
            {
                String line = "";
                for (int j = 0; j < N; j++)
                {
                    line += Convert.ToInt32(matrix[i, j]) + " ";
                }
                result += line + "\n";
            }
            return result;
        }
    }
}