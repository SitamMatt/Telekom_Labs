using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cwiczenie2.XModem;

namespace Cwiczenie2
{
    class Program
    {
        static SerialPort port;

        static void Main(string[] args)
        {
            //int a = 16;
            //byte b = 8;
            //Console.WriteLine(Convert.ToString(a, 2).PadLeft(8, '0'));
            //Console.WriteLine(Convert.ToString(b, 2).PadLeft(8, '0'));
            //Console.WriteLine(Convert.ToString(a^b, 2).PadLeft(8, '0'));
            //Console.WriteLine(Convert.ToString(0x11021, 2).PadLeft(32, '0'));
            var table = new byte[] { 5, 6, 8, 9 };
            //foreach (var item in table)
            //{
            //    Console.Write(Convert.ToString(item, 2).PadLeft(8, '0'));
            //}
            //Console.WriteLine();
            CRC16.Calculate(table);
            Console.ReadLine();
        }
    }
}
