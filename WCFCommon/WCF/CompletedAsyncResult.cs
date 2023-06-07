using System;
using System.Threading;

namespace WCFCommon.WCF
{
    // Simple async result implementation.
    public class CompletedAsyncResult<T> : IAsyncResult
    {
        public object AsyncState => Data;
        public WaitHandle AsyncWaitHandle => throw new Exception("The method or operation is not implemented.");
        public bool CompletedSynchronously => true;
        public bool IsCompleted => true;

        public T Data { get; }

        public CompletedAsyncResult(T data)
        {
            Data = data;
        }
    }
}
