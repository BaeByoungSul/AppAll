
using CommonLib;
using Models.Database;
using MyClientLib;
using MyClientMain;
using Services.FileService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using TabControl = System.Windows.Forms.TabControl;
using UserControl = System.Windows.Forms.UserControl;

namespace MyMain
{
    public partial class MainForm : Form
    {
        
        //private readonly List<string> listBtnNames = Enum.GetNames(typeof(ToolbarEnum)).ToList();
        private readonly List<string> listAllBtns = Enum.GetNames(typeof(ToolbarEnum)).ToList();

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

            //MyTabControl = new System.Windows.Forms.TabControl();
            //MyTabControl.Dock = DockStyle.Fill;
            //MyTabControl.ItemSize = new Size { Width = 25 };


            //splitContainer1.Panel2.Controls.Add(MyTabControl);

            statusVersion.Alignment = ToolStripItemAlignment.Right; 
     

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
            tabCtrlMain.SelectedIndexChanged += MyTabControl_SelectedIndexChanged;
            tabCtrlMain.ControlAdded += TabCtrlMain_ControlAdded;
            tabCtrlMain.ControlRemoved += TabCtrlMain_ControlRemoved;
            
        }

       

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Search_Menu();
           // await DownloadFileAll(); //테스트 
#if !DEBUG
            //string sServerFileVer = DownloadAssembly(filePath);
            await DownloadFileAll();
#else
            await Task.Delay(100);
#endif

        }
        
        #region Dowload File 
        private async Task DownloadFileAll()
        {
            string appPath = Application.StartupPath.ToString();

            List<string> files = new List<string>();
            IFileService _cli = MyStatic.CheckFile_Channel();


            // 메뉴 리스트에서 Download 받아야 할 것을 files List에 추가한다.
            // Skip: assem
            foreach (var menu in mainMenus)
            {
                // Console.WriteLine($"{menu.AssemblyName}, {menu.TypeName}");
                if (string.IsNullOrEmpty(menu.AssemblyName)) continue;
                if (string.IsNullOrWhiteSpace(menu.AssemblyName)) continue;
                if (files.Exists(s => s.Equals(menu.AssemblyName))) continue;

                var rtn = _cli.CheckFile(menu.AssemblyName);
                // 서버에 없어면 skip
                if (!rtn.FileExists) continue;   

                string filePath = Path.Combine(appPath, menu.AssemblyName);

                // Client Local File version과 일치하면 skip
                if (File.Exists(filePath))
                {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(filePath);
                    if (fvi.FileVersion == rtn.FileVersion) continue;
                }
                files.Add(menu.AssemblyName);

            }
            ((IClientChannel)_cli).Close();

            if (files.Count <= 0) return;

            // + @"\" + selectedMenu.AssemblyName;

            try
            {
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                foreach (var fileNm in files)
                {
                    DownloadResponse res = await MyStatic.DownloadFileAsync(fileNm);


                    CustomStream customStream = new CustomStream(res.FileStream, res.FileLength);
                    customStream.ProgressChanged += Download_ProgressChanged;

                    string targetFilePath = Path.Combine(appPath, fileNm);

                    // Create or overwrite 
                    FileStream targetStream = File.Create(targetFilePath);
                    customStream.CopyTo(targetStream);

                    customStream.ProgressChanged -= Download_ProgressChanged;

                    customStream.Close();  
                    targetStream.Close();

                    customStream.Dispose();
                    targetStream.Dispose();

                    customStream = null;
                    targetStream = null;
                    
                    
                }
                //sw.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Download_ProgressChanged(object sender, MyProgressChangedEventArgs e)
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
        #endregion Downlod File


        #region Toolstrip button Event

        /// <summary>
        /// 개발 폼에서 구현된 IMainToolbar에 클릭한 버튼의 종류를 넘긴다.
        /// 개발 폼에서는 해당 버튼을 구분하여 사용한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void ToolBarBtn_Click(object sender, EventArgs e)
        {
            ToolStripButton btn = sender as ToolStripButton ?? 
                throw new Exception("ToolStripButton is null");

            if (tabCtrlMain.SelectedIndex < 0)
            {
                return;
            }

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

            //var child = tabCtrlMain.SelectedTab.Controls[0] as IMainToolbar;

            if (tabCtrlMain.SelectedTab.Controls[0] is IMainToolbar child)
            {
                child.ToolbarClick(toolbar);
            }
            else
            {
                MessageBox.Show("툴바 Interface가 구현되어 있지 않습니다.");
            }
        }
        /// <summary>
        /// Main TabControl을 클릭할 때 해당 Assembly Version표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlMain.SelectedTab == null) return;
            // Assembly Version을 Status바에 표시
            DisplayAssemblyVersion(tabCtrlMain.SelectedTab);
            
            // change toolbar button all enable
            SetToolbarBtnEnable(listAllBtns);

            // 개발폼이 IMainToolbar 구현 되어 있어면 Toolbar Button enable, disable 조정
            if (tabCtrlMain.SelectedTab.Controls[0] is IMainToolbar child)
            {
                string[] sBtnNames = ((IMainToolbar)child).ValidToolbarButtons;
                SetToolbarBtnEnable(sBtnNames?.ToList() ?? listAllBtns);
            }

            
        }
        private void TabCtrlMain_ControlAdded(object sender, ControlEventArgs e)
        {
            // Assembly Version을 Status바에 표시
            DisplayAssemblyVersion(tabCtrlMain.SelectedTab);
        }
        private void TabCtrlMain_ControlRemoved(object sender, ControlEventArgs e)
        {
            // Assembly Version을 Status바에 표시
            DisplayAssemblyVersion(tabCtrlMain.SelectedTab);
        }
        /// <summary>
        /// 선택된 탭페이지의 Tag를 읽고 Assembly Version을 Status바에 표시
        /// </summary>
        /// <param name="pg"></param>
        private void DisplayAssemblyVersion(TabPage pg)
        {
            statusVersion.Text = string.Empty;
            if (pg == null) return;

            var tabTag = pg.Tag as MyMainMenu;
            
            var assem = MyLoadedAssems.FindLast(x => x.AssemblyName == tabTag.AssemblyName);
            statusVersion.Text = $"{assem.AssemblyName} : {assem.AssemblyVersion}";
            
        }
        private void Child_NotifyMsgChange(object sender, string message)
        {
            statusMsg.Text = message;   
        }
        private void Child_NotifyBtnChange(object sender, string[] trueBtnNames)
        {
            SetToolbarBtnEnable(trueBtnNames.ToList());
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            if (tabCtrlMain.SelectedIndex < 0) return;
            
            // remove event
            if (tabCtrlMain.SelectedTab.Controls[0] is IMainToolbar child)
            {
                child.NotifySetToolbar -= Child_NotifyBtnChange;
                child.NotifySetMessage -= Child_NotifyMsgChange;
            }

            //var pg = LastSelectPg;
            tabCtrlMain.TabPages.RemoveAt(tabCtrlMain.SelectedIndex);
            
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


                if (!File.Exists(filePath))
                    throw new Exception("Assemly Not Found");

                // Assembly Load
                Assembly assembly = LoadAndGetMyAssembly(selectedMenu.AssemblyName);
                if (assembly == null) throw new Exception("Assemly Not Loaded");
                
                // 해당 폼이 개발이 되었는지 확인
                Type myType = assembly.GetType(selectedMenu.TypeName) ?? 
                    throw new Exception("해당 기능이 구현되지 않았습니다.");

                // 해당 타입이 있어면 해당 탭 페이지로 이동
                if (selectedMenu.ShowInfo.Equals("Single"))
                {
                    foreach (TabPage pg in tabCtrlMain.TabPages)
                    {
                        if (pg.Controls.Count < 1) continue;
                        if (pg.Controls[0].GetType().FullName == selectedMenu.TypeName)
                        {
                            tabCtrlMain.SelectedTab = pg;
                            return;
                        }
                    }
                }


                // 메인 탭 컨트롤에 탭페이지 추가하는데 개발 사용자 컨트롤을 추가한다.

                if (!(assembly.CreateInstance(selectedMenu.TypeName) is UserControl ctrl1))
                    throw new Exception($"Not Found Type{selectedMenu.TypeName}");

                AddUserCtrlToTab(ctrl1, selectedMenu);

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        private void BtnFold_Click(object sender, EventArgs e)
        {
            if (btnFold.Text == ">")
            {
                //splitContainer1.Width = 200;
                //button1.Width= 200;
                splitContainer1.SplitterDistance = leftPanelWidth;
                btnFold.Text = "<";
                treeViewMenu.Visible = true;
            }
            else
            {
                leftPanelWidth = splitContainer1.Panel1.Width;
                splitContainer1.SplitterDistance = btnFold.Width;
//              panel1.Width = button1.Width;
                btnFold.Text = ">";
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

            // 이미 로드 되었어면 해당 Assembly return
            if (loadedAssembly != null)
               return loadedAssembly.DevAssembly;

            else
            {
                var assem = Assembly.Load(File.ReadAllBytes(assemblyName));
                MyLoadedAssems.Add(new MyLoadedAssembly
                {
                    AssemblyName = assemblyName,
                    AssemblyVersion = assem.GetName().Version?.ToString(),
                    DevAssembly = assem
                });
                return assem;
            }
     
        }
        public void AddUserCtrlToTab( UserControl ctrl1, MyMainMenu selectedMenu)
        {
            // Assembly Version을 표시 하기 위해서 Tag에 selectedMenu값을 넣는다
            var pg1 = new TabPage
            {
                Text = selectedMenu.MenuName,
                Tag = selectedMenu
            };
            tabCtrlMain.TabPages.Add(pg1);


            // 추가한 개발폼의 event를 추가한다.
            if (ctrl1 is IMainToolbar child)
            {
                child.NotifySetToolbar += Child_NotifyBtnChange;
                child.NotifySetMessage += Child_NotifyMsgChange;
            }

            pg1.Controls.Add(ctrl1);
            ctrl1.Dock = DockStyle.Fill;
            ctrl1.Show();

            tabCtrlMain.SelectedTab = pg1;
            Refresh();
            

        }
       
        private void SetToolbarBtnEnable(List<string> trueBtnNames)
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

        private void BtnMenuRefresh_Click(object sender, EventArgs e)
        {
            Search_Menu();
            
            //DownloadTest( MyBindingEnum.NetTcp);
            //UploadTest();


        }


        
    }

}
