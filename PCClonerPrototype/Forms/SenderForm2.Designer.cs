namespace PCClonerPrototype.Forms
{
    partial class SenderForm2
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
            lblConnectionCode = new Label();
            btnBack = new Button();
            SuspendLayout();
            // 
            // lblConnectionCode
            // 
            lblConnectionCode.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblConnectionCode.AutoSize = true;
            lblConnectionCode.Font = new Font("Arial", 30F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConnectionCode.Location = new Point(310, 175);
            lblConnectionCode.Name = "lblConnectionCode";
            lblConnectionCode.Size = new Size(132, 46);
            lblConnectionCode.TabIndex = 0;
            lblConnectionCode.Text = "label1";
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
            // SenderForm2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(btnBack);
            Controls.Add(lblConnectionCode);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SenderForm2";
            Text = "SenderForm2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblConnectionCode;
        private Button btnBack;
    }
}