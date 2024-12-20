using PCClonerPrototype;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Receiver;
using System.Net;

namespace PCClonerPrototype.Forms
{
    public partial class ReceiverForm : Form
    {
        private static MainPage _mainPage = new();
        private static ReceiverForm2 _receiverForm2 = new();
        public static string? connectionCode;
        private static bool _UDPResult = false;
        private static IPAddress? senderIP;

        public ReceiverForm()
        {
            InitializeComponent();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            connectionCode = txtConnectionCode.Text;
            _ = StartSenderUDP(connectionCode);
        }
        private static async Task StartSenderUDP(string connectionCode)
        {
            _UDPResult = false;

            var udpResult = await ReceiverUDP.SendUDPBroadcastAsync(connectionCode);
            
            _UDPResult = udpResult.Success;
            senderIP = udpResult.SenderIPAddress;

            if(_UDPResult == true)
            {
                _receiverForm2.Dock = DockStyle.Fill;
                _receiverForm2.TopLevel = false;
                MainForm.MainPanel.Controls.Clear();
                MainForm.MainPanel.Controls.Add(_receiverForm2);
                _receiverForm2.Show();

                _ = ReceiverForm2.StartReceiverHTTP(senderIP);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            txtConnectionCode.Text = "";
            _mainPage.Dock = DockStyle.Fill;
            _mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_mainPage);
            _mainPage.Show();
        }

    }
}
