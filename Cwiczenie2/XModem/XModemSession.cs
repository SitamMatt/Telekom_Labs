using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenie2.XModem
{
    public class XModemSession : IDisposable
    {
        private SerialPort _port;

        public XModemSession(string portName)
        {
            _port = new SerialPort(portName);
        }

        public XModemSession(SerialPort port)
        {
            _port = port;
        }

        public void Receive(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var receiver = new XModemReceiver(file, _port);
                receiver.Receive();
            }
        }

        public void Send(string fileName)
        {
            using (var file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var sender = new XModemSender(_port);
                sender.Send(file);
            }
        }

        public void Dispose()
        {
            _port?.Dispose();
        }
    }
}
