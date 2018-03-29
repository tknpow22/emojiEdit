namespace emojiEdit
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 絵文字データ
    /// </summary>
    class Emoji
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="unicode">ユニコード</param>
        /// <param name="jiscode">JIS コード</param>
        /// <param name="image">絵文字アイコン</param>
        /// <param name="imageForText">絵文字アイコン(テキスト表示用)</param>
        public Emoji(char unicode, int jiscode, Image image, Image imageForText)
        {
            this.Unicode = unicode;
            this.Jiscode = jiscode;
            this.Image = image;
            this.ImageForText = imageForText;
        }

        /// <summary>
        /// ユニコード
        /// </summary>
        public char Unicode
        {
            private set;
            get;
        }

        /// <summary>
        /// JISコード
        /// </summary>
        public int Jiscode
        {
            private set;
            get;
        }

        /// <summary>
        /// 絵文字アイコン
        /// </summary>
        public Image Image
        {
            private set;
            get;
        }

        /// <summary>
        /// 絵文字アイコン(テキスト表示用)
        /// </summary>
        public Image ImageForText
        {
            private set;
            get;
        }
    }

    /// <summary>
    /// 絵文字を保持する
    /// </summary>
    class EmojiBag : CDataBag
    {
        #region 変数

        /// <summary>
        /// グループ毎の絵文字のキャプション
        /// </summary>
        private string[] captionList = new string[0];

        /// <summary>
        /// グループ毎の絵文字数
        /// </summary>
        private int[] numIconInGroupList = new int[0];

        /// <summary>
        /// 絵文字アイコンイメージファイルを格納したディレクトリパス
        /// </summary>
        private string iconsDirname;

        /// <summary>
        /// 絵文字アイコンのグループおよび数の定義を格納したファイルパス
        /// </summary>
        private string iconsConfigFilename;

        #region 絵文字イメージと索引

        /// <summary>
        /// 絵文字イメージ
        /// </summary>
        private List<Emoji> emojiList = new List<Emoji>();

        /// <summary>
        /// 索引用: キー(絵文字グループ番号とグループ内でのID)から
        /// </summary>
        private Dictionary<string, Emoji> emojiListFromKey = new Dictionary<string, Emoji>();

        /// <summary>
        /// 索引用: 文字コード(Unicode)から
        /// </summary>
        private Dictionary<char, Emoji> emojiListFromUnicode = new Dictionary<char, Emoji>();

        /// <summary>
        /// 索引用: 文字コード(JIS)から
        /// </summary>
        private Dictionary<int, Emoji> emojiListFromJiscode = new Dictionary<int, Emoji>();

        #endregion

        /// <summary>
        /// 空の絵文字イメージ
        /// </summary>
        private Image emptyEmoji = new Bitmap(Commons.ICON_WIDTH, Commons.ICON_HEIGHT);

        #endregion

        #region プロパティ

        /// <summary>
        /// グループ毎の絵文字のキャプション
        /// </summary>
        public string[] CaptionList
        {
            get {
                return this.captionList;
            }
        }

        /// <summary>
        /// グループ毎の絵文字数
        /// </summary>
        public int[] NumIconInGroupList
        {
            get {
                return this.numIconInGroupList;
            }
        }

        /// <summary>
        /// 空の絵文字イメージ
        /// </summary>
        public Image EmptyEmoji
        {
            get {
                return this.emptyEmoji;
            }
        }

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EmojiBag()
        {
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public override void Initialize()
        {
            this.iconsDirname = Path.Combine(DataBags.Config.AppDirectory, Commons.EMOJI_RESOURCE_DIR);
            this.iconsConfigFilename = Path.Combine(this.iconsDirname, Commons.EMOJI_EMOJI_RESOURCE_CONFIG_FILE_NAME);

            using (Graphics graphicsEmptyEmojiImage = Graphics.FromImage(this.emptyEmoji)) {
                graphicsEmptyEmojiImage.FillRectangle(Brushes.White, 0, 0, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
            }

            this.Load();
        }

        /// <summary>
        /// 絵文字データを取得する
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">グループ内でのID</param>
        /// <returns>絵文字データ: 該当なしの場合は null</returns>
        public Emoji Get(int emojiGroupNo, int emojiId)
        {
            string key = string.Format("{0}_{1}", emojiGroupNo, emojiId);

            if (this.emojiListFromKey.ContainsKey(key)) {
                return this.emojiListFromKey[key];
            }

            return null;
        }

        /// <summary>
        /// 絵文字データを JIS コードから取得する
        /// </summary>
        /// <param name="jiscode">JIS コード</param>
        /// <returns>絵文字データ: 該当なしの場合は null</returns>
        public Emoji GetFromJiscode(int jiscode)
        {
            if (this.emojiListFromJiscode.ContainsKey(jiscode)) {
                return this.emojiListFromJiscode[jiscode];
            }

            return null;
        }

        /// <summary>
        /// 絵文字データを Unicode から取得する
        /// </summary>
        /// <param name="unicode">ユニコード</param>
        /// <returns>絵文字データ: 該当なしの場合は null</returns>
        public Emoji GetFromUnicode(char unicode)
        {
            if (this.emojiListFromUnicode.ContainsKey(unicode)) {
                return this.emojiListFromUnicode[unicode];
            }

            return null;
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 絵文字コード・アイコンイメージをロードする
        /// </summary>
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

                    (int unicode, int jiscode) = this.GetCharCodeFromFilename(filename);
                    if (unicode == 0 || jiscode == 0) {
                        continue;
                    }

                    try {
                        Image imgEmojiOrig = new Bitmap(filename);
                        Image imgEmoji = this.CreateEmojiIconImage(imgEmojiOrig, Commons.ICON_WIDTH, Commons.ICON_HEIGHT);
                        Image imgEmojiForText = this.CreateEmojiIconImage(imgEmojiOrig, Commons.TEXT_ICON_WIDTH, Commons.TEXT_ICON_HEIGHT);

                        Emoji emoji = new Emoji((char)unicode, jiscode, imgEmoji, imgEmojiForText);
                        this.emojiList.Add(emoji);
                        this.emojiListFromKey.Add(string.Format("{0}_{1}", emojiGroupNo, emojiId), emoji);
                        this.emojiListFromJiscode.Add(jiscode, emoji);
                        this.emojiListFromUnicode.Add((char)unicode, emoji);

                    } catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// サイズに応じた絵文字アイコンイメージを作成する
        /// </summary>
        /// <param name="imgEmojiOrig">元の絵文字アイコンイメージ</param>
        /// <param name="width">サイズ(幅)</param>
        /// <param name="height">サイズ(高さ)</param>
        /// <returns>新しい絵文字アイコンイメージ</returns>
        private Image CreateEmojiIconImage(Image imgEmojiOrig, int width, int height)
        {
            Image imgEmoji;

            if (imgEmojiOrig.Width < width && imgEmojiOrig.Height < height) {

                // サイズが指定より小さい場合はそのまま描画する
                int px = (width - imgEmojiOrig.Width) / 2;
                int py = (height - imgEmojiOrig.Height) / 2;
                imgEmoji = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(imgEmoji)) {
                    Rectangle srcRect = new Rectangle(0, 0, imgEmojiOrig.Width, imgEmojiOrig.Height);
                    Rectangle desRect = new Rectangle(px, py, imgEmojiOrig.Width, imgEmojiOrig.Height);
                    graphics.DrawImage(imgEmojiOrig, desRect, srcRect, GraphicsUnit.Pixel);
                }

            } else {
                imgEmoji = new Bitmap(imgEmojiOrig, width, height);
            }

            return imgEmoji;
        }

        /// <summary>
        /// 絵文字アイコンのグループおよび数の定義を格納したファイルを読み込む
        /// </summary>
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

                    if (string.IsNullOrEmpty(caption) || numberOfIcon <= 0) {
                        continue;
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

        /// <summary>
        /// 定義値を設定値から探す
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="configs">設定値</param>
        /// <returns>キーに対応する値</returns>
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

        /// <summary>
        /// 定義値を設定値から取得する
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="configs">設定値</param>
        /// <returns>キーに対応する値</returns>
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

        /// <summary>
        /// ファイル名から unicode, jiscode を得る
        /// </summary>
        /// <param name="filename">ファイル名</param>
        /// <returns>(unicode, jiscode): 取得できない場合はともに 0</returns>
        private (int, int) GetCharCodeFromFilename(string filename)
        {
            (int, int) result = (0, 0);

            do {
                // ファイル名から *_unicode_jiscode.png の unicode, jiscode を取得する
                // ファイル名から文字コード(4桁)を得る
                string fnameWOE = Path.GetFileNameWithoutExtension(filename);
                if (fnameWOE.Length < 10) {
                    break;
                }

                // unicode
                string sUnicode = fnameWOE.Substring(fnameWOE.Length - 9, 4);
                if (sUnicode.Length != 4) {
                    break;
                }

                int unicode;
                if (!int.TryParse(sUnicode, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out unicode)) {
                    break;
                }

                // jiscode
                string sJiscode = fnameWOE.Substring(fnameWOE.Length - 4, 4);
                if (sJiscode.Length != 4) {
                    break;
                }

                int jiscode;
                if (!int.TryParse(sJiscode, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out jiscode)) {
                    break;
                }

                result = (unicode, jiscode);

            } while (false);

            return result;
        }

        /// <summary>
        /// 絵文字アイコンイメージファイル名を返す。
        /// ただし、文字コードが無いものは対象としない。
        /// </summary>
        /// <param name="emojiGroupNo">絵文字グループ番号</param>
        /// <param name="emojiId">グループ内でのID</param>
        /// <returns>ファイル名</returns>
        private string GetEmojiFilename(int emojiGroupNo, int emojiId)
        {
            string result = null;

            // 絵文字アイコンイメージファイル名は gid_id_unicode_jiscode.png の形式とする。
            //      gid: 絵文字グループ番号
            //      id: 絵文字ID
            //      unicode: 文字コード(Unicode: UTF-16): 4桁16進表記
            //      jiscode: 文字コード(JIS): 4桁16進表記
            // gid_id_*.png 形式のファイルを探し最初に見つかったものを返す。

            string pattern = string.Format(@"{0}_{1}_*.png", emojiGroupNo, emojiId);

            DirectoryInfo di = new DirectoryInfo(this.iconsDirname);
            FileInfo[] files = di.GetFiles(pattern, SearchOption.TopDirectoryOnly);

            foreach (FileInfo fi in files) {
                result = fi.FullName;
                break;
            }

            return result;
        }

        #endregion
    }
}
