using System;
using System.ServiceModel;

namespace WCFCommon.WCF.NetTcp
{
    [ServiceContract]
    public interface IStringDuplicator
    {
        [OperationContract]
        string MakeDuplicate(string target);

        [OperationContract(AsyncPattern = true)]
        IAsyncResult BeginServiceAsyncMethod(string msg, AsyncCallback callback, object asyncState);

        // Note: There is no OperationContractAttribute for the end method.
        string EndServiceAsyncMethod(IAsyncResult result);
    }
}
