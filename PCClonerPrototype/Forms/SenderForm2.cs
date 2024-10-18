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
using Server;

namespace PCClonerPrototype.Forms
{
    public partial class SenderForm2 : Form
    {
        private static SenderForm1 _senderForm1 = new();
        public SenderForm2()
        {
            InitializeComponent();
            lblConnectionCode.Text = ReceiverUDP.GetConnectionCode();
            //TODO: Make the back button close everything and make the receiverUDP re-roll the code everytime.
            StartUDP();
        }

        private static async Task StartUDP()
        {
            await ReceiverUDP.ConnectionListener();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _senderForm1.Dock = DockStyle.Fill;
            _senderForm1.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderForm1);
            _senderForm1.Show();
        }
    }
}
