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

            SaveFileSelection();

            _senderForm2.StartNetworking();
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode? selectedNode = e.Node;
            if (selectedNode?.Tag?.Equals("Select Folder") == true)
            {
                if(selectedNode.Checked)
                {
                    FolderBrowserDialog dialog = new();
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        lblSelectedFolder.Visible = true;
                        lblSelectedFolderPath.Visible = true;
                        
                        lblSelectedFolder.Text = "Valittu kansio: " + Path.GetFileName(dialog.SelectedPath);
                        lblSelectedFolderPath.Text = "Kansion polku: " + dialog.SelectedPath;
                        
                        Program.selectedFolderPath = dialog.SelectedPath;
                    } 
                    else
                    {
                        selectedNode.Checked = false;
                    }
                }
                else
                {
                    lblSelectedFolder.Visible = false;
                    lblSelectedFolderPath.Visible = false;
                    
                    lblSelectedFolder.Text = "";
                    lblSelectedFolderPath.Text = "";

                    Program.selectedFolderPath = "";
                }
            }

            // Check if node has child nodes
            if (e.Action != TreeViewAction.Unknown)
            {
                // Check or uncheck all the child nodes
                if (e.Node?.Nodes?.Count > 0)
                {
                    CheckAllChildNodes(e.Node, e.Node.Checked);
                }
                else if (e.Node != null)
                {
                    CheckAllChildNodes(e.Node, false);
                }
            }
        }

        private void SaveFileSelection()
        {
            // Save nodes as keys and checks as values to program dictionary.
            Program.fileSelection?.Clear();
            foreach (TreeNode node in treeView1.Nodes)
            {
                SaveNodeSelection(node);
            }
        }

        private static void SaveNodeSelection(TreeNode node)
        {
            Program.fileSelection[node.Text] = node.Checked;
            foreach (TreeNode childNode in node.Nodes)
            {
                SaveNodeSelection(childNode);
            }
        }

        private static void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach(TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if(node.Nodes.Count > 0)
                {
                    CheckAllChildNodes(node, nodeChecked);
                }
            }
        }
    }
}
