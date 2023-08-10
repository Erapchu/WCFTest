using System;
using System.IO;
using System.ServiceModel;
using WCFCommon.WCF.NetPipe;

namespace WCFServer.NetTcp
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    internal class LargeObjectService : ILargeObjectService
    {
        public Stream GetLargeObject()
        {
            const string path = "testfile.txt";
            var fullPath = Path.GetFullPath(path);
            if (!File.Exists(fullPath))
                return null;
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, 65536);
            //var stream = File.OpenRead(fullPath);
            return stream;
        }
    }
}
