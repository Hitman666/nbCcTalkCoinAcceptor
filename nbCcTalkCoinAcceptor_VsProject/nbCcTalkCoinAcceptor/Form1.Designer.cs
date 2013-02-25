namespace nbCcTalkCoinAcceptor
{
    partial class nbCoinAcceptor
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
            this.btnConnectToCA = new System.Windows.Forms.Button();
            this.cbPolling = new System.Windows.Forms.CheckBox();
            this.cbInhibit = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labTotalMoneyIn = new System.Windows.Forms.Label();
            this.txtPortNumber = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnectToCA
            // 
            this.btnConnectToCA.Location = new System.Drawing.Point(82, 8);
            this.btnConnectToCA.Name = "btnConnectToCA";
            this.btnConnectToCA.Size = new System.Drawing.Size(75, 23);
            this.btnConnectToCA.TabIndex = 0;
            this.btnConnectToCA.Text = "Connect";
            this.btnConnectToCA.UseVisualStyleBackColor = true;
            this.btnConnectToCA.Click += new System.EventHandler(this.btnConnectToCA_Click);
            // 
            // cbPolling
            // 
            this.cbPolling.AutoSize = true;
            this.cbPolling.Location = new System.Drawing.Point(163, 12);
            this.cbPolling.Name = "cbPolling";
            this.cbPolling.Size = new System.Drawing.Size(95, 17);
            this.cbPolling.TabIndex = 2;
            this.cbPolling.Text = "Poll Start/Stop";
            this.cbPolling.UseVisualStyleBackColor = true;
            this.cbPolling.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbInhibit
            // 
            this.cbInhibit.AutoSize = true;
            this.cbInhibit.Location = new System.Drawing.Point(264, 12);
            this.cbInhibit.Name = "cbInhibit";
            this.cbInhibit.Size = new System.Drawing.Size(106, 17);
            this.cbInhibit.TabIndex = 3;
            this.cbInhibit.Text = "Inhibit Start/Stop";
            this.cbInhibit.UseVisualStyleBackColor = true;
            this.cbInhibit.CheckedChanged += new System.EventHandler(this.cbInhibit_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Total money IN:";
            // 
            // labTotalMoneyIn
            // 
            this.labTotalMoneyIn.AutoSize = true;
            this.labTotalMoneyIn.Location = new System.Drawing.Point(464, 13);
            this.labTotalMoneyIn.Name = "labTotalMoneyIn";
            this.labTotalMoneyIn.Size = new System.Drawing.Size(0, 13);
            this.labTotalMoneyIn.TabIndex = 5;
            // 
            // txtPortNumber
            // 
            this.txtPortNumber.Location = new System.Drawing.Point(43, 10);
            this.txtPortNumber.Name = "txtPortNumber";
            this.txtPortNumber.Size = new System.Drawing.Size(33, 20);
            this.txtPortNumber.TabIndex = 6;
            this.txtPortNumber.Text = "6";
            this.txtPortNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(11, 39);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(483, 340);
            this.txtLog.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Port:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nbCoinAcceptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 391);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPortNumber);
            this.Controls.Add(this.labTotalMoneyIn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbInhibit);
            this.Controls.Add(this.cbPolling);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnConnectToCA);
            this.Name = "nbCoinAcceptor";
            this.Text = "nbCoinAcceptor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnectToCA;
        private System.Windows.Forms.CheckBox cbPolling;
        private System.Windows.Forms.CheckBox cbInhibit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labTotalMoneyIn;
        private System.Windows.Forms.TextBox txtPortNumber;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label2;
    }
}

