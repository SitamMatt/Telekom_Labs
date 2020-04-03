using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telekom.BitCollections;

namespace Telekom.Huffman
{
    public class HuffmanTree
    {
        public static HuffmanTree Build(string text)
        {
            // grupowanie znaków w tekscie i policzenie ich liczebności
            // nastepnie słownik jest konwertowany w listę drzew o jednym węźle
            var rootsList = text.ToLookup(c => c).Select(lk => new HuffmanTreeNode(lk.Key, lk.Count())).ToList();
            var nodesArray = rootsList.ToArray();

            while (rootsList.Count > 1) // dopóki na liście jest więcej niż jedeń korzeń
            {
                // znajduje dwa korzenie z najmniejszym prawdopodobieństwem
                var twoLessProbable = rootsList.OrderBy(root => root.Probability).Take(2).ToArray();
                // usunięcie węzłów z listy
                rootsList.Remove(twoLessProbable[0]);
                rootsList.Remove(twoLessProbable[1]);
                // połączenie węzłów w nowe drzewo
                var mergedRoot = new HuffmanTreeNode(twoLessProbable);
                // dodanie drzewa do listy
                rootsList.Add(mergedRoot);
            }
            
            HuffmanTree tree = new HuffmanTree();
            tree.root = rootsList.Last();
            
            // słownik typu znak - kod
            var dictionary = new Dictionary<char, BitSequence>();
            for (int i = 0; i < nodesArray.Length; i++) // każdy węzeł który zawierał w sobie znak
            {
                HuffmanTreeNode ptr = nodesArray[i];
                char c = ptr.Char; // znak węzła
                HuffmanTreeNode parent = ptr.Parent;
                dictionary[c] = new BitSequence(32);
                // podąża od węzła w góry aż do korzenie drzewa
                while (ptr.Parent != null)
                {
                    dictionary[c].Push(parent.LeftChild == ptr); // jeżeli węzeł jest lewym dzieckiem to 1 inaczej 0
                    ptr = parent;
                    parent = ptr.Parent;
                }
                dictionary[c].Reverse(); // odwórcenie kodu bo wędrowaliśmy od końca
            }

            tree.Dictionary = dictionary;
            return tree;
        }

        private HuffmanTreeNode root;
        public Dictionary<char, BitSequence> Dictionary { get; set; }
    }
}