using System.Xml;

namespace Models.Database;

[DataContract(Namespace = "")]
public class MyCommand
{
    public MyCommand()
    {
        CommandName = string.Empty;
        ConnectionName = string.Empty;
        CommandText = string.Empty;
        Parameters = new List<MyPara>();
        ParaValues = new List<MyParaValue[]>();
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

    [DataMember( Order = 5)]
    public List<MyParaValue[]> ParaValues { get; set; }
    //public SetArrayParaValues ParaValues { get; set; }
}

/// <summary>
/// HeaderCommandName, HeaderParameter :
///   - ExecNonQuery 여러개의 Command를 실행할 때 
///   - 이전의 Command의 output parameter값을 참조하고 싶을때 사용
/// </summary>
[DataContract(Namespace ="")]
public class MyPara
{
    public MyPara()
    {
        ParameterName = String.Empty;
        HeaderCommandName = String.Empty;
        HeaderParameter = String.Empty;
    }
    [DataMember(Order = 0)]
    public string ParameterName { get; set; }
    [DataMember(Order = 1)]
    public int DbDataType { get; set; }
    [DataMember(Order = 2)]
    public int Direction { get; set; } = 1; // 기본값 : ParameterDirection.Input = 1
    [DataMember(Order = 3)]
    public string? HeaderCommandName { get; set; }
    [DataMember(Order = 4)]
    public string? HeaderParameter { get; set; }
}

//[CollectionDataContract(ItemName = "SetOfParaValue", Namespace ="")]
//public class SetArrayParaValues : List<MyParaValue[]> { }

[DataContract(Namespace = "")]
public class MyParaValue
{
    [DataMember]
    public string? ParameterName { get; set; }
    [DataMember]
    public string? ParameterValue { get; set; }
}


[DataContract (Namespace = "")]
public class MyReturn
{
    //[DataMember(Order = 0)]
    //public ReturnHeader RtnHeader { get; set; } = new ReturnHeader();

    [DataMember(Order = 0)]
    
    public string? ReturnCD { get; set; }

    [DataMember(Order = 1)]
    public string? ReturnMsg { get; set; }

    [DataMember(Order = 2)]
    public XmlElement? RtnBody { get; set; }
}


/// <summary>
/// Parameter Value Dictionary
/// </summary>
//[CollectionDataContract
//    (Name = "ParameterWithValue",
//     ItemName = "Parameter",
//     KeyName = "ParameterName",
//     ValueName = "ParameterValue",
//     Namespace =""
//    )]
//public class MyParaValue : Dictionary<string, string>
//{
//}


// 사용하지 않음 
// public class MyParaValue : Dictionary<string, string> { }

//public class DBOutPut
//{
//    public DBOutPut()
//    {
//        CommandName = String.Empty;
//        ParameterName = String.Empty;
//        OutValue = String.Empty;
//    }
//    public int Rowseq { get; set; }
//    public string CommandName { get; set; }
//    public string ParameterName { get; set; }
//    public string OutValue { get; set; }
//    public override string ToString()
//    {
//        string stringValue = "Rowseq: " + Rowseq.ToString();
//        stringValue += " CommandName: " + CommandName;
//        stringValue += " ParameterName: " + ParameterName;
//        stringValue += " OutValue: " + OutValue;

//        return stringValue;
//    }

//}
