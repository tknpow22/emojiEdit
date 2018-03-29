namespace emojiEdit
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using MimeKit;

    static class MailMessageUtils
    {
        #region 処理

        /// <summary>
        /// ファイルから読込(宛先、送信元は読み込まない)
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <returns>(件名, 本文)</returns>
        public static (string, string) LoadFromFile(string filename)
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
                    subjectText = GetSubjectText(headers);
                }

                bodyText = GetBodyText(data, indexBeginBody, data.Length - indexBeginBody);
            }

            return (subjectText, bodyText);
        }

        /// <summary>
        /// ファイルへ保存(宛先、送信元は保存しない)
        /// </summary>
        /// <param name="subjectText">件名</param>
        /// <param name="bodyText">本文</param>
        /// <param name="filename">ファイル名</param>
        public static void SaveToFile(string subjectText, string bodyText, string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write)) {
                MailMessage mailMessage = new MailMessage(
                    "",
                    "",
                    subjectText.Trim(),
                    bodyText.Trim());

                MimeMessage mimeMessage = mailMessage.GetMimeMessage();
                mimeMessage.WriteTo(fs);
            }
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// ヘッダ文字列から件名を取得する
        /// NOTE: 自分で出力した形式にしか対応していない
        /// </summary>
        /// <param name="headers">ヘッダ文字列</param>
        /// <returns>件名</returns>
        private static string GetSubjectText(string headers)
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
        private static string GetBodyText(byte[] data, int offset, int count)
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

        #endregion
    }
}
