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
            // podejmuje próbę odebrania pakietów
            // jeżeli minie czas wykounje kolejne podejście aż do osiągnięcia limitu
            // wtedy odbieranie zakończy się niepowodzeniem
            for (int attempts = 0; attempts < attemptsLimit; attempts++)
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    return ReadPacket(span);
                }
                catch (Exception e) when (e is TimeoutException || e is InvalidPacketException)
                {
                    // próba zakończona niepowodzeniem
                    Console.WriteLine(e.GetType().ToString());
                    SendFlag(ControlChars.NAK); // prośba o ponowne wysłanie pakietu
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
                throw new ExceededAttemptsException(); // rzucenie wyjątku o niepowodzeniu operacji
            }
        }

        private byte ReadPacket(Span<byte> span)
        {
            // odczytanie nagłówka pakietu
            serialPort.BaseStream.Read(span.Slice(0, 1));
            switch (span[0]) // sprawdzenie czy nagłówek jest prawidłowy
            {
                // jezeli jest prawidłowy to dalsza część pakietu zostanie obsłuzona przez odpowiednią funkcje
                case ControlChars.SOH: // pakiet 132 bity
                    serialPort.BaseStream.Read(span.Slice(1, 131));
                    ValidatePacket(span);
                    Console.WriteLine("Valid Packet Received");
                    SendFlag(ControlChars.ACK);  // powiadomieniu wysyłającego o poprawnym odbiorze paketu
                    return ControlChars.SOH;
                case ControlChars.C: // pakiet z sumą kontrolną CRC 133 bity
                    serialPort.BaseStream.Read(span.Slice(1, 132));
                    ValidatePacket(span);
                    Console.WriteLine("Valid CRC Packet Received");
                    SendFlag(ControlChars.ACK);  // powiadomieniu wysyłającego o poprawnym odbiorze paketu
                    return ControlChars.C;
                case ControlChars.EOT: // pakiet z nagłówiek EOT koniec emisji
                    Console.WriteLine("Received EOT packet. Ending...");
                    SendFlag(ControlChars.ACK); // powiadomieniu wysyłającego o poprawnym odbiorze paketu
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
            serialPort.DiscardInBuffer(); // oczyszczenie bufora z brudnych danych
            SendFlag(ControlChars.NAK); // komunikat do wysyłajacego aby rozpocząć transmisje
            Console.WriteLine("Sending NAK due to initialization");

            // odbiera kolejne pakiety aż do otrzymania nagłówka EOT
            while (ReceivePacket(span, token) != ControlChars.EOT) 
            {
                token.ThrowIfCancellationRequested();
                Span<byte> data = SlicePadding(span.Slice(3, 128)); // wyodrębnienie danych z pakietu
                stream.Write(data); // zapis do strumienia
            }
        }

        private Span<byte> SlicePadding(Span<byte> packet)
        {
            int paddingStart = packet.IndexOf(ControlChars.SUB); // wykrycie pierwszego wystąpienia znaku SUB
            // oznacza on margines aby zapełnić pakiet do 128 bajtów danych 
            // podczas odbioru należy go usunąć z danych
            if (paddingStart == -1)
            {
                paddingStart = 128;
            }
            
            var data =  packet.Slice(0, paddingStart);
            return data; // zwraca prawidłowe dane
        }

        private void SendFlag(in byte flag)
        {
            serialPort.BaseStream.WriteByte(flag);
        }

        private bool ValidatePacket(Span<byte> packet)
        {
            byte header = packet[0]; // nagłówek pakietu
            
            if (header != ControlChars.SOH && header != ControlChars.C) 
                throw new InvalidPacketException(); // nagłówek nie jest C lub SOH (nie jest pakietem danych)
            
            byte blockNumber = packet[1]; // numer bloku
            
            if(blockNumber != blockCounter) // sprawdzenie czy numer pakietu jest równy wyczekiwanemu
                throw new InvalidPacketException();

            if (header == ControlChars.SOH) // walidacja sumy kontrolnej pakietu SOH
            {
                int checksum = packet[132 - 1];
                int calculated = Utils.CalculateChecksum(packet.Slice(0, 132-1));
            
                if(checksum != calculated)
                    throw new InvalidPacketException();
            }else if (header == ControlChars.C) // walidacja sumy kontrolnej pakietu C
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
            blockCounter++; // pakiet OK, inkrementacja numeru bloku
            return true;
        }

    }
}
