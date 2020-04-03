using System;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace Telekom.XModem.Internal
{
    public class XModemSender
    {
        private SerialPort serialPort;
        private Stream stream;
        private int blockCounter = 1;
        private int attemptsLimit = 10;
        private bool crc = false;


        public XModemSender(SerialPort serialPort, FileStream fs, bool crc)
        {
            this.serialPort = serialPort;
            stream = fs;
            this.crc = crc;
        }

        public void Send(in CancellationToken token)
        {
            
            serialPort.DiscardInBuffer(); // czysczenie buforów wejścia i wyjścia
            serialPort.DiscardOutBuffer();
            
            token.ThrowIfCancellationRequested();
            
            Span<byte> span = stackalloc byte[133]; // alokacja na stosie bufora 
            WaitForReceiverSignal(token); // oczekiwanie na nagłówek NAK
            int bytesRead = 0;
            // dopóki wczytywane będą pełne boki danych ( 128 bajtów ). Ostatni pakiet jest zapełniany marginesem poza pętlą
            while ((bytesRead = PreparePacket(span)) == 128)  // dane są wczytywane i opakowywane w pakiet
            {
                SendPacket(crc ? span : span.Slice(0, 132), token); // wysyłanie każdego pakietu
            }
            
            // wysyłanie ostatniego pakietu
            SlicePacket(span, bytesRead); // zastąpienie wolnej przestrzeni danych pakietu znakami SUB
            SendPacket(crc ? span : span.Slice(0, 132), token);
            SendPacket(stackalloc byte[]{ControlChars.EOT}, token); // wysłanie nagłówka EOT - koniec przesyłania
        }

        private void SlicePacket(in Span<byte> span, int bytesRead)
        {
            int start = 3 + bytesRead; // margines zaczyna się od ostatniego bajtu danych ( + 3 bajty nagłówka )
            int length = 128 - bytesRead; // długośc marginesu
            span.Slice(start, length).Fill(ControlChars.SUB); // zapełnienie marginesu znakiem SUB
            // ponowne przeliczenie sumy kontrolnej
            if (crc)
            {
                var checksum = Utils.CalculateCRC(span.Slice(0, 131));
                Span<byte> buff = stackalloc byte[2];
                buff[0] = (byte) checksum;
                buff[1] = (byte) (checksum >> 8);
                buff.CopyTo(span.Slice(131,2));
            }
            else
            {
                int checksum = Utils.CalculateChecksum(span.Slice(0, 131)); // obliczenie sumy kontrolnej
                span[131] = (byte)checksum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <exception cref="TimeoutException"></exception>
        private void WaitForReceiverSignal(CancellationToken token)
        {
            var buffer = new byte[1];
            CancellationTokenSource cts = new CancellationTokenSource();
            // Token zewnętrzny (od użytkownika ) i wewnętrzny zostają połączone aby każdy z nich mógł zakończyć zadanie
            using CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, token); 
            var task = Task.Run(async () => await WaitForNak(buffer, linkedCts.Token));
            if (task.Wait(TimeSpan.FromSeconds(60)))
                if (task.Result)
                {
                    return; // zadanie zakończyło się przed czasem. Sukces
                }
                else
                {
                    linkedCts.Cancel();
                    throw new TimeoutException(); // Czas minął
                }

            throw new TimeoutException(); // Czas minął
        }
        
        /// <summary>
        /// Oczekuje wiadomości zawierającej znak NAK lub przerywa działanie jeżeli operacja została zakończona (za pomocą tokenu)
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<bool> WaitForNak(byte[] buffer, CancellationToken token)
        {
            byte header = 0;
            while (header != ControlChars.NAK)
            {
                await serialPort.BaseStream.ReadAsync(buffer, token);
                header = buffer[0];
            }
            return header == ControlChars.NAK;
        }

        private void SendPacket(in Span<byte> packet, CancellationToken token)
        {
            serialPort.ReadTimeout = 10000; // czas oczekiwania na operacje odczytu bufora to 10 sekund;
            byte header = 0;
            for (int attempts = 0; attempts < attemptsLimit; attempts++) // wykonaj maksymalną ilość prób wysłania pakietu
            {
                token.ThrowIfCancellationRequested();
                try
                {
                    Console.WriteLine("Sending Packet");
                    serialPort.BaseStream.Write(packet); // wysłanie pakietu
                    header = (byte) serialPort.BaseStream.ReadByte(); // czekanie na ACK
                    Console.WriteLine($"Received {header}");
                    switch (header)
                    {
                        case ControlChars.ACK: // ACK oznacza że pakiet dotarł i jest poprawny
                            return;
                        case ControlChars.NAK: // Nieprawidłowy pakiet lub czas minął. Przechodzi do następnej próby
                            throw new InvalidPacketException();
                    }
                }
                catch (TimeoutException e) // Czas na odczyt minął
                {
                    Console.WriteLine(e.GetType().ToString());
                }
                catch (InvalidPacketException e) // Pakiet jest nieprawidłowy
                {
                    Console.WriteLine(e.GetType().ToString());
                }
            }
            
            token.ThrowIfCancellationRequested();
            
            // W tym bloku wykonywane jest ostatnie podejście.
            // Niepowodzenie w tym przypadku powoduje rzucenie wyjątku informującego o przekroczeniu liczby prób
            try
            {
                // ostatnie podejście
                Console.WriteLine($"Sending Packet {blockCounter - 1}");
                serialPort.BaseStream.Write(packet);
                header = (byte) serialPort.BaseStream.ReadByte(); // czekanie na ACK
                Console.WriteLine($"Received {header}");
                switch (header)
                {
                    case ControlChars.ACK:
                        return;
                    case ControlChars.NAK:
                        throw new InvalidPacketException();
                }
            }
            catch (Exception e) when (e is TimeoutException || e is InvalidPacketException)
            {
                throw new ExceededAttemptsException();
            }
        }

        private int PreparePacket(in Span<byte> span)
        {
            int bytesRead = stream.Read(span.Slice(3, 128)); // odczyt z pliku
            span[0] = crc ? ControlChars.C : ControlChars.SOH; // ustawienie flagi
            span[1] = (byte)blockCounter; // ustawienie numeru bloku
            span[2] = (byte) (Byte.MaxValue - blockCounter);
            blockCounter++;

            if (crc)
            {
                // obliczenie sumy kontrolnej
                var checksum = Utils.CalculateCRC(span.Slice(0, 131));
                Span<byte> buff = stackalloc byte[2];
                buff[0] = (byte) checksum;
                buff[1] = (byte) (checksum >> 8);
                buff.CopyTo(span.Slice(131,2));
            }
            else
            {
                int checksum = Utils.CalculateChecksum(span.Slice(0, 131)); // obliczenie sumy kontrolnej
                span[131] = (byte)checksum;
            }
            return bytesRead;
        }
        
        
    }
}