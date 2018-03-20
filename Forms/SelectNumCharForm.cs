using System;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum SelectNumCharFormResult
    {
        SetNumChar,
        Cancel,
    }

    //
    // 文字数指定
    //
    public partial class SelectNumCharForm : Form
    {
        // フォームからの返却値
        private SelectNumCharFormResult formResult = SelectNumCharFormResult.Cancel;

        // 挿入か
        private bool isInsert = false;

        // コンストラクタ
        public SelectNumCharForm(bool isInsert)
        {
            InitializeComponent();

            this.isInsert = isInsert;

            if (this.isInsert) {
                this.Text = "挿入" + this.Text;
            } else {
                this.Text = "削除" + this.Text;
            }

            this.numericUpDownNumChar.Minimum = 1;
            this.numericUpDownNumChar.Maximum = ConfigBag.BODY_MAX_COLS_MAXIMUM;
        }

        // このダイアログを表示する
        public new SelectNumCharFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // プロパティ
        //

        // SetNumChar

        public int NumChar
        {
            private set;
            get;
        }

        //
        // イベントハンドラ
        //

        // 設定
        private void buttonSetNumChar_Click(object sender, EventArgs e)
        {
            this.SetNumChar();
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        // 文字数でキー押下
        private void numericUpDownNumChar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                this.SetNumChar();
            }
        }

        // フォームでキー押下
        private void EditTextForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Cancel();
            }
        }

        //
        // 内部処理
        //

        // 入力された文字数を設定する
        private void SetNumChar()
        {
            this.NumChar = (int)this.numericUpDownNumChar.Value;

            this.formResult = SelectNumCharFormResult.SetNumChar;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = SelectNumCharFormResult.Cancel;
            this.Close();
        }
    }
}
