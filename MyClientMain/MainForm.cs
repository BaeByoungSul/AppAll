
using CommonLib;
using Models.Database;
using MyClientLib;
using MyClientMain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TabControl = System.Windows.Forms.TabControl;
using UserControl = System.Windows.Forms.UserControl;

namespace MyMain
{
    public partial class MainForm : Form
    {
        private readonly TabControl MyTabControl;
        //private TabPage LastSelectPg;
        private readonly List<string> listBtnNames = Enum.GetNames(typeof(ToolbarEnum)).ToList();
        public MainForm()
        {
            InitializeComponent();
            // 해상도의 90% 크기, 스크린 중앙에 나타내기
            {
                Rectangle resolution = Screen.PrimaryScreen.Bounds;

                Size mainSize = new Size();
                mainSize.Width = Convert.ToInt32(resolution.Width * 0.9);
                mainSize.Height = Convert.ToInt32(resolution.Height * 0.9);
                this.Size = mainSize;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point((resolution.Width - this.Width) / 2,
                                          (resolution.Height - this.Height) / 2);

            }

            MyTabControl = new System.Windows.Forms.TabControl();
            MyTabControl.Dock = DockStyle.Fill;
            MyTabControl.ItemSize = new Size { Width = 25 };


            splitContainer1.Panel2.Controls.Add(MyTabControl);

            statusVersion.Alignment = ToolStripItemAlignment.Right; 
            Search_Menu();

            this.FormClosing += MainForm_FormClosing;

            // Toolbar Button 클릭 Event 추가: IChildFuncInterface
            btnRefresh.Click += ToolBarBtn_Click;
            btnSearch.Click += ToolBarBtn_Click;
            btnSave.Click += ToolBarBtn_Click;
            btnUpdate.Click += ToolBarBtn_Click;
            btnDelete.Click += ToolBarBtn_Click;
            btnPrint.Click += ToolBarBtn_Click;

            // Toolbar Button 클릭 Event 추가: 화면닫기, 종료
            btnClose.Click += BtnClose_Click;
            btnExit.Click += BtnExit_Click;

            // 메뉴 트리뷰 더블 클릭
            treeViewMenu.NodeMouseDoubleClick += TreeViewMenu_NodeMouseDoubleClick;

            // 메인 탭 컨트로 탭 페이지
            MyTabControl.SelectedIndexChanged += MyTabControl_SelectedIndexChanged;
            
        }

        #region Toolstrip button Event

        /// <summary>
        /// 개발 폼에서 구현된 IMyToolbar의 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void ToolBarBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton;
            if (btn == null)
                throw new Exception("ToolStripButton is null");

            //MessageBox.Show(btn.Name);

            if (MyTabControl.SelectedIndex < 0) return;

            ToolbarEnum toolbar = ToolbarEnum.Search;
            if (btn.Name.Equals(btnRefresh.Name))
                toolbar = ToolbarEnum.Refresh;
            else if (btn.Name.Equals(btnSearch.Name))
                toolbar = ToolbarEnum.Search;
            else if (btn.Name.Equals(btnSave.Name))
                toolbar = ToolbarEnum.Save;
            else if (btn.Name.Equals(btnUpdate.Name))
                toolbar = ToolbarEnum.Update;
            else if (btn.Name.Equals(btnDelete.Name))
                toolbar = ToolbarEnum.Delete;
            else if (btn.Name.Equals(btnPrint.Name))
                toolbar = ToolbarEnum.Print;
            else
                throw new Exception("Unknown Toolbar 버튼");
            
            if (MyTabControl.SelectedTab.Controls[0] is IMainToolbar child)
            {
                child.FormFunction(toolbar);
            }
            else
            {
                MessageBox.Show("툴바 Interface가 구현되어 있지 않습니다.");
            }
        }
        private void MyTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            statusVersion.Text = string.Empty;
            SetToolbar(listBtnNames);

            if (MyTabControl.SelectedTab == null) return;
            if (MyTabControl.SelectedTab.Controls.Count <= 0) return;
            
            // 개발폼이 IMyToolbar를 구현했어면 
            if (MyTabControl.SelectedTab.Controls[0] is IMainToolbar child)
            {
                string[] sBtnNames = ((IMainToolbar)child).ValidBtnNames;
                SetToolbar(sBtnNames?.ToList() ?? listBtnNames);
            }
            
            // Assembly Version 표시
            Type t = MyTabControl.SelectedTab.Controls[0].GetType();

            //string activeTypeName = t.FullName;
            //string activeAssemName = t.Assembly.ManifestModule.ScopeName;
            //string activeVersion = t.Assembly.GetName().Version.ToString();
            
            statusVersion.Text = $"{t.Assembly.GetName().Name} : {t.Assembly.GetName().Version}";
        }
        private void Child_NotifyMsgChange(object sender, string message)
        {
            statusMsg.Text = message;   
           // throw new NotImplementedException();
        }
        private void Child_NotifyBtnChange(object sender, string[] trueBtnNames)
        {
            SetToolbar(trueBtnNames.ToList());
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (MyTabControl.SelectedIndex < 0) return;
            
            //var pg = LastSelectPg;
            MyTabControl.TabPages.RemoveAt(MyTabControl.SelectedIndex);
            
            //if (pg != null)   MyTabControl.SelectedTab = pg;
            
            
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Are sure to exit?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }
        #endregion Toolstrip button

        #region Menu        
        List<MyMainMenu> mainMenus = new List<MyMainMenu>();
        //private DataTable mainMenus;
        private  void Search_Menu()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                MyCommand _cmd = new MyCommand
                {
                    CommandName = "MST",
                    ConnectionName = "BSBAE",
                    CommandType = (int)CommandType.StoredProcedure,
                    CommandText = "[TESTDB].[dbo].[USP_MENU_SEL]"
                };

                var rtn = MyStatic.GetDataSet(_cmd);  

                if (rtn.ReturnCD != "S")
                {
                    MessageBox.Show(rtn.ReturnMsg);
                    return;
                }
        
               // Table to List
                mainMenus = (from DataRow dr in rtn.ReturnDs.Tables[0].Rows
                            select new MyMainMenu()
                            {
                                MenuID = Convert.ToInt32(dr["MenuID"]),
                                ParentID = Convert.ToInt32(dr["ParentID"]),
                                MenuName = dr["MenuName"].ToString(),
                                AssemblyName = dr["AssemblyName"].ToString(),
                                TypeName = dr["TypeName"].ToString(),
                                ShowInfo = dr["ShowInfo"].ToString()
                            }).ToList();
                
                MakeTreeMenu(mainMenus);

                treeViewMenu.ExpandAll();
                treeViewMenu.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MakeTreeMenu(List<MyMainMenu> lstMenu)
        {
            treeViewMenu.Nodes.Clear();
            foreach (var item in lstMenu)
            {
                var itemNode = treeViewMenu.Nodes.Find(item.ParentID.ToString(), true);
                if (itemNode.Length == 1)
                {
                    itemNode[0].Nodes.Add(item.MenuID.ToString(), item.MenuName);
                    Console.WriteLine(itemNode[0].Name);
                }
                else
                {
                    treeViewMenu.Nodes.Add(item.MenuID.ToString(), item.MenuName);
                }
            }

        }

        private void TreeViewMenu_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                var selectedMenu = mainMenus.Find(x => x.MenuID.ToString() == e.Node.Name);
                if (selectedMenu == null) return;
                if (string.IsNullOrEmpty(selectedMenu.AssemblyName)) return;
                if (string.IsNullOrEmpty(selectedMenu.TypeName)) return;
                if (string.IsNullOrEmpty(selectedMenu.ShowInfo)) return;

                string filePath = Application.StartupPath.ToString() + @"\" +
                                selectedMenu.AssemblyName;

                // Release 모드 일때 서버파일 다운로드
#if !DEBUG
            string sServerFileVer = DownloadAssembly(filePath);          
#endif

                if (!File.Exists(filePath))
                    throw new Exception("Assemly Not Found");

                // Assembly Load
                Assembly assembly = LoadAndGetMyAssembly(selectedMenu.AssemblyName);
                if (assembly == null) throw new Exception("Assemly Not Loaded");
                
                // 해당 폼이 개발이 되었는지 확인
                Type myType = assembly.GetType(selectedMenu.TypeName);
                if (myType == null) throw new Exception("해당 기능이 구현되지 않았습니다.");

                // 해당 타입이 있어면 해당 탭 페이지로 이동
                if (selectedMenu.ShowInfo.Equals("Single"))
                {
                    foreach (TabPage pg in MyTabControl.TabPages)
                    {
                        if (pg.Controls.Count < 1) continue;
                        if (pg.Controls[0].GetType().FullName == selectedMenu.TypeName)
                        {
                            MyTabControl.SelectedTab = pg;
                            return;
                        }
                    }
                }


                // 메인 탭 컨트롤에 탭페이지 추가하는데 개발 사용자 컨트롤을 추가한다.
                var pg1 = new TabPage
                {
                    Text = selectedMenu.MenuName
                };
                MyTabControl.TabPages.Add(pg1);

                if (!(assembly.CreateInstance(selectedMenu.TypeName) is UserControl ctrl1))
                    throw new Exception($"Not Found Type{selectedMenu.TypeName}");

                if (ctrl1 is IMainToolbar chid)
                {
                    chid.NotifyBtnChange += Child_NotifyBtnChange;
                    chid.NotifyMsgChange += Child_NotifyMsgChange;
                }

                pg1.Controls.Add(ctrl1);
                ctrl1.Dock = DockStyle.Fill;
                ctrl1.Show();
  
                //if (selectedMenu.DevType.Equals("Form"))
                //{
                //    if (!(assembly.CreateInstance(selectedMenu.TypeName) is Form ctrl1))
                //        throw new Exception($"Not Found Type{selectedMenu.TypeName}");

                //    if (ctrl1 is IMainToolbar child)
                //    {
                //        child.NotifyBtnChange += Child_NotifyBtnChange;
                //        child.NotifyMsgChange += Child_NotifyMsgChange;
                //    }
                //    ctrl1.TopLevel = false;
                //    ctrl1.FormBorderStyle = FormBorderStyle.None;
                //    ctrl1.AutoScaleMode = AutoScaleMode.Dpi;

                //    pg1.Controls.Add(ctrl1);
                //    ctrl1.Dock = DockStyle.Fill;
                //    ctrl1.Show();

                //}

                MyTabControl.SelectedTab = pg1;
                Refresh();

                // 페이지가 한개 일 경우는 select change event가 발생하지 않아서
                if (MyTabControl.TabPages.Count == 1)
                    statusVersion.Text = $"{myType.Assembly.GetName().Name} : {myType.Assembly.GetName().Version}";
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            finally
            { 
                this.Cursor= Cursors.Default;
                Refresh();     
            }
        }

        /// <summary>
        /// 왼쪽 메뉴 판넬 줄이기, 크게하게
        /// </summary>
        private int leftPanelWidth;
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == ">")
            {
                //splitContainer1.Width = 200;
                //button1.Width= 200;
                splitContainer1.SplitterDistance = leftPanelWidth;
                button1.Text = "<";
                treeViewMenu.Visible = true;
            }
            else
            {
                leftPanelWidth = splitContainer1.Panel1.Width;
                splitContainer1.SplitterDistance = button1.Width;
//              panel1.Width = button1.Width;
                button1.Text = ">";
                treeViewMenu.Visible= false;
            }
            Refresh();
        }
        #endregion Menu


        #region "Add Form && Func"

        private List<MyLoadedAssembly> MyLoadedAssems = new List<MyLoadedAssembly>();
        /// <summary>
        /// 1. Assembly가 처음 사용되었으면 Load 후 Return
        /// 2. Assembly의 버젼이 같을 때 해당 Assembly Return
        /// 3. Assembly의 버젼이 바뀌었을 때 해당 Assembly Load 후 Return
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        private Assembly LoadAndGetMyAssembly(string assemblyName)
        {
            string slocalFileVer = string.Empty;

            if (string.IsNullOrEmpty(assemblyName)) return null;

            string filePath = Application.StartupPath.ToString() + @"\" + assemblyName;

            // 선택한 메뉴의 Assembly가 Load되었는지 확인
            MyLoadedAssembly loadedAssembly = MyLoadedAssems.FindLast(x => x.AssemblyName == assemblyName);

            // 첫번째 Assemby Load 일때
            if (loadedAssembly == null)
            {
                //var assem =  AppDomain.CurrentDomain.Load(File.ReadAllBytes(assemblyFileName));
                var assem = Assembly.Load(File.ReadAllBytes(assemblyName));
                MyLoadedAssems.Add(new MyLoadedAssembly
                {
                    AssemblyName = assemblyName,
                    AssemblyVersion = assem.GetName().Version?.ToString(),
                    DevAssembly = assem
                });
                return assem;
            }

            // Client Local File Version
            if (!File.Exists(filePath))
                slocalFileVer = "N/A";
            else
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
                slocalFileVer = fvi.FileVersion;
            }

            // Assembly의 버젼이 같을 때 해당 Assembly Return
            if (slocalFileVer == loadedAssembly.AssemblyVersion)
            {
                return loadedAssembly.DevAssembly;
            }

            // Assembly의 버젼이 바뀌었을 때 해당 Assembly Load 후 Return
            var assem2 = Assembly.Load(File.ReadAllBytes(assemblyName));
            MyLoadedAssems.Add(new MyLoadedAssembly
            {
                AssemblyName = assemblyName,
                AssemblyVersion = assem2.GetName().Version?.ToString(),
                DevAssembly = assem2
            });
            return assem2;

        }
        public void Addform(TabPage tp, Form f)
        {

            f.TopLevel = false;
            //no border if needed
            f.FormBorderStyle = FormBorderStyle.None;
            f.AutoScaleMode = AutoScaleMode.Dpi;

            if (!tp.Controls.Contains(f))
            {
                tp.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
                Refresh();
            }
            Refresh();
        }
        private void SetToolbar(List<string> trueBtnNames)
        {

            btnRefresh.Enabled = false;
            btnSearch.Enabled = false;
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnPrint.Enabled = false;

            foreach (string btnName in trueBtnNames ?? Enumerable.Empty<string>())
            {
                if (btnName == ToolbarEnum.Refresh.ToString()) btnRefresh.Enabled = true;
                else if (btnName == ToolbarEnum.Search.ToString()) btnSearch.Enabled = true;
                else if (btnName == ToolbarEnum.Save.ToString()) btnSave.Enabled = true;
                else if (btnName == ToolbarEnum.Update.ToString()) btnUpdate.Enabled = true;
                else if (btnName == ToolbarEnum.Delete.ToString()) btnDelete.Enabled = true;
                else if (btnName == ToolbarEnum.Print.ToString()) btnPrint.Enabled = true;
            }

        }
        #endregion

        private void btnMenuRefresh_Click(object sender, EventArgs e)
        {
            Search_Menu();
            
            //DownloadTest( MyBindingEnum.NetTcp);
            //UploadTest();


        }

        #region Dowload File

        /// <summary>
        /// WCF Client 파일 내려받기 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        //private async Task<string> DownloadAssembly(string filePath)
        private string DownloadAssembly(string filePath)
        {
            string slocalFileVer = string.Empty;
            string sSeverFileVer = string.Empty;
            string assemName = Path.GetFileName(filePath);
            try
            {
                // Client Local File
                if (!File.Exists(filePath))
                    slocalFileVer = "N/A";
                else
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
                    slocalFileVer = fvi.FileVersion;
                }

                //var checkFile = _cli.CheckFile(assemName);
                var checkFile = MyStatic.CheckFile(assemName);
                sSeverFileVer = checkFile.FileVersion;

                // 서버에 해당 파일이 있고 버젼이 다르면 서버파일 다운로드
                if (checkFile.FileExists)
                {
                    if (!checkFile.FileVersion.Equals(slocalFileVer))
                    {
                        //DownloadFile fileRequest = new DownloadFile();
                        //fileRequest.FileName = assemName;

                        // 기존 파일 삭제 후 아래에서 다운로드
                        File.Delete(filePath);

                        //var myFile = await _cli.DownloadFileAsync(fileRequest);
                        var myFile = MyStatic.DownloadFile(assemName);
                        CustomStream customStream = new CustomStream(myFile.FileStream, myFile.FileLength);
                        customStream.ProgressChanged += CustomStream_ProgressChanged;
                        //customStream.ProgressChanged += DownloadProgressChanged;


                        FileStream targetStream = File.Create(filePath);
                        customStream.CopyTo(targetStream);
                        targetStream.Close();
                        customStream.Close();
                    }
                }
                else
                {
                    throw new Exception("아직 개발되지 않았습니다.");
                }


                //// Check File Version & Download File
                //using (FileClient _cli = new FileClient())
                //{
                //    // 서버파일 체크 
                //    CheckFileResponse checkFile = _cli.CheckFile(assemName);
                //    sSeverFileVer = checkFile.FileVersion;

                //    // 서버에 해당 파일이 있고 버젼이 다르면 서버파일 다운로드
                //    if (checkFile.FileExists)
                //    {
                //        if (!checkFile.FileVersion.Equals(slocalFileVer))
                //        {
                //            DownloadRequest fileRequest = new DownloadRequest();
                //            fileRequest.FileName = assemName;

                //            // 기존 파일 삭제 후 아래에서 다운로드
                //            File.Delete(filePath);

                //            //var myFile = await _cli.DownloadFileAsync(fileRequest);
                //            var myFile = _cli.DownloadFile(fileRequest);
                //            CustomStream customStream = new CustomStream(myFile.MyStream, myFile.FileLength);
                //            customStream.ProgressChanged += CustomStream_ProgressChanged;
                //            //customStream.ProgressChanged += DownloadProgressChanged;


                //            FileStream targetStream = File.Create(filePath);
                //            customStream.CopyTo(targetStream);
                //            targetStream.Close();
                //            customStream.Close();
                //        }
                //    }
                //    else
                //    {
                //        throw new Exception("아직 개발되지 않았습니다.");
                //    }
                //}
            }
            catch (Exception)
            {

                throw;
            }
            return sSeverFileVer;

        }
        /// <summary>
        /// 파일 내려받기 진행상황
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void CustomStream_ProgressChanged(object sender, MyProgressChangedEventArgs e)
        {
            long value = 100L * e.BytesRead / e.Length;
            if ((int)value > 100)
            {
                return;
            }
            if (toolStrip1.InvokeRequired)
            {
                toolStrip1.Invoke(new MethodInvoker(delegate
                {
                    statusProgress.Value = (int)value;
                }));
            }
            else
                statusProgress.Value = (int)value;

            // this.statusProgress.Value = (int)value;
            //statusProgress.re.Refresh();
        }

        #endregion Downlod Field

        //#region test 
        //private async void DownloadTest(MyBindingEnum myBindin)
        //{
        //    string filePath2 = Application.StartupPath.ToString() + @"\" +
        //                       "SoapUI-x64-5.5.0.exe";

        //    //DownloadAssembly(filePath2);
        //    string slocalFileVer = string.Empty;
        //    string sSeverFileVer = string.Empty;
        //    string assemName = Path.GetFileName(filePath2);
        //    try
        //    {
        //        // Client Local File
        //        if (!File.Exists(filePath2))
        //            slocalFileVer = "N/A";
        //        else
        //        {
        //            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath2);
        //            slocalFileVer = fvi.FileVersion;
        //        }
        //        // Check File Version & Download File
        //        using (FileClient _cli = new FileClient(myBindin))
        //        {
        //            // 서버파일 체크 
        //            CheckFileResponse checkFile = _cli.CheckFile(assemName);
        //            sSeverFileVer = checkFile.FileVersion;

        //            // 서버에 해당 파일이 있고 버젼이 다르면 서버파일 다운로드
        //            if (checkFile.FileExists)
        //            {
        //                if (!checkFile.FileVersion.Equals(slocalFileVer))
        //                {
        //                    DownloadRequest fileRequest = new DownloadRequest();
        //                    fileRequest.FileName = assemName;

        //                    // 기존 파일 삭제 후 아래에서 다운로드
        //                    File.Delete(filePath2);

        //                    //var myFile = _cli.DownloadFile(fileRequest);
        //                    var myFile = await _cli.DownloadFileAsync(fileRequest);
        //                    CustomStream customStream = new CustomStream(myFile.MyStream, myFile.FileLength);
        //                    customStream.ProgressChanged += CustomStream_ProgressChanged;
        //                    //customStream.ProgressChanged += DownloadProgressChanged;


        //                    FileStream targetStream = File.Create(filePath2);
        //                    customStream.CopyTo(targetStream);
        //                    targetStream.Close();
        //                    customStream.Close();
        //                    customStream.ProgressChanged -= CustomStream_ProgressChanged;
        //                }
        //            }
        //            else
        //            {
        //                throw new Exception("아직 개발되지 않았습니다.");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //private async void UploadTest()
        //{
        //    string filePath = "D:\\Software\\SoapUI-x64-5.5.0.exe";


        //    try
        //    {
        //        FileData myFile = new FileData();

        //        FileInfo sfi = new FileInfo(filePath);
        //        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);


        //        FileClient _cli = new FileClient();
        //        CheckFileResponse checkFile = _cli.CheckFile(sfi.Name);

        //        // 해당 파일이 있어면 백업받는다.
        //        // 서버파일과 버젼이 같으면 오류처리
        //        if (checkFile.FileExists)
        //        {
        //            if (versionInfo.FileVersion.Equals(checkFile.FileVersion))
        //            {
        //                throw new System.ArgumentException("Reqeust FileVersion is Equal to server version ");
        //            }
        //            // 백업 파일
        //            //BackUpFile(sfi.Name);
        //        }


        //        using (var sourceStream = File.OpenRead(sfi.FullName))
        //        {
        //            CustomStream customStream = new CustomStream(sourceStream, sfi.Length);
        //            customStream.ProgressChanged += UploadProgressChanged;
        //            //customStream.ProgressChanged += UploadProgressChanged;


        //            myFile.FileName = sfi.Name;
        //            myFile.FileLength= sfi.Length;  
        //            myFile.MyStream = customStream; // sourceStream;

        //            await _cli.UploadFileAsync(myFile);
        //            customStream.ProgressChanged -= UploadProgressChanged;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}
        //private void UploadProgressChanged(object sender, MyProgressChangedEventArgs e)
        //{
        //    long value = 100L * e.BytesRead / e.Length;
        //    if (toolStrip1.InvokeRequired)
        //    {
        //        toolStrip1.Invoke(new MethodInvoker(delegate 
        //        { 
        //            statusProgress.Value = (int)value; 
        //        }));
        //    }else
        //        statusProgress.Value = (int)value;


        //}


        //#endregion test 

    }

}
