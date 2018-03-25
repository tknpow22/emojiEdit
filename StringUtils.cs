using System;
using System.Text;

namespace emojiEdit
{
    //
    // 文字ユーティリティ
    //
    static class StringUtils
    {
        // エンコーディング
        private static Encoding encoding = Encoding.GetEncoding("Shift_JIS");

        // 文字列をできるだけ全角文字列に変換する
        public static string ToZenkaku(String str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.Wide);
        }

        // 文字列をできるだけ半角文字列に変換する
        public static string ToHankaku(String str)
        {
            return Microsoft.VisualBasic.Strings.StrConv(str, Microsoft.VisualBasic.VbStrConv.Narrow);
        }

        // 文字列が半角文字列で構成されているかを返す
        public static bool IsHankaku(String str)
        {
            int encodedLength = encoding.GetByteCount(str);
            return encodedLength == str.Length;
        }

        // 制御文字を削除する(改行は残す)
        public static string RemoveControlChars(String str)
        {
            str = str.Replace("\t", "");
            str = str.Replace("\r\n", "\n");
            str = str.Replace("\r", "\n");

            return str;
        }
    }
}
