namespace emojiEdit
{
    partial class MainForm
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.buttonEmojiTest = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorVersion = new System.Windows.Forms.ToolStripSeparator();
            this.menuVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSend = new System.Windows.Forms.Button();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.labelAttachments = new System.Windows.Forms.Label();
            this.buttonSelAttachments = new System.Windows.Forms.Button();
            this.textBoxAttachments = new System.Windows.Forms.TextBox();
            this.labelBody = new System.Windows.Forms.Label();
            this.checkBoxForceInsertLineFeed = new System.Windows.Forms.CheckBox();
            this.buttonSelectMailFrom = new System.Windows.Forms.Button();
            this.buttonSelectMailTo = new System.Windows.Forms.Button();
            this.labelSubject = new System.Windows.Forms.Label();
            this.textBoxMailTo = new System.Windows.Forms.TextBox();
            this.textBoxMailFrom = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.lableFrom = new System.Windows.Forms.Label();
            this.textBoxMailBody = new emojiEdit.EmojiTextBox();
            this.textBoxMailSubject = new emojiEdit.EmojiTextBox();
            this.emojiList = new emojiEdit.EmojiList();
            this.menuLoadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonEmojiTest
            // 
            this.buttonEmojiTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEmojiTest.Location = new System.Drawing.Point(352, 658);
            this.buttonEmojiTest.Name = "buttonEmojiTest";
            this.buttonEmojiTest.Size = new System.Drawing.Size(100, 35);
            this.buttonEmojiTest.TabIndex = 3;
            this.buttonEmojiTest.Text = "絵文字テスト(&E)";
            this.buttonEmojiTest.UseVisualStyleBackColor = true;
            this.buttonEmojiTest.Click += new System.EventHandler(this.buttonEmojiTest_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuSettings});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(464, 24);
            this.mainMenu.TabIndex = 0;
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLoadFile,
            this.menuSaveFile});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(67, 20);
            this.menuFile.Text = "ファイル(&F)";
            // 
            // menuSaveFile
            // 
            this.menuSaveFile.Name = "menuSaveFile";
            this.menuSaveFile.Size = new System.Drawing.Size(180, 22);
            this.menuSaveFile.Text = "保存(&S)";
            this.menuSaveFile.Click += new System.EventHandler(this.menuSaveFile_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuEditSettings,
            this.toolStripSeparatorVersion,
            this.menuVersion});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(59, 20);
            this.menuSettings.Text = "設定(&G)";
            // 
            // menuEditSettings
            // 
            this.menuEditSettings.Name = "menuEditSettings";
            this.menuEditSettings.Size = new System.Drawing.Size(136, 22);
            this.menuEditSettings.Text = "編集設定(&E)";
            this.menuEditSettings.Click += new System.EventHandler(this.menuEditSettings_Click);
            // 
            // toolStripSeparatorVersion
            // 
            this.toolStripSeparatorVersion.Name = "toolStripSeparatorVersion";
            this.toolStripSeparatorVersion.Size = new System.Drawing.Size(133, 6);
            // 
            // menuVersion
            // 
            this.menuVersion.Name = "menuVersion";
            this.menuVersion.Size = new System.Drawing.Size(136, 22);
            this.menuVersion.Text = "バージョン(&V)";
            this.menuVersion.Click += new System.EventHandler(this.menuVersion_Click);
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSend.Location = new System.Drawing.Point(13, 658);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(90, 35);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "送信(&S)";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerMain.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 27);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMain.Panel1.Controls.Add(this.labelAttachments);
            this.splitContainerMain.Panel1.Controls.Add(this.buttonSelAttachments);
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxAttachments);
            this.splitContainerMain.Panel1.Controls.Add(this.labelBody);
            this.splitContainerMain.Panel1.Controls.Add(this.checkBoxForceInsertLineFeed);
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxMailBody);
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxMailSubject);
            this.splitContainerMain.Panel1.Controls.Add(this.buttonSelectMailFrom);
            this.splitContainerMain.Panel1.Controls.Add(this.buttonSelectMailTo);
            this.splitContainerMain.Panel1.Controls.Add(this.labelSubject);
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxMailTo);
            this.splitContainerMain.Panel1.Controls.Add(this.textBoxMailFrom);
            this.splitContainerMain.Panel1.Controls.Add(this.labelTo);
            this.splitContainerMain.Panel1.Controls.Add(this.lableFrom);
            this.splitContainerMain.Panel1MinSize = 200;
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainerMain.Panel2.Controls.Add(this.emojiList);
            this.splitContainerMain.Panel2MinSize = 200;
            this.splitContainerMain.Size = new System.Drawing.Size(464, 625);
            this.splitContainerMain.SplitterDistance = 300;
            this.splitContainerMain.TabIndex = 1;
            // 
            // labelAttachments
            // 
            this.labelAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelAttachments.AutoSize = true;
            this.labelAttachments.Location = new System.Drawing.Point(11, 270);
            this.labelAttachments.Name = "labelAttachments";
            this.labelAttachments.Size = new System.Drawing.Size(47, 12);
            this.labelAttachments.TabIndex = 11;
            this.labelAttachments.Text = "添付(&A):";
            // 
            // buttonSelAttachments
            // 
            this.buttonSelAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelAttachments.Location = new System.Drawing.Point(398, 265);
            this.buttonSelAttachments.Name = "buttonSelAttachments";
            this.buttonSelAttachments.Size = new System.Drawing.Size(54, 23);
            this.buttonSelAttachments.TabIndex = 13;
            this.buttonSelAttachments.Text = "選択";
            this.buttonSelAttachments.UseVisualStyleBackColor = true;
            this.buttonSelAttachments.Click += new System.EventHandler(this.buttonSelAttachments_Click);
            // 
            // textBoxAttachments
            // 
            this.textBoxAttachments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAttachments.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxAttachments.Location = new System.Drawing.Point(78, 267);
            this.textBoxAttachments.Name = "textBoxAttachments";
            this.textBoxAttachments.Size = new System.Drawing.Size(314, 19);
            this.textBoxAttachments.TabIndex = 12;
            this.textBoxAttachments.Tag = "添付ファイル";
            // 
            // labelBody
            // 
            this.labelBody.AutoSize = true;
            this.labelBody.Location = new System.Drawing.Point(10, 100);
            this.labelBody.Name = "labelBody";
            this.labelBody.Size = new System.Drawing.Size(47, 12);
            this.labelBody.TabIndex = 8;
            this.labelBody.Text = "本文(&B):";
            // 
            // checkBoxForceInsertLineFeed
            // 
            this.checkBoxForceInsertLineFeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxForceInsertLineFeed.AutoSize = true;
            this.checkBoxForceInsertLineFeed.Location = new System.Drawing.Point(78, 245);
            this.checkBoxForceInsertLineFeed.Name = "checkBoxForceInsertLineFeed";
            this.checkBoxForceInsertLineFeed.Size = new System.Drawing.Size(293, 16);
            this.checkBoxForceInsertLineFeed.TabIndex = 10;
            this.checkBoxForceInsertLineFeed.Text = "送信時、メール本文に1行の文字数毎に改行を入れる(&L)";
            this.checkBoxForceInsertLineFeed.UseVisualStyleBackColor = true;
            this.checkBoxForceInsertLineFeed.Click += new System.EventHandler(this.checkBoxForceInsertLineFeed_CheckedChanged);
            // 
            // buttonSelectMailFrom
            // 
            this.buttonSelectMailFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectMailFrom.Location = new System.Drawing.Point(398, 26);
            this.buttonSelectMailFrom.Name = "buttonSelectMailFrom";
            this.buttonSelectMailFrom.Size = new System.Drawing.Size(54, 23);
            this.buttonSelectMailFrom.TabIndex = 5;
            this.buttonSelectMailFrom.Text = "選択";
            this.buttonSelectMailFrom.UseVisualStyleBackColor = true;
            this.buttonSelectMailFrom.Click += new System.EventHandler(this.buttonSelectMailFrom_Click);
            // 
            // buttonSelectMailTo
            // 
            this.buttonSelectMailTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectMailTo.Location = new System.Drawing.Point(398, 1);
            this.buttonSelectMailTo.Name = "buttonSelectMailTo";
            this.buttonSelectMailTo.Size = new System.Drawing.Size(54, 23);
            this.buttonSelectMailTo.TabIndex = 2;
            this.buttonSelectMailTo.Text = "選択";
            this.buttonSelectMailTo.UseVisualStyleBackColor = true;
            this.buttonSelectMailTo.Click += new System.EventHandler(this.buttonSelectMailTo_Click);
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(10, 55);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(46, 12);
            this.labelSubject.TabIndex = 6;
            this.labelSubject.Text = "件名(&J):";
            // 
            // textBoxMailTo
            // 
            this.textBoxMailTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailTo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxMailTo.Location = new System.Drawing.Point(78, 3);
            this.textBoxMailTo.Name = "textBoxMailTo";
            this.textBoxMailTo.Size = new System.Drawing.Size(314, 19);
            this.textBoxMailTo.TabIndex = 1;
            this.textBoxMailTo.Tag = "宛先";
            // 
            // textBoxMailFrom
            // 
            this.textBoxMailFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailFrom.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxMailFrom.Location = new System.Drawing.Point(78, 28);
            this.textBoxMailFrom.Name = "textBoxMailFrom";
            this.textBoxMailFrom.Size = new System.Drawing.Size(314, 19);
            this.textBoxMailFrom.TabIndex = 4;
            this.textBoxMailFrom.Tag = "送信元";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(11, 6);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(47, 12);
            this.labelTo.TabIndex = 0;
            this.labelTo.Text = "宛先(&O):";
            // 
            // lableFrom
            // 
            this.lableFrom.AutoSize = true;
            this.lableFrom.Location = new System.Drawing.Point(10, 31);
            this.lableFrom.Name = "lableFrom";
            this.lableFrom.Size = new System.Drawing.Size(58, 12);
            this.lableFrom.TabIndex = 3;
            this.lableFrom.Text = "送信元(&T):";
            // 
            // textBoxMailBody
            // 
            this.textBoxMailBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailBody.ColumnLine = 0;
            this.textBoxMailBody.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F);
            this.textBoxMailBody.HideSelection = false;
            this.textBoxMailBody.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxMailBody.Location = new System.Drawing.Point(78, 98);
            this.textBoxMailBody.Multiline = true;
            this.textBoxMailBody.Name = "textBoxMailBody";
            this.textBoxMailBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMailBody.Size = new System.Drawing.Size(374, 141);
            this.textBoxMailBody.TabIndex = 9;
            this.textBoxMailBody.Tag = "本文";
            this.textBoxMailBody.Enter += new System.EventHandler(this.EmojiTextBox_Enter);
            this.textBoxMailBody.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EmojiTextBox_KeyDown);
            // 
            // textBoxMailSubject
            // 
            this.textBoxMailSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailSubject.ColumnLine = 0;
            this.textBoxMailSubject.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxMailSubject.HideSelection = false;
            this.textBoxMailSubject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxMailSubject.Location = new System.Drawing.Point(78, 53);
            this.textBoxMailSubject.Name = "textBoxMailSubject";
            this.textBoxMailSubject.Size = new System.Drawing.Size(374, 39);
            this.textBoxMailSubject.TabIndex = 7;
            this.textBoxMailSubject.Tag = "件名";
            this.textBoxMailSubject.Enter += new System.EventHandler(this.EmojiTextBox_Enter);
            this.textBoxMailSubject.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EmojiTextBox_KeyDown);
            // 
            // emojiList
            // 
            this.emojiList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.emojiList.Location = new System.Drawing.Point(4, 4);
            this.emojiList.Name = "emojiList";
            this.emojiList.PlaybackFocusTextBox = null;
            this.emojiList.Size = new System.Drawing.Size(457, 314);
            this.emojiList.TabIndex = 0;
            // 
            // menuLoadFile
            // 
            this.menuLoadFile.Name = "menuLoadFile";
            this.menuLoadFile.Size = new System.Drawing.Size(180, 22);
            this.menuLoadFile.Text = "読込(&O)";
            this.menuLoadFile.Click += new System.EventHandler(this.menuLoadFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 701);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.buttonEmojiTest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "絵文字エディット";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonEmojiTest;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuEditSettings;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuSaveFile;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorVersion;
        private System.Windows.Forms.ToolStripMenuItem menuVersion;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Label labelBody;
        private System.Windows.Forms.CheckBox checkBoxForceInsertLineFeed;
        private EmojiTextBox textBoxMailBody;
        private EmojiTextBox textBoxMailSubject;
        private System.Windows.Forms.Button buttonSelectMailFrom;
        private System.Windows.Forms.Button buttonSelectMailTo;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.TextBox textBoxMailTo;
        private System.Windows.Forms.TextBox textBoxMailFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label lableFrom;
        private EmojiList emojiList;
        private System.Windows.Forms.Button buttonSelAttachments;
        private System.Windows.Forms.TextBox textBoxAttachments;
        private System.Windows.Forms.Label labelAttachments;
        private System.Windows.Forms.ToolStripMenuItem menuLoadFile;
    }
}

