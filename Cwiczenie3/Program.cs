using System;
using System.IO;

namespace Cwiczenie3
{
    class Program
    {
        static void Main(string[] args)
        {
            FileTransferServer server = new FileTransferServer("127.0.0.1", 6666);
            server.Listen();
            FileTransferClient client = new FileTransferClient();
            client.Connect("127.0.0.1", 6666);
            client.Send("Hello World");

            Console.ReadLine();
            //var dict = HuffmanCoding.CalculateCharacterFrequences("aabdbcedabeddd");
            //dict.Print();
            //var tree = HuffmanCoding.BuildHuffmanTree(dict);
            //var cod = HuffmanCoding.GetHuffmanCodes(tree);
            //Console.WriteLine();
            //cod.Print();
            //var f = new HuffmanBinaryFormatter();
            //var c = f.Encode("abadda");
            //File.WriteAllBytes(@"E:\huff.bin", c);
            //var data = File.ReadAllBytes(@"E:\huff.bin");
            //var s = f.Decode(data);
        }
    }
}
