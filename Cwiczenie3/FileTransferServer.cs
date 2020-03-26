using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Telekom.Sockets
{
    public class FileTransferServer
    {
        private Socket socket;
        private Socket client;
        private IPEndPoint ipEndPoint;

        public FileTransferServer(string address, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task AcceptAsync()
        {
            socket.Bind(ipEndPoint);
            socket.Listen(100);
            client = await socket.AcceptAsync();
        }

        public async Task ReceiveAsync(string filename)
        {
            var buffer = new byte[1024];
            var res = await client.ReceiveAsync(buffer, SocketFlags.None);
            var decoded = Telekom.Encoding.Huffman.Decode(buffer.AsSpan(0, res).ToArray());
            File.WriteAllText(filename, decoded);
        }

        public void Close()
        {
            socket.Close();
        }
    }
}