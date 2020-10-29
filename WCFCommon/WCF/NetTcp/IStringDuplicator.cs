using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.WCF.NetTcp
{
    [ServiceContract]
    public interface IStringDuplicator
    {
        [OperationContract]
        string MakeDuplicate(string target);
    }
}
