using System;
using System.IO;
using System.Text;

namespace emojiEdit
{
    //
    // JIS 関連ユーティリティ
    //
    static class JisUtils
    {
        // エンコーディング
        private static Encoding encoding = Encoding.GetEncoding("ISO-2022-JP");

        // コードの定義
        public static byte[] BEGIN_ESCAPE_SEQUENCE = new byte[] { 0x1b, 0x24, 0x42 };
        public static byte[] END_ESCAPE_SEQUENCE = new byte[] { 0x1b, 0x28, 0x42 };
        public static byte[] CRLF = new byte[] { 0x0d, 0x0a };
        public static byte[] WHITE_SPACE = new byte[] { 0x21, 0x21 };

        // 全角文字の文字コード(JIS)を取得する
        public static int GetCode(string sch)
        {
            int result = 0;
            try {
                byte[] codes = encoding.GetBytes(sch);

                if (8 == codes.Length
                 && codes[0] == 0x1b && codes[1] == 0x24 && codes[2] == 0x42
                 && codes[5] == 0x1b && codes[6] == 0x28 && codes[7] == 0x42) {
                    result = codes[3] << 8 | codes[4];
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        // 全角文字の文字コード(JIS)から文字を取得する
        public static string GetChar(int code)
        {
            (byte high, byte low) = JisUtils.SplitHL(code);

            if (0x21 <= high && high <= 0x7e) {
                // OK
            } else {
                return null;
            }

            if (0x21 <= low && low <= 0x7e) {
                // OK
            } else {
                return null;
            }

            byte[] bistr = new byte[] { 0x1b, 0x24, 0x42, high, low, 0x1b, 0x28, 0x42 };

            string result = "　";    // 全角スペース
            try {
                result = encoding.GetString(bistr);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        // 改行か
        public static bool IsCrLf(byte[] data, int dataLength, int index)
        {
            if (index <= dataLength - 2 && data[index] == 0x0d && data[index + 1] == 0x0a) {
                return true;
            }

            return false;
        }

        // 日本語文字列の開始シーケンスか
        public static bool IsBeginEscapeSequence(byte[] data, int dataLength, int index)
        {
            if (index <= dataLength - 3 && data[index] == 0x1b && data[index + 1] == 0x24 && data[index + 2] == 0x42) {
                return true;
            }

            return false;
        }

        // 日本語文字列の終了シーケンスか
        public static bool IsEndEscapeSequence(byte[] data, int dataLength, int index)
        {
            if (index <= dataLength - 3 && data[index] == 0x1b && data[index + 1] == 0x28 && data[index + 2] == 0x42) {
                return true;
            }

            return false;
        }

        // コードから high, low のバイトを得る
        public static (byte, byte) SplitHL(int code)
        {
            byte high = (byte)(code >> 8);
            high &= 0xff;
            byte low = (byte)(code & 0xff);

            return (high, low);
        }

        // high, low バイトからコードを得る
        public static int MergeHL(byte high, byte low)
        {
            int code = high << 8 | low;
            return code;
        }

        // 開始エスケープシーケンスを出力する
        public static void WriteBeginEscapeSequence(Stream stream)
        {
            stream.Write(JisUtils.BEGIN_ESCAPE_SEQUENCE, 0, JisUtils.BEGIN_ESCAPE_SEQUENCE.Length);
        }

        // 終了エスケープシーケンスを出力する
        public static void WriteEndEscapeSequence(Stream stream)
        {
            stream.Write(JisUtils.END_ESCAPE_SEQUENCE, 0, JisUtils.END_ESCAPE_SEQUENCE.Length);
        }

        // 改行を出力する
        public static void WriteCrLf(Stream stream)
        {
            stream.Write(JisUtils.CRLF, 0, JisUtils.CRLF.Length);
        }

        // 全角スペースを出力する
        public static void WriteWhiteSpace(Stream stream)
        {
            stream.Write(JisUtils.WHITE_SPACE, 0, JisUtils.WHITE_SPACE.Length);
        }
    }
}
