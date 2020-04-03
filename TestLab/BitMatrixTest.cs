using Cwiczenie1;
using Telekom.Bits;
using Xunit;

namespace TestLab
{
    public class BitMatrixTest
    {
        [Fact]
        public void IndexerTest()
        {
            BitMatrix matrix = new BitMatrix(new bool[,]
            {
                {false, true},
                {true, false}
            });
            
            Assert.False(matrix[0,0]);
            Assert.True(matrix[0,1]);
            Assert.True(matrix[1,0]);
            Assert.False(matrix[1,1]);
        }

        [Fact]
        public void IntConstructorTest()
        {
            BitMatrix matrix = new BitMatrix(new int[,]
            {
                {0,1},
                {1,0}
            });
            
            Assert.False(matrix[0,0]);
            Assert.True(matrix[0,1]);
            Assert.True(matrix[1,0]);
            Assert.False(matrix[1,1]);
        }

        [Fact]
        public void IdentityMatrixConstructor()
        {
            BitMatrix matrix = BitMatrix.Identity(2);
            
            Assert.True(matrix[0,0]);
            Assert.False(matrix[0,1]);
            Assert.False(matrix[1,0]);
            Assert.True(matrix[1,1]);
        }

        [Fact]
        public void HorizontalAppendTest()
        {
            BitMatrix matrix = new BitMatrix(new int[,]
            {
                {0,1},
                {1,0}
            });
            
            BitMatrix identity = BitMatrix.Identity(2);

            BitMatrix merged = identity.AppendHorizontally(matrix);
            Assert.True(merged[0,0]);
            Assert.True(merged[1,1]);
            Assert.True(merged[1,2]);
            Assert.True(merged[0,3]);
        }

        [Fact]
        public void TransposeTest()
        {
            BitMatrix matrix = new BitMatrix(new int[,]
            {
                {0,1},
                {1,0},
                {1,1}
            });

            BitMatrix transposed = matrix.Transpose();
            Assert.True(transposed[1,0]);
            Assert.True(transposed[0,1]);
            Assert.True(transposed[0,2]);
            Assert.True(transposed[1,2]);
        }
    }
}