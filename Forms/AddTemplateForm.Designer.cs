namespace emojiEdit
{
    partial class AddTemplateForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTemplateForm));
            this.buttonAddTemplate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.pictureTemplate = new System.Windows.Forms.PictureBox();
            this.contextMenuTemplate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuInsertTextTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPasteTextTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorInsertBody = new System.Windows.Forms.ToolStripSeparator();
            this.menuNewLineTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsertEmptyLineTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInsertCharTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorRemoveBody = new System.Windows.Forms.ToolStripSeparator();
            this.menuRemoveLineTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveCharTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxTemplate = new emojiEdit.EmojiTextBox();
            this.labelPanelTemplate = new System.Windows.Forms.Label();
            this.labelTextBoxTemplate = new System.Windows.Forms.Label();
            this.panelTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).BeginInit();
            this.contextMenuTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddTemplate
            // 
            this.buttonAddTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAddTemplate.Location = new System.Drawing.Point(12, 319);
            this.buttonAddTemplate.Name = "buttonAddTemplate";
            this.buttonAddTemplate.Size = new System.Drawing.Size(90, 33);
            this.buttonAddTemplate.TabIndex = 4;
            this.buttonAddTemplate.Text = "追加(&S)";
            this.buttonAddTemplate.UseVisualStyleBackColor = true;
            this.buttonAddTemplate.Click += new System.EventHandler(this.buttonAddTemplate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(332, 319);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 33);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panelTemplate
            // 
            this.panelTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTemplate.AutoScroll = true;
            this.panelTemplate.BackColor = System.Drawing.Color.White;
            this.panelTemplate.Controls.Add(this.pictureTemplate);
            this.panelTemplate.Location = new System.Drawing.Point(12, 24);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(410, 108);
            this.panelTemplate.TabIndex = 1;
            // 
            // pictureTemplate
            // 
            this.pictureTemplate.Location = new System.Drawing.Point(0, 0);
            this.pictureTemplate.Name = "pictureTemplate";
            this.pictureTemplate.Size = new System.Drawing.Size(100, 20);
            this.pictureTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureTemplate.TabIndex = 0;
            this.pictureTemplate.TabStop = false;
            // 
            // contextMenuTemplate
            // 
            this.contextMenuTemplate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuInsertTextTemplate,
            this.menuPasteTextTemplate,
            this.toolStripSeparatorInsertBody,
            this.menuNewLineTemplate,
            this.menuInsertEmptyLineTemplate,
            this.menuInsertCharTemplate,
            this.toolStripSeparatorRemoveBody,
            this.menuRemoveLineTemplate,
            this.menuRemoveCharTemplate});
            this.contextMenuTemplate.Name = "contextMenuBody";
            this.contextMenuTemplate.Size = new System.Drawing.Size(155, 170);
            // 
            // menuInsertTextTemplate
            // 
            this.menuInsertTextTemplate.Name = "menuInsertTextTemplate";
            this.menuInsertTextTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertTextTemplate.Text = "文字列挿入(&I)";
            // 
            // menuPasteTextTemplate
            // 
            this.menuPasteTextTemplate.Name = "menuPasteTextTemplate";
            this.menuPasteTextTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuPasteTextTemplate.Text = "貼り付け挿入(&V)";
            // 
            // toolStripSeparatorInsertBody
            // 
            this.toolStripSeparatorInsertBody.Name = "toolStripSeparatorInsertBody";
            this.toolStripSeparatorInsertBody.Size = new System.Drawing.Size(151, 6);
            // 
            // menuNewLineTemplate
            // 
            this.menuNewLineTemplate.Name = "menuNewLineTemplate";
            this.menuNewLineTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuNewLineTemplate.Text = "改行(&N)";
            // 
            // menuInsertEmptyLineTemplate
            // 
            this.menuInsertEmptyLineTemplate.Name = "menuInsertEmptyLineTemplate";
            this.menuInsertEmptyLineTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertEmptyLineTemplate.Text = "空行挿入(&R)";
            // 
            // menuInsertCharTemplate
            // 
            this.menuInsertCharTemplate.Name = "menuInsertCharTemplate";
            this.menuInsertCharTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertCharTemplate.Text = "空文字挿入(&C)";
            // 
            // toolStripSeparatorRemoveBody
            // 
            this.toolStripSeparatorRemoveBody.Name = "toolStripSeparatorRemoveBody";
            this.toolStripSeparatorRemoveBody.Size = new System.Drawing.Size(151, 6);
            // 
            // menuRemoveLineTemplate
            // 
            this.menuRemoveLineTemplate.Name = "menuRemoveLineTemplate";
            this.menuRemoveLineTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuRemoveLineTemplate.Text = "行削除(&L)";
            // 
            // menuRemoveCharTemplate
            // 
            this.menuRemoveCharTemplate.Name = "menuRemoveCharTemplate";
            this.menuRemoveCharTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuRemoveCharTemplate.Text = "文字削除(&D)";
            // 
            // textBoxTemplate
            // 
            this.textBoxTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTemplate.BackColor = System.Drawing.Color.White;
            this.textBoxTemplate.ColumnLine = 0;
            this.textBoxTemplate.Location = new System.Drawing.Point(12, 150);
            this.textBoxTemplate.Multiline = true;
            this.textBoxTemplate.Name = "textBoxTemplate";
            this.textBoxTemplate.ReadOnly = true;
            this.textBoxTemplate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxTemplate.Size = new System.Drawing.Size(410, 163);
            this.textBoxTemplate.TabIndex = 3;
            // 
            // labelPanelTemplate
            // 
            this.labelPanelTemplate.AutoSize = true;
            this.labelPanelTemplate.Location = new System.Drawing.Point(12, 9);
            this.labelPanelTemplate.Name = "labelPanelTemplate";
            this.labelPanelTemplate.Size = new System.Drawing.Size(132, 12);
            this.labelPanelTemplate.TabIndex = 0;
            this.labelPanelTemplate.Text = "登録されるサムネイル画像:";
            // 
            // labelTextBoxTemplate
            // 
            this.labelTextBoxTemplate.AutoSize = true;
            this.labelTextBoxTemplate.Location = new System.Drawing.Point(12, 135);
            this.labelTextBoxTemplate.Name = "labelTextBoxTemplate";
            this.labelTextBoxTemplate.Size = new System.Drawing.Size(95, 12);
            this.labelTextBoxTemplate.TabIndex = 2;
            this.labelTextBoxTemplate.Text = "登録される文字列:";
            // 
            // AddTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 361);
            this.Controls.Add(this.labelTextBoxTemplate);
            this.Controls.Add(this.labelPanelTemplate);
            this.Controls.Add(this.textBoxTemplate);
            this.Controls.Add(this.panelTemplate);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAddTemplate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 400);
            this.Name = "AddTemplateForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "テンプレート追加";
            this.Load += new System.EventHandler(this.AddTemplateForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddTemplateForm_KeyDown);
            this.panelTemplate.ResumeLayout(false);
            this.panelTemplate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).EndInit();
            this.contextMenuTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAddTemplate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panelTemplate;
        private System.Windows.Forms.PictureBox pictureTemplate;
        private System.Windows.Forms.ContextMenuStrip contextMenuTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuInsertTextTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuPasteTextTemplate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorInsertBody;
        private System.Windows.Forms.ToolStripMenuItem menuNewLineTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuInsertEmptyLineTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuInsertCharTemplate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRemoveBody;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveLineTemplate;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveCharTemplate;
        private EmojiTextBox textBoxTemplate;
        private System.Windows.Forms.Label labelPanelTemplate;
        private System.Windows.Forms.Label labelTextBoxTemplate;
    }
}