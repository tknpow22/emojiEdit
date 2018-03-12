using System.Drawing;

namespace emojiEdit
{
    //
    // 文字描画ユーティリティ
    //
    static class DrawStringUtils
    {
        // フォント
        private static Font font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);

        // 文字列フォーマット
        private static StringFormat sf;

        // 静的コンストラクタ
        static DrawStringUtils()
        {
            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
        }

        // アイコン領域に文字を描画する
        public static void DrawIconString(Graphics graphics, string str, int x, int y)
        {
            RectangleF rectIcon = new RectangleF(x + 1, y + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            graphics.DrawString(str, font, Brushes.Black, rectIcon, sf);
        }
    }
}
