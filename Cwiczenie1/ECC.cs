using System;
using System.IO;
using Cwiczenie1;
using Telekom.Encoding;

namespace Telekom
{
    public static class ECC
    {
        /// <summary>
        /// Zakodowywuje wiadomość do słowa kodowego
        /// </summary>
        /// <param name="errorsCorrection">liczba błędów mozliwych do skorygowania</param>
        /// <param name="input">wiadomość</param>
        /// <returns>tablicę bajtów zawierającą słowa kodowe</returns>
        public static byte[] Encode(int errorsCorrection, Stream input)
        {
            return errorsCorrection switch
            {
                1 => SingleECC.Encode(input), // korekcja 1 błędu
                2 => DoubleECC.Encode(input), // korekcja 2 błędów
                _ => throw new Exception("Unsupported errors correction mode") // korekcje większej ilości błędów nie obsługujemy
            };
        }
        /// <summary>
        /// Zakodowywuje wiadomość do słowa kodowego i zapisuje w strumieniu
        /// </summary>
        /// <param name="errorsCorrection">liczba błędów mozliwych do skorygowania</param>
        /// <param name="input">wiadomość</param>
        /// <param name="output">strumień wyjściowy</param>
        /// <exception cref="Exception"></exception>
        public static void Encode(int errorsCorrection, Stream input, Stream output)
        {
            switch (errorsCorrection)
            {
                case 1:
                    SingleECC.Encode(input, output); // korekcja 1 błędu
                    break;
                case 2:
                    DoubleECC.Encode(input, output); // korekcja 2 błędów
                    break;
                default:
                    throw new Exception("Unsupported errors correction mode"); // korekcje większej ilości błędów nie obsługujemy
            }
        }
        /// <summary>
        /// Odkowowywuje słowo kodowe
        /// </summary>
        /// <param name="errorsCorrection">algorytm którym zakodowano słowo kodowe</param>
        /// <param name="input">słowo kodowe</param>
        /// <returns>tablica bajtów odkodowanej wiadomości</returns>
        public static byte[] Decode(int errorsCorrection, Stream input)
        {
            return errorsCorrection switch
            {
                1 => SingleECC.Decode(input),
                2 => DoubleECC.Decode(input),
                _ => throw new Exception("Unsupported errors correction mode")
            };
        }
        /// <summary>
        /// Odkowowywuje słowo kodowe
        /// </summary>
        /// <param name="errorsCorrection">algorytm którym zakodowano słowo kodowe</param>
        /// <param name="input">słowo kodowe</param>
        /// <param name="output">strumień do którego zostanie zapisana odkodowana wiadomość</param>
        /// <exception cref="Exception"></exception>
        public static void Decode(int errorsCorrection, Stream input, Stream output)
        {
            switch (errorsCorrection)
            {
                case 1:
                    SingleECC.Decode(input, output);
                    break;
                case 2:
                    DoubleECC.Decode(input, output);
                    break;
                default:
                    throw new Exception("Unsupported errors correction mode");
            }
        }
    }
}