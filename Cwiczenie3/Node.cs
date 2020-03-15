using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public class Node
    {
        public bool isLeaf { get; set; }

        public char character = (char)0;
        public Node left;
        public Node right;
        public Node parent;

        public int Value { get; set; }

        public Node(int value, Node parent = null)
        {
            isLeaf = false;
            Value = value;
            this.parent = parent;
        }

        public Node(int value, char c, Node parent = null)
        {
            isLeaf = true;
            Value = value;
            character = c;
            this.parent = parent;
        }

        public Node(Node node1, Node node2)
        {
            isLeaf = false;
            this.Value = node1.Value + node2.Value;
            this.left = node1;
            this.right = node2;
            this.right.parent = this;
            this.left.parent = this;
        }

        public override string ToString()
        {
            return character.ToString();
        }
    }
}
