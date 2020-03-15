using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenie2
{
    public static class Extensions
    {
        public static void Print(this string[] strings)
        {
            foreach (var str in strings)
            {
                Console.WriteLine(str);
            }
        }
    }
}
