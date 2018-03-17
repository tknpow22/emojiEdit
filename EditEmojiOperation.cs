using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace emojiEdit
{
    //
    // 絵文字編集操作
    //
    class EditEmojiOperation
    {
        //
        // 変数
        //
        
        // 親ウィンドウ
        private IWin32Window ownerWindow;

        // 絵文字イメージを保持するパネルコントロール
        private Panel panelContents;

        // 絵文字イメージ
        private PictureBox pictureContents;

        // 絵文字イメージに対応する文字コードデータ(JIS)を保持する
        private List<int> contents;

        // コンストラクタ
        public EditEmojiOperation(
            IWin32Window ownerWindow,
            Panel panelContents,
            PictureBox pictureContents,
            int maxCols,
            int maxRows)
        {
            this.ownerWindow = ownerWindow;
            this.panelContents = panelContents;
            this.pictureContents = pictureContents;
            this.MaxCols = maxCols;
            this.MaxRows = maxRows;
        }

        //
        // プロパティ
        //

        // 絵文字イメージに対応する文字コードデータ
        public List<int> Contents
        {
            get {
                return this.contents;
            }
        }

        // 1行の文字数
        public int MaxCols
        {
            get; set;
        }

        // 行数
        public int MaxRows
        {
            get; set;
        }

        // 全体の文字数
        public int MaxChars
        {
            get {
                return this.MaxCols * this.MaxRows;
            }
        }

        //
        // 処理
        //

        // コードのデータから絵文字を設定する(主に件名用)
        public void LoadFromCodes(List<int> codeList, int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            List<int> contentsNew = this.GetInitialContents();
            this.InsertCodesToContents(codeList.ToArray(), col, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
            this.ScrollTop();
        }

        // ボディ部のデータから絵文字を設定する
        public void LoadFromBodyData(byte[] bodyData, int offset, int count)
        {
            List<int> contentsNew = this.GetInitialContents();
            this.SetContentsFromBodyDataWithDraw(bodyData, offset, count, contentsNew);

            this.ScrollTop();
        }

        // テキストを設定する
        public void InsertText(string text, int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            this.InsertTextDataToContentsWithDraw(text, col, row);
        }

        // コード一覧を挿入する
        public void InsertCodes(List<int> codeList, int srcCols, int srcRows, int col, int row)
        {
            // NOTE: col は未使用
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            List<int> codeListNew = new List<int>(new int[this.MaxCols * srcRows]);

            for (int crow = 0; crow < srcRows && crow < this.MaxRows; ++crow) {
                for (int ccol = 0; ccol < srcCols && ccol < this.MaxCols; ++ccol) {
                    int srcIndex = srcCols * crow + ccol;
                    int destIndex = this.MaxCols * crow + ccol;
                    codeListNew[destIndex] = codeList[srcIndex];
                }
            }

            // FIXME: 全部作り直してるので遅いかも
            List<int> contentsNew = this.CopyContents(this.contents);
            this.InsertCodesToContents(codeListNew.ToArray(), 0, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // テスト用に絵文字を設定する
        public void SetTestData()
        {
            List<int> contentsNew = this.GetInitialContents();
            this.SetContentsFromTestDataWithDraw(contentsNew);

            this.ScrollTop();
        }

        // 絵文字を設定する
        public void CallSelectEmoji(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            try {
                SelectEmojiForm dialog = new SelectEmojiForm();
                SelectEmojiFormResult dr = dialog.ShowDialog(this.ownerWindow);
                if (dr == SelectEmojiFormResult.Cancel) {
                    return;
                }
                
                if (dr == SelectEmojiFormResult.SetEmoji) {
                    this.SetCodeToContents(dialog.Code, col, row, this.contents);
                    this.DrawEmojiImageToPictureImage(dialog.EmojiImage, col, row);
                } else if (dr == SelectEmojiFormResult.Clear) {
                    this.SetCodeToContents(0, col, row, this.contents);
                    this.DrawEmojiImageToPictureImage(DataBags.Emojis.EmptyEmoji, col, row);
                }

            } catch (Exception ex) {
                MsgBox.Show(this.ownerWindow, ex.Message, "絵文字選択に失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 文字列行を挿入する
        public void CallEditText(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            try {
                EditTextForm dialog = new EditTextForm();
                EditTextFormResult dr = dialog.ShowDialog(this.ownerWindow);
                if (dr == EditTextFormResult.Cancel) {
                    return;
                }

                if (dr == EditTextFormResult.SetText) {
                    string text = dialog.InputText;
                    this.InsertTextDataToContentsWithDraw(text, col, row);
                }

            } catch (Exception ex) {
                MsgBox.Show(this.ownerWindow, ex.Message, "文字列編集に失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 文字列を挿入する
        public void CallEditOnelineText(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            try {
                EditOnelineTextForm dialog = new EditOnelineTextForm();
                EditOnelineTextFormResult dr = dialog.ShowDialog(this.ownerWindow);
                if (dr == EditOnelineTextFormResult.Cancel) {
                    return;
                }

                if (dr == EditOnelineTextFormResult.SetText) {
                    string text = dialog.InputText.Trim();
                    this.InsertTextDataToContentsWithDraw(text, col, row);
                }

            } catch (Exception ex) {
                MsgBox.Show(this.ownerWindow, ex.Message, "文字列編集に失敗しました", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 空行を挿入する
        public void InsertEmptyLine(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            // FIXME: 全部作り直してるので遅いかも
            List<int> contentsNew = this.CopyContents(this.contents);
            this.InsertCodesToContents(new int[this.MaxCols], 0, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 行を削除する
        public void RemoveLine(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            // FIXME: 全部作り直してるので遅いかも
            List<int> contentsNew = this.CopyContents(this.contents);
            this.RemoveCodesFromContents(0, row, this.MaxCols, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 1文字挿入する
        public void InsertChar(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            List<int> contentsNew = this.CopyContents(this.contents);
            this.InsertCodesToContents(new int[1], col, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 1文字削除する
        public void RemoveChar(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            // FIXME: 全部作り直してるので遅いかも
            List<int> contentsNew = this.CopyContents(this.contents);
            this.RemoveCodesFromContents(col, row, 1, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 改行
        public void NewLine(int col, int row)
        {
            if (0 <= col && col < this.MaxCols) {
                // OK
            } else {
                return;
            }

            if (0 <= row && row < this.MaxRows) {
                // OK
            } else {
                return;
            }

            // FIXME: 全部作り直してるので遅いかも
            List<int> contentsNew = this.CopyContents(this.contents);
            this.InsertCodesToContents(new int[this.MaxCols - col], col, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 絵文字イメージ、文字コードデータをクリアする
        public void Clear()
        {
            this.contents = this.GetInitialContents();
            this.pictureContents.Image = this.GetInitialPictureImage();
            this.ScrollTop();
        }

        // 先頭へスクロールする
        public void ScrollTop()
        {
            this.panelContents.AutoScrollPosition = new Point(0, 0);
        }

        //
        // 内部処理 - 文字コードデータ
        //

        // 新しい文字コードデータを取得する
        private List<int> GetInitialContents()
        {
            return new List<int>(new int[this.MaxChars]);
        }

        // ボディ部のデータから文字コードデータを設定する
        private void SetContentsFromBodyDataWithDraw(byte[] bodyData, int offset, int count, List<int> contentsNew)
        {
            int col = 0;
            int row = 0;

            bool inJpn = false;

            int dataLength = offset + count;

            for (int i = offset; i < dataLength;) {

                if (JisUtils.IsCrLf(bodyData, dataLength, i)) {
                    inJpn = false;  // 念のため
                    i += 2;
                    col = 0;
                    ++row;
                    continue;
                }
                if (JisUtils.IsBeginEscapeSequence(bodyData, dataLength, i)) {
                    inJpn = true;
                    i += 3;
                    continue;
                }
                if (JisUtils.IsEndEscapeSequence(bodyData, dataLength, i)) {
                    inJpn = false;
                    i += 3;
                    continue;
                }
                if (inJpn && i <= dataLength - 2) {
                    // 表示
                    if (col < DataBags.Config.BodyMaxCols && row < DataBags.Config.BodyMaxRows) {

                        int code = JisUtils.MergeHL(bodyData[i], bodyData[i + 1]);

                        int index = DataBags.Config.BodyMaxCols * row + col;
                        contentsNew[index] = code;
                    }

                    i += 2;
                    ++col;
                    continue;
                }
                throw new Exception("形式が違います: 日本語以外（半角英数等）には対応していません。");
            }
            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 入力されたテキストから指定位置に文字コードデータを挿入する
        private void InsertTextDataToContentsWithDraw(string text, int col, int row)
        {
            string textData = StringUtils.RemoveControlChars(text);
            string zenkakuTextData = StringUtils.ToZenkaku(textData);
            int count = this.SetTextDataToContents(zenkakuTextData, col, row, null);

            if (0 <= zenkakuTextData.IndexOf('\n')) {
                count = (int)Math.Ceiling((decimal)count / this.MaxCols) * this.MaxCols;
            }

            List<int> contentsNew = this.CopyContents(this.contents);
            this.InsertCodesToContents(new int[count], col, row, contentsNew);

            this.SetTextDataToContents(zenkakuTextData, col, row, contentsNew);

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 指定位置に文字コードデータを設定する
        private int SetTextDataToContents(string zenkakuTextData, int col, int row, List<int> ccontents)
        {
            int count = 0;

            int ccol = col;
            int crow = row;
            for (int srcIndex = 0; srcIndex < zenkakuTextData.Length; ++srcIndex) {
                string sch = zenkakuTextData.Substring(srcIndex, 1);
                if (sch == "\n") {
                    count += (this.MaxCols - ccol);
                    ccol = 0;
                    ++crow;
                    continue;
                }
                if (this.MaxCols <= ccol) {
                    ccol = 0;
                    ++crow;
                }
                if (this.MaxRows <= crow) {
                    break;
                }

                int destIndex = this.MaxCols * crow + ccol;
                if (this.MaxChars <= destIndex) {
                    break;
                }

                if (ccontents != null) {
                    int code = JisUtils.GetCode(sch);
                    ccontents[destIndex] = code;
                }
                ++count;
                ++ccol;
            }

            return count;
        }
        
        // テスト用に文字コードデータを設定する
        private void SetContentsFromTestDataWithDraw(List<int> contentsNew)
        {
            // NOTE: テスト用なので、行数および文字数のチェックをしていない。注意すること
            int testMaxCols = 9;
            System.Diagnostics.Debug.Assert(testMaxCols <= this.MaxCols);

            // 行数カウンタ
            int rowsCount = 0;

            // 絵文字グループ番号
            for (int emojiGroupNo = 1; emojiGroupNo <= DataBags.Emojis.NumIconInGroupList.Length; ++emojiGroupNo) {
                int numEmojiInGroup = DataBags.Emojis.NumIconInGroupList[emojiGroupNo - 1];

                // 絵文字ID: グループ内でのID
                for (int emojiId = 0; emojiId < numEmojiInGroup; ++emojiId) {

                    Emoji emoji = DataBags.Emojis.Get(emojiGroupNo, emojiId);
                    if (emoji == null) {
                        continue;
                    }

                    int col = emojiId % testMaxCols;
                    int row = rowsCount + (emojiId / testMaxCols);
                    int index = this.MaxCols * row + col;

                    contentsNew[index] = emoji.Code;
                }

                int rows = (int)Math.Ceiling((decimal)numEmojiInGroup / testMaxCols);
                rowsCount += rows;
            }

            this.DrawContentsToNewPictureImage(contentsNew);
        }

        // 指定位置に文字コードデータを設定する
        private void SetCodeToContents(int code, int col, int row, List<int> ccontents)
        {
            int index = this.MaxCols * row + col;
            ccontents[index] = code;
        }

        // 指定位置に文字コードデータを挿入する
        private void InsertCodesToContents(int[] codeList, int col, int row, List<int> ccontents)
        {
            int index = this.MaxCols * row + col;
            ccontents.InsertRange(index, codeList);
            ccontents.RemoveRange(this.MaxChars, ccontents.Count - this.MaxChars);
        }

        // 指定位置から文字コードデータを削除する
        private void RemoveCodesFromContents(int col, int row, int count, List<int> ccontents)
        {
            int index = this.MaxCols * row + col;
            ccontents.RemoveRange(index, count);
            ccontents.AddRange(new int[count]);
        }

        // 文字コードデータをコピーする
        private List<int> CopyContents(List<int> contentsOld)
        {
            List<int> contentsNew = new List<int>();
            contentsNew.AddRange(contentsOld);
            return contentsNew;
        }

        //
        // 内部処理 - イメージ
        //

        // 新しいピクチャーイメージを取得する
        private Image GetInitialPictureImage()
        {
            int maxWidth = Commons.FRAME_WIDTH * this.MaxCols;
            int maxHeight = Commons.FRAME_HEIGHT * this.MaxRows;

            Image imageContents = new Bitmap(maxWidth + 1, maxHeight + 1);

            using (Graphics graphicsContents = Graphics.FromImage(imageContents)) {
                graphicsContents.FillRectangle(Brushes.White, 0, 0, maxWidth + 1, maxHeight + 1);
                this.DrawFramesToPictureImage(graphicsContents);
            }

            return imageContents;
        }

        // ピクチャーイメージに枠線を描画する
        private void DrawFramesToPictureImage(Graphics graphicsContents)
        {
            for (int row = 0; row < this.MaxRows; ++row) {
                int y = Commons.FRAME_HEIGHT * row;
                for (int col = 0; col < this.MaxCols; ++col) {
                    int x = Commons.FRAME_WIDTH * col;
                    Rectangle rectFrame = new Rectangle(x, y, Commons.FRAME_WIDTH, Commons.FRAME_HEIGHT);
                    graphicsContents.DrawRectangle(Pens.Black, rectFrame);
                }
            }
        }

        // 文字コードデータを新しいピクチャーイメージに描画する
        private void DrawContentsToNewPictureImage(List<int> contentsNew)
        {
            Image imagePictureNew = this.GetInitialPictureImage();

            using (Graphics graphics = Graphics.FromImage(imagePictureNew)) {
                DrawUtils.DrawCodes(this.MaxCols, this.MaxRows, contentsNew, graphics);
            }

            this.contents = contentsNew;
            this.pictureContents.Image = imagePictureNew;
        }

        // 絵文字イメージをピクチャーイメージに描画する
        private void DrawEmojiImageToPictureImage(Image imgChar, int col, int row)
        {
            Image imageContents = this.pictureContents.Image;

            using (Graphics graphicsContents = Graphics.FromImage(imageContents)) {
                DrawUtils.DrawImage(imgChar, col, row, graphicsContents);
            }

            this.pictureContents.Image = imageContents;
        }
    }
}
