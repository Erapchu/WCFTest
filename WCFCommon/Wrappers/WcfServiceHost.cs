using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdater.Common.Communications
{
    public class WcfServiceHost
    {
        public object SingletonInstance { get; protected set; }
        public Type ServiceType { get; protected set; }
        public Type ContractType { get; protected set; }
        public Binding Binding { get; protected set; }
        public string ServiceUrl { get; protected set; }

        public ServiceHost ServiceHost { get; protected set; }

        protected bool fSkipErrorTrace = true;
        public bool SkipErrorTrace { get { return fSkipErrorTrace; } set { fSkipErrorTrace = value; } }

        public WcfServiceHost(object singletonInstance, Type contractType, Binding binding, string serviceUrl)
        {
            SingletonInstance = singletonInstance;
            ContractType = contractType;
            Binding = binding;
            ServiceUrl = serviceUrl;
        }

        public WcfServiceHost(Type serviceType, Type contractType, Binding binding, string serviceUrl)
        {
            ServiceType = serviceType;
            ContractType = contractType;
            Binding = binding;
            ServiceUrl = serviceUrl;
        }

        public void InitHostEndpoint(string endpointName = null)
        {
            try
            {
                if (SingletonInstance != null)
                    ServiceHost = new ServiceHost(SingletonInstance, new Uri[] { new Uri(ServiceUrl) });
                else
                    ServiceHost = new ServiceHost(ServiceType, new Uri[] { new Uri(ServiceUrl) });

                string endpointUrl = WcfServiceHelper.GetEndpointUrl(ServiceUrl, endpointName);
                ServiceHost.AddServiceEndpoint(ContractType, Binding, endpointUrl);
                ServiceHost.Open();
            }
            catch (Exception ex)
            {
                ServiceHost = null;

                //if (SkipErrorTrace)
                //    Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                //else
                //    Logger.Error(ex, "");
            }
        }

        public void CloseHostEndpoint()
        {
            if (ServiceHost != null)
            {
                if (ServiceHost.State == CommunicationState.Opened)
                {
                    try
                    {
                        ServiceHost.Close();
                    }
                    catch (Exception ex)
                    {
                        //if (SkipErrorTrace)
                        //    Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                        //else
                        //    Logger.Error(ex, "");
                    }
                }

                ServiceHost = null;
            }
        }

        public static bool ValidateAndCall<T>(T callback, Action<T> action) where T : class
        {
            bool res = false;
            try
            {
                //Logger.Info("Validating callback...");
                if (callback != null)
                {
                    //Logger.Info("Validating connection is alive...");
                    bool isValid = false;
                    try
                    {
                        var methodInfo = callback.GetType().GetMethod("ValidateConnection");
                        if (methodInfo != null)
                            methodInfo.Invoke(callback, new object[0]);
                        isValid = true;
                    }
                    catch (Exception ex)
                    {
                        //Logger.Warn("Callback channel validation error - {0}...", ex.Message);
                    }
                    //Logger.Info("Callback connection status - {0}", isValid);
                    if (isValid)
                    {
                        //Logger.Info("Calling callback - {0}", action?.Method.Name);
                        action?.Invoke(callback);
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
            }

            return res;
        }
    }
}
