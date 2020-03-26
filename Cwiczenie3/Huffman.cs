using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telekom.BitCollections;
using Telekom.Huffman;

namespace Telekom.Encoding
{
    public static class Huffman
    {
        public static byte[] Encode(string text)
        {
            HuffmanTree tree = HuffmanTree.Build(text);
            var dict = tree.Dictionary;
            var (encoded, padding) = Encode(text, dict);
            int maxLen = 2;
            int estimatedBufferSize = 1 + dict.Count * (1 + 1 + maxLen) + 1 + encoded.Length;
            var buff = new byte[estimatedBufferSize];
            using var ms = new MemoryStream(buff);
            ms.WriteByte((byte) dict.Count);
            foreach (var kvp in dict)
            {
                ms.WriteByte((byte) kvp.Key);
                ms.WriteByte((byte) kvp.Value.Length);
                ms.Write(kvp.Value.GetBytes().Item1);
            }

            ms.WriteByte((byte) padding);
            ms.Write(encoded);
            return buff.AsSpan(0, (int) ms.Position).ToArray();
        }

        public static string Decode(byte[] data)
        {
            using var ms = new MemoryStream(data);
            int dictLen = ms.ReadByte();
            var dict = new Dictionary<BitSequence, char>();
            for (int i = 0; i < dictLen; i++)
            {
                char c = (char) ms.ReadByte();
                int seqLen = ms.ReadByte();
                BitSequence seq = new BitSequence(seqLen);
                ms.Read(seq.rawData, 0, seq.ByteCapacity);
                seq.PushLength(seqLen);
                dict[seq] = c;
            }

            int padding = ms.ReadByte();

            int encodedLen = ms.Capacity - (int) ms.Position;
            byte[] encoded = new byte[encodedLen];
            ms.Read(encoded, 0, encodedLen);
            string decoded = Decode(encoded, padding, dict);
            return decoded;
        }

        private static string Decode(byte[] data, int padding, Dictionary<BitSequence, char> dictionary)
        {
            int len = data.Length * 8 - padding;
            StringBuilder sb = new StringBuilder();
            BitSequence encoded = new BitSequence(data, len);
            BitSequence temp = new BitSequence(8);
            for (int i = 0; i < len; i++)
            {
                temp.Push(encoded[i]);
                if (dictionary.ContainsKey(temp))
                {
                    sb.Append(dictionary[temp]);
                    temp = new BitSequence(16);
                }
            }

            return sb.ToString();
        }

        private static (byte[] encoded, int padding) Encode(string text, Dictionary<char, BitSequence> dictionary)
        {
            BitSequence sequence = new BitSequence(8*text.Length);
            foreach (char c in text)
            {
                sequence.Push(dictionary[c]);
            }
            return sequence.GetBytes();
        }
    }
}