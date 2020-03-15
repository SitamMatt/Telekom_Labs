using Cwiczenie3;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestLab
{
    public class BitStackTest
    {
        [Fact]
        public void StackTest()
        {
            Span<byte> span = stackalloc byte[] { 7 };
            BitStack stack = new BitStack(span);
            Assert.True(stack[7]);
            Assert.False(stack[3]);
            Assert.True(stack[6]);
            Assert.Equal(0, stack.Cursor);
            stack.Append(true);
            stack.Append(false);
            Assert.Equal(2, stack.Cursor);
            Assert.Equal(135, span[0]);
        }
    }
}
