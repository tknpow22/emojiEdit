using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum TemplateInputFormResult
    {
        SetTemplate,
        Cancel,
    }

    //
    // テンプレート
    //
    public partial class TemplateInputForm : Form
    {
        // フォームからの返却値
        private TemplateInputFormResult formResult = TemplateInputFormResult.Cancel;

        // テンプレート一覧データ
        private List<EmojiTemplate> emojiTemplateList;

        // 選択領域
        private int selectedY;
        private EmojiTemplate selectedEmojiTemplate;

        // 絵文字編集操作(テンプレート)
        private EditEmojiOperation editEmojiOperationTemplate;

        // コンテキストメニューが表示された時の pictureTemplate 上の座標位置
        private int ctxMenuTemplateX = 0;
        private int ctxMenuTemplateY = 0;

        // コンストラクタ
        public TemplateInputForm()
        {
            InitializeComponent();

            this.pictureTemplateList.Size = new Size(0, 0);

            this.numericUpDownCols.Minimum = Commons.TEMPLATE_MAX_COLS_MINIMUM;
            this.numericUpDownCols.Maximum = Commons.TEMPLATE_MAX_COLS_MAXIMUM;
            this.numericUpDownRows.Minimum = Commons.TEMPLATE_MAX_ROWS_MINIMUM;
            this.numericUpDownRows.Maximum = Commons.TEMPLATE_MAX_ROWS_MAXIMUM;

            this.numericUpDownCols.Value = Commons.TEMPLATE_MAX_COLS_MINIMUM;
            this.numericUpDownRows.Value = Commons.TEMPLATE_MAX_ROWS_MINIMUM;

            this.editEmojiOperationTemplate = new EditEmojiOperation(
                    this,
                    this.panelTemplate,
                    this.pictureTemplate,
                    Commons.TEMPLATE_MAX_COLS_MINIMUM,
                    Commons.TEMPLATE_MAX_ROWS_MINIMUM
                );
            this.editEmojiOperationTemplate.Clear();

            this.emojiTemplateList = DataBags.Templates.Get();

            this.DrawTemplateList();

            this.buttonSelectFromList.Enabled = false;
            this.buttonDelete.Enabled = false;
        }

        // このダイアログを表示する
        public new TemplateInputFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // プロパティ
        //

        // SetTemplate

        public List<int> Template
        {
            private set;
            get;
        }

        public int Cols
        {
            private set;
            get;
        }

        public int Rows
        {
            private set;
            get;
        }

        //
        // イベントハンドラ
        //

        // フォームロード時
        private void TemplateInputForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.buttonSetTemplate;
        }

        // フォームでキー押下
        private void TemplateInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape) {
            //    this.Cancel();
            //}
        }

        // フォームが閉じられた時
        private void TemplateInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveTemplate();
        }

        // 一覧から取得
        private void buttonSelectFromList_Click(object sender, EventArgs e)
        {
            if (this.selectedEmojiTemplate == null) {
                return;
            }

            EmojiTemplate emojiTemplate = this.selectedEmojiTemplate;
            List<int> codeList = emojiTemplate.CodeList;

            this.editEmojiOperationTemplate.MaxCols = emojiTemplate.Cols;
            this.editEmojiOperationTemplate.MaxRows = emojiTemplate.Rows;
            this.editEmojiOperationTemplate.Clear();

            this.editEmojiOperationTemplate.LoadFromCodes(codeList, 0, 0);

            this.numericUpDownCols.Value = emojiTemplate.Cols;
            this.numericUpDownRows.Value = emojiTemplate.Rows;
        }

        // 一覧へ追加
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            if (!this.CheckTemplate()) {
                return;
            }

            if (0 <= this.HasTemplateCodes(this.editEmojiOperationTemplate.Contents)) {
                MsgBox.Show(this, "同じテンプレートが存在します。", "既存テンプレートあり", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<int> codeList = new List<int>(this.editEmojiOperationTemplate.Contents);

            EmojiTemplate emojiTemplate = new EmojiTemplate(this.editEmojiOperationTemplate.MaxCols, this.editEmojiOperationTemplate.MaxRows, codeList);
            this.emojiTemplateList.Insert(0, emojiTemplate);

            this.DrawTemplateList();
            this.selectedY = 0;
            this.selectedEmojiTemplate = null;

            this.buttonSelectFromList.Enabled = false;
            this.buttonDelete.Enabled = false;
        }

        // 削除
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (this.selectedEmojiTemplate == null) {
                return;
            }

            EmojiTemplate emojiTemplate = this.selectedEmojiTemplate;
            this.emojiTemplateList.Remove(emojiTemplate);

            this.DrawTemplateList();
            this.selectedY = 0;
            this.selectedEmojiTemplate = null;

            this.buttonSelectFromList.Enabled = false;
            this.buttonDelete.Enabled = false;
        }

        // 更新
        private void buttonUpdateTemplateRegion_Click(object sender, EventArgs e)
        {
            int cols = (int)this.numericUpDownCols.Value;
            int rows = (int)this.numericUpDownRows.Value;

            List<int> codeListOrig = this.editEmojiOperationTemplate.Contents;

            int currentCols = this.editEmojiOperationTemplate.MaxCols;
            int currentRows = this.editEmojiOperationTemplate.MaxRows;

            this.editEmojiOperationTemplate.MaxCols = cols;
            this.editEmojiOperationTemplate.MaxRows = rows;
            this.editEmojiOperationTemplate.Clear();

            this.editEmojiOperationTemplate.InsertCodes(codeListOrig, currentCols, currentRows, 0, 0);
        }

        // 消去
        private void buttonClearTemplateRegion_Click(object sender, EventArgs e)
        {
            int cols = (int)this.numericUpDownCols.Value;
            int rows = (int)this.numericUpDownRows.Value;

            this.editEmojiOperationTemplate.MaxCols = cols;
            this.editEmojiOperationTemplate.MaxRows = rows;
            this.editEmojiOperationTemplate.Clear();
        }

        // 設定
        private void buttonSetTemplate_Click(object sender, EventArgs e)
        {
            if (!this.CheckTemplate()) {
                return;
            }

            List<int> codeList = this.editEmojiOperationTemplate.Contents;
            int cols = this.editEmojiOperationTemplate.MaxCols;
            int rows = this.editEmojiOperationTemplate.MaxRows;

            this.SetTemplate(codeList, cols, rows);
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        //
        // イベントハンドラ - テンプレート一覧
        //

        // パネル部のクリック時
        private void panelTemplateList_MouseClick(object sender, MouseEventArgs e)
        {
            this.HideSelectedRegion();

            this.selectedY = 0;
            this.selectedEmojiTemplate = null;

            this.buttonSelectFromList.Enabled = false;
            this.buttonDelete.Enabled = false;
        }

        // クリック時
        private void pictureTemplateList_MouseClick(object sender, MouseEventArgs e)
        {
            // NOTE: 注意: ピクチャーボックスのサイズによっては Image を null に設定しても、
            //             Size の値がデザイン時のものとなり、イベントが発生する。
            //             一応、初期化時に Size(0, 0) を設定しているが、イベントが入ってくる可能性もあるので、気をつけること

            (int y, EmojiTemplate emojiTemplate) = this.SelectedItem(e.X, e.Y);

            this.HideSelectedRegion();

            if (emojiTemplate != null) {
                int height = Commons.FRAME_HEIGHT * emojiTemplate.Rows;
                int width = Commons.FRAME_WIDTH * emojiTemplate.Cols;

                this.ShowSelectRegion(0, y, width, height);

                this.selectedY = y;
                this.selectedEmojiTemplate = emojiTemplate;

                this.buttonSelectFromList.Enabled = true;
                this.buttonDelete.Enabled = true;

            } else {
                this.selectedY = 0;
                this.selectedEmojiTemplate = null;

                this.buttonSelectFromList.Enabled = false;
                this.buttonDelete.Enabled = false;
            }
        }

        // ダブルクリック時
        private void pictureTemplateList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // NOTE: MouseClick が先に起きることに依存している
            if (this.selectedEmojiTemplate == null) {
                return;
            }

            List<int> codeList = this.selectedEmojiTemplate.CodeList;
            int cols = this.selectedEmojiTemplate.Cols;
            int rows = this.selectedEmojiTemplate.Rows;

            this.SetTemplate(this.selectedEmojiTemplate.CodeList, cols, rows);
        }

        //
        // イベントハンドラ - テンプレート
        //

        // ピクチャーイメージ(テンプレート)をクリック
        private void pictureTemplate_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) {
                this.ctxMenuTemplateX = e.X;
                this.ctxMenuTemplateY = e.Y;

                this.contextMenuTemplate.Show(Cursor.Position);
                return;
            }

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.CallSelectEmoji(col, row);
        }

        //
        // イベントハンドラ - コンテキストメニュー(テンプレート)
        //

        // コンテキストメニュー制御
        private void contextMenuTemplate_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Clipboard.ContainsText(TextDataFormat.Text)) {
                menuPasteTextTemplate.Enabled = true;
            } else {
                menuPasteTextTemplate.Enabled = false;
            }

        }

        // 文字列挿入
        private void menuInsertTextTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.CallEditText(col, row);
        }

        // 貼り付け挿入
        private void menuPasteTextTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            string text = Clipboard.GetText() ?? "";
            this.editEmojiOperationTemplate.InsertText(text, col, row);
        }

        // 改行
        private void menuNewLineTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.NewLine(col, row);
        }

        // 空行挿入
        private void menuInsertEmptyLineTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.InsertEmptyLine(col, row);
        }

        // 空文字挿入
        private void menuInsertCharTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.CallInsertChar(col, row);
        }

        // 行削除
        private void menuRemoveLineTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.RemoveLine(col, row);
        }

        // 文字削除
        private void menuRemoveCharTemplate_Click(object sender, EventArgs e)
        {
            int col = this.ctxMenuTemplateX / Commons.FRAME_WIDTH;
            int row = this.ctxMenuTemplateY / Commons.FRAME_HEIGHT;

            this.editEmojiOperationTemplate.CallRemoveChar(col, row);
        }

        //
        // 内部処理
        //

        // テンプレート一覧を描画する
        private void DrawTemplateList()
        {
            if (this.emojiTemplateList.Count != 0) {
                int maxCols = Commons.TEMPLATE_MAX_COLS_MAXIMUM;

                int maxRows = this.emojiTemplateList.Sum(template => template.Rows);

                int maxWidth = Commons.FRAME_WIDTH * maxCols + 1;
                int maxHeight = Commons.FRAME_HEIGHT * maxRows + 1;

                Image image = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(image)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    int row = 0;
                    foreach (EmojiTemplate template in this.emojiTemplateList) {
                        int width = Commons.FRAME_WIDTH * template.Cols;
                        int height = Commons.FRAME_HEIGHT * template.Rows;

                        Rectangle srcRect = new Rectangle(0, 0, width, height);
                        Rectangle desRect = new Rectangle(0, Commons.FRAME_HEIGHT * row, width, height);

                        graphics.DrawImage(template.Thumbnail, desRect, srcRect, GraphicsUnit.Pixel);

                        graphics.DrawRectangle(Pens.Black, desRect);

                        row += template.Rows;
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
                int height = Commons.FRAME_HEIGHT * emojiTemplate.Rows;
                int width = Commons.FRAME_WIDTH * emojiTemplate.Cols;

                if (py < y && y < py + height && 0 < x && x < width) {
                    return (py, emojiTemplate);
                }

                py += height;
            }

            return (0, null);
        }

        // テンプレート一覧の選択された内容に矩形を描画する
        private void ShowSelectRegion(int x, int y, int width, int height)
        {
            Image image = this.pictureTemplateList.Image;
            if (image != null) {
                using (Graphics graphics = Graphics.FromImage(image)) {
                    Rectangle desRect = new Rectangle(x, y, width, height);
                    graphics.DrawRectangle(Pens.Red, desRect);
                }
                this.pictureTemplateList.Image = image;
            }
        }

        // テンプレート一覧の選択された内容の矩形を解除する(通常枠を再描画する)
        private void HideSelectedRegion()
        {
            if (this.selectedEmojiTemplate != null) {
                int width = Commons.FRAME_WIDTH * this.selectedEmojiTemplate.Cols;
                int height = Commons.FRAME_HEIGHT * this.selectedEmojiTemplate.Rows;

                Image image = this.pictureTemplateList.Image;
                if (image != null) {
                    using (Graphics graphics = Graphics.FromImage(image)) {
                        Rectangle desRect = new Rectangle(0, this.selectedY, width, height);
                        graphics.DrawRectangle(Pens.Black, desRect);
                    }
                    this.pictureTemplateList.Image = image;
                }
            }
        }

        // 同じテンプレートが保持されているかを返す
        private int HasTemplateCodes(List<int> codeList)
        {
            for (int i = 0; i < this.emojiTemplateList.Count; ++i) {
                EmojiTemplate emojiTemplate = this.emojiTemplateList[i];
                if (emojiTemplate.CodeList.SequenceEqual(codeList)) {
                    return i;
                }
            }
            return -1;
        }

        // 入力されたテンプレートを設定する
        private void SetTemplate(List<int> codeList, int cols, int rows)
        {
            int index = this.HasTemplateCodes(codeList);
            if (0 <= index) {
                EmojiTemplate emojiTemplate = this.emojiTemplateList[index];
                this.emojiTemplateList.RemoveAt(index);
                this.emojiTemplateList.Insert(0, emojiTemplate);
            } else {
                EmojiTemplate emojiTemplate = new EmojiTemplate(this.editEmojiOperationTemplate.MaxCols, this.editEmojiOperationTemplate.MaxRows, codeList);
                this.emojiTemplateList.Insert(0, emojiTemplate);
            }

            this.Template = codeList;
            this.Cols = cols;
            this.Rows = rows;

            this.formResult = TemplateInputFormResult.SetTemplate;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = TemplateInputFormResult.Cancel;
            this.Close();
        }

        // テンプレートをチェックする
        private bool CheckTemplate()
        {
            // チェック(必須)
            {
                List<int> codeList = this.editEmojiOperationTemplate.Contents;
                int code = codeList.FirstOrDefault(lcode => lcode != 0);
                if (code == 0) {
                    MsgBox.Show(this, "テンプレートに値を設定してください。", "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        // テンプレートを保存する
        private void SaveTemplate()
        {
            DataBags.Templates.Set(this.emojiTemplateList);
        }
    }
}
