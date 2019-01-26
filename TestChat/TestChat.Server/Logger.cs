using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestChat.Server
{
    class Logger
    {
        public static void Log(string log)
        {
            string time = DateTime.Now.ToLongTimeString();
            Console.WriteLine("[{0}] {1}", time, log);
        }
    }
}
