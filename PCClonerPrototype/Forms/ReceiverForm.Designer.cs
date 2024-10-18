namespace PCClonerPrototype.Forms
{
    partial class ReceiverForm
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
            txtConnectionCode = new TextBox();
            btnNext = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.Location = new Point(713, 415);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 0;
            btnBack.Text = "Takaisin";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // txtConnectionCode
            // 
            txtConnectionCode.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtConnectionCode.Location = new Point(200, 157);
            txtConnectionCode.Name = "txtConnectionCode";
            txtConnectionCode.Size = new Size(400, 35);
            txtConnectionCode.TabIndex = 1;
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(109, 209);
            label1.Name = "label1";
            label1.Size = new Size(609, 27);
            label1.TabIndex = 3;
            label1.Text = "Kirjoita yhdistämis koodi lähettävästä windows koneesta";
            // 
            // ReceiverForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.AppWorkspace;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(btnNext);
            Controls.Add(txtConnectionCode);
            Controls.Add(btnBack);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ReceiverForm";
            Text = "ReceiverForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBack;
        private TextBox txtConnectionCode;
        private Button btnNext;
        private Label label1;
    }
}