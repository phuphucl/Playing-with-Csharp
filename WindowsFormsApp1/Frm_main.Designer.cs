namespace WindowsFormsApp1
{
    partial class Frm_main
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
            this.label1 = new System.Windows.Forms.Label();
            this.TxtNumber = new System.Windows.Forms.TextBox();
            this.BtnFindPrime = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.myProgressBar2 = new WindowsFormsApp1.MyProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Result";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TxtNumber
            // 
            this.TxtNumber.Location = new System.Drawing.Point(13, 13);
            this.TxtNumber.Name = "TxtNumber";
            this.TxtNumber.Size = new System.Drawing.Size(100, 20);
            this.TxtNumber.TabIndex = 1;
            this.TxtNumber.Text = "Type here";
            this.TxtNumber.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // BtnFindPrime
            // 
            this.BtnFindPrime.Location = new System.Drawing.Point(119, 12);
            this.BtnFindPrime.Name = "BtnFindPrime";
            this.BtnFindPrime.Size = new System.Drawing.Size(75, 21);
            this.BtnFindPrime.TabIndex = 2;
            this.BtnFindPrime.Text = "Find prime #";
            this.BtnFindPrime.UseVisualStyleBackColor = true;
            this.BtnFindPrime.Click += new System.EventHandler(this.BtnFindPrime_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(200, 11);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(281, 10);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 4;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(122, 177);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(197, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "label2";
            // 
            // myProgressBar2
            // 
            this.myProgressBar2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myProgressBar2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.myProgressBar2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.myProgressBar2.Location = new System.Drawing.Point(141, 273);
            this.myProgressBar2.LockUpdate = false;
            this.myProgressBar2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.myProgressBar2.Maximum = 100;
            this.myProgressBar2.Minimum = 0;
            this.myProgressBar2.Name = "myProgressBar2";
            this.myProgressBar2.ProgressDirection = WindowsFormsApp1.MyProgressBar.EnProgressDirection.Horizontal;
            this.myProgressBar2.Size = new System.Drawing.Size(280, 18);
            this.myProgressBar2.TabIndex = 8;
            this.myProgressBar2.TextColor = System.Drawing.Color.Black;
            this.myProgressBar2.Value = 33;
            // 
            // Frm_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.myProgressBar2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnFindPrime);
            this.Controls.Add(this.TxtNumber);
            this.Controls.Add(this.label1);
            this.Name = "Frm_main";
            this.Text = "Phuc";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtNumber;
        private System.Windows.Forms.Button BtnFindPrime;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private MyProgressBar myProgressBar1;
        private MyProgressBar myProgressBar2;
    }
}

