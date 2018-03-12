using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MimeKit;

namespace emojiEdit
{
    //
    // 絵文字エディット
    //
    public partial class MainForm : Form
    {
        // 件名ヘッダ
        private const string SUBJECT = "Subject:";

        // 保存時に固定出力するヘッダ
        private const string HEADERS = "Content-Type: text/plain; charset=ISO-2022-JP\r\nContent-Transfer-Encoding: 7bit\r\n";

        // コンテキストメニューが表示された時の pictureContentsSubject 上の座標位置
        private int ctxMenuSubjectX = 0;
        private int ctxMenuSubjectY = 0;

        // コンテキストメニューが表示された時の pictureContentsBody 上の座標位置
        private int ctxMenuBodyX = 0;
        private int ctxMenuBodyY = 0;

        // 絵文字編集操作(件名)
        private EditEmojiOperation editEmojiOperationSubject;

        // 絵文字編集操作(ボディ部)
        private EditEmojiOperation editEmojiOperationBody;

        // コンストラクタ
        public MainForm()
        {
            InitializeComponent();

            DataBags.Init();

            if (DataBags.Config.MainWindowWidth <= 0 || DataBags.Config.MainWindowHeight <= 0) {
                DataBags.Config.MainWindowWidth = this.Size.Width;
                DataBags.Config.MainWindowHeight = this.Size.Height;
            } else {
                this.Size = new Size(DataBags.Config.MainWindowWidth, DataBags.Config.MainWindowHeight);
            }

            this.MinimumSize = new Size(Commons.MinWndWidth, Commons.MinWndHeight);
            this.MaximumSize = new Size(Commons.MaxWndWidth, Commons.MaxWndHeight);

            this.textBoxMailFrom.Text = DataBags.Config.MailFrom;

            this.editEmojiOperationSubject = new EditEmojiOperation(
                    this,
                    this.panelContentsSubject,
                    this.pictureContentsSubject,
                    Commons.MAX_SUBJECT_COLS,
                    Commons.MAX_SUBJECT_ROWS
               );
            this.editEmojiOperationSubject.Clear();

            this.editEmojiOperationBody = new EditEmojiOperation(
                    this,
                    this.panelContentsBody,
                    this.pictureContentsBody,
                    DataBags.Config.BodyMaxCols,
                    DataBags.Config.BodyMaxRows
               );
            this.editEmojiOperationBody.Clear();
        }

        //
        // イベントハンドラ
        //

        // フォームが閉じられた時
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBags.Save();
        }

        // 画面サイズが変更された時
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal) {
                DataBags.Config.MainWindowWidth = this.Size.Width;
                DataBags.Config.MainWindowHeight = this.Size.Height;
            }
        }

        // メニュー - 読込
        private void menuLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "ファイルから読込";
            dialog.Filter = "emlファイル|*.eml|すべてのファイル|*.*";
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                try {

                    this.editEmojiOperationSubject.Clear();
                    this.editEmojiOperationBody.Clear();

                    this.LoadFromFile(dialog.FileName);
                } catch (Exception ex) {
                    MsgBox.Show(this, ex.Message, "読み込みに失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // メニュー - 保存
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
                    MsgBox.Show(this, ex.Message, "内容を保存できません", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // メニュー - 編集設定
        private void menuEditSettings_Click(object sender, EventArgs e)
        {
            EditSettingsForm dialog = new EditSettingsForm();
            EditSettingsFormResult dr = dialog.ShowDialog(this);
            if (dr == EditSettingsFormResult.Cancel) {
                return;
            } else if (dr == EditSettingsFormResult.Ok) {
                this.editEmojiOperationBody.MaxCols = DataBags.Config.BodyMaxCols;
                this.editEmojiOperationBody.MaxRows = DataBags.Config.BodyMaxRows;
                this.editEmojiOperationBody.Clear();
            }
        }

        // メニュー バージョン
        private void menuVersion_Click(object sender, EventArgs e)
        {
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            System.Diagnostics.Debug.WriteLine(fvi.FileVersion);

            StringBuilder message = new StringBuilder();
            message.AppendLine("絵文字エディット");
            message.AppendLine(string.Format("バージョン: {0}", fvi.FileVersion));

            MsgBox.Show(this, message.ToString(), "バージョン", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // フォーカスロスト
        private void textBoxToHankaku_Leave(object sender, EventArgs e)
        {
            TextBox control = (TextBox)sender;
            control.Text = StringUtils.ToHankaku(control.Text);
        }

        // 選択 - 宛先
        private void buttonSelectMailTo_Click(object sender, EventArgs e)
        {
            MailAddressForm dialog = new MailAddressForm();
            MailAddressFormResult dr = dialog.ShowDialog(this);
            if (dr == MailAddressFormResult.Cancel) {
                return;
            }

            string mailAddr = dialog.MailAddr.Trim();
            string mailTo = this.textBoxMailTo.Text.Trim();
            if (0 <= mailTo.IndexOf(mailAddr)) {
                return;
            }

            this.textBoxMailTo.Text = string.Format("{0}{1}{2}", mailTo, (0 < mailTo.Length) ? " " : "", mailAddr);
        }

        // 選択 - 送信元
        private void buttonSelectMailFrom_Click(object sender, EventArgs e)
        {
            MailAddressForm dialog = new MailAddressForm();
            MailAddressFormResult dr = dialog.ShowDialog(this);
            if (dr == MailAddressFormResult.Cancel) {
                return;
            }

            this.textBoxMailFrom.Text = dialog.MailAddr.Trim();
        }

        // 消去
        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.editEmojiOperationBody.Clear();
        }

        // 送信
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

                ////int subjectLength = this.GetSubjectLength();
                ////if (subjectLength == 0) {
                ////    MsgBox.Show(this, string.Format("「{0}」は必ず入力してください。", "件名"), "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ////    this.editEmojiOperationSubject.ScrollTop();
                ////    return;
                ////}
            }

            // チェック(半角)
            {
                TextBox[] hankakuTextBoxes = {
                    this.textBoxMailTo,
                    this.textBoxMailFrom,
                };

                foreach (TextBox hankakuTextBox in hankakuTextBoxes) {
                    if (!StringUtils.IsHankaku(hankakuTextBox.Text.Trim())) {
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


            MailboxAddress mailboxAddressFrom;
            try {
                string mailFrom = this.textBoxMailFrom.Text.Trim();
                mailboxAddressFrom = new MailboxAddress(mailFrom);
            } catch (Exception ex) {
                MsgBox.Show(this, string.Format("「{0}」を確認してください。\n\n{1}", this.textBoxMailFrom.Tag, ex.Message), "メールアドレス", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<MailboxAddress> mailboxAddressToList = new List<MailboxAddress>();
            try {
                string mailTo = this.textBoxMailTo.Text.Trim();
                string[] mailToArr = mailTo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mail in mailToArr) {
                    mailboxAddressToList.Add(new MailboxAddress(mail));
                }

            } catch (Exception ex) {
                MsgBox.Show(this, string.Format("「{0}」を確認してください。\n\n{1}", this.textBoxMailTo.Tag, ex.Message), "メールアドレス", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Header headerSubject = this.GetSubjectHeader();
            if (headerSubject == null) {
                MsgBox.Show(this, "件名のエンコードに失敗しました。", "件名", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try {
                using (MemoryStream body = new MemoryStream()) {

                    this.WriteBody(body);

                    MimePart mimePart = new MimePart("text/plain; charset=ISO-2022-JP");
                    mimePart.ContentTransferEncoding = ContentEncoding.SevenBit;
                    mimePart.Content = new MimeContent(body);

                    MimeMessage message = new MimeMessage();
                    message.From.Add(mailboxAddressFrom);
                    message.To.AddRange(mailboxAddressToList);
                    message.Headers.Replace(headerSubject);
                    message.Body = mimePart;

                    SendMailForm dialog = new SendMailForm(message);
                    dialog.ShowDialog(this);
                }
            } catch (Exception ex) {
                MsgBox.Show(this, string.Format("以下のエラーが発生しました。\n\n{0}", ex.Message), "送信準備失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // テスト
        private void buttonTest_Click(object sender, EventArgs e)
        {
            this.editEmojiOperationBody.SetTestData();
        }

        // ピクチャーイメージ(件名部)をクリック
        private void pictureContentsSubject_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                this.ctxMenuSubjectX = e.X;
                this.ctxMenuSubjectY = e.Y;

                this.contextMenuSubject.Show(Cursor.Position);
                return;
            }

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            this.editEmojiOperationSubject.CallSelectEmoji(col, row);
        }

        // ピクチャーイメージ(ボディ部)をクリック
        private void pictureContentsBody_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                this.ctxMenuBodyX = e.X;
                this.ctxMenuBodyY = e.Y;

                this.contextMenuBody.Show(Cursor.Position);
                return;
            }

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.CallSelectEmoji(col, row);
        }

        //
        // イベントハンドラ - コンテキストメニュー(ボディ部)
        //

        // 制御
        private void contextMenuBody_Opening(object sender, CancelEventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text)) {
                menuPasteTextBody.Enabled = true;
            } else {
                menuPasteTextBody.Enabled = false;
            }
        }

        // 文字列挿入
        private void menuInsertTextBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.CallEditText(col, row);
        }

        // 貼り付け挿入
        private void menuPasteTextBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            string text = Clipboard.GetText() ?? "";
            this.editEmojiOperationBody.InsertText(text, col, row);
        }

        // 改行
        private void menuNewLineBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.NewLine(col, row);
        }

        // 空行挿入
        private void menuInsertEmptyLineBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.InsertEmptyLine(col, row);
        }

        // 空文字挿入
        private void menuInsertCharBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.InsertChar(col, row);
        }

        // 行削除
        private void menuRemoveLineBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.RemoveLine(col, row);
        }

        // 1文字削除
        private void menuRemoveCharBody_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuBodyX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuBodyY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationBody.RemoveChar(col, row);
        }

        //
        // イベントハンドラ - コンテキストメニュー(件名)
        //

        // 文字列挿入
        private void menuInsertTextSubject_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuSubjectX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuSubjectY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationSubject.CallEditOnelineText(col, row);
        }

        // 空文字挿入
        private void menuInsertCharSubject_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuSubjectX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuSubjectY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationSubject.InsertChar(col, row);
        }

        // 1文字削除
        private void menuRemoveCharSubject_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuSubjectX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuSubjectY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationSubject.RemoveChar(col, row);
        }

        // 消去
        private void menuClearSubject_Click(object sender, EventArgs e)
        {
            this.editEmojiOperationSubject.Clear();
        }

        //
        // 内部処理
        //

        // 件名の有効長を取得する
        private int GetSubjectLength()
        {
            List<int> contents = this.editEmojiOperationSubject.Contents;
            int useCols = Commons.GetUseCols(Commons.MAX_SUBJECT_COLS, contents, 0);

            return useCols;
        }

        // 件名から Subject ヘッダを取得する
        private Header GetSubjectHeader()
        {
            Header result = null;

            string subject = this.GetSubjectString();

            Header header;
            if (Header.TryParse(subject, out header)) {
                result = header;
            }

            return result;
        }

        // 件名から Subject ヘッダ文字列を取得する
        private string GetSubjectString()
        {
            List<int> contents = this.editEmojiOperationSubject.Contents;
            int useCols = this.GetSubjectLength();

            MemoryStream stream = new MemoryStream();
            int chCount = 0;

            List<string> base64BlockList = new List<string>();

            for (int col = 0; col < useCols; ++col) {
                int code = contents[col];
                if (code == 0) {
                    JisUtils.WriteWhiteSpace(stream);
                } else {
                    (byte high, byte low) = JisUtils.SplitHL(code);

                    stream.WriteByte(high);
                    stream.WriteByte(low);
                }

                ++chCount;

                if (5 <= chCount) {
                    base64BlockList.Add(this.GetBase64Block(stream));

                    stream = new MemoryStream();
                    chCount = 0;
                }
            }
            if (0 < chCount) {
                base64BlockList.Add(this.GetBase64Block(stream));
            }

            stream.Dispose();

            StringBuilder subject = new StringBuilder();
            subject.Append(SUBJECT);
            if (base64BlockList.Count == 0) {
                subject.Append(" \r\n");
            } else {
                subject.Append(string.Join("", base64BlockList));
            }

            return subject.ToString();
        }

        // 件名の BASE64 ブロックを取得する
        private string GetBase64Block(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (MemoryStream buffer = new MemoryStream()) {
                JisUtils.WriteBeginEscapeSequence(buffer);
                stream.WriteTo(buffer);
                JisUtils.WriteEndEscapeSequence(buffer);

                byte[] data = buffer.ToArray();
                string base64text = Convert.ToBase64String(data, 0, data.Length);
                string block = string.Format(" =?iso-2022-jp?b?{0}?=\r\n", base64text);
                return block;
            }
        }

        // メールのボディ部の文字コードデータから絵文字を読み込む
        private void LoadFromFile(string filename)
        {
            FileInfo file = new FileInfo(filename);
            byte[] data = new byte[file.Length];

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read)) {

                int length = fs.Read(data, 0, data.Length);
                if (length != data.Length) {
                    throw new Exception("形式が違います: ボディ部の長さが足りません。");
                }

                // ボディ部の先頭を見つける
                int indexBeginBody = 0;
                for (int i = 0; i < data.Length; ++i) {
                    if (JisUtils.IsCrLf(data, data.Length, i) && JisUtils.IsCrLf(data, data.Length, i + 2)) {
                        indexBeginBody = i + 4;
                        break;
                    }
                }
                if (indexBeginBody == 0) {
                    throw new Exception("形式が違います: ボディ部が存在しません。");
                }

                string headers = Encoding.ASCII.GetString(data, 0, indexBeginBody);
                if (headers != null) {
                    List<int> codeList = this.GetSubjectCodeList(headers);
                    if (codeList != null) {
                        this.editEmojiOperationSubject.LoadFromCodes(codeList, 0, 0);
                    }
                }

                this.editEmojiOperationBody.LoadFromBodyData(data, indexBeginBody, data.Length - indexBeginBody);
            }
        }

        // ヘッダ文字列から件名部の文字コード一覧を取得する
        // NOTE: 自分で出力した形式にしか対応していない
        private List<int> GetSubjectCodeList(string headers)
        {
            int startIndex = headers.IndexOf(SUBJECT, 0, StringComparison.CurrentCultureIgnoreCase);
            if (startIndex < 0) {
                return null;
            }

            startIndex += SUBJECT.Length;

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
                return null;
            }

            string subject = headers.Substring(startIndex, endIndex - startIndex);
            Regex regex = new Regex(@"=\?(?<charset>.*?)\?(?<encoding>.)\?(?<value>.*?)\?=");

            List<int> codeList = new List<int>();

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
                        return null;
                    }

                    byte[] biSubjectPart = Convert.FromBase64String(val);
                    bool inJpn = false;

                    int dataLength = biSubjectPart.Length;

                    for (int i = 0; i < dataLength; ) {

                        if (JisUtils.IsBeginEscapeSequence(biSubjectPart, dataLength, i)) {
                            inJpn = true;
                            i += 3;
                            continue;
                        }
                        if (JisUtils.IsEndEscapeSequence(biSubjectPart, dataLength, i)) {
                            inJpn = false;
                            i += 3;
                            continue;
                        }
                        if (inJpn && i <= dataLength - 2) {

                            int code = JisUtils.MergeHL(biSubjectPart[i], biSubjectPart[i + 1]);
                            codeList.Add(code);

                            i += 2;
                            continue;
                        }
                        // 未対応
                        return null;
                    }
                }
            }

            return codeList;
        }

        // ファイルに保存する
        private void SaveToFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write)) {

                // ヘッダ部を出力する
                {
                    byte[] biHeaders = Encoding.ASCII.GetBytes(HEADERS);
                    fs.Write(biHeaders, 0, biHeaders.Length);

                    string subject = this.GetSubjectString();
                    byte[] biSubject = Encoding.ASCII.GetBytes(subject);
                    fs.Write(biSubject, 0, biSubject.Length);
                }

                fs.Write(JisUtils.CRLF, 0, JisUtils.CRLF.Length);

                this.WriteBody(fs);
            }
        }

        // 文字コードデータをメールのボディ部としてストリームに出力する
        private void WriteBody(Stream stream)
        {
            List<int> contents = this.editEmojiOperationBody.Contents;

            int useRows = DataBags.Config.BodyMaxRows;
            {
                int row = DataBags.Config.BodyMaxRows - 1;
                while (0 <= row) {
                    int useCols = Commons.GetUseCols(DataBags.Config.BodyMaxCols, contents, row);
                    if (useCols == 0) {
                        --row;
                    } else {
                        break;
                    }
                }
                useRows = row + 1;
            }

            for (int row = 0; row < useRows; ++row) {

                // 行ごとに日本語エスケープして内容を出力する
                int useCols = Commons.GetUseCols(DataBags.Config.BodyMaxCols, contents, row);
                if (0 < useCols) {
                    JisUtils.WriteBeginEscapeSequence(stream);

                    for (int col = 0; col < useCols; ++col) {
                        int index = DataBags.Config.BodyMaxCols * row + col;
                        int code = contents[index];
                        if (code == 0) {
                            JisUtils.WriteWhiteSpace(stream);
                        } else {
                            (byte high, byte low) = JisUtils.SplitHL(code);

                            stream.WriteByte(high);
                            stream.WriteByte(low);
                        }
                    }

                    JisUtils.WriteEndEscapeSequence(stream);
                }

                JisUtils.WriteCrLf(stream);
            }
        }
    }
}
