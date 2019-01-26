using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TestChat.Client.WinForms
{
    class Client
    {
        private string _userName;
        private string _address;
        private int _port;
        public bool Connected;
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private TextBox _chatBox;
        private MainForm _form;
        private Thread _receiveThread;

        public Client(string userName, string host, TextBox chatBox, MainForm form)
        {
            _userName = userName;
            _address = host.Split(':')[0];
            _port = int.Parse(host.Split(':')[1]);
            _chatBox = chatBox;
            _form = form;
            _Process();

        }

        private void _Process()
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(_address, _port);
                _stream = _tcpClient.GetStream();

                string message = _userName;
                SendMessage(message);

                _receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                _receiveThread.Start();
                Connected = true;
            }
            catch (Exception ex)
            {
                _chatBox.AppendText(ex.Message + Environment.NewLine);
            }
        }

        private void ReceiveMessage()
        {
            while (true)
            {
                byte[] data = new byte[64];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = _stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (_stream.DataAvailable);

                string message = builder.ToString();
                _chatBox.Invoke((MethodInvoker)(() => _chatBox.AppendText(message + Environment.NewLine)));
            }
            
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        public void Disconnect()
        {
            if (_receiveThread != null)
            {
                _receiveThread.Abort();
                _receiveThread.Join(500);
            }
            if (_stream != null)
                _stream.Close();
            if (_tcpClient != null)
                _tcpClient.Close();
            _form.FormDisconnected();
        }

    }
}
