using System;
using System.Windows.Forms;
using MyClientLib;
using System.Diagnostics;
using System.IO;
using Services.FileService;
using CommonLib;


namespace FileTest
{
    public partial class File_01: UserControl
    {
        public File_01()
        {
            InitializeComponent();
        }
        private void File_01_Load(object sender, EventArgs e)
        {
            txtFileName_t1.Text = "CRRedist2008_x64.msi;FarPoint.Excel.dll;mysql-connector-net-8.0.15.msi;";
            txtFileName_t1.Text = txtFileName_t1.Text + "ODTforVS2017_183000.exe;Orange_ora_std.zip";
        }
        private void btn_file_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string initFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                //openFileDialog.InitialDirectory = initFolder; 
                openFileDialog.Filter = "All files (*.*)|*.*|(*.dll)|*.dll";

                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    txt_fileName.Text = openFileDialog.FileName;


                }
            }
        }

        private void btn_t1_2_Click(object sender, EventArgs e)
        {
            try
            {
                
                var uploadFileData = new UploadFile();
                // file 명, Length
                FileInfo sfi = new FileInfo(txt_fileName.Text.Trim());
                uploadFileData.FileName = sfi.Name;
                uploadFileData.FileLength = sfi.Length;

                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (var sourceStream = File.OpenRead(sfi.FullName))
                {
                    CustomStream customStream = new CustomStream(sourceStream, sfi.Length);
                    customStream.ProgressChanged += CustomStream_ProgressChanged; ;

                    uploadFileData.FileStream = customStream;
                    var rtn = MyStatic.UploadFile(uploadFileData);
                    uploadFileData.FileStream.Close();  
                }
                sw.Stop();

                txrElapsedTime.Text = Convert.ToString(sw.ElapsedMilliseconds / 1000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void CustomStream_ProgressChanged(object sender, MyProgressChangedEventArgs e)
        {
            long value = 100L * e.BytesRead / e.Length;

            this.progBar_t1.Value = (int)value;
        }

        private void btnDownload_t1_Click(object sender, EventArgs e)
        {
            //string targetFilePath = Path.Combine(@"d:\", txtFileName_t1.Text);
            string appPath = Application.StartupPath.ToString(); // + @"\" + selectedMenu.AssemblyName;
            string targetFilePath = string.Empty; // + @"\" + selectedMenu.AssemblyName;

            try
            {
                string[] fileNames = txtFileName_t1.Text.Split( ';');

                Stopwatch sw = new Stopwatch();
                sw.Start();
                foreach (var fileNm in fileNames)
                {
                    DownloadResponse res = MyStatic.DownloadFile(fileNm);


                    CustomStream customStream = new CustomStream(res.FileStream, res.FileLength);
                    customStream.ProgressChanged += Download_ProgressChanged1;
                    
                    //if (System.IO.File.Exists(targetFilePath)) System.IO.File.Delete(targetFilePath);

                    // Target filestream 생성
                    //FileStream targetStream = File.Create(targetFilePath);
                    //FileStream targetStream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write);
                    targetFilePath = Path.Combine(appPath, fileNm);
                    FileStream targetStream = File.Create(targetFilePath);
                    customStream.CopyTo(targetStream);
                    targetStream.Close();
                    customStream.ProgressChanged -= Download_ProgressChanged1;
                }
                



                sw.Stop();
                txrElapsedTime.Text = Convert.ToString(sw.ElapsedMilliseconds / 1000);

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void Download_ProgressChanged1(object sender, MyProgressChangedEventArgs e)
        {
            long value = 100L * e.BytesRead / e.Length;

            this.progBar_t1_2.Value = (int)value;
        }

        
    }
}
