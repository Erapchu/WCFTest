using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AutoUpdater.Common.Communications
{
    public class WcfNetPipeHost: WcfServiceHost
    {
        public WcfNetPipeHost(object singletonInstance, Type contractType, string pipeName) : 
            base(singletonInstance, contractType, null, null)
        {
            Binding = WcfServiceHelper.CreateNamedPipesBinding();
            ServiceUrl = WcfServiceHelper.GetNamedPipeServiceUrl(pipeName);
        }

        public WcfNetPipeHost(Type serviceType, Type contractType, string pipeName) :
            base(serviceType, contractType, null, null)
        {
            Binding = WcfServiceHelper.CreateNamedPipesBinding();
            ServiceUrl = WcfServiceHelper.GetNamedPipeServiceUrl(pipeName);
        }
    }
}
