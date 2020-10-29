using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFCommon.Interfaces;

namespace AutoUpdater.Common.Communications
{
    public class WcfNetPipeClient<T>: WcfServiceClient<T> where T : IWcfBase
    {
        public WcfNetPipeClient(string pipeName, object callbackInstance = null, bool autoClose = true) : 
            base(null, null, callbackInstance, autoClose)
        {
            Binding = WcfServiceHelper.CreateNamedPipesBinding();
            ServiceUrl = WcfServiceHelper.GetNamedPipeServiceUrl(pipeName);
        }
    }
}
