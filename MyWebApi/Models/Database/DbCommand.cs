using System.Runtime.Serialization;

namespace Models.Database;
public class MyCommand
{
    public MyCommand()
    {
        CommandName = string.Empty;
        ConnectionName = string.Empty;
        CommandText = string.Empty;

    }
    public MyCommand(string commandName, string connectionName, int commandType, string commandText)
    {
        CommandName = commandName;
        ConnectionName = connectionName;
        CommandType = commandType;
        CommandText = commandText;
    }

    public string CommandName { get; set; }
    public string ConnectionName { get; set; }
    public int CommandType { get; set; }
    public string CommandText { get; set; }
    public List<MyPara>? Parameters { get; set; }

    //public List<MyParaValue>? ParaValues { get; set; }

    public List<Dictionary<string, string>>? ParaValues { get; set; }

    //public MyParaValue[][]? ParaValues { get; set; }
}

public class MyPara
{
    public MyPara()
    {
        ParameterName = String.Empty;
        HeaderCommandName = String.Empty;
        HeaderParameter = String.Empty;
    }
    public string ParameterName { get; set; }
    public int DbDataType { get; set; }
    public int Direction { get; set; }
    
    public string HeaderCommandName { get; set; }
    public string HeaderParameter { get; set; }
}

// 사용하지 않음 
// public class MyParaValue : Dictionary<string, string> { }

public class DBOutPut
{
    public DBOutPut()
    {
        CommandName = String.Empty;
        ParameterName = String.Empty;
        OutValue = String.Empty;
    }
    public int Rowseq { get; set; }
    public string CommandName { get; set; }
    public string ParameterName { get; set; }
    public string OutValue { get; set; }
    public override string ToString()
    {
        string stringValue = "Rowseq: " + Rowseq.ToString();
        stringValue += " CommandName: " + CommandName;
        stringValue += " ParameterName: " + ParameterName;
        stringValue += " OutValue: " + OutValue;

        return stringValue;
    }

}
