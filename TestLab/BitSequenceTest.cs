using System;
using Telekom.BitCollections;
using Telekom.Huffman;
using Xunit;

namespace TestLab
{
    public class BitSequenceTest
    {
        [Fact]
        public void IndexerGetterTest()
        {
            var seq = new BitSequence(16);
            seq.rawData = new byte[] {3, 5};
            Assert.False(seq[0]);
            Assert.True(seq[6]);
            Assert.True(seq[7]);
            Assert.False(seq[8]);
            Assert.False(seq[9]);
            Assert.True(seq[13]);
            Assert.True(seq[15]);
            Assert.Throws<ArgumentOutOfRangeException>(() => seq[16]);
        }

        [Fact]
        public void IndexerSetterTest()
        {
            var seq = new BitSequence(16);
            seq.rawData = new byte[] {3, 5};
            Assert.False(seq[0]);
            seq[0] = true;
            Assert.True(seq[0]);
            Assert.False(seq[5]);
            seq[5] = false;
            Assert.False(seq[5]);
            Assert.True(seq[6]);
            seq[6] = false;
            Assert.False(seq[6]);
            Assert.False(seq[12]);
            seq[12] = true;
            Assert.True(seq[12]);
            Assert.True(seq[13]);
            seq[13] = false;
            Assert.False(seq[13]);
            Assert.Throws<ArgumentOutOfRangeException>(() => seq[16] = true);
        }

        [Fact]
        public void PushTest()
        {
            var seq = new BitSequence(16);
            seq.Push();
            seq.Push(true);
            seq.Push(false);
            seq.Push(true);
            seq.Push(true);
            seq.Push(false);
            Assert.False(seq[0]);
            Assert.True(seq[1]);
            Assert.False(seq[2]);
            Assert.True(seq[3]);
            Assert.True(seq[4]);
            Assert.False(seq[5]);
            Assert.Equal(6, seq.Length);
        }

        [Fact]
        public void DeepCopyTest()
        {
            var seq1 = new BitSequence(16);
            seq1.Push(true);
            seq1.Push(true);
            var seq2 = seq1.DeepCopy();
            Assert.True(seq2[0]);
            Assert.True(seq2[1]);
            Assert.True(seq1[0]);
            Assert.True(seq1[1]);
            Assert.Equal(seq1.rawData.Length, seq2.rawData.Length);
            seq2[0] = false;
            Assert.True(seq1[0]);
            Assert.False(seq2[0]);
        }

        [Fact]
        public void ToStringTest()
        {
            var seq = new BitSequence(16);
            seq.Push();
            seq.Push(true);
            seq.Push(false);
            seq.Push(true);
            seq.Push(true);
            seq.Push(false);
            Assert.Equal("010110", seq.ToString());
        }

        [Fact]
        public void PushSequenceTest()
        {
            var seq1 = new BitSequence(16);
            var seq2 = new BitSequence(8);
            seq1.Push();
            seq1.Push(true);
            seq2.Push();
            seq2.Push(true);
            seq2.Push(true);
            seq2.Push(true);
            seq1.Push(seq2);
            Assert.Equal("010111", seq1.ToString());
            Assert.Equal(6, seq1.Length);
        }

        [Fact]
        public void BytesLengthTest()
        {
            var seq = new BitSequence(16);
            Assert.Equal(0, seq.BytesLength);
            seq.Push();
            Assert.Equal(1, seq.BytesLength);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            Assert.Equal(1, seq.BytesLength);
            seq.Push(true);
            Assert.Equal(2, seq.BytesLength);
        }

        [Fact]
        public void ToBytesTest()
        {
            var seq = new BitSequence(16);
            seq.Push();
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            var (arr, pad) = seq.GetBytes();
            Assert.Equal(7, pad);
            Assert.Equal(2, arr.Length);
            Assert.Equal(new byte[]{127,128}, arr);
        }

        [Fact]
        public void EqualityTest()
        {
            var seq1 = new BitSequence(16);
            var seq2 = new BitSequence(16);
            seq1.Push(true);
            seq1.Push(true);
            seq2.Push(seq1);
            Assert.Equal(seq1,seq2);
            Assert.Equal(seq1.GetHashCode(), seq2.GetHashCode());
        }

        [Fact]
        public void ReverseTest()
        {
            var seq = new BitSequence(16);
            seq.Push();
            seq.Push(true);
            seq.Push();
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push(true);
            seq.Push();
            seq.Push(true);
            Assert.Equal("010111101", seq.ToString());
            seq.Reverse();;
            Assert.Equal("101111010", seq.ToString());
        }
    }
}