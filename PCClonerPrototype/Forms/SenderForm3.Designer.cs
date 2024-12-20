namespace PCClonerPrototype.Forms
{
    partial class SenderForm3
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
            btnCancel = new Button();
            pBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(713, 415);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 0;
            btnCancel.Text = "Peruuta";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // pBar1
            // 
            pBar1.Location = new Point(200, 200);
            pBar1.Name = "pBar1";
            pBar1.Size = new Size(400, 23);
            pBar1.TabIndex = 1;
            // 
            // SenderForm3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(pBar1);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SenderForm3";
            Text = "SenderForm3";
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancel;
        private ProgressBar pBar1;
    }
}