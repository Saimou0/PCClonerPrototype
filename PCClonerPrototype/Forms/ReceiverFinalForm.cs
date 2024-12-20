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
    public partial class ReceiverFinalForm : Form
    {
        public ReceiverFinalForm()
        {
            InitializeComponent();
        }

        public void StatusReport(Dictionary<string, string> completedOperations)
        {
            foreach (var operation in completedOperations)
            {
                ListViewItem item = new(operation.Key);
                if (operation.Value.Equals("Completed")) item.SubItems.Add("Valmis");
                if (operation.Value.Equals("Error")) item.SubItems.Add("Virhe");
                listView1.Items.Add(item);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
