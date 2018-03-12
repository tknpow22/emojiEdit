using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace emojiEdit
{
    class MailAddress
    {
        public MailAddress(string address, string note)
        {
            this.Address = address;
            this.Note = note;
        }

        public string Address
        {
            get; set;
        }

        public string Note
        {
            get; set;
        }
    }

    //
    // メールアドレスを保持する
    //
    class MailAddressBag
    {
        // 設定ファイルパス
        private string filepath;

        // アドレス一覧
        List<MailAddress> addrList = new List<MailAddress>();

        // コンストラクタ
        public MailAddressBag()
        {
            this.filepath = Path.Combine(DataBags.Config.AppDirectory, Commons.MAIL_ADDRESSES_FILE_NAME);
        }

        // 初期化処理
        public void Init()
        {
            this.Load();
        }

        public List<MailAddress> Get()
        {
            return new List<MailAddress>(this.addrList);
        }

        public void Set(List<MailAddress> addrList)
        {
            this.addrList = new List<MailAddress>(addrList);
        }

        // 書き込み
        public void Save()
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

        //
        // 内部処理
        //

        // 読み込み
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
    }
}
