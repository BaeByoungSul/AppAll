using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileTest
{
    public partial class Etc01_Test : UserControl
    {
        public Etc01_Test()
        {
            InitializeComponent();
        }
        private void Etc01_Test_Load(object sender, EventArgs e)
        {
            LoadGrid_t1_1();
        }
        #region Load Control, Load Grid

        /// <summary>
        /// 폼 로드시 컨트롤 기본값 처리
        /// </summary>
        /// <returns></returns>
        private bool LoadControl()
        {
            // txt.. 기본값...


            return true;
        }

        /// <summary>
        /// 폼 로드시 컨트롤 Spread Layout정의
        /// </summary>
        /// <returns></returns>
        private bool LoadGrid_t1_1()
        {

            fp_t1_1.Sheets[0].DataAutoSizeColumns = false;
            fp_t1_1.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
            fp_t1_1.Sheets[0].AutoGenerateColumns = false;
            fp_t1_1.Sheets[0].DataAutoCellTypes = false;
            fp_t1_1.Sheets[0].Rows[-1].Height = 25;

            // Set the fields for the columns.
            fp_t1_1.Sheets[0].Columns[0].DataField = "AssemblyName";
            fp_t1_1.Sheets[0].Columns[1].DataField = "FullName";

            // Cell Alignment, Edit false
            for (int i = 0; i < fp_t1_1.Sheets[0].Columns.Count; i++)
            {
                this.fp_t1_1.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                this.fp_t1_1.Sheets[0].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                this.fp_t1_1.Sheets[0].Columns[i].Locked = true;
                this.fp_t1_1.Sheets[0].Columns[i].Tag = fp_t1_1.Sheets[0].Columns[i].DataField;

            }
            this.fp_t1_1.Sheets[0].Columns[0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.fp_t1_1.Sheets[0].Columns[1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;



            return true;
        }
        #endregion Load Control, Load Grid

        private void btn_t1_1_Click(object sender, EventArgs e)
        {
            try
            {
                //string assemName = Path.GetFileNameWithoutExtension(filePath);

                AppDomain currentDomain = AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();

                DataTable table = AssemblyDt();

                // loaded assembly loop
                foreach (Assembly assem in assems)
                {

                    DataRow row = table.NewRow();

                    row["AssemblyName"] = assem.GetName().Name;
                    row["FullName"] = assem.FullName;

                    table.Rows.Add(row);


                }

                fp_t1_1.DataSource = table;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Source + ": " + ex.Message);
                return;
            }
        }
        private DataTable AssemblyDt()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("AssemblyName", typeof(string));
            dataTable.Columns.Add("FullName", typeof(string));

            return dataTable;
        }

        private void btn_t1_4_Click(object sender, EventArgs e)
        {
            AppDomainSetup domaininfo = new AppDomainSetup();
            //domaininfo.ApplicationBase = System.Environment.CurrentDirectory;
            //Evidence adevidence = AppDomain.CurrentDomain.Evidence;
            //AppDomain domain = AppDomain.CreateDomain("MyDomain", adevidence, domaininfo);

            //Type type = typeof(Proxy);
            //var value = (Proxy)domain.CreateInstanceAndUnwrap(
            //    type.Assembly.FullName,
            //    type.FullName);

            //string path = @"D:\MyAssem.dll";
            //var assembly = value.GetAssembly(path);

            AppDomain appDomain = null;
            try
            {
                string path = @"D:\MyAssem.dll";
                byte[] buffer = File.ReadAllBytes(path);

                appDomain = AppDomain.CreateDomain("Test");
                Assembly assm = appDomain.Load(buffer);

                Type[] types = assm.GetTypes();
                foreach (Type type in types)
                {
                    Console.WriteLine(type.FullName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (appDomain != null)
                    AppDomain.Unload(appDomain);
            }
        }

        private void btn_t1_2_Click(object sender, EventArgs e)
        {
            Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();

            int iRow = fp_t1_1.Sheets[0].ActiveRowIndex;
            if (iRow < 1) return;

            string assemblyName = fp_t1_1.Sheets[0].GetText(iRow, 0);

            txt_t1_1.Clear();
            foreach (Assembly assembly in assems)
            {
                if (assembly.GetName().Name.Equals(assemblyName))
                {
                    txt_t1_1.Text += "Assembly Full Name:" + Environment.NewLine;
                    txt_t1_1.Text += assembly.FullName + Environment.NewLine;

                    // The AssemblyName type can be used to parse the full name.
                    AssemblyName assemName = assembly.GetName();
                    txt_t1_1.Text += "\nName: " + assemName.Name + Environment.NewLine;
                    txt_t1_1.Text += "Version:" + assemName.Version.Major + '.' + assemName.Version.Minor + Environment.NewLine;
                    txt_t1_1.Text += "\nAssembly CodeBase: " + assembly.CodeBase + Environment.NewLine;
                }
            }

            return;
        }

        private void btn_t1_3_Click(object sender, EventArgs e)
        {
            Assembly[] assems = AppDomain.CurrentDomain.GetAssemblies();

            int iRow = fp_t1_1.Sheets[0].ActiveRowIndex;
            if (iRow < 1) return;

            string assemblyName = fp_t1_1.Sheets[0].GetText(iRow, 0);

            Assembly loadedAssemby = assems.LastOrDefault(a => a.GetName().Name == assemblyName);

            txt_t1_2.Clear();

            foreach (Type t in loadedAssemby.GetTypes())
            {
                txt_t1_2.Text += t.FullName.ToString() + Environment.NewLine;

            }
        }
    }
    public class Proxy : MarshalByRefObject
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFile(assemblyPath);
            }
            catch (Exception)
            {
                return null;
                // throw new InvalidOperationException(ex);
            }
        }
    }
}
