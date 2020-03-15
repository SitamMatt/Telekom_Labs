using Cwiczenie3;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestLab
{
    public class BitSetTest
    {
        [Fact]
        public void ToStringTest()
        {
            List<int> list = new List<int>() { 1, 0, 1, 0, 0 };
            BitSet bits = new BitSet(list);
            Assert.Equal("10100", bits.ToString());
        }

        [Fact]
        public void EqualityOperatorTest()
        {
            List<int> list1 = new List<int>() { 1, 0, 1, 0, 0 };
            List<int> list2 = new List<int>() { 1, 0, 1, 0, 0, 0 };
            List<int> list3 = new List<int>() { 1, 1, 1, 0, 0 };
            List<int> list4 = new List<int>() { 1, 0, 1, 0, 0 };
            BitSet bits1 = new BitSet(list1);
            BitSet bits2 = new BitSet(list2);
            BitSet bits3 = new BitSet(list3);
            BitSet bits4 = new BitSet(list4);
            Assert.Equal(bits1, bits1);
            Assert.NotEqual(bits1, bits2);
            Assert.NotEqual(bits1, bits3);
            Assert.Equal(bits1, bits4);
        }
    }
}
