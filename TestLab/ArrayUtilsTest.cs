using Cwiczenie1;
using Xunit;

namespace TestLab
{
    public class ArrayUtilsTest
    {
        [Fact]
        public void Array2DCopyTest()
        {
            bool[,] array = new bool[,]
            {
                {true, false},
                {false, true}
            };
            
            bool[,] array2 = new bool[3,3];
            
            ArrayUtils.Copy(array, array2, 1,1);
            
            Assert.True(array2[1,1]);
            Assert.True(array2[2,2]);
        }
    }
}