using Models.Database;
using System.ServiceModel;

namespace Services.DbService;

[ServiceContract(Namespace = "http://nakdong.wcf.service")]
public interface IDbService
{
    [OperationContract]
    MyReturn ExecNonQuery(List<MyCommand> myCmds);


    [OperationContract]
    Task<MyReturn> ExecNonQueryA(List<MyCommand> myCmds);

    [OperationContract]
    //[return: MessageParameter(Name = "TestReply")]
    MyReturn GetDataSet(MyCommand myCmd);

    [OperationContract]
    Task<MyReturn> GetDataSetA(MyCommand myCmd);

    [OperationContract]
    MyReturn GetDataTest();


}

