using System;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum EditSettingsFormResult
    {
        Ok,
        Cancel,
    }

    //
    // 編集設定
    //
    public partial class EditSettingsForm : Form
    {
        // フォームからの返却値
        private EditSettingsFormResult formResult = EditSettingsFormResult.Cancel;

        // コンスラクタ
        public EditSettingsForm()
        {
            InitializeComponent();

            // 本文編集
            this.numericUpDownBodyCols.Minimum = ConfigBag.BODY_MAX_COLS_MINIMUM;
            this.numericUpDownBodyCols.Maximum = ConfigBag.BODY_MAX_COLS_MAXIMUM;
            this.numericUpDownBodyRows.Minimum = ConfigBag.BODY_MAX_ROWS_MINIMUM;
            this.numericUpDownBodyRows.Maximum = ConfigBag.BODY_MAX_ROWS_MAXIMUM;

            this.numericUpDownBodyCols.Value = DataBags.Config.BodyMaxCols;
            this.numericUpDownBodyRows.Value = DataBags.Config.BodyMaxRows;

            // 絵文字選択
            this.numericUpDownEmojiListCols.Minimum = ConfigBag.EMOJI_LIST_MAX_COLS_MINIMUM;
            this.numericUpDownEmojiListCols.Maximum = ConfigBag.EMOJI_LIST_MAX_COLS_MAXIMUM;

            this.numericUpDownEmojiListCols.Value = DataBags.Config.EmojiListMaxCols;
        }

        // このダイアログを表示する
        public new EditSettingsFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // イベントハンドラ
        //

        // 設定
        private void buttonSet_Click(object sender, EventArgs e)
        {
            this.CloseIfValid();
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.formResult = EditSettingsFormResult.Cancel;
            this.Close();
        }

        // フォームでキー押下
        private void EditSettingsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) {
                this.formResult = EditSettingsFormResult.Cancel;
                this.Close();
            } else if (e.KeyChar == (char)Keys.Enter) {
                this.CloseIfValid();
            }
        }

        //
        // 内部処理
        //

        // チェックして閉じる
        private void CloseIfValid()
        {
            this.formResult = EditSettingsFormResult.Ok;

            // NOTE: NumericUpDown コントロール側で補正されるためチェックしていない

            DataBags.Config.BodyMaxCols = (int)this.numericUpDownBodyCols.Value;
            DataBags.Config.BodyMaxRows = (int)this.numericUpDownBodyRows.Value;

            DataBags.Config.EmojiListMaxCols = (int)this.numericUpDownEmojiListCols.Value;

            this.Close();
        }
    }
}
