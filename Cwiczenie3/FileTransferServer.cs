using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cwiczenie3
{
    public class FileTransferServer
    {
        // change to private
        public Socket serverSocket;
        public Socket clientSocket;
        public byte[] buffer;

        public FileTransferServer(string address, int port)
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            }catch(Exception e)
            {

            }
           
        }

        public void Listen()
        {
            try
            {
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);
            }catch(Exception e)
            {

            }

        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(ar);
                ClientConnected(this, EventArgs.Empty);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }catch(Exception e)
            {

            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int received = clientSocket.EndReceive(ar);

                if(received == 0)
                {
                    return;
                }

                DataReceived(this, EventArgs.Empty);
            }catch(Exception e)
            {
                
            }
        }

        public event EventHandler ClientConnected;
        public event EventHandler DataReceived;
    }
}
