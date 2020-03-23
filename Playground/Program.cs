using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(Run).Wait();
        }

        static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task Run()
        {
            using var fs = new FileStream(@"E:/filename.txt", FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[1024];
            await fs.ReadAsync(buffer, cts.Token);
        }
    }
}
