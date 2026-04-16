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

            // 대상 폴더가 이미 존재하는 경우 날짜 비교
            if (Directory.Exists(destinationDir))
            {
                var targetDirInfo = new DirectoryInfo(destinationDir);

                // 복사할 원본 폴더가 대상 쪽 폴더보다 시간상 더 오래된(과거의) 경우
                if (dir.LastWriteTime < targetDirInfo.LastWriteTime)
                {
                    DialogResult result = MessageBox.Show(
                        $"원본 폴더 '{dir.Name}' 은(는) 대상 쪽의 폴더보다 오래된 폴더입니다.\n정말 이 폴더 내용을 덮어쓰고 복사하시겠습니까?",
                        "복사 확인",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    // 사용자가 No를 클릭하면 이 폴더 안의 내용 전체 건너뛰기
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                // 목적지 폴더가 없다면 생성
                Directory.CreateDirectory(destinationDir);
            }

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
                var leftFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
                var rightFiles = new Dictionary<string, FileInfo>(StringComparer.OrdinalIgnoreCase);
                var leftDirs = new Dictionary<string, DirectoryInfo>(StringComparer.OrdinalIgnoreCase);
                var rightDirs = new Dictionary<string, DirectoryInfo>(StringComparer.OrdinalIgnoreCase);

                // 1. 왼쪽 정보 수집 (현재 폴더 깊이에서만: 1-Depth)
                if (hasLeft)
                {
                    leftDirs = new DirectoryInfo(leftPath).GetDirectories().ToDictionary(d => d.Name, StringComparer.OrdinalIgnoreCase);
                    leftFiles = new DirectoryInfo(leftPath).GetFiles().ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
                }

                // 2. 오른쪽 정보 수집 (현재 폴더 깊이에서만: 1-Depth)
                if (hasRight)
                {
                    rightDirs = new DirectoryInfo(rightPath).GetDirectories().ToDictionary(d => d.Name, StringComparer.OrdinalIgnoreCase);
                    rightFiles = new DirectoryInfo(rightPath).GetFiles().ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
                }

                // 3. 왼쪽 리스트 뷰 채우기 + 색상 결정
                if (hasLeft)
                {
                    // 최상위 그룹 1: 폴더 항목을 하나의 묶음(<DIR>)처럼 표시
                    foreach (var ld in leftDirs.Values.OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(ld.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(ld.LastWriteTime.ToString("g"));

                        if (hasRight && rightDirs.TryGetValue(ld.Name, out var rd))
                        {
                            if (ld.LastWriteTime == rd.LastWriteTime) item.ForeColor = Color.Black;        // 1단계: 동일 (검은색)
                            else if (ld.LastWriteTime > rd.LastWriteTime) item.ForeColor = Color.Red;      // 2단계: 최신 폴더 (빨간색)
                            else item.ForeColor = Color.Gray;                                              // 2단계: 과거 폴더 (회색)
                        }
                        else
                        {
                            item.ForeColor = hasRight ? Color.Purple : Color.Black;                        // 3단계: 단독 폴더 (보라색)
                        }
                        lvwLeftDir.Items.Add(item);
                    }

                    // 최상위 그룹 2: 일반 파일 항목 표시
                    foreach (var lf in leftFiles.Values.OrderBy(f => f.Name))
                    {
                        var item = new ListViewItem(lf.Name);
                        item.SubItems.Add(FormatSizeInKb(lf.Length));
                        item.SubItems.Add(lf.LastWriteTime.ToString("g"));

                        if (hasRight && rightFiles.TryGetValue(lf.Name, out var rf))
                        {
                            if (lf.LastWriteTime == rf.LastWriteTime) item.ForeColor = Color.Black;
                            else if (lf.LastWriteTime > rf.LastWriteTime) item.ForeColor = Color.Red;
                            else item.ForeColor = Color.Gray;
                        }
                        else
                        {
                            item.ForeColor = hasRight ? Color.Purple : Color.Black;
                        }
                        lvwLeftDir.Items.Add(item);
                    }
                }

                // 4. 오른쪽 리스트 뷰 채우기 + 색상 결정
                if (hasRight)
                {
                    // 최상위 그룹 1: 폴더 항목을 하나의 묶음(<DIR>)처럼 표시
                    foreach (var rd in rightDirs.Values.OrderBy(d => d.Name))
                    {
                        var item = new ListViewItem(rd.Name);
                        item.SubItems.Add("<DIR>");
                        item.SubItems.Add(rd.LastWriteTime.ToString("g"));

                        if (hasLeft && leftDirs.TryGetValue(rd.Name, out var ld))
                        {
                            if (rd.LastWriteTime == ld.LastWriteTime) item.ForeColor = Color.Black;
                            else if (rd.LastWriteTime > ld.LastWriteTime) item.ForeColor = Color.Red;
                            else item.ForeColor = Color.Gray;
                        }
                        else
                        {
                            item.ForeColor = hasLeft ? Color.Purple : Color.Black;
                        }
                        lvwRightDir.Items.Add(item);
                    }

                    // 최상위 그룹 2: 일반 파일 항목 표시
                    foreach (var rf in rightFiles.Values.OrderBy(f => f.Name))
                    {
                        var item = new ListViewItem(rf.Name);
                        item.SubItems.Add(FormatSizeInKb(rf.Length));
                        item.SubItems.Add(rf.LastWriteTime.ToString("g"));

                        if (hasLeft && leftFiles.TryGetValue(rf.Name, out var lf))
                        {
                            if (rf.LastWriteTime == lf.LastWriteTime) item.ForeColor = Color.Black;
                            else if (rf.LastWriteTime > lf.LastWriteTime) item.ForeColor = Color.Red;
                            else item.ForeColor = Color.Gray;
                        }
                        else
                        {
                            item.ForeColor = hasLeft ? Color.Purple : Color.Black;
                        }
                        lvwRightDir.Items.Add(item);
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

