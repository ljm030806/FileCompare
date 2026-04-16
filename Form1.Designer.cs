namespace FileCompare
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            panel3 = new Panel();
            lvwLeftDir = new ListView();
            panel2 = new Panel();
            btnLeftDir = new Button();
            txtLeftDir = new TextBox();
            panel1 = new Panel();
            btnCopyFromLeft = new Button();
            lblAppname = new Label();
            panel6 = new Panel();
            lvwRightDir = new ListView();
            panel5 = new Panel();
            brnRightDir = new Button();
            txtRightDir = new TextBox();
            panel4 = new Panel();
            btnCopyFromRight = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(30, 30);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel6);
            splitContainer1.Panel2.Controls.Add(panel5);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(994, 466);
            splitContainer1.SplitterDistance = 491;
            splitContainer1.SplitterWidth = 10;
            splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(lvwLeftDir);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 200);
            panel3.Name = "panel3";
            panel3.Size = new Size(491, 266);
            panel3.TabIndex = 0;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwLeftDir.Location = new Point(0, 0);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(491, 266);
            lvwLeftDir.TabIndex = 0;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            lvwLeftDir.SelectedIndexChanged += lvwLeftDir_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnLeftDir);
            panel2.Controls.Add(txtLeftDir);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 100);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(10);
            panel2.Size = new Size(491, 100);
            panel2.TabIndex = 0;
            // 
            // btnLeftDir
            // 
            btnLeftDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnLeftDir.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLeftDir.Location = new Point(405, 39);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(75, 39);
            btnLeftDir.TabIndex = 2;
            btnLeftDir.Text = "폴더선택";
            btnLeftDir.UseVisualStyleBackColor = true;
            btnLeftDir.Click += btnLeftDir_Click;
            // 
            // txtLeftDir
            // 
            txtLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLeftDir.Location = new Point(13, 48);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(386, 23);
            txtLeftDir.TabIndex = 0;
            txtLeftDir.TextChanged += txtLeftDir_TextChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCopyFromLeft);
            panel1.Controls.Add(lblAppname);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new Padding(10);
            panel1.Size = new Size(491, 100);
            panel1.TabIndex = 0;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyFromLeft.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCopyFromLeft.Location = new Point(403, 33);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(75, 39);
            btnCopyFromLeft.TabIndex = 1;
            btnCopyFromLeft.Text = ">>>";
            btnCopyFromLeft.UseVisualStyleBackColor = true;
            btnCopyFromLeft.Click += btnCopyFromLeft_Click;
            // 
            // lblAppname
            // 
            lblAppname.AutoSize = true;
            lblAppname.Font = new Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblAppname.ForeColor = Color.Blue;
            lblAppname.Location = new Point(13, 23);
            lblAppname.Name = "lblAppname";
            lblAppname.Size = new Size(222, 45);
            lblAppname.TabIndex = 0;
            lblAppname.Text = "File Compare";
            // 
            // panel6
            // 
            panel6.Controls.Add(lvwRightDir);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 200);
            panel6.Name = "panel6";
            panel6.Size = new Size(493, 266);
            panel6.TabIndex = 0;
            // 
            // lvwRightDir
            // 
            lvwRightDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwRightDir.Location = new Point(0, 0);
            lvwRightDir.Name = "lvwRightDir";
            lvwRightDir.Size = new Size(493, 266);
            lvwRightDir.TabIndex = 0;
            lvwRightDir.UseCompatibleStateImageBehavior = false;
            lvwRightDir.SelectedIndexChanged += lvwRightDir_SelectedIndexChanged;
            // 
            // panel5
            // 
            panel5.Controls.Add(brnRightDir);
            panel5.Controls.Add(txtRightDir);
            panel5.Dock = DockStyle.Top;
            panel5.Location = new Point(0, 100);
            panel5.Name = "panel5";
            panel5.Size = new Size(493, 100);
            panel5.TabIndex = 0;
            // 
            // brnRightDir
            // 
            brnRightDir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            brnRightDir.Font = new Font("맑은 고딕", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 129);
            brnRightDir.Location = new Point(399, 39);
            brnRightDir.Name = "brnRightDir";
            brnRightDir.Size = new Size(75, 39);
            brnRightDir.TabIndex = 3;
            brnRightDir.Text = "폴더선택";
            brnRightDir.UseVisualStyleBackColor = true;
            brnRightDir.Click += brnRightDir_Click;
            // 
            // txtRightDir
            // 
            txtRightDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtRightDir.Location = new Point(14, 48);
            txtRightDir.Name = "txtRightDir";
            txtRightDir.Size = new Size(379, 23);
            txtRightDir.TabIndex = 1;
            txtRightDir.TextChanged += txtRightDir_TextChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnCopyFromRight);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(493, 100);
            panel4.TabIndex = 0;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCopyFromRight.Font = new Font("맑은 고딕", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCopyFromRight.Location = new Point(14, 33);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(75, 39);
            btnCopyFromRight.TabIndex = 2;
            btnCopyFromRight.Text = "<<<";
            btnCopyFromRight.UseVisualStyleBackColor = true;
            btnCopyFromRight.Click += btnCopyFromRight_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 526);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Padding = new Padding(30);
            Text = "File Compare v1.0";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel panel3;
        private Panel panel2;
        private Panel panel1;
        private Panel panel6;
        private Panel panel5;
        private Panel panel4;
        private Button btnCopyFromLeft;
        private Label lblAppname;
        private Button btnCopyFromRight;
        private TextBox txtLeftDir;
        private TextBox txtRightDir;
        private ListView lvwLeftDir;
        private Button btnLeftDir;
        private ListView lvwRightDir;
        private Button brnRightDir;
    }
}
