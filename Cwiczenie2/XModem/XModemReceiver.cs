using Cwiczenie2.XModem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Cwiczenie2
{
    class XModemReceiver
    {
        private readonly SerialPort _port;
        private readonly Stream _stream;
        private readonly byte[] _buffer = new byte[132];
        private bool _isReceiving = false;
        private ManualResetEvent EOTReceived;
        private byte _expectedBlockNumber = 1;
        private readonly AbstractLogger logger = new ConsoleLogger();

        public XModemReceiver(string port)
        {
            _port = new SerialPort(port);
        }

        public XModemReceiver(Stream stream, SerialPort port)
        {
            _stream = stream;
            _port = port;
        }

        public void Receive()
        {
            EOTReceived = new ManualResetEvent(false);
            _port.Open();
            //_port.ReadTimeout = 10000;
            _port.DataReceived += DataReceived;
            // wysłaj sygnał NAK co 10 sekund przez 1 minutę
            logger.Log("Receiving started...");
            int counter = 0;
            while (counter!=6 && !_isReceiving)
            {
                logger.Log("NAK Handshake");
                SendNAK();
                Thread.Sleep(10000); // 10 seconds interval
                counter++;
            }
            EOTReceived.WaitOne();
            logger.Log("Transmission finished");
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            logger.Log($"Received data: {_port.BytesToRead}");
            _port.Read(_buffer, 0, _buffer.Length);
            Span<byte> data = _buffer;
            switch (data[0])
            {
                case Xmodem.ControlChars.SOH:
                    logger.Log("SOH packet");
                    _isReceiving = true;
                    HandlePacket(data);
                    break;
                case Xmodem.ControlChars.EOT:
                    logger.Log("EOT packet");
                    SendACK();
                    EOTReceived.Set();
                    break;
                case Xmodem.ControlChars.C:
                    break;
            }
        }

        private void SendACK()
        {
            logger.Log("Sending ACK");
            byte[] buff =  { Xmodem.ControlChars.ACK };
            _port.Write(buff, 0, 1);
        }

        private void SendNAK()
        {
            logger.Log("Sending NAK");
            byte[] buff = { Xmodem.ControlChars.NAK };
            _port.Write(buff, 0, 1);
        }

        private void HandlePacket(ReadOnlySpan<byte> data)
        {
            byte blockNumber = data[1];
            if(blockNumber != _expectedBlockNumber)
            {
                // handle wrong block number;
                logger.Log($"Wrong block number (expected: {_expectedBlockNumber}, received: {blockNumber}");
                return;
            }
            int checksum = data[data.Length - 1];
            int calculated = Utils.CalculateChecksum(data.Slice(0, data.Length-1));
            if(checksum != calculated)
            {
                // handle checksum inequality
                logger.Log($"Wrong Checksum (expected: {checksum}, calculated: {calculated}");
                return;
            }
            _expectedBlockNumber++;
            int paddingStart = data.IndexOf(Xmodem.ControlChars.SUB);
            _stream.Write(data.Slice(3, paddingStart-3));
            logger.Log("Bytes succesfully has been written to stream");
            SendACK();
        }

        //private int CalculateChecksum(ReadOnlySpan<byte> data)
        //{

        //    int sum = 0;
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sum += data[i];
        //    }
        //    return sum % 256;
        //}
    }
}
