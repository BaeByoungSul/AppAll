using CommonLib;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyClientLib;
using System.IO;
using System.Xml;
using Models.Database;

namespace Huizhou
{
    public partial class Cost_02 : UserControl,IMainToolbar
    {
        #region IMyToolbar
        // toolbar button을 enable, disable하고 싶을 때
        public event MyBtnDelegate NotifyBtnChange;

        // status message를 나타내고 싶을 때 NotifyMsgChange?.invoke
        public event MyMsgDelegate NotifyMsgChange;

        private string[] _validBtnNames;
        public string[] ValidBtnNames
        {
            get { return _validBtnNames; }
            set
            {
                _validBtnNames = value;
                NotifyBtnChange?.Invoke(this, value);
            }
        }
        public void FormFunction(ToolbarEnum toolbar)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // 조회버튼
                if (toolbar == ToolbarEnum.Search)
                {
                    Search();
                }
                //                MessageBox.Show(toolbar.ToString());
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        #endregion 

        public Cost_02()
        {
            InitializeComponent();
        }

        private void Cost_02_Load(object sender, EventArgs e)
        {


            LoadControl();
            LoadGrid_t1_Sht1();
            LoadGrid_t1_Sht2();
            LoadGrid_t1_Sht3();

            LoadGrid_t2_Sht1();
            LoadGrid_t2_Sht2();
            LoadGrid_t2_Sht3();
            LoadGrid_t2_Sht4();

            LoadGrid_t3_Sht1();
            LoadGrid_t3_Sht2();

            LoadGrid_t4_Sht1();
            LoadGrid_t4_Sht2();
            LoadGrid_t4_Sht3();
            LoadGrid_t4_Sht4();

            LoadGrid_t5_Sht1();
            LoadGrid_t5_Sht2();


            LoadGrid_t6_Sht1();
            LoadGrid_t6_Sht2();
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            ValidBtnNames = new string[] { "Refresh", "Search" };
            NotifyMsgChange?.Invoke(this, "Load completed");
        }

        #region Load Control, Load Grid
        private bool LoadControl()
        {

            // txt.. 기본값...
            dtp_t1_c_ym.Format = DateTimePickerFormat.Custom;
            dtp_t1_c_ym.CustomFormat = "yyyyy-MM"; // this line gives you only the month and year.
            dtp_t1_c_ym.ShowUpDown = true;

            return true;
        }
        private bool LoadGrid_t1_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t1_1;
            int iSht = 0;
            int iCol = 0;
            int colindex;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[iSht].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY1"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY2"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;




            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);
            return true;


        }
        private bool LoadGrid_t1_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t1_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TSPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY1"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY2"; iCol++;
            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            return true;


        }
        private bool LoadGrid_t1_Sht3()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t1_1;
            int iSht = 2;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_CD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TSPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY1"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY2"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["QTY1"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["QTY2"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);


            return true;


        }

        private bool LoadGrid_t2_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t2_1;
            int iSht = 0;
            int iCol = 0;
            int colindex;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[iSht].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT1"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT2"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            colindex = fp.Sheets[iSht].Columns["AMT1"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["AMT2"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;




            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["AMT1"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["AMT2"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);
            return true;


        }
        private bool LoadGrid_t2_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t2_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT1"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT2"; iCol++;
            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            colindex = fp.Sheets[iSht].Columns["AMT1"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["AMT2"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["AMT1"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["AMT2"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            return true;


        }
        private bool LoadGrid_t2_Sht3()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t2_1;
            int iSht = 2;
            int iCol = 0;
            //int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "P111_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "P112_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "Q111_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "P111_AMT_B"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "P112_AMT_B"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "Q111_AMT_B"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_B"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                //fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            for (int i = 3; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].CellType = num;
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                if (i >= 3 && i <= 6) fp.Sheets[iSht].Columns[i].BackColor = Color.Beige;
            }



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            for (int i = 3; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);

            }


            return true;


        }
        private bool LoadGrid_t2_Sht4()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t2_1;
            int iSht = 3;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TCC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "FCC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                //fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            colindex = fp.Sheets[iSht].Columns["AMT_A"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["AMT_A"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);


            return true;


        }
        private bool LoadGrid_t3_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t3_1;
            int iSht = 0;
            int iCol = 0;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_AMT_A"; iCol++;

            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_AMT"; iCol++;



            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            for (int i = 5; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
                if (i >= 5 && i <= 12) fp.Sheets[iSht].Columns[i].BackColor = Color.Beige;
            }



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = 5; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }


            return true;


        }
        private bool LoadGrid_t3_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t3_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_CD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TSPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };
            colindex = fp.Sheets[iSht].Columns["QTY_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
            }
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;
            fp.Sheets[iSht].Columns[colindex + 1].BackColor = Color.Beige;



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }



            return true;


        }
        private bool LoadGrid_t4_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 0;
            int iCol = 0;
            int colindex = -1;
            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.

            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "COSTACC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "COSTACC_DESC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G70_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G80_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G90_A"; iCol++;

            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G70"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G80"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_G90"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            colindex = fp.Sheets[iSht].Columns["MFG_AMT_G70_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
            }
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;
            fp.Sheets[iSht].Columns[colindex + 1].BackColor = Color.Beige;
            fp.Sheets[iSht].Columns[colindex + 2].BackColor = Color.Beige;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }


            return true;


        }
        private bool LoadGrid_t4_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "COSTACC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };
            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;





            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }



            return true;


        }
        private bool LoadGrid_t4_Sht3()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 2;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };
            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;





            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }



            return true;


        }
        private bool LoadGrid_t4_Sht4()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 3;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "COSTACC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MFG_AMT"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };
            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;





            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;

            colindex = fp.Sheets[iSht].Columns["MFG_AMT_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }



            return true;


        }
        private bool LoadGrid_t5_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t5_1;
            int iSht = 0;
            int iCol = 0;
            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_AMT_A"; iCol++;

            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "BEGIN_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "IPGO_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CHGO_AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "END_AMT"; iCol++;



            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };


            for (int i = 5; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
                if (i >= 5 && i <= 12) fp.Sheets[iSht].Columns[i].BackColor = Color.Beige;
            }



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = 5; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }


            return true;


        }
        private bool LoadGrid_t5_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t5_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;


            iCol = 0;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CITEM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CTYPE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_CD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TSPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT"; iCol++;


            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;

            }

            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };
            colindex = fp.Sheets[iSht].Columns["QTY_A"].Index;
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
            }
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;
            fp.Sheets[iSht].Columns[colindex + 1].BackColor = Color.Beige;



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }



            return true;


        }
        private bool LoadGrid_t6_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t6_1;
            int iSht = 0;
            int iCol = 0;
            int colindex;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK2"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODENM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODENM_E"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            colindex = fp.Sheets[iSht].Columns["AMT_A"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;


            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;


            colindex = fp.Sheets[iSht].Columns["ACODENM"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            colindex = fp.Sheets[iSht].Columns["ACODENM_E"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;


            return true;


        }
        private bool LoadGrid_t6_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t6_1;
            int iSht = 1;
            int iCol = 0;
            int colindex;


            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACCNO_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK2"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT_A"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "AMT"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODENM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODENM_E"; iCol++;

            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[iSht].Columns.Count; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[iSht].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[iSht].Columns[i].Locked = true;
                fp.Sheets[iSht].Columns[i].Tag = fp.Sheets[iSht].Columns[i].DataField;
                fp.Sheets[iSht].Columns[i].CellType = textCell;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].BackColor = Color.Beige;


            colindex = fp.Sheets[iSht].Columns["ACODENM"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            colindex = fp.Sheets[iSht].Columns["ACODENM_E"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;


            return true;


        }


        #endregion Load Control, Load Grid




        #region  TabControl Event
        /// <summary>
        /// 선택 탭 페이지가 바뀔 때
        /// Mdi Main Toolbar의 Button속성을 바꿔라는 Event를 발생 시킨다
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FunctNameEnum 참조
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    ValidBtnNames = new string[] { "Refresh", "Search" };
                    break;
                default:
                    break;
            }


        }
        #endregion  TabControl Event

        #region 툴바 CRUD
        private void Search()
        {
            try
            {
                if (tabControl1.SelectedIndex == 0) Search_T1();
                else if (tabControl1.SelectedIndex == 1) Search_T2();
                else if (tabControl1.SelectedIndex == 2) Search_T3();
                else if (tabControl1.SelectedIndex == 3) Search_T4();
                else if (tabControl1.SelectedIndex == 4) Search_T5();
                else if (tabControl1.SelectedIndex == 5) Search_T6();


            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool Search_T1()
        {

            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T1_SEL]  " + Environment.NewLine;
            _sql += "  @YYMM          = '" + dtp_t1_c_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
            _sql += "  @GAP_DATA_FLAG = '" + (chk_t1_1.Checked ? "Y" : "N") + "' ";

            var _cmd = new MyCommand() //("MST", "HUIZHOU", (int)CommandType.Text, _sql);
            {
                CommandName = "MST",
                ConnectionName = "HUIZHOU",
                CommandType = (int)CommandType.Text,
                CommandText = _sql
            };
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.fp_t1_1.Sheets[0].RowCount = 0;
                this.fp_t1_1.Sheets[1].RowCount = 0;
                this.fp_t1_1.Sheets[2].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);
                if (rtn.ReturnCD == "S")
                {
                    this.fp_t1_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                    this.fp_t1_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];
                    this.fp_t1_1.Sheets[2].DataSource = rtn.ReturnDs.Tables[2];
                }
             


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return true;

        }

        private bool Search_T2()
        {

            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T2_SEL]  " + Environment.NewLine;
            _sql += "  @YYMM          = '" + dtp_t2_c_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
            _sql += "  @GAP_DATA_FLAG = '" + (chk_t2_1.Checked ? "Y" : "N") + "' ";

            var _cmd = new MyCommand() //("MST", "HUIZHOU", (int)CommandType.Text, _sql);
            {
                CommandName = "MST",
                ConnectionName = "HUIZHOU",
                CommandType = (int)CommandType.Text,
                CommandText = _sql
            };
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.fp_t2_1.Sheets[0].RowCount = 0;
                this.fp_t2_1.Sheets[1].RowCount = 0;
                this.fp_t2_1.Sheets[2].RowCount = 0;
                this.fp_t2_1.Sheets[3].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);


                if (rtn.ReturnCD == "S")
                {
                    this.fp_t2_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                    this.fp_t2_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];
                    this.fp_t2_1.Sheets[2].DataSource = rtn.ReturnDs.Tables[2];
                    this.fp_t2_1.Sheets[3].DataSource = rtn.ReturnDs.Tables[3];
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return true;

        }
        private bool Search_T3()
        {
            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T3_SEL]  " + Environment.NewLine;
            _sql += "  @YYMM          = '" + dtp_t3_c_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
            _sql += "  @GAP_DATA_FLAG = '" + (chk_t3_1.Checked ? "Y" : "N") + "' ";

            var _cmd = new MyCommand() //("MST", "HUIZHOU", (int)CommandType.Text, _sql);
            {
                CommandName = "MST",
                ConnectionName = "HUIZHOU",
                CommandType = (int)CommandType.Text,
                CommandText = _sql
            };
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.fp_t3_1.Sheets[0].RowCount = 0;
                this.fp_t3_1.Sheets[1].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);
                if (rtn.ReturnCD == "S")
                {
                    this.fp_t3_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                    this.fp_t3_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return true;

        }
        private bool Search_T4()
        {

            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T4_SEL]  " + Environment.NewLine;
            _sql += "  @YYMM          = '" + dtp_t4_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
            _sql += "  @GAP_DATA_FLAG = '" + (chk_t4_1.Checked ? "Y" : "N") + "' ";

            var _cmd = new MyCommand()//("MST", "HUIZHOU", (int)CommandType.Text, _sql);
            {
                CommandName = "MST",
                ConnectionName = "HUIZHOU",
                CommandType = (int)CommandType.Text,
                CommandText = _sql
            };
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.fp_t4_1.Sheets[0].RowCount = 0;
                this.fp_t4_1.Sheets[1].RowCount = 0;
                this.fp_t4_1.Sheets[2].RowCount = 0;
                this.fp_t4_1.Sheets[3].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t4_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                this.fp_t4_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];
                this.fp_t4_1.Sheets[2].DataSource = rtn.ReturnDs.Tables[2];
                this.fp_t4_1.Sheets[3].DataSource = rtn.ReturnDs.Tables[3];
                




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }


            return true;

        }
        private bool Search_T5()
        {
            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T5_SEL]  " + Environment.NewLine;
            _sql += "  @YYMM          = '" + dtp_t5_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
            _sql += "  @GAP_DATA_FLAG = '" + (chk_t5_1.Checked ? "Y" : "N") + "' ";

            var _cmd = new MyCommand() //("MST", "HUIZHOU", (int)CommandType.Text, _sql);
            {
                CommandName = "MST",
                ConnectionName = "HUIZHOU",
                CommandType = (int)CommandType.Text,
                CommandText = _sql
            };
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.fp_t5_1.Sheets[0].RowCount = 0;
                this.fp_t5_1.Sheets[1].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t5_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                this.fp_t5_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];

                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return true;

        }
        private bool Search_T6()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T6_SEL1] ";
                _sql += " @YYMM  = '" + dtp_t6_ym.Value.ToString("yyyyMM") + "', ";
                _sql += "  @GAP_DATA_FLAG = '" + (chk_t6_1.Checked ? "Y" : "N") + "' ";

                var _cmd = new MyCommand() {
                    CommandName = "MST",
                    ConnectionName = "HUIZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = _sql
                };

                this.fp_t6_1.Sheets[0].RowCount = 0;
                var rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t6_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];

                _sql = " EXECUTE ZBBS2..[USP_BCOST_0020_T6_SEL2] ";
                _sql += " @YYMM  = '" + dtp_t6_ym.Value.ToString("yyyyMM") + "', ";
                _sql += "  @GAP_DATA_FLAG = '" + (chk_t6_1.Checked ? "Y" : "N") + "' ";

                _cmd = new MyCommand() {
                    CommandName = "MST",
                    ConnectionName = "HUIZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = _sql
                };

                this.fp_t6_1.Sheets[1].RowCount = 0;
                rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t6_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[0];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return true;

        }

        #endregion

        #region 기타 함수 및 이벤트
        private void dtp_t1_c_ym_ValueChanged(object sender, EventArgs e)
        {

            //DateTime lastDayOfMonth = new DateTime(
            //    dtp_t1_c_ym.Value.Year, dtp_t1_c_ym.Value.Month, DateTime.DaysInMonth(dtp_t1_c_ym.Value.Year, dtp_t1_c_ym.Value.Month));

            DateTime dateTime = dtp_t1_c_ym.Value;
            dtp_t2_c_ym.Value = dateTime;
            dtp_t3_c_ym.Value = dateTime;
            dtp_t4_ym.Value = dateTime;
            dtp_t5_ym.Value = dateTime;
            dtp_t6_ym.Value = dateTime;


        }

        #endregion

    }
}
