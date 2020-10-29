using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WCFCommon.Helpers
{
    public static class ServerNameResolver
    {
        private static readonly string _configFileName = "config.txt";
        internal static string GetConfigPath() 
        {
            var codebaseUri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var parentFolder = Path.GetDirectoryName(codebaseUri.LocalPath);
            var configPath = Path.Combine(parentFolder, _configFileName);
            return configPath;
        }

        public static void SaveServerName(string serverName = null)
        {
            try
            {
                using (var sw = new StreamWriter(GetConfigPath()))
                {
                    sw.WriteLine(serverName ?? Dns.GetHostName());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public static string GetServerName()
        {
            try
            {
                var serverName = string.Empty;
                using (var sr = new StreamReader(GetConfigPath()))
                {
                    serverName = sr.ReadToEnd();
                }
                return serverName;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
        }
    }
}
