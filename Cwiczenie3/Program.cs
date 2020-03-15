using System;
using System.IO;

namespace Cwiczenie3
{
    class Program
    {
        static void Main(string[] args)
        {
            //var dict = HuffmanCoding.CalculateCharacterFrequences("aabdbcedabeddd");
            //dict.Print();
            //var tree = HuffmanCoding.BuildHuffmanTree(dict);
            //var cod = HuffmanCoding.GetHuffmanCodes(tree);
            //Console.WriteLine();
            //cod.Print();
            var f = new HuffmanBinaryFormatter();
            var c = f.Encode("abadda");
            File.WriteAllBytes(@"E:\huff.bin", c);
            var data = File.ReadAllBytes(@"E:\huff.bin");
            var s = f.Decode(data);
        }
    }
}
