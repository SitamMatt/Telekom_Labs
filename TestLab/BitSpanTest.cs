using Cwiczenie3;
using System;
using Xunit;

namespace TestLab
{
    public class BitSpanTest
    {
        [Fact]
        public void IndexerTest()
        {
            Span<byte> span = stackalloc byte[] { 7 };
            BitSpan bitSpan = new BitSpan(span);
            Assert.True(bitSpan[7]);
            Assert.False(bitSpan[3]);
            Assert.True(bitSpan[6]);
            bitSpan[6] = false;
            Assert.False(bitSpan[6]);
            Assert.False(bitSpan[1]);
            bitSpan[1] = true;
            Assert.True(bitSpan[1]);
            Assert.Equal(span[0], 69);
        }

        [Fact]
        public void RangeTest()
        {
            Span<byte> span = stackalloc byte[] { 87 };
            BitSpan bitSpan = new BitSpan(span);
            byte b1 = bitSpan.ByteFromRange(0, 4);
            Assert.Equal(5, b1);
            byte b2 = bitSpan.ByteFromRange(2, 3);
            Assert.Equal(2, b2);
        }
    }
}
