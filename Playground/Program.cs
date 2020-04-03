using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            byte res = 0;
            for (int j = 7; j >= 0; j--)
            {
                res |= (byte) (1 << j);
            }
            Console.WriteLine(res);
        }


    }
}
