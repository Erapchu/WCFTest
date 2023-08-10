using WCFCommon.Interfaces;

namespace WCFCommon.Wrappers
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
