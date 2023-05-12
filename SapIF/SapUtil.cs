using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SapIF
{
    public static class SapUtil
    {
        private static Dictionary<string, string> GetHeaderDic(string zInterfaceId, string zUserId )
        {
            Dictionary<string, string> headerDic = new Dictionary<string, string>();
            headerDic.Add("zInterfaceId", zInterfaceId );
            headerDic.Add("zConSysId", "KII_CHA");
            headerDic.Add("zProSysId", "GRP_ECC_PP");
            headerDic.Add("zUserId", zUserId);
            headerDic.Add("zPiUser", "IF_KIICHA");
            headerDic.Add("zTimeId", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            headerDic.Add("zLang", "");

            return headerDic;
        }

        public static string PP0370_Req(PP0370_ReqBody sapReqBody)
        {

            Dictionary<string, string> headerDic = GetHeaderDic("GRP_PP0370", "bbs");


            StringBuilder sb = new StringBuilder();
            StringWriter strw = new StringWriter(sb);

            using (XmlTextWriter w = new XmlTextWriter(strw))
            {
                w.Formatting = Formatting.Indented;

                w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

                w.WriteStartElement("soapenv", "Header", null);
                w.WriteEndElement(); // End Of soapenv:Header

                w.WriteStartElement("soapenv", "Body", null);
                w.WriteStartElement("inf", "MT_GRP_PP0370_Con", null);

                // 공통부분
                w.WriteStartElement("Header");
                {
                    foreach (KeyValuePair<string, string> di in headerDic)
                    {
                        w.WriteElementString(di.Key, di.Value);
                    }

                }
                w.WriteEndElement(); // End Of Header
                // 공통부분 끝


                w.WriteStartElement("Body");
                {
                    //List<string> lstWerks = sapReqBody.Werks;

                    foreach (var werks in sapReqBody.Werks)
                    {
                        w.WriteStartElement("T_WERKS");
                        w.WriteElementString("WERKS", werks);
                        w.WriteEndElement();
                    }

                    // List<string> lstMatnr = sapReqBody.Matnr;
                    foreach (var matnr in sapReqBody.Matnr)
                    {
                        w.WriteStartElement("T_MATNR");
                        w.WriteElementString("MATNR", matnr);
                        w.WriteEndElement();
                    }
                    //List<string> lstLgort = sapReqBody.Lgort;
                    foreach (var lgort in sapReqBody.Lgort)
                    {
                        w.WriteStartElement("T_LGORT");
                        w.WriteElementString("LGORT", lgort);
                        w.WriteEndElement();
                    }
                }

                w.WriteEndElement(); // End Of Body


                w.WriteEndElement(); // End Of inf:MT_GRP_PP0370_Con
                w.WriteEndElement(); // End Of soapenv:Body
                w.WriteEndElement(); // End Of First Start
                w.Close();

            }

            return sb.ToString(); 

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sb.ToString());
            //return xmlDoc;
        }

        public static DataSet PP0370_Response( XmlElement rtn)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable("Header");
            dt1.Columns.Add("zResultCd", typeof(string));
            dt1.Columns.Add("zResultMsg", typeof(string));
            ds.Tables.Add(dt1);

            DataTable dt2 = new DataTable("STOCK_LIST");
            dt2.Columns.Add("WERKS", typeof(string));
            dt2.Columns.Add("MATNR", typeof(string));
            dt2.Columns.Add("MAKTX", typeof(string));
            dt2.Columns.Add("LGORT", typeof(string));
            dt2.Columns.Add("CHARG", typeof(string));
            dt2.Columns.Add("ZZCHARG", typeof(string));
            dt2.Columns.Add("MEINS", typeof(string));
            dt2.Columns.Add("KALAB", typeof(string));
            dt2.Columns.Add("INSME", typeof(string));
            dt2.Columns.Add("SPEME", typeof(string));
            dt2.Columns.Add("UMLME", typeof(string));
            ds.Tables.Add(dt2);

            if (rtn?.OuterXml != null)
            {
                StringReader theReader = new StringReader(rtn.OuterXml);
                ds.ReadXml(theReader, XmlReadMode.IgnoreSchema);
            }

            return ds;

        }
        public static string PP0320_REQ(ZPP6350T zPP6350T)
        {
            Dictionary<string, string> headerDic = GetHeaderDic("GRP_PP0320", "bbs");

            StringBuilder sb = new StringBuilder();
            StringWriter strw = new StringWriter(sb);

            using (XmlTextWriter w = new XmlTextWriter(strw))
            {
                w.Formatting = Formatting.Indented;

                w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

                w.WriteStartElement("soapenv", "Header", null);
                w.WriteEndElement(); // End Of soapenv:Header

                w.WriteStartElement("soapenv", "Body", null);

                // 변경해줘야 할 부분 
                w.WriteStartElement("inf", "MT_GRP_PP0320_Con", null);

                // 공통부분 Strart Header
                w.WriteStartElement("Header");
                {
                    foreach (KeyValuePair<string, string> di in headerDic)
                    {
                        w.WriteElementString(di.Key, di.Value);
                    }

                }
                w.WriteEndElement(); // End Of Header

                //zPP.ZACTY = "3";   // 처리유형
                //zPP.ZREAC = "";   // 취소여부
                //zPP.AUERU = "";   // 확정유형
                //zPP.LMNGA = "239.2";   // 양품수량
                //zPP.XMNGA = "";   // 불량수량

                w.WriteStartElement("Body");
                {
                    w.WriteStartElement("ZPP6350T");
                    w.WriteElementString("AUFNR", zPP6350T.AUFNR);
                    w.WriteElementString("VORNR", zPP6350T.VORNR);
                    w.WriteElementString("ZWSQU", zPP6350T.ZWSQU);
                    w.WriteElementString("ARBPL", zPP6350T.ARBPL);
                    w.WriteElementString("MATNR", zPP6350T.MATNR);
                    w.WriteElementString("ZACTY", zPP6350T.ZACTY);
                    w.WriteElementString("LMNGA", zPP6350T.LMNGA);
                    w.WriteElementString("MEINH", zPP6350T.MEINH);
                    w.WriteElementString("BUDAT", zPP6350T.BUDAT);
                    w.WriteElementString("CHARG", zPP6350T.CHARG);
                    w.WriteEndElement();
                }
                w.WriteEndElement(); // End Of Body


                w.WriteEndElement(); // End Of inf:MT_GRP_PP0370_Con
                w.WriteEndElement(); // End Of soapenv:Body
                w.WriteEndElement(); // End Of First Start
                w.Close();

            }

            return sb.ToString();

            //Console.WriteLine(strw.ToString());

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sb.ToString());
            //return xmlDoc;


        }
        public static string PP0100_REQ(List<ZPP6210T> lstReq)
        {
            Dictionary<string, string> headerDic = GetHeaderDic("GRP_PP0100", "bbs");

            StringBuilder sb = new StringBuilder();
            StringWriter strw = new StringWriter(sb);

            using (XmlTextWriter w = new XmlTextWriter(strw))
            {
                w.Formatting = Formatting.Indented;

                w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

                w.WriteStartElement("soapenv", "Header", null);
                w.WriteEndElement(); // End Of soapenv:Header

                w.WriteStartElement("soapenv", "Body", null);

                // 변경해줘야 할 부분 
                w.WriteStartElement("inf", "MT_GRP_PP0100_Con", null);

                // 공통부분 Strart Header
                w.WriteStartElement("Header");
                {
                    foreach (KeyValuePair<string, string> di in headerDic)
                    {
                        w.WriteElementString(di.Key, di.Value);
                    }

                }
                w.WriteEndElement(); // End Of Header

                w.WriteStartElement("Body");
                {
                    foreach (var item in lstReq)
                    {
                        w.WriteStartElement("ZPP6210T");
                        w.WriteElementString("WERKS", item.WERKS);
                        w.WriteElementString("BUDAT", item.BUDAT);
                        w.WriteElementString("ZSERNO", item.ZSERNO);
                        w.WriteElementString("MATNR", item.MATNR);
                        w.WriteElementString("CHARG", item.CHARG);
                        w.WriteElementString("AUFNR", item.AUFNR);
                        w.WriteElementString("VORNR", item.VORNR);
                        w.WriteElementString("BWART", item.BWART);
                        w.WriteElementString("MENGE", item.MENGE);
                        w.WriteElementString("MEINS", item.MEINS);
                        w.WriteElementString("LGORT", item.LGORT);
                        w.WriteElementString("DATUV", item.DATUV);
                        w.WriteEndElement();
                    }

                }
                w.WriteEndElement(); // End Of Body


                w.WriteEndElement(); // End Of inf:MT_GRP_PP????_Con
                w.WriteEndElement(); // End Of soapenv:Body
                w.WriteEndElement(); // End Of First Start
                w.Close();

            }

            return sb.ToString();
            //Console.WriteLine(strw.ToString());

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sb.ToString());
            //return xmlDoc;

        }
        public static string PP0060_REQ(string werks, string yyyymm)
        {
            Dictionary<string, string> headerDic = GetHeaderDic("GRP_PP0060", "bbs");

            StringBuilder sb = new StringBuilder();
            StringWriter strw = new StringWriter(sb);

            using (XmlTextWriter w = new XmlTextWriter(strw))
            {
                w.Formatting = Formatting.Indented;

                w.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                w.WriteAttributeString("xmlns", "inf", null, "http://grpeccpp.esp.com/infesp");

                w.WriteStartElement("soapenv", "Header", null);
                w.WriteEndElement(); // End Of soapenv:Header

                w.WriteStartElement("soapenv", "Body", null);

                // 변경해줘야 할 부분 
                w.WriteStartElement("inf", "MT_GRP_PP0060_Con", null);

                // 공통부분 Strart Header
                w.WriteStartElement("Header");
                {
                    foreach (KeyValuePair<string, string> di in headerDic)
                    {
                        w.WriteElementString(di.Key, di.Value);
                    }

                }
                w.WriteEndElement(); // End Of Header

                w.WriteStartElement("Body");
                {
                    w.WriteElementString("WERKS", werks);
                    w.WriteElementString("SPMON", yyyymm);

                }
                w.WriteEndElement(); // End Of Body


                w.WriteEndElement(); // End Of inf:MT_GRP_PP????_Con
                w.WriteEndElement(); // End Of soapenv:Body
                w.WriteEndElement(); // End Of First Start
                w.Close();

            }
            return sb.ToString();   

            //Console.WriteLine(strw.ToString());

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(sb.ToString());
            //return xmlDoc;

        }

        

    }

    public class PP0370_ReqBody
    {
        public List<string> Werks { get; set; }
        public List<string> Matnr { get; set; }
        public List<string> Dispo { get; set; }
        public List<string> Charg { get; set; }
        public List<string> Lgort { get; set; }
    }

    public class ZPP6350T
    {
        public string AUFNR { get; set; }   // 생산오더번호
        public string VORNR { get; set; }   // 작업번호
        public string ZWSQU { get; set; }   // 차수
        public string ARBPL { get; set; }   // 작업장
        public string MATNR { get; set; }   // 품목코드
        public string ZACTY { get; set; }   // 처리 유형
        public string ZREAC { get; set; }   // 취소여부
        public string AUERU { get; set; }   // 확정 유형
        public string LMNGA { get; set; }   // 양품수량
        public string XMNGA { get; set; }   // 불량수량
        public string MEINH { get; set; }   // 단위
        public string BUDAT { get; set; }   // 전기일 = 작업일
        public string CHARG { get; set; }   // 배치번호
        public string ISM01 { get; set; }   // 실적액티비티1
        public string ISM02 { get; set; }   // 실적액티비티2
        public string ISM03 { get; set; }   // 실적액티비티3
        public string ISM04 { get; set; }   // 실적액티비티4
        public string ISM05 { get; set; }   // 실적액티비티5
        public string ISM06 { get; set; }   // 실적액티비티6
        public string RUECK { get; set; }   // 작업 완료확정 번호
        public string RMZHL { get; set; }   // 확정 카운터
        public string MJAHR { get; set; }   // 자재전표 연도
        public string MBLNR { get; set; }   // 자재전표 번호
        public string ZEILE { get; set; }   // 자재전표 품목
        public string HSDAT { get; set; }   // 생산일자(에어백 원단, 검단 이전 공정 생산일)
        public string ZPRDCO { get; set; }  // 생산처
        public string ZINCDV { get; set; }  // 입고구분
        public string LGORT { get; set; }   // 입고저장위치
        public string VBELN { get; set; }   // 판매 문서
        public string POSNR { get; set; }   // 판매 문서 품목
        public string ZCOLNM { get; set; }  // 칼라명 
        public string GRUND { get; set; }     // 차이사유
    }
    public class ZPP6210T
    {
        public string WERKS { get; set; }     // 플랜트	
        public string BUDAT { get; set; }     // 전기(GR)일자	
        public string ZSERNO { get; set; }    // 일련번호	
        public string MATNR { get; set; }     // 출고자재코드	
        public string CHARG { get; set; }     // 출고 Batch No.	
        public string ZPKG_NO { get; set; }   // 출고 SUB LOT 번호	
        public string AUFNR { get; set; }     // 생산오더 번호	
        public string ZJOBNO { get; set; }    // 작업지시번호	
        public string VORNR { get; set; }     // 촐고공정번호	
        public string BWART { get; set; }     // 이동유형(261,262)	
        public string RSNUM { get; set; }     // 출고예약번호	
        public string MENGE { get; set; }     // 투입출고수량(기본단위)	
        public string MEINS { get; set; }     // 기본단위	
        public string ERFMG { get; set; }     // 출고수량(생산단위)	
        public string ERFME { get; set; }     // 생산단위	
        public string LGORT { get; set; }     // 자재창고	
        public string MJAHR { get; set; }     // 자재문서년도(사용안함)	
        public string MBLNR { get; set; }     // 자재문서번호(사용안함)	
        public string KZEAR { get; set; }     // ‘ ‘ ? 부분출고, ‘X’ ? 최종출고확정	
        public string DATUV { get; set; }     // 작업일자(실제출고일자)	
        public string ZBFGI { get; set; }     // "Backflush 출고요청 - ""X"""	
        public string ZPROC { get; set; }     // 실적처리상태('N'-미처리,'Y'-처리완료,'E'-에러발생)	
        public string MDATE { get; set; }     // MES 생성일자	
        public string ZDATS { get; set; }     // SAP 처리일자	
        public string ZMSG { get; set; }      // 처리 Message	
        public string GRUND { get; set; }     // 이동 사유	
    }

}
