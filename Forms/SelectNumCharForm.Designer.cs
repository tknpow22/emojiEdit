namespace emojiEdit
{
    partial class SelectNumCharForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectNumCharForm));
            this.buttonSetNumChar = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.numericUpDownNumChar = new System.Windows.Forms.NumericUpDown();
            this.labelNumChar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumChar)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSetNumChar
            // 
            this.buttonSetNumChar.Location = new System.Drawing.Point(14, 40);
            this.buttonSetNumChar.Name = "buttonSetNumChar";
            this.buttonSetNumChar.Size = new System.Drawing.Size(87, 33);
            this.buttonSetNumChar.TabIndex = 2;
            this.buttonSetNumChar.Text = "設定(&S)";
            this.buttonSetNumChar.UseVisualStyleBackColor = true;
            this.buttonSetNumChar.Click += new System.EventHandler(this.buttonSetNumChar_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(123, 40);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(87, 33);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // numericUpDownNumChar
            // 
            this.numericUpDownNumChar.Location = new System.Drawing.Point(61, 12);
            this.numericUpDownNumChar.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numericUpDownNumChar.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumChar.Name = "numericUpDownNumChar";
            this.numericUpDownNumChar.Size = new System.Drawing.Size(68, 19);
            this.numericUpDownNumChar.TabIndex = 1;
            this.numericUpDownNumChar.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownNumChar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numericUpDownNumChar_KeyDown);
            // 
            // labelNumChar
            // 
            this.labelNumChar.AutoSize = true;
            this.labelNumChar.Location = new System.Drawing.Point(12, 14);
            this.labelNumChar.Name = "labelNumChar";
            this.labelNumChar.Size = new System.Drawing.Size(43, 12);
            this.labelNumChar.TabIndex = 0;
            this.labelNumChar.Text = "文字数:";
            // 
            // SelectNumCharForm
            // 
            this.AcceptButton = this.buttonSetNumChar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 82);
            this.Controls.Add(this.labelNumChar);
            this.Controls.Add(this.numericUpDownNumChar);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSetNumChar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectNumCharForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文字数指定";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditTextForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNumChar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonSetNumChar;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.NumericUpDown numericUpDownNumChar;
        private System.Windows.Forms.Label labelNumChar;
    }
}