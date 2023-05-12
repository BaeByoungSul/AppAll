using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using CommonLib;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread;
using MyClientLib;
using Models.Database;

namespace Suzhou
{
    public partial class Cost_01: UserControl, IMainToolbar
    {
        #region IMainToolbar
        // toolbar button을 enable, disable하고 싶을 때
        public event MyBtnDelegate NotifySetToolbar;
        // status message를 나타내고 싶을 때
        public event MyMsgDelegate NotifySetMessage;

        private string[] _validToolbarButtons;
        public string[] ValidToolbarButtons
        {
            get { return _validToolbarButtons; }
            set
            {
                _validToolbarButtons = value;
                NotifySetToolbar?.Invoke(this, value);
            }
        }
        public void ToolbarClick(ToolbarEnum toolbar)
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

        public Cost_01()
        {
            InitializeComponent();
            this.Load += Cost_01_Load;
        }

        #region Load Control, Load Grid
        private void Cost_01_Load(object sender, EventArgs e)
        {

            LoadGrid_t2_Sht1();
            LoadGrid_t2_Sht2();
            LoadGrid_t3_1();
            LoadGrid_t4_Sht1();
            LoadGrid_t4_Sht2();

            LoadGrid_t4_Sht3();
            LoadGrid_t4_Sht4();

            LoadGrid_t5_Sht1();
            LoadGrid_t5_Sht2();

            LoadGrid_t6_Sht1();
            LoadGrid_t6_Sht2();

            LoadGrid_t7_Sht1();
            LoadGrid_t7_Sht2();

            Search_Load_T3();

            dtp_t3_ym.ValueChanged += Dtp_t3_ym_ValueChanged;
            SetControl_Value();

            btn_t1_1.Click += Btn_t1_1_Click;
            btn_t1_copy.Click += Btn_t1_copy_Click;

            fp_t3_1.CellClick += Fp_t3_1_CellClick;
            bnt_t3_1.Click += Bnt_t3_1_Click;

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            ValidToolbarButtons = new string[] { "Refresh" };
            NotifySetMessage?.Invoke(this, "Load completed");
        }

        private void SetControl_Value()
        {

            dtp_t1_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t1_ym2.Value = DateTime.Today.AddMonths(-1);
            dtp_t2_c_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t3_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t4_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t5_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t6_ym.Value = DateTime.Today.AddMonths(-1);
            dtp_t7_ym.Value = DateTime.Today.AddMonths(-1);

            this.dtp_t3_ymd.Enabled = false;


        }
        private bool LoadGrid_t2_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t2_1;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fp.ActiveSheet = fp.Sheets[0];


            fp.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;


            //  fp.ActiveSheet.ColumnHeaderVisible = true;
            // FarPoint.Win.Spread.HideRowFilter hideRowFilter = new FarPoint.Win.Spread.HideRowFilter(fp.ActiveSheet);
            // fp.ActiveSheet.Columns[4].AllowAutoFilter = true;
            //fp.ActiveSheet.AutoFilterMode = FarPoint.Win.Spread.AutoFilterMode.FilterGadget;

            fp.Sheets[0].DataAutoSizeColumns = false;
            fp.Sheets[0].AutoGenerateColumns = false;
            fp.Sheets[0].DataAutoCellTypes = false;
            fp.Sheets[0].Rows.Default.Height = 25;
            //fp.Sheets[0].ColumnHeader.Rows.Default.Height = 25;
            fp.Sheets[0].ColumnHeader.Rows[0].Height = 25;
            fp.Sheets[0].ColumnHeader.Rows[1].Height = 25;

            fp.Sheets[0].RowCount = 0;

            fp.Sheets[0].FrozenColumnCount = 6;

            // Set the fields for the columns.
            int iCol = 0;
            fp.Sheets[0].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "ITEMCLASS"; iCol++;

            fp.Sheets[0].Columns[iCol].DataField = "BEGIN_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "IPGO_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "CHGO_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "END_QTY"; iCol++;

            fp.Sheets[0].Columns[iCol].DataField = "PROD_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "BUY1_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "BUY2_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "IPGO_BL_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "IPGO_REPACK_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "IPGO_ETC_QTY"; iCol++;

            fp.Sheets[0].Columns[iCol].DataField = "SALE_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "SAMPLE_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "INPUT_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "CHGO_BL_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "CHGO_REPACK_QTY"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "CHGO_ETC_QTY"; iCol++;


            //fp.Sheets[0].ColumnFooter.Columns[12].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            //fp.Sheets[0].ColumnFooter.Cells[0, 12].RowSpan = 2;
            //fp.Sheets[0].ColumnFooter.Cells[0, 0].Value = "test";


            for (int i = 5; i <= 8; i++)
            {
                fp.Sheets[0].Columns[i].BackColor = Color.Beige;
            }

            // Cell Alignment, Edit false
            for (int i = 0; i < fp.Sheets[0].Columns.Count; i++)
            {
                fp.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[0].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[0].Columns[i].Locked = true;
                fp.Sheets[0].Columns[i].Tag = fp.Sheets[0].Columns[i].DataField;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            for (int i = 5; i <= fp.Sheets[0].ColumnCount - 1; i++)
            {
                fp.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[0].Columns[i].CellType = num;
            }



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[0].ColumnFooter.Visible = true;
            fp.Sheets[0].ColumnFooter.RowCount = 1;
            fp.Sheets[0].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = 5; i <= fp.Sheets[0].ColumnCount - 1; i++)
            {
                fp.Sheets[0].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[0].ColumnFooter.Cells[0, i].CellType = num;
                fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
                // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            }

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
            fp.Sheets[iSht].Columns[iCol].DataField = "PROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SPROCESS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCLASS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_CD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY"; iCol++;

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

            colindex = fp.Sheets[iSht].Columns["QTY"].Index;
            //colindex = fp.Sheets[iSht].GetColumnFromTag(null, "ACODE_DESC").Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;



            return true;


        }
        private bool LoadGrid_t3_1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t3_1;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fp.ActiveSheet = fp.Sheets[0];


            fp.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[0].DataAutoSizeColumns = false;
            fp.Sheets[0].AutoGenerateColumns = false;
            fp.Sheets[0].DataAutoCellTypes = false;
            fp.Sheets[0].Rows.Default.Height = 25;
            //fp.Sheets[0].ColumnHeader.Rows.Default.Height = 25;
            fp.Sheets[0].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[0].RowCount = 0;


            // Set the fields for the columns.
            int iCol = 0;
            fp.Sheets[0].Columns[iCol].DataField = "CHK"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "STEP_DESC"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "PROCD"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "EXEC_SQL"; iCol++;

            // Cell Alignment, Edit false
            for (int i = 0; i < fp.Sheets[0].Columns.Count; i++)
            {
                fp.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                fp.Sheets[0].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[0].Columns[i].Locked = true;
                fp.Sheets[0].Columns[i].Tag = fp.Sheets[0].Columns[i].DataField;
            }


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 2,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            CheckBoxCellType checkBox = new CheckBoxCellType()
            {
                ThreeState = false
            };
            fp.Sheets[0].Columns[0].Locked = false;
            fp.Sheets[0].Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            fp.Sheets[0].Columns[0].CellType = checkBox;


            fp.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;


            TextCellType textCell = new TextCellType()
            {
                Multiline = true
            };
            fp.Sheets[0].Columns[3].Locked = false;
            fp.Sheets[0].Columns[3].CellType = textCell;


            return true;


        }
        private bool LoadGrid_t4_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 0;
            int iCol = 0;
            int colindex;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fp.ActiveSheet = fp.Sheets[0];

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
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE_DESC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CC_DESC"; iCol++;
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


            colindex = fp.Sheets[iSht].Columns["ACODE_DESC"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[iSht].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);




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
            fp.Sheets[iSht].Columns[iCol].DataField = "CC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "CC_DESC"; iCol++;
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
            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;


            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";

            colindex = fp.Sheets[iSht].Columns["AMT"].Index;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);


            return true;


        }
        private bool LoadGrid_t4_Sht3()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 2;
            int iCol = 0;
            int colindex;


            ////////////////////////////////////////////////////////
            /// Sheet3 
            ////////////////////////////////////////////////////////

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
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE_DESC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "C111_AMT"; iCol++;

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

            colindex = fp.Sheets[iSht].Columns["ACODE_DESC"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;

            colindex = fp.Sheets[iSht].Columns["C111_AMT"].Index;
            fp.Sheets[iSht].Columns[colindex].CellType = num;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;





            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            fp.Sheets[iSht].ColumnFooter.Cells[0, colindex].CellType = num;
            fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, colindex, FarPoint.Win.Spread.Model.AggregationType.Sum);


            return true;


        }
        private bool LoadGrid_t4_Sht4()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t4_1;
            int iSht = 3;
            int iCol = 0;

            fp.Sheets[iSht].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[iSht].DataAutoSizeColumns = false;
            fp.Sheets[iSht].AutoGenerateColumns = false;
            fp.Sheets[iSht].DataAutoCellTypes = false;
            fp.Sheets[iSht].Rows.Default.Height = 25;
            fp.Sheets[iSht].ColumnHeader.Rows[0].Height = 25;

            fp.Sheets[iSht].RowCount = 0;

            // Set the fields for the columns.
            fp.Sheets[iSht].Columns[iCol].DataField = "TCC"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "YYMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ACODE"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "SEMOK"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TOT_AMT"; iCol++;

            for (int i = 1; i < 31; i++, iCol++)
            {
                fp.Sheets[iSht].Columns[iCol].DataField = i.ToString();
            }



            // Cell Alignment, Edit false
            TextCellType textCell = new TextCellType();
            for (int i = 0; i < fp.Sheets[0].Columns.Count; i++)
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




            for (int i = 4; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;
                if (i > 4) fp.Sheets[iSht].Columns[i].Visible = false;
            }



            // Show the column footer.
            // 첫번째 행이 null 일 경우 합계가 되지 않음
            fp.Sheets[iSht].ColumnFooter.Visible = true;
            fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            //fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            for (int i = 4; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
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
            fp.ActiveSheet = fp.Sheets[0];

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
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCLASS"; iCol++;
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
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCLASS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_GB"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "TRANS_CD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "UCOST"; iCol++;
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

            int colindex = fp.Sheets[iSht].Columns["QTY"].Index;


            for (int i = colindex; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            {
                fp.Sheets[iSht].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[iSht].Columns[i].CellType = num;

            }



            //// Show the column footer.
            //// 첫번째 행이 null 일 경우 합계가 되지 않음
            //fp.Sheets[iSht].ColumnFooter.Visible = true;
            //fp.Sheets[iSht].ColumnFooter.RowCount = 1;
            //fp.Sheets[iSht].ColumnFooter.DefaultStyle.ForeColor = Color.Purple;
            ////fp.Sheets[0].ColumnFooter.Cells[0, 5].Value = "합계";
            //for (int i = 4; i <= fp.Sheets[iSht].ColumnCount - 1; i++)
            //{
            //    fp.Sheets[iSht].ColumnFooter.Cells[0, i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            //    fp.Sheets[iSht].ColumnFooter.Cells[0, i].CellType = num;
            //    fp.Sheets[iSht].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Sum);
            //    // fp.Sheets[0].ColumnFooter.SetAggregationType(0, i, FarPoint.Win.Spread.Model.AggregationType.Avg );
            //}



            return true;


        }

        private bool LoadGrid_t6_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t6_1;
            int iSht = 0;
            int iCol = 0;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fp.ActiveSheet = fp.Sheets[0];

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
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCLASS"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PROD_QTY"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "RUNHH"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "RUNMM"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "MAKE_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "GOODS_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "RAW_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "UTIL_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "PACK_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "FIXED_COST"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "HALF_COST"; iCol++;


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
        private bool LoadGrid_t6_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t6_1;
            int iSht = 1;
            int iCol = 0;


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
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCD"; iCol++;
            fp.Sheets[iSht].Columns[iCol].DataField = "ITEMCLASS"; iCol++;
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
        private bool LoadGrid_t7_Sht1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t7_1;
            int iSht = 0;
            int iCol = 0;
            int colindex;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fp.ActiveSheet = fp.Sheets[0];


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



            colindex = fp.Sheets[iSht].Columns["ACODENM"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            colindex = fp.Sheets[iSht].Columns["ACODENM_E"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;


            return true;


        }
        private bool LoadGrid_t7_Sht2()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t7_1;
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


            colindex = fp.Sheets[iSht].Columns["ACODENM"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            colindex = fp.Sheets[iSht].Columns["ACODENM_E"].Index;
            fp.Sheets[iSht].Columns[colindex].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;


            return true;


        }


        #endregion Load Control, Load Grid

        #region Toolbar Button
        private void Search()
        {
            try
            {
                if (tabControl1.SelectedIndex == 1) Search_T2();
                else if (tabControl1.SelectedIndex == 3) Search_T4();
                else if (tabControl1.SelectedIndex == 4) Search_T5();
                else if (tabControl1.SelectedIndex == 5) Search_T6();
                else if (tabControl1.SelectedIndex == 6) Search_T7();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Search_T2()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                MyCommand cmd = MkCommand_T2_1();
                this.fp_t2_1.Sheets[0].RowCount = 0;
                this.fp_t2_1.Sheets[1].RowCount = 0;

                var rtn = MyStatic.GetDataSet(cmd);

                if (rtn.ReturnCD == "S")
                {
                    this.fp_t2_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                    this.fp_t2_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];
                }




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private bool Search_T4()
        {
            string _sql = string.Empty;

            _sql = " EXECUTE ZBBS..[USP_COST_01_T4_SEL1] ";
            _sql += " @YYMM  = '" + dtp_t4_ym.Value.ToString("yyyyMM") + "' ;";
            var _cmd1 = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };

            _sql = " EXECUTE ZBBS..[USP_COST_01_T4_SEL2] ";
            _sql += " @YYMM  = '" + dtp_t4_ym.Value.ToString("yyyyMM") + "' ;";
            var _cmd2 = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };

            try
            {
                this.Cursor = Cursors.WaitCursor;

                
                this.fp_t4_1.Sheets[0].RowCount = 0;
                this.fp_t4_1.Sheets[1].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd1);

                this.fp_t4_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                this.fp_t4_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];

                
                /////

                this.fp_t4_1.Sheets[2].RowCount = 0;
                this.fp_t4_1.Sheets[3].RowCount = 0;

                var rtn2 = MyStatic.GetDataSet(_cmd2);

                this.fp_t4_1.Sheets[2].DataSource = rtn2.ReturnDs.Tables[0];


                // COLUMN HEADER VISIBLE FALSE
                for (int j = 5; j < fp_t4_1.Sheets[3].Columns.Count - 1; j++)
                {
                    fp_t4_1.Sheets[3].Columns[j].Visible = false;
                }

                // COLUMN HEADER Text만들고 VISIBLE TRUE
                int iCol = 5;
                foreach (DataRow row in rtn.ReturnDs.Tables[1].Rows)
                {
                    fp_t4_1.Sheets[3].ColumnHeader.Cells[0, iCol].Text = row["FCC"].ToString();
                    fp_t4_1.Sheets[3].Columns[iCol].Visible = true;
                    iCol++;
                }
                this.fp_t4_1.Sheets[3].DataSource = rtn2.ReturnDs.Tables[2];

                



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

            _sql = " EXECUTE ZBBS..USP_COST_01_T5_SEL1 ";
            _sql += " @YYMM  = '" + dtp_t5_ym.Value.ToString("yyyyMM") + "'  ";
            var _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };
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

            string _sql = string.Empty;


            _sql = " EXECUTE ZBBS..USP_COST_01_T6_SEL1 ";
            _sql += " @YYMM  = '" + dtp_t6_ym.Value.ToString("yyyyMM") + "' ";

            var _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };
            try
            {
                this.Cursor = Cursors.WaitCursor;


                this.fp_t6_1.Sheets[0].RowCount = 0;
                this.fp_t6_1.Sheets[1].RowCount = 0;

                var rtn = MyStatic.GetDataSet(_cmd);
                
                this.fp_t6_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                this.fp_t6_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[1];



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
        private bool Search_T7()
        {
            string _sql = string.Empty;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                _sql = " EXECUTE ZBBS..USP_COST_01_T7_SEL1 ";
                _sql += " @YYMM  = '" + dtp_t7_ym.Value.ToString("yyyyMM") + "' ";
                var _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };
                
                var rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t7_1.Sheets[0].RowCount = 0;
                this.fp_t7_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];

                _sql = " EXECUTE ZBBS..USP_COST_01_T7_SEL2 ";
                _sql += " @YYMM  = '" + dtp_t7_ym.Value.ToString("yyyyMM") + "' ";
                _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = _sql };

                
                rtn = MyStatic.GetDataSet(_cmd);

                this.fp_t7_1.Sheets[1].RowCount = 0;
                this.fp_t7_1.Sheets[1].DataSource = rtn.ReturnDs.Tables[0];



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


        #region "Event"
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // FunctNameEnum 참조
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                case 2:
                    ValidToolbarButtons = new string[] { "Refresh" };
                    break;
                case 1:
                case 3:
                case 4:
                case 5:
                case 6:
                    ValidToolbarButtons = new string[] { "Refresh", "Search" };
                    break;
                default:
                    break;
            }
        }
        private void Btn_t1_1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<MyCommand> cmds = MkCommand_T1_1();

                DBInsert(cmds);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void Btn_t1_copy_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                List<MyCommand> cmds = MkCommand_T1_2();

                DBInsert(cmds);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void Dtp_t3_ym_ValueChanged(object sender, EventArgs e)
        {
            DateTime lastDayOfMonth = new DateTime(
                dtp_t3_ym.Value.Year, dtp_t3_ym.Value.Month, DateTime.DaysInMonth(dtp_t3_ym.Value.Year, dtp_t3_ym.Value.Month));

            dtp_t3_ymd.Value = lastDayOfMonth;
        }

        /// <summary>
        /// 탭3: 원가계산 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bnt_t3_1_Click(object sender, EventArgs e)
        {

            string end_ym = string.Empty;
            string end_ymd = string.Empty;
            end_ym = dtp_t3_ym.Value.ToString("yyyyMM");
            end_ymd = dtp_t3_ymd.Value.ToString("yyyyMMdd");

            string exec_sql = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                txt_t3_1.Clear();
                // 원재료 원가 수불부
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[0, 0].Value))
                {
                    txt_t3_1.Text += "원재료 원가 수불부" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_01(end_ym);
                    DBInsert(myCmds, false);
                }

                // 제조비 집계 및 배부
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[1, 0].Value))
                {
                    txt_t3_1.Text += "제조비 집계 및 배부비율 " + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_02(end_ym);
                    DBInsert(myCmds, false);
                }

                // 제조경비 품종별 배부
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[2, 0].Value))
                {
                    txt_t3_1.Text += "제조경비 품종별 배부" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_03(end_ym);
                    DBInsert(myCmds, false);
                }

                // 중간체 원가계산 및 원가수불부 작성
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[3, 0].Value))
                {
                    txt_t3_1.Text += "중간체 원가계산 및 원가수불부 작성" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_04(end_ym);
                    DBInsert(myCmds, false);
                }
                // 제품 원가계산 및 원가수불부 작성
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[4, 0].Value))
                {
                    txt_t3_1.Text += "제품 원가계산 및 원가수불부 작성" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_05(end_ym);
                    DBInsert(myCmds, false);
                }
                // 원재료 분개
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[5, 0].Value))
                {
                    txt_t3_1.Text += "원재료 분개" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_06(end_ymd);
                    DBInsert(myCmds, false);
                }
                // 제품 분개
                if (Convert.ToBoolean(fp_t3_1.Sheets[0].Cells[5, 0].Value))
                {
                    txt_t3_1.Text += "제품 분개" + Environment.NewLine;
                    txt_t3_1.Refresh();
                    var myCmds = MkCommand_T3_Cost_07(end_ymd);
                    DBInsert(myCmds, false);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        /// <summary>
        /// 선택 컬럼 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fp_t3_1_CellClick(object sender, CellClickEventArgs e)
        {
            //int i = 0;
            if (e.ColumnHeader == false) return;

            // Column Index
            if (e.Column != 0) return;

            bool allChecked = false;

            if (fp_t3_1.Sheets[0].ColumnHeader.Cells[0, 0].Value == null ||
                Convert.ToBoolean(fp_t3_1.Sheets[0].ColumnHeader.Cells[0, 0].Value) == false)
            {
                allChecked = false;
            }
            else
            {
                allChecked = true;
            }


            if (allChecked == true) //선택 해제 
            {
                for (int i = 0; i < fp_t3_1.Sheets[0].RowCount; i++)
                {
                    fp_t3_1.Sheets[0].Cells[i, 0].Value = false;
                }
                fp_t3_1.Sheets[0].ColumnHeader.Cells[0, 0].Value = false;

            }
            else
            {
                for (int i = 0; i < fp_t3_1.Sheets[0].RowCount; i++)
                {
                    fp_t3_1.Sheets[0].Cells[i, 0].Value = true;
                }
                fp_t3_1.Sheets[0].ColumnHeader.Cells[0, 0].Value = true;
            }


        }

        #endregion
        #region "Db CRUD"
        /// <summary>
        /// output값이 필요없어면 void로...
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>

        private static void DBInsert(List<MyCommand> cmds, bool blMsg = true)
        {
            try
            {
                
                var rtn = MyStatic.ExecNonQuery(cmds);

                if (rtn.ReturnCD == "S")
                {
                    if (blMsg)
                        MessageBox.Show("정상적으로 처리되었습니다.");

                    return;
                }
                else
                {
                    MessageBox.Show(rtn.ReturnMsg);
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }


        }

        private bool Search_Load_T3()
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var cmd = MkCommand_T3_1();
                var rtn = MyStatic.GetDataSet(cmd);
                this.fp_t3_1.Sheets[0].RowCount = 0;
                if (rtn.ReturnCD == "S")
                {
                    this.fp_t3_1.Sheets[0].DataSource = rtn.ReturnDs.Tables[0];
                    spread_AutoRowHeight(fp_t3_1);
                }
                else
                {
                    MessageBox.Show(rtn.ReturnMsg);
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



        #endregion "DB CRUD"
        #region "Make Command"
        private List<MyCommand> MkCommand_T1_1()
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();

                MyCommand _cmd = new MyCommand()
                {
                    CommandName = "MST",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.StoredProcedure,
                    CommandText = "ZBBS..USP_INV_END_01"
                };

                // 파라미터 정의
                var myPara = new MyPara();
                myPara = new MyPara()
                {
                    ParameterName = "@YYMM",
                    DbDataType = (int)SqlDbType.VarChar,
                    Direction = (int)ParameterDirection.Input
                };
                _cmd.Parameters.Add(myPara);
                myPara = new MyPara()
                {
                    ParameterName = "@CREATE_BY",
                    DbDataType = (int)SqlDbType.VarChar,
                    Direction = (int)ParameterDirection.Input
                };
                _cmd.Parameters.Add(myPara);



                // 파라미터 값 처리
                List<MyParaValue[]> paraValueList = new List<MyParaValue[]>();
                MyParaValue[] valueSet1 = new MyParaValue[2];
                valueSet1[0] = new MyParaValue { ParameterName = "@YYMM", ParameterValue = dtp_t1_ym.Value.ToString("yyyyMM") };
                valueSet1[1] = new MyParaValue { ParameterName = "@CREATE_BY", ParameterValue = "-1" };

                paraValueList.Add(valueSet1);
                _cmd.ParaValues = paraValueList;

                cmds.Add(_cmd);
                return cmds;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private List<MyCommand> MkCommand_T1_2()
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();

                MyCommand _cmd = new MyCommand
                {
                    CommandName = "MST",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.StoredProcedure,
                    CommandText = "ZBBS..USP_CAL_00_DATA_COPY"
                };
                // 파라미터 정의
                var myPara = new MyPara();
                myPara = new MyPara
                {
                    ParameterName = "@YYMM",
                    DbDataType = (int)SqlDbType.VarChar,
                    Direction = (int)ParameterDirection.Input
                };

                List<MyParaValue[]> paraValueList = new List<MyParaValue[]>();
                
                MyParaValue[] valueSet1 = new MyParaValue[1];
                valueSet1[0] = new MyParaValue { ParameterName = "@YYMM", ParameterValue = dtp_t1_ym2.Value.ToString("yyyyMM") };
                paraValueList.Add(valueSet1);

                _cmd.ParaValues = paraValueList;

                cmds.Add(_cmd);
                return cmds;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private MyCommand MkCommand_T2_1()
        {
            try
            {
                string _itemclass = string.Empty;
                if (string.IsNullOrEmpty(cbo_t2_itemclass.Text))
                    _itemclass = string.Empty;
                else
                    _itemclass = cbo_t2_itemclass.Text.Substring(0, 1);

                string _sql = string.Empty;

                _sql = " EXECUTE ZBBS..USP_COST_01_T2_SEL ";
                _sql += " @YYMM  = '" + dtp_t2_c_ym.Value.ToString("yyyyMM") + "', " + Environment.NewLine;
                _sql += " @ITEM_CLASS = '" + _itemclass + "' " + Environment.NewLine;

                return new MyCommand //("MST", "SUZHOU", (int)CommandType.Text, _sql) 
                {
                    CommandName="MST",
                    ConnectionName="SUZHOU",
                    CommandType = (int) CommandType.Text,
                    CommandText = _sql
                };

            }
            catch (Exception)
            {

                throw;
            }

        }
        private MyCommand MkCommand_T3_1()
        {
            try
            {
                string _sql = string.Empty;

                _sql = " EXECUTE ZBBS..USP_COST_01_T3_SEL ";
                return new MyCommand//("MST", "SUZHOU", (int)CommandType.Text, _sql);
                {
                    CommandName = "MST",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = _sql
                };
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 원재료 원가수불부 작성
        /// </summary>
        /// <param name="ym"></param>
        /// <returns></returns>
        private List<MyCommand> MkCommand_T3_Cost_01(string ym)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();

                string execString1 = string.Empty;
                string execString2 = string.Empty;

                execString1 = "EXEC ZBBS..USP_CAL_01_RAW_AMT" +
                             " @YYMM = '" + ym + "'";
                execString2 = "EXEC ZBBS..USP_CAL_02_AVG_RAW_COST" +
                     " @YYMM = '" + ym + "'";


                var _cmd = new MyCommand
                {
                    CommandName = "MST1",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString1
                };
                cmds.Add(_cmd);

                _cmd = new MyCommand

                {
                    CommandName = "MST2",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString2
                };
                cmds.Add(_cmd);

                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 제조비집계및 배부 
        /// </summary>
        /// <param name="ym"></param>
        /// <returns></returns>
        private List<MyCommand> MkCommand_T3_Cost_02(string ym)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();
                string execString1 = string.Empty;
                string execString2 = string.Empty;
                string execString3 = string.Empty;

                execString1 = "EXEC ZBBS..USP_CAL_11_MFG_COST_CC " +
                              " @YYMM = '" + ym + "'";
                execString2 = "EXEC ZBBS..USP_CAL_12_DIST_RATE " +
                              " @YYMM = '" + ym + "'";
                execString3 = "EXEC ZBBS..USP_CAL_13_DIST_COST " +
                              " @YYMM = '" + ym + "'";

                var _cmd = new MyCommand
                {
                    CommandName = "MST1",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString1
                };
                cmds.Add(_cmd);

                _cmd = new MyCommand
                {
                    CommandName = "MST2",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString2
                };
                cmds.Add(_cmd);
                _cmd = new MyCommand
                {
                    CommandName = "MST3",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString3
                }; 

                cmds.Add(_cmd);
                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 제조경비 품종별 배부 
        /// </summary>
        /// <param name="ym"></param>
        /// <returns></returns>
        private List<MyCommand> MkCommand_T3_Cost_03(string ym)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();
                string execString1 = string.Empty;

                execString1 = "EXEC ZBBS..USP_CAL_21_DIST_ITEM " +
                              " @YYMM    = '" + ym + "' ";


                var _cmd = new MyCommand{ 
                    CommandName = "MST1", 
                    ConnectionName = "SUZHOU", 
                    CommandType = (int)CommandType.Text, 
                    CommandText = execString1 
                };
                cmds.Add(_cmd);

                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 중간체 원가계산 및 원가 수불부
        /// </summary>
        /// <param name="ym"></param>
        /// <returns></returns>
        private List<MyCommand> MkCommand_T3_Cost_04(string ym)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();
                string execString1 = string.Empty;
                string execString2 = string.Empty;

                execString1 = "EXEC ZBBS..USP_CAL_31_HALF_COST " +
                              " @YYMM = '" + ym + "' ";

                execString2 = "EXEC ZBBS..USP_CAL_32_AVG_HALF_COST " +
                              " @YYMM = '" + ym + "' ";

                var _cmd = new MyCommand
                {
                    CommandName = "MST1",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString1
                };
                
                cmds.Add(_cmd);

                _cmd = new MyCommand {
                    CommandName = "MST2",
                    ConnectionName = "SUZHOU",
                    CommandType = (int)CommandType.Text,
                    CommandText = execString2
                };
                cmds.Add(_cmd);

                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 제품 원가계산 및 원가 수불부
        /// </summary>
        /// <param name="ym"></param>
        /// <returns></returns>
        private List<MyCommand> MkCommand_T3_Cost_05(string ym)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();
                string execString1 = string.Empty;
                string execString2 = string.Empty;

                execString1 = "EXEC ZBBS..USP_CAL_41_GOODS_COST " +
                              " @YYMM = '" + ym + "' ";

                execString2 = "EXEC ZBBS..USP_CAL_42_AVG_GOODS_COST " +
                              " @YYMM = '" + ym + "' ";

                var _cmd = new MyCommand{ CommandName = "MST1", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = execString1 };
                cmds.Add(_cmd);

                _cmd = new MyCommand{ CommandName = "MST2", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = execString2 };
                cmds.Add(_cmd);

                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }
        private List<MyCommand> MkCommand_T3_Cost_06(string ymd)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();

                string execString = string.Empty;
                execString = "EXEC ZBBS..USP_CAL_51_JOURNAL_RAW " +
                             " @JOURNAL_YMD = '" + ymd + "'";

                var _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = execString };
                cmds.Add(_cmd);


                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }
        private List<MyCommand> MkCommand_T3_Cost_07(string ymd)
        {
            try
            {
                List<MyCommand> cmds = new List<MyCommand>();

                string execString = string.Empty;
                execString = "EXEC ZBBS..USP_CAL_52_JOURNAL_GOODS   " +
                             " @JOURNAL_YMD = '" + ymd + "'";

                var _cmd = new MyCommand{ CommandName = "MST", ConnectionName = "SUZHOU", CommandType = (int)CommandType.Text, CommandText = execString };
                cmds.Add(_cmd);


                return cmds;

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion "Make Command"
        #region Etc function
        private void spread_AutoRowHeight(FarPoint.Win.Spread.FpSpread fp)
        {
            int iRows = 0;
            FarPoint.Win.Spread.Row row;
            float sizerow;
            //float sizercol;

            iRows = fp.Sheets[0].RowCount;
            for (int i = 0; i < iRows; i++)
            {
                row = fp.Sheets[0].Rows[i];
                sizerow = row.GetPreferredHeight();
                if (sizerow > 25) sizerow += 8;
                row.Height = sizerow;
            }
        }
        private void ExecuteSecure(System.Action action)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => action()));
            }
            else
            {
                action();
            }
        }


        #endregion
    }
}
