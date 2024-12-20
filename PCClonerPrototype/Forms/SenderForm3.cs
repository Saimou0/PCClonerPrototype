using Sender;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCClonerPrototype.Forms
{
    public partial class SenderForm3 : Form
    {
        private static SenderForm1 _senderForm1 = new();
        private static SenderFinalForm _senderFinalForm = new();
        private static CancellationTokenSource? _cancellationTokenSource;

        public SenderForm3()
        {
            InitializeComponent();
        }

        public static async Task StartSenderHTTP()
        {
            _cancellationTokenSource = new();

            await Task.Run(async () =>
            {
                await SenderHTTP.Start(SenderUDP.clientEP, Program.fileSelection, Program.selectedFolderPath, _cancellationTokenSource.Token);
            });

            _senderFinalForm.Dock = DockStyle.Fill;
            _senderFinalForm.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderFinalForm);
            _senderFinalForm.Show();

            _senderFinalForm.StatusReport(SenderHTTP.GetSenderOperations());

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _senderForm1.Dock = DockStyle.Fill;
            _senderForm1.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderForm1);
            _senderForm1.Show();
        }
    }
}
