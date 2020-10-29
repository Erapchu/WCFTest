using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.WCF.NetTcp
{
    public class StringDuplicator : IStringDuplicator
    {
        public string MakeDuplicate(string target)
        {
            return target + " " + target;
        }
    }
}
