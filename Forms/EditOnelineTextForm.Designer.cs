namespace emojiEdit
{
    partial class EditOnelineTextForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditOnelineTextForm));
            this.textBoxInputText = new System.Windows.Forms.TextBox();
            this.buttonSetStr = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxInputText
            // 
            this.textBoxInputText.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxInputText.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxInputText.Location = new System.Drawing.Point(12, 11);
            this.textBoxInputText.Name = "textBoxInputText";
            this.textBoxInputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxInputText.Size = new System.Drawing.Size(358, 23);
            this.textBoxInputText.TabIndex = 0;
            this.textBoxInputText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxInputText_KeyDown);
            // 
            // buttonSetStr
            // 
            this.buttonSetStr.Location = new System.Drawing.Point(12, 40);
            this.buttonSetStr.Name = "buttonSetStr";
            this.buttonSetStr.Size = new System.Drawing.Size(118, 33);
            this.buttonSetStr.TabIndex = 1;
            this.buttonSetStr.Text = "設定(&S)";
            this.buttonSetStr.UseVisualStyleBackColor = true;
            this.buttonSetStr.Click += new System.EventHandler(this.buttonSetStr_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(252, 40);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(118, 33);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "キャンセル(&C)";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // EditOnelineTextForm
            // 
            this.AcceptButton = this.buttonSetStr;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 82);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSetStr);
            this.Controls.Add(this.textBoxInputText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditOnelineTextForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "文字列編集";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditTextForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxInputText;
        private System.Windows.Forms.Button buttonSetStr;
        private System.Windows.Forms.Button buttonCancel;
    }
}