using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Telekom.Sockets
{
    public class FileTransferClient
    {
        private Socket socket;
        private IPEndPoint ipEndPoint;

        public FileTransferClient(string address, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task ConnectAsync()
        {
            await socket.ConnectAsync(ipEndPoint);
        }

        public async Task SendAsync(string filename)
        {
            //using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            //{
            //var bytes = new byte[fs.Length];
            var msg = File.ReadAllText(filename);
            var bytes = Telekom.Encoding.Huffman.Encode(msg);
                //fs.Read(bytes);
            var res = await socket.SendAsync(bytes, SocketFlags.None);
            //}
        }

        public void Close()
        {
            socket.Close();
        }
    }
}