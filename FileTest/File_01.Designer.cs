namespace FileTest
{
    partial class File_01
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.txrElapsedTime = new System.Windows.Forms.TextBox();
            this.txt_fileName = new System.Windows.Forms.TextBox();
            this.btn_file = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.progBar_t1_2 = new System.Windows.Forms.ProgressBar();
            this.progBar_t1 = new System.Windows.Forms.ProgressBar();
            this.txtFileName_t1 = new System.Windows.Forms.TextBox();
            this.btn_t1_2 = new System.Windows.Forms.Button();
            this.btnDownload_t1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbo1 = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Download Files";
            // 
            // txrElapsedTime
            // 
            this.txrElapsedTime.Location = new System.Drawing.Point(633, 320);
            this.txrElapsedTime.Name = "txrElapsedTime";
            this.txrElapsedTime.ReadOnly = true;
            this.txrElapsedTime.Size = new System.Drawing.Size(117, 21);
            this.txrElapsedTime.TabIndex = 18;
            // 
            // txt_fileName
            // 
            this.txt_fileName.Location = new System.Drawing.Point(175, 18);
            this.txt_fileName.Name = "txt_fileName";
            this.txt_fileName.ReadOnly = true;
            this.txt_fileName.Size = new System.Drawing.Size(575, 21);
            this.txt_fileName.TabIndex = 17;
            // 
            // btn_file
            // 
            this.btn_file.Location = new System.Drawing.Point(40, 10);
            this.btn_file.Name = "btn_file";
            this.btn_file.Size = new System.Drawing.Size(129, 35);
            this.btn_file.TabIndex = 16;
            this.btn_file.Text = "Upload File 선택";
            this.btn_file.UseVisualStyleBackColor = true;
            this.btn_file.Click += new System.EventHandler(this.btn_file_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(150, 25);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(950, 600);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbo1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txt_fileName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btn_file);
            this.tabPage1.Controls.Add(this.progBar_t1_2);
            this.tabPage1.Controls.Add(this.txrElapsedTime);
            this.tabPage1.Controls.Add(this.progBar_t1);
            this.tabPage1.Controls.Add(this.txtFileName_t1);
            this.tabPage1.Controls.Add(this.btn_t1_2);
            this.tabPage1.Controls.Add(this.btnDownload_t1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(942, 567);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File Sync";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(576, 329);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "시간(초)";
            // 
            // progBar_t1_2
            // 
            this.progBar_t1_2.BackColor = System.Drawing.Color.BlueViolet;
            this.progBar_t1_2.Location = new System.Drawing.Point(40, 211);
            this.progBar_t1_2.Name = "progBar_t1_2";
            this.progBar_t1_2.Size = new System.Drawing.Size(710, 23);
            this.progBar_t1_2.TabIndex = 14;
            // 
            // progBar_t1
            // 
            this.progBar_t1.BackColor = System.Drawing.Color.BlueViolet;
            this.progBar_t1.Location = new System.Drawing.Point(40, 51);
            this.progBar_t1.Name = "progBar_t1";
            this.progBar_t1.Size = new System.Drawing.Size(710, 23);
            this.progBar_t1.TabIndex = 13;
            // 
            // txtFileName_t1
            // 
            this.txtFileName_t1.Location = new System.Drawing.Point(175, 169);
            this.txtFileName_t1.Name = "txtFileName_t1";
            this.txtFileName_t1.Size = new System.Drawing.Size(575, 21);
            this.txtFileName_t1.TabIndex = 11;
            // 
            // btn_t1_2
            // 
            this.btn_t1_2.Location = new System.Drawing.Point(40, 80);
            this.btn_t1_2.Name = "btn_t1_2";
            this.btn_t1_2.Size = new System.Drawing.Size(126, 41);
            this.btn_t1_2.TabIndex = 9;
            this.btn_t1_2.Text = "Upload";
            this.btn_t1_2.UseVisualStyleBackColor = true;
            this.btn_t1_2.Click += new System.EventHandler(this.btn_t1_2_Click);
            // 
            // btnDownload_t1
            // 
            this.btnDownload_t1.Location = new System.Drawing.Point(40, 240);
            this.btnDownload_t1.Name = "btnDownload_t1";
            this.btnDownload_t1.Size = new System.Drawing.Size(126, 41);
            this.btnDownload_t1.TabIndex = 10;
            this.btnDownload_t1.Text = "Download";
            this.btnDownload_t1.UseVisualStyleBackColor = true;
            this.btnDownload_t1.Click += new System.EventHandler(this.btnDownload_t1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 70);
            this.button1.TabIndex = 21;
            this.button1.Text = "Zebra Print";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbo1
            // 
            this.cbo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo1.FormattingEnabled = true;
            this.cbo1.Items.AddRange(new object[] {
            "Data Matrix",
            "QR Code"});
            this.cbo1.Location = new System.Drawing.Point(237, 349);
            this.cbo1.Name = "cbo1";
            this.cbo1.Size = new System.Drawing.Size(121, 20);
            this.cbo1.TabIndex = 22;
            // 
            // File_01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "File_01";
            this.Size = new System.Drawing.Size(950, 600);
            this.Load += new System.EventHandler(this.File_01_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txrElapsedTime;
        private System.Windows.Forms.TextBox txt_fileName;
        private System.Windows.Forms.Button btn_file;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ProgressBar progBar_t1;
        private System.Windows.Forms.TextBox txtFileName_t1;
        private System.Windows.Forms.Button btn_t1_2;
        private System.Windows.Forms.Button btnDownload_t1;
        private System.Windows.Forms.ProgressBar progBar_t1_2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbo1;
    }
}
