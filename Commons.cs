using System;
using System.Collections.Generic;

namespace emojiEdit
{
    //
    // 共通
    //
    static class Commons
    {
        // メイン画面の幅と高さ
        public const int MinWndWidth = 468;
        public const int MaxWndWidth = 740;
        public const int MinWndHeight = 530;
        public const int MaxWndHeight = 1800;

        // アイコンの幅と高さ
        public const int ICON_WIDTH = 32;
        public const int ICON_HEIGHT = 32;

        // 文字枠の幅と高さ
        public const int FRAME_WIDTH = Commons.ICON_WIDTH + 2;
        public const int FRAME_HEIGHT = Commons.ICON_HEIGHT + 2;

        // フォント
        public const string CONTENTS_FONT_NAME = "ＭＳ ゴシック";
        public const int CONTENTS_FONT_SIZE = 12;

        // 設定ファイル名
        public const string CONFIG_FILE_NAME = "config.ini";

        // メールアドレスを保管するファイル名
        public const string MAIL_ADDRESSES_FILE_NAME = "mailAddresses.txt";

        // 絵文字アイコンイメージファイルを格納したディレクトリ名
        public const string EMOJI_RESOURCE_DIR = "icons";
        // 絵文字アイコンのグループおよび数の定義を格納したファイル名(EMOJI_RESOURCE_DIR の配下に配置する)
        public const string EMOJI_EMOJI_RESOURCE_CONFIG_FILE_NAME = "iconsConfig.txt";

        // 送信したメール内容を保存するディレクトリ
        public const string MAIL_LOG_DIR = "mails";

        // 件名に入力できる文字数
        public const int MAX_SUBJECT_COLS = 256;
        public const int MAX_SUBJECT_ROWS = 1;

        //
        // 処理
        //

        // List<int> を文字数 * 行数に見立てて、特定の行の文字数を返す
        public static int GetUseCols(int maxCols, List<int> contents, int row)
        {
            int useCols = maxCols;
            {
                int col = maxCols - 1;
                while (0 <= col) {
                    int index = maxCols * row + col;
                    int code = contents[index];
                    if (code == 0) {
                        --col;
                    } else {
                        break;
                    }
                }
                useCols = col + 1;
            }
            return useCols;
        }

        // メールアドレスが入力されているかをチェックする
        // NOTE: ちゃんとしたチェックは難しいので、文字列の中に '@' があれば OK としている
        // 複数のアドレスを含む場合、スペースで区切ってあるものとする
        public static bool IsMailAddr(string str, bool multi = false)
        {
            str = str.Trim();

            if (!StringUtils.IsHankaku(str)) {
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
    }
}
