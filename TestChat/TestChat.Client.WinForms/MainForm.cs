using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestChat.Client.WinForms
{
    public partial class MainForm : Form
    {

        Client _client;
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AcceptButton = sendButton;
            FormDisconnected();
        }

        private void _FormConnected()
        {
            userNameTextBox.Enabled = false;
            hostTextBox.Enabled = false;
            inputTextBox.Enabled = true;
            sendButton.Enabled = true;
            connectionButton.Text = "Disconnect";
        }

        public void FormDisconnected()
        {
            userNameTextBox.Enabled = true;
            hostTextBox.Enabled = true;
            inputTextBox.Enabled = false;
            sendButton.Enabled = false;
            connectionButton.Text = "Connect";
        }

        private void connectionButton_Click(object sender, EventArgs e)
        {
            if (_client == null || !_client.Connected)
            {
                if (userNameTextBox.Text != null && hostTextBox.Text != null)
                {
                    _client = new Client(userNameTextBox.Text, hostTextBox.Text, chatTextBox, this);
                    if (_client.Connected)
                    {
                        _FormConnected();
                    }
                }
            }
            else
            {
                _client.Disconnect();
                FormDisconnected();
                _client = null;
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string message = inputTextBox.Text;
            if (message != null)
            {
                inputTextBox.Clear();
                _client.SendMessage(message);
            }
        }
    }
}
