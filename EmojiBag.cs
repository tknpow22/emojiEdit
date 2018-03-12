using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace emojiEdit
{
    /// <summary>
    /// 絵文字データ
    /// </summary>
    class Emoji
    {
        public Emoji(int code, Image image)
        {
            this.Code = code;
            this.Image = image;
        }

        public int Code
        {
            private set;
            get;
        }

        public Image Image
        {
            private set;
            get;
        }
    }

    /// <summary>
    /// 絵文字を保持する
    /// </summary>
    class EmojiBag
    {
        // グループ毎の絵文字のキャプション
        private string[] captionList = new string[0];

        // グループ毎の絵文字数
        private int[] numIconInGroupList = new int[0];

        // 絵文字アイコンイメージファイルを格納したディレクトリパス
        private string iconsDirname;
        // 絵文字アイコンのグループおよび数の定義を格納したファイルパス
        private string iconsConfigFilename;

        // 絵文字イメージ
        private List<Emoji> emojiList = new List<Emoji>();
        // 索引用: キー(絵文字グループ番号とグループ内でのID)から
        private Dictionary<string, Emoji> emojiListFromKey = new Dictionary<string, Emoji>();
        // 索引用: 文字コードから
        private Dictionary<int, Emoji> emojiListFromCode = new Dictionary<int, Emoji>();

        // 空の絵文字イメージ
        private Image emptyEmoji = new Bitmap(Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

        // コンストラクタ
        public EmojiBag()
        {
        }

        // 初期化処理
        public void Init()
        {
            this.iconsDirname = Path.Combine(DataBags.Config.AppDirectory, Commons.EMOJI_RESOURCE_DIR);
            this.iconsConfigFilename = Path.Combine(this.iconsDirname, Commons.EMOJI_EMOJI_RESOURCE_CONFIG_FILE_NAME);

            using (Graphics graphicsEmptyEmojiImage = Graphics.FromImage(this.emptyEmoji)) {
                graphicsEmptyEmojiImage.FillRectangle(Brushes.White, 0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            }

            this.Load();
        }

        //
        // プロパティ
        //

        // グループ毎の絵文字のキャプション
        public string[] CaptionList
        {
            get {
                return this.captionList;
            }
        }

        // グループ毎の絵文字数
        public int[] NumIconInGroupList
        {
            get {
                return this.numIconInGroupList;
            }
        }

        // 空の絵文字イメージ
        public Image EmptyEmoji
        {
            get {
                return this.emptyEmoji;
            }
        }

        //
        // 処理
        //

        // 絵文字データを取得する
        public Emoji Get(int emojiGroupNo, int emojiId)
        {
            string key = string.Format("{0}_{1}", emojiGroupNo, emojiId);

            if (this.emojiListFromKey.ContainsKey(key)) {
                return this.emojiListFromKey[key];
            }

            return null;
        }

        // 絵文字データを取得する
        public Emoji Get(int code)
        {
            if (this.emojiListFromCode.ContainsKey(code)) {
                return this.emojiListFromCode[code];
            }

            return null;
        }

        //
        // 内部処理
        //

        // 絵文字コード・アイコンイメージをロードする
        private void Load()
        {
            this.LoadIconsConfig();
            
            // 絵文字グループ番号
            for (int emojiGroupNo = 1; emojiGroupNo <= this.numIconInGroupList.Length; ++emojiGroupNo) {
                int emojiCountInGroup = this.numIconInGroupList[emojiGroupNo - 1];

                // 絵文字ID: グループ内でのID
                for (int emojiId = 0; emojiId < emojiCountInGroup; ++emojiId) {

                    string filename = this.GetEmojiFilename(emojiGroupNo, emojiId);
                    if (filename == null) {
                        continue;
                    }

                    // ファイル名から文字コード(4桁)を得る
                    string fnameWOE = Path.GetFileNameWithoutExtension(filename);
                    if (fnameWOE.Length < 8) {
                        continue;   // 念のため
                    }

                    string scode = fnameWOE.Substring(fnameWOE.Length - 4);
                    if (scode.Length != 4) {
                        continue;
                    }

                    int code;
                    if (!int.TryParse(scode, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out code)) {
                        continue;
                    }

                    try {
                        Image imgEmojiOrig = new Bitmap(filename);
                        Image imgEmoji;
                        if (imgEmojiOrig.Width < Commons.ICON_WIDTH && imgEmojiOrig.Height < Commons.ICON_HEIGHT) {
                            // サイズが規定より小さい場合はそのまま描画する
                            int px = (Commons.ICON_WIDTH - imgEmojiOrig.Width) / 2;
                            int py = (Commons.ICON_HEIGHT - imgEmojiOrig.Height) / 2;
                            imgEmoji = new Bitmap(Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
                            using (Graphics graphics = Graphics.FromImage(imgEmoji)) {
                                Rectangle srcRect = new Rectangle(0, 0, imgEmojiOrig.Width, imgEmojiOrig.Height);
                                Rectangle desRect = new Rectangle(px, py, imgEmojiOrig.Width, imgEmojiOrig.Height);
                                graphics.DrawImage(imgEmojiOrig, desRect, srcRect, GraphicsUnit.Pixel);
                            }

                        } else {
                            imgEmoji = new Bitmap(imgEmojiOrig, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
                        }

                        Emoji emoji = new Emoji(code, imgEmoji);
                        this.emojiList.Add(emoji);
                        this.emojiListFromKey.Add(string.Format("{0}_{1}", emojiGroupNo, emojiId), emoji);
                        this.emojiListFromCode.Add(code, emoji);

                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        // 絵文字アイコンのグループおよび数の定義を格納したファイルを読み込む
        private void LoadIconsConfig()
        {
            try {
                string configText = File.ReadAllText(this.iconsConfigFilename, Encoding.ASCII);
                string[] configs = configText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // @coding string
                // @number-of-groups number
                string coding = "UTF-8";
                int numOfGroups = 0;
                foreach (string aConfig in configs) {
                    string config = aConfig.Trim();
                    if (config.StartsWith("@coding")) {
                        string lcoding = this.GetConfigValue("@coding", config);
                        if (lcoding != null) {
                            coding = lcoding;
                        }
                    }
                    if (config.StartsWith("@number-of-groups")) {
                        string numOfGroupsStr = this.GetConfigValue("@number-of-groups", config);
                        int lnumOfGroups;
                        if (int.TryParse(numOfGroupsStr, out lnumOfGroups)) {
                            numOfGroups = lnumOfGroups;
                        }
                    }
                }

                // @group-N-caption string
                // @group-N-number-of-icon number
                List<string> captions = new List<string>();
                List<int> numberOfIcons = new List<int>();
                configText = File.ReadAllText(this.iconsConfigFilename, Encoding.GetEncoding(coding));
                configs = configText.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int groupNo = 1; groupNo <= numOfGroups; ++groupNo) {
                    string caption = "";
                    int numberOfIcon = 0;
                    {
                        string key = string.Format("@group-{0}-caption", groupNo);
                        caption = this.FindConfigValue(key, configs);
                    }
                    {
                        string key = string.Format("@group-{0}-number-of-icon", groupNo);
                        string val = this.FindConfigValue(key, configs);
                        int lnumberOfIcon;
                        if (int.TryParse(val, out lnumberOfIcon)) {
                            numberOfIcon = lnumberOfIcon;
                        }
                    }
                    captions.Add(caption);
                    numberOfIcons.Add(numberOfIcon);
                }

                this.captionList = captions.ToArray();
                this.numIconInGroupList = numberOfIcons.ToArray();

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        // 定義値を探す
        private string FindConfigValue(string key, string[] configs)
        {
            foreach (string aConfig in configs) {
                string config = aConfig.Trim();
                if (config.StartsWith(key)) {
                    return this.GetConfigValue(key, config);
                }
            }
            return null;
        }

        // 定義値を得る
        private string GetConfigValue(string key, string config)
        {
            string[] vals = config.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (vals.Length == 0) {
                return null;
            }
            if (vals[0] != key) {
                return null;
            }
            if (vals.Length <= 1) {
                return null;
            }
            string result = config.Substring(key.Length);
            result = result.Trim();
            return result.Trim();
        }

        // 絵文字アイコンイメージファイル名を返す
        // ただし、文字コードが無いものは対象としない。
        private string GetEmojiFilename(int emojiGroupNo, int emojiId)
        {
            string result = null;

            // 絵文字アイコンイメージファイル名は、gid_id.png または gid_id_code.png の形式とする。
            //      gid: 絵文字グループ番号
            //      id: 絵文字ID
            //      code: 文字コード(JIS): 16進表記
            // ただし、code は無い場合がある。
            // gid_id_code.png の形式のファイルを対象として探す。

            string pattern = string.Format(@"{0}_{1}_*.png", emojiGroupNo, emojiId);

            DirectoryInfo di = new DirectoryInfo(this.iconsDirname);
            FileInfo[] files = di.GetFiles(pattern, SearchOption.TopDirectoryOnly);

            foreach (FileInfo fi in files) {
                result = fi.FullName;
                break;
            }

            return result;
        }
    }
}
