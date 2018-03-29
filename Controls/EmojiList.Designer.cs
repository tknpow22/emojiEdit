namespace emojiEdit
{
    partial class EmojiList
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelHistory = new System.Windows.Forms.Label();
            this.pictureEmojiGroup0 = new System.Windows.Forms.PictureBox();
            this.panelHistory = new System.Windows.Forms.Panel();
            this.tabControlEmojiList = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmojiGroup0)).BeginInit();
            this.panelHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHistory
            // 
            this.labelHistory.AutoSize = true;
            this.labelHistory.Location = new System.Drawing.Point(3, 0);
            this.labelHistory.Name = "labelHistory";
            this.labelHistory.Size = new System.Drawing.Size(31, 12);
            this.labelHistory.TabIndex = 0;
            this.labelHistory.Text = "履歴:";
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
            // 
            // panelHistory
            // 
            this.panelHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelHistory.AutoScroll = true;
            this.panelHistory.BackColor = System.Drawing.Color.White;
            this.panelHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHistory.Controls.Add(this.pictureEmojiGroup0);
            this.panelHistory.Location = new System.Drawing.Point(3, 15);
            this.panelHistory.Name = "panelHistory";
            this.panelHistory.Size = new System.Drawing.Size(357, 76);
            this.panelHistory.TabIndex = 1;
            // 
            // tabControlEmojiList
            // 
            this.tabControlEmojiList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEmojiList.Location = new System.Drawing.Point(4, 97);
            this.tabControlEmojiList.Name = "tabControlEmojiList";
            this.tabControlEmojiList.SelectedIndex = 0;
            this.tabControlEmojiList.Size = new System.Drawing.Size(356, 237);
            this.tabControlEmojiList.TabIndex = 2;
            // 
            // EmojiList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelHistory);
            this.Controls.Add(this.panelHistory);
            this.Controls.Add(this.tabControlEmojiList);
            this.Name = "EmojiList";
            this.Size = new System.Drawing.Size(363, 337);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmojiGroup0)).EndInit();
            this.panelHistory.ResumeLayout(false);
            this.panelHistory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHistory;
        private System.Windows.Forms.PictureBox pictureEmojiGroup0;
        private System.Windows.Forms.Panel panelHistory;
        private System.Windows.Forms.TabControl tabControlEmojiList;
    }
}
