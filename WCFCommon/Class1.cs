using System.Runtime.Serialization;

namespace WCFCommon
{
    [DataContract]
    public class Class1
    {
        [DataMember]
        public string Test { get; init; }
    }
}
