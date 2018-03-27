namespace emojiEdit
{
    partial class SelectTemplateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTemplateForm));
            this.buttonSelectTemplate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.panelTemplateList = new System.Windows.Forms.Panel();
            this.pictureTemplateList = new System.Windows.Forms.PictureBox();
            this.panelTemplateList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplateList)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelectTemplate
            // 
            this.buttonSelectTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectTemplate.Location = new System.Drawing.Point(12, 369);
            this.buttonSelectTemplate.Name = "buttonSelectTemplate";
            this.buttonSelectTemplate.Size = new System.Drawing.Size(90, 33);
            this.buttonSelectTemplate.TabIndex = 2;
            this.buttonSelectTemplate.Text = "設定(&S)";
            this.buttonSelectTemplate.UseVisualStyleBackColor = true;
            this.buttonSelectTemplate.Click += new System.EventHandler(this.buttonSelectTemplate_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(432, 369);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 33);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(416, 338);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(106, 25);
            this.buttonDelete.TabIndex = 1;
            this.buttonDelete.Text = "削除(&D)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // panelTemplateList
            // 
            this.panelTemplateList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTemplateList.AutoScroll = true;
            this.panelTemplateList.BackColor = System.Drawing.Color.White;
            this.panelTemplateList.Controls.Add(this.pictureTemplateList);
            this.panelTemplateList.Location = new System.Drawing.Point(12, 12);
            this.panelTemplateList.Name = "panelTemplateList";
            this.panelTemplateList.Size = new System.Drawing.Size(510, 320);
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
            // SelectTemplateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.Controls.Add(this.panelTemplateList);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSelectTemplate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 450);
            this.Name = "SelectTemplateForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "テンプレート";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectTemplateForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SelectTemplateForm_KeyDown);
            this.panelTemplateList.ResumeLayout(false);
            this.panelTemplateList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTemplateList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonSelectTemplate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Panel panelTemplateList;
        private System.Windows.Forms.PictureBox pictureTemplateList;
    }
}