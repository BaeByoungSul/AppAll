namespace FileTest
{
    partial class Etc01_Test
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
            this.txt_t1_2 = new System.Windows.Forms.TextBox();
            this.btn_t1_3 = new System.Windows.Forms.Button();
            this.txt_t1_1 = new System.Windows.Forms.TextBox();
            this.btn_t1_2 = new System.Windows.Forms.Button();
            this.btn_t1_1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ItemSize = new System.Drawing.Size(170, 20);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(950, 600);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.fp_t1_1);
            this.tabPage1.Controls.Add(this.txt_t1_2);
            this.tabPage1.Controls.Add(this.btn_t1_3);
            this.tabPage1.Controls.Add(this.txt_t1_1);
            this.tabPage1.Controls.Add(this.btn_t1_2);
            this.tabPage1.Controls.Add(this.btn_t1_1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(942, 572);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Assembly";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // fp_t1_1
            // 
            this.fp_t1_1.AccessibleDescription = "";
            this.fp_t1_1.Location = new System.Drawing.Point(27, 59);
            this.fp_t1_1.Name = "fp_t1_1";
            this.fp_t1_1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fp_t1_1_Sheet1});
            this.fp_t1_1.Size = new System.Drawing.Size(581, 357);
            this.fp_t1_1.TabIndex = 16;
            this.fp_t1_1.SetActiveViewport(0, -1, -1);
            // 
            // fp_t1_1_Sheet1
            // 
            this.fp_t1_1_Sheet1.Reset();
            this.fp_t1_1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.fp_t1_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            fp_t1_1_Sheet1.ColumnCount = 2;
            fp_t1_1_Sheet1.RowCount = 0;
            this.fp_t1_1_Sheet1.ActiveColumnIndex = -1;
            this.fp_t1_1_Sheet1.ActiveRowIndex = -1;
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "Assembly Name";
            this.fp_t1_1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "FullName";
            this.fp_t1_1_Sheet1.Columns.Get(0).Label = "Assembly Name";
            this.fp_t1_1_Sheet1.Columns.Get(0).Width = 159F;
            this.fp_t1_1_Sheet1.Columns.Get(1).Label = "FullName";
            this.fp_t1_1_Sheet1.Columns.Get(1).Width = 363F;
            this.fp_t1_1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.fp_t1_1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txt_t1_2
            // 
            this.txt_t1_2.Location = new System.Drawing.Point(614, 314);
            this.txt_t1_2.Multiline = true;
            this.txt_t1_2.Name = "txt_t1_2";
            this.txt_t1_2.Size = new System.Drawing.Size(244, 102);
            this.txt_t1_2.TabIndex = 14;
            // 
            // btn_t1_3
            // 
            this.btn_t1_3.Location = new System.Drawing.Point(692, 249);
            this.btn_t1_3.Name = "btn_t1_3";
            this.btn_t1_3.Size = new System.Drawing.Size(150, 44);
            this.btn_t1_3.TabIndex = 13;
            this.btn_t1_3.Text = "Assembly Class조회";
            this.btn_t1_3.UseVisualStyleBackColor = true;
            this.btn_t1_3.Click += new System.EventHandler(this.btn_t1_3_Click);
            // 
            // txt_t1_1
            // 
            this.txt_t1_1.Location = new System.Drawing.Point(614, 59);
            this.txt_t1_1.Multiline = true;
            this.txt_t1_1.Name = "txt_t1_1";
            this.txt_t1_1.Size = new System.Drawing.Size(244, 169);
            this.txt_t1_1.TabIndex = 12;
            // 
            // btn_t1_2
            // 
            this.btn_t1_2.Location = new System.Drawing.Point(745, 9);
            this.btn_t1_2.Name = "btn_t1_2";
            this.btn_t1_2.Size = new System.Drawing.Size(113, 44);
            this.btn_t1_2.TabIndex = 11;
            this.btn_t1_2.Text = "Assembly조회";
            this.btn_t1_2.UseVisualStyleBackColor = true;
            this.btn_t1_2.Click += new System.EventHandler(this.btn_t1_2_Click);
            // 
            // btn_t1_1
            // 
            this.btn_t1_1.Location = new System.Drawing.Point(31, 9);
            this.btn_t1_1.Name = "btn_t1_1";
            this.btn_t1_1.Size = new System.Drawing.Size(113, 44);
            this.btn_t1_1.TabIndex = 10;
            this.btn_t1_1.Text = "Assembly조회";
            this.btn_t1_1.UseVisualStyleBackColor = true;
            this.btn_t1_1.Click += new System.EventHandler(this.btn_t1_1_Click);
            // 
            // Etc01_Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Etc01_Test";
            this.Size = new System.Drawing.Size(950, 600);
            this.Load += new System.EventHandler(this.Etc01_Test_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fp_t1_1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private FarPoint.Win.Spread.FpSpread fp_t1_1;
        private FarPoint.Win.Spread.SheetView fp_t1_1_Sheet1;
        private System.Windows.Forms.TextBox txt_t1_2;
        private System.Windows.Forms.Button btn_t1_3;
        private System.Windows.Forms.TextBox txt_t1_1;
        private System.Windows.Forms.Button btn_t1_2;
        private System.Windows.Forms.Button btn_t1_1;
    }
}
