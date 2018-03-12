using System;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum EditTextFormResult
    {
        SetText,
        Cancel,
    }

    //
    // 文字挿入
    //
    public partial class EditTextForm : Form
    {
        // フォームからの返却値
        private EditTextFormResult formResult = EditTextFormResult.Cancel;

        // コンストラクタ
        public EditTextForm()
        {
            InitializeComponent();
        }

        // このダイアログを表示する
        public new EditTextFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // プロパティ
        //

        // SetText

        public string InputText
        {
            private set;
            get;
        }

        //
        // イベントハンドラ
        //

        // 設定
        private void buttonSetStr_Click(object sender, EventArgs e)
        {
            this.SetText();
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        // 文字領域でキー押下
        private void textBoxInputText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) {
                if (e.Control || e.Shift) {
                    this.SetText();
                }
            }
        }

        // フォームでキー押下
        private void EditTextForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape) {
            //    this.Cancel();
            //}
        }

        //
        // 内部処理
        //

        // 入力された文字を設定する
        private void SetText()
        {
            this.InputText = textBoxInputText.Text;

            this.formResult = EditTextFormResult.SetText;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = EditTextFormResult.Cancel;
            this.Close();
        }
    }
}
