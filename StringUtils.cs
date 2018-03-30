namespace emojiEdit
{
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 文字ユーティリティ
    /// </summary>
    static class StringUtils
    {
        #region 変数

        /// <summary>
        /// SJIS エンコーディング
        /// </summary>
        private static Encoding sjisEncoding = Encoding.GetEncoding("Shift_JIS");

        #endregion

        #region 処理

        /// <summary>
        /// 文字列が ASCII(制御文字も含む) で構成されているかを返す
        /// </summary>
        /// <param name="str">テストする文字列</param>
        /// <returns>すべて ASCII ならば true</returns>
        public static bool IsAsciiWithControl(string str)
        {
            return Regex.IsMatch(str, "^[\x00-\x7F]+$");
        }

        /// <summary>
        /// 文字列が ASCII(制御文字を含まない) で構成されているかを返す
        /// </summary>
        /// <param name="str">テストする文字列</param>
        /// <returns>すべて ASCII ならば true</returns>
        public static bool IsAscii(string str)
        {
            return Regex.IsMatch(str, "^[\x20-\x7E]+$");
        }

        /// <summary>
        /// 文字列中の半角カタカナだけを全角に変換する
        /// </summary>
        /// <param name="str">元の文字列</param>
        /// <returns>処理後の文字列</returns>
        public static string ToZenkakuWithHankakuKanaOnly(string str)
        {
            return Regex.Replace(str, "[\uFF61-\uFF9F]+", match => Microsoft.VisualBasic.Strings.StrConv(match.Value, Microsoft.VisualBasic.VbStrConv.Wide));
        }

        #endregion
    }
}
