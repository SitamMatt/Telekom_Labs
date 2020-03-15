using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie3
{
    public static class Extensions
    {
        public static void Print(this IReadOnlyDictionary<char, int> dict)
        {
            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key} : {item.Value}");
            }
        }

        public static void Print(this IReadOnlyDictionary<char, List<int>> dict)
        {
            foreach (var item in dict)
            {
                string s = "";
                foreach (var f in item.Value)
                {
                    s += f;
                }
                Console.WriteLine($"{item.Key} : {s}");
            }
        }

        public static void Print(this Node node)
        {
            Stack<Node> stack = new Stack<Node>();
            Node p = node;
            while (true)
            {
                
            }
        }

        public static byte ToByte(this List<int> list)
        {
            string s = "";
            foreach (var f in list)
            {
                s += f;
            }
            return Convert.ToByte(s, 2);
        }

        //private static int getLevel(Node p)
        //{

        //}
    }
}
