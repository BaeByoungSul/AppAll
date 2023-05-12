namespace SapIF
{
    partial class SapIF1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.fp_t1_1 = new FarPoint.Win.Spread.FpSpread();
            this.fp_t1_1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMatnr_t1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLgort_t1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWerks_t1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fp_t2_1 = new FarPoint.Win.Spread.FpSpread();
            this.fp_t2_1_Sheet2 = new FarPoint.Win.Spread.SheetView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoBtn_t3_2 = new System.Windows.Forms.RadioButton();
            this.rdoBtn_t3_1 = new System.Windows.Forms.RadioButton();
            this.fp_t3_1 = new FarPoint.Win.Spread.FpSpread();
            this.fp_t3_1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1_Sheet1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t2_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t2_1_Sheet2)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t3_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t3_1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(150, 20);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(950, 600);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fp_t1_1);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(942, 572);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SAP 재고조회";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fp_t1_1
            // 
            this.fp_t1_1.AccessibleDescription = "fp_t1_1, Sheet1, Row 0, Column 0, ";
            this.fp_t1_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fp_t1_1.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fp_t1_1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fp_t1_1.Location = new System.Drawing.Point(3, 54);
            this.fp_t1_1.Name = "fp_t1_1";
            this.fp_t1_1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fp_t1_1_Sheet1});
            this.fp_t1_1.Size = new System.Drawing.Size(936, 515);
            this.fp_t1_1.TabIndex = 18;
            // 
            // fp_t1_1_Sheet1
            // 
            this.fp_t1_1_Sheet1.Reset();
            this.fp_t1_1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fp_t1_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fp_t1_1_Sheet1.ColumnCount = 12;
            fp_t1_1_Sheet1.RowCount = 1;
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "플랜트";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "저장위치";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "자재코드";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "자재명";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "배치번호";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "배치번호2";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "등급";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "단위";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "가용재고";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "품질검사재고";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "보류재고";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전중";
            this.fp_t1_1_Sheet1.ColumnHeader.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.fp_t1_1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fp_t1_1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.fp_t1_1_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.fp_t1_1_Sheet1.Columns.Get(0).Label = "플랜트";
            this.fp_t1_1_Sheet1.Columns.Get(0).Width = 75F;
            this.fp_t1_1_Sheet1.Columns.Get(1).Label = "저장위치";
            this.fp_t1_1_Sheet1.Columns.Get(1).Width = 67F;
            this.fp_t1_1_Sheet1.Columns.Get(2).Label = "자재코드";
            this.fp_t1_1_Sheet1.Columns.Get(2).Width = 95F;
            this.fp_t1_1_Sheet1.Columns.Get(3).Label = "자재명";
            this.fp_t1_1_Sheet1.Columns.Get(3).Width = 233F;
            this.fp_t1_1_Sheet1.Columns.Get(4).Label = "배치번호";
            this.fp_t1_1_Sheet1.Columns.Get(4).Width = 97F;
            this.fp_t1_1_Sheet1.Columns.Get(5).Label = "배치번호2";
            this.fp_t1_1_Sheet1.Columns.Get(5).Width = 135F;
            this.fp_t1_1_Sheet1.Columns.Get(6).Label = "등급";
            this.fp_t1_1_Sheet1.Columns.Get(6).Width = 49F;
            this.fp_t1_1_Sheet1.Columns.Get(7).Label = "단위";
            this.fp_t1_1_Sheet1.Columns.Get(7).Width = 57F;
            this.fp_t1_1_Sheet1.Columns.Get(8).Label = "가용재고";
            this.fp_t1_1_Sheet1.Columns.Get(8).Width = 93F;
            this.fp_t1_1_Sheet1.Columns.Get(9).Label = "품질검사재고";
            this.fp_t1_1_Sheet1.Columns.Get(9).Width = 93F;
            this.fp_t1_1_Sheet1.Columns.Get(10).Label = "보류재고";
            this.fp_t1_1_Sheet1.Columns.Get(10).Width = 93F;
            this.fp_t1_1_Sheet1.Columns.Get(11).Label = "이전중";
            this.fp_t1_1_Sheet1.Columns.Get(11).Width = 93F;
            this.fp_t1_1_Sheet1.DefaultStyle.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fp_t1_1_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.fp_t1_1_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.fp_t1_1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fp_t1_1_Sheet1.RowHeader.Columns.Get(0).Width = 53F;
            this.fp_t1_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtMatnr_t1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtLgort_t1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtWerks_t1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(936, 51);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "조회조건";
            // 
            // txtMatnr_t1
            // 
            this.txtMatnr_t1.Location = new System.Drawing.Point(539, 20);
            this.txtMatnr_t1.Name = "txtMatnr_t1";
            this.txtMatnr_t1.Size = new System.Drawing.Size(267, 21);
            this.txtMatnr_t1.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(483, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "자재코드";
            // 
            // txtLgort_t1
            // 
            this.txtLgort_t1.Location = new System.Drawing.Point(332, 20);
            this.txtLgort_t1.Name = "txtLgort_t1";
            this.txtLgort_t1.Size = new System.Drawing.Size(135, 21);
            this.txtLgort_t1.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(297, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "창고";
            // 
            // txtWerks_t1
            // 
            this.txtWerks_t1.Location = new System.Drawing.Point(85, 20);
            this.txtWerks_t1.Name = "txtWerks_t1";
            this.txtWerks_t1.Size = new System.Drawing.Size(137, 21);
            this.txtWerks_t1.TabIndex = 33;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 32;
            this.label1.Text = "플랜트";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.fp_t2_1);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(942, 572);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SAP생산실적";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // fp_t2_1
            // 
            this.fp_t2_1.AccessibleDescription = "fp_t1_1, Sheet1, Row 0, Column 0, ";
            this.fp_t2_1.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fp_t2_1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fp_t2_1.Location = new System.Drawing.Point(3, 80);
            this.fp_t2_1.Name = "fp_t2_1";
            this.fp_t2_1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fp_t2_1_Sheet2});
            this.fp_t2_1.Size = new System.Drawing.Size(984, 317);
            this.fp_t2_1.TabIndex = 20;
            this.fp_t2_1.SetActiveViewport(0, -1, -1);
            // 
            // fp_t2_1_Sheet2
            // 
            this.fp_t2_1_Sheet2.Reset();
            this.fp_t2_1_Sheet2.SheetName = "Sheet2";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fp_t2_1_Sheet2.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fp_t2_1_Sheet2.ColumnCount = 10;
            fp_t2_1_Sheet2.RowCount = 0;
            this.fp_t2_1_Sheet2.ActiveColumnIndex = -1;
            this.fp_t2_1_Sheet2.ActiveRowIndex = -1;
            this.fp_t2_1_Sheet2.RowHeader.Columns.Default.Resizable = false;
            this.fp_t2_1_Sheet2.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.fp_t3_1);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(942, 572);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SAP생산투입";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoBtn_t3_2);
            this.groupBox1.Controls.Add(this.rdoBtn_t3_1);
            this.groupBox1.Location = new System.Drawing.Point(11, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 57);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "선택";
            // 
            // rdoBtn_t3_2
            // 
            this.rdoBtn_t3_2.AutoSize = true;
            this.rdoBtn_t3_2.Location = new System.Drawing.Point(142, 26);
            this.rdoBtn_t3_2.Name = "rdoBtn_t3_2";
            this.rdoBtn_t3_2.Size = new System.Drawing.Size(95, 16);
            this.rdoBtn_t3_2.TabIndex = 1;
            this.rdoBtn_t3_2.Text = "기간오픈정보";
            this.rdoBtn_t3_2.UseVisualStyleBackColor = true;
            // 
            // rdoBtn_t3_1
            // 
            this.rdoBtn_t3_1.AutoSize = true;
            this.rdoBtn_t3_1.Checked = true;
            this.rdoBtn_t3_1.Location = new System.Drawing.Point(22, 26);
            this.rdoBtn_t3_1.Name = "rdoBtn_t3_1";
            this.rdoBtn_t3_1.Size = new System.Drawing.Size(71, 16);
            this.rdoBtn_t3_1.TabIndex = 0;
            this.rdoBtn_t3_1.TabStop = true;
            this.rdoBtn_t3_1.Text = "생산투입";
            this.rdoBtn_t3_1.UseVisualStyleBackColor = true;
            // 
            // fp_t3_1
            // 
            this.fp_t3_1.AccessibleDescription = "fp_t1_1, Sheet1, Row 0, Column 0, ";
            this.fp_t3_1.Font = new System.Drawing.Font("Gulim", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fp_t3_1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.fp_t3_1.Location = new System.Drawing.Point(3, 73);
            this.fp_t3_1.Name = "fp_t3_1";
            this.fp_t3_1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fp_t3_1_Sheet1});
            this.fp_t3_1.Size = new System.Drawing.Size(984, 361);
            this.fp_t3_1.TabIndex = 19;
            this.fp_t3_1.SetActiveViewport(0, -1, -1);
            // 
            // fp_t3_1_Sheet1
            // 
            this.fp_t3_1_Sheet1.Reset();
            this.fp_t3_1_Sheet1.SheetName = "Sheet2";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fp_t3_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fp_t3_1_Sheet1.ColumnCount = 10;
            fp_t3_1_Sheet1.RowCount = 0;
            this.fp_t3_1_Sheet1.ActiveColumnIndex = -1;
            this.fp_t3_1_Sheet1.ActiveRowIndex = -1;
            this.fp_t3_1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fp_t3_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(834, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "구분자(,)";
            // 
            // SapIF1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "SapIF1";
            this.Size = new System.Drawing.Size(950, 600);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1_Sheet1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fp_t2_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t2_1_Sheet2)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t3_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t3_1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private FarPoint.Win.Spread.FpSpread fp_t1_1;
        private FarPoint.Win.Spread.SheetView fp_t1_1_Sheet1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMatnr_t1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLgort_t1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWerks_t1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private FarPoint.Win.Spread.FpSpread fp_t2_1;
        private FarPoint.Win.Spread.SheetView fp_t2_1_Sheet2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoBtn_t3_2;
        private System.Windows.Forms.RadioButton rdoBtn_t3_1;
        private FarPoint.Win.Spread.FpSpread fp_t3_1;
        private FarPoint.Win.Spread.SheetView fp_t3_1_Sheet1;
        private System.Windows.Forms.Label label4;
    }
}
