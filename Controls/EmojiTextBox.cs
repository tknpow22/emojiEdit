namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// 絵文字対応の TextBox
    /// </summary>
    partial class EmojiTextBox : TextBox
    {
        #region 定義

        /// <summary>
        /// 表示タイプ(表示領域等の計算で使用する)
        /// </summary>
        private enum DrawType
        {
            None,
            Emoji,
            ControlChar,
            NormalChar
        }

        /// <summary>
        /// 表示情報(表示領域等の計算で使用する)
        /// </summary>
        private class DrawDimension
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="sch"></param>
            public DrawDimension(string sch)
            {
                this.OriginalSChar = sch;
                this.OriginalChar = sch[0];

                Emoji emoji = DataBags.Emojis.GetFromUnicode(this.OriginalChar);
                if (emoji != null) {
                    this.Type = DrawType.Emoji;
                    this.Emoji = emoji;
                } else if (this.OriginalChar == '\t') {
                    this.ControlChar = "\u02EA";
                    this.Type = DrawType.ControlChar;
                } else if (this.OriginalChar == '\n') {
                    this.ControlChar = "\u21B2";
                    this.Type = DrawType.ControlChar;
                } else if (this.OriginalChar == '\r') {
                    this.Type = DrawType.ControlChar;
                    this.ControlChar = "\r";
                } else {
                    this.Type = DrawType.NormalChar;
                }
            }

            /// <summary>
            /// 元の文字(string)
            /// </summary>
            public string OriginalSChar
            {
                get; private set;
            }

            /// <summary>
            /// 元の文字(char)
            /// </summary>
            public char OriginalChar
            {
                get; private set;
            }

            /// <summary>
            /// 表示タイプ
            /// </summary>
            public DrawType Type
            {
                get; private set;
            }

            /// <summary>
            /// 表示エリア
            /// </summary>
            public Rectangle Area
            {
                get; set;
            }

            /// <summary>
            /// 表示エリア(パディング用)
            /// </summary>
            public Rectangle PaddingArea
            {
                get; set;
            }

            /// <summary>
            /// 選択されているか
            /// </summary>
            public bool Selection
            {
                get; set;
            }

            /// <summary>
            /// 絵文字
            /// </summary>
            public Emoji Emoji
            {
                get; private set;
            }

            /// <summary>
            /// 制御文字
            /// </summary>
            public string ControlChar
            {
                get; private set;
            }
        }

        #endregion

        #region Windows API

        #region 定数

        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_SETREDRAW = 0x000B;
        private const int WM_PAINT = 0x000F;
        private const int WM_SETFONT = 0x0030;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEWHEEL = 0x020A;

        private const int EM_SETSEL = 0x00B1;
        private const int EM_GETRECT = 0x00B2;
        private const int EM_SETTABSTOPS = 0x00CB;
        private const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        private const int EM_SETWORDBREAKPROC = 0x00D0;

        private const int COLOR_HIGHLIGHT = 13;
        private const int COLOR_HIGHLIGHTTEXT = 14;

        #endregion

        #region 定義

        /// <summary>
        /// EditWordBreakProc を呼び出すためのデリゲート定義
        /// </summary>
        /// <param name="lpch"></param>
        /// <param name="ichCurrent"></param>
        /// <param name="cch"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private delegate int EditWordBreakProcDelegate(IntPtr lpch, int ichCurrent, int cch, int code);

        #endregion

        #region API

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, EditWordBreakProcDelegate lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, out Commons.RECT lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetSysColor(int nIndex);

        #endregion

        #endregion

        #region 静的変数

        /// <summary>
        /// 制御文字の表示色
        /// </summary>
        private static Color controlCharColor = Color.LightBlue;

        /// <summary>
        /// カラム線描画用の Pen
        /// </summary>
        private static Pen columnLinePen = new Pen(Color.LightBlue);

        #endregion

        #region 変数

        /// <summary>
        /// TextRenderer.DrawText() のための format フラグ
        /// </summary>
        private TextFormatFlags textFormatFlags = TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;

        /// <summary>
        /// 文字列選択時にイメージを反転させるための属性
        /// </summary>
        private ImageAttributes negativeImageAttributes;

        /// <summary>
        /// EditWordBreakProc を呼び出すためのデリゲート
        /// </summary>
        private EditWordBreakProcDelegate editWordBreakProcDelegate;

        /// <summary>
        /// 背景色(ブラシ)
        /// </summary>
        private Brush backColorBrush;

        /// <summary>
        /// 選択された文字の前景色
        /// </summary>
        private Color selectionCharForeColor;

        /// <summary>
        /// 選択された文字の背景色
        /// </summary>
        private Color selectionCharBackColor;

        /// <summary>
        /// 選択された文字の背景色(ブラシ)
        /// </summary>
        private Brush selectionCharBackBrush;

        /// <summary>
        /// 描画に使用するフォント
        /// </summary>
        private Font displayFont;

        /// <summary>
        /// マウスの左ボタンを押している間 true にするフラグ
        /// </summary>
        private bool mouseLButtonDown = false;

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmojiTextBox()
        {
            this.InitializeComponent();

            this.SetStyle(
                ControlStyles.DoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint, true);

            //
            // イベントハンドラを設定する
            //

            this.MouseDown += TextBox_MouseDown;

            // コンテキストメニューを設定する
            {
                ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("切り取り(&T)", null, this.ToolStripMenuItem_Click, "Cut");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("コピー(&C)", null, this.ToolStripMenuItem_Click, "Copy");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("貼り付け(&P)", null, this.ToolStripMenuItem_Click, "Paste");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }
                {
                    ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("削除(&D)", null, this.ToolStripMenuItem_Click, "Delete");
                    contextMenuStrip.Items.Add(toolStripMenuItem);
                }

                contextMenuStrip.Opening += ContextMenuStrip_Opening;

                this.ContextMenuStrip = contextMenuStrip;
            }

            this.TextChanged += this.TextBox_TextChanged;

            //
            // 文字列選択時にイメージを反転させるための属性を作成する
            //
            {
                ColorMatrix cm = new ColorMatrix();
                cm.Matrix00 = -1;
                cm.Matrix11 = -1;
                cm.Matrix22 = -1;
                cm.Matrix33 = 1;
                cm.Matrix40 = 1;
                cm.Matrix41 = 1;
                cm.Matrix42 = 1;
                cm.Matrix44 = 1;

                this.negativeImageAttributes = new ImageAttributes();
                negativeImageAttributes.SetColorMatrix(cm);
            }

            // EditWordBreakProc を呼び出すためのデリゲート
            this.editWordBreakProcDelegate = new EditWordBreakProcDelegate(this.EditWordBreakProc);

            // 背景色(ブラシ)
            this.backColorBrush = new SolidBrush(this.BackColor);

            // 選択された文字の前景色
            this.selectionCharForeColor = GetSystemColor(COLOR_HIGHLIGHTTEXT);

            // 選択された文字の背景色
            this.selectionCharBackColor = GetSystemColor(COLOR_HIGHLIGHT);

            // 選択された文字の背景色(ブラシ)
            this.selectionCharBackBrush = new SolidBrush(this.selectionCharBackColor);

            // 描画に使用するフォント
            this.displayFont = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// カラム線の表示位置
        /// </summary>
        [Browsable(true)]
        public int ColumnLine
        {
            get; set;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary>
        /// MouseDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            // マウスの右ボタンを押し、そのままドラッグして、TextBox の外側でボタンを離した際に、
            // 標準のコンテキストメニューが表示されないようにする。
            if (e.Button == MouseButtons.Right) {
                ((Control)sender).Capture = false;
            }
        }

        /// <summary>
        /// ContextMenuStrip - Opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            bool hasSelection = this.SelectionLength != 0;

            this.ContextMenuStrip.Items["Cut"].Enabled = hasSelection;
            this.ContextMenuStrip.Items["Copy"].Enabled = hasSelection;
            this.ContextMenuStrip.Items["Delete"].Enabled = hasSelection;

            this.ContextMenuStrip.Items["Paste"].Enabled = Clipboard.ContainsText();
        }

        /// <summary>
        /// ToolStripMenuItem - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;

            switch (toolStripMenuItem.Name) {
            case "Cut":
                this.Cut();
                this.Focus();
                break;
            case "Copy":
                this.Copy();
                this.Focus();
                break;
            case "Paste":
                this.Paste();
                this.Focus();
                break;
            case "Delete":
                this.SelectedText = "";
                this.Focus();
                break;
            }
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// OnHandleCreated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            SendMessage(this.Handle, WM_SETFONT, this.displayFont.ToHfont(), 1);
            SendMessage(this.Handle, EM_SETTABSTOPS, 1, new int[] { 32 });  // TABSTOPS: 8 カラムに相当する
            if (!this.DesignMode && this.Multiline) {
                SendMessage(this.Handle, EM_SETWORDBREAKPROC, 0, this.editWordBreakProcDelegate);
            }
        }

        /// <summary>
        /// OnPaint
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            this.Draw(e.Graphics, true, false);
        }

        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            bool invalidate = false;
            switch (m.Msg) {
            case EM_SETSEL:
                invalidate = true;
                break;
            case WM_KEYUP:
            case WM_KEYDOWN:
                invalidate = true;
                break;
            case WM_LBUTTONDOWN:
                this.mouseLButtonDown = true;
                invalidate = true;
                break;
            case WM_MOUSEMOVE:
                if (this.mouseLButtonDown) {
                    invalidate = true;
                }
                break;
            case WM_LBUTTONUP:
                this.mouseLButtonDown = false;
                invalidate = true;
                break;
            }

            if (invalidate) {
                this.Invalidate();
            }
        }

        /// <summary>
        /// 絵文字等の描画を行う(固定長のみ対応)
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="drawNormalChar">通常の文字も描画する場合は true</param>
        /// <param name="emojiOnly">絵文字のみ描画</param>
        private void Draw(Graphics graphics, bool drawNormalChar, bool emojiOnly)
        {
            Commons.RECT textBoxRect;
            SendMessage(this.Handle, EM_GETRECT, 0, out textBoxRect);

            Size baseFontSize = TextRenderer.MeasureText(graphics, "あ", this.displayFont, new Size(), this.textFormatFlags);

            List<DrawDimension> drawDimensionList = this.CalcDrawDimensions(graphics, textBoxRect, baseFontSize);

            foreach (DrawDimension drawDimension in drawDimensionList) {

                if (drawDimension.Type == DrawType.Emoji) {
                    Image image = drawDimension.Emoji.ImageForText;
                    Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                    Rectangle destRect = drawDimension.Area;

                    if (drawDimension.Selection) {
                        // 選択されている場合は、反転して描画する
                        TextRenderer.DrawText(graphics, "\u3000", this.displayFont, new Point(drawDimension.Area.X, drawDimension.Area.Y), this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                        graphics.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, this.negativeImageAttributes);
                    } else {
                        graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                    }

                } else if (drawDimension.Type == DrawType.ControlChar) {

                    if (!emojiOnly) {
                        Point drawPoint = new Point(drawDimension.Area.X, drawDimension.Area.Y);

                        if (drawDimension.Selection) {
                            // 選択されている場合は、反転の背景を先に描画する
                            TextRenderer.DrawText(graphics, "\x20", this.displayFont, drawPoint, this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                        }
                        TextRenderer.DrawText(graphics, drawDimension.ControlChar, this.displayFont, drawPoint, controlCharColor, this.textFormatFlags);
                    }

                } else if (drawNormalChar && drawDimension.Type == DrawType.NormalChar) {

                    Point drawPoint = new Point(drawDimension.Area.X, drawDimension.Area.Y);

                    if (drawDimension.Selection) {
                        TextRenderer.DrawText(graphics, drawDimension.OriginalSChar, this.displayFont, drawPoint, this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                    } else {
                        TextRenderer.DrawText(graphics, drawDimension.OriginalSChar, this.displayFont, drawPoint, this.ForeColor, this.BackColor, this.textFormatFlags);
                    }
                }

                if (drawDimension.Selection && !drawDimension.PaddingArea.IsEmpty) {
                    graphics.FillRectangle(this.selectionCharBackBrush, drawDimension.PaddingArea);
                }
            }

            if (!emojiOnly && 0 < this.ColumnLine) {
                int x = textBoxRect.Left + (baseFontSize.Width * this.ColumnLine);
                graphics.DrawLine(columnLinePen, new Point(x, 0), new Point(x, this.ClientSize.Height));
            }
        }

        /// <summary>
        /// 表示領域等を計算する(固定長のみ対応)
        /// FIXME: 文字末尾にタブがある場合の位置計算がおかしい
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="textBoxRect">テキストエリアの領域</param>
        /// <param name="baseFontSize">ベースとなるフォントサイズ</param>
        /// <returns>計算した領域情報</returns>
        private List<DrawDimension> CalcDrawDimensions(Graphics graphics, Commons.RECT textBoxRect, Size baseFontSize)
        {
            Size baseFontSizeHalf = TextRenderer.MeasureText(graphics, "A", this.displayFont, new Size(), this.textFormatFlags);

            string textBoxText = this.Text;
            int selectionStart = this.SelectionStart;
            int selectionLength = this.SelectionLength;

            // 描画対象の文字列インデックス範囲を得る
            int firstCharIndex = 0;
            int lastCharIndex = textBoxText.Length;
            if (this.Multiline) {
                // 最初の位置を得る
                int firstVisibleLine = SendMessage(this.Handle, EM_GETFIRSTVISIBLELINE, 0, 0);
                if (0 <= firstVisibleLine) {
                    int firstIndex = this.GetFirstCharIndexFromLine(firstVisibleLine);
                    if (0 <= firstIndex) {
                        firstCharIndex = firstIndex;
                    }
                }

                // 表示行数を得る
                int rows = this.ClientSize.Height / baseFontSize.Height;

                // 最後の位置を得る
                int lastIndex = this.GetFirstCharIndexFromLine(firstVisibleLine + rows);
                if (0 <= lastIndex) {
                    lastCharIndex = lastIndex;
                }
            }

            List<DrawDimension> drawDimensionList = new List<DrawDimension>();

            int columns = 0;
            int prevLine = this.GetLineFromCharIndex(firstCharIndex);
            for (int chIndex = firstCharIndex; chIndex < lastCharIndex; ++chIndex) {
                if (chIndex < 0 || textBoxText.Length <= chIndex) {
                    break;
                }

                int currentLine = this.GetLineFromCharIndex(chIndex);

                string targetSChar = textBoxText.Substring(chIndex, 1);

                DrawDimension drawDimension = new DrawDimension(targetSChar);
                Point point = this.GetFixedPositionFromCharIndex(textBoxRect, chIndex);

                if (drawDimension.Type == DrawType.Emoji) {
                    drawDimension.Area = new Rectangle(point.X, point.Y, Commons.TEXT_ICON_WIDTH, Commons.TEXT_ICON_HEIGHT);
                    columns += 2;
                } else if (drawDimension.Type == DrawType.ControlChar) {
                    drawDimension.Area = new Rectangle(point.X, point.Y, baseFontSizeHalf.Width, baseFontSizeHalf.Height);

                    if (drawDimension.OriginalChar == '\t') {
                        int tabColumns = 8 - (columns % 8);

                        if (chIndex + 1 < textBoxText.Length) {
                            Point pointNext = this.GetFixedPositionFromCharIndex(textBoxRect, chIndex + 1);
                            drawDimension.PaddingArea = new Rectangle(point.X + baseFontSizeHalf.Width, point.Y, pointNext.X - point.X - baseFontSizeHalf.Width, baseFontSizeHalf.Height);
                        } else {
                            int width = baseFontSizeHalf.Width * tabColumns - baseFontSizeHalf.Width;
                            drawDimension.PaddingArea = new Rectangle(point.X + baseFontSizeHalf.Width, point.Y, width, baseFontSizeHalf.Height);
                        }

                        columns += tabColumns;
                    } else if (drawDimension.OriginalChar == '\n') {
                        columns = 0;
                    } else if (drawDimension.OriginalChar == '\r') {
                        // 表示されないので columns には何もしない
                    }

                } else if (drawDimension.Type == DrawType.NormalChar) {
                    // NOTE: とりあえず固定長で考える
                    if (StringUtils.IsHalfSizeDisplay(targetSChar)) {
                        drawDimension.Area = new Rectangle(point.X, point.Y, baseFontSizeHalf.Width, baseFontSizeHalf.Height);
                        columns += 1;
                    } else {
                        drawDimension.Area = new Rectangle(point.X, point.Y, baseFontSize.Width, baseFontSize.Height);
                        columns += 2;
                    }
                }
                if (selectionStart <= chIndex && chIndex < selectionStart + selectionLength) {
                    drawDimension.Selection = true;

                    // 前行の文字末尾がタブの場合の処理
                    if (prevLine != currentLine && selectionStart <= chIndex - 1 && 0 < drawDimensionList.Count) {
                        DrawDimension prevDrawDimension = drawDimensionList[drawDimensionList.Count - 1];
                        if (prevDrawDimension.OriginalChar == '\t') {
                            Rectangle area = prevDrawDimension.Area;
                            prevDrawDimension.PaddingArea = new Rectangle(area.X + area.Width, area.Y, textBoxRect.Right - (area.X + area.Width), area.Height);
                        }
                    }
                }

                prevLine = currentLine;

                drawDimensionList.Add(drawDimension);
            }

            return drawDimensionList;
        }

        /// <summary>
        /// 文字列のインデックスに対応する表示座標を取得する(調整済み)
        /// FIXME: 無理矢理なので .NET Framework の実装によっては破綻する可能性あり
        /// </summary>
        /// <param name="textBoxRect">EM_GETRECT で取得した RECT 構造体</param>
        /// <param name="index">インデックス</param>
        /// <returns>調整済みの表示座標</returns>
        public Point GetFixedPositionFromCharIndex(Commons.RECT textBoxRect, int index)
        {
            Point pointOrig = this.GetPositionFromCharIndex(index);
            Point result = new Point(pointOrig.X, pointOrig.Y);

            if (!this.Multiline) {
                result.Y += textBoxRect.Top;
            }

            return result;
        }

        /// <summary>
        /// 改行制御処理
        /// </summary>
        /// <param name="lpch"></param>
        /// <param name="ichCurrent"></param>
        /// <param name="cch"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private int EditWordBreakProc(IntPtr lpch, int ichCurrent, int cch, int code)
        {
            return 0;
        }

        /// <summary>
        /// システムカラーを取得する
        /// </summary>
        /// <param name="systemColorId">システムカラーの ID</param>
        /// <returns>Color</returns>
        private Color GetSystemColor(int systemColorId)
        {
            int sysColor = GetSysColor(systemColorId);

            int red = sysColor & 0xff;
            int green = (sysColor >> 8) & 0xff;
            int blue = (sysColor >> 16) & 0xff;

            return Color.FromArgb(red, green, blue);
        }

#endregion
    }
}
