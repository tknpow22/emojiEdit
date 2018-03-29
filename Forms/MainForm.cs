namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using MimeKit;

    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 直近にフォーカスを持っていた絵文字対応の TextBox を覚えておく
        /// </summary>
        private EmojiTextBox lastActiveEmojiTextBox;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            DataBags.Initialize();

            // 画面サイズ設定
            this.MinimumSize = new Size(Commons.MIN_MAIN_WINDOW_WIDTH, Commons.MIN_MAIN_WINDOW_HEIGHT);
            this.MaximumSize = new Size(Commons.MAX_MAIN_WINDOW_WIDTH, Commons.MAX_MAIN_WINDOW_HEIGHT);
            this.Size = new Size(DataBags.Config.MainWindowWidth, DataBags.Config.MainWindowHeight);

            // 件名設定
            this.textBoxMailSubject.Font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);

            // 本文設定
            this.textBoxMailBody.Font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);
            this.textBoxMailBody.ColumnLine = DataBags.Config.MaxBodyCols;
            // コンテキストメニューを追加する
            {
                ContextMenuStrip contextMenuStrip = this.textBoxMailBody.ContextMenuStrip;
                contextMenuStrip.Items.Add(new ToolStripSeparator());
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("テンプレート選択(&S)", null, this.textBoxMailBodyToolStripMenuItem_Click, "SelectTemplate");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("テンプレート追加(&A)", null, this.textBoxMailBodyToolStripMenuItem_Click, "AddTemplate");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }

                contextMenuStrip.Opening += textBoxMailBodyContextMenuStrip_Opening;
            }

            // 送信元の設定
            this.textBoxMailFrom.Text = DataBags.Config.MailFrom;

            // 送信時、メール本文に1行の文字数毎に改行を入れる
            this.checkBoxForceInsertLineFeed.Checked = DataBags.Config.ForceInsertLineFeed;

            // 絵文字一覧を初期設定する
            this.emojiList.InitializeEmojiList();
        }

        #region イベントハンドラ

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.emojiList.ShowFirstGroupFirstEmoji();

            this.ActiveControl = this.textBoxMailSubject;
        }

        /// <summary>
        /// FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBags.Terminate();
        }

        /// <summary>
        /// SizeChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                DataBags.Config.MainWindowWidth = this.Size.Width;
                DataBags.Config.MainWindowHeight = this.Size.Height;
            }
        }

        /// <summary>
        /// メニュー - 読込 - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "ファイルから読込";
            dialog.Filter = "emlファイル|*.eml|すべてのファイル|*.*";
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                try {
                    (string subjectText, string bodyText) = MailMessageUtils.LoadFromFile(dialog.FileName);

                    this.textBoxMailSubject.Text = subjectText;
                    this.textBoxMailBody.Text = bodyText;

                } catch (Exception ex) {
                    MsgBox.Show(this, ex.Message, "読込に失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// メニュー - 保存 - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "ファイルへ保存";
            dialog.Filter = "emlファイル|*.eml";
            ////fileDialog.AddExtension = true;
            ////fileDialog.DefaultExt = "eml";
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                try {
                    MailMessageUtils.SaveToFile(this.textBoxMailSubject.Text, this.textBoxMailBody.Text, dialog.FileName);
                } catch (Exception ex) {
                    MsgBox.Show(this, ex.Message, "保存に失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// メニュー - 編集設定 - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEditSettings_Click(object sender, EventArgs e)
        {
            EditSettingsForm dialog = new EditSettingsForm();
            EditSettingsFormResult dr = dialog.ShowDialog(this);
            if (dr == EditSettingsFormResult.Cancel) {
                return;
            } else if (dr == EditSettingsFormResult.Ok) {

                this.textBoxMailBody.ColumnLine = DataBags.Config.MaxBodyCols;
                this.textBoxMailBody.Invalidate();

                // 絵文字一覧を初期設定する
                this.emojiList.InitializeEmojiList();
                this.emojiList.ShowFirstGroupFirstEmoji();

                if (this.lastActiveEmojiTextBox == null) {
                    this.lastActiveEmojiTextBox = this.textBoxMailBody;
                }

                this.emojiList.PlaybackFocusTextBox = this.lastActiveEmojiTextBox;
                this.lastActiveEmojiTextBox.Focus();
            }
        }

        /// <summary>
        /// メニュー バージョン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuVersion_Click(object sender, EventArgs e)
        {
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);

            StringBuilder message = new StringBuilder();
            message.AppendLine("絵文字エディット");
            message.AppendLine(string.Format("バージョン: {0}", fvi.FileVersion));

            MsgBox.Show(this, message.ToString(), "バージョン", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 選択(宛先)ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectMailTo_Click(object sender, EventArgs e)
        {
            MailAddressForm dialog = new MailAddressForm();
            MailAddressFormResult dr = dialog.ShowDialog(this);
            if (dr == MailAddressFormResult.Cancel) {
                return;
            }

            string mailAddr = dialog.MailAddr.Trim();
            string mailTo = this.textBoxMailTo.Text.Trim();

            List<string> mailToList =  new List<string>(mailTo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            if (mailToList.Contains(mailAddr)) {
                return;
            }

            this.textBoxMailTo.Text = string.Format("{0}{1}{2}", mailTo, (0 < mailTo.Length) ? "\x20" : "", mailAddr);
        }

        /// <summary>
        /// 選択(送信元)ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectMailFrom_Click(object sender, EventArgs e)
        {
            MailAddressForm dialog = new MailAddressForm();
            MailAddressFormResult dr = dialog.ShowDialog(this);
            if (dr == MailAddressFormResult.Cancel) {
                return;
            }

            this.textBoxMailFrom.Text = dialog.MailAddr.Trim();
        }

        /// <summary>
        /// 送信ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            // チェック(必須)
            {
                TextBox[] requiredTextBoxes = {
                    this.textBoxMailTo,
                    this.textBoxMailFrom,
                };

                foreach (TextBox requiredTextBox in requiredTextBoxes) {
                    if (requiredTextBox.Text.Trim().Length == 0) {
                        MsgBox.Show(this, string.Format("「{0}」は必ず入力してください。", requiredTextBox.Tag), "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        requiredTextBox.Focus();
                        return;
                    }
                }
            }

            // チェック(ASCII)
            {
                TextBox[] hankakuTextBoxes = {
                    this.textBoxMailTo,
                    this.textBoxMailFrom,
                };

                foreach (TextBox hankakuTextBox in hankakuTextBoxes) {
                    if (!StringUtils.IsAscii(hankakuTextBox.Text.Trim())) {
                        MsgBox.Show(this, string.Format("「{0}」は半角で入力してください。", hankakuTextBox.Tag), "半角入力", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        hankakuTextBox.Focus();
                        return;
                    }
                }
            }

            // チェック(メール)
            {
                if (!Commons.IsMailAddr(this.textBoxMailTo.Text.Trim(), true)) {
                    MsgBox.Show(this, string.Format("「{0}」を確認してください。\n・name@domain の形式のみ有効です。\n・複数入力する場合は半角スペースで区切って下さい。", this.textBoxMailTo.Tag), "メールアドレス", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.textBoxMailTo.Focus();
                    return;
                }

                if (!Commons.IsMailAddr(this.textBoxMailFrom.Text.Trim(), false)) {
                    MsgBox.Show(this, string.Format("「{0}」を確認してください。\n・name@domain の形式のみ有効です。\n・複数入力はできません。", this.textBoxMailFrom.Tag), "メールアドレス", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.textBoxMailFrom.Focus();
                    return;
                }
            }

            MimeMessage mimeMessage;
            try {

                MailMessage mailMessage = new MailMessage(
                    this.textBoxMailTo.Text.Trim(),
                    this.textBoxMailFrom.Text.Trim(),
                    this.textBoxMailSubject.Text.Trim(),
                    this.textBoxMailBody.Text);

                mimeMessage = mailMessage.GetMimeMessage();

            } catch (Exception ex) {
                string message;

                if (ex is MailMessageAddressToException) {
                    message = string.Format("「{0}」を確認してください。", this.textBoxMailTo.Tag);
                } else if (ex is MailMessageAddressFromException) {
                    message = string.Format("「{0}」を確認してください。", this.textBoxMailFrom.Tag);
                } else if (ex is MailMessageEncodeSubjectException) {
                    message = string.Format("「{0}」を確認してください。", this.textBoxMailSubject.Tag);
                } else {
                    message = string.Format("以下のエラーが発生しました。\n\n{0}", ex.Message);
                }

                MsgBox.Show(this, message, "送信準備失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SendMailForm dialog = new SendMailForm(mimeMessage);
            dialog.ShowDialog(this);
        }

        /// <summary>
        /// 絵文字テストボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEmojiTest_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.emojiList.SetTestEmoji(this.textBoxMailBody);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// メール本文に1行の文字数毎に改行を入れる - CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxForceInsertLineFeed_CheckedChanged(object sender, EventArgs e)
        {
            DataBags.Config.ForceInsertLineFeed = this.checkBoxForceInsertLineFeed.Checked;
        }

        /// <summary>
        /// 絵文字対応の TextBox - Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmojiTextBox_Enter(object sender, EventArgs e)
        {
            this.emojiList.PlaybackFocusTextBox = sender as EmojiTextBox;
            this.lastActiveEmojiTextBox = sender as EmojiTextBox;
        }

        /// <summary>
        /// 絵文字対応の TextBox - KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmojiTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            EmojiTextBox emojiTextBox = (EmojiTextBox)sender;

            switch (e.KeyCode) {
            case Keys.Enter:
                if (e.Control) {
                    Emoji emoji = this.emojiList.GetCurrentEmoji();
                    if (emoji != null) {
                        emojiTextBox.SelectedText = new string(emoji.Unicode, 1);
                        emojiTextBox.SelectionLength = 0;

                        // 履歴へ登録する
                        this.emojiList.RegistEmojiHistory(emoji.Jiscode);
                    }
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
                break;

            case Keys.Left:
                if (!e.Shift && e.Control) {
                    this.emojiList.ChangeEmojiSelectionOnCurrentGroup(-1);
                    e.Handled = true;
                } else if (e.Shift && e.Control) {
                    this.emojiList.ChangeEmojiGroupSelection(-1);
                    e.Handled = true;
                }
                break;

            case Keys.Right:
                if (!e.Shift && e.Control) {
                    this.emojiList.ChangeEmojiSelectionOnCurrentGroup(1);
                    e.Handled = true;
                } else if (e.Shift && e.Control) {
                    this.emojiList.ChangeEmojiGroupSelection(1);
                    e.Handled = true;
                }
                break;
            }

            if (e.Handled) {
                emojiTextBox.Focus();
            }
        }

        /// <summary>
        /// 本文テキストボックス - ContextMenuStrip - Opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMailBodyContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.textBoxMailBody.ContextMenuStrip.Items["AddTemplate"].Enabled = this.textBoxMailBody.SelectionLength != 0;
            this.textBoxMailBody.ContextMenuStrip.Items["SelectTemplate"].Enabled = DataBags.Templates.Count != 0;
        }

        /// <summary>
        /// 本文テキストボックス - ToolStripMenuItem - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMailBodyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;

            switch (toolStripMenuItem.Name) {
            case "AddTemplate": {
                    if (this.textBoxMailBody.SelectionLength == 0) {
                        return;
                    }

                    string templateText = this.textBoxMailBody.SelectedText;
                    if (templateText.Length == 0) {
                        return;
                    }
                    try {
                        AddTemplateForm dialog = new AddTemplateForm(templateText);
                        dialog.ShowDialog(this);
                    } catch (EmojiTemplateException) {
                        MsgBox.Show(this, "テンプレートには表示できる文字列を指定してください。", "テンプレートを作成できません", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                break;
            case "SelectTemplate": {
                    SelectTemplateForm dialog = new SelectTemplateForm();
                    SelectTemplateFormResult dr = dialog.ShowDialog(this);
                    if (dr == SelectTemplateFormResult.Cancel) {
                        this.textBoxMailBody.Focus();
                        return;
                    }
                    if (dr == SelectTemplateFormResult.SelectTemplate) {
                        this.textBoxMailBody.Focus();

                        this.textBoxMailBody.SelectedText = dialog.Template;
                        this.textBoxMailBody.SelectionLength = 0;
                    }
                }
                break;
            }
        }
        #endregion
    }
}
