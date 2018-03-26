namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// フォームからの返却値
    /// </summary>
    public enum MailAddressFormResult
    {
        SetAddr,
        Cancel,
    }

    /// <summary>
    /// メールアドレス
    /// </summary>
    public partial class MailAddressForm : Form
    {
        #region 変数

        #region ソートの状態

        /// <summary>
        /// ソート対象の項目
        /// </summary>
        private int currentSortColumn = 0;

        /// <summary>
        /// ソートの状態
        /// </summary>
        private SortOrder currentSortOrder = SortOrder.Ascending;

        #endregion

        /// <summary>
        /// フォームからの返却値
        /// </summary>
        private MailAddressFormResult formResult = MailAddressFormResult.Cancel;

        #endregion

        /// <summary>
        /// メールアドレス一覧ソート用のクラス
        /// </summary>
        class ListViewMailAddressComparer : System.Collections.IComparer
        {
            /// <summary>
            /// 項目
            /// </summary>
            private int column;

            /// <summary>
            /// ソートオーダー
            /// </summary>
            private SortOrder sortOrder;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="column">項目</param>
            /// <param name="sortOrder">ソートオーダー</param>
            public ListViewMailAddressComparer(int column, SortOrder sortOrder)
            {
                this.column = column;
                this.sortOrder = sortOrder;
            }

            /// <summary>
            /// 比較関数
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(object x, object y)
            {
                ListViewItem itemx = (ListViewItem)x;
                ListViewItem itemy = (ListViewItem)y;

                string textx = itemx.SubItems[this.column].Text;
                string texty = itemy.SubItems[this.column].Text;

                if (this.sortOrder == SortOrder.Ascending) {
                    return string.Compare(textx, texty);
                } else {
                    return -string.Compare(textx, texty);
                }
            }
        }

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MailAddressForm()
        {
            InitializeComponent();

            List<MailAddress> addrList = DataBags.MailAddresses.Get();
            foreach (MailAddress mailAddress in addrList) {
                this.AddItem(mailAddress);
            }

            this.listViewMailAddress.ListViewItemSorter = new ListViewMailAddressComparer(this.currentSortColumn, this.currentSortOrder);
            this.buttonSelectFromList.Enabled = false;
            this.buttonUpsert.Enabled = false;
            this.buttonDelete.Enabled = false;
        }

        /// <summary>
        /// このダイアログを表示する
        /// </summary>
        /// <param name="owner">親ウィンドウ</param>
        /// <returns>フォームからの返却値</returns>
        public new MailAddressFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        #endregion

        #region プロパティ

        #region SetAddr

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string MailAddr
        {
            private set;
            get;
        }

        #endregion

        #endregion

        #region イベントハンドラ

        /// <summary>
        /// Form - Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MailAddressForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.textBoxMailAddr;
        }

        /// <summary>
        /// Form - FormClosed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MailAddressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveAddr();
        }

        /// <summary>
        /// Form - KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MailAddressForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape) {
            //    this.Cancel();
            //}
        }

        /// <summary>
        /// 一覧 - SelectedIndexChanged(選択変更)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMailAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
            if (items.Count == 0) {
                this.buttonSelectFromList.Enabled = false;
                this.buttonDelete.Enabled = false;
            } else {
                this.buttonSelectFromList.Enabled = true;
                this.buttonDelete.Enabled = true;
            }
        }

        /// <summary>
        /// 一覧 - KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMailAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {
                ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
                if (0 < items.Count) {
                    this.RemoveItem(items[0]);
                }
            }
        }

        /// <summary>
        /// 一覧 - ColumnClick(ヘッダカラムクリック)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMailAddress_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SetNextSortStatus(e.Column);
            this.listViewMailAddress.ListViewItemSorter = new ListViewMailAddressComparer(this.currentSortColumn, this.currentSortOrder);
            this.textBoxMailAddr.Focus();
        }

        /// <summary>
        /// 一覧 - MouseDoubleClick(マウスダブルクリック)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewMailAddress_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
            if (items.Count == 0) {
                return;
            }

            ListViewItem item = items[0];
            string mailAddr = item.Text;
            this.SetAddr(mailAddr);
        }

        /// <summary>
        /// メールアドレス - TextChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxMailAddr_TextChanged(object sender, EventArgs e)
        {
            string mailAddr = this.textBoxMailAddr.Text.Trim();
            if (mailAddr.Length == 0) {
                this.buttonUpsert.Enabled = false;
            } else {
                this.buttonUpsert.Enabled = true;
            }
        }

        /// <summary>
        /// 一覧から取得ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectFromList_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
            if (items.Count == 0) {
                return;
            }

            ListViewItem item = items[0];
            this.textBoxMailAddr.Text = item.Text;
            this.textBoxNote.Text = item.SubItems[1].Text;

            this.textBoxMailAddr.Focus();
        }

        /// <summary>
        /// 一覧へ設定ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonUpsert_Click(object sender, EventArgs e)
        {
            if (!this.CheckAddr()) {
                return;
            }

            string mailAddr = this.textBoxMailAddr.Text.Trim();
            string note = this.textBoxNote.Text.Trim();

            ListViewItem item = this.FindItem(mailAddr);
            if (item != null) {
                item.SubItems[1].Text = note;
            } else {
                item = this.AddItem(mailAddr, note);
            }
            item.Selected = true;
            this.textBoxMailAddr.Focus();
        }

        /// <summary>
        /// 削除ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
            if (items.Count == 0) {
                return;
            }

            ListViewItem item = items[0];
            this.RemoveItem(item);
        }

        /// <summary>
        /// 設定ボタン - Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSetAddr_Click(object sender, EventArgs e)
        {
            if (!this.CheckAddr()) {
                return;
            }

            string mailAddr = this.textBoxMailAddr.Text.Trim();
            this.SetAddr(mailAddr);
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
        /// 一覧に追加する
        /// </summary>
        /// <param name="mailAddr">メールアドレス</param>
        /// <param name="note">備考</param>
        /// <returns>追加した ListViewItem</returns>
        private ListViewItem AddItem(string mailAddr, string note)
        {
            ListViewItem item = new ListViewItem();

            item.Text = mailAddr.Trim();
            item.SubItems.Add(note.Trim());
            this.listViewMailAddress.Items.Add(item);

            return item;
        }

        /// <summary>
        /// 一覧に追加する
        /// </summary>
        /// <param name="mailAddress">MailAddress</param>
        /// <returns>追加した ListViewItem</returns>
        private ListViewItem AddItem(MailAddress mailAddress)
        {
            return this.AddItem(mailAddress.Address, mailAddress.Note);
        }

        /// <summary>
        /// 一覧から削除する
        /// </summary>
        /// <param name="item">削除する ListViewItem</param>
        private void RemoveItem(ListViewItem item)
        {
            this.listViewMailAddress.Items.Remove(item);
            this.textBoxMailAddr.Focus();
        }

        /// <summary>
        /// 一覧からメールアドレスを探す
        /// </summary>
        /// <param name="mailAddr">メールアドレス</param>
        /// <returns>見つかった ListViewItem: 存在しない場合は null</returns>
        private ListViewItem FindItem(string mailAddr)
        {
            if (0 < this.listViewMailAddress.Items.Count) {
                ListViewItem item = this.listViewMailAddress.FindItemWithText(mailAddr, false, 0, false);
                if (item != null /*&& item.Text == mailAddr*/) {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 入力されたメールアドレスを返却用に設定する
        /// </summary>
        /// <param name="mailAddr">メールアドレス</param>
        private void SetAddr(string mailAddr)
        {
            ListViewItem item = this.FindItem(mailAddr);
            if (item == null) {
                string note = this.textBoxNote.Text.Trim();
                this.AddItem(mailAddr, note);
            }

            this.MailAddr = mailAddr;

            this.formResult = MailAddressFormResult.SetAddr;
            this.Close();
        }

        /// <summary>
        /// キャンセル処理
        /// </summary>
        private void Cancel()
        {
            this.formResult = MailAddressFormResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// ソートの状態を設定する
        /// </summary>
        /// <param name="column">項目</param>
        private void SetNextSortStatus(int column)
        {
            if (column == this.currentSortColumn) {
                this.currentSortOrder = this.InvertSortOrder(this.currentSortOrder);
            } else {
                this.currentSortOrder = SortOrder.Ascending;
                this.currentSortColumn = column;
            }
        }

        /// <summary>
        /// ソート順を得る
        /// </summary>
        /// <param name="current">現在のソート順</param>
        /// <returns>新しいソート順</returns>
        private SortOrder InvertSortOrder(SortOrder current)
        {
            return (current == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
        }

        /// <summary>
        /// 入力されたメールアドレスをチェックする
        /// </summary>
        /// <returns>OK なら true</returns>
        private bool CheckAddr()
        {
            // チェック(必須)
            {
                if (this.textBoxMailAddr.Text.Trim().Length == 0) {
                    MsgBox.Show(this, string.Format("「{0}」は必ず入力してください。", this.textBoxMailAddr.Tag), "必須項目", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.textBoxMailAddr.Focus();
                    return false;
                }
            }

            // チェック(メール)
            {
                if (!Commons.IsMailAddr(this.textBoxMailAddr.Text.Trim(), false)) {
                    MsgBox.Show(this, string.Format("「{0}」を確認してください。\n・name@domain の形式のみ有効です。\n・複数入力はできません。", this.textBoxMailAddr.Tag), "メールアドレス", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.textBoxMailAddr.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// メールアドレスを保存する
        /// </summary>
        private void SaveAddr()
        {
            List<MailAddress> addrList = new List<MailAddress>();
            foreach (ListViewItem item in this.listViewMailAddress.Items) {
                string mailAddr = item.Text.Trim();
                string note = item.SubItems[1].Text.Trim();

                if (mailAddr.Length == 0) {
                    continue;
                }

                addrList.Add(new MailAddress(mailAddr, note));
            }

            DataBags.MailAddresses.Set(addrList);
        }

        #endregion
    }
}
