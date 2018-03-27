namespace emojiEdit
{
    using System.Drawing;

    /// <summary>
    /// 描画ユーティリティ
    /// </summary>
    static class DrawUtils
    {
        /// <summary>
        /// 文字列フォーマット
        /// </summary>
        private static StringFormat sf;

        /// <summary>
        /// 静的コンストラクタ
        /// </summary>
        static DrawUtils()
        {
            sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
        }

        /// <summary>
        /// 文字イメージをピクチャーイメージの指定位置に描画する
        /// </summary>
        /// <param name="image">文字イメージ(絵文字アイコン等)</param>
        /// <param name="col">文字位置</param>
        /// <param name="row">行位置</param>
        /// <param name="graphics">ピクチャーイメージの Graphics</param>
        public static void DrawImage(Image image, int col, int row, Graphics graphics)
        {
            Rectangle srcRect = new Rectangle(0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            Rectangle desRect = new Rectangle(Commons.FRAME_WIDTH * col + 1, Commons.FRAME_HEIGHT * row + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

            graphics.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
        }
    }
}
