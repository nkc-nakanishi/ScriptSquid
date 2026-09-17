using SubSquid;

namespace MainSquid
{
    public partial class MainForm : Form
    {
        TextBox folderBox;
        TextBox filterBox;
        TextBox commandView;
        Button scanButton;
        Button browseButton;
        ListView resultList;
        ComboBox filterHistory;
        Label countLabel;
        List<FileInfoData> fileDataList = new();

        public MainForm()
        {
            InitializeComponent();
            BuildUI();
            LoadCommandList();
        }

        private void BuildUI()
        {
            // フォーム設定
            this.Text = "ScriptSquid";
            this.Width = 900;
            this.Height = 600;

            // フォルダ入力
            folderBox = new TextBox()
            {
                Left = 10,
                Top = 10,
                Width = 600
            };
            this.Controls.Add(folderBox);

            filterHistory = new ComboBox()
            {
                Left = 410,
                Top = 540,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            filterHistory.SelectedIndexChanged += FilterHistory_SelectedIndexChanged;
            this.Controls.Add(filterHistory);

            // スキャンボタン
            scanButton = new Button()
            {
                Left = 620,
                Top = 11,
                Width = 120,
                Height = 30,
                Text = "Scan"
            };
            scanButton.Click += ScanButton_Click;
            this.Controls.Add(scanButton);

            filterBox = new TextBox()
            {
                Left = 100,
                Top = 540,
                Width = 300,
                PlaceholderText = "filter..."
            };
            filterBox.TextChanged += FilterBox_TextChanged;
            this.Controls.Add(filterBox);

            browseButton = new Button()
            {
                Left = 750,
                Top = 11,
                Width = 100,
                Height = 30,
                Text = "Browse"
            };
            browseButton.Click += BrowseButton_Click;
            this.Controls.Add(browseButton);

            commandView = new TextBox()
            {
                Left = 870,
                Top = 50,
                Width = 300,
                Height = 480,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 10),
                Text = "Command List\n-------------\n" // 初期表示
            };
            this.Controls.Add(commandView);

            // 結果リスト
            resultList = new ListView()
            {
                Left = 10,
                Top = 50,
                Width = 850,
                Height = 480,
                View = View.Details,
                FullRowSelect = true
            };
            resultList.Columns.Add("File", 200);
            resultList.Columns.Add("Lines", 80);
            resultList.Columns.Add("Size(KB)", 80);
            resultList.Columns.Add("Updated", 140);
            resultList.Columns.Add("Path", 300);

            resultList.DoubleClick += ResultList_DoubleClick;

            this.Controls.Add(resultList);

            // 件数ラベル
            countLabel = new Label()
            {
                Left = 10,
                Top = 540,
                Width = 300,
                Text = "Total: 0"
            };
            this.Controls.Add(countLabel);
        }
        private void ScanButton_Click(object sender, EventArgs e)
        {
            string root = folderBox.Text;
            if (!Directory.Exists(root)) return;

            fileDataList.Clear();   // ← 重要！

            var csFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);

            foreach (var file in csFiles)
            {
                int lines = CountLines(file);
                var info = new FileInfo(file);

                fileDataList.Add(new FileInfoData
                {
                    FileName = Path.GetFileName(file),
                    Lines = lines,
                    SizeKB = info.Length / 1024,
                    Updated = info.LastWriteTime.ToString("yyyy/MM/dd HH:mm"),
                    FullPath = file
                });
            }
            countLabel.Text = $"Total: {csFiles.Length}";
            RenderList(fileDataList);   // ← これが無いと ListView に出ない
        }

        private int CountLines(string path)
        {
            using var sr = new StreamReader(path);
            int count = 0;
            while (sr.ReadLine() != null)
                count++;
            return count;
        }
        private void ResultList_DoubleClick(object sender, EventArgs e)
        {
            if (resultList.SelectedItems.Count == 0) return;

            string path = resultList.SelectedItems[0].SubItems[4].Text;
            System.Diagnostics.Process.Start("notepad.exe", path);
        }
        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog();
            dialog.Description = "Select folder to scan";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderBox.Text = dialog.SelectedPath;
            }
        }
        private void RenderList(IEnumerable<FileInfoData> list)
        {
            resultList.Items.Clear();

            foreach (var d in list)
            {
                var item = new ListViewItem(d.FileName);
                item.SubItems.Add(d.Lines.ToString());
                item.SubItems.Add(d.SizeKB.ToString());
                item.SubItems.Add(d.Updated);
                item.SubItems.Add(d.FullPath);

                resultList.Items.Add(item);
            }

            countLabel.Text = $"Total: {list.Count()}";
        }
        private void FilterBox_TextChanged(object sender, EventArgs e)
        {
            string text = filterBox.Text.Trim();

            // 空は追加しない
            if (!string.IsNullOrEmpty(text))
            {
                // 重複チェック
                if (!filterHistory.Items.Contains(text))
                    filterHistory.Items.Add(text);
            }

            var filtered = ApplyFilter(text);
            RenderList(filtered);
        }
        private void LoadCommandList()
        {
            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(exeDir, "CommandList.txt");
            if (File.Exists(path))
            {
                commandView.Text = File.ReadAllText(path);
            }
            else
            {
                commandView.Text = "CommandList.txt not found.";
            }
        }

        private IEnumerable<FileInfoData> ApplyFilter(string expr)
        {
            expr = expr.Trim();
            if (string.IsNullOrEmpty(expr)) return fileDataList;

            string[] ops = { ">=", "<=", ">", "<", "=", "≒" };
            string op = ops.FirstOrDefault(o => expr.Contains(o));
            if (op == null) return fileDataList;

            var parts = expr.Split(op);
            if (parts.Length != 2) return fileDataList;

            string left = parts[0].Trim();
            string right = parts[1].Trim().Trim('"');

            return FilterBy(left, op, right);
        }
        private IEnumerable<FileInfoData> FilterBy(string left, string op, string right)
        {
            switch (left.ToLower())
            {
                case "Lines":
                    if (int.TryParse(right, out int num))
                        return CompareNumber(fileDataList, d => d.Lines, op, num);
                    break;

                case "SizeKB":
                    if (long.TryParse(right, out long size))
                        return CompareNumber(fileDataList, d => d.SizeKB, op, size);
                    break;

                case "FileName":
                    return CompareString(fileDataList, d => d.FileName, op, right);

                case "FullPath":
                    return CompareString(fileDataList, d => d.FullPath, op, right);

                case "Updated":
                    return CompareString(fileDataList, d => d.Updated, op, right);
            }

            return fileDataList;
        }

        private IEnumerable<FileInfoData> CompareNumber<T>(
        IEnumerable<FileInfoData> list,
        Func<FileInfoData, T> selector,
        string op,
        T value) where T : IComparable
        {
            return op switch
            {
                ">" => list.Where(d => selector(d).CompareTo(value) > 0),
                "<" => list.Where(d => selector(d).CompareTo(value) < 0),
                "=" => list.Where(d => selector(d).CompareTo(value) == 0),
                ">=" => list.Where(d => selector(d).CompareTo(value) >= 0),
                "<=" => list.Where(d => selector(d).CompareTo(value) <= 0),
                _ => list
            };
        }

        private IEnumerable<FileInfoData> CompareString(
        IEnumerable<FileInfoData> list,
        Func<FileInfoData, string> selector,
        string op,
        string value)
        {
            return op switch
            {
                "=" => list.Where(d => selector(d) == value),
                "≒" => list.Where(d => selector(d).Contains(value, StringComparison.OrdinalIgnoreCase)),
                _ => list
            };
        }
        private void FilterHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterBox.Text = filterHistory.SelectedItem.ToString();
        }

    }

}
