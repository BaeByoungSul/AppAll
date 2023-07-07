using System;
using System.Data;
using System.Windows.Forms;
using CommonLib;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread;
using System.Net;
using System.Collections.Generic;

namespace SapIF
{
    public partial class SapIF1 : UserControl, IMainToolbar
    {
        #region IMyToolbar
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


        public SapIF1()
        {
            InitializeComponent();
            this.Load += SapIF1_Load;
        }
        
        #region Form Load, Load Control, Load Grid
        private void SapIF1_Load(object sender, EventArgs e)
        {
            txtWerks_t1.Text = "5131";
            txtLgort_t1.Text = "5701";

            LoadGrid_t1_1();
        }
        private bool LoadGrid_t1_1()
        {
            FarPoint.Win.Spread.FpSpread fp = fp_t1_1;

            fp.SetCursor(CursorType.Normal, Cursors.Arrow);
            fp.SetCursor(CursorType.LockedCell, Cursors.Arrow);

            //fp.SetContextMenu();

            fp.TabStripInsertTab = false;
            fp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;

            fp.Sheets[0].OperationMode = FarPoint.Win.Spread.OperationMode.Normal;

            fp.Sheets[0].DataAutoSizeColumns = false;
            fp.Sheets[0].AutoGenerateColumns = false;
            fp.Sheets[0].DataAutoCellTypes = false;
            fp.Sheets[0].Rows.Default.Height = 25;
            //fp.Sheets[0].ColumnHeader.Rows.Default.Height = 25;
            fp.Sheets[0].ColumnHeader.Rows[0].Height = 25;
            // fp.Sheets[0].ColumnHeader.Rows[1].Height = 25;

            fp.Sheets[0].RowCount = 0;

            fp.Sheets[0].FrozenColumnCount = 6;

            // Set the fields for the columns.
            int iCol = 0;
            fp.Sheets[0].Columns[iCol].DataField = "WERKS"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "LGORT"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "MATNR"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "MAKTX"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "CHARG"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "ZZCHARG"; iCol++;

            fp.Sheets[0].Columns[iCol].DataField = "ZZGRADE"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "MEINS"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "KALAB"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "INSME"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "SPEME"; iCol++;
            fp.Sheets[0].Columns[iCol].DataField = "UMLME"; iCol++;



            // Cell Alignment, Edit false
            for (int i = 0; i < fp.Sheets[0].Columns.Count; i++)
            {
                fp.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                fp.Sheets[0].Columns[i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                fp.Sheets[0].Columns[i].Locked = true;
                fp.Sheets[0].Columns[i].Tag = fp.Sheets[0].Columns[i].DataField;
            }

            fp.Sheets[0].Columns[3].HorizontalAlignment = CellHorizontalAlignment.Left;


            NumberCellType num = new NumberCellType()
            {
                DecimalPlaces = 3,
                DecimalSeparator = ".",
                FixedPoint = true,
                Separator = ",",
                ShowSeparator = true
            };

            for (int i = 8; i <= fp.Sheets[0].ColumnCount - 1; i++)
            {
                fp.Sheets[0].Columns[i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                fp.Sheets[0].Columns[i].CellType = num;
            }




            return true;


        }

        #endregion Form Load, Load Control, Load Grid

        #region 툴바 CRUD
        private void Search()
        {
            try
            {
                if (tabControl1.SelectedIndex == 0) Search_T1();
                //else if (tabControl1.SelectedIndex == 3) Search_T4();
                //else if (tabControl1.SelectedIndex == 4) Search_T5();
                //else if (tabControl1.SelectedIndex == 5) Search_T6();
                //else if (tabControl1.SelectedIndex == 6) Search_T7();

            }
            catch (Exception)
            {

                throw;
            }
        }
        private static readonly string PP0370_QAS = "http://infheaidrdb01.kolon.com:51000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_QAS&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";
        private static readonly string PP0370_PRD = "http://grpeai.kolon.com:50000/XISOAPAdapter/MessageServlet?senderParty=&senderService=INF_ESP_PRD&receiverParty=&receiverService=&interface=SI_GRP_PP0370_SO&interfaceNamespace=http://grpeccpp.esp.com/infesp";
        private  void Search_T1()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var lstWerks = txtWerks_t1.Text.Split(',');
                var lstLgort = txtLgort_t1.Text.Split(',');
                var lstMatnr = txtMatnr_t1.Text.Split(',');

                PP0370_ReqBody reqBody = new PP0370_ReqBody()
                {
                    Werks = new List<string> (),
                    Lgort = new List<string> (),
                    Matnr = new List<string> ()

                };

                //};
                foreach (var werks in lstWerks)
                {
                    reqBody.Werks.Add(werks.Trim());
                };
                foreach (var lgort in lstLgort)
                {
                    reqBody.Lgort.Add(lgort.Trim());
                }
                foreach (var matnr in lstMatnr)
                {
                    reqBody.Matnr.Add(matnr.Trim()  );
                }

                string request = SapUtil.PP0370_Req(reqBody);

                this.fp_t1_1.Sheets[0].RowCount = 0;

                var rtn = MyStatic.SapInterface(PP0370_QAS, request);
                //var rtn = MyStatic.SapInterface(PP0370_PRD, request);
                DataSet ds = SapUtil.PP0370_Response(rtn);

                
                fp_t1_1.Sheets[0].DataSource = ds.Tables[1];

                //var req = new Req_PP0370();
                //req.Header = new ReqHeader
                //{
                //    ZInterfaceId = "GRP_PP0370",
                //    ZConSysId = "KII_CHA",
                //    ZProSysId = "GRP_ECC_PP",
                //    ZUserId = "bbs",
                //    ZPiUser = "IF_KIICHA",
                //    ZTimeId = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                //    ZLang = ""
                //};
                //req.Body.TWERKS = new CWERKS[] { 
                //    //new CWERKS{ WERKS= "1108" }
                //    new CWERKS{ WERKS= "5131" }
                //    //new CWERKS{ WERKS= "1201" }
                //};
                //req.Body.TLGORT = new CLGORT[]
                //{
                //    //new  CLGORT{ LGORT = "3101"}
                //    new  CLGORT{ LGORT = "5701"}
                //    //new  CLGORT{ LGORT = "1103"}
                //};
                ////req.Body.TMATNR = new CMATNR[]
                ////{
                ////    new CMATNR{ MATNR = "10403395"}
                ////};

                //var reqstring = ReqString_PP0370(req);
                //Console.WriteLine(reqstring);
                //byte[] authBytes = Encoding.ASCII.GetBytes($"{req.Header.ZPiUser}:Interface!12");

                //var client = new HttpClient();

                //var request = new HttpRequestMessage(HttpMethod.Post, PP0370_Uri);

                //var reqContent = new StringContent(reqstring, Encoding.UTF8, "text/xml");
                //request.Content = reqContent;

                //request.Headers.Add("SOAPAction", SoapAction);
                //request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authBytes));
                //var response = await client.SendAsync(request);
                //Console.WriteLine(response);

                //var resStreamg = await response.Content.ReadAsStreamAsync();
                //XDocument xDoc = XDocument.Load(resStreamg);
                //XElement headElement = xDoc.Descendants("Header").FirstOrDefault();
                //XElement bodyElement = xDoc.Descendants("Body").FirstOrDefault();

                //PP0370_Result result = new PP0370_Result();

                //var header = headElement.DeserializeXml<MyHeader>("Header");
                //if (header.zResultCd == "S")
                //{
                //    var stockList = bodyElement.DeserializeXml<STOCK_LIST>("Body", "STOCK_LIST");
                //    fp_t1_1.Sheets[0].DataSource = stockList;
                //}


                //return;

                //var serializer = new XmlSerializer(typeof(MyHeader));
                //var aaa = (MyHeader)serializer.Deserialize(headElement.CreateReader());

                //XmlAttributeOverrides attributeOverrides = new XmlAttributeOverrides();
                //attributeOverrides.Add(typeof(List<STOCK_LIST>), new XmlAttributes()
                //{
                //    XmlRoot = new XmlRootAttribute("Body"),
                //});
                //attributeOverrides.Add(typeof(STOCK_LIST), new XmlAttributes()
                //{
                //    XmlType = new XmlTypeAttribute("STOCK_LIST"),
                //});

                //var serializer2 = new XmlSerializer(typeof(List<STOCK_LIST>), attributeOverrides);
                //var bbb = (List<STOCK_LIST>)serializer2.Deserialize(bodyElement.CreateReader());

                //Console.WriteLine(headElement);
                //Console.WriteLine(bodyElement);


                //attributeOverrides.Add(typeof(SapReturn), new XmlAttributes()
                //{
                //    XmlType = new XmlTypeAttribute("Stock_List"),
                //});

                //System.Xml.Serialization.XmlSerializer serializer =
                //    new System.Xml.Serialization.XmlSerializer(typeof(List<T>), new XmlRootAttribute("Result_Ds"));

                //System.Xml.XmlReader reader = headElement.CreateReader();

                //SapReturn result = (SapReturn)serializer.Deserialize(reader);
                //reader.Close();

                //return result;
                //var resMsg = response.Content.ReadAsStringAsync().Result;
                //XDocument xDoc = XDocument.Load(resMsg);
                //xDoc.DeserializeXml<Stockli>

                //return  client.SendAsync(request);
            }
            catch (WebException webEx)
            {
                MessageBox.Show(webEx.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion 툴바 CRUD
    }
}
