using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace emojiEdit
{
    //
    // メッセージボックス
    //
    class MsgBox
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HOOKPROC lpfn, IntPtr hInstance, IntPtr threadId);
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);
        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hHook, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        private delegate IntPtr HOOKPROC(int nCode, IntPtr wParam, IntPtr lParam);

        private const int GWL_HINSTANCE = (-6);
        private const int WH_CBT = 5;
        private const int HCBT_ACTIVATE = 5;

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOACTIVATE = 0x0010;

        private struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        //
        // 変数
        //

        // 親ウィンドウ
        private IWin32Window ownerWindow = null;

        // フックハンドル
        private IntPtr hHook = (IntPtr)0;

        //
        // 処理
        //

        // メッセージボックスを表示する
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

        //
        // 内部処理
        //

        // コンストラクタ
        private MsgBox(IWin32Window window)
        {
            this.ownerWindow = window;
        }

        // メッセージボックスを表示する
        private DialogResult Show(
            string messageBoxText,
            string caption,
            MessageBoxButtons button,
            MessageBoxIcon icon)
        {
            // フックを設定する。
            IntPtr hInstance = GetWindowLong(this.ownerWindow.Handle, GWL_HINSTANCE);
            IntPtr threadId = GetCurrentThreadId();
            this.hHook = SetWindowsHookEx(WH_CBT, new HOOKPROC(HookProc), hInstance, threadId);

            return MessageBox.Show(this.ownerWindow, messageBoxText, caption, button, icon);
        }

        // フックプロシージャ
        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == HCBT_ACTIVATE) {
                RECT rcForm = new RECT(0, 0, 0, 0);
                RECT rcMsgBox = new RECT(0, 0, 0, 0);

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
    }
}
