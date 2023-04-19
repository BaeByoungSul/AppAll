using System.Data;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using System.Transactions;
//using Newtonsoft.Json;
using Models.Database;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System.Windows.Input;
using System.Configuration.Provider;
using System.Reflection.PortableExecutable;

namespace Services.DbService    ;


/// <summary>
/// 2022.12.19: 새로 작성
/// </summary>
public class DbService : IDbService
{
    private readonly List<ConnectDBInfo> _connectInfos;

    private List<DBOutPut> OutputList { get; set; } = new ();
    public DbService( List<ConnectDBInfo> dBInfos)
    {
        _connectInfos = dBInfos;
    }


    /// <summary>
    /// 1. DB 연결 생성
    /// 2. DB Command 생성
    /// 3. 파라미터 값 처리
    /// 3. 실행
    /// </summary>
    /// <param name="myCmds"></param>
    /// <returns></returns>
    public List<DBOutPut> ExecNonQuery(List<MyCommand> myCmds)
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
        
        try
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                // db 연결 생성하고 저장
                var dicConn = new Dictionary<string, IDbConnection>();
                foreach (var connName in connNames)
                {
                    var dbConn = CreateDbConnection(connName);
                    if (dbConn == null)
                        throw new InvalidDbException($"DB연결이 되지 않습니다.({connName})");
                    dbConn.Open();
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

                    if (dbCommand.Parameters.Count <= 0)
                    {
                        dbCommand.ExecuteNonQuery();
                        continue;
                    }

                    // Parmeter 값을 조회하기 위해서 Mapping되는 MyCommand 찾기
                    var myCommand = myCmds.Find(x => x.CommandName.Equals(commandName));
                    if (myCommand == null) 
                        throw new InvalidDbException("Command를 찾을 수 없습니다.");

                    // 한개의 Command를 여러번 실행한다.
                    // Parameter value dictionary 개수 많큼 실행
                    foreach (var valuePairs in myCommand.ParaValues ?? Enumerable.Empty<Dictionary<string, string>>())
                    {
                        // parameter값 처리
                        SetParameterValue(dbCommand, myCommand, valuePairs);
                        
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
        catch (Exception )
        {
            throw;
           // throw new Exception(ex.Message);
        }

        return OutputList;
        
    }
    public async Task<List<DBOutPut>> ExecNonQueryAsync(List<MyCommand> myCmds)
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
                        await ((SqlConnection)dbConn).OpenAsync();
                    else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
                        await ((OracleConnection)dbConn).OpenAsync();
                    else if (providerName.Equals("MySql.Data.MySqlClient"))
                        await ((MySqlConnection)dbConn).OpenAsync();

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
                            await ((SqlCommand)dbCommand).ExecuteNonQueryAsync();
                        else if (myCmdDB[commandName].Equals("Oracle.ManagedDataAccess.Client"))
                            await ((OracleCommand)dbCommand).ExecuteNonQueryAsync();
                        else if (myCmdDB[commandName].Equals("MySql.Data.MySqlClient"))
                            await ((MySqlCommand)dbCommand).ExecuteNonQueryAsync();

                        continue;
                    }

                    // MyCommand 찾기
                    var myCommand = myCmds.Find(x => x.CommandName.Equals(commandName));
                    if (myCommand == null)
                        throw new InvalidDbException("Command를 찾을 수 없습니다.");

                    // MyCommand 파라미터을 참조하여 D/B Command를 여러번 실행한다.
                    // Parameter value dictionary 개수 많큼 실행
                    foreach (var valuePairs in myCommand.ParaValues ?? Enumerable.Empty<Dictionary<string, string>>())
                    {
                        // parameter값 처리
                        SetParameterValue(dbCommand, myCommand, valuePairs);

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
        catch (Exception)
        {
            throw;
            // throw new Exception(ex.Message);
        }

        return OutputList;

    }
    public DataSet GetDataSet(MyCommand myCmd)
    {
        try
        {
            var dbConn = CreateDbConnection(myCmd.ConnectionName);
            if (dbConn == null)
                throw new InvalidDbException("D/B 연결이 null 입니다.");

            dbConn.Open();
            var dbCommand = CreateDbCommand(myCmd, dbConn);
            if (dbCommand == null)
                throw new InvalidDbException("Create DB Command Fail.");

            Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

            // parameter값 처리
            if (paraValuePair != null)
                SetParameterValue(dbCommand, myCmd, paraValuePair);

            // DB Adapter
            IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
            if (dbAdapter == null)
                throw new InvalidDbException("Create DB Adapter Fail.");

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
    public async Task<DataSet> GetDataSetAsync(MyCommand myCmd)
    {
        try
        {
            string providerName; 
            var dbConn = CreateDbConnection(myCmd.ConnectionName, out providerName);
            if (dbConn == null) 
                throw new InvalidDbException("D/B 연결이 null 입니다.");

            if (providerName.Equals("Microsoft.Data.SqlClient"))
                await ((SqlConnection)dbConn).OpenAsync();
            else if (providerName.Equals("Oracle.ManagedDataAccess.Client"))
                await ((OracleConnection)dbConn).OpenAsync();
            else if (providerName.Equals("MySql.Data.MySqlClient"))
                await ((MySqlConnection)dbConn).OpenAsync();

            var dbCommand = CreateDbCommand(myCmd, dbConn);
            if (dbCommand == null) 
                throw new InvalidDbException("Create DB Command Fail.");
            
            Dictionary<string, string>? paraValuePair = myCmd.ParaValues?.FirstOrDefault();

            // parameter값 처리
            if (paraValuePair != null)
                SetParameterValue(dbCommand, myCmd, paraValuePair);

            // DB Adapter
            IDataAdapter? dbAdapter = CreateDbAdapter(dbCommand, myCmd.ConnectionName);
            if (dbAdapter == null) 
                throw new InvalidDbException("Create DB Adapter Fail.");
            // DB Command exec 
            DataSet? ds = new("Result_Ds");
            await Task.Run(() => dbAdapter.Fill(ds));

            
            //var reader = await ((SqlCommand)dbCommand).ExecuteReaderAsync();
            //while (!reader.IsClosed)
            //    ds.Tables.Add().Load(reader);

            return ds;
        }
        catch (Exception)
        {
            throw;
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
            if (string.IsNullOrEmpty( myPara.ParameterName) ) continue;

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
    private bool IsExistConnectName(string connectionName) {
        
        
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
    private bool SetParameterValue (IDbCommand dbCmd, MyCommand myCmd, Dictionary<string,string> valuePairs)
    {
        try
        {
            foreach (IDbDataParameter dbPara in dbCmd.Parameters)
            {
                if (dbPara.Direction == ParameterDirection.Input) { }
                else if (dbPara.Direction == ParameterDirection.InputOutput) { }
                else continue;
                
                // parameter 값이 있어면 넣고 없어면 DBNull
                if(valuePairs.ContainsKey(dbPara.ParameterName))
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
                DBOutPut? output = OutputList
                        .OrderByDescending(x => x.Rowseq)
                        .FirstOrDefault(x => x.CommandName == myPara.HeaderCommandName &&
                         x.ParameterName == myPara.HeaderParameter);
                if (output == null) 
                    throw new InvalidDbException($"참조가 필요한 {myPara.HeaderCommandName} Command의 {myPara.HeaderParameter} 파라미터 Output값을 찾을 수 없습니다.");

                dbPara.Value = output.OutValue;
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

            DBOutPut rtnPara = new()
            {
                Rowseq = OutputList.Count + 1,
                CommandName = commandName,
                ParameterName = param.ParameterName.Trim(),
                OutValue = param.Value?.ToString() ?? String.Empty
            };
            OutputList.Add(rtnPara);

            Console.WriteLine(rtnPara);

        }
    }

}

///// <summary>
///// DB Connection Info
///// </summary>
//public class ConnectDBInfo
//{
//    public string ConnectionName { get; set; } = String.Empty;
//    public string ConnectionString { get; set; } = String.Empty;
//    public string ProviderName { get; set; } = String.Empty;
//}
