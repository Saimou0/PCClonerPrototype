using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PCClonerPrototype;

namespace PCClonerPrototype.Forms
{
    public partial class SenderForm1 : Form
    {
        private static MainPage _mainPage = new();
        private static SenderForm2 _senderForm2 = new();
        public SenderForm1()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            _mainPage.Dock = DockStyle.Fill;
            _mainPage.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_mainPage);
            _mainPage.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            _senderForm2.Dock = DockStyle.Fill;
            _senderForm2.TopLevel = false;
            MainForm.MainPanel.Controls.Clear();
            MainForm.MainPanel.Controls.Add(_senderForm2);
            _senderForm2.Show();
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if(e.Action != TreeViewAction.Unknown)
            {
                if(e.Node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                }
            }
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach(TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if(node.Nodes.Count > 0)
                {
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
    }
}
