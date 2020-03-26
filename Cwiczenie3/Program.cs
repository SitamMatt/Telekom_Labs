using System;
using System.Threading;
using System.Threading.Tasks;
using Telekom.Sockets;

namespace Telekom.Huffman
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Hello World";
            var encoded = Telekom.Encoding.Huffman.Encode(text);
            var decoded = Telekom.Encoding.Huffman.Decode(encoded);
            Console.WriteLine(text == decoded);
            var server = new FileTransferServer("127.0.0.1", 5678);
            var client = new FileTransferClient("127.0.0.1", 5678);
            var t1 = server.AcceptAsync();
            var t2 = client.ConnectAsync();
            Task.WaitAll(t2, t1);
            var t3 = client.SendAsync(@"E:\filename.txt");
            Thread.Sleep(5000);
            var t4 = server.ReceiveAsync(@"E:\file.txt");
            Task.WaitAll(t4, t3);
        }
    }
}
