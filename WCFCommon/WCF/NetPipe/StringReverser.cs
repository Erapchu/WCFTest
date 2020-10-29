using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.WCF.NetPipe
{
    public class StringReverser : IStringReverser
    {
        public string ReverseString(string value)
        {
            char[] retVal = value.ToCharArray();
            int idx = 0;
            for (int i = value.Length - 1; i >= 0; i--)
                retVal[idx++] = value[i];

            return new string(retVal);
        }
    }
}
