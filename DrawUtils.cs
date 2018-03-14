using System.Collections.Generic;
using System.Drawing;

namespace emojiEdit
{
    //
    // 描画ユーティリティ
    //
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

        // アイコン領域に文字を描画する
        public static void DrawIconString(Graphics graphics, string str, int x, int y)
        {
            RectangleF rectIcon = new RectangleF(x + 1, y + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            graphics.DrawString(str, font, Brushes.Black, rectIcon, sf);
        }

        // 文字イメージをにピクチャーイメージの指定位置描画する
        public static void DrawImage(Image image, int col, int row, Graphics graphics)
        {
            Rectangle srcRect = new Rectangle(0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            Rectangle desRect = new Rectangle(Commons.FRAME_WIDTH * col + 1, Commons.FRAME_HEIGHT * row + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

            graphics.DrawImage(image, desRect, srcRect, GraphicsUnit.Pixel);
        }

        // 文字コードデータをピクチャーイメージに描画する
        public static void DrawCodes(int maxCols, int maxRows, List<int> codeList, Graphics graphics)
        {
            int index = 0;

            for (int row = 0; row < maxRows; ++row) {
                for (int col = 0; col < maxCols; ++col) {

                    if (codeList.Count <= index) {
                        return;
                    }

                    int code = codeList[index];
                    ++index;

                    if (code == 0) {
                        continue;
                    }

                    Emoji emoji = DataBags.Emojis.Get(code);
                    if (emoji != null) {
                        DrawUtils.DrawImage(emoji.Image, col, row, graphics);
                    } else {
                        string sch = JisUtils.GetChar(code);
                        if (sch != null) {
                            int x = Commons.FRAME_WIDTH * col;
                            int y = Commons.FRAME_HEIGHT * row;

                            DrawUtils.DrawIconString(graphics, sch, x + 1, y + 1);
                        }
                    }
                }
            }
        }
    }
}
