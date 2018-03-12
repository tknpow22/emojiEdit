namespace emojiEdit
{
    partial class SelectEmojiForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectEmojiForm));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.tabControlEmojiList = new System.Windows.Forms.TabControl();
            this.pictureEmojiGroup0 = new System.Windows.Forms.PictureBox();
            this.panelHistory = new System.Windows.Forms.Panel();
            this.labelHistory = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmojiGroup0)).BeginInit();
            this.panelHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(212, 379);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 33);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClear.Location = new System.Drawing.Point(7, 379);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(94, 33);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "消去(&D)";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // tabControlEmojiList
            // 
            this.tabControlEmojiList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEmojiList.Location = new System.Drawing.Point(7, 106);
            this.tabControlEmojiList.Name = "tabControlEmojiList";
            this.tabControlEmojiList.SelectedIndex = 0;
            this.tabControlEmojiList.Size = new System.Drawing.Size(299, 267);
            this.tabControlEmojiList.TabIndex = 2;
            // 
            // pictureEmojiGroup0
            // 
            this.pictureEmojiGroup0.Location = new System.Drawing.Point(0, 0);
            this.pictureEmojiGroup0.Name = "pictureEmojiGroup0";
            this.pictureEmojiGroup0.Size = new System.Drawing.Size(100, 50);
            this.pictureEmojiGroup0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureEmojiGroup0.TabIndex = 0;
            this.pictureEmojiGroup0.TabStop = false;
            this.pictureEmojiGroup0.Tag = "0";
            this.pictureEmojiGroup0.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureEmojiGroupX_MouseClick);
            // 
            // panelHistory
            // 
            this.panelHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHistory.AutoScroll = true;
            this.panelHistory.BackColor = System.Drawing.Color.White;
            this.panelHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHistory.Controls.Add(this.pictureEmojiGroup0);
            this.panelHistory.Location = new System.Drawing.Point(7, 24);
            this.panelHistory.Name = "panelHistory";
            this.panelHistory.Size = new System.Drawing.Size(295, 76);
            this.panelHistory.TabIndex = 1;
            // 
            // labelHistory
            // 
            this.labelHistory.AutoSize = true;
            this.labelHistory.Location = new System.Drawing.Point(5, 9);
            this.labelHistory.Name = "labelHistory";
            this.labelHistory.Size = new System.Drawing.Size(31, 12);
            this.labelHistory.TabIndex = 0;
            this.labelHistory.Text = "履歴:";
            // 
            // SelectEmojiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 421);
            this.Controls.Add(this.labelHistory);
            this.Controls.Add(this.panelHistory);
            this.Controls.Add(this.tabControlEmojiList);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectEmojiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "絵文字選択";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EmojiListForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmojiGroup0)).EndInit();
            this.panelHistory.ResumeLayout(false);
            this.panelHistory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TabControl tabControlEmojiList;
        private System.Windows.Forms.PictureBox pictureEmojiGroup0;
        private System.Windows.Forms.Panel panelHistory;
        private System.Windows.Forms.Label labelHistory;
    }
}