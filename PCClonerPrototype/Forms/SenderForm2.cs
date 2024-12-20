using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCClonerPrototype;
using Sender;

namespace PCClonerPrototype.Forms
{
    public partial class SenderForm2 : Form
    {
        private static SenderForm1 _senderForm1 = new();
        private static SenderForm3 _senderForm3 = new();
        private static CancellationTokenSource? _cancellationTokenSource;
        private static bool _UDPResult = false;
        public SenderForm2()
        {
            InitializeComponent();
        }

        public async Task StartNetworking()
        {
            _cancellationTokenSource = new();

            try
            {
                while (true)
                {
                    // Regenerate udp connection code and start listening
                    SenderUDP.RegenerateCode();
                    lblConnectionCode.Text = SenderUDP.GetConnectionCode();
                    _UDPResult = await SenderUDP.ConnectionListenerAsync(_cancellationTokenSource.Token);

                    // break the loop if cancellation has been requested.
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation requested, exiting loop.");
                        break;
                    }

                    // Check udp result
                    if (_UDPResult == true)
                    {
                        _senderForm3.Dock = DockStyle.Fill;
                        _senderForm3.TopLevel = false;
                        MainForm.MainPanel.Controls.Clear();
                        MainForm.MainPanel.Controls.Add(_senderForm3);
                        _senderForm3.Show();

                        _UDPResult = false;

                        _cancellationTokenSource.Dispose();

                        // Start HTTP
                        SenderForm3.StartSenderHTTP();
                
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was canceled");
            }
            finally
            {
                _cancellationTokenSource.Dispose();
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            Program.fileSelection.Clear();

            _senderForm1.Dock = DockStyle.Fill;
            _senderForm1.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderForm1);
            _senderForm1.Show();
        }
    }
}
