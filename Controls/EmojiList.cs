namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// 絵文字一覧
    /// </summary>
    partial class EmojiList : UserControl
    {
        /// <summary>
        /// 履歴に表示する絵文字アイコンの最大数
        /// </summary>
        private int maxEmojiHistoryCount;

        /// <summary>
        /// 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<List<int>> emojiGroupJiscodeMaps;

        /// <summary>
        /// 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
        /// </summary>
        private List<Image> emojiGroupImages;

        /// <summary>
        /// 絵文字アイコンイメージを保持するピクチャーボックスを保持する(インデックス 0 は履歴用で未使用)
        /// </summary>
        private List<PictureBox> emojiPictureBoxes;

        /// <summary>
        /// 絵文字アイコンの現在の選択インデックス位置を保持する(インデックス 0 は履歴用で未使用)
        /// </summary>
        private List<int> currentEmojiIds;

        /// <summary>
        /// 履歴の行数
        /// </summary>
        private int emojiHistoryRows;

        #region 履歴一覧イメージのサイズ

        /// <summary>
        /// 履歴一覧イメージのサイズ(幅)
        /// </summary>
        private int emojiHistoryMaxWidth;

        /// <summary>
        /// 履歴一覧イメージのサイズ(高さ)
        /// </summary>
        private int emojiHistoryMaxHeight;

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmojiList()
        {
            InitializeComponent();

            this.pictureEmojiGroup0.MouseClick += new MouseEventHandler(this.pictureEmojiGroupX_MouseClick);
            this.tabControlEmojiList.MouseClick += new MouseEventHandler(this.tabControlEmojiList_MouseClick);
        }

        /// <summary>
        ///  絵文字一覧の最初のページの最初の絵文字にスクロールする
        /// </summary>
        public void ShowFirstGroupFirstEmoji()
        {
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                int emojiId = this.currentEmojiIds[emojiGroupNo];
                if (0 <= emojiId) {
                    this.tabControlEmojiList.SelectedIndex = emojiGroupNo - 1;
                    this.ScrollToCurrentEmoji(emojiGroupNo, emojiId);
                    break;
                }
            }
        }

        /// <summary>
        /// 絵文字一覧を初期設定する
        /// </summary>
        public void InitializeEmojiList()
        {
            // 絵文字一覧に関連する変数の初期化
            this.InitializeEmojiListValues();

            // 絵文字一覧に絵文字アイコンと対応する文字コードをロードする
            this.LoadEmojiList();

            // 絵文字一覧に関連するコントロールを設定する
            this.SetupEmojiListControls();

            // 絵文字一覧の履歴を描画する
            this.RedrawEmojiListHistory();
        }

        /// <summary>
        /// テストデータを設定する
        /// </summary>
        /// <param name="emojiTextBox">設定先の絵文字対応のテキストボックス</param>
        public void SetTestEmoji(EmojiTextBox emojiTextBox)
        {
            emojiTextBox.SuspendLayout();

            emojiTextBox.Clear();

            // 行数カウンタ
            int rowsCount = 0;

            // 絵文字グループ番号
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                // 絵文字ID: グループ内でのID
                for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                    (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);
                    row = rowsCount + row;
                    if (col == 0 && row != 0) {
                        emojiTextBox.Text += "\r\n";
                    }

                    Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                    if (emoji == null) {
                        emojiTextBox.Text += "\u3000";
                    } else {
                        emojiTextBox.Text += emoji.Unicode;
                    }
                }

                int rows = this.GetGroupRows(numEmojiInGroup);
                rowsCount += rows;
            }

            emojiTextBox.ResumeLayout();
            emojiTextBox.Focus();
            emojiTextBox.SelectionLength = 0;
        }

        /// <summary>
        /// 現在表示されているタブの選択されている絵文字を得る
        /// </summary>
        /// <returns>Emoji、選択がない場合は null</returns>
        public Emoji GetCurrentEmoji()
        {
            if (this.tabControlEmojiList.TabCount == 0) {
                return null;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            int emojiGroupNo = tabIndexCurrent + 1;

            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return null;
            }

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];

            int jiscode = emojiGroupJiscodeMap[emojiIdCurrent];
            if (jiscode == 0) {
                return null;
            }

            Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
            return emoji;
        }

        /// <summary>
        ///  絵文字履歴を登録する
        /// </summary>
        /// <param name="jiscode">登録する JIS コード</param>
        public void RegistEmojiHistory(int jiscode)
        {
            List<int> emojiGroupJiscodeMapHistory = this.emojiGroupJiscodeMaps[0];
            if (!emojiGroupJiscodeMapHistory.Contains(jiscode)) {
                emojiGroupJiscodeMapHistory.Insert(0, jiscode);
                emojiGroupJiscodeMapHistory.RemoveRange(this.maxEmojiHistoryCount, 1);
            } else {
                emojiGroupJiscodeMapHistory.Remove(jiscode);
                emojiGroupJiscodeMapHistory.Insert(0, jiscode);
            }
            this.RedrawEmojiListHistory();

            DataBags.Config.SetEmojiHistory(emojiGroupJiscodeMapHistory);
        }

        /// <summary>
        /// 絵文字一覧の現在表示しているタブ上の絵文字選択を変更する
        /// </summary>
        /// <param name="direction">-1の場合は左, +1の場合は右へ移動する</param>
        public void ChangeEmojiSelectionOnCurrentGroup(int direction)
        {
            System.Diagnostics.Debug.Assert(direction == -1 || direction == 1);

            if (this.tabControlEmojiList.TabCount == 0) {
                return;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            int emojiGroupNo = tabIndexCurrent + 1;

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];
            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return;
            }

            for (int countLimit = 0; countLimit < emojiGroupJiscodeMap.Count; ++countLimit) {
                emojiIdCurrent += emojiGroupJiscodeMap.Count + direction;
                int emojiIdNew = emojiIdCurrent % emojiGroupJiscodeMap.Count;
                int jiscode = emojiGroupJiscodeMap[emojiIdNew];
                if (jiscode != 0) {
                    emojiIdCurrent = emojiIdNew;
                    break;
                }
            }

            this.ChangeEmojiSelection(emojiGroupNo, emojiIdCurrent);
        }

        /// <summary>
        /// 絵文字一覧の表示しているタブを変更する
        /// </summary>
        /// <param name="direction">-1の場合は左, +1の場合は右へ移動する</param>
        public void ChangeEmojiGroupSelection(int direction)
        {
            System.Diagnostics.Debug.Assert(direction == -1 || direction == 1);

            if (this.tabControlEmojiList.TabCount == 0) {
                return;
            }

            int tabIndexCurrent = this.tabControlEmojiList.SelectedIndex;
            tabIndexCurrent += this.tabControlEmojiList.TabCount + direction;
            int tabIndexNew = tabIndexCurrent % this.tabControlEmojiList.TabCount;

            this.tabControlEmojiList.SelectedIndex = tabIndexNew;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 処理後にフォーカスを戻す絵文字対応のテキストボックス
        /// </summary>
        public EmojiTextBox PlaybackFocusTextBox
        {
            get; set;
        }

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// 絵文字一覧タブコントロール - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControlEmojiList_MouseClick(object sender, MouseEventArgs e)
        {
            this.PlaybackFocus();
        }

        /// <summary>
        /// 絵文字一覧タブページコントロール - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabEmojiGroupX_MouseClick(object sender, MouseEventArgs e)
        {
            this.PlaybackFocus();
        }

        /// <summary>
        /// 絵文字アイコン - MouseClick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                numEmojiInGroup = this.maxEmojiHistoryCount;
            } else {
                numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];
            }

            int rows = this.GetGroupRows(numEmojiInGroup);

            int col = e.X / Commons.FRAME_WIDTH;
            int row = e.Y / Commons.FRAME_HEIGHT;

            if (0 <= col && col < DataBags.Config.MaxEmojiListCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < rows) {
                // OK
            } else {
                return;
            }

            List<int> emojiGroupJiscodeMap = this.emojiGroupJiscodeMaps[emojiGroupNo];

            int emojiId = this.GetEmojiIdFromColAndRow(col, row);

            if (emojiId < emojiGroupJiscodeMap.Count) {
                // OK
            } else {
                return;
            }

            int jiscode = emojiGroupJiscodeMap[emojiId];
            Emoji emoji = null;
            if (jiscode != 0) {
                emoji = DataBags.Emojis.GetFromJiscode(jiscode);
            }
            if (emoji != null) {

                // タブ上の絵文字の選択表示を変更する
                this.ChangeEmojiSelection(emojiGroupNo, emojiId);

                if (this.PlaybackFocusTextBox != null) {
                    EmojiTextBox emojiTextBox = this.PlaybackFocusTextBox;
                    emojiTextBox.Focus();
                    emojiTextBox.SelectedText = new string(emoji.Unicode, 1);
                    emojiTextBox.SelectionLength = 0;
                }

                // 履歴へ登録する
                this.RegistEmojiHistory(jiscode);
            }

            this.PlaybackFocus();
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 絵文字一覧に関連する変数の初期化
        /// </summary>
        private void InitializeEmojiListValues()
        {
            // 履歴に表示する絵文字アイコンの最大数
            this.maxEmojiHistoryCount = DataBags.Config.MaxEmojiListCols * 2; // NOTE: MAX_COLS の倍数とすること

            // 絵文字アイコンの文字コード(JIS)をグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupJiscodeMaps = new List<List<int>>();

            // 絵文字アイコンをグループ毎に管理する(インデックス 0 は履歴用)
            this.emojiGroupImages = new List<Image>();

            // 絵文字アイコンイメージを保持するピクチャーボックスを保持する(インデックス 0 は履歴用で未使用)
            this.emojiPictureBoxes = new List<PictureBox>();

            // 現在選択されている絵文字IDをグループごとに管理する(インデックス 0 は履歴用で未使用: ただし実装の都合上、履歴用にも値が設定されるが使っていない)
            this.currentEmojiIds = new List<int>();

            // 履歴の行数
            this.emojiHistoryRows = (int)Math.Ceiling((decimal)this.maxEmojiHistoryCount / DataBags.Config.MaxEmojiListCols);

            // 履歴一覧イメージのサイズ
            this.emojiHistoryMaxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
            this.emojiHistoryMaxHeight = Commons.FRAME_HEIGHT * this.emojiHistoryRows;
        }

        /// <summary>
        /// 絵文字一覧に絵文字アイコンと対応する文字コードをロードする
        /// </summary>
        private void LoadEmojiList()
        {
            //
            // 絵文字アイコン(履歴)
            //
            {
                List<int> emojiGroupJiscodeMapHistory = DataBags.Config.GetEmojiHistory();
                int emojiHistoryCount = emojiGroupJiscodeMapHistory.Count;

                emojiGroupJiscodeMapHistory.AddRange(new int[this.maxEmojiHistoryCount]);
                emojiGroupJiscodeMapHistory.RemoveRange(this.maxEmojiHistoryCount, emojiHistoryCount);

                this.emojiGroupJiscodeMaps.Add(emojiGroupJiscodeMapHistory);
            }
            {
                Image emojiGroupHistoryImage = new Bitmap(this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {
                    graphics.FillRectangle(Brushes.White, 0, 0, this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);
                }

                this.emojiGroupImages.Add(emojiGroupHistoryImage);
            }
            this.currentEmojiIds.Add(-1);   // インデックス 0 は履歴用で未使用

            //
            // 絵文字アイコン
            //

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                int rows = this.GetGroupRows(numEmojiInGroup);

                int maxWidth = Commons.FRAME_WIDTH * DataBags.Config.MaxEmojiListCols;
                int maxHeight = Commons.FRAME_HEIGHT * rows;

                List<int> emojiGroupJiscodeMap = new List<int>();
                emojiGroupJiscodeMap.AddRange(new int[numEmojiInGroup]);   // 個数分作成する。初期値は 0

                Image emojiGroupImage = new Bitmap(maxWidth, maxHeight);

                using (Graphics graphics = Graphics.FromImage(emojiGroupImage)) {

                    graphics.FillRectangle(Brushes.White, 0, 0, maxWidth, maxHeight);

                    // 絵文字ID: グループ内でのID
                    int firstEmojiId = -1;  // 有効な最初の絵文字ID
                    for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                        Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                        if (emoji == null) {
                            continue;
                        }
                        emojiGroupJiscodeMap[emojiId] = emoji.Jiscode;

                        (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

                        DrawUtils.DrawImage(emoji.Image, col, row, graphics);

                        if (firstEmojiId == -1) {
                            firstEmojiId = emojiId;
                        }
                    }
                    this.currentEmojiIds.Add(firstEmojiId);
                }

                this.emojiGroupImages.Add(emojiGroupImage);
                this.emojiGroupJiscodeMaps.Add(emojiGroupJiscodeMap);
            }
        }

        /// <summary>
        /// 絵文字一覧に関連するコントロールを設定する
        /// </summary>
        private void SetupEmojiListControls()
        {
            this.tabControlEmojiList.SuspendLayout();

            this.tabControlEmojiList.Controls.Clear();

            this.emojiPictureBoxes.Add(null);   // インデックス 0 は履歴用で未使用

            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {

                PictureBox pictureEmojiGroup = new PictureBox();
                pictureEmojiGroup.Location = new Point(0, 0);
                pictureEmojiGroup.Name = string.Format("pictureEmojiGroup{0}", emojiGroupNo);
                pictureEmojiGroup.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureEmojiGroup.TabIndex = emojiGroupNo;
                pictureEmojiGroup.TabStop = false;
                pictureEmojiGroup.Tag = string.Format("{0}", emojiGroupNo);
                pictureEmojiGroup.MouseClick += new MouseEventHandler(this.pictureEmojiGroupX_MouseClick);

                pictureEmojiGroup.Image = this.emojiGroupImages[emojiGroupNo];

                TabPage tabEmojiGroup = new TabPage();
                tabEmojiGroup.AutoScroll = true;
                tabEmojiGroup.Controls.Add(pictureEmojiGroup);
                tabEmojiGroup.Name = string.Format("tabEmojiGroup{0}", emojiGroupNo);
                tabEmojiGroup.TabIndex = emojiGroupNo;
                tabEmojiGroup.Text = DataBags.Emojis.CaptionList[emojiGroupNo - 1];
                tabEmojiGroup.UseVisualStyleBackColor = true;
                tabEmojiGroup.MouseClick += new MouseEventHandler(this.tabEmojiGroupX_MouseClick);

                this.tabControlEmojiList.Controls.Add(tabEmojiGroup);
                this.emojiPictureBoxes.Add(pictureEmojiGroup);
            }

            this.tabControlEmojiList.ResumeLayout();

            // 絵文字一覧のタブ用のイメージに現在の選択マークを表示する
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                this.DrawSelectedEmojiFrame(true, emojiGroupNo);
            }
        }

        /// <summary>
        /// 絵文字一覧の履歴を描画する
        /// </summary>
        private void RedrawEmojiListHistory()
        {
            List<int> emojiGroupJiscodeMapHistory = this.emojiGroupJiscodeMaps[0];
            Image emojiGroupHistoryImage = this.emojiGroupImages[0];

            using (Graphics graphics = Graphics.FromImage(emojiGroupHistoryImage)) {

                graphics.FillRectangle(Brushes.White, 0, 0, this.emojiHistoryMaxWidth, this.emojiHistoryMaxHeight);

                for (int emojiId = 0; emojiId < emojiGroupJiscodeMapHistory.Count; ++emojiId) {

                    int jiscode = emojiGroupJiscodeMapHistory[emojiId];
                    Emoji emoji = DataBags.Emojis.GetFromJiscode(jiscode);
                    if (emoji == null) {
                        continue;
                    }

                    (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

                    DrawUtils.DrawImage(emoji.Image, col, row, graphics);
                }
            }
            this.pictureEmojiGroup0.Image = emojiGroupHistoryImage;
        }

        /// <summary>
        /// タブ上の絵文字の選択表示を変更する
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">絵文字ID</param>
        private void ChangeEmojiSelection(int emojiGroupNo, int emojiId)
        {
            if (emojiGroupNo <= 0) {
                return;
            }
            this.DrawSelectedEmojiFrame(false, emojiGroupNo);
            this.currentEmojiIds[emojiGroupNo] = emojiId;
            this.DrawSelectedEmojiFrame(true, emojiGroupNo);
            this.RerawEmojiGroup(emojiGroupNo);
            this.ScrollToCurrentEmoji(emojiGroupNo, emojiId);
        }

        /// <summary>
        /// 絵文字一覧のタブ用のイメージに現在の選択マークを表示/非表示する
        /// </summary>
        /// <param name="selected">選択する場合 true、解除の場合 false</param>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        private void DrawSelectedEmojiFrame(bool selected, int emojiGroupNo)
        {
            int emojiIdCurrent = this.currentEmojiIds[emojiGroupNo];
            if (emojiIdCurrent < 0) {
                return;
            }

            (int col, int row) = this.GetColAndRowFromEmojiId(emojiIdCurrent);

            Image image = this.emojiGroupImages[emojiGroupNo];

            using (Graphics graphics = Graphics.FromImage(image)) {
                DrawUtils.DrawFrame(selected, col, row, graphics);
            }
        }

        /// <summary>
        /// 絵文字一覧のタブページを再描画する
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        private void RerawEmojiGroup(int emojiGroupNo)
        {
            PictureBox pictureBox = this.emojiPictureBoxes[emojiGroupNo];
            if (pictureBox != null) {
                pictureBox.Invalidate();
            }
        }

        /// <summary>
        /// 絵文字一覧のタブページの現在の絵文字にスクロールする
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">絵文字ID</param>
        private void ScrollToCurrentEmoji(int emojiGroupNo, int emojiId)
        {
            (int col, int row) = this.GetColAndRowFromEmojiId(emojiId);

            int x = Commons.FRAME_WIDTH * col;
            int y = Commons.FRAME_HEIGHT * row;

            TabPage tabPage = this.tabControlEmojiList.TabPages[emojiGroupNo - 1];
            tabPage.AutoScrollPosition = new Point(x, y);
        }

        /// <summary>
        /// 絵文字グループをタブに表示する際の行数を取得する
        /// </summary>
        /// <param name="numEmojiInGroup">グループ内の絵文字の数</param>
        /// <returns>行数</returns>
        private int GetGroupRows(int numEmojiInGroup)
        {
            int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / DataBags.Config.MaxEmojiListCols);
            return rows;
        }

        /// <summary>
        /// 絵文字ID から表示する列と行を得る
        /// </summary>
        /// <param name="emojiId">絵文字ID</param>
        /// <returns>(列,行)</returns>
        private (int, int) GetColAndRowFromEmojiId(int emojiId)
        {
            int col = emojiId % DataBags.Config.MaxEmojiListCols;
            int row = emojiId / DataBags.Config.MaxEmojiListCols;

            return (col, row);
        }

        /// <summary>
        /// 列と行から絵文字IDを得る
        /// </summary>
        /// <param name="col">列</param>
        /// <param name="row">行</param>
        /// <returns>絵文字ID</returns>
        private int GetEmojiIdFromColAndRow(int col, int row)
        {
            int emojiId = DataBags.Config.MaxEmojiListCols * row + col;
            return emojiId;
        }

        /// <summary>
        /// フォーカスを戻す
        /// </summary>
        private void PlaybackFocus()
        {
            if (this.PlaybackFocusTextBox != null) {
                this.PlaybackFocusTextBox.Focus();
            }
        }

        #endregion
    }
}
