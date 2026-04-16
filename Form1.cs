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
            string leftPath = txtLeftDir.Text;
            string rightPath = txtRightDir.Text;

            if (!Directory.Exists(leftPath) || !Directory.Exists(rightPath))
            {
                MessageBox.Show("양쪽 폴더가 모두 선택되어 있어야 복사할 수 있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvwLeftDir.SelectedItems.Count == 0)
            {
                MessageBox.Show("복사할 항목을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 왼쪽(Left) 리스트에서 선택된 항목들만 복사
                foreach (ListViewItem item in lvwLeftDir.SelectedItems)
                {
                    string relPath = item.Text;
                    string sourcePath = Path.Combine(leftPath, relPath);
                    string destPath = Path.Combine(rightPath, relPath);

                    // 폴더인 경우 (<DIR>) 내부 파일까지 전부 복사
                    if (item.SubItems.Count > 1 && item.SubItems[1].Text == "<DIR>")
                    {
                        CopyDirectory(sourcePath, destPath);
                    }
                    else // 파일인 경우 단일 파일 복사 (하위 폴더의 상대경로 파일 포함)
                    {
                        CopySingleFile(sourcePath, destPath);
                    }
                }

                // 복사가 완료되면 결과를 바로 양측에 반영
                UpdateAndCompareListViews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("복사 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            string leftPath = txtLeftDir.Text;
            string rightPath = txtRightDir.Text;

            if (!Directory.Exists(leftPath) || !Directory.Exists(rightPath))
            {
                MessageBox.Show("양쪽 폴더가 모두 선택되어 있어야 복사할 수 있습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvwRightDir.SelectedItems.Count == 0)
            {
                MessageBox.Show("복사할 항목을 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                // 오른쪽(Right) 리스트에서 선택된 항목들만 복사
                foreach (ListViewItem item in lvwRightDir.SelectedItems)
                {
                    string relPath = item.Text;
                    string sourcePath = Path.Combine(rightPath, relPath);
                    string destPath = Path.Combine(leftPath, relPath);

                    // 폴더인 경우 (<DIR>) 내부 파일까지 전부 복사
                    if (item.SubItems.Count > 1 && item.SubItems[1].Text == "<DIR>")
                    {
                        CopyDirectory(sourcePath, destPath);
                    }
                    else // 파일인 경우 단일 파일 복사
                    {
                        CopySingleFile(sourcePath, destPath);
                    }
                }

                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("복사 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ▼ 단일 파일을 복사하며 옛날 파일 덮어쓰기 여부를 확인하는 공통 로직
        private void CopySingleFile(string sourceFilePath, string targetFilePath)
        {
            FileInfo sourceFile = new FileInfo(sourceFilePath);
            if (!sourceFile.Exists) return;

            // 목적지 파일의 폴더 경로가 존재하지 않는다면(하위 폴더 구조 등) 생성
            string targetDir = Path.GetDirectoryName(targetFilePath);
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // 원본 파일이 복사될 대상 쪽에 이미 존재하는 경우 날짜 비교
            if (File.Exists(targetFilePath))
            {
                FileInfo targetFile = new FileInfo(targetFilePath);

                // 복사할 원본 파일이 대상 파일보다 시간상 더 오래된(과거의) 파일인 경우
                if (sourceFile.LastWriteTime < targetFile.LastWriteTime)
                {
                    DialogResult result = MessageBox.Show(
                        $"원본 파일 '{sourceFile.Name}' 은(는) 대상 쪽의 파일보다 오래된 파일입니다.\n정말 최신 파일을 덮어쓰고 복사하시겠습니까?",
                        "복사 확인",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    // 사용자가 No를 클릭하면 이 파일은 건너뛰기
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            // 조건 충족 시 복사 (덮어쓰기 허용)
            sourceFile.CopyTo(targetFilePath, true);
        }

        // ▼ 폴더와 그 내부의 모든 파일, 하위 폴더들을 재귀적으로 복사하는 헬퍼 메서드
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return;

            // 목적지 폴더가 없다면 생성
            Directory.CreateDirectory(destinationDir);

            // 1. 현재 폴더 안의 모든 파일을 복사 (각 파일별로 공통 로직 사용)
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                CopySingleFile(file.FullName, targetFilePath);
            }

            // 2. 하위 폴더들을 다시 재귀적으로 복사
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir);
            }
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
                // Key를 전체 경로 대신 기준 폴더로부터의 상대 경로(Relative Path)로 설정하여 비교합니다.
                var leftFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
                var rightFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);

                // 1. 디렉터리 표시 및 왼쪽 파일 정보 수집 (모든 하위 디렉터리 포함 SearchOption.AllDirectories)
                if (hasLeft)
                {
                    // 최상위 디렉터리만 (<DIR>) 폴더 정보로 먼저 띄워줍니다. (보기 편하게)
                    foreach (var d in Directory.EnumerateDirectories(leftPath).Select(p => new DirectoryInfo(p)).OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(d.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(d.LastWriteTime.ToString("g"));
                        lvwLeftDir.Items.Add(item);
                    }
                    
                    // 하위 폴더들의 파일까지 전부 불러와서 Name 부분에는 상대경로로 저장합니다.
                    var allFiles = new DirectoryInfo(leftPath).GetFiles("*", SearchOption.AllDirectories);
                    foreach (var f in allFiles)
                    {
                        string relativePath = Path.GetRelativePath(leftPath, f.FullName);
                        leftFiles[relativePath] = f;
                    }
                }

                // 2. 디렉터리 표시 및 오른쪽 파일 정보 수집 (모든 하위 디렉터리 포함)
                if (hasRight)
                {
                    foreach (var d in Directory.EnumerateDirectories(rightPath).Select(p => new DirectoryInfo(p)).OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(d.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(d.LastWriteTime.ToString("g"));
                        lvwRightDir.Items.Add(item);
                    }
                    
                    var allFiles = new DirectoryInfo(rightPath).GetFiles("*", SearchOption.AllDirectories);
                    foreach (var f in allFiles)
                    {
                        string relativePath = Path.GetRelativePath(rightPath, f.FullName);
                        rightFiles[relativePath] = f;
                    }
                }

                // 3. 왼쪽 파일 목록 채우기 + 색상 결정
                if (hasLeft)
                {
                    foreach (var kvp in leftFiles.OrderBy(f => f.Key))
                    {
                        string relPath = kvp.Key;
                        FileInfo lf = kvp.Value;

                        var litem = new ListViewItem(relPath);
                        litem.SubItems.Add(FormatSizeInKb(lf.Length));
                        litem.SubItems.Add(lf.LastWriteTime.ToString("g"));

                        if (hasRight && rightFiles.TryGetValue(relPath, out var rf))
                        {
                            if (lf.LastWriteTime == rf.LastWriteTime) litem.ForeColor = Color.Black;        // 1단계: 동일 (검은색)
                            else if (lf.LastWriteTime > rf.LastWriteTime) litem.ForeColor = Color.Red;      // 2단계: 최신 (빨간색)
                            else litem.ForeColor = Color.Gray;                                              // 2단계: 과거 (회색)
                        }
                        else
                        {
                            litem.ForeColor = hasRight ? Color.Purple : Color.Black;                        // 3단계: 단독 파일 (보라색)
                        }
                        lvwLeftDir.Items.Add(litem);
                    }
                }

                // 4. 오른쪽 파일 목록 채우기 + 색상 결정
                if (hasRight)
                {
                    foreach (var kvp in rightFiles.OrderBy(f => f.Key))
                    {
                        string relPath = kvp.Key;
                        FileInfo rf = kvp.Value;

                        var ritem = new ListViewItem(relPath);
                        ritem.SubItems.Add(FormatSizeInKb(rf.Length));
                        ritem.SubItems.Add(rf.LastWriteTime.ToString("g"));

                        if (hasLeft && leftFiles.TryGetValue(relPath, out var lf))
                        {
                            if (rf.LastWriteTime == lf.LastWriteTime) ritem.ForeColor = Color.Black;        // 1단계: 동일 (검은색)
                            else if (rf.LastWriteTime > lf.LastWriteTime) ritem.ForeColor = Color.Red;      // 2단계: 최신 (빨간색)
                            else ritem.ForeColor = Color.Gray;                                              // 2단계: 과거 (회색)
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

        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 왼쪽 리스트 뷰 선택 시 동작
        }

        private void lvwRightDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 오른쪽 리스트 뷰 선택 시 동작
        }
    }
}

