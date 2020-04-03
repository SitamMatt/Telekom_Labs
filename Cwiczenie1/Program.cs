using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Telekom;
using Telekom.Bits;
using Telekom.Encoding;

namespace Cwiczenie1
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "1101000110"; // wiadomość do zakodowania (10 bitów)
            var bytes = BitsUtils.ToBinary(str); // konwersja do postaci bitowej
            using MemoryStream ms = new MemoryStream(bytes); 
            var encoded = Telekom.ECC.Encode(1, ms); // Zakodowanie wiadomości do słowa kodowego
            BitVector vector = new BitVector(encoded); 
            vector.SwitchBit(3); // zamiana 3 bitu (pierwsze słowo kodowe)
            vector.SwitchBit(12); // zamiana 12 bitu (drugie słowo kodowe)
            using MemoryStream ms1 = new MemoryStream(encoded);
            var docoded = SingleECC.Decode(ms1); // Odkodowanie słowa kodowego (i korekcja błędów)
        }
    }
}