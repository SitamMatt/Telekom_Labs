using System;

namespace Cwiczenie1
{
    public static class ArrayUtils
    {
        public static void Copy(bool[,] source, bool[,] destination, int verticalOffset, int horizontalOffset)
        {
            int len = source.GetLength(0);
            int destWidth = destination.GetLength(1);
            int sourceWidth = source.GetLength(1);

            for (var x = 0; x < len; x++)
            {
                Array.Copy(source, x*sourceWidth, destination, (x+verticalOffset)*destWidth + horizontalOffset, sourceWidth);
            }
        }
    }
}