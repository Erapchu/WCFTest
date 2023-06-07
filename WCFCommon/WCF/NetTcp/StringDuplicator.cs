using System;
using System.ServiceModel;
using System.Threading;

namespace WCFCommon.WCF.NetTcp
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class StringDuplicator : IStringDuplicator
    {
        public IAsyncResult BeginServiceAsyncMethod(string msg, AsyncCallback callback, object asyncState)
        {
            //Thread.Sleep(10000);
            Console.WriteLine("BeginServiceAsyncMethod called with: \"{0}\"", msg);
            return new CompletedAsyncResult<string>(msg);
        }

        public string EndServiceAsyncMethod(IAsyncResult result)
        {
            //Thread.Sleep(10000);
            CompletedAsyncResult<string> r = result as CompletedAsyncResult<string>;
            Console.WriteLine("EndServiceAsyncMethod called with: \"{0}\"", r.Data);
            return r.Data;
        }

        public string MakeDuplicate(string target)
        {
            return target + " " + target;
        }
    }
}
