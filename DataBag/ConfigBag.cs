namespace emojiEdit
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// 設定
    /// </summary>
    class ConfigBag : CDataBag
    {
        #region INI ファイルの読み書き

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

        #endregion

        #region 設定ファイル定数

        /// <summary>
        /// セクション名
        /// </summary>
        private const string SECTION_NAME = "Config";

        #region キー名

        /// <summary>
        /// 絵文字履歴
        /// </summary>
        private const string KEY_EMOJI_HISTORY = "EmojiHistory";

        #region メイン画面のサイズ

        /// <summary>
        /// メイン画面のサイズ(幅)
        /// </summary>
        private const string KEY_MAIN_WINDOW_WIDTH = "MainWindowWidth";

        /// <summary>
        /// メイン画面のサイズ(高さ)
        /// </summary>
        private const string KEY_MAIN_WINDOW_HEIGHT = "MainWindowHeight";

        #endregion

        /// <summary>
        /// 本文の文字数
        /// </summary>
        private const string KEY_MAX_BODY_COLS = "MaxBodyCols";

        /// <summary>
        /// 絵文字一覧の文字数
        /// </summary>
        private const string KEY_MAX_EMOJI_LIST_COLS = "MaxEmojiListCols";

        #region SMTP サーバー設定

        /// <summary>
        /// SMTP サーバー設定(ホスト名)
        /// </summary>
        private const string KEY_SMTP_SERVER = "SmtpServer";

        /// <summary>
        /// SMTP サーバー設定(ポート)
        /// </summary>
        private const string KEY_SMTP_PORT = "SmtpPort";

        /// <summary>
        /// SMTP サーバー設定(ユーザーID)
        /// </summary>
        private const string KEY_SMTP_USER_ID = "SmtpUserId";

        /// <summary>
        /// SMTP サーバー設定(パスワード)
        /// </summary>
        private const string KEY_SMTP_PASSWORD = "SmtpPassword";

        #endregion

        /// <summary>
        /// 送信元
        /// </summary>
        private const string KEY_MAIL_FROM = "MailFrom";

        /// <summary>
        /// メール本文に1行の文字数毎に改行を入れる
        /// </summary>
        private const string KEY_FORCE_INSERT_LINE_FEED = "ForceInsertLineFeed";

        ///// <summary>
        ///// 本文のちらつきを少しだけ抑制する
        ///// </summary>
        //private const string KEY_SUPPRESS_BODY_TEXT_FLICKER = "SuppressBodyTextFlicker";

        #endregion

        #endregion

        #region 変数

        /// <summary>
        /// アプリケーションのディレクトリ
        /// </summary>
        private string appDirectory;

        /// <summary>
        /// 設定ファイルパス
        /// </summary>
        private string filepath;

        /// <summary>
        /// 絵文字履歴
        /// </summary>
        private List<int> emojiHistory;

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConfigBag()
        {
            this.appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            this.filepath = Path.Combine(this.appDirectory, Commons.CONFIG_FILE_NAME);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            // 絵文字履歴
            this.emojiHistory = new List<int>();

            // 画面サイズ
            this.MainWindowWidth = 0;
            this.MainWindowHeight = 0;

            // 本文の文字数と行数
            this.MaxBodyCols = Commons.MAX_BODY_COLS_DEFALUT;

            // 絵文字一覧の文字数
            this.MaxEmojiListCols = Commons.MAX_EMOJI_LIST_COLS_DEFAULT;

            // SMTP サーバー
            this.SmtpServer = "";
            this.SmtpPort = Commons.SMTP_PORT_DEFAULT;
            this.SmtpUserId = "";
            this.SmtpPassword = "";

            // 送信情報
            this.MailFrom = "";

            // メール本文に1行の文字数毎に改行を入れる
            this.ForceInsertLineFeed = false;

            //// 本文のちらつきを少しだけ抑制する
            //this.SuppressBodyTextFlicker = false;

            //
            // 読み込み
            //
            this.Load();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            this.Save();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// アプリケーションのディレクトリ
        /// </summary>
        public string AppDirectory
        {
            get {
                return this.appDirectory;
            }
        }

        #region 絵文字履歴

        /// <summary>
        /// 絵文字履歴を取得する
        /// </summary>
        /// <returns>絵文字履歴</returns>
        public List<int> GetEmojiHistory()
        {
            return new List<int>(this.emojiHistory);
        }

        /// <summary>
        /// 絵文字履歴を設定する
        /// </summary>
        /// <param name="emojiHistory">絵文字履歴</param>
        public void SetEmojiHistory(List<int> emojiHistory)
        {
            this.emojiHistory = new List<int>(emojiHistory);
        }

        #endregion

        #region メイン画面のサイズ

        /// <summary>
        /// メイン画面のサイズ(幅)
        /// </summary>
        public int MainWindowWidth
        {
            get; set;
        }

        /// <summary>
        /// メイン画面のサイズ(高さ)
        /// </summary>
        public int MainWindowHeight
        {
            get; set;
        }

        #endregion

        /// <summary>
        /// 本文の文字数
        /// </summary>
        public int MaxBodyCols
        {
            get; set;
        }

        /// <summary>
        /// 絵文字一覧の文字数
        /// </summary>
        public int MaxEmojiListCols
        {
            get; set;
        }

        #region SMTP サーバー設定

        /// <summary>
        /// SMTP サーバー設定(ホスト名)
        /// </summary>
        public string SmtpServer
        {
            get; set;
        }

        /// <summary>
        /// SMTP サーバー設定(ポート)
        /// </summary>
        public int SmtpPort
        {
            get; set;
        }

        /// <summary>
        /// SMTP サーバー設定(ユーザーID)
        /// </summary>
        public string SmtpUserId
        {
            get; set;
        }

        /// <summary>
        /// SMTP サーバー設定(パスワード)
        /// </summary>
        public string SmtpPassword
        {
            get; set;
        }

        #endregion

        /// <summary>
        /// 送信元
        /// </summary>
        public string MailFrom
        {
            get; set;
        }

        /// <summary>
        /// メール本文に1行の文字数毎に改行を入れる
        /// </summary>
        public bool ForceInsertLineFeed
        {
            get; set;
        }

        ///// <summary>
        ///// 本文のちらつきを少しだけ抑制する
        ///// </summary>
        //public bool SuppressBodyTextFlicker
        //{
        //    get; set;
        //}

        #endregion

        #region 内部処理

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load()
        {
            // 絵文字履歴
            {
                string emojiHistoryText = this.GetString(SECTION_NAME, KEY_EMOJI_HISTORY);

                List<int> codes = Commons.GetCodesFromHexaStrings(emojiHistoryText);
                var distinctCodes = codes.Distinct(); // 一意にする
                var nonezeroCodes = distinctCodes.Where(val => val != 0);   // 0 を除く
                this.emojiHistory.AddRange(nonezeroCodes);
            }

            // 画面サイズ
            {
                int wndWidth = this.GetInt(SECTION_NAME, KEY_MAIN_WINDOW_WIDTH);
                int wndHeight = this.GetInt(SECTION_NAME, KEY_MAIN_WINDOW_HEIGHT);

                if (wndWidth < Commons.MIN_MAIN_WINDOW_WIDTH || Commons.MAX_MAIN_WINDOW_WIDTH < wndWidth) {
                    wndWidth = Commons.MIN_MAIN_WINDOW_WIDTH;
                }
                if (wndHeight < Commons.MIN_MAIN_WINDOW_HEIGHT || Commons.MAX_MAIN_WINDOW_HEIGHT < wndHeight) {
                    wndHeight = Commons.MIN_MAIN_WINDOW_HEIGHT;
                }

                this.MainWindowWidth = wndWidth;
                this.MainWindowHeight = wndHeight;
            }

            // 本文の文字数
            {
                int maxCols = this.GetInt(SECTION_NAME, KEY_MAX_BODY_COLS, Commons.MAX_BODY_COLS_DEFALUT);

                if (maxCols < Commons.MAX_BODY_COLS_MINIMUM || Commons.MAX_BODY_COLS_MAXIMUM < maxCols) {
                    maxCols = Commons.MAX_BODY_COLS_DEFALUT;
                }

                this.MaxBodyCols = maxCols;
            }

            // 絵文字一覧の文字数
            {
                int maxCols = this.GetInt(SECTION_NAME, KEY_MAX_EMOJI_LIST_COLS, Commons.MAX_EMOJI_LIST_COLS_DEFAULT);

                if (maxCols < Commons.MAX_EMOJI_LIST_COLS_MINIMUM || Commons.MAX_EMOJI_LIST_COLS_MAXIMUM < maxCols) {
                    maxCols = Commons.MAX_EMOJI_LIST_COLS_DEFAULT;
                }

                this.MaxEmojiListCols = maxCols;
            }

            // SMTP サーバー
            {
                string smtpServer = this.GetString(SECTION_NAME, KEY_SMTP_SERVER);
                int smtpPort = this.GetInt(SECTION_NAME, KEY_SMTP_PORT, Commons.SMTP_PORT_DEFAULT);
                string smtpUserId = this.GetString(SECTION_NAME, KEY_SMTP_USER_ID);
                string smtpPassword = this.GetString(SECTION_NAME, KEY_SMTP_PASSWORD);

                if (smtpPort <= 0 || 65536 <= smtpPort) {
                    smtpPort = Commons.SMTP_PORT_DEFAULT;
                }

                this.SmtpServer = smtpServer;
                this.SmtpPort = smtpPort;
                this.SmtpUserId = smtpUserId;
                this.SmtpPassword = smtpPassword;
            }

            // 送信元
            {
                string mailFrom = this.GetString(SECTION_NAME, KEY_MAIL_FROM);

                this.MailFrom = mailFrom;
            }

            // メール本文に1行の文字数毎に改行を入れる
            {
                string forceInsertLineFeedStr = this.GetString(SECTION_NAME, KEY_FORCE_INSERT_LINE_FEED, "False");
                if (forceInsertLineFeedStr.ToLower() == "true") {
                    this.ForceInsertLineFeed = true;
                } else {
                    this.ForceInsertLineFeed = false;
                }
            }

            //// 本文のちらつきを少しだけ抑制する
            //{
            //    string suppressBodyTextFlickerStr = this.GetString(SECTION_NAME, KEY_SUPPRESS_BODY_TEXT_FLICKER, "False");
            //    if (suppressBodyTextFlickerStr.ToLower() == "true") {
            //        this.SuppressBodyTextFlicker = true;
            //    } else {
            //        this.SuppressBodyTextFlicker = false;
            //    }
            //}
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            // 絵文字履歴
            {
                string hexaStr = Commons.GetHexaStringsFromCodes(this.emojiHistory);
                this.WriteString(SECTION_NAME, KEY_EMOJI_HISTORY, hexaStr);
            }

            // 画面サイズ
            {
                this.WriteInt(SECTION_NAME, KEY_MAIN_WINDOW_WIDTH, this.MainWindowWidth);
                this.WriteInt(SECTION_NAME, KEY_MAIN_WINDOW_HEIGHT, this.MainWindowHeight);
            }

            // 本文の文字数
            {
                this.WriteInt(SECTION_NAME, KEY_MAX_BODY_COLS, this.MaxBodyCols);
            }

            // 絵文字一覧の文字数
            {
                this.WriteInt(SECTION_NAME, KEY_MAX_EMOJI_LIST_COLS, this.MaxEmojiListCols);
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

            // メール本文に1行の文字数毎に改行を入れる
            {
                this.WriteString(SECTION_NAME, KEY_FORCE_INSERT_LINE_FEED, this.ForceInsertLineFeed ? "True" : "False");
            }

            //// 本文のちらつきを少しだけ抑制する
            //{
            //    this.WriteString(SECTION_NAME, KEY_SUPPRESS_BODY_TEXT_FLICKER, this.SuppressBodyTextFlicker ? "True" : "False");
            //}
        }

        /// <summary>
        /// 設定ファイルから文字列を取得する
        /// </summary>
        /// <param name="lpApplicationName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="lpDefault">デフォルト値</param>
        /// <returns></returns>
        private string GetString(string lpApplicationName, string lpKeyName, string lpDefault = "")
        {
            StringBuilder buffer = new StringBuilder(2560);
            GetPrivateProfileString(lpApplicationName, lpKeyName, lpDefault, buffer, buffer.Capacity, this.filepath);
            return buffer.ToString();
        }

        /// <summary>
        /// 設定ファイルから数値を取得する
        /// </summary>
        /// <param name="lpApplicationName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="nDefault">デフォルト値</param>
        /// <returns></returns>
        private int GetInt(string lpApplicationName, string lpKeyName, int nDefault = 0)
        {
            string str = this.GetString(lpApplicationName, lpKeyName, "");
            int result;

            if (int.TryParse(str, out result)) {
                return result;
            }

            return nDefault;
        }

        /// <summary>
        /// 設定ファイルに文字列を書き込む
        /// </summary>
        /// <param name="lpApplicationName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="lpString">デフォルト値</param>
        private void WriteString(string lpApplicationName, string lpKeyName, string lpString)
        {
            WritePrivateProfileString(lpApplicationName, lpKeyName, lpString, this.filepath);
        }

        /// <summary>
        /// 設定ファイルに数値を文字列として書き込む
        /// </summary>
        /// <param name="lpApplicationName">セクション名</param>
        /// <param name="lpKeyName">キー名</param>
        /// <param name="nInt">デフォルト値</param>
        private void WriteInt(string lpApplicationName, string lpKeyName, int nInt)
        {
            string sInt = string.Format("{0}", nInt);
            this.WriteString(lpApplicationName, lpKeyName, sInt);
        }

        #endregion
    }
}
