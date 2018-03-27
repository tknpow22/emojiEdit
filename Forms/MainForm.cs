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

        #region 絵文字一覧

        /// <summary>
        /// 履歴に表示する絵文字アイコンの最大数
        /// </summary>
        private int maxHistoryCount;

        /// <summary>
        /// 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<List<int>> emojiGroupJiscodeMaps;

        /// <summary>
        /// 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<Image> emojiGroupImages;

        /// <summary>
        /// 履歴の行数
        /// </summary>
        private int historyRows;

        /// <summary>
        /// 履歴一覧イメージのサイズ
        /// </summary>
        private int historyMaxWidth;
        private int historyMaxHeight;

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

                DataBags.Config.MaxBodyCols = dialog.MaxBodyCols;

                this.textBoxMailBody.ColumnLine = DataBags.Config.MaxBodyCols;
                this.textBoxMailBody.Invalidate();

                #region 絵文字一覧

                DataBags.Config.MaxEmojiListCols = dialog.MaxEmojiListCols;

                // 絵文字一覧を初期設定する
                this.InitializeEmojiList();

                #endregion
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

            this.textBoxMailTo.Text = string.Format("{0}{1}{2}", mailTo, (0 < mailTo.Length) ? " " : "", mailAddr);
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

                    int col = emojiId % DataBags.Config.MaxEmojiListCols;
                    int row = rowsCount + (emojiId / DataBags.Config.MaxEmojiListCols);
                    if (col == 0 && row != 0) {
                        this.textBoxMailBody.Text += "\r\n";
                    }

                    Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                    if (emoji == null) {
                        this.textBoxMailBody.Text += "　";
                    } else {
                        this.textBoxMailBody.Text += emoji.Unicode;
                    }
                }

                int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / DataBags.Config.MaxEmojiListCols);
                rowsCount += rows;
            }

            this.textBoxMailBody.ResumeLayout();
            this.textBoxMailBody.Focus();
            this.textBoxMailBody.SelectionLength = 0;
            Cursor.Current = Cursors.Default;
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
        /// メール本文に1行の文字数毎に改行を入れる - CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxForceInsertLineFeed_CheckedChanged(object sender, EventArgs e)
        {
            DataBags.Config.ForceInsertLineFeed = this.checkBoxForceInsertLineFeed.Checked;
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
                numEmojiInGroup = maxHistoryCount;
            } else {
                numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];
            }

            int maxRows = (int)Math.Ceiling((decimal)numEmojiInGroup / DataBags.Config.MaxEmojiListCols);

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            if (0 <= col && col < DataBags.Config.MaxEmojiListCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < maxRows) {
                // OK
            } else {
                return;
            }

            List<int> emojiGroupCodeMap = emojiGroupJiscodeMaps[emojiGroupNo];

            int index = DataBags.Config.MaxEmojiListCols * row + col;

            if (index < emojiGroupCodeMap.Count) {
                // OK
            } else {
                return;
            }

            int code = emojiGroupCodeMap[index];
            Emoji emoji = DataBags.Emojis.GetFromJiscode(code);

            if (code != 0 && emoji != null) {

                if (this.lastActiveEmojiTextBox == null) {
                    this.lastActiveEmojiTextBox = this.textBoxMailBody;
                }
                lastActiveEmojiTextBox.Focus();

                lastActiveEmojiTextBox.SelectedText = new string(emoji.Unicode, 1);
                lastActiveEmojiTextBox.SelectionLength = 0;

                // 履歴へ登録する
                List<int> emojiGroupCodeMapHistory = emojiGroupJiscodeMaps[0];
                if (!emojiGroupCodeMapHistory.Contains(code)) {
                    emojiGroupCodeMapHistory.Insert(0, code);
                    emojiGroupCodeMapHistory.RemoveRange(maxHistoryCount, 1);
                } else {
                    emojiGroupCodeMapHistory.Remove(code);
                    emojiGroupCodeMapHistory.Insert(0, code);
                }
                this.RedrawEmojiListHistory();

                DataBags.Config.SetEmojiHistory(emojiGroupCodeMapHistory);
            }
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
            this.maxHistoryCount = DataBags.Config.MaxEmojiListCols * 2; // NOTE: MAX_COLS の倍数とすること

            // 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupJiscodeMaps = new List<List<int>>();

            // 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupImages = new List<Image>();

            // 履歴の行数
            this.historyRows = (int)Math.Ceiling((decimal)maxHistoryCount / DataBags.Config.MaxEmojiListCols);

            // 履歴一覧イメージのサイズ
            this.historyMaxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
            this.historyMaxHeight = Commons.FRAME_HEIGHT * historyRows;
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
                List<int> emojiGroupCodeMapHistory = DataBags.Config.GetEmojiHistory();
                int emojiHistoryCount = emojiGroupCodeMapHistory.Count;

                emojiGroupCodeMapHistory.AddRange(new int[maxHistoryCount]);
                emojiGroupCodeMapHistory.RemoveRange(maxHistoryCount, emojiHistoryCount);

                emojiGroupJiscodeMaps.Add(emojiGroupCodeMapHistory);
            }
            {
                Image emojiGroupHistoryImage = new Bitmap(historyMaxWidth, historyMaxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {
                    graphics.FillRectangle(Brushes.White, 0, 0, historyMaxWidth, historyMaxHeight);
                }

                emojiGroupImages.Add(emojiGroupHistoryImage);
            }

            //
            // 絵文字アイコン
            //

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / DataBags.Config.MaxEmojiListCols);

                int maxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
                int maxHeight = Commons.FRAME_HEIGHT * rows;

                List<int> emojiGroupCodeMap = new List<int>();
                emojiGroupCodeMap.AddRange(new int[numEmojiInGroup]);

                Image emojiGroupImage = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupImage)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    // 絵文字ID: グループ内でのID
                    for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                        Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                        if (emoji == null) {
                            continue;
                        }

                        emojiGroupCodeMap[emojiId] = emoji.Jiscode;

                        int col = emojiId % DataBags.Config.MaxEmojiListCols;
                        int row = emojiId / DataBags.Config.MaxEmojiListCols;

                        DrawUtils.DrawImage(emoji.Image, col, row, graphics);
                    }
                }

                emojiGroupImages.Add(emojiGroupImage);
                emojiGroupJiscodeMaps.Add(emojiGroupCodeMap);
            }
        }

        /// <summary>
        /// 絵文字一覧に関連するコントロールを設定する
        /// </summary>
        private void SetupEmojiListControls()
        {
            this.tabControlEmojiList.SuspendLayout();

            this.tabControlEmojiList.Controls.Clear();

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                PictureBox pictureEmojiGroup = new PictureBox();
                pictureEmojiGroup.Location = new Point(0, 0);
                pictureEmojiGroup.Name = string.Format("pictureEmojiGroup{0}", emojiGroupNo);
                pictureEmojiGroup.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureEmojiGroup.TabIndex = emojiGroupNo;
                pictureEmojiGroup.TabStop = false;
                pictureEmojiGroup.Tag = string.Format("{0}", emojiGroupNo);
                pictureEmojiGroup.MouseClick += new MouseEventHandler(this.pictureEmojiGroupX_MouseClick);

                pictureEmojiGroup.Image = emojiGroupImages[emojiGroupNo];

                TabPage tabEmojiGroup = new TabPage();
                tabEmojiGroup.AutoScroll = true;
                tabEmojiGroup.Controls.Add(pictureEmojiGroup);
                tabEmojiGroup.Name = string.Format("tabEmojiGroup{0}", emojiGroupNo);
                tabEmojiGroup.TabIndex = emojiGroupNo;
                tabEmojiGroup.Text = DataBags.Emojis.CaptionList[emojiGroupNo - 1];
                tabEmojiGroup.UseVisualStyleBackColor = true;

                this.tabControlEmojiList.Controls.Add(tabEmojiGroup);
            }

            this.tabControlEmojiList.ResumeLayout();
        }

        /// <summary>
        /// 絵文字一覧の履歴を描画する
        /// </summary>
        private void RedrawEmojiListHistory()
        {
            List<int> emojiGroupCodeMapHistory = emojiGroupJiscodeMaps[0];
            Image emojiGroupHistoryImage = emojiGroupImages[0];

            using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {

                graphics.FillRectangle(Brushes.White, 0, 0, historyMaxWidth, historyMaxHeight);

                for (int i = 0; i < emojiGroupCodeMapHistory.Count; ++i) {

                    int code = emojiGroupCodeMapHistory[i];
                    Emoji emoji = DataBags.Emojis.GetFromJiscode(code);
                    if (emoji == null) {
                        continue;
                    }

                    int col = i % DataBags.Config.MaxEmojiListCols;
                    int row = i / DataBags.Config.MaxEmojiListCols;

                    DrawUtils.DrawImage(emoji.Image, col, row, graphics);
                }
            }
            this.pictureEmojiGroup0.Image = emojiGroupHistoryImage;
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
