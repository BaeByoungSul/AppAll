using Models.Database;
using System.Data;
using System.Runtime.Serialization;

namespace Services.DbService;

public interface IDbService
{
    List<DBOutPut> ExecNonQuery(List<MyCommand> myCmds);
    Task<List<DBOutPut>> ExecNonQueryAsync(List<MyCommand> myCmds);
    DataSet GetDataSet(MyCommand myCmd);
    Task<DataSet> GetDataSetAsync(MyCommand myCmd);

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
