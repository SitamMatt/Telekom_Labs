using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Text;

namespace Cwiczenie2.XModem
{
    class XModemSender
    {
        private readonly SerialPort _port;

        public XModemSender(SerialPort port)
        {
            _port = port;
        }

        public void Send(Stream stream)
        {
            _port.Open();
            byte header = 0;
            byte[] buffer = new byte[132];
            Span<byte> span = buffer;
            byte blockCounter = 1;
            // czekanie na nagłówek NAK
            while (header != Xmodem.ControlChars.NAK)
            {
                header = (byte)_port.ReadByte();
            }
            int read;
            while((read = stream.Read(span.Slice(3, 128))) != 0)
            {
                for(int i = read; i < buffer.Length - 2; i++)
                {
                    buffer[i] = Xmodem.ControlChars.SUB;
                }
                // przygotuj blok do wysłania
                buffer[131] = (byte)Utils.CalculateChecksum(span.Slice(0, span.Length - 1));
                buffer[0] = Xmodem.ControlChars.SOH;
                buffer[1] = blockCounter;
                buffer[2] = (byte)(255 - blockCounter);

                while (header == Xmodem.ControlChars.NAK)
                {
                    // wyślij blok danych
                    _port.Write(buffer, 0, buffer.Length);
                    // oczekuj odpowiedzi od obierającego
                    header = (byte)_port.ReadByte();
                    if (header == Xmodem.ControlChars.ACK)
                    {
                        break;
                    }
                }
            }

            SendEOT();

            Console.ReadLine();
        }

        private void SendEOT()
        {
            byte[] buff = { Xmodem.ControlChars.EOT };
            _port.Write(buff, 0, 1);
        }
    }
}
