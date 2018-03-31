namespace emojiEdit
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// メッセージボックス
    /// </summary>
    class MsgBox
    {
        #region Windows API

        #region 定数

        private const int GWL_HINSTANCE = (-6);
        private const int WH_CBT = 5;
        private const int HCBT_ACTIVATE = 5;

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;

        #endregion

        #region 定義

        /// <summary>
        /// HOOKPROC デリゲートの定義
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

        #endregion

        #region API

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetWindowRect(IntPtr hWnd, out Commons.RECT lpRect);

        #endregion

        #endregion

        #region 変数

        /// <summary>
        /// 親ウィンドウ
        /// </summary>
        private IWin32Window ownerWindow = null;

        /// <summary>
        /// フックハンドル
        /// </summary>
        private IntPtr hHook = (IntPtr)0;

        #endregion

        #region 処理

        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <param name="messageBoxText">メッセージ</param>
        /// <param name="caption">キャプション</param>
        /// <param name="button">ボタン指定</param>
        /// <param name="icon">アイコン指定</param>
        /// <returns>DialogResult</returns>
        public static DialogResult Show(
            IWin32Window owner,
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            MessageBoxIcon icon)
        {
            MsgBox mbox = new MsgBox(owner);
            return mbox.Show(messageBoxText, caption, button, icon);
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="window">親ウィンドウ</param>
        private MsgBox(IWin32Window window)
        {
            this.ownerWindow = window;
        }

        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="messageBoxText">メッセージ</param>
        /// <param name="caption">キャプション</param>
        /// <param name="button">ボタン指定</param>
        /// <param name="icon">アイコン指定</param>
        /// <returns>DialogResult</returns>
        private DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            MessageBoxIcon icon)
        {
            // フックを設定する
            IntPtr hInstance = GetWindowLong(this.ownerWindow.Handle, GWL_HINSTANCE);
            IntPtr threadId = GetCurrentThreadId();
            this.hHook = SetWindowsHookEx(WH_CBT, new HOOKPROC(HookProc), hInstance, threadId);

            return MessageBox.Show(this.ownerWindow, messageBoxText, caption, button, icon);
        }

        /// <summary>
        /// フックプロシージャ
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == HCBT_ACTIVATE) {
                Commons.RECT rcForm = new Commons.RECT(0, 0, 0, 0);
                Commons.RECT rcMsgBox = new Commons.RECT(0, 0, 0, 0);

                GetWindowRect(this.ownerWindow.Handle, out rcForm);
                GetWindowRect(wParam, out rcMsgBox);

                // センター位置を計算する。
                int x = (rcForm.Left + (rcForm.Right - rcForm.Left) / 2) - ((rcMsgBox.Right - rcMsgBox.Left) / 2);
                int y = (rcForm.Top + (rcForm.Bottom - rcForm.Top) / 2) - ((rcMsgBox.Bottom - rcMsgBox.Top) / 2);

                SetWindowPos(wParam, 0, x, y, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);

                IntPtr result = CallNextHookEx(this.hHook, nCode, wParam, lParam);

                // フックを解除する。
                UnhookWindowsHookEx(this.hHook);
                this.hHook = (IntPtr)0;

                return result;

            } else {
                return CallNextHookEx(this.hHook, nCode, wParam, lParam);
            }
        }

        #endregion
    }
}
