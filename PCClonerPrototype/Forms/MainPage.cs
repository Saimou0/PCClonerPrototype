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
    public partial class MainPage : Form
    {
        private static SenderForm1 _senderForm1 = new();
        private static ReceiverForm _receiverForm = new();
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnSender_Click(object sender, EventArgs e)
        {
            _senderForm1.Dock = DockStyle.Fill;
            _senderForm1.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderForm1);
            _senderForm1.Show();
        }

        private void btnReceiver_Click(object sender, EventArgs e)
        {
            _receiverForm.Dock = DockStyle.Fill;
            _receiverForm.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_receiverForm);
            _receiverForm.Show();
        }
    }
}
