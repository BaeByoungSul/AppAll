using System.Data;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Transactions;
using CoreWCF;
using CoreWCF.Channels;
using Newtonsoft.Json;
using System.Drawing;
using System.Security.Cryptography;


namespace BBS.WCF;

/// <summary>
/// 2022.12.19: 새로 작성
/// </summary>
public class DBService_bak : IDBService_bak
{
    private readonly IConfigurationRoot _configuration;
    private readonly List<ConnectDBInfo> _connectInfos;
    private List<DBOutPut> OutputList { get; set; } = new();
    public DBService_bak()
    {
        _configuration = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false, reloadOnChange: true)
        .Build();


        var strSection = "DbConnectInfo";
        _connectInfos = _configuration.GetSection(strSection).Get<List<ConnectDBInfo>>();

        Console.WriteLine("DBService Service Created...{0}, {1}", GetClientAddress(), DateTime.Now);

        //foreach (var item in _connectInfos)
        //{
        //    Console.WriteLine($"{item.ConnectionName}: {item.ConnectionString}");
        //}
    }
    private static string? GetClientAddress()
    {
        // creating object of service when request comes   
        OperationContext context = OperationContext.Current;

        //Getting Incoming Message details   
        MessageProperties prop = context.IncomingMessageProperties;

        //Getting client endpoint details from message header   
        RemoteEndpointMessageProperty? endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
        return endpoint?.Address;
    }
    /// <summary>
    /// 1. DB 연결 생성
    /// 2. DB Command 생성
    /// 3. 파라미터 값 처리
    /// 3. 실행
    /// </summary>
    /// <param name="myCmds"></param>
    /// <returns></returns>
    public SvcReturn ExecNonQuery(List<MyCommand> myCmds)
    {
        // print paramete command loop
        // 같인 commandName이 있는지 ?
        // 파라미터 Validation ?
        foreach (var command in myCmds)
        {
            Console.WriteLine($"{command.CommandName}");
            Console.WriteLine($"{command.ConnectionName}");
            Console.WriteLine($"{command.CommandType}");
            Console.WriteLine($"{command.CommandText}");
            foreach (var para in command.Parameters ?? Enumerable.Empty<MyPara>())
            {
                Console.WriteLine($"{para.ParameterName}");
            }

            foreach (MyParaValue pairs in command.ParaValues ?? Enumerable.Empty<MyParaValue>())
            {
                foreach (var item in pairs)
                {
                    Console.WriteLine($"{item.Key}: {item.Value} ");
                }
            }

        }

        // Command명 중복이 있는지 점검
        int cntCommand = myCmds.Count;
        int cntCommandName = myCmds.Select(x => x.CommandName).Distinct().Count();
        if (cntCommand != cntCommandName)
            return new SvcReturn
            {
                ReturnCD = "FAIL",
                ReturnMsg = "Command명이 중복입니다.",
                ReturnStr = string.Empty
            };

        // Connect명 체크
        IEnumerable<string> connNames = myCmds.Select(x => x.ConnectionName).Distinct();
        foreach (var connName in connNames)
        {
            if (!IsExistConnectName(connName))
                return new SvcReturn
                {
                    ReturnCD = "FAIL",
                    ReturnMsg = $"연결명이 없습니다.({connName})",
                    ReturnStr = string.Empty
                };
        }

        try
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                // db 연결을 저장
                var dicConn = new Dictionary<string, IDbConnection>();
                //IEnumerable<string> connNames = myCmds.Select(x => x.ConnectionName).Distinct();
                foreach (var connName in connNames)
                {
                    var dbConn = CreateDbConnection(connName);

                    if (dbConn == null)
                        throw new Exception("There's null Connection.");

                    dbConn.Open();
                    dicConn.Add(connName, dbConn);
                }

                // db Command를 저장
                var dicCommand = new Dictionary<string, IDbCommand>();
                foreach (var myCmd in myCmds)
                {
                    var dbCmd = CreateDbCommand(myCmd, dicConn[myCmd.ConnectionName]);
                    dicCommand.Add(myCmd.CommandName, dbCmd);
                }

                // Parmeter 값을 주고 DB Command를  실행
                // 한개의 Command를 여러번 실행 할 수 있어야 한다.
                foreach (KeyValuePair<string, IDbCommand> dbCmdPair in dicCommand)
                {
                    string commandName = dbCmdPair.Key;
                    IDbCommand dbCommand = dbCmdPair.Value;

                    if (commandName == null) throw new Exception("Find Command Error1.");
                    if (dbCommand == null) throw new Exception("Find Command Error2.");

                    if (dbCommand.Parameters.Count <= 0)
                    {
                        dbCommand.ExecuteNonQuery();
                        continue;
                    }

                    // Parmeter 값을 조회하기 위해서 Mapping되는 MyCommand 찾기
                    var myCommand = myCmds.Find(x => x.CommandName.Equals(commandName));
                    if (myCommand == null) throw new Exception("Find Command Error3.");

                    // 한개의 Command를 여러번 실행한다.
                    foreach (var pairs in myCommand.ParaValues)
                    {
                        // 한 세트의 파라미터 값
                        Dictionary<string, string> paraValuePair = pairs;

                        // 파라미터 값 처리
                        foreach (IDbDataParameter dbPara in dbCommand.Parameters)
                        {
                            if (dbPara.Direction == ParameterDirection.Input ||
                                dbPara.Direction == ParameterDirection.InputOutput)
                            {
                                // 파라미트 값이 없어면  DBNull 처리
                                if (paraValuePair == null)
                                {
                                    dbPara.Value = System.DBNull.Value;
                                }
                                else if (paraValuePair.ContainsKey(dbPara.ParameterName))
                                {
                                    // data type의 차이는 일단 Exception
                                    dbPara.Value = paraValuePair[dbPara.ParameterName];
                                    dbPara.Value ??= System.DBNull.Value;
                                }
                                else
                                {
                                    dbPara.Value = System.DBNull.Value;
                                }
                                // Header값의 output을 참조해야 한다면
                                // OutputList에서 값을 찾아서 넣는다.
                                var myPara = myCommand.Parameters
                                    .Find(x => x.ParameterName == dbPara.ParameterName &&
                                               !string.IsNullOrEmpty(x.HeaderCommandName) &&
                                               !string.IsNullOrEmpty(x.HeaderParameter));

                                // 해당 파라미터가 헤더 command의 파라미터 참조해야 한다면
                                if (myPara != null)
                                {
                                    DBOutPut? output = OutputList
                                        .OrderByDescending(x => x.Rowseq)
                                        .FirstOrDefault(x => x.CommandName == myPara.HeaderCommandName &&
                                                             x.ParameterName == myPara.HeaderParameter);
                                    if (output == null)
                                        throw new Exception("There is nothing to refer header output value");
                                    dbPara.Value = output.OutValue;
                                }

                            }
                        }

                        dbCommand.ExecuteNonQuery();

                        // output및 return 파라미터값 저장
                        foreach (IDbDataParameter param in dbCommand.Parameters)
                        {
                            if (param.Direction == ParameterDirection.Output ||
                                param.Direction == ParameterDirection.InputOutput ||
                                param.Direction == ParameterDirection.ReturnValue)
                            {

                                DBOutPut rtnData = new()
                                {
                                    Rowseq = OutputList.Count + 1,
                                    CommandName = myCommand.CommandName,
                                    ParameterName = param.ParameterName.Trim(),
                                    OutValue = param.Value?.ToString() ?? string.Empty
                                };

                                OutputList.Add(rtnData);
                                Console.WriteLine(rtnData);
                            }

                        }
                    }

                }
                // commit commands
                scope.Complete();
            }// end using transactionscope
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        string xmlString = MyDbStatic.ToXML(OutputList, "Result_Ds");

        return new SvcReturn
        {
            ReturnCD = "OK",
            ReturnMsg = string.Empty,
            ReturnStr = xmlString
        };
    }
    public SvcReturn GetXmlData(MyCommand myCmd)
    {
        if (!IsExistConnectName(myCmd.ConnectionName))
            return new SvcReturn
            {
                ReturnCD = "FAIL",
                ReturnMsg = $"연결명이 없습니다.({myCmd.ConnectionName})",
                ReturnStr = string.Empty
            };

        try
        {
            var ds = GetDataSet(myCmd);

            // 쿼리 데이터셋 to Xml string  
            StringWriter writer = new();
            ds.WriteXml(writer, XmlWriteMode.WriteSchema);

            return new SvcReturn
            {
                ReturnCD = "OK",
                ReturnMsg = string.Empty,
                ReturnStr = writer.ToString()
            };

        }
        catch (Exception ex)
        {

            throw new FaultException(ex.Message);
        }

    }
    public SvcReturn GetJsonData(MyCommand myCmd)
    {
        if (!IsExistConnectName(myCmd.ConnectionName))
            return new SvcReturn
            {
                ReturnCD = "FAIL",
                ReturnMsg = $"연결명이 없습니다.({myCmd.ConnectionName})",
                ReturnStr = string.Empty
            };

        try
        {
            var ds = GetDataSet(myCmd);
            // 쿼리 데이터셋 to json string  
            string json = JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented);

            return new SvcReturn
            {
                ReturnCD = "OK",
                ReturnMsg = string.Empty,
                ReturnStr = json
            };

        }
        catch (Exception ex)
        {

            throw new FaultException(ex.Message);
        }

    }

    private DataSet GetDataSet(MyCommand myCmd)
    {
        try
        {
            var dbConn = CreateDbConnection(myCmd.ConnectionName);
            if (dbConn == null)
                throw new Exception("There's null Connection.");
            dbConn.Open();
            var dbCommand = CreateDbCommand(myCmd, dbConn);
            if (dbCommand == null) throw new Exception("Create DB Command Fail");

            Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

            // Parameter 값 셋팅
            // 한 세트의 파라미터 값
            foreach (IDbDataParameter dbPara in dbCommand.Parameters)
            {
                if (dbPara.Direction == ParameterDirection.Input ||
                    dbPara.Direction == ParameterDirection.InputOutput)
                {
                    // 파라미트 값이 없어면  DBNull 처리
                    if (paraValuePair == null)
                    {
                        dbPara.Value = System.DBNull.Value;
                    }
                    else if (paraValuePair.ContainsKey(dbPara.ParameterName))
                    {
                        // data type의 차이는 일단 Exception
                        dbPara.Value = paraValuePair[dbPara.ParameterName];
                        dbPara.Value ??= System.DBNull.Value;
                    }
                    else
                    {
                        dbPara.Value = System.DBNull.Value;
                    }
                }
            }

            // DB Adapter
            IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
            if (dbAdapter == null) throw new Exception("Create DB Adapter Fail");

            // DB Command exec 
            DataSet? ds = new("Result_Ds");
            dbAdapter.Fill(ds);

            return ds;
        }
        catch (Exception)
        {

            throw;
        }

    }
    private IDbConnection? CreateDbConnection(string connectionName)
    {
        // db 연결정보 가져오기
        var connInfo = _connectInfos.Find(x => x.ConnectionName == connectionName);
        if (connInfo == null)
            throw new Exception("There's no Connection Info.");

        IDbConnection? dbConnection = null;
        if (connInfo.ProviderName.Equals("Microsoft.Data.SqlClient"))
            dbConnection = new SqlConnection(connInfo.ConnectionString);
        else if (connInfo.ProviderName.Equals("Oracle.ManagedDataAccess.Client"))
            dbConnection = new OracleConnection(connInfo.ConnectionString);
        else if (connInfo.ProviderName.Equals("MySql.Data.MySqlClient"))
            dbConnection = new MySqlConnection(connInfo.ConnectionString);

        return dbConnection;
    }
    private IDbCommand CreateDbCommand(MyCommand myCmd, IDbConnection dbConn)
    {
        var connInfo = _connectInfos.Find(x => x.ConnectionName == myCmd.ConnectionName);
        if (connInfo == null) throw new Exception("There's no Connection Info.");

        string sProviderName = connInfo.ProviderName;

        IDbCommand dbCommand = dbConn.CreateCommand();

        dbCommand.Connection = dbConn;
        dbCommand.CommandType = (CommandType)myCmd.CommandType;
        dbCommand.CommandText = myCmd.CommandText;

        if (sProviderName.Equals("Oracle.ManagedDataAccess.Client"))
            ((OracleCommand)dbCommand).BindByName = true;

        foreach (var myPara in myCmd.Parameters ?? Enumerable.Empty<MyPara>())
        {
            IDbDataParameter? dbParameter = null;

            if (sProviderName.Equals("Microsoft.Data.SqlClient"))
                dbParameter = new SqlParameter(myPara.ParameterName, (SqlDbType)myPara.DbDataType);
            else if (sProviderName.Equals("Oracle.ManagedDataAccess.Client"))
                dbParameter = new OracleParameter(myPara.ParameterName, (OracleDbType)myPara.DbDataType);
            else if (sProviderName.Equals("MySql.Data.MySqlClient"))
                dbParameter = new MySqlParameter(myPara.ParameterName, (MySqlDbType)myPara.DbDataType);

            if (dbParameter == null)
            {
                throw new Exception("Create DB parameter Error.");
            }
            dbParameter.Direction = (ParameterDirection)Convert.ToInt32(myPara.Direction);

            dbCommand.Parameters.Add(dbParameter);
        } // enf of parameter

        return dbCommand;
    }
    private IDbDataAdapter? CreateDbAdapter(IDbCommand dbcmd, string connectionName)
    {
        string sProviderName = string.Empty;

        // db 연결정보 가져오기
        var connInfo = _connectInfos.Find(x => x.ConnectionName == connectionName);
        if (connInfo == null)
            throw new Exception("There's no Connection Info.");

        sProviderName = connInfo.ProviderName;

        IDbDataAdapter dbAdapter;

        if (sProviderName.Equals("Microsoft.Data.SqlClient"))
            dbAdapter = new SqlDataAdapter((SqlCommand)dbcmd);
        else if (sProviderName.Equals("Oracle.ManagedDataAccess.Client"))
            dbAdapter = new OracleDataAdapter((OracleCommand)dbcmd);
        else if (sProviderName.Equals("MySql.Data.MySqlClient"))
            dbAdapter = new MySqlDataAdapter((MySqlCommand)dbcmd);
        else
            return null;

        return dbAdapter;
    }

    private bool IsExistConnectName(string connectionName)
    {


        var connInfo = _connectInfos.Find(x => x.ConnectionName == connectionName);
        if (connInfo == null) return false;

        return true;
    }

    public MyReturn GetDataTest()
    {
        string conString = "Data source=172.20.105.36,12345;User ID=sa;Password=wkehd123!@;Initial Catalog=TESTDB;Connection Timeout=30;TrustServerCertificate=true";
        //SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        //Employee emp = new Employee();

        using (var con = new SqlConnection(conString))
        {
            cmd = new SqlCommand("select * from testdb..Student;select * from testdb..Student;", con);
            //cmd = new SqlCommand("select * from testdb..Employee;", con);
            sda = new SqlDataAdapter(cmd);

            DataSet? ds = new("Result_Ds");
            sda.Fill(ds);

            StringWriter writer = new() ;
            ds.WriteXml(writer, XmlWriteMode.WriteSchema);

            XmlDocument doc = new XmlDocument();
            doc.InnerXml = writer.ToString();

            //var rtnHeader = new ReturnHeader(){
            //    ReturnCD ="S",
            //    ReturnMsg="Success"
            //};
            var rtn = new MyReturn()
            {
                //RtnHeader = rtnHeader,
                ReturnCD = "S",
                ReturnMsg = "Success",
                RtnBody = doc.DocumentElement
            };
            return rtn;
            //doc.Load( );
            // In your case:
            //return doc.DocumentElement;

            //XmlElement xE = (XmlElement)Serialize(ds);

            //StringWriter writer = new();
            //ds.WriteXml(writer, XmlWriteMode.WriteSchema);

            //return xE;
        }
    }
    /// <summary>
    /// Serialize given object into XmlElement.
    /// </summary>
    /// <param name="transformObject">Input object for serialization.</param>
    /// <returns>Returns serialized XmlElement.</returns>
    #region Serialize given object into stream.
    public XmlElement Serialize(object transformObject)
    {
        XmlElement serializedElement = null;
        try
        {
            MemoryStream memStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(transformObject.GetType());
            serializer.Serialize(memStream, transformObject);
            memStream.Position = 0;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memStream);
            serializedElement = xmlDoc.DocumentElement;
        }
        catch (Exception SerializeException)
        {

        }
        return serializedElement;
    }
    #endregion // End - Serialize given object into stream.
}

/// <summary>
/// DB Connection Info
/// </summary>
public class ConnectDBInfo
{
    public string ConnectionName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
}
public static class MyDbStatic
{
    public static string ToXML<T>(List<T> outList, string rootName)
    {
        XmlSerializer serializer = new(typeof(List<T>), new XmlRootAttribute(rootName));

        // Remove Declaration
        var settings = new XmlWriterSettings
        {
            Indent = true,
            OmitXmlDeclaration = true
        };

        // Remove Namespace
        var xmlNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

        using (var swr = new StringWriter())
        using (XmlWriter writer = XmlWriter.Create(swr, settings))
        {
            serializer.Serialize(writer, outList, xmlNs);
            return swr.ToString();
        }

    }

}
