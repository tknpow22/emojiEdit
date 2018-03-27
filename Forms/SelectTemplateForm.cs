namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// フォームからの返却値
    /// </summary>
    public enum SelectTemplateFormResult
    {
        SelectTemplate,
        Cancel,
    }

    /// <summary>
    /// テンプレート選択
    /// </summary>
    public partial class SelectTemplateForm : Form
    {
        #region 変数

        /// <summary>
        /// フォームからの返却値
        /// </summary>
        private SelectTemplateFormResult formResult = SelectTemplateFormResult.Cancel;

        /// <summary>
        /// テンプレート一覧データ
        /// </summary>
        private List<EmojiTemplate> emojiTemplateList;

        /// <summary>
        /// 1 つのテンプレートの幅
        /// </summary>
        private int aTemplateImageWidth;

        /// <summary>
        /// 1 つのテンプレートの高さ
        /// </summary>
        private int aTemplateImageHeight;

        #region 選択領域

        /// <summary>
        /// 選択時の Y 座標
        /// </summary>
        private int selectedY;

        /// <summary>
        /// 選択中のテンプレート
        /// </summary>
        private EmojiTemplate selectedEmojiTemplate;

        #endregion

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectTemplateForm()
        {
            InitializeComponent();

            this.pictureTemplateList.Size = new Size(0, 0);

            this.emojiTemplateList = DataBags.Templates.Get();

            this.aTemplateImageWidth = Commons.FRAME_WIDTH * Commons.TEMPLATE_THUMBNAIL_COLS;
            this.aTemplateImageHeight = Commons.FRAME_HEIGHT * Commons.TEMPLATE_THUMBNAIL_ROWS;

            this.DrawTemplateList();

            this.buttonDelete.Enabled = false;
            this.buttonSelectTemplate.Enabled = false;
        }

        /// <summary>
        /// このダイアログを表示する
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <returns>フォームからの返却値</returns>
        public new SelectTemplateFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        #endregion

        #region プロパティ

        #region SelectTemplate

        public string Template
        {
            private set;
            get;
        }

        #endregion

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// Form - KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTemplateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                this.Cancel();
            }
        }

        /// <summary>
        /// Form - FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectTemplateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveTemplate();
        }

        /// <summary>
        /// 削除ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.selectedEmojiTemplate == null) {
                return;
            }

            this.emojiTemplateList.Remove(this.selectedEmojiTemplate);

            this.DrawTemplateList();
            this.selectedY = 0;
            this.selectedEmojiTemplate = null;

            this.buttonDelete.Enabled = false;
            this.buttonSelectTemplate.Enabled = false;
        }

        /// <summary>
        /// 設定ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectTemplate_Click(object sender, EventArgs e)
        {
            if (!this.CheckTemplate()) {
                return;
            }

            this.SelectTemplate();
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

        #region イベントハンドラ - テンプレート一覧

        /// <summary>
        /// パネル部 - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelTemplateList_MouseClick(object sender, MouseEventArgs e)
        {
            this.HideSelectedRegion();

            this.selectedY = 0;
            this.selectedEmojiTemplate = null;

            this.buttonDelete.Enabled = false;
            this.buttonSelectTemplate.Enabled = false;
        }

        /// <summary>
        /// ピクチャーボックス - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureTemplateList_MouseClick(object sender, MouseEventArgs e)
        {
            // NOTE: 注意: ピクチャーボックスのサイズによっては Image を null に設定しても、
            //             Size の値がデザイン時のものとなり、イベントが発生する。
            //             一応、初期化時に Size(0, 0) を設定しているが、イベントが入ってくる可能性もあるので、気をつけること

            (int y, EmojiTemplate emojiTemplate) = this.SelectedItem(e.X, e.Y);

            this.HideSelectedRegion();

            if (emojiTemplate != null) {

                this.ShowSelectRegion(0, y);

                this.selectedY = y;
                this.selectedEmojiTemplate = emojiTemplate;

                this.buttonDelete.Enabled = true;
                this.buttonSelectTemplate.Enabled = true;

            } else {
                this.selectedY = 0;
                this.selectedEmojiTemplate = null;

                this.buttonDelete.Enabled = false;
                this.buttonSelectTemplate.Enabled = false;
            }
        }

        // ダブルクリック時
        private void pictureTemplateList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // NOTE: MouseClick が先に起きることに依存している
            this.SelectTemplate();
        }

        #endregion

        #endregion

        #region 内部処理

        // テンプレート一覧を描画する
        private void DrawTemplateList()
        {
            if (this.emojiTemplateList.Count != 0) {

                int maxWidth = this.aTemplateImageWidth + 1;
                int maxHeight = this.aTemplateImageHeight * this.emojiTemplateList.Count + 1;

                Image image = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(image)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    int row = 0;
                    foreach (EmojiTemplate template in this.emojiTemplateList) {
                        int y = this.aTemplateImageHeight * row;

                        Rectangle srcRect = new Rectangle(0, 0, this.aTemplateImageWidth, this.aTemplateImageHeight);
                        Rectangle desRect = new Rectangle(0, y, this.aTemplateImageWidth, this.aTemplateImageHeight);

                        graphics.DrawImage(template.Thumbnail, desRect, srcRect, GraphicsUnit.Pixel);

                        graphics.DrawRectangle(Pens.Black, desRect);
                        ++row;
                    }
                }

                this.pictureTemplateList.Image = image;

            } else {
                this.panelTemplateList.AutoScrollPosition = new Point(0, 0);
                this.pictureTemplateList.Image = null;
            }
        }

        // テンプレート一覧から選択された項目を取得する
        private (int, EmojiTemplate) SelectedItem(int x, int y)
        {
            int py = 0;

            foreach (EmojiTemplate emojiTemplate in this.emojiTemplateList) {

                if (py < y && y < py + this.aTemplateImageHeight && 0 < x && x < this.aTemplateImageWidth) {
                    return (py, emojiTemplate);
                }

                py += this.aTemplateImageHeight;
            }

            return (0, null);
        }

        /// <summary>
        /// テンプレート一覧の選択された内容に矩形を描画する
        /// </summary>
        /// <param name="x">X 座標</param>
        /// <param name="y">Y 座標</param>
        private void ShowSelectRegion(int x, int y)
        {
            Image image = this.pictureTemplateList.Image;
            if (image != null) {
                using (Graphics graphics = Graphics.FromImage(image)) {
                    Rectangle desRect = new Rectangle(x, y, this.aTemplateImageWidth, this.aTemplateImageHeight);
                    graphics.DrawRectangle(Pens.Red, desRect);
                }
                this.pictureTemplateList.Image = image;
            }
        }

        /// <summary>
        /// テンプレート一覧の選択された内容の矩形を解除する(通常枠を再描画する)
        /// </summary>
        private void HideSelectedRegion()
        {
            if (this.selectedEmojiTemplate != null) {
                Image image = this.pictureTemplateList.Image;
                if (image != null) {
                    using (Graphics graphics = Graphics.FromImage(image)) {
                        Rectangle desRect = new Rectangle(0, this.selectedY, this.aTemplateImageWidth, this.aTemplateImageHeight);
                        graphics.DrawRectangle(Pens.Black, desRect);
                    }
                    this.pictureTemplateList.Image = image;
                }
            }
        }

        /// <summary>
        /// テンプレートを設定する
        /// </summary>
        private void SelectTemplate()
        {
            if (this.selectedEmojiTemplate == null) {
                return;
            }

            int index = this.emojiTemplateList.IndexOf(this.selectedEmojiTemplate);
            if (0 <= index) {
                this.emojiTemplateList.RemoveAt(index);
                this.emojiTemplateList.Insert(0, this.selectedEmojiTemplate);
            }

            this.Template = this.selectedEmojiTemplate.Text;

            this.formResult = SelectTemplateFormResult.SelectTemplate;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = SelectTemplateFormResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// テンプレートをチェックする
        /// </summary>
        /// <returns></returns>
        private bool CheckTemplate()
        {
            if (this.selectedEmojiTemplate == null) {
                MsgBox.Show(this, "テンプレートを選択してください。", "テンプレートの選択", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// テンプレートを保存する
        /// </summary>
        private void SaveTemplate()
        {
            DataBags.Templates.Set(this.emojiTemplateList);
        }

        #endregion
    }
}
