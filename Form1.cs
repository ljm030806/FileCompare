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

            try
            {
                // 왼쪽(Left) 폴더 전체를 오른쪽(Right)으로 복사
                CopyDirectory(leftPath, rightPath);

             

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

            try
            {
                // 오른쪽(Right) 폴더 전체를 왼쪽(Left)으로 복사
                CopyDirectory(rightPath, leftPath);

                // 복사가 완료되면 결과를 바로 양측에 반영
                UpdateAndCompareListViews();
            }
            catch (Exception ex)
            {
                MessageBox.Show("복사 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ▼ 폴더와 그 내부의 모든 파일, 하위 폴더들을 재귀적으로 복사하는 헬퍼 메서드
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) return;

            // 목적지 폴더가 없다면 생성
            Directory.CreateDirectory(destinationDir);

            // 1. 현재 폴더 안의 모든 파일을 복사 (덮어쓰기 허용)
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);

                // 원본 파일이 복사될 대상 쪽에 이미 존재하는 경우 날짜 비교
                if (File.Exists(targetFilePath))
                {
                    FileInfo targetFile = new FileInfo(targetFilePath);

                    // 복사할 원본 파일이 대상 파일보다 시간상 더 오래된(과거의) 파일인 경우
                    if (file.LastWriteTime < targetFile.LastWriteTime)
                    {
                        DialogResult result = MessageBox.Show(
                            $"원본 파일 '{file.Name}' 은(는) 대상 폴더의 파일보다 오래된 파일입니다.\n정말 최신 파일을 덮어쓰고 복사하시겠습니까?",
                            "복사 확인",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        // 사용자가 No를 클릭하면 이 파일은 건너뛰기
                        if (result == DialogResult.No)
                        {
                            continue;
                        }
                    }
                }

                file.CopyTo(targetFilePath, true);
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

