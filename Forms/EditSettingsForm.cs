namespace emojiEdit
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// フォームからの返却値
    /// </summary>
    public enum EditSettingsFormResult
    {
        Ok,
        Cancel,
    }

    /// <summary>
    /// 編集設定
    /// </summary>
    public partial class EditSettingsForm : Form
    {
        #region 変数

        /// <summary>
        /// フォームからの返却値
        /// </summary>
        private EditSettingsFormResult formResult = EditSettingsFormResult.Cancel;

        #endregion

        #region 処理

        /// <summary>
        /// コンスラクタ
        /// </summary>
        public EditSettingsForm()
        {
            InitializeComponent();

            // 本文編集
            this.numericUpDownBodyCols.Minimum = Commons.MAX_BODY_COLS_MINIMUM;
            this.numericUpDownBodyCols.Maximum = Commons.MAX_BODY_COLS_MAXIMUM;

            this.numericUpDownBodyCols.Value = DataBags.Config.MaxBodyCols;

            // 絵文字選択
            this.numericUpDownEmojiListCols.Minimum = Commons.MAX_EMOJI_LIST_COLS_MINIMUM;
            this.numericUpDownEmojiListCols.Maximum = Commons.MAX_EMOJI_LIST_COLS_MAXIMUM;

            this.numericUpDownEmojiListCols.Value = DataBags.Config.MaxEmojiListCols;

            //// 本文のちらつきを少しだけ抑制する
            //this.checkBoxSuppressBodyTextFlicker.Checked = DataBags.Config.SuppressBodyTextFlicker;
        }

        /// <summary>
        /// このダイアログを表示する
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <returns>フォームからの返却値</returns>
        public new EditSettingsFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// Form - KeyPress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditSettingsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) {
                this.formResult = EditSettingsFormResult.Cancel;
                this.Close();
            } else if (e.KeyChar == (char)Keys.Enter) {
                this.CloseIfValid();
            }
        }

        /// <summary>
        /// 設定ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSet_Click(object sender, EventArgs e)
        {
            this.CloseIfValid();
        }

        /// <summary>
        /// キャンセルボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.formResult = EditSettingsFormResult.Cancel;
            this.Close();
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// チェックして閉じる
        /// </summary>
        private void CloseIfValid()
        {
            this.formResult = EditSettingsFormResult.Ok;

            // NOTE: 値の範囲は NumericUpDown コントロール側で補正されるためチェックしていない
            //       チェックボックスも同じ

            DataBags.Config.MaxBodyCols = (int)this.numericUpDownBodyCols.Value;
            DataBags.Config.MaxEmojiListCols = (int)this.numericUpDownEmojiListCols.Value;
            //DataBags.Config.SuppressBodyTextFlicker = this.checkBoxSuppressBodyTextFlicker.Checked;

            this.Close();
        }

        #endregion
    }
}
