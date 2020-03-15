using System;
using System.Collections.Generic;
using System.Text;

namespace Cwiczenie2
{
    public class ConsoleLogger : AbstractLogger
    {
        public override void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
