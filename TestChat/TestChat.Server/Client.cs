using System;
using System.Net.Sockets;
using System.Text;

namespace TestChat.Server
{
    class Client
    {
        public string Id { get; private set; }
        public NetworkStream Stream { get; private set; }
        private string _userName;
        private TcpClient _tcpClient;
        private Server _server;

        public Client(TcpClient tcpClient, Server server)
        {
            Id = Guid.NewGuid().ToString();
            _tcpClient = tcpClient;
            _server = server;
            server.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = _tcpClient.GetStream();
                string message = _GetMessage();
                _userName = message;
                message = String.Format("{0} is here", _userName);
                _server.SendLast(Id);
                _server.BroadcastMessage(message, Id);

                while (true)
                {
                    try
                    {
                        message = _GetMessage();
                        message = String.Format("{0}: {1}", _userName, message);
                        _server.BroadcastMessage(message, Id);
                    }
                    catch
                    {
                        message = String.Format("{0}: disconnected", _userName);
                        _server.BroadcastMessage(message, Id);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ex.Message);
            }
            finally
            {
                _server.RemoveConnection(Id);
                Close();
            }
        }

        private string _GetMessage()
        {
            byte[] data = new byte[64];
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        public void Close()
        {   
            if (Stream != null)
                Stream.Close();
            if (_tcpClient != null)
                _tcpClient.Close();
        }
    }
}
