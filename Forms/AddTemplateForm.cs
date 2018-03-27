namespace emojiEdit
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// フォームからの返却値
    /// </summary>
    public enum AddTemplateFormResult
    {
        AddTemplate,
        Cancel,
    }

    /// <summary>
    /// テンプレート追加
    /// </summary>
    public partial class AddTemplateForm : Form
    {
        #region 変数

        /// <summary>
        /// フォームからの返却値
        /// </summary>
        private AddTemplateFormResult formResult = AddTemplateFormResult.Cancel;

        /// <summary>
        /// テンプレート
        /// </summary>
        private EmojiTemplate emojiTemplate;

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddTemplateForm(string templateText)
        {
            InitializeComponent();

            this.textBoxTemplate.Font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);
            this.textBoxTemplate.Text = templateText;

            this.emojiTemplate = new EmojiTemplate(templateText);
            this.pictureTemplate.Image = this.emojiTemplate.Thumbnail;
        }

        /// <summary>
        /// このダイアログを表示する
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <returns>フォームからの返却値</returns>
        public new AddTemplateFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// From - Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTemplateForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.buttonAddTemplate;
        }

        /// <summary>
        /// Form - KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTemplateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Cancel();
            }
        }

        /// <summary>
        /// 追加ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddTemplate_Click(object sender, EventArgs e)
        {
            this.AddTemplate();
        }

        /// <summary>
        /// キャンセルボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// テンプレートを追加する
        /// </summary>
        private void AddTemplate()
        {
            DataBags.Templates.Add(this.emojiTemplate);

            this.formResult = AddTemplateFormResult.AddTemplate;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = AddTemplateFormResult.Cancel;
            this.Close();
        }

        #endregion
    }
}
