namespace emojiEdit
{
    partial class EditSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSettingsForm));
            this.lableBodyCols = new System.Windows.Forms.Label();
            this.numericUpDownBodyCols = new System.Windows.Forms.NumericUpDown();
            this.buttonSet = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxBodyEdit = new System.Windows.Forms.GroupBox();
            this.groupEmojiList = new System.Windows.Forms.GroupBox();
            this.labelEmojiListCols = new System.Windows.Forms.Label();
            this.numericUpDownEmojiListCols = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBodyCols)).BeginInit();
            this.groupBoxBodyEdit.SuspendLayout();
            this.groupEmojiList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEmojiListCols)).BeginInit();
            this.SuspendLayout();
            // 
            // lableBodyCols
            // 
            this.lableBodyCols.AutoSize = true;
            this.lableBodyCols.Location = new System.Drawing.Point(6, 20);
            this.lableBodyCols.Name = "lableBodyCols";
            this.lableBodyCols.Size = new System.Drawing.Size(59, 12);
            this.lableBodyCols.TabIndex = 0;
            this.lableBodyCols.Text = "文字数(&N):";
            // 
            // numericUpDownBodyCols
            // 
            this.numericUpDownBodyCols.Location = new System.Drawing.Point(88, 18);
            this.numericUpDownBodyCols.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownBodyCols.Minimum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownBodyCols.Name = "numericUpDownBodyCols";
            this.numericUpDownBodyCols.Size = new System.Drawing.Size(50, 19);
            this.numericUpDownBodyCols.TabIndex = 1;
            this.numericUpDownBodyCols.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(12, 123);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(90, 33);
            this.buttonSet.TabIndex = 2;
            this.buttonSet.Text = "設定(&S)";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(200, 123);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 33);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBoxBodyEdit
            // 
            this.groupBoxBodyEdit.Controls.Add(this.lableBodyCols);
            this.groupBoxBodyEdit.Controls.Add(this.numericUpDownBodyCols);
            this.groupBoxBodyEdit.Location = new System.Drawing.Point(12, 12);
            this.groupBoxBodyEdit.Name = "groupBoxBodyEdit";
            this.groupBoxBodyEdit.Size = new System.Drawing.Size(278, 48);
            this.groupBoxBodyEdit.TabIndex = 0;
            this.groupBoxBodyEdit.TabStop = false;
            this.groupBoxBodyEdit.Text = "本文編集";
            // 
            // groupEmojiList
            // 
            this.groupEmojiList.Controls.Add(this.labelEmojiListCols);
            this.groupEmojiList.Controls.Add(this.numericUpDownEmojiListCols);
            this.groupEmojiList.Location = new System.Drawing.Point(12, 66);
            this.groupEmojiList.Name = "groupEmojiList";
            this.groupEmojiList.Size = new System.Drawing.Size(278, 51);
            this.groupEmojiList.TabIndex = 1;
            this.groupEmojiList.TabStop = false;
            this.groupEmojiList.Text = "絵文字一覧";
            // 
            // labelEmojiListCols
            // 
            this.labelEmojiListCols.AutoSize = true;
            this.labelEmojiListCols.Location = new System.Drawing.Point(6, 20);
            this.labelEmojiListCols.Name = "labelEmojiListCols";
            this.labelEmojiListCols.Size = new System.Drawing.Size(59, 12);
            this.labelEmojiListCols.TabIndex = 0;
            this.labelEmojiListCols.Text = "文字数(&B):";
            // 
            // numericUpDownEmojiListCols
            // 
            this.numericUpDownEmojiListCols.Location = new System.Drawing.Point(88, 18);
            this.numericUpDownEmojiListCols.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownEmojiListCols.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownEmojiListCols.Name = "numericUpDownEmojiListCols";
            this.numericUpDownEmojiListCols.Size = new System.Drawing.Size(50, 19);
            this.numericUpDownEmojiListCols.TabIndex = 1;
            this.numericUpDownEmojiListCols.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // EditSettingsForm
            // 
            this.AcceptButton = this.buttonSet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 166);
            this.Controls.Add(this.groupEmojiList);
            this.Controls.Add(this.groupBoxBodyEdit);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "編集設定";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditSettingsForm_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBodyCols)).EndInit();
            this.groupBoxBodyEdit.ResumeLayout(false);
            this.groupBoxBodyEdit.PerformLayout();
            this.groupEmojiList.ResumeLayout(false);
            this.groupEmojiList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEmojiListCols)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lableBodyCols;
        private System.Windows.Forms.NumericUpDown numericUpDownBodyCols;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxBodyEdit;
        private System.Windows.Forms.GroupBox groupEmojiList;
        private System.Windows.Forms.Label labelEmojiListCols;
        private System.Windows.Forms.NumericUpDown numericUpDownEmojiListCols;
    }
}