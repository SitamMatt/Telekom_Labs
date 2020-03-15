using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public class HuffmanBinaryFormatter
    {
        public int CharNumber { get; set; }
        public Dictionary<BitSet, char> CharCodes { get; set; } = new Dictionary<BitSet, char>();

        public string Decode(Span<byte> data)
        {
            CharNumber = data[0];
            int offset = 1;
            for (int i = 0; i < CharNumber; i++)
            {
                char c = (char)data[offset];
                offset++;
                List<int> vs = new List<int>();
                while(data[offset] != 255)
                {
                    vs.Add(data[offset]);
                    offset++;
                }
                BitSet set = new BitSet(vs);
                CharCodes[set] = c;
                offset++;
            }
            int paddingLen = data[offset];
            offset++;
            int len = (data.Length - offset ) * 8 - paddingLen;
            var bytes = data.Slice(offset);
            var bits = new BitSpan(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len;)
            {
                BitSet code;
                int os = 0;
                do
                {
                    os++;
                    code = bits.BitSetFromRange(i, os);
                }
                while (!CharCodes.ContainsKey(code));
                i += os;
                sb.Append(CharCodes[code]);
            }
            return sb.ToString();
        }

        public byte[] Encode(ReadOnlySpan<char> text)
        {
            var dict = HuffmanCoding.CalculateCharacterFrequences(text);
            var tree = HuffmanCoding.BuildHuffmanTree(dict);
            var cod = HuffmanCoding.GetHuffmanCodes(tree);
            int len = 0;
            byte[] buffer = new byte[1024];
            buffer[0] = (byte)dict.Count;
            len++;
            foreach (var item in cod)
            {
                buffer[len] = (byte)item.Key;
                len++;
                foreach (var b in item.Value)
                {
                    buffer[len] = (byte)b;
                    len++;
                }
                buffer[len] = 255;
                len++;
                //buffer[len] = item.Value.ToByte();
                //len++;
            }
            int paddingIndex = len;
            len++;
            var span = buffer.AsSpan(len);
            var bits = new BitStack(span);
            foreach (var item in text)
            {
                bits.Append(cod[item]);
            }
            byte padding = (byte)(8 - ((bits.Cursor) % 8));
            buffer[paddingIndex] = padding;
            for (int i = 0; i < padding; i++)
            {
                bits.Append(false);
            }
            len += bits.Cursor % 8 == 0 ? bits.Cursor / 8 : bits.Cursor /8 + 1;
            return buffer.AsSpan(0, len).ToArray();
        }


    }
}
