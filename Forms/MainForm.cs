namespace emojiEdit
{
    using MimeKit;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 直近にフォーカスを持っていた絵文字対応の TextBox を覚えておく
        /// </summary>
        private EmojiTextBox lastActiveEmojiTextBox;

        #region 絵文字一覧

        /// <summary>
        /// 履歴に表示する絵文字アイコンの最大数
        /// </summary>
        private int maxEmojiHistoryCount;

        /// <summary>
        /// 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<List<int>> emojiGroupJiscodeMaps;

        /// <summary>
        /// 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<Image> emojiGroupImages;

        /// <summary>
        /// 絵文字アイコンイメージを保持するピクチャーボックスを保持する(インデックス 0 は履歴用で未使用)
        /// </summary>
        private List<PictureBox> emojiPictureBoxes;

        /// <summary>
        /// 絵文字アイコンの現在の選択インデックス位置を保持する(インデックス 0 は履歴用で未使用)
        /// </summary>
        private List<int> currentEmojiIds;

        /// <summary>
        /// 履歴の行数
        /// </summary>
        private int emojiHistoryRows;

        /// <summary>
        /// 履歴一覧イメージのサイズ
        /// </summary>
        private int emojiHistoryMaxWidth;
        private int emojiHistoryMaxHeight;

        #endregion

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

            #region 絵文字一覧

            // 絵文字一覧を初期設定する
            this.InitializeEmojiList();

            #endregion
        }

        #region イベントハンドラ

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.ShowFirstGroupFirstEmoji();

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
                    this.LoadFromFile(dialog.FileName);
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
                    this.SaveToFile(dialog.FileName);
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

                #region 絵文字一覧

                // 絵文字一覧を初期設定する
                this.InitializeEmojiList();

                this.ShowFirstGroupFirstEmoji();

                #endregion

                (this.lastActiveEmojiTextBox ?? this.textBoxMailBody).Focus();
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
            this.textBoxMailBody.SuspendLayout();

            this.textBoxMailBody.Clear();

            // 行数カウンタ
            int rowsCount = 0;

            // 絵文字グループ番号
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                // 絵文字ID: グループ内でのID
                for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                    (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);
                    row = rowsCount + row;
                    if (col == 0 && row != 0) {
                        this.textBoxMailBody.Text += "\r\n";
                    }

                    Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                    if (emoji == null) {
                        this.textBoxMailBody.Text += "\u3000";
                    } else {
                        this.textBoxMailBody.Text += emoji.Unicode;
                    }
                }

                int rows = this.GetGroupRows(numEmojiInGroup);
                rowsCount += rows;
            }

            this.textBoxMailBody.ResumeLayout();
            this.textBoxMailBody.Focus();
            this.textBoxMailBody.SelectionLength = 0;
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
                    Emoji emoji = this.GetCurrentEmoji();
                    if (emoji != null) {
                        emojiTextBox.SelectedText = new string(emoji.Unicode, 1);
                        emojiTextBox.SelectionLength = 0;

                        // 履歴へ登録する
                        this.RegistEmojiHistory(emoji.Jiscode);
                    }
                    e.SuppressKeyPress = true;
                    e.Handled = true;
                }
                break;

            case Keys.Up:
                if (e.Control) {
                    this.ChangeEmojiSelectionOnCurrentGroup(-1);
                    e.Handled = true;
                }
                break;

            case Keys.Down:
                if (e.Control) {
                    this.ChangeEmojiSelectionOnCurrentGroup(1);
                    e.Handled = true;
                }
                break;

            case Keys.Left:
                if (e.Control) {
                    this.ChangeEmojiGroupSelection(-1);
                    e.Handled = true;
                }
                break;

            case Keys.Right:
                if (e.Control) {
                    this.ChangeEmojiGroupSelection(1);
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

        #region 絵文字一覧

        /// <summary>
        /// 絵文字一覧タブコントロール - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlEmojiList_MouseClick(object sender, MouseEventArgs e)
        {
            (this.lastActiveEmojiTextBox ?? this.textBoxMailBody).Focus();
        }

        /// <summary>
        /// 絵文字一覧タブページコントロール - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabEmojiGroupX_MouseClick(object sender, MouseEventArgs e)
        {
            (this.lastActiveEmojiTextBox ?? this.textBoxMailBody).Focus();
        }

        /// <summary>
        /// 絵文字アイコン - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureEmojiGroupX_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureEmojiGroupX = (PictureBox)sender;

            string tag = (string)pictureEmojiGroupX.Tag;

            if (tag == null || string.Empty == tag) {
                return;
            }

            int emojiGroupNo;
            if (!int.TryParse(tag, out emojiGroupNo)) {
                return;
            }

            int numEmojiInGroup;
            if (emojiGroupNo == 0) {
                numEmojiInGroup = this.maxEmojiHistoryCount;
            } else {
                numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];
            }

            int rows = this.GetGroupRows(numEmojiInGroup);

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            if (0 <= col && col < DataBags.Config.MaxEmojiListCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < rows) {
                // OK
            } else {
                return;
            }

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];

            int emojiId = this.GetEmojiIdFromColAndRow(col, row);

            if (emojiId < emojiGroupJiscodeMap.Count) {
                // OK
            } else {
                return;
            }

            int jiscode = emojiGroupJiscodeMap[emojiId];
            Emoji emoji = null;
            if (jiscode != 0) {
                emoji = DataBags.Emojis.GetFromJiscode(jiscode);
            }
            if (emoji != null) {

                // タブ上の絵文字の選択表示を変更する
                this.ChangeEmojiSelection(emojiGroupNo, emojiId);

                EmojiTextBox emojiTextBox = this.lastActiveEmojiTextBox ?? this.textBoxMailBody;
                emojiTextBox.Focus();
                emojiTextBox.SelectedText = new string(emoji.Unicode, 1);
                emojiTextBox.SelectionLength = 0;

                this.lastActiveEmojiTextBox = emojiTextBox;

                // 履歴へ登録する
                this.RegistEmojiHistory(jiscode);
            }
            (this.lastActiveEmojiTextBox ?? this.textBoxMailBody).Focus();
        }

        #endregion

        #endregion

        #region 内部処理

        #region 絵文字一覧

        /// <summary>
        /// 絵文字一覧を初期設定する
        /// </summary>
        private void InitializeEmojiList()
        {
            // 絵文字一覧に関連する変数の初期化
            this.InitializeEmojiListValues();

            // 絵文字一覧に絵文字アイコンと対応する文字コードをロードする
            this.LoadEmojiList();

            // 絵文字一覧に関連するコントロールを設定する
            this.SetupEmojiListControls();

            // 絵文字一覧の履歴を描画する
            this.RedrawEmojiListHistory();
        }

        /// <summary>
        /// 絵文字一覧に関連する変数の初期化
        /// </summary>
        private void InitializeEmojiListValues()
        {
            // 履歴に表示する絵文字アイコンの最大数
            this.maxEmojiHistoryCount = DataBags.Config.MaxEmojiListCols * 2; // NOTE: MAX_COLS の倍数とすること

            // 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupJiscodeMaps = new List<List<int>>();

            // 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupImages = new List<Image>();

            // 絵文字アイコンイメージを保持するピクチャーボックスを保持する(インデックス 0 は履歴用で未使用)
            this.emojiPictureBoxes = new List<PictureBox>();

            // 現在選択されている絵文字IDをグループごとに管理する(インデックス 0 は履歴用で未使用: ただし実装の都合上、履歴用にも値が設定されるが使っていない)
            this.currentEmojiIds = new List<int>();

            // 履歴の行数
            this.emojiHistoryRows = (int)Math.Ceiling((decimal)this.maxEmojiHistoryCount / DataBags.Config.MaxEmojiListCols);

            // 履歴一覧イメージのサイズ
            this.emojiHistoryMaxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
            this.emojiHistoryMaxHeight = Commons.FRAME_HEIGHT * this.emojiHistoryRows;
        }

        /// <summary>
        /// 絵文字一覧に絵文字アイコンと対応する文字コードをロードする
        /// </summary>
        private void LoadEmojiList()
        {
            //
            // 絵文字アイコン(履歴)
            //
            {
                List<int> emojiGroupJiscodeMapHistory = DataBags.Config.GetEmojiHistory();
                int emojiHistoryCount = emojiGroupJiscodeMapHistory.Count;

                emojiGroupJiscodeMapHistory.AddRange(new int[this.maxEmojiHistoryCount]);
                emojiGroupJiscodeMapHistory.RemoveRange(this.maxEmojiHistoryCount, emojiHistoryCount);

                this.emojiGroupJiscodeMaps.Add(emojiGroupJiscodeMapHistory);
            }
            {
                Image emojiGroupHistoryImage = new Bitmap(this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {
                    graphics.FillRectangle(Brushes.White, 0, 0, this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);
                }

                this.emojiGroupImages.Add(emojiGroupHistoryImage);
            }
            this.currentEmojiIds.Add(-1);   // インデックス 0 は履歴用で未使用

            //
            // 絵文字アイコン
            //

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                int rows = this.GetGroupRows(numEmojiInGroup);

                int maxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
                int maxHeight = Commons.FRAME_HEIGHT * rows;

                List<int> emojiGroupJiscodeMap = new List<int>();
                emojiGroupJiscodeMap.AddRange(new int[numEmojiInGroup]);   // 個数分作成する。初期値は 0

                Image emojiGroupImage = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupImage)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    // 絵文字ID: グループ内でのID
                    int firstEmojiId = -1;  // 有効な最初の絵文字ID
                    for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                        Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                        if (emoji == null) {
                            continue;
                        }
                        emojiGroupJiscodeMap[emojiId] = emoji.Jiscode;

                        (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

                        DrawUtils.DrawImage(emoji.Image, col, row, graphics);

                        if (firstEmojiId == -1) {
                            firstEmojiId = emojiId;
                        }
                    }
                    this.currentEmojiIds.Add(firstEmojiId);
                }

                this.emojiGroupImages.Add(emojiGroupImage);
                this.emojiGroupJiscodeMaps.Add(emojiGroupJiscodeMap);
            }
        }

        /// <summary>
        /// 絵文字一覧に関連するコントロールを設定する
        /// </summary>
        private void SetupEmojiListControls()
        {
            this.tabControlEmojiList.SuspendLayout();

            this.tabControlEmojiList.Controls.Clear();

            this.emojiPictureBoxes.Add(null);   // インデックス 0 は履歴用で未使用

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                PictureBox pictureEmojiGroup = new PictureBox();
                pictureEmojiGroup.Location = new Point(0, 0);
                pictureEmojiGroup.Name = string.Format("pictureEmojiGroup{0}", emojiGroupNo);
                pictureEmojiGroup.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureEmojiGroup.TabIndex = emojiGroupNo;
                pictureEmojiGroup.TabStop = false;
                pictureEmojiGroup.Tag = string.Format("{0}", emojiGroupNo);
                pictureEmojiGroup.MouseClick += new MouseEventHandler(this.pictureEmojiGroupX_MouseClick);

                pictureEmojiGroup.Image = this.emojiGroupImages[emojiGroupNo];

                TabPage tabEmojiGroup = new TabPage();
                tabEmojiGroup.AutoScroll = true;
                tabEmojiGroup.Controls.Add(pictureEmojiGroup);
                tabEmojiGroup.Name = string.Format("tabEmojiGroup{0}", emojiGroupNo);
                tabEmojiGroup.TabIndex = emojiGroupNo;
                tabEmojiGroup.Text = DataBags.Emojis.CaptionList[emojiGroupNo - 1];
                tabEmojiGroup.UseVisualStyleBackColor = true;
                tabEmojiGroup.MouseClick += new MouseEventHandler(this.tabEmojiGroupX_MouseClick);

                this.tabControlEmojiList.Controls.Add(tabEmojiGroup);
                this.emojiPictureBoxes.Add(pictureEmojiGroup);
            }

            this.tabControlEmojiList.ResumeLayout();

            // 絵文字一覧のタブ用のイメージに現在の選択マークを表示する
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                this.DrawSelectedEmojiFrame(true, emojiGroupNo);
            }
        }

        /// <summary>
        /// 絵文字一覧の履歴を描画する
        /// </summary>
        private void RedrawEmojiListHistory()
        {
            List<int> emojiGroupJiscodeMapHistory = this.emojiGroupJiscodeMaps[0];
            Image emojiGroupHistoryImage = this.emojiGroupImages[0];

            using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {

                graphics.FillRectangle(Brushes.White, 0, 0, this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);

                for (int emojiId = 0; emojiId < emojiGroupJiscodeMapHistory.Count; ++emojiId) {

                    int jiscode = emojiGroupJiscodeMapHistory[emojiId];
                    Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
                    if (emoji == null) {
                        continue;
                    }

                    (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

                    DrawUtils.DrawImage(emoji.Image, col, row, graphics);
                }
            }
            this.pictureEmojiGroup0.Image = emojiGroupHistoryImage;
        }

        /// <summary>
        /// 絵文字一覧の表示しているタブを変更する
        /// </summary>
        /// <param name="direction">-1の場合は左, +1の場合は右へ移動する</param>
        private void ChangeEmojiGroupSelection(int direction)
        {
            System.Diagnostics.Debug.Assert(direction == -1 || direction == 1);

            if (this.tabControlEmojiList.TabCount == 0) {
                return;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            tabIndexCurrent += this.tabControlEmojiList.TabCount + direction;
            int tabIndexNew = tabIndexCurrent % this.tabControlEmojiList.TabCount;

            this.tabControlEmojiList.SelectedIndex = tabIndexNew;
        }

        /// <summary>
        /// 絵文字一覧の現在表示しているタブ上の絵文字選択を変更する
        /// </summary>
        /// <param name="direction">-1の場合は左, +1の場合は右へ移動する</param>
        private void ChangeEmojiSelectionOnCurrentGroup(int direction)
        {
            System.Diagnostics.Debug.Assert(direction == -1 || direction == 1);

            if (this.tabControlEmojiList.TabCount == 0) {
                return;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            int emojiGroupNo = tabIndexCurrent + 1;

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];
            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return;
            }

            for (int countLimit = 0; countLimit < emojiGroupJiscodeMap.Count; ++countLimit) {
                emojiIdCurrent += emojiGroupJiscodeMap.Count + direction;
                int emojiIdNew = emojiIdCurrent % emojiGroupJiscodeMap.Count;
                int jiscode = emojiGroupJiscodeMap[emojiIdNew];
                if (jiscode != 0) {
                    emojiIdCurrent = emojiIdNew;
                    break;
                }
            }

            this.ChangeEmojiSelection(emojiGroupNo, emojiIdCurrent);
        }

        /// <summary>
        /// タブ上の絵文字の選択表示を変更する
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">絵文字ID</param>
        private void ChangeEmojiSelection(int emojiGroupNo, int emojiId)
        {
            if (emojiGroupNo <= 0) {
                return;
            }
            this.DrawSelectedEmojiFrame(false, emojiGroupNo);
            this.currentEmojiIds[emojiGroupNo] = emojiId;
            this.DrawSelectedEmojiFrame(true, emojiGroupNo);
            this.RerawEmojiGroup(emojiGroupNo);
            this.ScrollToCurrentEmoji(emojiGroupNo, emojiId);
        }

        /// <summary>
        /// 絵文字一覧のタブ用のイメージに現在の選択マークを表示/非表示する
        /// </summary>
        /// <param name="selected">選択する場合 true、解除の場合 false</param>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        private void DrawSelectedEmojiFrame(bool selected, int emojiGroupNo)
        {
            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return;
            }

            (int col, int row) = this.GetColAndRowFromEmojiId(emojiIdCurrent);

            Image image = this.emojiGroupImages[emojiGroupNo];

            using (Graphics graphics = Graphics.FromImage(image)) {
                DrawUtils.DrawFrame(selected, col, row, graphics);
            }
        }

        /// <summary>
        /// 絵文字一覧のタブページを再描画する
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        private void RerawEmojiGroup(int emojiGroupNo)
        {
            PictureBox pictureBox = this.emojiPictureBoxes[emojiGroupNo];
            if (pictureBox != null) {
                pictureBox.Invalidate();
            }
        }

        /// <summary>
        ///  絵文字一覧の最初のページの最初の絵文字にスクロールする
        /// </summary>
        private void ShowFirstGroupFirstEmoji()
        {
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                int emojiId = this.currentEmojiIds[emojiGroupNo];
                if (0 <= emojiId) {
                    this.tabControlEmojiList.SelectedIndex = emojiGroupNo - 1;
                    this.ScrollToCurrentEmoji(emojiGroupNo, emojiId);
                    break;
                }
            }
        }

        /// <summary>
        /// 絵文字一覧のタブページの現在の絵文字にスクロールする
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">絵文字ID</param>
        private void ScrollToCurrentEmoji(int emojiGroupNo, int emojiId)
        {
            (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

            int x = Commons.FRAME_WIDTH * col;
            int y = Commons.FRAME_HEIGHT * row;

            TabPage tabPage = this.tabControlEmojiList.TabPages[emojiGroupNo - 1];
            tabPage.AutoScrollPosition = new Point(x, y);
        }

        /// <summary>
        /// 絵文字グループをタブに表示する際の行数を取得する
        /// </summary>
        /// <param name="numEmojiInGroup">グループ内の絵文字の数</param>
        /// <returns>行数</returns>
        private int GetGroupRows(int numEmojiInGroup)
        {
            int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / DataBags.Config.MaxEmojiListCols);
            return rows;
        }

        /// <summary>
        /// 絵文字ID から表示する列と行を得る
        /// </summary>
        /// <param name="emojiId">絵文字ID</param>
        /// <returns>(列,行)</returns>
        private (int, int) GetColAndRowFromEmojiId(int emojiId)
        {
            int col = emojiId % DataBags.Config.MaxEmojiListCols;
            int row = emojiId / DataBags.Config.MaxEmojiListCols;

            return (col, row);
        }

        /// <summary>
        /// 列と行から絵文字IDを得る
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="row">行</param>
        /// <returns>絵文字ID</returns>
        private int GetEmojiIdFromColAndRow(int col, int row)
        {
            int emojiId = DataBags.Config.MaxEmojiListCols * row + col;
            return emojiId;
        }

        /// <summary>
        /// 現在表示されているタブの選択されている絵文字を得る
        /// </summary>
        /// <returns>Emoji、選択がない場合は null</returns>
        private Emoji GetCurrentEmoji()
        {
            if (this.tabControlEmojiList.TabCount == 0) {
                return null;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            int emojiGroupNo = tabIndexCurrent + 1;

            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return null;
            }

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];

            int jiscode = emojiGroupJiscodeMap[emojiIdCurrent];
            if (jiscode == 0) {
                return null;
            }

            Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
            return emoji;
        }

        /// <summary>
        ///  絵文字履歴を登録する
        /// </summary>
        /// <param name="jiscode">登録する JIS コード</param>
        private void RegistEmojiHistory(int jiscode)
        {
            List<int> emojiGroupJiscodeMapHistory = this.emojiGroupJiscodeMaps[0];
            if (!emojiGroupJiscodeMapHistory.Contains(jiscode)) {
                emojiGroupJiscodeMapHistory.Insert(0, jiscode);
                emojiGroupJiscodeMapHistory.RemoveRange(this.maxEmojiHistoryCount, 1);
            } else {
                emojiGroupJiscodeMapHistory.Remove(jiscode);
                emojiGroupJiscodeMapHistory.Insert(0, jiscode);
            }
            this.RedrawEmojiListHistory();

            DataBags.Config.SetEmojiHistory(emojiGroupJiscodeMapHistory);
        }

        #endregion

        /// <summary>
        /// ファイルから読込(宛先、送信元は読み込まない)
        /// </summary>
        /// <param name="filename"></param>
        private void LoadFromFile(string filename)
        {
            FileInfo file = new FileInfo(filename);
            byte[] data = new byte[file.Length];

            string subjectText = "";
            string bodyText = "";

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {

                int length = fs.Read(data, 0, data.Length);
                if (length != data.Length) {
                    throw new Exception("形式が違います: ボディ部の長さが足りません。");
                }

                // ボディ部の先頭を見つける
                int indexBeginBody = 0;
                for (int i = 0; i < data.Length; ++i) {
                    if (CharCodeUtils.IsCrLf(data, data.Length, i) && CharCodeUtils.IsCrLf(data, data.Length, i + 2)) {
                        indexBeginBody = i + 4;
                        break;
                    }
                }
                if (indexBeginBody == 0) {
                    throw new Exception("形式が違います: ボディ部が存在しません。");
                }

                string headers = Encoding.ASCII.GetString(data, 0, indexBeginBody);
                if (headers != null) {
                    subjectText = this.GetSubjectText(headers);
                }

                bodyText = this.GetBodyText(data, indexBeginBody, data.Length - indexBeginBody);
            }

            this.textBoxMailSubject.Text = subjectText;
            this.textBoxMailBody.Text = bodyText;
        }

        /// <summary>
        /// ヘッダ文字列から件名を取得する
        /// NOTE: 自分で出力した形式にしか対応していない
        /// </summary>
        /// <param name="headers">ヘッダ文字列</param>
        /// <returns>件名</returns>
        private string GetSubjectText(string headers)
        {
            int startIndex = headers.IndexOf(MailMessage.SUBJECT, 0, StringComparison.CurrentCultureIgnoreCase);
            if (startIndex < 0) {
                return "";
            }

            startIndex += MailMessage.SUBJECT.Length;

            int index = startIndex;
            int endIndex = startIndex;
            while (true) {
                index = headers.IndexOf("\r\n", index);
                if (index < 0) {
                    break;
                }
                if (headers.Length <= index + 2) {
                    break;
                }
                endIndex = index;
                if (headers[index + 2] != ' ' && headers[index + 2] != '\t') {
                    break;
                }
                index += 2;
            }
            if (endIndex == startIndex) {
                return "";
            }

            string subject = headers.Substring(startIndex, endIndex - startIndex);
            Regex regex = new Regex(@"=\?(?<charset>.*?)\?(?<encoding>.)\?(?<value>.*?)\?=");

            StringBuilder result = new StringBuilder();

            MatchCollection matchCollection = regex.Matches(subject);
            foreach (Match match in matchCollection) {
                if (match.Success) {
                    string charset = match.Groups["charset"].Value.ToLower();
                    string encoding = match.Groups["encoding"].Value.ToLower();
                    string val = match.Groups["value"].Value;

                    if (charset == "iso-2022-jp" && encoding == "b") {
                        // OK
                    } else {
                        // 未対応
                        return "";
                    }

                    byte[] dataPart = Convert.FromBase64String(val);
                    bool inJpn = false;

                    int dataPartLength = dataPart.Length;

                    for (int i = 0; i < dataPartLength;) {

                        if (CharCodeUtils.IsBeginEscapeSequence(dataPart, dataPartLength, i)) {
                            inJpn = true;
                            i += 3;
                            continue;
                        }
                        if (CharCodeUtils.IsEndEscapeSequence(dataPart, dataPartLength, i)) {
                            inJpn = false;
                            i += 3;
                            continue;
                        }
                        if (inJpn) {
                            if (i <= dataPartLength - 2) {

                                int jiscode = CharCodeUtils.MergeHL(dataPart[i], dataPart[i + 1]);
                                Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
                                if (emoji != null) {
                                    result.Append(emoji.Unicode);
                                } else {
                                    string sch = CharCodeUtils.GetCharFromJisCode(jiscode);
                                    if (sch != null) {
                                        result.Append(sch);
                                    }
                                }

                                i += 2;
                            } else {
                                i += 1;
                            }
                        } else {
                            char ascii = (char)dataPart[i];
                            result.Append(ascii);
                            i += 1;
                        }
                    }
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// ボディ部データから本文を取得する
        /// NOTE: 自分で出力した形式にしか対応していない
        /// </summary>
        /// <param name="data">ボディ部データ</param>
        /// <param name="offset">オフセット</param>
        /// <param name="count">バイト数</param>
        /// <returns>本文</returns>
        private string GetBodyText(byte[] data, int offset, int count)
        {
            bool inJpn = false;

            int dataLength = data.Length;

            StringBuilder result = new StringBuilder();

            for (int i = offset; i < dataLength;) {

                if (CharCodeUtils.IsCrLf(data, dataLength, i)) {
                    inJpn = false;
                    i += 2;
                    result.Append("\r\n");
                    continue;
                }
                if (CharCodeUtils.IsBeginEscapeSequence(data, dataLength, i)) {
                    inJpn = true;
                    i += 3;
                    continue;
                }
                if (CharCodeUtils.IsEndEscapeSequence(data, dataLength, i)) {
                    inJpn = false;
                    i += 3;
                    continue;
                }
                if (inJpn) {
                    if (i <= dataLength - 2) {
                        int jiscode = CharCodeUtils.MergeHL(data[i], data[i + 1]);
                        Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
                        if (emoji != null) {
                            result.Append(emoji.Unicode);
                        } else {
                            string sch = CharCodeUtils.GetCharFromJisCode(jiscode);
                            if (sch != null) {
                                result.Append(sch);
                            }
                        }

                        i += 2;
                    } else {
                        i += 1;
                    }
                } else {
                    char ascii = (char)data[i];
                    result.Append(ascii);
                    i += 1;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// ファイルへ保存(宛先、送信元は保存しない)
        /// </summary>
        /// <param name="filename">ファイル名</param>
        private void SaveToFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
                MailMessage mailMessage = new MailMessage(
                    "",
                    "",
                    this.textBoxMailSubject.Text.Trim(),
                    this.textBoxMailBody.Text);

                MimeMessage mimeMessage = mailMessage.GetMimeMessage();
                mimeMessage.WriteTo(fs);
            }
        }

        #endregion
    }
}
