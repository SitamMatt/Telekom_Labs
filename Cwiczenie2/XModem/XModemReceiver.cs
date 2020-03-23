using System;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace Telekom.XModem.Internal
{
    class XModemReceiver
    {
        private SerialPort serialPort;
        private FileStream stream;
        private int attemptsLimit = 10;
        private int blockCounter = 1;

        public XModemReceiver(SerialPort serialPort, FileStream fs)
        {
            this.serialPort = serialPort;
            stream = fs;
        }

        private byte ReceivePacket(Span<byte> span, CancellationToken token)
        {
            for (int attempts = 0; attempts < attemptsLimit; attempts++)
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    return ReadPacket(span);
                }
                catch (Exception e) when (e is TimeoutException || e is InvalidPacketException)
                {
                    Console.WriteLine(e.GetType().ToString());
                    SendFlag(ControlChars.NAK);
                    Console.WriteLine($"Sending NAK due to {e.GetType()}");
                }
            }

            token.ThrowIfCancellationRequested();
            try
            {
                return ReadPacket(span); // ostatnie podejście
            }
            catch (Exception e) when (e is TimeoutException || e is InvalidPacketException)
            {
                throw new ExceededAttemptsException();
            }
        }

        private byte ReadPacket(Span<byte> span)
        {
            serialPort.BaseStream.Read(span.Slice(0, 1));
            switch (span[0])
            {
                case ControlChars.SOH:
                    serialPort.BaseStream.Read(span.Slice(1, 131));
                    ValidatePacket(span);
                    Console.WriteLine("Valid Packet Received");
                    SendFlag(ControlChars.ACK);
                    return ControlChars.SOH;
                case ControlChars.C:
                    serialPort.BaseStream.Read(span.Slice(1, 132));
                    ValidatePacket(span);
                    Console.WriteLine("Valid CRC Packet Received");
                    SendFlag(ControlChars.ACK);
                    return ControlChars.C;
                case ControlChars.EOT:
                    Console.WriteLine("Received EOT packet. Ending...");
                    SendFlag(ControlChars.ACK);
                    return ControlChars.EOT;
                default:
                    throw new InvalidPacketException();
            }
        }

        public void Receive(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            Span<byte> span = stackalloc byte[133];
            serialPort.ReadTimeout = 10 * 1000;
            serialPort.DiscardInBuffer();
            SendFlag(ControlChars.NAK);
            Console.WriteLine("Sending NAK due to initialization");

            while (ReceivePacket(span, token) != ControlChars.EOT)
            {
                token.ThrowIfCancellationRequested();
                Span<byte> data = SlicePadding(span.Slice(3, 128));
                stream.Write(data);
            }
        }

        private Span<byte> SlicePadding(Span<byte> packet)
        {
            int paddingStart = packet.IndexOf(ControlChars.SUB);
            if (paddingStart == -1)
            {
                paddingStart = 128;
            }
            
            var data =  packet.Slice(0, paddingStart);
            return data;
        }

        private void SendFlag(in byte flag)
        {
            serialPort.BaseStream.WriteByte(flag);
        }

        private bool ValidatePacket(Span<byte> packet)
        {
            byte header = packet[0];
            
            if (header != ControlChars.SOH && header != ControlChars.C)
                throw new InvalidPacketException();
            
            byte blockNumber = packet[1];
            
            if(blockNumber != blockCounter)
                throw new InvalidPacketException();

            if (header == ControlChars.SOH)
            {
                int checksum = packet[132 - 1];
                int calculated = Utils.CalculateChecksum(packet.Slice(0, 132-1));
            
                if(checksum != calculated)
                    throw new InvalidPacketException();
            }else if (header == ControlChars.C)
            {
                var checksum = packet.Slice(131, 2);
                var calculated = Utils.CalculateCRC(packet.Slice(0, 131));
                Span<byte> buff = stackalloc byte[2];
                buff[0] = (byte) calculated;
                buff[1] = (byte) (calculated >> 8);
                
                if (checksum.SequenceEqual(buff))
                {
                    Console.WriteLine("Packet OK");
                }
                else
                {
                    throw new InvalidPacketException();
                }
                    
            }
            blockCounter++;
            return true;
        }

    }
}
