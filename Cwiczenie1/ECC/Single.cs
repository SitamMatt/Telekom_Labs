using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Cwiczenie1.ECC
{
    public static class Single
    {
        // Matrix
        public static BitMatrix P = new BitMatrix(new[,]
        {
            {0, 1, 1, 1},
            {1, 0, 1, 1},
            {1, 1, 0, 1},
            {1, 1, 1, 0},
            {0, 0, 1, 1},
            {1, 0, 0, 1},
            {1, 1, 0, 0},
            {0, 1, 1, 0}
        });
        
        // Generator Matrix
        public static BitMatrix G = BitMatrix.Identity(8).MergeHorizontally(P);

        public static BitMatrix H = P.Transpose().MergeHorizontally(BitMatrix.Identity(4));
        
        
        public static BitMatrix Encode(in byte b)
        {
            var s = new BitMatrix(b);
            var vector = s * G;
            return vector;
        }

        public static bool CheckCorrectness(BitMatrix encoded)
        {
            var result = encoded * H.Transpose();
            return result.IsZero();
        }

        public static void Correct(BitMatrix encoded)
        {
            var result = H * encoded.Transpose();
            for (int i = 0; i < H.N; i++)
            {
                bool c = true;
                for (int j = 0; j < H.M; j++)
                {
                    c &= result[j, 0] == H[j,i];
                }

                if (c)
                {
                    encoded[0, i] = !encoded[0, i];
                    return;
                }
            }
            // BitMatrix.Print(result);
        }

        public static BitMatrix Decode(BitMatrix encoded)
        {
            var result = new BitMatrix(1, 8);
            for (int i = 0; i < result.N; i++)
            {
                result[0, i] = encoded[0, i];
            }

            return result;
        }
    }
}