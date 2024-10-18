namespace PCClonerPrototype.Forms
{
    partial class MainPage
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
            btnSender = new Button();
            btnReceiver = new Button();
            SuspendLayout();
            // 
            // btnSender
            // 
            btnSender.BackColor = SystemColors.ScrollBar;
            btnSender.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSender.Location = new Point(100, 160);
            btnSender.Name = "btnSender";
            btnSender.Size = new Size(200, 100);
            btnSender.TabIndex = 0;
            btnSender.Text = "Lähettävä";
            btnSender.UseVisualStyleBackColor = false;
            btnSender.Click += btnSender_Click;
            // 
            // btnReceiver
            // 
            btnReceiver.BackColor = SystemColors.ScrollBar;
            btnReceiver.Font = new Font("Arial", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReceiver.Location = new Point(500, 160);
            btnReceiver.Name = "btnReceiver";
            btnReceiver.Size = new Size(200, 100);
            btnReceiver.TabIndex = 1;
            btnReceiver.Text = "Vastaanottava";
            btnReceiver.UseVisualStyleBackColor = false;
            btnReceiver.Click += btnReceiver_Click;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(btnReceiver);
            Controls.Add(btnSender);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.None;
            Name = "MainPage";
            Text = "MainPage";
            ResumeLayout(false);
        }

        #endregion

        private Button btnSender;
        private Button btnReceiver;
    }
}