namespace RIXoxTestClient
{
    partial class ClientForm
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
            this.components = new System.ComponentModel.Container();
            this.cbMox = new System.Windows.Forms.CheckBox();
            this.txtMode = new System.Windows.Forms.TextBox();
            this.txtVFOB = new System.Windows.Forms.TextBox();
            this.txtVFOA = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.radioDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radioDataBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // cbMox
            // 
            this.cbMox.AutoSize = true;
            this.cbMox.Location = new System.Drawing.Point(96, 122);
            this.cbMox.Name = "cbMox";
            this.cbMox.Size = new System.Drawing.Size(15, 14);
            this.cbMox.TabIndex = 16;
            this.cbMox.UseVisualStyleBackColor = true;
            // 
            // txtMode
            // 
            this.txtMode.Location = new System.Drawing.Point(96, 97);
            this.txtMode.Name = "txtMode";
            this.txtMode.Size = new System.Drawing.Size(100, 20);
            this.txtMode.TabIndex = 15;
            // 
            // txtVFOB
            // 
            this.txtVFOB.Location = new System.Drawing.Point(96, 72);
            this.txtVFOB.Name = "txtVFOB";
            this.txtVFOB.Size = new System.Drawing.Size(100, 20);
            this.txtVFOB.TabIndex = 14;
            // 
            // txtVFOA
            // 
            this.txtVFOA.Location = new System.Drawing.Point(96, 46);
            this.txtVFOA.Name = "txtVFOA";
            this.txtVFOA.Size = new System.Drawing.Size(100, 20);
            this.txtVFOA.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "MOX";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "VFOB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "VFOA";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Start Client";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioDataBindingSource
            // 
            this.radioDataBindingSource.DataSource = typeof(RIXoxTestClient.RadioData);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(141, 145);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(131, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Send updates to server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbMox);
            this.Controls.Add(this.txtMode);
            this.Controls.Add(this.txtVFOB);
            this.Controls.Add(this.txtVFOA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.radioDataBindingSource, "vfob", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Name = "ClientForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.radioDataBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbMox;
        private System.Windows.Forms.TextBox txtMode;
        private System.Windows.Forms.TextBox txtVFOB;
        private System.Windows.Forms.TextBox txtVFOA;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.BindingSource radioDataBindingSource;
        private System.Windows.Forms.Button button2;
    }
}

