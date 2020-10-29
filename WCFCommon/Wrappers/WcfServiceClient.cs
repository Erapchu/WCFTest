using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using WCFCommon.Interfaces;

namespace AutoUpdater.Common.Communications
{
    public class WcfServiceClient<T> where T : IWcfBase
    {
        public Binding Binding { get; protected set; }
        public string ServiceUrl { get; protected set; }
        public object CallbackInstance { get; protected set; }
        protected bool fAutoClose = false;
        protected ChannelFactory<T> fChannelFactory = null;
        protected T fChannel = default(T);
        protected T Channel
        {
            get
            {
                if (fChannel == null)
                    fChannel = CreateChannel();
                return fChannel;
            }
        }

        protected bool fSkipErrorTrace = true;
        public bool SkipErrorTrace { get { return fSkipErrorTrace; } set { fSkipErrorTrace = value; } }

        public WcfServiceClient(Binding binding, string serviceUrl, object callbackInstance = null, bool autoClose = true)
        {
            Binding = binding;
            ServiceUrl = serviceUrl;
            CallbackInstance = callbackInstance;
            fAutoClose = autoClose;
        }

        public bool Call(Action<T> action)
        {
            bool res = false;
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        var channel = Channel;
                        channel.ValidateConnection();

                        //if (CallbackInstance != null)
                        //    channel.SetNotifierCallback();

                        action?.Invoke(channel);
                        res = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        //if (SkipErrorTrace)
                        //    Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                        //else
                        //    Logger.Error(ex, "");
                        Close();
                    }
                }
            }
            finally
            {
                InternalClose();
            }
            return res;
        }

        public TResult Call<TResult>(Func<T, TResult> action)
        {
            TResult res = default(TResult);
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        var channel = Channel;
                        channel.ValidateConnection();

                        //if (CallbackInstance != null)
                        //    channel.SetNotifierCallback();

                        if (action != null)
                            res = action.Invoke(channel);
                        break;
                    }
                    catch (Exception ex)
                    {
                        //if (SkipErrorTrace)
                        //    Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                        //else
                        //    Logger.Error(ex, "");
                        Close();
                    }
                }
            }
            finally
            {
                InternalClose();
            }
            return res;
        }

        private T CreateChannel()
        {
            var instanceContext = new InstanceContext(CallbackInstance);
            if (CallbackInstance != null)
                fChannelFactory = new DuplexChannelFactory<T>(instanceContext, Binding, new EndpointAddress(ServiceUrl));
            else
                fChannelFactory = new ChannelFactory<T>(Binding, new EndpointAddress(ServiceUrl));
            fChannelFactory.Open();
            return fChannelFactory.CreateChannel();
        }

        public void Close()
        {
            bool savedAutoClose = fAutoClose;
            try
            {
                if (fChannelFactory != null && fChannelFactory.State != CommunicationState.Closed)
                {
                    fAutoClose = true;
                    InternalClose();
                }
            }
            catch (Exception ex)
            {
                //if (SkipErrorTrace)
                //    Logger.Warn("{0}: {1}", ex.GetType().Name, ex.Message);
                //else
                //    Logger.Warn(ex, "");
            }
            finally
            {
                fAutoClose = savedAutoClose;
                fChannelFactory = null;
                fChannel = default(T);
            }
        }

        private void InternalClose()
        {
            if (fAutoClose && fChannelFactory != null && fChannelFactory.State != CommunicationState.Closed)
            {
                try
                {
                    fChannelFactory.Close();
                }
                catch (Exception ex)
                {
                    //Logger.Error("{0}: {1}", ex.GetType().Name, ex.Message);
                }
                finally
                {
                    fChannelFactory = null;
                    fChannel = default(T);
                }
            }
        }
    }
}
