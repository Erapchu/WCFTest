using WCFCommon;
using WCFCommon.WCF.NetPipe;

namespace WCFServer.NetPipe
{
    public class StringReverser : IStringReverser
    {
        private string _lastString;

        public string ReverseString(string value)
        {
            _lastString = value;

            char[] retVal = value.ToCharArray();
            int idx = 0;
            for (int i = value.Length - 1; i >= 0; i--)
                retVal[idx++] = value[i];

            return new string(retVal);
        }

        public Class1 TestMethod(Class1 test)
        {
            return test;
        }
    }
}
