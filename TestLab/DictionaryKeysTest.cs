using System.Collections.Generic;
using Telekom.BitCollections;
using Telekom.Huffman;
using Xunit;

namespace TestLab
{
    public class DictionaryKeysTest
    {
        [Fact]
        public void KeysTest()
        {
            Dictionary<BitSequence, char> dict=  new Dictionary<BitSequence, char>();
            var seq1 = new BitSequence(16);
            var seq2 = new BitSequence(16);
            seq1.Push(true);
            seq1.Push(true);
            seq2.Push(seq1);
            dict[seq1] = 'c';
            Assert.True(dict.ContainsKey(seq2));
        }
    }
}