namespace emojiEdit
{
    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using MailKit;
    using MailKit.Net.Smtp;
    using MimeKit;

    /// <summary>
    /// 送信
    /// </summary>
    public partial class SendMailForm : Form
    {
        #region 変数

        /// <summary>
        /// 送信メッセージ
        /// </summary>
        private MimeMessage message;

        #endregion

        #region 処理

        /// <summary>
        /// コンスラクタ
        /// </summary>
        /// <param name="message"></param>
        public SendMailForm(MimeMessage message)
        {
            InitializeComponent();

            this.message = message;

            this.textBoxSmtpServer.Text = DataBags.Config.SmtpServer;
            this.numericUpDownSmtpPort.Value = DataBags.Config.SmtpPort;
            this.textBoxSmtpUserId.Text = DataBags.Config.SmtpUserId;
            this.textBoxSmtpPassword.Text = DataBags.Config.SmtpPassword;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// 送信ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, EventArgs e)
        {
            this.textBoxLog.Clear();

            // チェック(必須)
            {
                TextBox[] requiredTextBoxes = {
                    this.textBoxSmtpServer,
                    this.textBoxSmtpUserId,
                    this.textBoxSmtpPassword,
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
                    this.textBoxSmtpServer,
                    this.textBoxSmtpUserId,
                    this.textBoxSmtpPassword,
                };

                foreach (TextBox hankakuTextBox in hankakuTextBoxes) {
                    if (!StringUtils.IsAscii(hankakuTextBox.Text.Trim())) {
                        MsgBox.Show(this, string.Format("「{0}」は半角で入力してください。", hankakuTextBox.Tag), "半角入力", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        hankakuTextBox.Focus();
                        return;
                    }
                }
            }

            // NOTE: ポート番号のチェックは NumericUpDown コントロールの設定で代替する

            string smtpServer = this.textBoxSmtpServer.Text.Trim();
            int smtpPort = (int)this.numericUpDownSmtpPort.Value;
            string smtpUserId = this.textBoxSmtpUserId.Text.Trim();
            string smtpPassword = this.textBoxSmtpPassword.Text.Trim();

            // 送信
            {
                this.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                try {

                    bool rc;
                    using (MemoryStream logStream = new MemoryStream())
                    using (MemoryStream logMailStream = new MemoryStream()) {
                        rc = this.Send(
                            smtpServer,
                            smtpPort,
                            smtpUserId,
                            smtpPassword,
                            this.message,
                            logStream,
                            logMailStream);

                        this.SetLog(logStream);
                        this.LogMail(logMailStream);
                    }

                    if (rc) {
                        DataBags.Config.SmtpServer = smtpServer;
                        DataBags.Config.SmtpPort = smtpPort;
                        DataBags.Config.SmtpUserId = smtpUserId;
                        DataBags.Config.SmtpPassword = smtpPassword;

                        if (0 < message.From.Count) {
                            MailboxAddress mailboxAddress = message.From[0] as MailboxAddress;
                            if (mailboxAddress != null) {
                                DataBags.Config.MailFrom = mailboxAddress.Address;
                            }
                        }

                        MsgBox.Show(this, "送信が完了しました。", "送信完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    } else {
                        MsgBox.Show(this, "送信に失敗しました。", "送信失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                } catch (Exception ex) {
                    MsgBox.Show(this, ex.Message, "送信できません", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.Enabled = true;
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 閉じるボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 送信処理
        /// </summary>
        /// <param name="smtpServer">SMTP サーバー名</param>
        /// <param name="smtpPort">SMTP ポート番号</param>
        /// <param name="smtpUserId">SMTP ユーザーID</param>
        /// <param name="smtpPassword">SMTP パスワード</param>
        /// <param name="message">メッセージ</param>
        /// <param name="logStream">ログ用 Stream</param>
        /// <param name="logMailStream">メールログ用 Stream</param>
        /// <returns></returns>
        private bool Send(
            string smtpServer,
            int smtpPort,
            string smtpUserId,
            string smtpPassword,
            MimeMessage message,
            Stream logStream,
            Stream logMailStream) {

            bool result = false;

            try {

                using (ProtocolLogger logger = new ProtocolLogger(logStream, true))
                using (SmtpClient smtpClient = new SmtpClient(logger)) {

                    smtpClient.Connect(smtpServer, smtpPort);
                    smtpClient.Authenticate(smtpUserId, smtpPassword);

                    smtpClient.Send(message);

                    smtpClient.Disconnect(true);
                }

                this.message.WriteTo(logMailStream);

                result = true;

            } catch (Exception ex) {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// ログを表示する
        /// </summary>
        /// <param name="logStream">ログ用 Stream</param>
        private void SetLog(MemoryStream logStream)
        {
            logStream.Seek(0, SeekOrigin.Begin);

            byte[] buffer = new byte[logStream.Length];
            int length;
            StringBuilder logText = new StringBuilder();

            while (0 < (length = logStream.Read(buffer, 0, buffer.Length))) {
                string logStr = "";
                try {
                    logStr = Encoding.GetEncoding("ISO-2022-JP").GetString(buffer, 0, length);
                } catch (Exception) {
                    logStr = Encoding.ASCII.GetString(buffer, 0, length);
                }
                logText.Append(logStr);
            }
            this.textBoxLog.Text = logText.ToString();
        }

        /// <summary>
        /// メール内容を保存する
        /// </summary>
        /// <param name="mailStream">メールログ用 Stream</param>
        public void LogMail(MemoryStream mailStream)
        {
            try {
                DateTime now = DateTime.Now;

                string subDirectory = now.ToString("yyyyMMdd");
                string filename = now.ToString("yyyyMMddHHmmssfff");

                string mailLogSubDirectory = Path.Combine(Path.Combine(DataBags.Config.AppDirectory, Commons.MAIL_LOG_DIR), subDirectory);
                Directory.CreateDirectory(mailLogSubDirectory);

                string filepath = Path.Combine(mailLogSubDirectory, filename + ".eml");
                File.WriteAllBytes(filepath, mailStream.ToArray());
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
