namespace PCClonerPrototype.Forms
{
    partial class SenderForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TreeNode treeNode1 = new TreeNode("Kuvat");
            TreeNode treeNode2 = new TreeNode("Videot");
            TreeNode treeNode3 = new TreeNode("Musiikki");
            TreeNode treeNode4 = new TreeNode("Tiedostot");
            TreeNode treeNode5 = new TreeNode("Ladatut tiedostot");
            TreeNode treeNode6 = new TreeNode("Työpöytä");
            TreeNode treeNode7 = new TreeNode("Käyttäjän tiedostot", new TreeNode[] { treeNode1, treeNode2, treeNode3, treeNode4, treeNode5, treeNode6 });
            TreeNode treeNode8 = new TreeNode("Wi-Fi yhteydet ja salasanat");
            TreeNode treeNode9 = new TreeNode("Ajurit");
            TreeNode treeNode10 = new TreeNode("Työpöydän taustakuva");
            TreeNode treeNode11 = new TreeNode("Selainten kirjanmerkit ja muut tiedot");
            treeView1 = new TreeView();
            panel1 = new Panel();
            btnBack = new Button();
            btnNext = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // treeView1
            // 
            treeView1.BackColor = SystemColors.Window;
            treeView1.CheckBoxes = true;
            treeView1.Dock = DockStyle.Fill;
            treeView1.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            treeView1.HideSelection = false;
            treeView1.Indent = 25;
            treeView1.ItemHeight = 35;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeNode1.Name = "Node1";
            treeNode1.Text = "Kuvat";
            treeNode2.Name = "Node2";
            treeNode2.Text = "Videot";
            treeNode3.Name = "Node3";
            treeNode3.Text = "Musiikki";
            treeNode4.Name = "Node4";
            treeNode4.Text = "Tiedostot";
            treeNode5.Name = "Node5";
            treeNode5.Text = "Ladatut tiedostot";
            treeNode6.Name = "Node6";
            treeNode6.Text = "Työpöytä";
            treeNode7.Name = "Node0";
            treeNode7.Text = "Käyttäjän tiedostot";
            treeNode8.Name = "Node7";
            treeNode8.Text = "Wi-Fi yhteydet ja salasanat";
            treeNode9.Name = "Node8";
            treeNode9.Text = "Ajurit";
            treeNode10.Name = "Node9";
            treeNode10.Text = "Työpöydän taustakuva";
            treeNode11.Name = "Node10";
            treeNode11.Text = "Selainten kirjanmerkit ja muut tiedot";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode7, treeNode8, treeNode9, treeNode10, treeNode11 });
            treeView1.Size = new Size(776, 346);
            treeView1.TabIndex = 0;
            treeView1.AfterCheck += treeView1_AfterCheck;
            treeView1.BeforeSelect += treeView1_BeforeSelect;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ScrollBar;
            panel1.Controls.Add(treeView1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(776, 346);
            panel1.TabIndex = 0;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(713, 415);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 1;
            btnBack.Text = "Takaisin";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(632, 415);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 23);
            btnNext.TabIndex = 2;
            btnNext.Text = "Seuraava";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // SenderForm1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(btnNext);
            Controls.Add(btnBack);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SenderForm1";
            Text = "SenderForm";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TreeView treeView1;
        private Button btnBack;
        private Button btnNext;
    }
}