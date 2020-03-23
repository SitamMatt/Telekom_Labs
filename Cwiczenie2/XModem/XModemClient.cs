using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Telekom.XModem.Internal;

namespace Telekom.XModem
{
    public class XModemClient : IDisposable
    {
        private SerialPort serialPort;

        public bool IsOperationPending { get; set; } = false;

        public XModemClient(string port)
        {
            serialPort = new SerialPort(port);
            serialPort.Open();
        }

        public void Dispose()
        {
            serialPort?.Dispose();
        }

        public void Send(string filename, CancellationToken token, bool crc = false)
        {
            try
            {
                IsOperationPending = true;
                using FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                XModemSender sender = new XModemSender(serialPort, fs, crc);
                sender.Send(token);
                IsOperationPending = false;
            }
            catch (OperationCanceledException)
            {
                IsOperationPending = false;
                throw;
            }
            catch (ExceededAttemptsException)
            {
                IsOperationPending = false;
                throw;
            }
        }

        public Task SendAsync(string filename, CancellationToken token, bool crc = false)
        {
            return Task.Run(() => Send(filename, token, crc), token);

        }

        public Task ReceiveAsync(string filename)
        {
            return ReceiveAsync(filename, CancellationToken.None);
        }

        public Task ReceiveAsync(string filename, CancellationToken token)
        {
            return Task.Run(() => Receive(filename, token), token);
        }

        public void Receive(string filename, CancellationToken token)
        {
            try
            {
                IsOperationPending = true;
                using FileStream fs = new FileStream(filename, FileMode.Create);
                XModemReceiver receiver = new XModemReceiver(serialPort, fs);
                receiver.Receive(token);
                IsOperationPending = false;
            }
            catch (OperationCanceledException)
            {
                IsOperationPending = false;
                throw;
            }
            catch (ExceededAttemptsException)
            {
                IsOperationPending = false;
                throw;
            }
        }
    }
}