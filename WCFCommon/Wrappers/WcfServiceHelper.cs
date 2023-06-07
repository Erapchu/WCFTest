using System;
using System.ServiceModel;

namespace WCFCommon.Wrappers
{
    public static class WcfServiceHelper
    {
        public static readonly string WCF_PIPE_URL_PREFIX = "net.pipe://localhost/";

        public static string GetNamedPipeServiceUrl(string pipeName)
        {
            var _pipeName = pipeName;
            if (_pipeName.StartsWith("/")) _pipeName = _pipeName.Substring(1);
            return WCF_PIPE_URL_PREFIX + _pipeName;
        }

        public static string GetEndpointUrl(string serviceUrl, string endpointName)
        {
            string endpointUrl = serviceUrl;
            if (!string.IsNullOrEmpty(endpointName))
            {
                if (!endpointUrl.EndsWith("/") && !endpointUrl.StartsWith("/"))
                    endpointUrl += "/";
                endpointUrl += endpointName;
            }
            return endpointUrl;
        }

        public static System.ServiceModel.Channels.Binding CreateNamedPipesBinding()
        {
            var binding = new NetNamedPipeBinding();
            binding.MaxBufferPoolSize = 50 * 1024 * 1024; // 50 mb
            binding.MaxBufferSize = 50 * 1024 * 1024;
            binding.MaxReceivedMessageSize = 50 * 1024 * 1024;
            return binding;
        }

        public static System.ServiceModel.Channels.Binding CreateTcpStreamingBinding()
        {
            var binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Streamed;
            binding.MaxReceivedMessageSize = long.MaxValue;
            binding.ReceiveTimeout = TimeSpan.FromMinutes(10); // 10 mins
            binding.SendTimeout = TimeSpan.FromMinutes(10); // 10 mins
            return binding;
        }

        public static System.ServiceModel.Channels.Binding CreateTcpBufferedBinding()
        {
            var binding = new NetTcpBinding();
            binding.TransferMode = TransferMode.Buffered;
            binding.MaxReceivedMessageSize = 100 * 1024 * 1024; // 100 MB
            binding.ReceiveTimeout = TimeSpan.FromMinutes(10); // 10 mins
            binding.SendTimeout = TimeSpan.FromMinutes(10); // 10 mins
            return binding;
        }
    }
}
