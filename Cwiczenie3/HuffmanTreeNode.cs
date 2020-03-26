using System.Collections.Generic;

namespace Telekom.Huffman
{
    internal class HuffmanTreeNode
    {
        public HuffmanTreeNode Parent { get; set; }
        public HuffmanTreeNode LeftChild { get; set; }
        public HuffmanTreeNode RightChild { get; set; }
        public bool IsLeaf => LeftChild == null && RightChild == null;
        
        public char Char { get; private set; }
        public int Probability { get; private set; }

        public HuffmanTreeNode(char c, int probability)
        {
            Char = c;
            Probability = probability;
        }

        public override string ToString()
        {
            return IsLeaf ? $"\'{Char}\' : {Probability}" : Probability.ToString();
        }

        public HuffmanTreeNode(HuffmanTreeNode leftChild, HuffmanTreeNode rightChild)
        {
            LeftChild = leftChild;
            RightChild = rightChild;
            Probability = LeftChild.Probability + RightChild.Probability;
            LeftChild.Parent = this;
            RightChild.Parent = this;
        }

        public HuffmanTreeNode(HuffmanTreeNode[] children) : this(children[0], children[1]) {}
    }
}