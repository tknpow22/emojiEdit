namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 共通
    /// </summary>
    static class Commons
    {
        #region 定義

        #region Windows API

        /// <summary>
        /// RECT 定義
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        #region メイン画面の幅と高さ

        #region メイン画面の幅

        /// <summary>
        /// メイン画面の幅(最小値)
        /// </summary>
        public const int MIN_MAIN_WINDOW_WIDTH = 480;

        /// <summary>
        /// メイン画面の幅(最大値)
        /// </summary>
        public const int MAX_MAIN_WINDOW_WIDTH = 1400;

        #endregion

        #region メイン画面の高さ

        /// <summary>
        /// メイン画面の高さ(最小値)
        /// </summary>
        public const int MIN_MAIN_WINDOW_HEIGHT = 740;

        /// <summary>
        /// メイン画面の高さ(最大値)
        /// </summary>
        public const int MAX_MAIN_WINDOW_HEIGHT = 1400;

        #endregion

        #region 本文の文字数

        // MAX_BODY_COLS_MINIMUM <= MAX_BODY_COLS_DEFALUT <= MAX_BODY_COLS_MAXIMUM となること

        /// <summary>
        /// 本文の文字数(最小)
        /// </summary>
        public const int MAX_BODY_COLS_MINIMUM = 9;

        /// <summary>
        /// 本文の文字数(デフォルト)
        /// </summary>
        public const int MAX_BODY_COLS_DEFALUT = 12;

        /// <summary>
        /// 本文の文字数(最大)
        /// </summary>
        public const int MAX_BODY_COLS_MAXIMUM = 20;

        #endregion


        #region 絵文字一覧の文字数

        // MAX_EMOJI_LIST_COLS_MINIMUM <= MAX_EMOJI_LIST_COLS_DEFAULT <= MAX_EMOJI_LIST_COLS_MAXIMUM となること

        /// <summary>
        /// 絵文字一覧の文字数(最小)
        /// </summary>
        public const int MAX_EMOJI_LIST_COLS_MINIMUM = 6;

        /// <summary>
        /// 絵文字一覧の文字数(デフォルト)
        /// </summary>
        public const int MAX_EMOJI_LIST_COLS_DEFAULT = 9;

        /// <summary>
        /// 絵文字一覧の文字数(最大)
        /// </summary>
        public const int MAX_EMOJI_LIST_COLS_MAXIMUM = 20;

        #endregion

        #endregion

        #region テキストボックスへの描画用のアイコンの幅と高さ

        /// <summary>
        /// テキストボックスへの描画用のアイコンの幅
        /// </summary>
        public const int TEXT_ICON_WIDTH = 32;

        /// <summary>
        /// テキストボックスへの描画用のアイコンの高さ
        /// </summary>
        public const int TEXT_ICON_HEIGHT = 32;

        #endregion

        #region アイコンの幅と高さ

        /// <summary>
        /// アイコンの幅
        /// </summary>
        public const int ICON_WIDTH = 32;

        /// <summary>
        /// アイコンの高さ
        /// </summary>
        public const int ICON_HEIGHT = 32;

        #endregion

        #region 文字枠の幅と高さ

        /// <summary>
        /// 文字枠の幅
        /// </summary>
        public const int FRAME_WIDTH = Commons.ICON_WIDTH + 2;

        /// <summary>
        /// 文字枠の高さ
        /// </summary>
        public const int FRAME_HEIGHT = Commons.ICON_HEIGHT + 2;

        #endregion

        #region フォント

        /// <summary>
        /// フォントファミリー
        /// </summary>
        public const string CONTENTS_FONT_NAME = "MS Gothic";

        /// <summary>
        /// フォントサイズ
        /// </summary>
        public const int CONTENTS_FONT_SIZE = 24;

        #endregion

        /// <summary>
        /// デフォルトの SMTP ポート番号
        /// </summary>
        public const int SMTP_PORT_DEFAULT = 587;

        /// <summary>
        /// 設定ファイル名
        /// </summary>
        public const string CONFIG_FILE_NAME = "config.ini";

        /// <summary>
        /// メールアドレスを保管するファイル名
        /// </summary>
        public const string MAIL_ADDRESSES_FILE_NAME = "mailAddresses.txt";

        #region 絵文字アイコン設定

        /// <summary>
        /// 絵文字アイコンイメージファイルを格納したディレクトリ名
        /// </summary>
        public const string EMOJI_RESOURCE_DIR = "icons";

        /// <summary>
        /// 絵文字アイコンのグループおよび数の定義を格納したファイル名(EMOJI_RESOURCE_DIR の配下に配置する)
        /// </summary>
        public const string EMOJI_EMOJI_RESOURCE_CONFIG_FILE_NAME = "iconsConfig.txt";

        #endregion

        /// <summary>
        /// 送信したメール内容を保存するディレクトリ
        /// </summary>
        public const string MAIL_LOG_DIR = "mails";

        #endregion

        #region テンプレート

        /// <summary>
        /// テンプレートを保管するファイル名
        /// </summary>
        public const string TEMPLATE_FILE_NAME = "templates.txt";

        /// <summary>
        /// テンプレートのサムネイル画像の幅(文字数)
        /// </summary>
        public const int TEMPLATE_THUMBNAIL_COLS = 12;

        /// <summary>
        /// テンプレートのサムネイル画像の高さ(文字数)
        /// </summary>
        public const int TEMPLATE_THUMBNAIL_ROWS = 3;

        #endregion

        #region 処理

        /// <summary>
        /// メールアドレスが入力されているかをチェックする
        /// NOTE: ちゃんとしたチェックは難しいので、文字列の中に '@' があれば OK としている
        /// 複数のアドレスを含む場合、スペースで区切ってあるものとする
        /// </summary>
        /// <param name="str">メールアドレスとおぼしき文字列</param>
        /// <param name="multi">複数アドレスかの場合は true</param>
        /// <returns>OK なら true</returns>
        public static bool IsMailAddr(string str, bool multi = false)
        {
            str = str.Trim();

            if (!StringUtils.IsAscii(str)) {
                return false;
            }

            if (!multi) {
                if (0 <= str.IndexOf(' ')) {
                    return false;
                }
            }

            string[] strArr = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (!multi && strArr.Length != 1) {
                return false;
            }

            foreach (string addr in strArr) {
                if (addr.StartsWith("@") || addr.EndsWith("@")) {
                    return false;
                }
                if (addr.IndexOf("@") <= 0) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// カンマ区切りの16進数コードの羅列(XXXX,XXXX,...)からコード一覧を得る
        /// </summary>
        /// <param name="csvHexaCodes">文字列</param>
        /// <returns>コード一覧</returns>
        public static List<int> GetCodesFromHexaStrings(string csvHexaCodes)
        {
            List<int> codeList = new List<int>();

            do {

                string[] items = csvHexaCodes.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 0) {
                    break;
                }

                foreach (string item in items) {
                    int code;

                    if (item.Length != 4) {
                        continue;
                    }

                    if (!int.TryParse(item, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out code)) {
                        continue;
                    }

                    codeList.Add(code);
                }

            } while (false);

            return codeList;
        }

        /// <summary>
        /// コード一覧からカンマ区切りの16進数コードの羅列(XXXX,XXXX,...)を得る
        /// </summary>
        /// <param name="codeList">コード一覧</param>
        /// <returns>文字列</returns>
        public static string GetHexaStringsFromCodes(List<int> codeList)
        {
            var hexaList = codeList.Select(code => string.Format("{0:X4}", code));
            return string.Join(",", hexaList);
        }

        #endregion
    }
}
