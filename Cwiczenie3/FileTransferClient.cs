using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Cwiczenie3
{
    public class FileTransferClient
    {
        // change to private
        public Socket socket;

        public event EventHandler Connected;
        public event EventHandler ConnectionFailed;

        public bool IsConnected { get; set; } = false;

        public FileTransferClient()
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception e)
            {
            }
        }

        public void Connect(string address, int port)
        {
            //try
            //{
                var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
                var handle = socket.BeginConnect(endpoint, ConnectCallback, null);

                bool res = handle.AsyncWaitHandle.WaitOne(10000, true);
                if (socket.Connected)
                {
                    Connected(this, EventArgs.Empty);
                    socket.EndConnect(handle);
                    IsConnected = true;
                }
                else
                {
                    socket.Close();
                    throw new Exception("elo");
                }
            //}
            //catch (Exception)
            //{

            //}
            
        }

        public void Send(string msg)
        {
            var bytes = Encoding.ASCII.GetBytes(msg);
            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch (Exception)
            {
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            //try
            //{
                
            //}
            //catch (Exception e)
            //{
                
            //}
        }

        public void Send(byte[] data)
        {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, SendCallback, null);
        }
    }
}
