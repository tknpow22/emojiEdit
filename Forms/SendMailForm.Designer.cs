namespace emojiEdit
{
    partial class SendMailForm
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
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendMailForm));
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.textBoxSmtpPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxSmtpUserId = new System.Windows.Forms.TextBox();
            this.labelUserId = new System.Windows.Forms.Label();
            this.textBoxSmtpServer = new System.Windows.Forms.TextBox();
            this.labelSmtpServer = new System.Windows.Forms.Label();
            this.numericUpDownSmtpPort = new System.Windows.Forms.NumericUpDown();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBoxServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSmtpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(12, 142);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(118, 33);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "送信(&S)";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(333, 142);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(118, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "閉じる(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.textBoxSmtpPassword);
            this.groupBoxServer.Controls.Add(this.labelPassword);
            this.groupBoxServer.Controls.Add(this.textBoxSmtpUserId);
            this.groupBoxServer.Controls.Add(this.labelUserId);
            this.groupBoxServer.Controls.Add(this.textBoxSmtpServer);
            this.groupBoxServer.Controls.Add(this.labelSmtpServer);
            this.groupBoxServer.Controls.Add(this.numericUpDownSmtpPort);
            this.groupBoxServer.Controls.Add(this.labelPort);
            this.groupBoxServer.Location = new System.Drawing.Point(12, 12);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(439, 124);
            this.groupBoxServer.TabIndex = 0;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = "SMTPサーバー設定(&E)";
            // 
            // textBoxSmtpPassword
            // 
            this.textBoxSmtpPassword.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxSmtpPassword.Location = new System.Drawing.Point(120, 93);
            this.textBoxSmtpPassword.Name = "textBoxSmtpPassword";
            this.textBoxSmtpPassword.Size = new System.Drawing.Size(310, 19);
            this.textBoxSmtpPassword.TabIndex = 7;
            this.textBoxSmtpPassword.Tag = "パスワード";
            this.textBoxSmtpPassword.Leave += new System.EventHandler(this.textBoxToHankaku_Leave);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(6, 96);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(71, 12);
            this.labelPassword.TabIndex = 6;
            this.labelPassword.Text = "パスワード(&W):";
            // 
            // textBoxSmtpUserId
            // 
            this.textBoxSmtpUserId.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxSmtpUserId.Location = new System.Drawing.Point(120, 68);
            this.textBoxSmtpUserId.Name = "textBoxSmtpUserId";
            this.textBoxSmtpUserId.Size = new System.Drawing.Size(310, 19);
            this.textBoxSmtpUserId.TabIndex = 5;
            this.textBoxSmtpUserId.Tag = "ユーザーID";
            this.textBoxSmtpUserId.Leave += new System.EventHandler(this.textBoxToHankaku_Leave);
            // 
            // labelUserId
            // 
            this.labelUserId.AutoSize = true;
            this.labelUserId.Location = new System.Drawing.Point(6, 71);
            this.labelUserId.Name = "labelUserId";
            this.labelUserId.Size = new System.Drawing.Size(74, 12);
            this.labelUserId.TabIndex = 4;
            this.labelUserId.Text = "ユーザーID(&U):";
            // 
            // textBoxSmtpServer
            // 
            this.textBoxSmtpServer.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxSmtpServer.Location = new System.Drawing.Point(120, 18);
            this.textBoxSmtpServer.Name = "textBoxSmtpServer";
            this.textBoxSmtpServer.Size = new System.Drawing.Size(313, 19);
            this.textBoxSmtpServer.TabIndex = 1;
            this.textBoxSmtpServer.Tag = "SMTPサーバー";
            this.textBoxSmtpServer.Leave += new System.EventHandler(this.textBoxToHankaku_Leave);
            // 
            // labelSmtpServer
            // 
            this.labelSmtpServer.AutoSize = true;
            this.labelSmtpServer.Location = new System.Drawing.Point(6, 21);
            this.labelSmtpServer.Name = "labelSmtpServer";
            this.labelSmtpServer.Size = new System.Drawing.Size(94, 12);
            this.labelSmtpServer.TabIndex = 0;
            this.labelSmtpServer.Text = "SMTPサーバー(&M):";
            // 
            // numericUpDownSmtpPort
            // 
            this.numericUpDownSmtpPort.Location = new System.Drawing.Point(120, 43);
            this.numericUpDownSmtpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownSmtpPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSmtpPort.Name = "numericUpDownSmtpPort";
            this.numericUpDownSmtpPort.Size = new System.Drawing.Size(70, 19);
            this.numericUpDownSmtpPort.TabIndex = 3;
            this.numericUpDownSmtpPort.Tag = "ポート番号";
            this.numericUpDownSmtpPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(6, 45);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(74, 12);
            this.labelPort.TabIndex = 2;
            this.labelPort.Text = "ポート番号(&P):";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(12, 181);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(439, 181);
            this.textBoxLog.TabIndex = 3;
            // 
            // SendMailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 374);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.groupBoxServer);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SendMailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "送信";
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSmtpPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.TextBox textBoxSmtpPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxSmtpUserId;
        private System.Windows.Forms.Label labelUserId;
        private System.Windows.Forms.TextBox textBoxSmtpServer;
        private System.Windows.Forms.Label labelSmtpServer;
        private System.Windows.Forms.NumericUpDown numericUpDownSmtpPort;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxLog;
    }
}