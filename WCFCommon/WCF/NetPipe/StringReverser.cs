using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.WCF.NetPipe
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
    }
}
