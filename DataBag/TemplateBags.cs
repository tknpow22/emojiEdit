using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace emojiEdit
{
    //
    // テンプレート
    //
    class EmojiTemplate
    {
        public EmojiTemplate(int cols, int rows, List<int> codeList)
        {
            this.Cols = cols;
            this.Rows = rows;
            this.CodeList = codeList;

            this.Thumbnail = this.CreateThumbnail();
        }

        // 文字数
        public int Cols
        {
            get; set;
        }

        // 行数
        public int Rows
        {
            get;
        }

        // 文字コード
        public List<int> CodeList
        {
            get;
        }

        public Image Thumbnail
        {
            get;
        }

        //
        // 内部処理
        //
        private Image CreateThumbnail()
        {
            int width = Commons.FRAME_WIDTH * this.Cols;
            int height = Commons.FRAME_HEIGHT * this.Rows;

            Image image = new Bitmap(width, height);

            using (Graphics graphics = Graphics.FromImage(image)) {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                DrawUtils.DrawCodes(this.Cols, this.Rows, this.CodeList, graphics);
            }

            return image; 
        }
    }

    //
    // テンプレートを保持する
    //
    class TemplateBags
    {
        // 設定ファイルパス
        private string filepath;

        // テンプレート一覧
        List<EmojiTemplate> templateList = new List<EmojiTemplate>();

        public TemplateBags()
        {
            this.filepath = Path.Combine(DataBags.Config.AppDirectory, Commons.TEMPLATE_FILE_NAME);
        }

        // 初期化処理
        public void Init()
        {
            this.Load();
        }

        public List<EmojiTemplate> Get()
        {
            return new List<EmojiTemplate>(this.templateList);
        }

        public void Set(List<EmojiTemplate> templateList)
        {
            this.templateList = new List<EmojiTemplate>(templateList);
        }

        // 書き込み
        public void Save()
        {
            try {
                using (TextWriter writer = new StreamWriter(this.filepath, false, new UTF8Encoding(false))) {
                    foreach (EmojiTemplate emojiTemplate in this.templateList) {
                        string hexaStr = Commons.GetHexaStringsFromCodes(emojiTemplate.CodeList);
                        string line = string.Format("{0}\t{1}\t{2}", emojiTemplate.Cols, emojiTemplate.Rows, hexaStr);
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
            // フォーマットは、
            // cols<tab>rows<tab>XXXX,XXXX,XXXX,...
            // cols: 文字数
            // rows: 行数
            // XXXX,...: 4桁の16進数文字列をカンマ区切りで記述
            // tab: タブコード(\t)

            try {
                string[] lines = File.ReadAllLines(this.filepath);

                foreach (string line in lines) {
                    string[] columns = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (columns.Length < 3) {
                        continue;
                    }
                    string colsStr = columns[0].Trim();
                    string rowsStr = columns[1].Trim();
                    string hexStr = columns[2].Trim();

                    int cols;
                    int rows;

                    if (!int.TryParse(colsStr, out cols) || !int.TryParse(rowsStr, out rows)) {
                        continue;
                    }

                    if (cols < Commons.TEMPLATE_MAX_COLS_MINIMUM || Commons.TEMPLATE_MAX_COLS_MAXIMUM < cols) {
                        cols = Commons.TEMPLATE_MAX_COLS_DEFALUT;
                    }
                    if (rows < Commons.TEMPLATE_MAX_ROWS_MINIMUM || Commons.TEMPLATE_MAX_ROWS_MAXIMUM < rows) {
                        rows = Commons.TEMPLATE_MAX_ROWS_DEFALUT;
                    }

                    List<int> codeList = Commons.GetCodesFromHexaStrings(hexStr);

                    EmojiTemplate emojiTemplate = new EmojiTemplate(cols, rows, codeList);
                    this.templateList.Add(emojiTemplate);
                }

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
