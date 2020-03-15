using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cwiczenie3
{
    class HuffmanCoding
    {

        /// <summary>
        /// Oblicza prawdopodobieństwo wystąpienia danego znaku
        /// </summary>
        /// <param name="text"></param>
        /// <returns>
        /// Zwraca słownik gdzie kluczem jest znak, natomiast wartością jest prawdopodobieństwo
        /// </returns>
        public static IReadOnlyDictionary<char, int> CalculateCharacterFrequences(ReadOnlySpan<char> text)
        {
            var dict = new Dictionary<char, int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (dict.ContainsKey(text[i]))
                {
                    dict[text[i]]++;
                }
                else
                {
                    dict.Add(text[i], 1);
                }
            }
            return dict;
        }

        public static Node BuildHuffmanTree(IReadOnlyDictionary<char, int> dict)
        {
            List<Node> minheap = new List<Node>();
            foreach (var item in dict)
            {
                minheap.Add(new Node(item.Value, item.Key));
            }

            while(minheap.Count != 1)
            {
                minheap = minheap.OrderBy(n => n.Value).ToList();
                var twomin = minheap.TakeLast(2).ToArray();
                minheap.Remove(twomin[0]);
                minheap.Remove(twomin[1]);
                minheap.Add(new Node(twomin[0], twomin[1]));
            }
            return minheap.Last();
        }

        public static IReadOnlyDictionary<char, List<int>> GetHuffmanCodes(Node tree)
        {
            var dict = new Dictionary<char, List<int>>();
            Stack<Node> stack = new Stack<Node>();
            if (tree.isLeaf)
            {
                return null;
            }
            Node actual = tree;
            while (!(actual.isLeaf && stack.Count == 0))
            {
                if (actual.isLeaf)
                {
                    dict[actual.character] = GetCode(actual);
                    //Console.WriteLine(actual.character);
                    actual = stack.Pop();
                }else
                {
                    stack.Push(actual.right);
                    actual = actual.left;
                }
            }
            dict[actual.character] = GetCode(actual);
            return dict;
        }

        private static List<int> GetCode(Node node)
        {
            var res = new List<int>();
            Node p = node.parent;
            while (p != null)
            {
                if(p.left == node)
                {
                    res.Add(0);
                }
                else
                {
                    res.Add(1);
                }
                p = p.parent;
            }
            res.Reverse();
            return res;
        }
    }
}
