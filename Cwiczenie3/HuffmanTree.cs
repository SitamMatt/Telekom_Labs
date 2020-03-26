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
            var rootsList = text.ToLookup(c => c).Select(lk => new HuffmanTreeNode(lk.Key, lk.Count())).ToList();
            var nodesArray = rootsList.ToArray();
            while (rootsList.Count > 1)
            {
                var twoLessProbable = rootsList.OrderBy(root => root.Probability).Take(2).ToArray();
                rootsList.Remove(twoLessProbable[0]);
                rootsList.Remove(twoLessProbable[1]);
                var mergedRoot = new HuffmanTreeNode(twoLessProbable);
                rootsList.Add(mergedRoot);
            }
            
            HuffmanTree tree = new HuffmanTree();
            tree.root = rootsList.Last();
            
            var dictionary = new Dictionary<char, BitSequence>();
            for (int i = 0; i < nodesArray.Length; i++)
            {
                HuffmanTreeNode ptr = nodesArray[i];
                char c = ptr.Char;
                HuffmanTreeNode parent = ptr.Parent;
                dictionary[c] = new BitSequence(4);
                while (ptr.Parent != null)
                {
                    dictionary[c].Push(parent.LeftChild == ptr);
                    ptr = parent;
                    parent = ptr.Parent;
                }
                dictionary[c].Reverse();
            }

            tree.Dictionary = dictionary;
            return tree;
        }

        private HuffmanTreeNode root;
        public Dictionary<char, BitSequence> Dictionary { get; set; }
    }
}