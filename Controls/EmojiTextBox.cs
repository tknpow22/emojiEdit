namespace emojiEdit
{
    using System;
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
        // NOTE: @SELTABSTOP: 選択時、タブストップを反転表示する => 実装しない。
        //         - タブは原則入力しない(コピペは可能)
        //         - 実装方法がイマイチなので処理時間がかかりそう(描画がちらつくのはイヤ)
        //         - 実装方法がイマイチなため、正しく描画されない可能性がある
        //           ちなみにタブの次のカラム位置の計算はテキストボックスの一番最後の文字がタブコードだったときのためのみ行っている
        //           (一番最後以外は次の文字の位置で計算できるため)
        //       いずれ、良い方法が見つかれば実装するかも。。。

        #region Windows API

        #region 定数

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

        private const int EM_GETRECT = 0x00B2;
        //@SELTABSTOP:private const int EM_SETTABSTOPS = 0x00CB;
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
        //@SELTABSTOP:[DllImport("user32.dll")]
        //@SELTABSTOP:private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);
        [DllImport("user32.dll")]
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
        /// 選択された文字の前景色
        /// </summary>
        private Color selectionCharForeColor;

        /// <summary>
        /// 選択された文字の背景色
        /// </summary>
        private Color selectionCharBackColor;

        //@SELTABSTOP:/// <summary>
        //@SELTABSTOP:/// 選択された文字の背景色(ブラシ)
        //@SELTABSTOP:/// </summary>
        //@SELTABSTOP:private Brush selectionCharBackBrush;

        /// <summary>
        /// 描画に使用するフォント
        /// </summary>
        private Font font;

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

            // 選択された文字の前景色
            this.selectionCharForeColor = GetSystemColor(COLOR_HIGHLIGHTTEXT);

            // 選択された文字の背景色
            this.selectionCharBackColor = GetSystemColor(COLOR_HIGHLIGHT);

            //@SELTABSTOP:// 選択された文字の背景色(ブラシ)
            //@SELTABSTOP:this.selectionCharBackBrush = new SolidBrush(this.selectionCharBackColor);

            // 描画に使用するフォント
            this.font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);
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
            SendMessage(this.Handle, WM_SETFONT, this.font.ToHfont(), 1);
            //@SELTABSTOP:SendMessage(this.Handle, EM_SETTABSTOPS, 1, new int[] { 32 });  // TABSTOPS: 8 カラムに相当する
            if (!this.DesignMode && this.Multiline) {
                SendMessage(this.Handle, EM_SETWORDBREAKPROC, 0, this.editWordBreakProcDelegate);
            }
        }

        /// <summary>
        /// DrawToBitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="targetBounds"></param>
        public new void DrawToBitmap(Bitmap bitmap, Rectangle targetBounds)
        {
            base.DrawToBitmap(bitmap, targetBounds);
            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                this.Draw(graphics, false, true);
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
        /// 絵文字等の描画を行う
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="drawNormalChar">通常の文字も描画する場合は true</param>
        /// <param name="emojiOnly">絵文字のみ描画</param>
        private void Draw(Graphics graphics, bool drawNormalChar, bool emojiOnly)
        {
            Commons.RECT textBoxRect;
            SendMessage(this.Handle, EM_GETRECT, 0, out textBoxRect);

            Size fontSize = TextRenderer.MeasureText(graphics, "あ", this.Font, new Size(), this.textFormatFlags);
            //@SELTABSTOP:Size fontSizeHalf = TextRenderer.MeasureText(graphics, "A", this.Font, new Size(), this.textFormatFlags);

            // 描画対象の文字列インデックス範囲を得る
            int firstCharIndex = 0;
            int lastCharIndex = this.TextLength;
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
                int rows = this.ClientSize.Height / fontSize.Height;

                // 最後の位置を得る
                int lastIndex = this.GetFirstCharIndexFromLine(firstVisibleLine + rows);
                if (0 <= lastIndex) {
                    lastCharIndex = lastIndex;
                }
            }

            //@SELTABSTOP:int virColumn = 0;  // 仮想のカラム位置

            // 範囲内文字列の絵文字等の表示を行う
            string targetText = this.Text;
            for (int chIndex = firstCharIndex; chIndex < lastCharIndex; ++chIndex) {
                if (chIndex < 0 || targetText.Length <= chIndex) {
                    break;
                }

                string targetSChar = targetText.Substring(chIndex, 1);
                char targetChar = targetSChar[0];

                bool drawEmoji = false;
                bool drawControlChar = false;
                string controlChar = "";

                Emoji emoji = DataBags.Emojis.GetFromUnicode(targetChar);
                if (emoji != null) {
                    drawEmoji = true;
                } else if (targetChar == '\t') {
                    controlChar = "\u02EA";
                    drawControlChar = true;
                } else if (targetChar == '\n') {
                    controlChar = "\u21B2";
                    drawControlChar = true;
                }

                Point pointOrig = this.GetPositionFromCharIndex(chIndex);
                Point point = this.FixCharPosition(textBoxRect, pointOrig);

                if (drawEmoji) {
                    Image image = emoji.ImageForText;
                    Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                    Rectangle destRect = new Rectangle(point.X, point.Y, Commons.TEXT_ICON_WIDTH, Commons.TEXT_ICON_HEIGHT);

                    if (this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                        // 選択されている場合は、反転して描画する
                        TextRenderer.DrawText(graphics, "\u3000", this.Font, new Point(point.X, point.Y), this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                        graphics.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, this.negativeImageAttributes);
                    } else {
                        graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                    }

                    //@SELTABSTOP:virColumn += 2;

                } else if (drawControlChar) {

                    //@SELTABSTOP:int numColumn = 8 - (virColumn % 8);
                    if (!emojiOnly) {
                        if (this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                            // 選択されている場合は、反転の背景を先に描画する
                            if (targetChar == '\t') {
                                //@SELTABSTOP:if (chIndex + 1 < targetText.Length) {
                                //@SELTABSTOP:    Point pointNextOrig = this.GetPositionFromCharIndex(chIndex + 1);
                                //@SELTABSTOP:    Point pointNext = this.FixCharPosition(textBoxRect, pointNextOrig);
                                //@SELTABSTOP:    graphics.FillRectangle(this.selectionCharBackBrush, new Rectangle(point.X, point.Y, pointNext.X - point.X, fontSize.Height));
                                //@SELTABSTOP:} else {
                                //@SELTABSTOP:    int width = fontSizeHalf.Width * numColumn;
                                //@SELTABSTOP:    graphics.FillRectangle(this.selectionCharBackBrush, new Rectangle(point.X, point.Y, width, fontSize.Height));
                                //@SELTABSTOP:}
                            } else if (targetChar == '\n') {
                                TextRenderer.DrawText(graphics, "\x20", this.Font, new Point(point.X, point.Y), this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                            }
                        }
                        TextRenderer.DrawText(graphics, controlChar, this.Font, new Point(point.X, point.Y), controlCharColor, this.textFormatFlags);
                    }

                    //@SELTABSTOP:// NOTE: '\r' は表示されないので数えない
                    //@SELTABSTOP:if (targetChar == '\t') {
                    //@SELTABSTOP:    virColumn += numColumn;
                    //@SELTABSTOP:} else if (targetChar == '\n') {
                    //@SELTABSTOP:    virColumn = 0;
                    //@SELTABSTOP:}

                } else if (drawNormalChar) {

                    if (this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                        TextRenderer.DrawText(graphics, targetSChar, this.Font, new Point(point.X, point.Y), this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                    } else {
                        TextRenderer.DrawText(graphics, targetSChar, this.Font, new Point(point.X, point.Y), this.ForeColor, this.BackColor, this.textFormatFlags);
                    }

                    //@SELTABSTOP:if (StringUtils.IsHalfSizeDisplay(targetSChar)) {
                    //@SELTABSTOP:    virColumn += 1;
                    //@SELTABSTOP:} else {
                    //@SELTABSTOP:    virColumn += 2;
                    //@SELTABSTOP:}
                }
            }

            if (!emojiOnly && 0 < this.ColumnLine) {
                int x = textBoxRect.Left + (fontSize.Width * this.ColumnLine);
                graphics.DrawLine(columnLinePen, new Point(x, 0), new Point(x, this.ClientSize.Height));
            }
        }

        /// <summary>
        /// 表示座標を微調整する
        /// FIXME: 無理矢理なので .NET Framework の実装によっては破綻する可能性あり
        /// </summary>
        /// <param name="textBoxRect">EM_GETRECT で取得した RECT 構造体</param>
        /// <param name="point"></param>
        private Point FixCharPosition(Commons.RECT textBoxRect, Point point)
        {
            Point result = new Point(point.X, point.Y);

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
