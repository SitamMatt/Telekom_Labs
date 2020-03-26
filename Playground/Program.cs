using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(Run).Wait();
        }

        static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Run()
        {
            var server = new TcpListener(IPAddress.Parse("127.0.0.1"),6758 );
            var client = new TcpClient("127.0.0.1", 6787);
            var cliennt = await server.AcceptTcpClientAsync();
            cliennt.GetStream();
        }
    }
}
