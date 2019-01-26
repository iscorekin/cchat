using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestChat.Server
{
    class Program
    {
        private static Server _server;
        private static Thread _listenThread;

        static void Main(string[] args)
        {
            try
            {
                _server = new Server();
                _listenThread = new Thread(new ThreadStart(_server.Listen));
                _listenThread.Start();
            }
            catch (Exception ex)
            {
                _server.Disconnect();
                Logger.Log(ex.Message);
            }
        }
    }
}
