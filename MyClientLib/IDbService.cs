using Models.Database;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services.DbService
{
    [ServiceContract(Namespace = "")]
    public interface IDbService
    {
        [OperationContract]
        MyReturn ExecNonQuery(List<MyCommand> myCmds);

        [OperationContract]
        MyReturn GetDataSet(MyCommand myCmd);

        [OperationContract]
        MyReturn GetDataTest();



        //[OperationContract]
        //Task<MyReturn> ExecNonQueryA(List<MyCommand> myCmds);

        //[OperationContract]
        ////[return: MessageParameter(Name = "TestReply")]
        //Task<MyReturn> GetDataSetA(MyCommand myCmd);



    }
}



