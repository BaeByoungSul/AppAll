using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyClientLib
{
    /// <summary>
    /// Spread 우클릭
    /// </summary>
    public static class MyContextMenu
    {

        public static void SetContextMenu(this FarPoint.Win.Spread.FpSpread sprd)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem01 = new ToolStripMenuItem()
            {
                Text = "엑셀 저장(xlsx)",
                Name = "SaveAsxlsx"
            };
            //menuItem01.Click += delegate (object sender, EventArgs e) { OnContextMenuClick2(sender, e, sprd); };
            menuItem01.Click += (object sender, EventArgs e) => { OnContextMenuClick(sender, e, sprd); };

            ToolStripMenuItem menuItem02 = new ToolStripMenuItem()
            {
                Text = "엑셀 저장(xls)",
                Name = "SaveAsxls"
            };
            menuItem02.Click += (object sender, EventArgs e) => { OnContextMenuClick(sender, e, sprd); };

            ToolStripMenuItem menuItem03 = new ToolStripMenuItem()
            {
                Text = "선택복사",
                Name = "ClipboardCopy_Sel"
            };
            menuItem03.Click += (object sender, EventArgs e) => { OnContextMenuClick(sender, e, sprd); };

            ToolStripMenuItem menuItem04 = new ToolStripMenuItem()
            {
                Text = "전체복사",
                Name = "ClipboardCopy_All"
            };
            menuItem04.Click += (object sender, EventArgs e) => { OnContextMenuClick(sender, e, sprd); };

            ToolStripMenuItem menuItem05 = new ToolStripMenuItem()
            {
                Text = "선택합계",
                Name = "Summary_Sel"
            };
            menuItem05.Click += (object sender, EventArgs e) => { OnContextMenuClick(sender, e, sprd); };

            contextMenuStrip.Items.Add(menuItem01);
            contextMenuStrip.Items.Add(menuItem02);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(menuItem03);
            contextMenuStrip.Items.Add(menuItem04);
            contextMenuStrip.Items.Add(menuItem05);

            sprd.ContextMenuStrip = contextMenuStrip;

        }

        private static void OnContextMenuClick(object sender, EventArgs e, FpSpread sprd)
        {
            //MessageBox.Show(sprd.Name);

            ToolStripMenuItem clickedMenu = (ToolStripMenuItem)sender;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "Save File",
                //InitialDirectory = ""
                Filter = "Excel files (*.xlsx)|*.xlsx|Excel 97-2007 Workbook(*.xls)|*.xls",
                RestoreDirectory = true
            }
             ;
            if (clickedMenu.Name == "SaveAsxlsx")
            {
                dialog.FilterIndex = 1;
            }
            else if (clickedMenu.Name == "SaveAsxls")
            {
                dialog.FilterIndex = 2;
            }

            switch (clickedMenu.Name)
            {

                case "SaveAsxlsx":
                case "SaveAsxls":
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        sprdExportExcel(sprd, clickedMenu.Name, dialog.FileName);
                        System.Diagnostics.Process.Start(dialog.FileName);
                    }
                    break;
                case "ClipboardCopy_Sel":
                    sprd.ActiveSheet.ClipboardCopy();
                    break;
                case "ClipboardCopy_All":
                    FarPoint.Win.Spread.Model.CellRange cr
                        = new FarPoint.Win.Spread.Model.CellRange(-1, 0, sprd.ActiveSheet.RowCount, sprd.ActiveSheet.ColumnCount);
                    sprd.ActiveSheet.ClipboardCopy(cr, FarPoint.Win.Spread.ClipboardCopyOptions.All);

                    //sprd.ActiveSheet.AddSelection(-1, -1, sprd.Sheets[0].RowCount, sprd.Sheets[0].ColumnCount);
                    break;
                case "Summary_Sel":
                    double dbl_rtn = sprd.sprdSelectSum();

                    MessageBox.Show(dbl_rtn.ToString("#,###.##"));
                    break;
                default:
                    break;
            }

        }
        private static void sprdExportExcel(FpSpread sprd, string menuName, string fileName)
        {
            if (menuName == "SaveAsxlsx")
            {
                sprd.ActiveSheet.Protect = false;
                sprd.SaveExcel(fileName, FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat | FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                sprd.ActiveSheet.Protect = true;
            }
            else if (menuName == "SaveAsxls")
            {
                sprd.ActiveSheet.Protect = false;
                sprd.SaveExcel(fileName,
                    FarPoint.Excel.ExcelSaveFlags.DataOnly |
                    FarPoint.Excel.ExcelSaveFlags.SaveCustomColumnHeaders);
                sprd.ActiveSheet.Protect = true;
            }

        }
        private static double sprdSelectSum(this FarPoint.Win.Spread.FpSpread sprd)
        {
            FarPoint.Win.Spread.Model.CellRange[] cr = sprd.Sheets[0].GetSelections();
            double dblSum = 0.0;
            for (int i = 0; i < cr.Count(); i++)
                for (int j = cr[i].Row; j < cr[i].Row + cr[i].RowCount; j++)
                    for (int k = cr[i].Column; k < cr[i].Column + cr[i].ColumnCount; k++)
                        dblSum += Convert.ToDouble(sprd.Sheets[0].Cells[j, k].Value);


            return dblSum;
        }



    }
}
