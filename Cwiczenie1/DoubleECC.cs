using System;
using System.IO;
using Cwiczenie1;
using Telekom.Bits;

namespace Telekom.Encoding
{
    public static class DoubleECC
    {
        /// <summary>
        /// Koduje wiadomość do słowa kodowego
        /// </summary>
        /// <param name="input">wiadomość</param>
        /// <returns>tablica zawierająca słowa kodowe</returns>
        public static byte[] Encode(Stream input)
        {
            byte[] result = new byte[2 * input.Length]; // wyjściowe dane będą dwa razy większe
            using MemoryStream ms = new MemoryStream(result);
            for (int i = 0; i < input.Length; i++) // kodowanie każdego 8-bitowego segmentu
            {
                byte b = (byte)input.ReadByte();
                var vector = b.ToBitVector();  // interpretacja bajta jako wektora bitów 
                var encoded = G * vector; // kodowanie za pomocą mnożenie wektora wiadomości przez macierz generującą G
                ms.Write(encoded.Bytes); // zapis słowa kodowego do strumienia
            }

            return result; // słowo kodowe
        }
        /// <summary>
        /// Odkodowywuje słowa kodowe
        /// </summary>
        /// <param name="input">słowa kodowe</param>
        /// <returns>tablica zawierające odkodowaną wiadomość</returns>
        /// <exception cref="Exception"></exception>
        public static byte[] Decode(Stream input)
        {
            byte[] result = new byte[input.Length / 2]; // wiadomość będzie 2 razy mniejsza od słowa kodowego
            using MemoryStream ms = new MemoryStream(result);
            Span<byte> buff = stackalloc byte[2];
            for (int i = 0; i < input.Length/2; i++) // pętla przez każde 16 bitów
            {
                input.Read(buff);
                BitVector vector = new BitVector(buff); // interpretacja 16 bitów jako wektor
                var column = H * vector; // mnożenie wektora (słowa kodowego) przez macierz parzystoci H
                if (!column.IsZero()) // sprawdzenie czy otrzymany wektor jest zerowy
                {
                    // wektor nie jest zerowy wiec nastąpił błąd
                    if (H.HasColumn(column, out int number)) // poszukiwanie numeru kolumny macierzy H która jest równa wektorowi
                    {
                        vector.SwitchBit(number); // przełączenie bitu na znalezionej pozycji
                    }
                    // jeżeli wektor nie pasuje do żadnej kolumny macierzy H to znajdź indeksy dwóch kolumn których suma daje wskazany wektor
                    else if (H.HasColumnSum(column, out int number1, out int number2))
                    {
                        // przełączenie bitów na znalezionej pozycji
                        vector.SwitchBit(number1);
                        vector.SwitchBit(number2);
                    }
                    else
                    {
                        throw new Exception("Two many errors detected"); // wyjątek operacja nie powiodła się
                    }
                }
                ms.WriteByte(buff[0]); // zapis wiadomości do strumienia (pierwsze 8 bitów słowa kodowego)
            }

            return result;
        }
        
        // macierz P (służy do budowania macierzy G i H)
        private static BitMatrix P = new BitMatrix(new int[,]
        {
            {0, 1, 1, 1, 1, 1, 1, 1},	
            {1, 0, 1, 1, 1, 1, 1, 1},	
            {1, 1, 0, 1, 1, 1, 1, 1},	
            {1, 1, 1, 0, 1, 1, 1, 1},	
            {1, 1, 1, 1, 0, 1, 1, 1},	
            {1, 1, 1, 1, 1, 0, 1, 1},	
            {1, 1, 1, 1, 1, 1, 0, 1},	
            {1, 1, 1, 1, 1, 1, 1, 0}
        });

        // macierz generująca G służąca do zakodowania wiadomości
        private static BitMatrix G = BitMatrix.Identity(8).AppendHorizontally(P).Transpose();
        // macierz parzystości H służąca do detekcji i korekcji błędów słowa kodowego
        private static BitMatrix H = P.Transpose().AppendHorizontally(BitMatrix.Identity(8));

        
        /// <summary>
        /// Działa tak samo jak Encode z tą różnicą że zapis nastepuje do strumienia
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void Encode(Stream input, Stream output)
        {
            for (int i = 0; i < input.Length; i++)
            {
                byte b = (byte) input.ReadByte();
                var vector = b.ToBitVector();
                var encoded = G * vector;
                output.Write(encoded.Bytes);
            }
        }

        /// <summary>
        /// Działa tak samo jak Decode z tą różnicą że zapis następuje do strumienia
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <exception cref="Exception"></exception>
        public static void Decode(Stream input, Stream output)
        {
            Span<byte> buff = stackalloc byte[2];
            for (int i = 0; i < input.Length / 2; i++)
            {
                input.Read(buff);
                BitVector vector = new BitVector(buff);
                var column = H * vector;
                if (!column.IsZero())
                {
                    if (H.HasColumn(column, out int number))
                    {
                        vector.SwitchBit(number);
                    }
                    else if (H.HasColumnSum(column, out int number1, out int number2))
                    {
                        vector.SwitchBit(number1);
                        vector.SwitchBit(number2);
                    }
                    else
                    {
                        throw new Exception("Two many errors detected");
                    }
                }

                output.WriteByte(buff[0]);
            }
        }
    }
}