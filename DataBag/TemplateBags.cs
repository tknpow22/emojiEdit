namespace emojiEdit
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using System.IO;

    /// <summary>
    /// テンプレートとして不適切な場合の例外
    /// </summary>
    class EmojiTemplateException : Exception
    {
    }

    /// <summary>
    /// テンプレート
    /// </summary>
    class EmojiTemplate
    {
        #region 処理

        /// <summary>
        /// コンストラクタ
        /// NOTE: !!注意!!: 不適切な文字列に対しては例外を投げるので注意
        /// </summary>
        /// <param name="text">テンプレート文字列</param>
        public EmojiTemplate(string text)
        {
            this.Text = text;

            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");
            text = text.Replace("\t", " ");
            string[] textList = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // サムネイル画像作成用の文字列を取得する
            List<string> thumbnailTextList = new List<string>();
            for (int i = 0; i < textList.Length && i < Commons.TEMPLATE_THUMBNAIL_ROWS; ++i) {
                string line = textList[i].Trim();
                if (0 < line.Length) {
                    string lineNew = line.Substring(0, Math.Min(line.Length, Commons.TEMPLATE_THUMBNAIL_COLS));
                    thumbnailTextList.Add(lineNew);
                }
            }

            if (thumbnailTextList.Count == 0) {
                throw new EmojiTemplateException();
            }

            this.Thumbnail = this.CreateThumbnail(thumbnailTextList);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 文字列
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// サムネイル画像
        /// </summary>
        public Image Thumbnail
        {
            get;
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// サムネイル画像を作成する
        /// </summary>
        /// <returns>Image</returns>
        private Image CreateThumbnail(List<string> textList)
        {
            int width = Commons.FRAME_WIDTH * Commons.TEMPLATE_THUMBNAIL_COLS;
            int height = Commons.FRAME_HEIGHT * Commons.TEMPLATE_THUMBNAIL_ROWS;

            Bitmap image = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(image)) {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
            }

            EmojiTextBox textBox = new EmojiTextBox();
            textBox.Font = new Font(Commons.CONTENTS_FONT_NAME, Commons.CONTENTS_FONT_SIZE);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Multiline = true;
            textBox.Width = width;
            textBox.Height = height;
            textBox.Text = string.Join("\r\n", textList);

            textBox.DrawToBitmap(image, new Rectangle(0, 0, textBox.Width, textBox.Height));

            return image;
        }

        #endregion
    }

    /// <summary>
    /// テンプレートを保持する
    /// </summary>
    class TemplateBags : CDataBag
    {
        #region 変数

        /// <summary>
        /// 設定ファイルパス
        /// </summary>
        private string filepath;

        /// <summary>
        /// 設定ファイルのエンコーディング
        /// </summary>
        private Encoding fileEncoding = Encoding.GetEncoding("UTF-16");

        /// <summary>
        /// テンプレート一覧
        /// </summary>
        List<EmojiTemplate> templateList = new List<EmojiTemplate>();

        #endregion

        #region 処理

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateBags()
        {
            this.filepath = Path.Combine(DataBags.Config.AppDirectory, Commons.TEMPLATE_FILE_NAME);
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
        /// テンプレート一覧を取得する
        /// </summary>
        /// <returns>テンプレート一覧</returns>
        public List<EmojiTemplate> Get()
        {
            return new List<EmojiTemplate>(this.templateList);
        }

        /// <summary>
        /// テンプレート一覧を設定する
        /// </summary>
        /// <param name="templateList">テンプレート一覧</param>
        public void Set(List<EmojiTemplate> templateList)
        {
            this.templateList = new List<EmojiTemplate>(templateList);
        }

        /// <summary>
        /// テンプレートを追加する
        /// </summary>
        /// <param name="emojiTemplate">テンプレート</param>
        public void Add(EmojiTemplate emojiTemplate)
        {
            this.templateList.Add(emojiTemplate);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// テンプレート一覧の数を返す
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            get {
                return this.templateList.Count;
            }
        }

        #endregion

        #region 内部処理

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load()
        {
            // 1 行 1 テンプレートで保存
            // 文字列中の '\t', '\r', '\n' を復元して使用する

            try {
                string[] lines = File.ReadAllLines(this.filepath, this.fileEncoding);

                foreach (string line in lines) {

                    // テンプレート毎に例外が発生する可能性があるためハンドリングする
                    try {

                        string text = line;

                        text = text.Replace("\\t", "\t");
                        text = text.Replace("\\r\\n", "\r\n");
                        text = text.Replace("\\r", "\n");

                        this.templateList.Add(new EmojiTemplate(text));
                    } catch (EmojiTemplateException ex) {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
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
                using (TextWriter writer = new StreamWriter(this.filepath, false, this.fileEncoding)) {
                    foreach (EmojiTemplate emojiTemplate in this.templateList) {
                        string text = emojiTemplate.Text;

                        text = text.Replace("\t", "\\t");
                        text = text.Replace("\r\n", "\\r\\n");
                        text = text.Replace("\r", "\\n");

                        writer.WriteLine(text);
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
