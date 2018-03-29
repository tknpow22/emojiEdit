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
    public partial class EmojiTextBox : TextBox
    {
        #region Windows API

        #region 定数

        private const int WM_PAINT = 0x000F;
        private const int WM_HSCROLL = 0x0114;
        private const int WM_VSCROLL = 0x0115;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_MOUSEWHEEL = 0x020A;

        private const int EM_SETWORDBREAKPROC = 0x00D0;
        private const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        private const int EM_GETRECT = 0x00B2;

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
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, EditWordBreakProcDelegate lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, out Commons.RECT lParam);
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
        /// マウスの左ボタンを押している間 true にするフラグ
        /// </summary>
        private bool mouseLButtonDown = false;

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

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmojiTextBox()
        {
            this.InitializeComponent();

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
                this.Draw(graphics, true);
            }
        }

        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            bool draw = false;
            bool invalidate = false;
            if (m.Msg == WM_PAINT) {
                draw = true;
            } else if (m.Msg == WM_LBUTTONUP) {
                this.mouseLButtonDown = false;
                invalidate = true;
            } else if (m.Msg == WM_LBUTTONDOWN) {
                this.mouseLButtonDown = true;
                ////invalidate = true;  // 少しでも再描画を減らす
            } else if (m.Msg == WM_HSCROLL
                    || m.Msg == WM_VSCROLL
                    ////|| m.Msg == WM_KEYDOWN  // 少しでも再描画を減らす
                    || m.Msg == WM_KEYUP
                    || m.Msg == WM_MOUSEWHEEL) {
                invalidate = true;
            } else if (m.Msg == WM_MOUSEMOVE && this.mouseLButtonDown) {
                invalidate = true;
            }

            if (draw) {
                try {

                    using (Graphics graphics = Graphics.FromHwnd(this.Handle)) {
                        this.Draw(graphics, false);
                    }

                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

            } else if (invalidate) {
                this.Invalidate();
            }
        }

        /// <summary>
        /// 絵文字等の描画を行う
        /// </summary>
        /// <param name="graphics">Graphics</param>
        /// <param name="emojiOnly">絵文字のみ描画</param>
        private void Draw(Graphics graphics, bool emojiOnly)
        {
            Commons.RECT textBoxRect;
            SendMessage(this.Handle, EM_GETRECT, 0, out textBoxRect);

            Size fontSize = TextRenderer.MeasureText(graphics, "■", this.Font, new Size(), this.textFormatFlags);

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

                Point point = this.GetPositionFromCharIndex(chIndex);

                int pointX = point.X;
                int pointY = point.Y;

                // 表示座標を微調整する
                // FIXME: 無理矢理なので .NET Framework の実装によっては破綻する可能性あり
                if (!this.Multiline) {
                    pointY += textBoxRect.Top;
                }

                if (drawEmoji) {
                    Image image = emoji.ImageForText;
                    Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
                    Rectangle destRect = new Rectangle(pointX, pointY, Commons.TEXT_ICON_WIDTH, Commons.TEXT_ICON_HEIGHT);

                    if (this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                        graphics.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, this.negativeImageAttributes);
                    } else {
                        graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                } else if (drawControlChar) {
                    if (!emojiOnly) {
                        if (targetChar == '\n' && this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                            // 改行が選択されている場合は、反転して描画する
                            TextRenderer.DrawText(graphics, "\x20", this.Font, new Point(pointX, pointY), this.selectionCharForeColor, this.selectionCharBackColor, this.textFormatFlags);
                        }
                        TextRenderer.DrawText(graphics, controlChar, this.Font, new Point(pointX, pointY), controlCharColor, this.textFormatFlags);
                    }
                }
            }

            if (!emojiOnly && 0 < this.ColumnLine) {
                int x = textBoxRect.Left + (fontSize.Width * this.ColumnLine);
                graphics.DrawLine(columnLinePen, new Point(x, 0), new Point(x, this.ClientSize.Height));
            }
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
