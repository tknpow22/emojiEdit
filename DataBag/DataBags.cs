﻿
namespace emojiEdit
{
    //
    // データ保持
    //
    static class DataBags
    {
        // 設定
        private static ConfigBag config = new ConfigBag();
        public static ConfigBag Config
        {
            get {
                return config;
            }
        }

        // 絵文字
        private static EmojiBag emojis = new EmojiBag();
        public static EmojiBag Emojis
        {
            get {
                return emojis;
            }
        }

        // メールアドレス
        private static MailAddressBag mailAddresses = new MailAddressBag();
        public static MailAddressBag MailAddresses
        {
            get {
                return mailAddresses;
            }
        }

        // テンプレート
        private static TemplateBags templates = new TemplateBags();
        public static TemplateBags Templates
        {
            get {
                return templates;
            }
        }

        // 初期化済みフラグ
        private static bool initialize = false; // FIXME: やり方としてどうか…

        // 静的コンストラクタ
        static DataBags()
        {
            Init(); // FIXME: 念のため
        }

        // 初期化
        public static void Init()
        {
            if (initialize) {
                return;
            }

            config.Init();
            emojis.Init();
            mailAddresses.Init();
            templates.Init();

            initialize = true;
        }

        // 保存する
        public static void Save()
        {
            config.Save();
            mailAddresses.Save();
            templates.Save();
        }
    }
}
