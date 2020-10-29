using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.Interfaces
{
    [ServiceContract]
    public interface IWcfBase
    {
        [OperationContract(IsOneWay = true)]
        void ValidateConnection();
    }
}
