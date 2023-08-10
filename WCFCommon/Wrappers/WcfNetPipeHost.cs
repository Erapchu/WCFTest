using System;

namespace WCFCommon.Wrappers
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
