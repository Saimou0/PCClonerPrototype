namespace PCClonerPrototype.Forms
{
    partial class ReceiverForm2
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
            btnBack = new Button();
            pBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.Location = new Point(713, 415);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 1;
            btnBack.Text = "Peruuta";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // pBar1
            // 
            pBar1.Location = new Point(200, 200);
            pBar1.Name = "pBar1";
            pBar1.Size = new Size(400, 23);
            pBar1.TabIndex = 2;
            // 
            // ReceiverForm2
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(pBar1);
            Controls.Add(btnBack);
            Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.None;
            Name = "ReceiverForm2";
            Text = "ReceiverForm2";
            ResumeLayout(false);
        }

        #endregion

        private Button btnBack;
        private ProgressBar pBar1;
    }
}