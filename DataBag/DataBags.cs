namespace emojiEdit
{
    /// <summary>
    /// データ保持の実装用の interface 的なもの
    /// </summary>
    abstract class CDataBag
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Terminate()
        {
        }
    }

    /// <summary>
    /// データ保持
    /// </summary>
    static class DataBags
    {
        #region 変数

        /// <summary>
        /// 初期化済みフラグ
        /// </summary>
        private static bool initialize = false; // FIXME: やり方としてどうか…

        /// <summary>
        /// 設定
        /// </summary>
        private static ConfigBag config = new ConfigBag();

        /// <summary>
        /// 絵文字
        /// </summary>
        private static EmojiBag emojis = new EmojiBag();

        /// <summary>
        /// メールアドレス
        /// </summary>
        private static MailAddressBag mailAddresses = new MailAddressBag();

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定
        /// </summary>
        public static ConfigBag Config
        {
            get {
                return config;
            }
        }

        /// <summary>
        /// 絵文字
        /// </summary>
        public static EmojiBag Emojis
        {
            get {
                return emojis;
            }
        }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public static MailAddressBag MailAddresses
        {
            get {
                return mailAddresses;
            }
        }

        #endregion

        #region 処理

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static DataBags()
        {
            Initialize(); // FIXME: 念のため
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public static void Initialize()
        {
            if (initialize) {
                return;
            }

            config.Initialize();
            emojis.Initialize();
            mailAddresses.Initialize();

            initialize = true;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public static void Terminate()
        {
            config.Terminate();
            emojis.Terminate();
            mailAddresses.Terminate();
        }

        #endregion
    }
}
