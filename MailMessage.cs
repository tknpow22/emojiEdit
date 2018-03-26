namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using MimeKit;

    #region 例外定義

    /// <summary>
    /// 宛先での例外
    /// </summary>
    class MailMessageAddressToException : Exception
    {
    }

    /// <summary>
    /// 送信元での例外
    /// </summary>
    class MailMessageAddressFromException : Exception
    {
    }

    /// <summary>
    /// 件名での例外
    /// </summary>
    class MailMessageEncodeSubjectException : Exception
    {
    }

    #endregion

    /// <summary>
    /// メール
    /// </summary>
    class MailMessage
    {
        #region 定数

        /// <summary>
        /// 件名ヘッダ名
        /// </summary>
        public const string SUBJECT = "Subject:";

        #endregion

        #region 内部変数

        /// <summary>
        /// 宛先
        /// </summary>
        private string to;

        /// <summary>
        /// 送信元
        /// </summary>
        private string from;

        /// <summary>
        /// 件名
        /// </summary>
        private string subject;

        /// <summary>
        /// 本文
        /// </summary>
        private string body;

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="to">宛先</param>
        /// <param name="from">送信元</param>
        /// <param name="subject">件名</param>
        /// <param name="body">本文</param>
        public MailMessage(string to, string from, string subject, string body)
        {
            this.to = to;
            this.from = from;

            // 件名
            {
                subject = UnifyLinefeedCodes(subject);
                subject = subject.Replace("\n", "");    // 改行を除く
                subject = StringUtils.ToZenkakuWithHankakuKanaOnly(subject);
            }
            this.subject = subject;

            // 本文
            {
                body = UnifyLinefeedCodes(body);
                ////body = body.Replace("\n.\n", "\n..\n"); // ドット対応: MailKit が対応するため処理しない
                body = StringUtils.ToZenkakuWithHankakuKanaOnly(body);
            }
            this.body = body;
        }

        /// <summary>
        /// 送信用の MimeMessage を取得する
        /// </summary>
        /// <returns>MimeMessage</returns>
        public MimeMessage GetMimeMessage()
        {
            List<MailboxAddress> mailboxAddressToList = new List<MailboxAddress>();
            try {
                string[] toList = this.to.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string mail in toList) {
                    mailboxAddressToList.Add(new MailboxAddress(mail));
                }
            } catch (Exception) {
                throw new MailMessageAddressToException();
            }

            MailboxAddress mailboxAddressFrom;
            try {
                mailboxAddressFrom = new MailboxAddress(this.from);
            } catch (Exception) {
                throw new MailMessageAddressFromException();
            }

            Header subjectHeader;
            try {
                subjectHeader = this.GetSubjectHeader();
                if (subjectHeader == null) {
                    throw new MailMessageEncodeSubjectException();
                }
            } catch (Exception ex) {
                throw ex;
            }

            try {

                using (MemoryStream contentStream = new MemoryStream()) {

                    MemoryStream bodyStream = GetBodyStream(this.body);
                    bodyStream.WriteTo(contentStream);

                    MimePart mimePart = new MimePart("text/plain; charset=ISO-2022-JP; format=flowed; delsp=yes");
                    mimePart.ContentTransferEncoding = ContentEncoding.SevenBit;
                    mimePart.Content = new MimeContent(bodyStream);

                    MimeMessage mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.AddRange(mailboxAddressToList);
                    mimeMessage.Headers.Replace(subjectHeader);
                    mimeMessage.Body = mimePart;

                    return mimeMessage;
                }

            } catch (Exception ex) {
                throw ex;
            }
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 件名のヘッダを得る
        /// </summary>
        /// <returns>Header</returns>
        private Header GetSubjectHeader()
        {
            Header result = null;

            string subject = this.GetBase64Subject();

            Header header;
            if (Header.TryParse(subject, out header)) {
                result = header;
            }

            return result;
        }

        /// <summary>
        /// BASE64 エンコーディングした件名を得る
        /// </summary>
        /// <returns>件名(BASE64)</returns>
        private string GetBase64Subject()
        {
            List<string> base64EncodingBlocks = GetBase64EncodingBlocks(this.subject, 10);

            StringBuilder subject = new StringBuilder();
            subject.Append(SUBJECT);
            if (base64EncodingBlocks.Count == 0) {
                subject.Append(" \r\n");
            } else {
                subject.Append(string.Join("", base64EncodingBlocks));
            }

            return subject.ToString();
        }

        /// <summary>
        /// 件名を固定長に分け、BASE64 エンコーディングして返す
        /// NOTE: 改行は処理できないので渡さないこと
        /// NOTE: 半角カタカナは処理できないので渡さないこと
        /// </summary>
        /// <param name="subjectText">件名</param>
        /// <param name="maxCols">件名を分けるための長さ</param>
        /// <returns>複数の BASE64 文字列の一覧</returns>
        public static List<string> GetBase64EncodingBlocks(string subjectText, int maxCols)
        {
            List<string> base64EncodingBlocks = new List<string>();

            MemoryStream stream = new MemoryStream();

            int cols = 0;
            bool jisIn = false;

            for (int i = 0; i < subjectText.Length; ++i) {
                string sch = subjectText.Substring(i, 1);

                if (StringUtils.IsAsciiWithControl(sch)) {
                    if (jisIn) {
                        CharCodeUtils.WriteJisEndEscapeSequence(stream);
                    }

                    jisIn = false;

                    char ascii = sch[0];
                    stream.WriteByte((byte)(ascii & 0xFF));
                    ++cols;

                } else {
                    if (!jisIn) {
                        CharCodeUtils.WriteJisBeginEscapeSequence(stream);
                    }
                    jisIn = true;

                    int jiscode;
                    Emoji emoji = DataBags.Emojis.GetFromUnicode(sch[0]);
                    if (emoji != null) {
                        jiscode = emoji.Jiscode;
                    } else {
                        jiscode = CharCodeUtils.GetJisCodeFromChar(sch);
                        if (jiscode == 0) {
                            jiscode = CharCodeUtils.JIS_WHITE_SPACE_CODE;
                        }
                    }

                    (byte high, byte low) = CharCodeUtils.SplitHL(jiscode);

                    stream.WriteByte(high);
                    stream.WriteByte(low);
                    ++cols;
                }

                if (maxCols <= cols) {

                    if (jisIn) {
                        CharCodeUtils.WriteJisEndEscapeSequence(stream);
                    }
                    base64EncodingBlocks.Add(GetBase64EncodingBlock(stream));
                    stream = new MemoryStream();
                    if (jisIn) {
                        CharCodeUtils.WriteJisBeginEscapeSequence(stream);
                    }

                    cols = 0;
                }
            }
            if (jisIn) {
                CharCodeUtils.WriteJisEndEscapeSequence(stream);
            }
            if (0 < stream.Length) {
                base64EncodingBlocks.Add(GetBase64EncodingBlock(stream));
            }

            return base64EncodingBlocks;
        }

        /// <summary>
        /// 改行コードを統一する
        /// </summary>
        /// <param name="str">文字列</param>
        /// <returns>改行コード変更後の文字列</returns>
        private static string UnifyLinefeedCodes(string str)
        {
            str = str.Replace("\r\n", "\n");
            str = str.Replace("\r", "\n");
            return str;
        }

        /// <summary>
        /// BASE64 エンコーディングの 1 つのブロックを取得する
        /// </summary>
        /// <param name="stream">データが格納された MemoryStream</param>
        /// <returns>BASE64 文字列</returns>
        private static string GetBase64EncodingBlock(MemoryStream stream)
        {
            byte[] blockData = stream.ToArray();
            string base64Text = Convert.ToBase64String(blockData, 0, blockData.Length);
            string base64EncodingBlock = string.Format(" =?iso-2022-jp?b?{0}?=\r\n", base64Text);
            return base64EncodingBlock;
        }

        /// <summary>
        /// 本文を JIS エンコーディングした MemoryStream を返す
        /// NOTE: 半角カタカナは処理できないので渡さないこと
        /// </summary>
        /// <param name="bodyText">本文</param>
        /// <returns>エンコーディング内容を格納した MemoryStream</returns>
        private static MemoryStream GetBodyStream(string bodyText)
        {
            int maxBodyCols;
            if (DataBags.Config.ForceInsertLineFeed) {
                maxBodyCols = DataBags.Config.MaxBodyCols * 2;
            } else {
                maxBodyCols = int.MaxValue;
            }

            MemoryStream stream = new MemoryStream();

            int cols = 0;
            bool jisIn = false;

            for (int i = 0; i < bodyText.Length; ++i) {
                string sch = bodyText.Substring(i, 1);

                if (StringUtils.IsAsciiWithControl(sch)) {
                    if (jisIn) {
                        CharCodeUtils.WriteJisEndEscapeSequence(stream);
                    }

                    jisIn = false;

                    if (sch[0] == '\n') {
                        CharCodeUtils.WriteCrLf(stream);
                        cols = 0;
                    } else {
                        char ascii = sch[0];
                        stream.WriteByte((byte)(ascii & 0xFF));
                        ++cols;
                    }

                } else {
                    if (!jisIn) {
                        CharCodeUtils.WriteJisBeginEscapeSequence(stream);
                    }
                    jisIn = true;

                    int jiscode;
                    Emoji emoji = DataBags.Emojis.GetFromUnicode(sch[0]);
                    if (emoji != null) {
                        jiscode = emoji.Jiscode;
                    } else {
                        jiscode = CharCodeUtils.GetJisCodeFromChar(sch);
                        if (jiscode == 0) {
                            jiscode = CharCodeUtils.JIS_WHITE_SPACE_CODE;
                        }
                    }

                    (byte high, byte low) = CharCodeUtils.SplitHL(jiscode);

                    stream.WriteByte(high);
                    stream.WriteByte(low);
                    cols += 2;
                }

                if (maxBodyCols <= cols) {

                    if (jisIn) {
                        CharCodeUtils.WriteJisEndEscapeSequence(stream);
                    }
                    CharCodeUtils.WriteCrLfWithFlowed(stream);
                    if (jisIn) {
                        CharCodeUtils.WriteJisBeginEscapeSequence(stream);
                    }

                    cols = 0;
                }
            }
            if (jisIn) {
                CharCodeUtils.WriteJisEndEscapeSequence(stream);
            }

            return stream;
        }

        #endregion
    }
}
