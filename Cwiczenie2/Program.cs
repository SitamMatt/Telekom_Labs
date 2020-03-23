using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Telekom.XModem;

namespace Cwiczenie2
{
    class Program
    {
        //static SerialPort port;

        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Task.Run(Run).Wait();
            stopwatch.Stop();
            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        }

        static CancellationTokenSource cts = new CancellationTokenSource();

        static void Run()
        {
            using(XModemClient client = new XModemClient("COM1"))
            {
                try
                {
                    client.Send(@"E:\filename.txt", cts.Token, true);
                    //client.Receive(@"E:/filename.txt", cts.Token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
