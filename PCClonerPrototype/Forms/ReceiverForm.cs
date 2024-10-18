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
using Client;

namespace PCClonerPrototype.Forms
{
    public partial class ReceiverForm : Form
    {
        private static MainPage _mainPage = new();
        private static SenderForm1 _senderForm = new();
        public static int connectionCode;
        public ReceiverForm()
        {
            InitializeComponent();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            connectionCode = int.Parse(txtConnectionCode.Text);
            StartSender(connectionCode);
        }
        public void StartSender(int connectionCode)
        {
            SenderUDP.Start(connectionCode);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _mainPage.Dock = DockStyle.Fill;
            _mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_mainPage);
            _mainPage.Show();
        }

    }
}
