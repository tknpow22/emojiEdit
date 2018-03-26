namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// メールアドレス
    /// </summary>
    class MailAddress
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="address">メールアドレス</param>
        /// <param name="note">備考</param>
        public MailAddress(string address, string note)
        {
            this.Address = address;
            this.Note = note;
        }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string Address
        {
            get; private set;
        }

        /// <summary>
        /// 備考
        /// </summary>
        public string Note
        {
            get; private set;
        }
    }

    /// <summary>
    /// メールアドレスを保持する
    /// </summary>
    class MailAddressBag : CDataBag
    {
        #region 変数

        // 設定ファイルパス
        private string filepath;

        // アドレス一覧
        List<MailAddress> addrList = new List<MailAddress>();

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MailAddressBag()
        {
            this.filepath = Path.Combine(DataBags.Config.AppDirectory, Commons.MAIL_ADDRESSES_FILE_NAME);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            this.Load();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public override void Terminate()
        {
            this.Save();
        }

        /// <summary>
        /// アドレス一覧を取得する
        /// </summary>
        /// <returns>アドレス一覧</returns>
        public List<MailAddress> Get()
        {
            return new List<MailAddress>(this.addrList);
        }

        /// <summary>
        /// アドレス一覧を設定する
        /// </summary>
        /// <param name="addrList">アドレス一覧</param>
        public void Set(List<MailAddress> addrList)
        {
            this.addrList = new List<MailAddress>(addrList);
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load()
        {
            try {
                using (TextReader reader = new StreamReader(this.filepath, Encoding.GetEncoding("UTF-8"))) {
                    string line;
                    while ((line = reader.ReadLine()) != null) {
                        string[] columns = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (columns.Length == 0) {
                            continue;
                        }
                        string address = columns[0].Trim();
                        if (address.Length == 0) {
                            continue;
                        }

                        string note = "";
                        if (1 < columns.Length) {
                            note = columns[1].Trim();
                        }

                        this.addrList.Add(new MailAddress(address, note));
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            try {
                using (TextWriter writer = new StreamWriter(this.filepath, false, new UTF8Encoding(false))) {
                    foreach (MailAddress mailAddr in this.addrList) {
                        string line = string.Format("{0}\t{1}", mailAddr.Address.Trim(), mailAddr.Note.Trim());
                        writer.WriteLine(line);
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
