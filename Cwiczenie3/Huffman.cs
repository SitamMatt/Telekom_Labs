using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Telekom.BitCollections;
using Telekom.Huffman;

namespace Telekom.Encoding
{
    public static class Huffman
    {
        public static byte[] Encode(string text)
        {
            HuffmanTree tree = HuffmanTree.Build(text); // budowa drzewa
            var dict = tree.Dictionary; // słownik kodów
            var (encoded, padding) = Encode(text, dict); // zakodowanie wiadomośc
            int maxLen = 4;
            int estimatedBufferSize = 1 + dict.Count * (1 + 1 + maxLen) + 1 + encoded.Length; // przewidywany rozmiar pliku wyjściowego
            var buff = new byte[estimatedBufferSize]; // utworzenie bufora
            using var ms = new MemoryStream(buff);
            // zapis kolejnych składowych formatu
            ms.WriteByte((byte) dict.Count); // liczba znaków
            foreach (var kvp in dict)
            {
                ms.WriteByte((byte) kvp.Key); // znak
                ms.WriteByte((byte) kvp.Value.Length); // długośc kodu
                ms.Write(kvp.Value.GetBytes().Item1); // kod
            }

            ms.WriteByte((byte) padding); // przesuniecie (margines do 8 bitów)
            ms.Write(encoded); // zapis danych
            return buff.AsSpan(0, (int) ms.Position).ToArray();
        }

        public static string Decode(byte[] data)
        {
            using var ms = new MemoryStream(data);
            int dictLen = ms.ReadByte(); // ilość znaków
            var dict = new Dictionary<BitSequence, char>();
            // odczytanie słownika
            for (int i = 0; i < dictLen; i++)
            {
                char c = (char) ms.ReadByte(); // znak
                int seqLen = ms.ReadByte(); // długośc znaku
                BitSequence seq = new BitSequence(seqLen); // kod
                ms.Read(seq.rawData, 0, seq.ByteCapacity);
                seq.PushLength(seqLen); // odczytanie kodu
                dict[seq] = c; // dodanie do słownika
            }

            int padding = ms.ReadByte(); // margines

            int encodedLen = ms.Capacity - (int) ms.Position; 
            byte[] encoded = new byte[encodedLen];
            ms.Read(encoded, 0, encodedLen);
            string decoded = Decode(encoded, padding, dict); // odkowowanie 
            return decoded;
        }

        private static string Decode(byte[] data, int padding, Dictionary<BitSequence, char> dictionary)
        {
            int len = data.Length * 8 - padding;
            StringBuilder sb = new StringBuilder();
            BitSequence encoded = new BitSequence(data, len);
            BitSequence temp = new BitSequence(8);
            for (int i = 0; i < len; i++) // pętla przez każdy bit danych
            {
                temp.Push(encoded[i]); // dodanie bitu do stosu
                if (dictionary.ContainsKey(temp)) // jezeli stos w obecnej postaci odpowiada jakiemuś kodowi
                {
                    // to dodaj odpowiedni znak do wyniku
                    sb.Append(dictionary[temp]);
                    temp = new BitSequence(16); // wyzeruj stos
                }
            }

            return sb.ToString();
        }

        private static (byte[] encoded, int padding) Encode(string text, Dictionary<char, BitSequence> dictionary)
        {
            BitSequence sequence = new BitSequence(8*text.Length);
            foreach (char c in text) // pętla przez każdy znak tekstu
            {
                sequence.Push(dictionary[c]); // dodaje do sekwencji bitów kod odpowiadajacy znakowi
            }
            return sequence.GetBytes();
        }
    }
}