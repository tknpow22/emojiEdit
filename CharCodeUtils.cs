namespace emojiEdit
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 文字コード関連ユーティリティ
    /// </summary>
    static class CharCodeUtils
    {
        #region 変数

        /// <summary>
        /// JIS エンコーディング
        /// </summary>
        private static Encoding jisEncoding = Encoding.GetEncoding("ISO-2022-JP");

        #endregion

        #region コードの定義

        /// <summary>
        /// JIS 開始シーケンス
        /// </summary>
        public static byte[] JIS_BEGIN_ESCAPE_SEQUENCE = new byte[] { 0x1b, 0x24, 0x42 };

        /// <summary>
        /// JIS 終了シーケンス
        /// </summary>
        public static byte[] JIS_END_ESCAPE_SEQUENCE = new byte[] { 0x1b, 0x28, 0x42 };

        /// <summary>
        /// 改行コード
        /// </summary>
        public static byte[] CRLF = new byte[] { 0x0d, 0x0a };

        /// <summary>
        /// JIS 空白
        /// </summary>
        public static byte[] JIS_WHITE_SPACE = new byte[] { 0x21, 0x21 };

        /// <summary>
        /// JIS 空白コード
        /// </summary>
        public static int JIS_WHITE_SPACE_CODE = 0x2121;

        #endregion

        #region 処理

        /// <summary>
        /// 全角文字の文字コード(JIS)を取得する
        /// </summary>
        /// <param name="sch">文字(1文字)</param>
        /// <returns>文字コード: 取得できない場合は 0</returns>
        public static int GetJisCodeFromChar(string sch)
        {
            int result = 0;
            try {
                byte[] codes = jisEncoding.GetBytes(sch);

                if (8 == codes.Length
                 && codes[0] == JIS_BEGIN_ESCAPE_SEQUENCE[0]
                 && codes[1] == JIS_BEGIN_ESCAPE_SEQUENCE[1]
                 && codes[2] == JIS_BEGIN_ESCAPE_SEQUENCE[2]
                 && codes[5] == JIS_END_ESCAPE_SEQUENCE[0]
                 && codes[6] == JIS_END_ESCAPE_SEQUENCE[1]
                 && codes[7] == JIS_END_ESCAPE_SEQUENCE[2]) {
                    result = codes[3] << 8 | codes[4];
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 全角文字の文字コード(JIS)から文字を取得する
        /// </summary>
        /// <param name="jiscode">JIS コード</param>
        /// <returns>文字(1文字): 取得できない場合は null</returns>
        public static string GetCharFromJisCode(int jiscode)
        {
            (byte high, byte low) = CharCodeUtils.SplitHL(jiscode);

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

            byte[] bistr = new byte[] {
                JIS_BEGIN_ESCAPE_SEQUENCE[0],
                JIS_BEGIN_ESCAPE_SEQUENCE[1],
                JIS_BEGIN_ESCAPE_SEQUENCE[2],
                high, low,
                JIS_END_ESCAPE_SEQUENCE[0],
                JIS_END_ESCAPE_SEQUENCE[1],
                JIS_END_ESCAPE_SEQUENCE[2]
            };

            string result = null;
            try {
                result = jisEncoding.GetString(bistr);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// データが改行かどうかを返す
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="startIndex">開始インデックス</param>
        /// <returns>改行の場合 true</returns>
        public static bool IsCrLf(byte[] data, int startIndex)
        {
            if (startIndex + 2 <= data.Length
             && data[startIndex] == CRLF[0]
             && data[startIndex + 1] == CRLF[1]) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// データが JIS 開始シーケンスかを返す
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="startIndex">開始インデックス</param>
        /// <returns>JIS 開始シーケンスの場合 true</returns>
        public static bool IsBeginEscapeSequence(byte[] data, int startIndex)
        {
            if (startIndex + 3 <= data.Length
             && data[startIndex] == JIS_BEGIN_ESCAPE_SEQUENCE[0]
             && data[startIndex + 1] == JIS_BEGIN_ESCAPE_SEQUENCE[1]
             && data[startIndex + 2] == JIS_BEGIN_ESCAPE_SEQUENCE[2]) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// データが JIS 終了シーケンスかを返す
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="startIndex">開始インデックス</param>
        /// <returns>JIS 終了シーケンスの場合 true</returns>
        public static bool IsEndEscapeSequence(byte[] data, int startIndex)
        {
            if (startIndex + 3 <= data.Length
             && data[startIndex] == JIS_END_ESCAPE_SEQUENCE[0]
             && data[startIndex + 1] == JIS_END_ESCAPE_SEQUENCE[1]
             && data[startIndex + 2] == JIS_END_ESCAPE_SEQUENCE[2]) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// コード値から high, low のバイトを得る
        /// </summary>
        /// <param name="code">コード値</param>
        /// <returns>(high, low)</returns>
        public static (byte, byte) SplitHL(int code)
        {
            byte high = (byte)(code >> 8);
            high &= 0xff;
            byte low = (byte)(code & 0xff);

            return (high, low);
        }

        // high, low バイトからコードを得る
        /// <summary>
        /// high, low バイトからコードを得る
        /// </summary>
        /// <param name="high">high バイト</param>
        /// <param name="low">low バイト</param>
        /// <returns>コード値</returns>
        public static int MergeHL(byte high, byte low)
        {
            int code = high << 8 | low;
            return code;
        }

        /// <summary>
        /// JIS 開始シーケンスを出力する
        /// </summary>
        /// <param name="stream">出力先の Stream</param>
        public static void WriteJisBeginEscapeSequence(Stream stream)
        {
            stream.Write(CharCodeUtils.JIS_BEGIN_ESCAPE_SEQUENCE, 0, CharCodeUtils.JIS_BEGIN_ESCAPE_SEQUENCE.Length);
        }

        /// <summary>
        /// JIS 終了シーケンスを出力する
        /// </summary>
        /// <param name="stream">出力先の Stream</param>
        public static void WriteJisEndEscapeSequence(Stream stream)
        {
            stream.Write(CharCodeUtils.JIS_END_ESCAPE_SEQUENCE, 0, CharCodeUtils.JIS_END_ESCAPE_SEQUENCE.Length);
        }

        /// <summary>
        /// 改行コードを出力する
        /// </summary>
        /// <param name="stream">出力先の Stream</param>
        public static void WriteCrLf(Stream stream)
        {
            stream.Write(CharCodeUtils.CRLF, 0, CharCodeUtils.CRLF.Length);
        }

        /// <summary>
        /// WHITE SPACE + 改行コードを出力する
        /// </summary>
        /// <param name="stream">出力先の Stream</param>
        public static void WriteCrLfWithFlowed(Stream stream)
        {
            stream.WriteByte(0x20);
            stream.Write(CharCodeUtils.CRLF, 0, CharCodeUtils.CRLF.Length);
        }

        /// <summary>
        /// JIS 空白を出力する
        /// </summary>
        /// <param name="stream"></param>
        public static void WriteJisWhiteSpace(Stream stream)
        {
            stream.Write(CharCodeUtils.JIS_WHITE_SPACE, 0, CharCodeUtils.JIS_WHITE_SPACE.Length);
        }

        #endregion
    }
}
