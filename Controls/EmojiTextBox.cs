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

        #region SuppressFlicker が true の時に使用する

        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            public IntPtr hdc;
            public bool fErase;
            // rcPaint was a by-value RECT structure
            public int rcPaint_left;
            public int rcPaint_top;
            public int rcPaint_right;
            public int rcPaint_bottom;
            public bool fRestore;
            public bool fIncUpdate;
            public int reserved1;
            public int reserved2;
            public int reserved3;
            public int reserved4;
            public int reserved5;
            public int reserved6;
            public int reserved7;
            public int reserved8;
        }

        #endregion

        #endregion

        #region API

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, EditWordBreakProcDelegate lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, out Commons.RECT lParam);

        #region SuppressFlicker が true の時に使用する

        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hwnd, ref PAINTSTRUCT lpPaint);
        [DllImport("user32.dll")]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT lpPaint);
        [DllImport("gdi32.dll")]
        private static extern int RealizePalette(IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectPalette(IntPtr hDC, IntPtr hPalette, int bForceBackground);

        #endregion

        #endregion

        #endregion

        #region 変数

        /// <summary>
        /// 制御文字の表示色
        /// </summary>
        private static Color controlCharColor = Color.LightBlue;

        /// <summary>
        /// カラム線描画用の Pen
        /// </summary>
        private static Pen columnLinePen = new Pen(Color.LightBlue);

        #region SuppressFlicker が true の時に使用する

        /// <summary>
        /// 通常文字の選択色
        /// </summary>
        private static Color selectionNormalCharBackColor = Color.FromArgb(0x00, 0x78, 0xd7);

        #endregion

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

        #region SuppressFlicker が true の時に使用する

        /// <summary>
        /// 背景描画色
        /// </summary>
        private Brush backgroundBrush;

        #endregion

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

            #region SuppressFlicker が true の時に使用する
            
            // 背景描画色
            this.backgroundBrush = new SolidBrush(this.BackColor);

            #endregion
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

        /// <summary>
        /// ちらつきを抑える
        /// </summary>
        [Browsable(false)]
        public bool SuppressFlicker
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
                this.Draw(graphics, false, true);
            }
        }

        #region SuppressFlicker が true の時に使用する

        //
        // Original Source
        //      https://referencesource.microsoft.com/
        //          .NET 4.6.1 for Windows 10 November Update
        //          System.Windows.Forms.Control
        //

        /// <summary>
        /// WmPaint
        /// </summary>
        /// <param name="m"></param>
        private void WmPaint(ref Message m)
        {
            bool doubleBuffered = true;

            IntPtr hWnd = IntPtr.Zero;
            IntPtr dc;
            Rectangle clip;
            PAINTSTRUCT ps = new PAINTSTRUCT();
            bool needDisposeDC = false;

            try {
                if (m.WParam == IntPtr.Zero) {
                    // Cache Handle not only for perf but to avoid object disposed exception in case the window
                    // is destroyed in an event handler (VSW#261657).
                    hWnd = this.Handle;
                    dc = BeginPaint(this.Handle, ref ps);
                    needDisposeDC = true;
                    clip = new Rectangle(ps.rcPaint_left, ps.rcPaint_top,
                                         ps.rcPaint_right - ps.rcPaint_left,
                                         ps.rcPaint_bottom - ps.rcPaint_top);
                } else {
                    dc = m.WParam;
                    clip = ClientRectangle;
                }

                // Consider: Why don't check the clip condition when non-doubleBuffered?
                //           we should probably get rid of the !doubleBuffered condition.
                if (!doubleBuffered || (clip.Width > 0 && clip.Height > 0)) {
                    IntPtr oldPal = IntPtr.Zero;
                    BufferedGraphics bufferedGraphics = null;
                    PaintEventArgs pevent = null;
                    System.Drawing.Drawing2D.GraphicsState state = null;

                    try {
                        if (doubleBuffered || m.WParam == IntPtr.Zero) {
                            oldPal = SetUpPalette(dc, false, false);
                        }

                        if (doubleBuffered) {
                            try {
                                bufferedGraphics = BufferedGraphicsManager.Current.Allocate(dc, ClientRectangle);
                            } catch (Exception ex) {
                                // BufferContext.Allocate will throw out of memory exceptions
                                // when it fails to create a device dependent bitmap while trying to 
                                // get information about the device we are painting on.
                                // That is not the same as a system running out of memory and there is a 
                                // very good chance that we can continue to paint successfully. We cannot
                                // check whether double buffering is supported in this case, and we will disable it.
                                // We could set a specific string when throwing the exception and check for it here
                                // to distinguish between that case and real out of memory exceptions but we
                                // see no reasons justifying the additional complexity.
                                if (IsCriticalException(ex) && !(ex is OutOfMemoryException)) {
                                    throw;
                                }
                                doubleBuffered = false; // paint directly on the window DC.
                            }
                        }

                        if (bufferedGraphics != null) {
                            bufferedGraphics.Graphics.SetClip(clip);
                            pevent = new PaintEventArgs(bufferedGraphics.Graphics, clip);
                            state = pevent.Graphics.Save();
                        }
                        ////} else {
                        ////    pevent = new PaintEventArgs(dc, clip);
                        ////}

                        if (pevent != null) {
                            using (pevent) {
                                try {
                                    if ((m.WParam == IntPtr.Zero) /*&& GetStyle(ControlStyles.AllPaintingInWmPaint)*/ || doubleBuffered) {
                                        ////PaintWithErrorHandling(pevent, PaintLayerBackground);
                                        bufferedGraphics.Graphics.FillRectangle(this.backgroundBrush, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
                                        // Consider: This condition could be elimiated, 
                                        //           do we have to save/restore the state of the buffered graphics? 
                                    }
                                } finally {
                                    if (state != null) {
                                        pevent.Graphics.Restore(state);
                                    }
                                    ////} else {
                                    ////    pevent.ResetGraphics();
                                    ////}
                                }
                                ////PaintWithErrorHandling(pevent, PaintLayerForeground);
                                this.Draw(bufferedGraphics.Graphics, true, false);

                                if (bufferedGraphics != null) {
                                    bufferedGraphics.Render();
                                }
                            }
                        }
                    } finally {
                        if (oldPal != IntPtr.Zero) {
                            SelectPalette(dc, oldPal, 0);
                        }
                        if (bufferedGraphics != null) {
                            bufferedGraphics.Dispose();
                        }
                    }
                }
            } finally {
                if (needDisposeDC) {
                    EndPaint(this.Handle, ref ps);
                }
            }
        }

#pragma warning disable 618
        private static bool IsCriticalException(Exception ex)
        {
            return ex is NullReferenceException
                    || ex is StackOverflowException
                    || ex is OutOfMemoryException
                    || ex is System.Threading.ThreadAbortException
                    || ex is ExecutionEngineException
                    || ex is IndexOutOfRangeException
                    || ex is AccessViolationException;
        }
#pragma warning restore 618

        /// <summary>
        /// SetUpPalette
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="force"></param>
        /// <param name="realizePalette"></param>
        /// <returns></returns>
        private static IntPtr SetUpPalette(IntPtr dc, bool force, bool realizePalette)
        {
            IntPtr halftonePalette = Graphics.GetHalftonePalette();

            IntPtr result = SelectPalette(dc, halftonePalette, (force ? 0 : 1));

            if (result != IntPtr.Zero && realizePalette) {
                RealizePalette(dc);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PAINT && this.IsHandleCreated && this.SuppressFlicker && m.HWnd == this.Handle) {
                try {
                    this.WmPaint(ref m);
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            } else {
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

                        // バッファ利用することで少しはちらつきが収まればと試したが、効果なし。
                        // また、SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true); を行い、
                        // OnPaint を override する方法は、フォント設定などがややこしく、諦めた。
                        ////
                        ////BufferedGraphicsContext bufferedGraphicsContext = BufferedGraphicsManager.Current;
                        ////using (Graphics graphicsNew = this.CreateGraphics())
                        ////using (Graphics graphicsSrc = Graphics.FromHwnd(this.Handle))
                        ////using (BufferedGraphics bufferedGraphics = bufferedGraphicsContext.Allocate(graphicsNew, this.ClientRectangle)) {
                        ////    bufferedGraphics.Graphics.SetClip(this.ClientRectangle);
                        ////    IntPtr hdcSrc = graphicsSrc.GetHdc();
                        ////    IntPtr hdcDest = bufferedGraphics.Graphics.GetHdc();
                        ////    BitBlt(hdcDest, 0, 0, this.Width, this.Height, hdcSrc, 0, 0, SRCCOPY);
                        ////    bufferedGraphics.Graphics.ReleaseHdc(hdcDest);
                        ////    graphicsSrc.ReleaseHdc(hdcSrc);
                        ////    this.Draw(bufferedGraphics.Graphics, false);
                        ////    bufferedGraphics.Render();
                        ////}

                        using (Graphics graphics = Graphics.FromHwnd(this.Handle)) {
                            this.Draw(graphics, false, false);
                        }

                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }

                } else if (invalidate) {
                    this.Invalidate();
                }
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
                        // 選択されている場合は、反転して描画する
                        if (this.SuppressFlicker) {
                            TextRenderer.DrawText(graphics, "\u3000", this.Font, new Point(pointX, pointY), Color.White, selectionNormalCharBackColor, this.textFormatFlags);
                        }
                        graphics.DrawImage(image, destRect, srcRect.X, srcRect.Y, srcRect.Width, srcRect.Height, GraphicsUnit.Pixel, this.negativeImageAttributes);
                    } else {
                        graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                } else if (drawControlChar) {
                    if (!emojiOnly) {
                        TextRenderer.DrawText(graphics, controlChar, this.Font, new Point(pointX, pointY), controlCharColor, this.textFormatFlags);
                    }
                } else if (drawNormalChar) {
                    if (this.SelectionStart <= chIndex && chIndex < this.SelectionStart + this.SelectionLength) {
                        TextRenderer.DrawText(graphics, targetSChar, this.Font, new Point(pointX, pointY), Color.White, selectionNormalCharBackColor, this.textFormatFlags);
                    } else {
                        TextRenderer.DrawText(graphics, targetSChar, this.Font, new Point(pointX, pointY), this.ForeColor, this.BackColor, this.textFormatFlags);
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

        #endregion
    }
}
