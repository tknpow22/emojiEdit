using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace emojiEdit
{
    //
    // 設定
    //
    class ConfigBag
    {
        //
        // INI ファイルの読み書き
        //

        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
               string lpApplicationName,
               string lpKeyName,
               string lpDefault,
               StringBuilder lpReturnedstring,
               int nSize,
               string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        //
        // 定数
        //

        // BODY 部の文字数と行数

        // BODY_MAX_COLS_MINIMUM <= BODY_MAX_COLS_DEFALUT <= BODY_MAX_COLS_MAXIMUM となること
        public const int BODY_MAX_COLS_MINIMUM = 9;
        private const int BODY_MAX_COLS_DEFALUT = 12;
        public const int BODY_MAX_COLS_MAXIMUM = 20;

        // BODY_MAX_ROWS_MINIMUM <= BODY_MAX_ROWS_DEFAULT <= BODY_MAX_ROWS_MAXIMUM となること
        public const int BODY_MAX_ROWS_MINIMUM = 70;
        private const int BODY_MAX_ROWS_DEFAULT = 100;
        public const int BODY_MAX_ROWS_MAXIMUM = 200;

        // 絵文字一覧の文字数
        // EMOJI_LIST_MAX_COLS_MINIMUM <= EMOJI_LIST_MAX_COLS_DEFAULT <= EMOJI_LIST_MAX_COLS_MAXIMUM となること
        public const int EMOJI_LIST_MAX_COLS_MINIMUM = 6;
        private const int EMOJI_LIST_MAX_COLS_DEFAULT = 9;
        public const int EMOJI_LIST_MAX_COLS_MAXIMUM = 20;

        // デフォルトの SMTP ポート番号

        public const int SMTP_PORT_DEFAULT = 587;

        //
        // 設定ファイル定数
        //

        // セクション名
        private const string SECTION_NAME = "Config";

        //
        // キー名
        //

        // 絵文字履歴
        private const string KEY_EMOJI_HISTORY = "EmojiHistory";

        // メイン画面のサイズ
        private const string KEY_MAIN_WINDOW_WIDTH = "MainWindowWidth";
        private const string KEY_MAIN_WINDOW_HEIGHT = "MainWindowHeight";

        // BODY 部の文字数と行数
        private const string KEY_BODY_MAX_COLS = "BodyMaxCols";
        private const string KEY_BODY_MAX_ROWS = "BodyMaxRows";

        // 絵文字一覧の文字数
        private const string KEY_EMOJI_LIST_MAX_COLS = "EmojiListMaxCols";

        // SMTP サーバー
        private const string KEY_SMTP_SERVER = "SmtpServer";
        private const string KEY_SMTP_PORT = "SmtpPort";
        private const string KEY_SMTP_USER_ID = "SmtpUserId";
        private const string KEY_SMTP_PASSWORD = "SmtpPassword";

        // 送信情報
        private const string KEY_MAIL_FROM = "MailFrom";

        //
        // 変数
        //

        // アプリケーションのディレクトリ
        private string appDirectory;

        // 設定ファイルパス
        private string filepath;

        //
        // 処理
        //

        // コンストラクタ
        public ConfigBag()
        {
            this.appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            this.filepath = Path.Combine(this.appDirectory, Commons.CONFIG_FILE_NAME);
        }

        // 初期化処理
        public void Init()
        {
            // 絵文字履歴
            this.emojiHistory = new List<int>();

            // 画面サイズ
            this.MainWindowWidth = 0;
            this.MainWindowHeight = 0;

            // BODY 部の文字数と行数
            this.BodyMaxCols = BODY_MAX_COLS_DEFALUT;
            this.BodyMaxRows = BODY_MAX_ROWS_DEFAULT;

            // 絵文字一覧の文字数
            this.EmojiListMaxCols = EMOJI_LIST_MAX_COLS_DEFAULT;

            // SMTP サーバー
            this.SmtpServer = "";
            this.SmtpPort = SMTP_PORT_DEFAULT;
            this.SmtpUserId = "";
            this.SmtpPassword = "";

            // 送信情報
            this.MailFrom = "";


            //
            // 読み込み
            //
            this.Load();
        }

        // 書き込み
        public void Save()
        {
            // 絵文字履歴
            {
                StringBuilder buffer = new StringBuilder();
                foreach (int code in emojiHistory) {
                    if (buffer.Length != 0) {
                        buffer.Append(",");
                    }
                    buffer.AppendFormat("{0:X4}", code);
                }
                this.WriteString(SECTION_NAME, KEY_EMOJI_HISTORY, buffer.ToString());
            }

            // 画面サイズ
            {
                this.WriteInt(SECTION_NAME, KEY_MAIN_WINDOW_WIDTH, this.MainWindowWidth);
                this.WriteInt(SECTION_NAME, KEY_MAIN_WINDOW_HEIGHT, this.MainWindowHeight);
            }

            // BODY 部の文字数と行数
            {
                this.WriteInt(SECTION_NAME, KEY_BODY_MAX_COLS, this.BodyMaxCols);
                this.WriteInt(SECTION_NAME, KEY_BODY_MAX_ROWS, this.BodyMaxRows);
            }

            // 絵文字一覧の文字数
            {
                this.WriteInt(SECTION_NAME, KEY_EMOJI_LIST_MAX_COLS, this.EmojiListMaxCols);
            }

            // SMTP サーバー
            {
                this.WriteString(SECTION_NAME, KEY_SMTP_SERVER, this.SmtpServer);
                this.WriteInt(SECTION_NAME, KEY_SMTP_PORT, this.SmtpPort);
                this.WriteString(SECTION_NAME, KEY_SMTP_USER_ID, this.SmtpUserId);
                this.WriteString(SECTION_NAME, KEY_SMTP_PASSWORD, this.SmtpPassword);
            }

            // 送信情報
            {
                this.WriteString(SECTION_NAME, KEY_MAIL_FROM, this.MailFrom);
            }
        }

        //
        // プロパティ
        //

        // アプリケーションのディレクトリ
        public string AppDirectory
        {
            get {
                return this.appDirectory;
            }
        }

        // 絵文字履歴
        private List<int> emojiHistory;

        public List<int> GetEmojiHistory()
        {
            return new List<int>(this.emojiHistory);
        }

        public void SetEmojiHistory(List<int> emojiHistory)
        {
            this.emojiHistory = new List<int>(emojiHistory);
        }

        // メイン画面のサイズ
        public int MainWindowWidth
        {
            get; set;
        }

        public int MainWindowHeight
        {
            get; set;
        }

        // BODY 部の文字数と行数
        public int BodyMaxCols
        {
            get; set;
        }

        public int BodyMaxRows
        {
            get; set;
        }

        // 絵文字一覧の文字数
        public int EmojiListMaxCols
        {
            get; set;
        }

        // SMTP サーバー
        public string SmtpServer
        {
            get; set;
        }

        public int SmtpPort
        {
            get; set;
        }

        public string SmtpUserId
        {
            get; set;
        }

        public string SmtpPassword
        {
            get; set;
        }

        // 送信情報
        public string MailFrom
        {
            get; set;
        }

        //
        // 内部処理
        //

        // 読み込み
        private void Load()
        {
            // 絵文字履歴
            {
                string emojiHistoryText = this.GetString(SECTION_NAME, KEY_EMOJI_HISTORY);

                string[] histories = emojiHistoryText.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string history in histories) {
                    int code;

                    if (history.Length != 4) {
                        continue;
                    }

                    if (int.TryParse(history, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out code)
                     && code != 0 && !this.emojiHistory.Contains(code)) {
                        this.emojiHistory.Add(code);
                    }
                }
            }

            // 画面サイズ
            {
                int wndWidth = this.GetInt(SECTION_NAME, KEY_MAIN_WINDOW_WIDTH);
                int wndHeight = this.GetInt(SECTION_NAME, KEY_MAIN_WINDOW_HEIGHT);

                if (wndWidth < Commons.MinWndWidth || Commons.MaxWndWidth < wndWidth) {
                    wndWidth = Commons.MinWndWidth;
                }
                if (wndHeight < Commons.MinWndHeight || Commons.MaxWndHeight < wndHeight) {
                    wndHeight = Commons.MinWndHeight;
                }

                this.MainWindowWidth = wndWidth;
                this.MainWindowHeight = wndHeight;
            }

            // BODY 部の文字数と行数
            {
                int maxCols = this.GetInt(SECTION_NAME, KEY_BODY_MAX_COLS, BODY_MAX_COLS_DEFALUT);
                int maxRows = this.GetInt(SECTION_NAME, KEY_BODY_MAX_ROWS, BODY_MAX_ROWS_DEFAULT);

                if (maxCols < BODY_MAX_COLS_MINIMUM || BODY_MAX_COLS_MAXIMUM < maxCols) {
                    maxCols = BODY_MAX_COLS_DEFALUT;
                }
                if (maxRows < BODY_MAX_ROWS_MINIMUM || BODY_MAX_ROWS_MAXIMUM < maxRows) {
                    maxRows = BODY_MAX_ROWS_DEFAULT;
                }

                this.BodyMaxCols = maxCols;
                this.BodyMaxRows = maxRows;
            }

            // 絵文字一覧の文字数
            {
                int maxCols = this.GetInt(SECTION_NAME, KEY_EMOJI_LIST_MAX_COLS, EMOJI_LIST_MAX_COLS_DEFAULT);

                if (maxCols < EMOJI_LIST_MAX_COLS_MINIMUM || EMOJI_LIST_MAX_COLS_MAXIMUM < maxCols) {
                    maxCols = EMOJI_LIST_MAX_COLS_DEFAULT;
                }

                this.EmojiListMaxCols = maxCols;
            }

            // SMTP サーバー
            {
                string smtpServer = this.GetString(SECTION_NAME, KEY_SMTP_SERVER);
                int smtpPort = this.GetInt(SECTION_NAME, KEY_SMTP_PORT, SMTP_PORT_DEFAULT);
                string smtpUserId = this.GetString(SECTION_NAME, KEY_SMTP_USER_ID);
                string smtpPassword = this.GetString(SECTION_NAME, KEY_SMTP_PASSWORD);

                if (smtpPort <= 0 || 65536 <= smtpPort) {
                    smtpPort = SMTP_PORT_DEFAULT;
                }

                this.SmtpServer = smtpServer;
                this.SmtpPort = smtpPort;
                this.SmtpUserId = smtpUserId;
                this.SmtpPassword = smtpPassword;
            }

            // 送信情報
            {
                string mailFrom = this.GetString(SECTION_NAME, KEY_MAIL_FROM);

                this.MailFrom = mailFrom;
            }
        }

        // 設定ファイルから文字列を取得する
        private string GetString(string lpApplicationName, string lpKeyName, string lpDefault = "")
        {
            StringBuilder buffer = new StringBuilder(2560);
            GetPrivateProfileString(lpApplicationName, lpKeyName, lpDefault, buffer, buffer.Capacity, this.filepath);
            return buffer.ToString();
        }

        // 設定ファイルから数値を取得する
        private int GetInt(string lpApplicationName, string lpKeyName, int nDefault = 0)
        {
            string str = this.GetString(lpApplicationName, lpKeyName, "");
            int result;

            if (int.TryParse(str, out result)) {
                return result;
            }

            return nDefault;
        }

        // 設定ファイルに文字列を書き込む
        private void WriteString(string lpApplicationName, string lpKeyName, string lpString)
        {
            WritePrivateProfileString(lpApplicationName, lpKeyName, lpString, this.filepath);
        }

        // 設定ファイルに数値を文字列として書き込む
        private void WriteInt(string lpApplicationName, string lpKeyName, int nInt)
        {
            string sInt = string.Format("{0}", nInt);
            this.WriteString(lpApplicationName, lpKeyName, sInt);
        }
    }
}
