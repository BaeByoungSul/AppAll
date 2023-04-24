using CoreWCF;
using System.Data;
using System.Runtime.Serialization;
using System.Xml;

namespace BBS.WCF;

[ServiceContract(Namespace = "http://nakdong.wcf.service")]
public interface IDBService_bak
{
    [OperationContract]
    SvcReturn ExecNonQuery(List<MyCommand> myCmds);

    [OperationContract]
    SvcReturn GetXmlData(MyCommand myCmd);

    [OperationContract]
    SvcReturn GetJsonData(MyCommand myCmd);

    [OperationContract]
    MyReturn GetDataTest();

}
[DataContract]
public class MyReturn
{
    //[DataMember(Order = 0)]
    //public ReturnHeader RtnHeader { get; set; } = new ReturnHeader();

    [DataMember(Order = 0)]
    public string ReturnCD { get; set; }

    [DataMember(Order = 1)]
    public string ReturnMsg { get; set; }

    [DataMember(Order = 2)]
    public XmlElement? RtnBody { get; set; }
}
[DataContract]
public class ReturnHeader
{
    public string ReturnCD { get; set; }

    public string ReturnMsg { get; set; }
}

[DataContract]
public class SvcReturn
{
    public SvcReturn()
    {
        ReturnCD = string.Empty;
        ReturnMsg = string.Empty;
        ReturnStr = string.Empty;
    }
    public SvcReturn(string returnCD, string returnMsg, string returnStr)
    {
        ReturnCD = returnCD;
        ReturnMsg = returnMsg;
        ReturnStr = returnStr;
    }

    [DataMember(Order = 0)]
    public string ReturnCD { get; set; }

    [DataMember(Order = 1)]
    public string ReturnMsg { get; set; }

    [DataMember(Order = 2)]
    public string ReturnStr { get; set; }
}


[DataContract]
public class MyCommand
{
    public MyCommand()
    {
        CommandName = string.Empty;
        ConnectionName = string.Empty;
        CommandText = string.Empty;
        //Parameters = new List<MyPara> { new MyPara() }; 
        Parameters = new List<MyPara>();
        ParaValues = new List<MyParaValue>();
        // ParaValues = new List<Dictionary<string, string>> { new Dictionary<string, string> { } }

    }

    [DataMember(Order = 0, IsRequired = true)]
    public string CommandName { get; set; }
    [DataMember(Order = 1, IsRequired = true)]
    public string ConnectionName { get; set; }
    [DataMember(Order = 2, IsRequired = true)]
    public int CommandType { get; set; }

    [DataMember(Order = 3, IsRequired = true)]
    public string CommandText { get; set; }

    [DataMember(Order = 4)]
    public List<MyPara> Parameters { get; set; }

    [DataMember(Order = 5)]
    public List<MyParaValue> ParaValues { get; set; }


    //[DataMember(Order = 5)]
    //public List<Dictionary<string, string>> ParaValues { get; set; }

    //[DataMember(Order = 5)]
    //public MyParaValue[][]? ParaValues { get; set; }
}
[DataContract]
public class MyPara
{
    public MyPara()
    {
        ParameterName = string.Empty;
        HeaderCommandName = string.Empty;
        HeaderParameter = string.Empty;
    }
    [DataMember(Order = 0)]
    public string ParameterName { get; set; }
    [DataMember(Order = 1)]
    public int DbDataType { get; set; }
    [DataMember(Order = 2)]
    public int Direction { get; set; }
    [DataMember(Order = 3)]
    public string HeaderCommandName { get; set; }
    [DataMember(Order = 4)]
    public string HeaderParameter { get; set; }
}

// https://learn.microsoft.com/ko-kr/dotnet/framework/wcf/feature-details/collection-types-in-data-contracts
[CollectionDataContract
    (Name = "ParameterWithValue",
    ItemName = "Parameter",
    KeyName = "ParameterName",
    ValueName = "ParameterValue")]
public class MyParaValue : Dictionary<string, string> { }

public class DBOutPut
{
    public DBOutPut()
    {
        CommandName = string.Empty;
        ParameterName = string.Empty;
        OutValue = string.Empty;
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

//[DataContract]
//public class MyParaValue
//{
//    List<MyPara> _para;
//    public MyParaValue(List<string> para, List<string> )
//    {
//        //ParameterName = String.Empty;
//        //ParaValue = String.Empty;
//        _para = para;   
//    }

//    [DataMember(Order = 0)]
//    public List<string> ParameterName { get; set; }

//    [DataMember(Order = 1)]
//    public List<string> ParaValue { get; set; }

//}
