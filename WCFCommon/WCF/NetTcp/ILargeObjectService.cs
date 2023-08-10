using System.IO;
using System.ServiceModel;

namespace WCFCommon.WCF.NetTcp
{
    [ServiceContract]
    public interface ILargeObjectService
    {
        [OperationContract]
        Stream GetLargeObject();
    }
}
