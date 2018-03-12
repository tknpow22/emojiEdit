using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum SelectEmojiFormResult
    {
        SetEmoji,
        Clear,
        Cancel,
    }

    //
    // 絵文字選択
    //
    public partial class SelectEmojiForm : Form
    {
        // 履歴および絵文字一覧で 1 行に表示する絵文字アイコンの最大数
        private int maxCols = DataBags.Config.EmojiListMaxCols;

        // 履歴に表示する絵文字アイコンの最大数
        private int maxHistoryCount;

        // 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
        private List<List<int>> emojiGroupCodeMaps = new List<List<int>>();

        // 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
        private List<Image> emojiGroupImages = new List<Image>();

        // 履歴の行数
        private int historyRows;

        // 履歴一覧イメージのサイズ
        private int historyMaxWidth;
        private int historyMaxHeight;

        // フォームからの返却値
        private SelectEmojiFormResult formResult = SelectEmojiFormResult.Cancel;

        // コンストラクタ
        public SelectEmojiForm()
        {
            InitializeComponent();

            // 履歴および絵文字一覧で 1 行に表示する絵文字アイコンの最大数
            this.maxCols = DataBags.Config.EmojiListMaxCols;

            // 履歴に表示する絵文字アイコンの最大数
            this.maxHistoryCount = this.maxCols * 2; // NOTE: MAX_COLS の倍数とすること

            // 履歴の行数
            this.historyRows = (int)Math.Ceiling((decimal)maxHistoryCount / maxCols);

            // 履歴一覧イメージのサイズ
            this.historyMaxWidth = Commons.FRAME_WIDTH * maxCols;
            this.historyMaxHeight = Commons.FRAME_HEIGHT * historyRows;

            // 絵文字アイコンと対応する文字コードをロードする
            this.LoadEmoji();

            // 画面の幅を設定する
            this.Width = Commons.FRAME_WIDTH * maxCols + 60;

            // タブを設定する
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                PictureBox pictureEmojiGroup = new PictureBox();
                pictureEmojiGroup.Location = new Point(0, 0);
                pictureEmojiGroup.Name = string.Format("pictureEmojiGroup{0}", emojiGroupNo);
                pictureEmojiGroup.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureEmojiGroup.TabIndex = emojiGroupNo;
                pictureEmojiGroup.TabStop = false;
                pictureEmojiGroup.Tag = string.Format("{0}", emojiGroupNo);
                pictureEmojiGroup.MouseClick += new MouseEventHandler(this.pictureEmojiGroupX_MouseClick);

                pictureEmojiGroup.Image = emojiGroupImages[emojiGroupNo];

                TabPage tabEmojiGroup = new TabPage();
                tabEmojiGroup.AutoScroll = true;
                tabEmojiGroup.Controls.Add(pictureEmojiGroup);
                tabEmojiGroup.Name = string.Format("tabEmojiGroup{0}", emojiGroupNo);
                tabEmojiGroup.TabIndex = emojiGroupNo;
                tabEmojiGroup.Text = DataBags.Emojis.CaptionList[emojiGroupNo - 1];
                tabEmojiGroup.UseVisualStyleBackColor = true;

                this.tabControlEmojiList.Controls.Add(tabEmojiGroup);
            }

            // 履歴を描画する
            this.RedrawHistory();
            this.pictureEmojiGroup0.Image = emojiGroupImages[0];
        }

        // このダイアログを表示する
        public new SelectEmojiFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // プロパティ
        //

        // SetEmoji

        public Image EmojiImage
        {
            private set;
            get;
        }

        public int Code
        {
            private set;
            get;
        }

        //
        // イベントハンドラ
        //

        // 消去
        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.formResult = SelectEmojiFormResult.Clear;
            this.Close();
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.formResult = SelectEmojiFormResult.Cancel;
            this.Close();
        }

        // 絵文字アイコンをクリック
        private void pictureEmojiGroupX_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBox pictureEmojiGroupX = (PictureBox)sender;

            string tag = (string)pictureEmojiGroupX.Tag;

            if (tag == null || string.Empty == tag) {
                return;
            }

            int emojiGroupNo;
            if (!int.TryParse(tag, out emojiGroupNo)) {
                return;
            }

            int numEmojiInGroup;
            if (emojiGroupNo == 0) {
                numEmojiInGroup = maxHistoryCount;
            } else {
                numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];
            }

            int maxRows = (int)Math.Ceiling((decimal)numEmojiInGroup / maxCols);

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            if (0 <= col && col < maxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < maxRows) {
                // OK
            } else {
                return;
            }

            List<int> emojiGroupCodeMap = emojiGroupCodeMaps[emojiGroupNo];

            int index = maxCols * row + col;

            if (index < emojiGroupCodeMap.Count) {
                // OK
            } else {
                return;
            }

            int code = emojiGroupCodeMap[index];
            Emoji emoji = DataBags.Emojis.Get(code);

            if (code != 0 && emoji != null) {
                this.Code = code;
                this.EmojiImage = emoji.Image;

                // 履歴へ登録する
                List<int> emojiGroupCodeMapHistory = emojiGroupCodeMaps[0];
                if (!emojiGroupCodeMapHistory.Contains(code)) {
                    emojiGroupCodeMapHistory.Insert(0, code);
                    emojiGroupCodeMapHistory.RemoveRange(maxHistoryCount, 1);
                } else {
                    emojiGroupCodeMapHistory.Remove(code);
                    emojiGroupCodeMapHistory.Insert(0, code);
                }
                this.RedrawHistory();

                DataBags.Config.SetEmojiHistory(emojiGroupCodeMapHistory);

                this.formResult = SelectEmojiFormResult.SetEmoji;
                this.Close();
            }
        }

        // フォームでキー押下
        private void EmojiListForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) {
                this.formResult = SelectEmojiFormResult.Cancel;
                this.Close();
            }
        }

        //
        // 内部処理
        //

        // 絵文字アイコンと対応する文字コードをロードする
        private void LoadEmoji()
        {
            //
            // 絵文字アイコン(履歴)
            //
            {
                List<int> emojiGroupCodeMapHistory = DataBags.Config.GetEmojiHistory();
                int emojiHistoryCount = emojiGroupCodeMapHistory.Count;

                emojiGroupCodeMapHistory.AddRange(new int[maxHistoryCount]);
                emojiGroupCodeMapHistory.RemoveRange(maxHistoryCount, emojiHistoryCount);

                emojiGroupCodeMaps.Add(emojiGroupCodeMapHistory);
            }
            {
                Image emojiGroupHistoryImage = new Bitmap(historyMaxWidth, historyMaxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {
                    graphics.FillRectangle(Brushes.White, 0, 0, historyMaxWidth, historyMaxHeight);
                }

                emojiGroupImages.Add(emojiGroupHistoryImage);
            }

            //
            // 絵文字アイコン
            //

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / maxCols);

                int maxWidth = Commons.FRAME_WIDTH * maxCols;
                int maxHeight = Commons.FRAME_HEIGHT * rows;

                List<int> emojiGroupCodeMap = new List<int>();
                emojiGroupCodeMap.AddRange(new int[numEmojiInGroup]);

                Image emojiGroupImage = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupImage)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    // 絵文字ID: グループ内でのID
                    for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                        Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                        if (emoji == null) {
                            continue;
                        }

                        emojiGroupCodeMap[emojiId] = emoji.Code;

                        int col = emojiId % maxCols;
                        int row = emojiId / maxCols;

                        Rectangle srcRect = new Rectangle(0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
                        Rectangle desRect = new Rectangle(Commons.FRAME_WIDTH * col + 1, Commons.FRAME_HEIGHT * row + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

                        graphics.DrawImage(emoji.Image, desRect, srcRect, GraphicsUnit.Pixel);
                    }
                }

                emojiGroupImages.Add(emojiGroupImage);
                emojiGroupCodeMaps.Add(emojiGroupCodeMap);
            }
        }

        // 履歴を再描画する
        private void RedrawHistory()
        {
            List<int> emojiGroupCodeMapHistory = emojiGroupCodeMaps[0];
            Image emojiGroupHistoryImage = emojiGroupImages[0];

            using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {

                graphics.FillRectangle(Brushes.White, 0, 0, historyMaxWidth, historyMaxHeight);

                for (int i = 0; i < emojiGroupCodeMapHistory.Count; ++i) {

                    int code = emojiGroupCodeMapHistory[i];
                    Emoji emoji = DataBags.Emojis.Get(code);
                    if (emoji == null) {
                        continue;
                    }

                    int col = i % maxCols;
                    int row = i / maxCols;

                    Rectangle srcRect = new Rectangle(0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
                    Rectangle desRect = new Rectangle(Commons.FRAME_WIDTH * col + 1, Commons.FRAME_HEIGHT * row + 1, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

                    graphics.DrawImage(emoji.Image, desRect, srcRect, GraphicsUnit.Pixel);
                }
            }
            this.pictureEmojiGroup0.Image = emojiGroupHistoryImage;
        }
    }
}
