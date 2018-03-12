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
            this.panelContentsBody = new System.Windows.Forms.Panel();
            this.pictureContentsBody = new System.Windows.Forms.PictureBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.contextMenuBody = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuInsertTextBody = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPasteTextBody = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorInsertBody = new System.Windows.Forms.ToolStripSeparator();
            this.menuNewLineBody = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsertEmptyLineBody = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsertCharBody = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorRemoveBody = new System.Windows.Forms.ToolStripSeparator();
            this.menuRemoveLineBody = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveCharBody = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonTest = new System.Windows.Forms.Button();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLoadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorVersion = new System.Windows.Forms.ToolStripSeparator();
            this.menuVersion = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSend = new System.Windows.Forms.Button();
            this.panelContentsSubject = new System.Windows.Forms.Panel();
            this.pictureContentsSubject = new System.Windows.Forms.PictureBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.textBoxMailTo = new System.Windows.Forms.TextBox();
            this.textBoxMailFrom = new System.Windows.Forms.TextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.lableFrom = new System.Windows.Forms.Label();
            this.contextMenuSubject = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuInsertTextSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorInsertSubject = new System.Windows.Forms.ToolStripSeparator();
            this.menuInsertCharSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorRemoveSubject = new System.Windows.Forms.ToolStripSeparator();
            this.menuRemoveCharSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorClearSubject = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearSubject = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonSelectMailTo = new System.Windows.Forms.Button();
            this.buttonSelectMailFrom = new System.Windows.Forms.Button();
            this.panelContentsBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureContentsBody)).BeginInit();
            this.contextMenuBody.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.panelContentsSubject.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureContentsSubject)).BeginInit();
            this.contextMenuSubject.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContentsBody
            // 
            this.panelContentsBody.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContentsBody.AutoScroll = true;
            this.panelContentsBody.Controls.Add(this.pictureContentsBody);
            this.panelContentsBody.Location = new System.Drawing.Point(12, 138);
            this.panelContentsBody.Name = "panelContentsBody";
            this.panelContentsBody.Size = new System.Drawing.Size(430, 304);
            this.panelContentsBody.TabIndex = 9;
            // 
            // pictureContentsBody
            // 
            this.pictureContentsBody.Location = new System.Drawing.Point(0, 0);
            this.pictureContentsBody.Name = "pictureContentsBody";
            this.pictureContentsBody.Size = new System.Drawing.Size(100, 50);
            this.pictureContentsBody.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureContentsBody.TabIndex = 0;
            this.pictureContentsBody.TabStop = false;
            this.pictureContentsBody.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureContentsBody_MouseClick);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(353, 448);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(89, 35);
            this.buttonClear.TabIndex = 12;
            this.buttonClear.Text = "消去(&D)";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // contextMenuBody
            // 
            this.contextMenuBody.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInsertTextBody,
            this.menuPasteTextBody,
            this.toolStripSeparatorInsertBody,
            this.menuNewLineBody,
            this.menuInsertEmptyLineBody,
            this.menuInsertCharBody,
            this.toolStripSeparatorRemoveBody,
            this.menuRemoveLineBody,
            this.menuRemoveCharBody});
            this.contextMenuBody.Name = "contextMenuBody";
            this.contextMenuBody.Size = new System.Drawing.Size(156, 170);
            this.contextMenuBody.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuBody_Opening);
            // 
            // menuInsertTextBody
            // 
            this.menuInsertTextBody.Name = "menuInsertTextBody";
            this.menuInsertTextBody.Size = new System.Drawing.Size(155, 22);
            this.menuInsertTextBody.Text = "文字列挿入(&I)";
            this.menuInsertTextBody.Click += new System.EventHandler(this.menuInsertTextBody_Click);
            // 
            // menuPasteTextBody
            // 
            this.menuPasteTextBody.Name = "menuPasteTextBody";
            this.menuPasteTextBody.Size = new System.Drawing.Size(155, 22);
            this.menuPasteTextBody.Text = "貼り付け挿入(&V)";
            this.menuPasteTextBody.Click += new System.EventHandler(this.menuPasteTextBody_Click);
            // 
            // toolStripSeparatorInsertBody
            // 
            this.toolStripSeparatorInsertBody.Name = "toolStripSeparatorInsertBody";
            this.toolStripSeparatorInsertBody.Size = new System.Drawing.Size(152, 6);
            // 
            // menuNewLineBody
            // 
            this.menuNewLineBody.Name = "menuNewLineBody";
            this.menuNewLineBody.Size = new System.Drawing.Size(155, 22);
            this.menuNewLineBody.Text = "改行(&N)";
            this.menuNewLineBody.Click += new System.EventHandler(this.menuNewLineBody_Click);
            // 
            // menuInsertEmptyLineBody
            // 
            this.menuInsertEmptyLineBody.Name = "menuInsertEmptyLineBody";
            this.menuInsertEmptyLineBody.Size = new System.Drawing.Size(155, 22);
            this.menuInsertEmptyLineBody.Text = "空行挿入(&R)";
            this.menuInsertEmptyLineBody.Click += new System.EventHandler(this.menuInsertEmptyLineBody_Click);
            // 
            // menuInsertCharBody
            // 
            this.menuInsertCharBody.Name = "menuInsertCharBody";
            this.menuInsertCharBody.Size = new System.Drawing.Size(155, 22);
            this.menuInsertCharBody.Text = "空1文字挿入(&C)";
            this.menuInsertCharBody.Click += new System.EventHandler(this.menuInsertCharBody_Click);
            // 
            // toolStripSeparatorRemoveBody
            // 
            this.toolStripSeparatorRemoveBody.Name = "toolStripSeparatorRemoveBody";
            this.toolStripSeparatorRemoveBody.Size = new System.Drawing.Size(152, 6);
            // 
            // menuRemoveLineBody
            // 
            this.menuRemoveLineBody.Name = "menuRemoveLineBody";
            this.menuRemoveLineBody.Size = new System.Drawing.Size(155, 22);
            this.menuRemoveLineBody.Text = "行削除(&L)";
            this.menuRemoveLineBody.Click += new System.EventHandler(this.menuRemoveLineBody_Click);
            // 
            // menuRemoveCharBody
            // 
            this.menuRemoveCharBody.Name = "menuRemoveCharBody";
            this.menuRemoveCharBody.Size = new System.Drawing.Size(155, 22);
            this.menuRemoveCharBody.Text = "1文字削除(&D)";
            this.menuRemoveCharBody.Click += new System.EventHandler(this.menuRemoveCharBody_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest.Location = new System.Drawing.Point(258, 448);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(89, 35);
            this.buttonTest.TabIndex = 11;
            this.buttonTest.Text = "テスト(&T)";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuSettings});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(454, 24);
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
            // menuLoadFile
            // 
            this.menuLoadFile.Name = "menuLoadFile";
            this.menuLoadFile.Size = new System.Drawing.Size(115, 22);
            this.menuLoadFile.Text = "読込(&O)";
            this.menuLoadFile.Click += new System.EventHandler(this.menuLoadFile_Click);
            // 
            // menuSaveFile
            // 
            this.menuSaveFile.Name = "menuSaveFile";
            this.menuSaveFile.Size = new System.Drawing.Size(115, 22);
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
            this.menuSettings.Size = new System.Drawing.Size(57, 20);
            this.menuSettings.Text = "設定(&S)";
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
            this.buttonSend.Location = new System.Drawing.Point(12, 448);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(89, 35);
            this.buttonSend.TabIndex = 10;
            this.buttonSend.Text = "送信(&M)";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // panelContentsSubject
            // 
            this.panelContentsSubject.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContentsSubject.AutoScroll = true;
            this.panelContentsSubject.BackColor = System.Drawing.SystemColors.Control;
            this.panelContentsSubject.Controls.Add(this.pictureContentsSubject);
            this.panelContentsSubject.Location = new System.Drawing.Point(76, 80);
            this.panelContentsSubject.Name = "panelContentsSubject";
            this.panelContentsSubject.Size = new System.Drawing.Size(366, 52);
            this.panelContentsSubject.TabIndex = 8;
            // 
            // pictureContentsSubject
            // 
            this.pictureContentsSubject.Location = new System.Drawing.Point(0, 0);
            this.pictureContentsSubject.Name = "pictureContentsSubject";
            this.pictureContentsSubject.Size = new System.Drawing.Size(100, 20);
            this.pictureContentsSubject.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureContentsSubject.TabIndex = 0;
            this.pictureContentsSubject.TabStop = false;
            this.pictureContentsSubject.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureContentsSubject_MouseClick);
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(12, 82);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(46, 12);
            this.labelSubject.TabIndex = 7;
            this.labelSubject.Text = "件名(&T):";
            // 
            // textBoxMailTo
            // 
            this.textBoxMailTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailTo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxMailTo.Location = new System.Drawing.Point(76, 30);
            this.textBoxMailTo.Name = "textBoxMailTo";
            this.textBoxMailTo.Size = new System.Drawing.Size(309, 19);
            this.textBoxMailTo.TabIndex = 2;
            this.textBoxMailTo.Tag = "宛先";
            this.textBoxMailTo.Leave += new System.EventHandler(this.textBoxToHankaku_Leave);
            // 
            // textBoxMailFrom
            // 
            this.textBoxMailFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailFrom.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxMailFrom.Location = new System.Drawing.Point(76, 55);
            this.textBoxMailFrom.Name = "textBoxMailFrom";
            this.textBoxMailFrom.Size = new System.Drawing.Size(309, 19);
            this.textBoxMailFrom.TabIndex = 5;
            this.textBoxMailFrom.Tag = "送信元";
            this.textBoxMailFrom.Leave += new System.EventHandler(this.textBoxToHankaku_Leave);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(10, 33);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(47, 12);
            this.labelTo.TabIndex = 1;
            this.labelTo.Text = "宛先(&O):";
            // 
            // lableFrom
            // 
            this.lableFrom.AutoSize = true;
            this.lableFrom.Location = new System.Drawing.Point(10, 58);
            this.lableFrom.Name = "lableFrom";
            this.lableFrom.Size = new System.Drawing.Size(58, 12);
            this.lableFrom.TabIndex = 4;
            this.lableFrom.Text = "送信元(&F):";
            // 
            // contextMenuSubject
            // 
            this.contextMenuSubject.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInsertTextSubject,
            this.toolStripSeparatorInsertSubject,
            this.menuInsertCharSubject,
            this.toolStripSeparatorRemoveSubject,
            this.menuRemoveCharSubject,
            this.toolStripSeparatorClearSubject,
            this.menuClearSubject});
            this.contextMenuSubject.Name = "contextMenu";
            this.contextMenuSubject.Size = new System.Drawing.Size(156, 110);
            // 
            // menuInsertTextSubject
            // 
            this.menuInsertTextSubject.Name = "menuInsertTextSubject";
            this.menuInsertTextSubject.Size = new System.Drawing.Size(155, 22);
            this.menuInsertTextSubject.Text = "文字列挿入(&I)";
            this.menuInsertTextSubject.Click += new System.EventHandler(this.menuInsertTextSubject_Click);
            // 
            // toolStripSeparatorInsertSubject
            // 
            this.toolStripSeparatorInsertSubject.Name = "toolStripSeparatorInsertSubject";
            this.toolStripSeparatorInsertSubject.Size = new System.Drawing.Size(152, 6);
            // 
            // menuInsertCharSubject
            // 
            this.menuInsertCharSubject.Name = "menuInsertCharSubject";
            this.menuInsertCharSubject.Size = new System.Drawing.Size(155, 22);
            this.menuInsertCharSubject.Text = "空1文字挿入(&C)";
            this.menuInsertCharSubject.Click += new System.EventHandler(this.menuInsertCharSubject_Click);
            // 
            // toolStripSeparatorRemoveSubject
            // 
            this.toolStripSeparatorRemoveSubject.Name = "toolStripSeparatorRemoveSubject";
            this.toolStripSeparatorRemoveSubject.Size = new System.Drawing.Size(152, 6);
            // 
            // menuRemoveCharSubject
            // 
            this.menuRemoveCharSubject.Name = "menuRemoveCharSubject";
            this.menuRemoveCharSubject.Size = new System.Drawing.Size(155, 22);
            this.menuRemoveCharSubject.Text = "1文字削除(&D)";
            this.menuRemoveCharSubject.Click += new System.EventHandler(this.menuRemoveCharSubject_Click);
            // 
            // toolStripSeparatorClearSubject
            // 
            this.toolStripSeparatorClearSubject.Name = "toolStripSeparatorClearSubject";
            this.toolStripSeparatorClearSubject.Size = new System.Drawing.Size(152, 6);
            // 
            // menuClearSubject
            // 
            this.menuClearSubject.Name = "menuClearSubject";
            this.menuClearSubject.Size = new System.Drawing.Size(155, 22);
            this.menuClearSubject.Text = "消去(&L)";
            this.menuClearSubject.Click += new System.EventHandler(this.menuClearSubject_Click);
            // 
            // buttonSelectMailTo
            // 
            this.buttonSelectMailTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectMailTo.Location = new System.Drawing.Point(386, 28);
            this.buttonSelectMailTo.Name = "buttonSelectMailTo";
            this.buttonSelectMailTo.Size = new System.Drawing.Size(56, 23);
            this.buttonSelectMailTo.TabIndex = 3;
            this.buttonSelectMailTo.Text = "選択";
            this.buttonSelectMailTo.UseVisualStyleBackColor = true;
            this.buttonSelectMailTo.Click += new System.EventHandler(this.buttonSelectMailTo_Click);
            // 
            // buttonSelectMailFrom
            // 
            this.buttonSelectMailFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectMailFrom.Location = new System.Drawing.Point(386, 53);
            this.buttonSelectMailFrom.Name = "buttonSelectMailFrom";
            this.buttonSelectMailFrom.Size = new System.Drawing.Size(56, 23);
            this.buttonSelectMailFrom.TabIndex = 6;
            this.buttonSelectMailFrom.Text = "選択";
            this.buttonSelectMailFrom.UseVisualStyleBackColor = true;
            this.buttonSelectMailFrom.Click += new System.EventHandler(this.buttonSelectMailFrom_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 491);
            this.Controls.Add(this.buttonSelectMailFrom);
            this.Controls.Add(this.buttonSelectMailTo);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.panelContentsSubject);
            this.Controls.Add(this.labelSubject);
            this.Controls.Add(this.textBoxMailTo);
            this.Controls.Add(this.textBoxMailFrom);
            this.Controls.Add(this.labelTo);
            this.Controls.Add(this.lableFrom);
            this.Controls.Add(this.mainMenu);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.panelContentsBody);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "絵文字エディット";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.panelContentsBody.ResumeLayout(false);
            this.panelContentsBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureContentsBody)).EndInit();
            this.contextMenuBody.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.panelContentsSubject.ResumeLayout(false);
            this.panelContentsSubject.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureContentsSubject)).EndInit();
            this.contextMenuSubject.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panelContentsBody;
        private System.Windows.Forms.PictureBox pictureContentsBody;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.ContextMenuStrip contextMenuBody;
        private System.Windows.Forms.ToolStripMenuItem menuInsertEmptyLineBody;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveLineBody;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.ToolStripMenuItem menuInsertCharBody;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveCharBody;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRemoveBody;
        private System.Windows.Forms.ToolStripMenuItem menuInsertTextBody;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorInsertBody;
        private System.Windows.Forms.ToolStripMenuItem menuNewLineBody;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuEditSettings;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuLoadFile;
        private System.Windows.Forms.ToolStripMenuItem menuSaveFile;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ToolStripMenuItem menuPasteTextBody;
        private System.Windows.Forms.Panel panelContentsSubject;
        private System.Windows.Forms.PictureBox pictureContentsSubject;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.TextBox textBoxMailTo;
        private System.Windows.Forms.TextBox textBoxMailFrom;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.Label lableFrom;
        private System.Windows.Forms.ContextMenuStrip contextMenuSubject;
        private System.Windows.Forms.ToolStripMenuItem menuInsertTextSubject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorInsertSubject;
        private System.Windows.Forms.ToolStripMenuItem menuInsertCharSubject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRemoveSubject;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveCharSubject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorClearSubject;
        private System.Windows.Forms.ToolStripMenuItem menuClearSubject;
        private System.Windows.Forms.Button buttonSelectMailTo;
        private System.Windows.Forms.Button buttonSelectMailFrom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorVersion;
        private System.Windows.Forms.ToolStripMenuItem menuVersion;
    }
}

