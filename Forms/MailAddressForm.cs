using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace emojiEdit
{
    // フォームからの返却値
    public enum MailAddressFormResult
    {
        SetAddr,
        Cancel,
    }

    //
    // メールアドレス
    //
    public partial class MailAddressForm : Form
    {
        // ソートの状態
        private int currentSortColumn = 0;
        private SortOrder currentSortOrder = SortOrder.Ascending;

        // フォームからの返却値
        private MailAddressFormResult formResult = MailAddressFormResult.Cancel;

        //
        // メールアドレス一覧ソート用
        //
        class ListViewMailAddressComparer : System.Collections.IComparer
        {
            private int column;
            private SortOrder sortOrder;

            public ListViewMailAddressComparer(int column, SortOrder sortOrder)
            {
                this.column = column;
                this.sortOrder = sortOrder;
            }

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

        // コンストラクタ
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

        // このダイアログを表示する
        public new MailAddressFormResult ShowDialog(IWin32Window owner)
        {
            base.ShowDialog(owner);

            return formResult;
        }

        //
        // プロパティ
        //

        // SetAddr

        public string MailAddr
        {
            private set;
            get;
        }

        //
        // イベントハンドラ
        //

        // フォームロード時
        private void MailAddressForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = this.textBoxMailAddr;
        }

        // 一覧選択
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

        // 一覧キーダウン
        private void listViewMailAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {
                ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
                if (0 < items.Count) {
                    this.RemoveItem(items[0]);
                }
            }
        }

        // 一覧ヘッダカラムクリック
        private void listViewMailAddress_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SetNextSortStatus(e.Column);
            this.listViewMailAddress.ListViewItemSorter = new ListViewMailAddressComparer(this.currentSortColumn, this.currentSortOrder);
            this.textBoxMailAddr.Focus();
        }

        // 一覧ダブルクリック
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

        // メールアドレスが変更された時
        private void textBoxMailAddr_TextChanged(object sender, EventArgs e)
        {
            string mailAddr = this.textBoxMailAddr.Text.Trim();
            if (mailAddr.Length == 0) {
                this.buttonUpsert.Enabled = false;
            } else {
                this.buttonUpsert.Enabled = true;
            }
        }

        // 一覧から取得
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

        // 追加・更新
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

        // 削除
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = this.listViewMailAddress.SelectedItems;
            if (items.Count == 0) {
                return;
            }

            ListViewItem item = items[0];
            this.RemoveItem(item);
        }

        // 設定
        private void buttonSetAddr_Click(object sender, EventArgs e)
        {
            if (!this.CheckAddr()) {
                return;
            }

            string mailAddr = this.textBoxMailAddr.Text.Trim();
            this.SetAddr(mailAddr);
        }

        // キャンセル
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Cancel();
        }

        // フォームでキー押下
        private void MailAddressForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape) {
            //    this.Cancel();
            //}
        }

        // フォームが閉じられた時
        private void MailAddressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SaveAddr();
        }

        //
        // 内部処理
        //

        // 一覧に追加する
        private ListViewItem AddItem(string mailAddr, string note)
        {
            ListViewItem item = new ListViewItem();

            item.Text = mailAddr.Trim();
            item.SubItems.Add(note.Trim());
            this.listViewMailAddress.Items.Add(item);

            return item;
        }

        // 一覧に追加する
        private ListViewItem AddItem(MailAddress mailAddress)
        {
            return this.AddItem(mailAddress.Address, mailAddress.Note);
        }

        // 一覧から削除する
        private void RemoveItem(ListViewItem item)
        {
            this.listViewMailAddress.Items.Remove(item);
            this.textBoxMailAddr.Focus();
        }

        // 一覧項目を探す
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

        // 入力されたメールアドレスを設定する
        private void SetAddr(string mailAddr)
        {
            this.MailAddr = mailAddr;

            this.formResult = MailAddressFormResult.SetAddr;
            this.Close();
        }

        // キャンセルする
        private void Cancel()
        {
            this.formResult = MailAddressFormResult.Cancel;
            this.Close();
        }

        // ソートの状態を設定する
        private void SetNextSortStatus(int column)
        {
            if (column == this.currentSortColumn) {
                this.currentSortOrder = this.InvertSortOrder(this.currentSortOrder);
            } else {
                this.currentSortOrder = SortOrder.Ascending;
                this.currentSortColumn = column;
            }
        }

        // ソート順を得る
        private SortOrder InvertSortOrder(SortOrder current)
        {
            return (current == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
        }

        // メールアドレスをチェックする
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

        // メールアドレスを保存する
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
    }
}
