namespace emojiEdit
{
    partial class TemplateInputForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemplateInputForm));
            this.buttonSetTemplate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelDivider = new System.Windows.Forms.Label();
            this.buttonSelectFromList = new System.Windows.Forms.Button();
            this.panelTemplate = new System.Windows.Forms.Panel();
            this.pictureTemplate = new System.Windows.Forms.PictureBox();
            this.numericUpDownCols = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRows = new System.Windows.Forms.NumericUpDown();
            this.labelCols = new System.Windows.Forms.Label();
            this.labelRows = new System.Windows.Forms.Label();
            this.buttonUpdateTemplateRegion = new System.Windows.Forms.Button();
            this.panelTemplateList = new System.Windows.Forms.Panel();
            this.pictureTemplateList = new System.Windows.Forms.PictureBox();
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
            this.buttonClearTemplateRegion = new System.Windows.Forms.Button();
            this.panelTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRows)).BeginInit();
            this.panelTemplateList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplateList)).BeginInit();
            this.contextMenuTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSetTemplate
            // 
            this.buttonSetTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetTemplate.Location = new System.Drawing.Point(12, 369);
            this.buttonSetTemplate.Name = "buttonSetTemplate";
            this.buttonSetTemplate.Size = new System.Drawing.Size(118, 33);
            this.buttonSetTemplate.TabIndex = 12;
            this.buttonSetTemplate.Text = "設定(&S)";
            this.buttonSetTemplate.UseVisualStyleBackColor = true;
            this.buttonSetTemplate.Click += new System.EventHandler(this.buttonSetTemplate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(404, 369);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(118, 33);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonInsert
            // 
            this.buttonInsert.Location = new System.Drawing.Point(124, 153);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(106, 25);
            this.buttonInsert.TabIndex = 2;
            this.buttonInsert.Text = "一覧へ追加(&I)";
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(416, 153);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(106, 25);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "削除(&D)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelDivider
            // 
            this.labelDivider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDivider.Location = new System.Drawing.Point(12, 360);
            this.labelDivider.Name = "labelDivider";
            this.labelDivider.Size = new System.Drawing.Size(510, 2);
            this.labelDivider.TabIndex = 11;
            // 
            // buttonSelectFromList
            // 
            this.buttonSelectFromList.Location = new System.Drawing.Point(12, 153);
            this.buttonSelectFromList.Name = "buttonSelectFromList";
            this.buttonSelectFromList.Size = new System.Drawing.Size(106, 25);
            this.buttonSelectFromList.TabIndex = 1;
            this.buttonSelectFromList.Text = "一覧から取得(&G)";
            this.buttonSelectFromList.UseVisualStyleBackColor = true;
            this.buttonSelectFromList.Click += new System.EventHandler(this.buttonSelectFromList_Click);
            // 
            // panelTemplate
            // 
            this.panelTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTemplate.AutoScroll = true;
            this.panelTemplate.BackColor = System.Drawing.SystemColors.Control;
            this.panelTemplate.Controls.Add(this.pictureTemplate);
            this.panelTemplate.Location = new System.Drawing.Point(12, 215);
            this.panelTemplate.Name = "panelTemplate";
            this.panelTemplate.Size = new System.Drawing.Size(510, 142);
            this.panelTemplate.TabIndex = 10;
            // 
            // pictureTemplate
            // 
            this.pictureTemplate.Location = new System.Drawing.Point(0, 0);
            this.pictureTemplate.Name = "pictureTemplate";
            this.pictureTemplate.Size = new System.Drawing.Size(100, 20);
            this.pictureTemplate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureTemplate.TabIndex = 0;
            this.pictureTemplate.TabStop = false;
            this.pictureTemplate.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureTemplate_MouseClick);
            // 
            // numericUpDownCols
            // 
            this.numericUpDownCols.Location = new System.Drawing.Point(77, 188);
            this.numericUpDownCols.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownCols.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownCols.Name = "numericUpDownCols";
            this.numericUpDownCols.Size = new System.Drawing.Size(43, 19);
            this.numericUpDownCols.TabIndex = 5;
            this.numericUpDownCols.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // numericUpDownRows
            // 
            this.numericUpDownRows.Location = new System.Drawing.Point(179, 188);
            this.numericUpDownRows.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownRows.Name = "numericUpDownRows";
            this.numericUpDownRows.Size = new System.Drawing.Size(43, 19);
            this.numericUpDownRows.TabIndex = 7;
            this.numericUpDownRows.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // labelCols
            // 
            this.labelCols.AutoSize = true;
            this.labelCols.Location = new System.Drawing.Point(12, 190);
            this.labelCols.Name = "labelCols";
            this.labelCols.Size = new System.Drawing.Size(59, 12);
            this.labelCols.TabIndex = 4;
            this.labelCols.Text = "文字数(&O):";
            // 
            // labelRows
            // 
            this.labelRows.AutoSize = true;
            this.labelRows.Location = new System.Drawing.Point(126, 190);
            this.labelRows.Name = "labelRows";
            this.labelRows.Size = new System.Drawing.Size(47, 12);
            this.labelRows.TabIndex = 6;
            this.labelRows.Text = "行数(&R):";
            // 
            // buttonUpdateTemplateRegion
            // 
            this.buttonUpdateTemplateRegion.Location = new System.Drawing.Point(228, 184);
            this.buttonUpdateTemplateRegion.Name = "buttonUpdateTemplateRegion";
            this.buttonUpdateTemplateRegion.Size = new System.Drawing.Size(84, 25);
            this.buttonUpdateTemplateRegion.TabIndex = 8;
            this.buttonUpdateTemplateRegion.Text = "更新(&E)";
            this.buttonUpdateTemplateRegion.UseVisualStyleBackColor = true;
            this.buttonUpdateTemplateRegion.Click += new System.EventHandler(this.buttonUpdateTemplateRegion_Click);
            // 
            // panelTemplateList
            // 
            this.panelTemplateList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTemplateList.AutoScroll = true;
            this.panelTemplateList.BackColor = System.Drawing.Color.White;
            this.panelTemplateList.Controls.Add(this.pictureTemplateList);
            this.panelTemplateList.Location = new System.Drawing.Point(12, 12);
            this.panelTemplateList.Name = "panelTemplateList";
            this.panelTemplateList.Size = new System.Drawing.Size(510, 135);
            this.panelTemplateList.TabIndex = 0;
            this.panelTemplateList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelTemplateList_MouseClick);
            // 
            // pictureTemplateList
            // 
            this.pictureTemplateList.Location = new System.Drawing.Point(0, 0);
            this.pictureTemplateList.Name = "pictureTemplateList";
            this.pictureTemplateList.Size = new System.Drawing.Size(100, 20);
            this.pictureTemplateList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureTemplateList.TabIndex = 0;
            this.pictureTemplateList.TabStop = false;
            this.pictureTemplateList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureTemplateList_MouseClick);
            this.pictureTemplateList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureTemplateList_MouseDoubleClick);
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
            this.contextMenuTemplate.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuTemplate_Opening);
            // 
            // menuInsertTextTemplate
            // 
            this.menuInsertTextTemplate.Name = "menuInsertTextTemplate";
            this.menuInsertTextTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertTextTemplate.Text = "文字列挿入(&I)";
            this.menuInsertTextTemplate.Click += new System.EventHandler(this.menuInsertTextTemplate_Click);
            // 
            // menuPasteTextTemplate
            // 
            this.menuPasteTextTemplate.Name = "menuPasteTextTemplate";
            this.menuPasteTextTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuPasteTextTemplate.Text = "貼り付け挿入(&V)";
            this.menuPasteTextTemplate.Click += new System.EventHandler(this.menuPasteTextTemplate_Click);
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
            this.menuNewLineTemplate.Click += new System.EventHandler(this.menuNewLineTemplate_Click);
            // 
            // menuInsertEmptyLineTemplate
            // 
            this.menuInsertEmptyLineTemplate.Name = "menuInsertEmptyLineTemplate";
            this.menuInsertEmptyLineTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertEmptyLineTemplate.Text = "空行挿入(&R)";
            this.menuInsertEmptyLineTemplate.Click += new System.EventHandler(this.menuInsertEmptyLineTemplate_Click);
            // 
            // menuInsertCharTemplate
            // 
            this.menuInsertCharTemplate.Name = "menuInsertCharTemplate";
            this.menuInsertCharTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuInsertCharTemplate.Text = "空文字挿入(&C)";
            this.menuInsertCharTemplate.Click += new System.EventHandler(this.menuInsertCharTemplate_Click);
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
            this.menuRemoveLineTemplate.Click += new System.EventHandler(this.menuRemoveLineTemplate_Click);
            // 
            // menuRemoveCharTemplate
            // 
            this.menuRemoveCharTemplate.Name = "menuRemoveCharTemplate";
            this.menuRemoveCharTemplate.Size = new System.Drawing.Size(154, 22);
            this.menuRemoveCharTemplate.Text = "文字削除(&D)";
            this.menuRemoveCharTemplate.Click += new System.EventHandler(this.menuRemoveCharTemplate_Click);
            // 
            // buttonClearTemplateRegion
            // 
            this.buttonClearTemplateRegion.Location = new System.Drawing.Point(318, 184);
            this.buttonClearTemplateRegion.Name = "buttonClearTemplateRegion";
            this.buttonClearTemplateRegion.Size = new System.Drawing.Size(84, 25);
            this.buttonClearTemplateRegion.TabIndex = 9;
            this.buttonClearTemplateRegion.Text = "消去(&L)";
            this.buttonClearTemplateRegion.UseVisualStyleBackColor = true;
            this.buttonClearTemplateRegion.Click += new System.EventHandler(this.buttonClearTemplateRegion_Click);
            // 
            // TemplateInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.Controls.Add(this.buttonClearTemplateRegion);
            this.Controls.Add(this.panelTemplateList);
            this.Controls.Add(this.buttonUpdateTemplateRegion);
            this.Controls.Add(this.labelRows);
            this.Controls.Add(this.labelCols);
            this.Controls.Add(this.numericUpDownRows);
            this.Controls.Add(this.numericUpDownCols);
            this.Controls.Add(this.panelTemplate);
            this.Controls.Add(this.buttonSelectFromList);
            this.Controls.Add(this.labelDivider);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonInsert);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSetTemplate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 450);
            this.Name = "TemplateInputForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "テンプレート";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TemplateInputForm_FormClosed);
            this.Load += new System.EventHandler(this.TemplateInputForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TemplateInputForm_KeyDown);
            this.panelTemplate.ResumeLayout(false);
            this.panelTemplate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRows)).EndInit();
            this.panelTemplateList.ResumeLayout(false);
            this.panelTemplateList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplateList)).EndInit();
            this.contextMenuTemplate.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSetTemplate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelDivider;
        private System.Windows.Forms.Button buttonSelectFromList;
        private System.Windows.Forms.Panel panelTemplate;
        private System.Windows.Forms.PictureBox pictureTemplate;
        private System.Windows.Forms.NumericUpDown numericUpDownCols;
        private System.Windows.Forms.NumericUpDown numericUpDownRows;
        private System.Windows.Forms.Label labelCols;
        private System.Windows.Forms.Label labelRows;
        private System.Windows.Forms.Button buttonUpdateTemplateRegion;
        private System.Windows.Forms.Panel panelTemplateList;
        private System.Windows.Forms.PictureBox pictureTemplateList;
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
        private System.Windows.Forms.Button buttonClearTemplateRegion;
    }
}