namespace emojiEdit
{
    using System.Drawing;

    /// <summary>
    /// 描画ユーティリティ
    /// </summary>
    static class DrawUtils
    {
        // フォント
        private static Font font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);

        // 文字列フォーマット
        private static StringFormat sf;

        // 静的コンストラクタ
        static DrawUtils()
        {
            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
        }

        // 文字イメージをにピクチャーイメージの指定位置描画する
        public static void DrawImage(Image image, int col, int row, Graphics graphics)
        {
            Rectangle srcRect = new Rectangle(0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            Rectangle desRect = new Rectangle(Commons.FRAME_WIDTH * col + 1, Commons.FRAME_HEIGHT * row + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

            graphics.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}
