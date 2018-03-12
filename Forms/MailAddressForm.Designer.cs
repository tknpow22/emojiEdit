namespace emojiEdit
{
    partial class MailAddressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailAddressForm));
            this.buttonSetAddr = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listViewMailAddress = new System.Windows.Forms.ListView();
            this.columnMailAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNote = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonUpsert = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.labelMailAddr = new System.Windows.Forms.Label();
            this.textBoxMailAddr = new System.Windows.Forms.TextBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.labelDivider = new System.Windows.Forms.Label();
            this.buttonSelectFromList = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSetAddr
            // 
            this.buttonSetAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSetAddr.Location = new System.Drawing.Point(12, 319);
            this.buttonSetAddr.Name = "buttonSetAddr";
            this.buttonSetAddr.Size = new System.Drawing.Size(118, 33);
            this.buttonSetAddr.TabIndex = 9;
            this.buttonSetAddr.Text = "設定(&S)";
            this.buttonSetAddr.UseVisualStyleBackColor = true;
            this.buttonSetAddr.Click += new System.EventHandler(this.buttonSetAddr_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(404, 319);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(118, 33);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // listViewMailAddress
            // 
            this.listViewMailAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewMailAddress.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnMailAddr,
            this.columnNote});
            this.listViewMailAddress.FullRowSelect = true;
            this.listViewMailAddress.HideSelection = false;
            this.listViewMailAddress.Location = new System.Drawing.Point(12, 12);
            this.listViewMailAddress.MultiSelect = false;
            this.listViewMailAddress.Name = "listViewMailAddress";
            this.listViewMailAddress.Size = new System.Drawing.Size(510, 209);
            this.listViewMailAddress.TabIndex = 0;
            this.listViewMailAddress.UseCompatibleStateImageBehavior = false;
            this.listViewMailAddress.View = System.Windows.Forms.View.Details;
            this.listViewMailAddress.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewMailAddress_ColumnClick);
            this.listViewMailAddress.SelectedIndexChanged += new System.EventHandler(this.listViewMailAddress_SelectedIndexChanged);
            this.listViewMailAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewMailAddress_KeyDown);
            this.listViewMailAddress.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listViewMailAddress_MouseDoubleClick);
            // 
            // columnMailAddr
            // 
            this.columnMailAddr.Text = "メールアドレス";
            this.columnMailAddr.Width = 276;
            // 
            // columnNote
            // 
            this.columnNote.Text = "備考";
            this.columnNote.Width = 188;
            // 
            // buttonUpsert
            // 
            this.buttonUpsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonUpsert.Location = new System.Drawing.Point(124, 227);
            this.buttonUpsert.Name = "buttonUpsert";
            this.buttonUpsert.Size = new System.Drawing.Size(106, 25);
            this.buttonUpsert.TabIndex = 2;
            this.buttonUpsert.Text = "追加・更新(&U)";
            this.buttonUpsert.UseVisualStyleBackColor = true;
            this.buttonUpsert.Click += new System.EventHandler(this.buttonUpsert_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Location = new System.Drawing.Point(416, 227);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(106, 25);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "削除(&D)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // labelMailAddr
            // 
            this.labelMailAddr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMailAddr.AutoSize = true;
            this.labelMailAddr.Location = new System.Drawing.Point(10, 261);
            this.labelMailAddr.Name = "labelMailAddr";
            this.labelMailAddr.Size = new System.Drawing.Size(88, 12);
            this.labelMailAddr.TabIndex = 4;
            this.labelMailAddr.Text = "メールアドレス(&M):";
            // 
            // textBoxMailAddr
            // 
            this.textBoxMailAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMailAddr.Location = new System.Drawing.Point(104, 258);
            this.textBoxMailAddr.Name = "textBoxMailAddr";
            this.textBoxMailAddr.Size = new System.Drawing.Size(418, 19);
            this.textBoxMailAddr.TabIndex = 5;
            this.textBoxMailAddr.Tag = "メールアドレス";
            this.textBoxMailAddr.TextChanged += new System.EventHandler(this.textBoxMailAddr_TextChanged);
            // 
            // labelNote
            // 
            this.labelNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(10, 286);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(47, 12);
            this.labelNote.TabIndex = 6;
            this.labelNote.Text = "備考(&N):";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNote.Location = new System.Drawing.Point(104, 283);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(418, 19);
            this.textBoxNote.TabIndex = 7;
            this.textBoxNote.Tag = "備考";
            // 
            // labelDivider
            // 
            this.labelDivider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelDivider.Location = new System.Drawing.Point(12, 310);
            this.labelDivider.Name = "labelDivider";
            this.labelDivider.Size = new System.Drawing.Size(510, 2);
            this.labelDivider.TabIndex = 8;
            // 
            // buttonSelectFromList
            // 
            this.buttonSelectFromList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectFromList.Location = new System.Drawing.Point(12, 227);
            this.buttonSelectFromList.Name = "buttonSelectFromList";
            this.buttonSelectFromList.Size = new System.Drawing.Size(106, 25);
            this.buttonSelectFromList.TabIndex = 1;
            this.buttonSelectFromList.Text = "一覧から取得(&G)";
            this.buttonSelectFromList.UseVisualStyleBackColor = true;
            this.buttonSelectFromList.Click += new System.EventHandler(this.buttonSelectFromList_Click);
            // 
            // MailAddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 361);
            this.Controls.Add(this.buttonSelectFromList);
            this.Controls.Add(this.labelDivider);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.textBoxMailAddr);
            this.Controls.Add(this.labelMailAddr);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonUpsert);
            this.Controls.Add(this.listViewMailAddress);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSetAddr);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 400);
            this.Name = "MailAddressForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "メールアドレス";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MailAddressForm_FormClosed);
            this.Load += new System.EventHandler(this.MailAddressForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MailAddressForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSetAddr;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listViewMailAddress;
        private System.Windows.Forms.ColumnHeader columnMailAddr;
        private System.Windows.Forms.ColumnHeader columnNote;
        private System.Windows.Forms.Button buttonUpsert;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Label labelMailAddr;
        private System.Windows.Forms.TextBox textBoxMailAddr;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.Label labelDivider;
        private System.Windows.Forms.Button buttonSelectFromList;
    }
}