namespace WindowsFormsApp1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.AttackBox = new System.Windows.Forms.TextBox();
            this.URLweb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(222, 258);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(459, 61);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start DDos Attack";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AttackBox
            // 
            this.AttackBox.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.AttackBox.Location = new System.Drawing.Point(222, 98);
            this.AttackBox.Multiline = true;
            this.AttackBox.Name = "AttackBox";
            this.AttackBox.Size = new System.Drawing.Size(145, 55);
            this.AttackBox.TabIndex = 2;
            this.AttackBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // URLweb
            // 
            this.URLweb.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.URLweb.Location = new System.Drawing.Point(222, 168);
            this.URLweb.Multiline = true;
            this.URLweb.Name = "URLweb";
            this.URLweb.Size = new System.Drawing.Size(459, 55);
            this.URLweb.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 41);
            this.label2.TabIndex = 5;
            this.label2.Text = "Attack with";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 41);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target URL:";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(387, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 48);
            this.label3.TabIndex = 7;
            this.label3.Text = "Browsers";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(222, 325);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(459, 69);
            this.button2.TabIndex = 8;
            this.button2.Text = "Close All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.URLweb);
            this.Controls.Add(this.AttackBox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "MoltiProcess DDoS Attack";
            this.Click += new System.EventHandler(this.Form1_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox AttackBox;
        private System.Windows.Forms.TextBox URLweb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

