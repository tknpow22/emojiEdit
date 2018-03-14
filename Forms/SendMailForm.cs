using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace emojiEdit
{
    //
    // 送信
    //
    public partial class SendMailForm : Form
    {
        // 送信メッセージ
        MimeMessage message;

        // コンスラクタ
        public SendMailForm(MimeMessage message)
        {
            InitializeComponent();

            this.message = message;

            this.textBoxSmtpServer.Text = DataBags.Config.SmtpServer;
            this.numericUpDownSmtpPort.Value = DataBags.Config.SmtpPort;
            this.textBoxSmtpUserId.Text = DataBags.Config.SmtpUserId;
            this.textBoxSmtpPassword.Text = DataBags.Config.SmtpPassword;
        }

        //
        // イベントハンドラ
        //

        // 送信
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

            // チェック(半角)
            {
                TextBox[] hankakuTextBoxes = {
                    this.textBoxSmtpServer,
                    this.textBoxSmtpUserId,
                    this.textBoxSmtpPassword,
                };

                foreach (TextBox hankakuTextBox in hankakuTextBoxes) {
                    if (!StringUtils.IsHankaku(hankakuTextBox.Text.Trim())) {
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
                    using (MemoryStream mailStream = new MemoryStream()) {
                        rc = this.Send(
                            smtpServer,
                            smtpPort,
                            smtpUserId,
                            smtpPassword,
                            this.message,
                            logStream,
                            mailStream);

                        this.SetLog(logStream);
                        this.LogMail(mailStream);
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

        // 閉じる
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // フォーカスロスト
        private void textBoxToHankaku_Leave(object sender, EventArgs e)
        {
            TextBox control = (TextBox)sender;
            control.Text = StringUtils.ToHankaku(control.Text);
        }

        //
        // 内部処理
        //

        // 送信
        private bool Send(
            string smtpServer,
            int smtpPort,
            string smtpUserId,
            string smtpPassword,
            MimeMessage message,
            Stream logStream,
            Stream mailStream) {

            bool result = false;

            try {

                using (ProtocolLogger logger = new ProtocolLogger(logStream, true))
                using (SmtpClient smtpClient = new SmtpClient(logger)) {

                    smtpClient.Connect(smtpServer, smtpPort);
                    smtpClient.Authenticate(smtpUserId, smtpPassword);

                    smtpClient.Send(message);

                    smtpClient.Disconnect(true);
                }

                this.message.WriteTo(mailStream);

                result = true;

            } catch (Exception ex) {
                throw ex;
            }

            return result;
        }

        // ログを表示する
        private void SetLog(MemoryStream logStream)
        {
            logStream.Seek(0, SeekOrigin.Begin);

            byte[] buffer = new byte[logStream.Length];
            int length;
            StringBuilder logText = new StringBuilder();

            while (0 < (length = logStream.Read(buffer, 0, buffer.Length))) {
                try {
                    string logStr = Encoding.ASCII.GetString(buffer, 0, length);
                    logText.Append(logStr);
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            this.textBoxLog.Text = logText.ToString();
        }

        // メール内容を保存する
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
    }
}
