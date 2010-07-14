namespace RIXoxTestServer
{
    partial class ServerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtVFOA = new System.Windows.Forms.TextBox();
            this.txtVFOB = new System.Windows.Forms.TextBox();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.cbMox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "VFOA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "VFOB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Mode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "MOX";
            // 
            // txtVFOA
            // 
            this.txtVFOA.Location = new System.Drawing.Point(97, 39);
            this.txtVFOA.Name = "txtVFOA";
            this.txtVFOA.Size = new System.Drawing.Size(100, 20);
            this.txtVFOA.TabIndex = 5;
            this.txtVFOA.Text = "50.125";
            this.txtVFOA.TextChanged += new System.EventHandler(this.txtVFOA_TextChanged);
            // 
            // txtVFOB
            // 
            this.txtVFOB.Location = new System.Drawing.Point(97, 65);
            this.txtVFOB.Name = "txtVFOB";
            this.txtVFOB.Size = new System.Drawing.Size(100, 20);
            this.txtVFOB.TabIndex = 6;
            this.txtVFOB.Text = "7.000";
            this.txtVFOB.TextChanged += new System.EventHandler(this.txtVFOB_TextChanged);
            // 
            // txtMode
            // 
            this.txtMode.Location = new System.Drawing.Point(97, 90);
            this.txtMode.Name = "txtMode";
            this.txtMode.Size = new System.Drawing.Size(100, 20);
            this.txtMode.TabIndex = 7;
            this.txtMode.Text = "USB";
            this.txtMode.TextChanged += new System.EventHandler(this.txtMode_TextChanged);
            // 
            // cbMox
            // 
            this.cbMox.AutoSize = true;
            this.cbMox.Location = new System.Drawing.Point(97, 115);
            this.cbMox.Name = "cbMox";
            this.cbMox.Size = new System.Drawing.Size(15, 14);
            this.cbMox.TabIndex = 8;
            this.cbMox.UseVisualStyleBackColor = true;
            this.cbMox.CheckedChanged += new System.EventHandler(this.cbMox_CheckedChanged);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.cbMox);
            this.Controls.Add(this.txtMode);
            this.Controls.Add(this.txtVFOB);
            this.Controls.Add(this.txtVFOA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "ServerForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtVFOA;
        private System.Windows.Forms.TextBox txtVFOB;
        private System.Windows.Forms.TextBox txtMode;
        private System.Windows.Forms.CheckBox cbMox;
    }
}

