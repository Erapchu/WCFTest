using System.ServiceModel;

namespace WCFCommon.WCF.NetPipe
{
    [ServiceContract]
    public interface IStringReverser
    {
        [OperationContract]
        string ReverseString(string value);

        [OperationContract]
        Class1 TestMethod(Class1 test);
    }
}
