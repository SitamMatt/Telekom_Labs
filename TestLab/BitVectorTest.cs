using Telekom.Bits;
using Xunit;

namespace TestLab
{
    public class BitVectorTest
    {
        [Fact]
        public void IndexerTest()
        {
            byte b = 255;
            BitVector vector = new BitVector(b);
            Assert.True(vector[0]);
            Assert.True(vector[1]);
            Assert.True(vector[2]);
            Assert.True(vector[3]);
            Assert.True(vector[4]);
            Assert.True(vector[5]);
            Assert.True(vector[6]);
            Assert.True(vector[7]);
            vector[5] = false;
            Assert.False(vector[5]);
        }

        [Fact]
        public void BitMatrixMultiplicityTest()
        {
            BitMatrix matrix = new BitMatrix(new int[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1},
                {0,1,1,1},
                {1,0,1,1},
                {1,1,0,1}
            });
            
            BitVector vector = new BitVector(255);

            BitVector result = matrix * vector;
            
        }
    }
}