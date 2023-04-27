using Microsoft.Data.SqlClient;
using Models.Database;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Transactions;
using System.Xml;

namespace Services.DbService;

public class DbService : IDbService
{
    //private List<DBOutPut> OutputList { get; set; } = new();

    private readonly List<ConnectDBInfo> _connectInfos;
    private readonly DataTable _outputDt;
    private readonly IConfigurationRoot _configuration;

    public DbService()
    {
       // _connectInfos2 = dBInfos;
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false, reloadOnChange: true)
            .Build();


        var strSection = "DbConnectInfo";
        _connectInfos = _configuration.GetSection(strSection).Get<List<ConnectDBInfo>>();

        _outputDt = CreateOutputTable();
    }
    /// <summary>
    /// 1. DB 연결 생성
    /// 2. DB Command 생성
    /// 3. 파라미터 값 처리
    /// 3. 실행
    /// </summary>
    /// <param name="myCmds"></param>
    /// <returns></returns>
    public MyReturn ExecNonQuery(List<MyCommand> myCmds)
    {
        // Command명 중복이 있는지 점검
        int cntCommand = myCmds.Count;
        int cntCommandName = myCmds.Select(x => x.CommandName).Distinct().Count();
        if (cntCommand != cntCommandName)
            throw new InvalidDbException("중복된 Command명이 있습니다.");

        // Connect명 체크
        IEnumerable<string> connNames = myCmds.Select(x => x.ConnectionName).Distinct();
        foreach (var connName in connNames)
        {
            if (!IsExistConnectName(connName))
                throw new InvalidDbException($"연결명이 없습니다.({connName})");
        }

        // MyCommand 명 별 DB종류 
        Dictionary<string, string> myCmdDB = new Dictionary<string, string>();
        foreach (var cmd in myCmds)
        {
            var connInfo = _connectInfos.Find(x => x.ConnectionName == cmd.ConnectionName);
            if (connInfo == null) throw new InvalidDbException($"연결명이 없습니다.({cmd.ConnectionName})");
            myCmdDB.Add(cmd.CommandName, connInfo.ProviderName);
        }
        try
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                // db 연결 생성하고 저장
                var dicConn = new Dictionary<string, IDbConnection>();
                string providerName = string.Empty;
                foreach (var connName in connNames)
                {
                    var dbConn = CreateDbConnection(connName, out providerName);

                    if (dbConn == null)
                        throw new InvalidDbException($"DB연결이 되지 않습니다.({connName})");

                    if (providerName.Equals("Microsoft.Data.SqlClient"))
                        ((SqlConnection)dbConn).Open();
                    else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
                        ((OracleConnection)dbConn).Open();
                    else if (providerName.Equals("MySql.Data.MySqlClient"))
                        ((MySqlConnection)dbConn).Open();

                    dicConn.Add(connName, dbConn);
                }

                // db Command 생성하고 저장
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

                    if (commandName == null || dbCommand == null)
                        throw new InvalidDbException("Command를 찾을 수 없습니다");

                    // D/B command parameter가 없어면 바로 실행
                    if (dbCommand.Parameters.Count <= 0)
                    {
                        if (myCmdDB[commandName].Equals("Microsoft.Data.SqlClient"))
                            ((SqlCommand)dbCommand).ExecuteNonQuery();
                        else if (myCmdDB[commandName].Equals("Oracle.ManagedDataAccess.Client"))
                            ((OracleCommand)dbCommand).ExecuteNonQuery();
                        else if (myCmdDB[commandName].Equals("MySql.Data.MySqlClient"))
                            ((MySqlCommand)dbCommand).ExecuteNonQuery();

                        continue;
                    }

                    // MyCommand 찾기
                    var myCommand = myCmds.Find(x => x.CommandName.Equals(commandName));
                    if (myCommand == null)
                        throw new InvalidDbException("Command를 찾을 수 없습니다.");

                    // MyCommand 파라미터을 참조하여 D/B Command를 여러번 실행한다.
                    // Parameter value dictionary 개수 많큼 실행
                    //foreach (var valuePairs in myCommand.ParaValues ?? Enumerable.Empty<Dictionary<string, string>>())
                    foreach (var valuePairs in myCommand.ParaValues ?? Enumerable.Empty<MyParaValue[]>())
                    {
                        // parameter값 처리
                        SetParameterValue(dbCommand, myCommand, valuePairs.ToList());

                        // 실행
                        dbCommand.ExecuteNonQuery();


                        //output값 저장
                        SaveOutputValue(dbCommand, commandName);
                    }

                }
                // commit commands
                scope.Complete();
            }// end using transactionscope
        }
        catch (Exception ex)
        {
            throw new FaultException(ex.ToString());
        }

        StringWriter writer = new();
        _outputDt.WriteXml(writer, XmlWriteMode.WriteSchema);

        XmlDocument doc = new()
        {
            InnerXml = writer.ToString()
        };

        var rtn = new MyReturn()
        {
            ReturnCD = "S",
            ReturnMsg = "Success",
            RtnBody = doc.DocumentElement
        };
        return rtn;

        //  return OutputList;
    }
    public MyReturn GetDataSet(MyCommand myCmd)
    {
        try
        {
            string providerName;
            var dbConn = CreateDbConnection(myCmd.ConnectionName, out providerName);
            if (dbConn == null)
                throw new InvalidDbException("D/B 연결이 null 입니다.");

            if (providerName.Equals("Microsoft.Data.SqlClient"))
                ((SqlConnection)dbConn).Open();
            else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
                ((OracleConnection)dbConn).Open();
            else if (providerName.Equals("MySql.Data.MySqlClient"))
                ((MySqlConnection)dbConn).Open();

            var dbCommand = CreateDbCommand(myCmd, dbConn);
            if (dbCommand == null)
                throw new InvalidDbException("Create DB Command Fail.");

            //Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

            var paraValuePair = myCmd.ParaValues?.FirstOrDefault();
            // parameter값 처리
            if (paraValuePair != null)
                SetParameterValue(dbCommand, myCmd, paraValuePair.ToList());

            // DB Adapter
            IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
            if (dbAdapter == null)
                throw new InvalidDbException("Create DB Adapter Fail.");
            // DB Command exec 
            DataSet? ds = new("Result_Ds");
            dbAdapter.Fill(ds);

            StringWriter writer = new();
            ds.WriteXml(writer, XmlWriteMode.WriteSchema);

            XmlDocument doc = new()
            {
                InnerXml = writer.ToString()
            };

            var rtn = new MyReturn()
            {
                ReturnCD = "S",
                ReturnMsg = "Success",
                RtnBody = doc.DocumentElement
            };
            return rtn;
        }
        catch (InvalidDbException ex)
        {
            return new MyReturn()
            {
                //RtnHeader = rtnHeader,
                ReturnCD = "F",
                ReturnMsg = ex.Message,
                RtnBody = null
            };
        }
        catch (Exception ex) { throw new FaultException(ex.ToString()); }

    }


    /// <summary>
    /// TransactionScopeAsyncFlowOption.Enabled 추가함 
    /// </summary>
    /// <param name="myCmds"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDbException"></exception>
    //public async Task<MyReturn> ExecNonQueryA(List<MyCommand> myCmds)
    //{
    //    // Command명 중복이 있는지 점검
    //    int cntCommand = myCmds.Count;
    //    int cntCommandName = myCmds.Select(x => x.CommandName).Distinct().Count();
    //    if (cntCommand != cntCommandName)
    //        throw new InvalidDbException("중복된 Command명이 있습니다.");

    //    // Connect명 체크
    //    IEnumerable<string> connNames = myCmds.Select(x => x.ConnectionName).Distinct();
    //    foreach (var connName in connNames)
    //    {
    //        if (!IsExistConnectName(connName))
    //            throw new InvalidDbException($"연결명이 없습니다.({connName})");
    //    }

    //    // MyCommand 명 별 DB종류 
    //    Dictionary<string, string> myCmdDB = new Dictionary<string, string>();
    //    foreach (var cmd in myCmds)
    //    {
    //        var connInfo = _connectInfos.Find(x => x.ConnectionName == cmd.ConnectionName);
    //        if (connInfo == null) throw new InvalidDbException($"연결명이 없습니다.({cmd.ConnectionName})");
    //        myCmdDB.Add(cmd.CommandName, connInfo.ProviderName);
    //    }
    //    try
    //    {
    //        using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,  TransactionScopeAsyncFlowOption.Enabled))
    //        {
    //            // db 연결 생성하고 저장
    //            var dicConn = new Dictionary<string, IDbConnection>();
    //            string providerName = string.Empty;
    //            foreach (var connName in connNames)
    //            {
    //                var dbConn = CreateDbConnection(connName, out providerName);

    //                if (dbConn == null)
    //                    throw new InvalidDbException($"DB연결이 되지 않습니다.({connName})");

    //                if (providerName.Equals("Microsoft.Data.SqlClient"))
    //                    await ((SqlConnection)dbConn).OpenAsync();
    //                else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
    //                    await ((OracleConnection)dbConn).OpenAsync();
    //                else if (providerName.Equals("MySql.Data.MySqlClient"))
    //                    await ((MySqlConnection)dbConn).OpenAsync();

    //                dicConn.Add(connName, dbConn);
    //            }

    //            // db Command 생성하고 저장
    //            var dicCommand = new Dictionary<string, IDbCommand>();
    //            foreach (var myCmd in myCmds)
    //            {
    //                var dbCmd = CreateDbCommand(myCmd, dicConn[myCmd.ConnectionName]);
    //                dicCommand.Add(myCmd.CommandName, dbCmd);
    //            }

    //            // Parmeter 값을 주고 DB Command를  실행
    //            // 한개의 Command를 여러번 실행 할 수 있어야 한다.
    //            foreach (KeyValuePair<string, IDbCommand> dbCmdPair in dicCommand)
    //            {
    //                string commandName = dbCmdPair.Key;
    //                IDbCommand dbCommand = dbCmdPair.Value;

    //                if (commandName == null || dbCommand == null)
    //                    throw new InvalidDbException("Command를 찾을 수 없습니다");

    //                // D/B command parameter가 없어면 바로 실행
    //                if (dbCommand.Parameters.Count <= 0)
    //                {
    //                    if (myCmdDB[commandName].Equals("Microsoft.Data.SqlClient"))
    //                       await ((SqlCommand)dbCommand).ExecuteNonQueryAsync();
    //                    else if (myCmdDB[commandName].Equals("Oracle.ManagedDataAccess.Client"))
    //                       await ((OracleCommand)dbCommand).ExecuteNonQueryAsync();
    //                    else if (myCmdDB[commandName].Equals("MySql.Data.MySqlClient"))
    //                       await ((MySqlCommand)dbCommand).ExecuteNonQueryAsync();

    //                    continue;
    //                }

    //                // MyCommand 찾기git 
    //                var myCommand = myCmds.Find(x => x.CommandName.Equals(commandName));
    //                if (myCommand == null)
    //                    throw new InvalidDbException("Command를 찾을 수 없습니다.");

    //                // MyCommand 파라미터을 참조하여 D/B Command를 여러번 실행한다.
    //                // Parameter value dictionary 개수 많큼 실행
    //                foreach (var valuePairs in myCommand.ParaValues ?? Enumerable.Empty<Dictionary<string, string>>())
    //                {
    //                    // parameter값 처리
    //                    SetParameterValue(dbCommand, myCommand, valuePairs);

    //                    // 실행
    //                    //dbCommand.ExecuteNonQuery();
    //                    if (myCmdDB[commandName].Equals("Microsoft.Data.SqlClient"))
    //                        await ((SqlCommand)dbCommand).ExecuteNonQueryAsync();
    //                    else if (myCmdDB[commandName].Equals("Oracle.ManagedDataAccess.Client"))
    //                        await ((OracleCommand)dbCommand).ExecuteNonQueryAsync();
    //                    else if (myCmdDB[commandName].Equals("MySql.Data.MySqlClient"))
    //                        await ((MySqlCommand)dbCommand).ExecuteNonQueryAsync();
                        
    //                    //output값 저장
    //                    SaveOutputValue(dbCommand, commandName);
    //                }
    //            }
    //            // commit commands
    //            scope.Complete();
    //        }// end using transactionscope
    //    }
    //    catch (InvalidDbException ex)
    //    {
    //        return new MyReturn()
    //        {
    //            //RtnHeader = rtnHeader,
    //            ReturnCD = "F",
    //            ReturnMsg = ex.Message,
    //            RtnBody = null
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new FaultException(ex.ToString());
    //    }

    //    StringWriter writer = new();
    //    _outputDt.WriteXml(writer, XmlWriteMode.WriteSchema);

    //    XmlDocument doc = new()
    //    {
    //        InnerXml = writer.ToString()
    //    };

    //    var rtn = new MyReturn()
    //    {
    //        ReturnCD = "S",
    //        ReturnMsg = "Success",
    //        RtnBody = doc.DocumentElement
    //    };
    //    return rtn;

    //}

    //public async Task<MyReturn> GetDataSetA(MyCommand myCmd)
    //{
    //    try
    //    {
    //        string providerName;
    //        var dbConn = CreateDbConnection(myCmd.ConnectionName, out providerName);
    //        if (dbConn == null)
    //            throw new InvalidDbException("D/B 연결이 null 입니다.");

    //        if (providerName.Equals("Microsoft.Data.SqlClient"))
    //            await ((SqlConnection)dbConn).OpenAsync();
    //        else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
    //            await ((OracleConnection)dbConn).OpenAsync();
    //        else if (providerName.Equals("MySql.Data.MySqlClient"))
    //            await ((MySqlConnection)dbConn).OpenAsync();

    //        var dbCommand = CreateDbCommand(myCmd, dbConn);
    //        if (dbCommand == null)
    //            throw new InvalidDbException("Create DB Command Fail.");

    //        Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

    //        // parameter값 처리
    //        if (paraValuePair != null)
    //            SetParameterValue(dbCommand, myCmd, paraValuePair);

    //        // DB Adapter
    //        IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
    //        if (dbAdapter == null)
    //            throw new InvalidDbException("Create DB Adapter Fail.");
    //        // DB Command exec 
    //        DataSet? ds = new("Result_Ds");
    //        await Task.Run(() => dbAdapter.Fill(ds));

    //        StringWriter writer = new();
    //        ds.WriteXml(writer, XmlWriteMode.WriteSchema);

    //        XmlDocument doc = new()
    //        {
    //            InnerXml = writer.ToString()
    //        };

    //        var rtn = new MyReturn()
    //        {
    //            ReturnCD = "S",
    //            ReturnMsg = "Success",
    //            RtnBody = doc.DocumentElement
    //        };
    //        return rtn;
    //    }
    //    catch (InvalidDbException ex)
    //    {
    //        return new MyReturn()
    //        {
    //            //RtnHeader = rtnHeader,
    //            ReturnCD = "F",
    //            ReturnMsg = ex.Message,
    //            RtnBody = null
    //        };
    //    }
    //    catch (Exception ex) { throw new FaultException(ex.ToString()); }

    //}

    //public async Task<MyReturn> GetDataSetAsync2(MyCommand myCmd)
    //{
    //    try
    //    {
    //        string providerName;
    //        var dbConn = CreateDbConnection(myCmd.ConnectionName, out providerName);
    //        if (dbConn == null)
    //            throw new InvalidDbException("D/B 연결이 null 입니다.");

    //        if (providerName.Equals("Microsoft.Data.SqlClient"))
    //            await ((SqlConnection)dbConn).OpenAsync();
    //        else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
    //            await ((OracleConnection)dbConn).OpenAsync();
    //        else if (providerName.Equals("MySql.Data.MySqlClient"))
    //            await ((MySqlConnection)dbConn).OpenAsync();

    //        var dbCommand = CreateDbCommand(myCmd, dbConn);
    //        if (dbCommand == null)
    //            throw new InvalidDbException("Create DB Command Fail.");

    //        Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

    //        // parameter값 처리
    //        if (paraValuePair != null)
    //            SetParameterValue(dbCommand, myCmd, paraValuePair);

    //        // DB Adapter
    //        IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
    //        if (dbAdapter == null)
    //            throw new InvalidDbException("Create DB Adapter Fail.");
    //        // DB Command exec 
    //        DataSet? ds = new("Result_Ds");
    //        await Task.Run(() => dbAdapter.Fill(ds));
            

    //        StringWriter writer = new();
    //        ds.WriteXml(writer, XmlWriteMode.WriteSchema);

    //        XmlDocument doc = new()
    //        {
    //            InnerXml = writer.ToString()
    //        };

    //        var rtn = new MyReturn()
    //        {
    //            ReturnCD = "S",
    //            ReturnMsg = "Success",
    //            RtnBody = doc.DocumentElement
    //        };
    //        return rtn;
    //    }
    //    catch (InvalidDbException ex)
    //    {
    //        return new MyReturn()
    //        {
    //            //RtnHeader = rtnHeader,
    //            ReturnCD = "F",
    //            ReturnMsg = ex.Message,
    //            RtnBody = null
    //        };
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }


    //}
    
    
    public MyReturn GetDataTest()
    {
        string conString = "Data source=172.20.105.36,12345;User ID=sa;Password=wkehd123!@;Initial Catalog=TESTDB;Connection Timeout=30;TrustServerCertificate=true";
        //SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
       // DataTable dt;
        //Employee emp = new Employee();

        using (var con = new SqlConnection(conString))
        {
            cmd = new SqlCommand("select * from testdb..Student;select * from testdb..Student;", con);
            //cmd = new SqlCommand("select * from testdb..Employee;", con);
            sda = new SqlDataAdapter(cmd);

            DataSet? ds = new("Result_Ds");
            sda.Fill(ds);

            StringWriter writer = new();
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


    private IDbConnection? CreateDbConnection(string connectionName, out string providerName)
    {
        // db 연결정보 가져오기
        var connInfo = _connectInfos.Find(x => x.ConnectionName == connectionName);
        if (connInfo == null)
            throw new InvalidDbException("There's no Connection Info.");

        providerName = connInfo.ProviderName;

        IDbConnection? dbConnection = null;
        if (providerName.Equals("Microsoft.Data.SqlClient"))
            dbConnection = new SqlConnection(connInfo.ConnectionString);
        else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
            dbConnection = new OracleConnection(connInfo.ConnectionString);
        else if (providerName.Equals("MySql.Data.MySqlClient"))
            dbConnection = new MySqlConnection(connInfo.ConnectionString);

        return dbConnection;
    }
    private IDbConnection? CreateDbConnection(string connectionName)
    {
        // db 연결정보 가져오기
        var connInfo = _connectInfos.Find(x => x.ConnectionName == connectionName);
        if (connInfo == null)
            throw new InvalidDbException("There's no Connection Info.");

        string providerName = connInfo.ProviderName;

        IDbConnection? dbConnection = null;
        if (providerName.Equals("Microsoft.Data.SqlClient"))
            dbConnection = new SqlConnection(connInfo.ConnectionString);
        else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
            dbConnection = new OracleConnection(connInfo.ConnectionString);
        else if (providerName.Equals("MySql.Data.MySqlClient"))
            dbConnection = new MySqlConnection(connInfo.ConnectionString);

        return dbConnection;
    }
    private IDbCommand CreateDbCommand(MyCommand myCmd, IDbConnection dbConn)
    {
        var connInfo = _connectInfos.Find(x => x.ConnectionName == myCmd.ConnectionName);
        if (connInfo == null) throw new InvalidDbException("There's no Connection Info.");

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
            if (string.IsNullOrEmpty(myPara.ParameterName)) continue;

            if (sProviderName.Equals("Microsoft.Data.SqlClient"))
                dbParameter = new SqlParameter(myPara.ParameterName, (SqlDbType)myPara.DbDataType);
            else if (sProviderName.Equals("Oracle.ManagedDataAccess.Client"))
                dbParameter = new OracleParameter(myPara.ParameterName, (OracleDbType)myPara.DbDataType);
            else if (sProviderName.Equals("MySql.Data.MySqlClient"))
                dbParameter = new MySqlParameter(myPara.ParameterName, (MySqlDbType)myPara.DbDataType);

            if (dbParameter == null)
                throw new InvalidDbException("Create DB parameter Error.");

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
            throw new InvalidDbException("There's no Connection Info.");

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

    /// <summary>
    /// 파라미터 값을 처리한다.
    /// </summary>
    /// <param name="dbCmd"></param>
    /// <param name="myCmd"></param>
    /// <param name="valuePairs"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDbException"></exception>
    private bool SetParameterValue(IDbCommand dbCmd, MyCommand myCmd, List<MyParaValue> valuePairs)
    {
        try
        {
            foreach (IDbDataParameter dbPara in dbCmd.Parameters)
            {
                if (dbPara.Direction == ParameterDirection.Input) { }
                else if (dbPara.Direction == ParameterDirection.InputOutput) { }
                else continue;

                // parameter 값이 있어면 넣고 없어면 DBNull
                var para = valuePairs.Find(x => x.ParameterName == dbPara.ParameterName);
                if (para != null){
                    dbPara.Value = para.ParameterValue ;
                    dbPara.Value ??= System.DBNull.Value;
                }
                else
                    dbPara.Value = System.DBNull.Value;

                // 상위 Output 파라미터값을 참조하는지 ?
                // 상위 Output값을 찾아서 SetValue
                var myPara = myCmd.Parameters?
                    .Find(x => x.ParameterName == dbPara.ParameterName &&
                               !string.IsNullOrEmpty(x.HeaderCommandName) &&
                               !string.IsNullOrEmpty(x.HeaderParameter));

                // 참조할 output이 없어면 Skip
                if (myPara == null) continue;

                // 해당 파라미터가 헤더 command의 파라미터 참조해야 한다면
                //DBOutPut? output = OutputList
                //        .OrderByDescending(x => x.Rowseq)
                //        .FirstOrDefault(x => x.CommandName == myPara.HeaderCommandName &&
                //         x.ParameterName == myPara.HeaderParameter);
                //if (output == null)
                //    throw new InvalidDbException($"참조가 필요한 {myPara.HeaderCommandName} Command의 {myPara.HeaderParameter} 파라미터 Output값을 찾을 수 없습니다.");
                //dbPara.Value = output.OutValue;

                //DataRow[] rows = null;

                // output 테이블에서 필요한 행을 가져옴
                // _outputDt.DefaultView.Sort = " ROWSEQ DESC ";
                DataRow[] rows = _outputDt.Select(" CommandName  = '" + myPara.HeaderCommandName + "'" +
                                                  " AND ParameterName= '" + myPara.HeaderParameter + "'",
                                                  " Rowseq Desc");
                if (rows.Length < 1)
                {
                    throw new InvalidDbException($"참조가 필요한 {myPara.HeaderCommandName} Command의 {myPara.HeaderParameter} 파라미터 Output값을 찾을 수 없습니다.");
                }
                
                dbPara.Value = rows[0]["OutputValue"];
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }

    }
    private bool SetParameterValue_bak(IDbCommand dbCmd, MyCommand myCmd, Dictionary<string, string> valuePairs)
    {
        try
        {
            foreach (IDbDataParameter dbPara in dbCmd.Parameters)
            {
                if (dbPara.Direction == ParameterDirection.Input) { }
                else if (dbPara.Direction == ParameterDirection.InputOutput) { }
                else continue;

                // parameter 값이 있어면 넣고 없어면 DBNull
                if (valuePairs.ContainsKey(dbPara.ParameterName))
                {
                    dbPara.Value = valuePairs[dbPara.ParameterName];
                    dbPara.Value ??= System.DBNull.Value;
                }
                else
                    dbPara.Value = System.DBNull.Value;

                // 상위 Output 파라미터값을 참조하는지 ?
                // 상위 Output값을 찾아서 SetValue
                var myPara = myCmd.Parameters?
                    .Find(x => x.ParameterName == dbPara.ParameterName &&
                               !string.IsNullOrEmpty(x.HeaderCommandName) &&
                               !string.IsNullOrEmpty(x.HeaderParameter));

                // 참조할 output이 없어면 Skip
                if (myPara == null) continue;

                // 해당 파라미터가 헤더 command의 파라미터 참조해야 한다면
                //DBOutPut? output = OutputList
                //        .OrderByDescending(x => x.Rowseq)
                //        .FirstOrDefault(x => x.CommandName == myPara.HeaderCommandName &&
                //         x.ParameterName == myPara.HeaderParameter);
                //if (output == null)
                //    throw new InvalidDbException($"참조가 필요한 {myPara.HeaderCommandName} Command의 {myPara.HeaderParameter} 파라미터 Output값을 찾을 수 없습니다.");
                //dbPara.Value = output.OutValue;

                //DataRow[] rows = null;

                // output 테이블에서 필요한 행을 가져옴
                // _outputDt.DefaultView.Sort = " ROWSEQ DESC ";
                DataRow[] rows = _outputDt.Select(" CommandName  = '" + myPara.HeaderCommandName + "'" +
                                                  " AND ParameterName= '" + myPara.HeaderParameter + "'",
                                                  " Rowseq Desc");
                if (rows.Length < 1)
                {
                    throw new InvalidDbException($"참조가 필요한 {myPara.HeaderCommandName} Command의 {myPara.HeaderParameter} 파라미터 Output값을 찾을 수 없습니다.");
                }

                dbPara.Value = rows[0]["OutputValue"];
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }

    }


    /// <summary>
    /// Output & InputOutput & ReturnValue 를 저장
    /// </summary>
    /// <param name="dbCmd"></param>
    /// <param name="commandName"></param>
    private void SaveOutputValue(IDbCommand dbCmd, string commandName)
    {
        foreach (IDbDataParameter param in dbCmd.Parameters)
        {
            if (param.Direction == ParameterDirection.Output) { }
            else if (param.Direction == ParameterDirection.InputOutput) { }
            else if ((param.Direction == ParameterDirection.ReturnValue)) { }
            else continue;

            //DBOutPut rtnPara = new()
            //{
            //    Rowseq = OutputList.Count + 1,
            //    CommandName = commandName,
            //    ParameterName = param.ParameterName.Trim(),
            //    OutValue = param.Value?.ToString() ?? String.Empty
            //};
            //OutputList.Add(rtnPara);
            // Console.WriteLine(rtnPara);

            DataRow row = _outputDt.NewRow();
            // rowseq 자동 증가
            row["CommandName"] = commandName;
            row["ParameterName"] = param.ParameterName.Trim();
            row["OutputValue"] = param.Value?.ToString() ?? String.Empty;
            _outputDt.Rows.Add(row);
            _outputDt.AcceptChanges();

        }
    }

    private static DataTable CreateOutputTable()
    {
        DataTable dt = new DataTable("Table");
        
        DataColumn colseq = new DataColumn("Rowseq");
        colseq.DataType = System.Type.GetType("System.Int32");
        colseq.AutoIncrement = true; colseq.AutoIncrementSeed = 1; colseq.AutoIncrementStep = 1;

        dt.Columns.Add(colseq);
        dt.Columns.Add("CommandName", typeof(string));
        dt.Columns.Add("ParameterName", typeof(string));
        dt.Columns.Add("OutputValue", typeof(string));

        return dt;

        //        public int Rowseq { get; set; }
        //public string CommandName { get; set; }
        //public string ParameterName { get; set; }
        //public string OutValue { get; set; }
    }


}
