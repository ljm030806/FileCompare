using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void txtLeftDir_TextChanged(object sender, EventArgs e)
        {
            // 경로 텍스트박스를 직접 수정할 때도 바로 반영되길 원한다면 추가 가능합니다.
            // UpdateAndCompareListViews();
        }

        private void txtRightDir_TextChanged(object sender, EventArgs e)
        {
            // UpdateAndCompareListViews();
        }

        private void brnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                    // 폴더 선택 후 양쪽 비교 및 즉시 반영
                    UpdateAndCompareListViews();
                }
            }
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                    // 폴더 선택 후 양쪽 비교 및 즉시 반영
                    UpdateAndCompareListViews();
                }
            }
        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
        }

        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lvwRightDir_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        // ▼ 양쪽 폴더를 동시에 읽고 비교 기준에 맞춰 색상을 바로 칠해주는 통합 메서드
        private void UpdateAndCompareListViews()
        {
            string leftPath = txtLeftDir.Text;
            string rightPath = txtRightDir.Text;

            bool hasLeft = !string.IsNullOrWhiteSpace(leftPath) && Directory.Exists(leftPath);
            bool hasRight = !string.IsNullOrWhiteSpace(rightPath) && Directory.Exists(rightPath);

            lvwLeftDir.BeginUpdate();
            lvwRightDir.BeginUpdate();
            lvwLeftDir.Items.Clear();
            lvwRightDir.Items.Clear();

            try
            {
                var leftFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
                var rightFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);

                // 1. 디렉터리 표시 및 왼쪽 파일 정보 수집
                if (hasLeft)
                {
                    foreach (var d in Directory.EnumerateDirectories(leftPath).Select(p => new DirectoryInfo(p)).OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(d.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(d.LastWriteTime.ToString("g"));
                        lvwLeftDir.Items.Add(item);
                    }
                    leftFiles = Directory.EnumerateFiles(leftPath).Select(p => new FileInfo(p)).ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
                }

                // 2. 디렉터리 표시 및 오른쪽 파일 정보 수집
                if (hasRight)
                {
                    foreach (var d in Directory.EnumerateDirectories(rightPath).Select(p => new DirectoryInfo(p)).OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(d.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(d.LastWriteTime.ToString("g"));
                        lvwRightDir.Items.Add(item);
                    }
                    rightFiles = Directory.EnumerateFiles(rightPath).Select(p => new FileInfo(p)).ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
                }

                // 3. 왼쪽 파일 목록 채우기 + 색상 결정
                if (hasLeft)
                {
                    foreach (var lf in leftFiles.Values.OrderBy(f => f.Name))
                    {
                        var litem = new ListViewItem(lf.Name);
                        litem.SubItems.Add(FormatSizeInKb(lf.Length));
                        litem.SubItems.Add(lf.LastWriteTime.ToString("g"));

                        if (hasRight && rightFiles.TryGetValue(lf.Name, out var rf))
                        {
                            if (lf.LastWriteTime == rf.LastWriteTime) litem.ForeColor = Color.Black;        // 1단계: 동일 (검은색)
                            else if (lf.LastWriteTime > rf.LastWriteTime) litem.ForeColor = Color.Red;      // 2단계: 파일이름 같고 수정시간이 더 최신 New (빨간색)
                            else litem.ForeColor = Color.Gray;                                              // 2단계: 파일이름 같고 수정시간이 더 과거 Old (회색)
                        }
                        else
                        {
                            // 반대편 폴더가 존재하는데 이 파일이 반대편에 없다면 '단독파일', 반대편 폴더가 아예 비어있으면 그냥 일반 파일
                            litem.ForeColor = hasRight ? Color.Purple : Color.Black;                        // 3단계: 단독 파일 (보라색)
                        }
                        lvwLeftDir.Items.Add(litem);
                    }
                }

                // 4. 오른쪽 파일 목록 채우기 + 색상 결정
                if (hasRight)
                {
                    foreach (var rf in rightFiles.Values.OrderBy(f => f.Name))
                    {
                        var ritem = new ListViewItem(rf.Name);
                        ritem.SubItems.Add(FormatSizeInKb(rf.Length));
                        ritem.SubItems.Add(rf.LastWriteTime.ToString("g"));

                        if (hasLeft && leftFiles.TryGetValue(rf.Name, out var lf))
                        {
                            if (rf.LastWriteTime == lf.LastWriteTime) ritem.ForeColor = Color.Black;        // 1단계: 동일 (검은색)
                            else if (rf.LastWriteTime > lf.LastWriteTime) ritem.ForeColor = Color.Red;      // 2단계: New (빨간색)
                            else ritem.ForeColor = Color.Gray;                                              // 2단계: Old (회색)
                        }
                        else
                        {
                            ritem.ForeColor = hasLeft ? Color.Purple : Color.Black;                         // 3단계: 단독 파일 (보라색)
                        }
                        lvwRightDir.Items.Add(ritem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "파일 목록 갱신 오류: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                lvwLeftDir.EndUpdate();
                lvwRightDir.EndUpdate();
            }
        }

        // 바이트 단위의 파일 크기를 알아보기 쉽게 KB 단위 변환 (콤마 포함)
        private string FormatSizeInKb(long bytes)
        {
            return (bytes / 1024.0).ToString("N0") + " KB";
        }
    }
}

