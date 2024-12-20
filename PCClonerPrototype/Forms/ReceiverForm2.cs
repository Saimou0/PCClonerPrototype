using Receiver;
using Sender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCClonerPrototype.Forms
{
    public partial class ReceiverForm2 : Form
    {
        private static ReceiverForm _receiverForm1 = new();
        private static ReceiverFinalForm _receiverFinalForm = new();
        private static CancellationTokenSource? _cancellationTokenSource;
        public ReceiverForm2()
        {
            InitializeComponent();
        }

        public static async Task StartReceiverHTTP(IPAddress senderIP)
        {
            _cancellationTokenSource = new();

            await Task.Run(async () =>
            {
                await ReceiverHTTP.Start(senderIP, _cancellationTokenSource.Token);
            });

            _receiverFinalForm.Dock = DockStyle.Fill;
            _receiverFinalForm.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_receiverFinalForm);
            _receiverFinalForm.Show();

            _receiverFinalForm.StatusReport(ReceiverHTTP.GetReceiverOperations());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _receiverForm1.Dock = DockStyle.Fill;
            _receiverForm1.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_receiverForm1);
            _receiverForm1.Show();

        }
    }
}
