using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFCommon.WCF.NetTcp;
using WCFCommon.WCF.NetPipe;
using System.ServiceModel.Channels;
using WCFCommon.Helpers;
using System.Net;
using System.ServiceModel.Discovery;

namespace WCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type 1 to start Net.Pipe WCF service host.");
            Console.WriteLine("Type 2 to start Net.Tcp WCF service host.");

            int result;
            while (!int.TryParse(Console.ReadLine(), out result)) { }
            switch (result)
            {
                case 1:
                    StartWcfNetPipe();
                    break;
                case 2:
                    StartWcfNetTcp();
                    break;
            }
        }

        private static void StartWcfNetTcp()
        {
            using (var host = new ServiceHost(typeof(StringDuplicator), new Uri($"net.tcp://{Dns.GetHostName()}")))
            {
                //Net Tcp Binding
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;

                //Endpoint (target)
                var duplicatorEndpoint = host.AddServiceEndpoint(typeof(IStringDuplicator), binding, "TcpDuplicate");
                duplicatorEndpoint.ListenUriMode = System.ServiceModel.Description.ListenUriMode.Unique;

                //Discovery behavior and endpoint
                host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                //Open service
                host.Open();

                foreach (var channelDispatcher in host.ChannelDispatchers)
                    Console.WriteLine($"Endpoint Uri: {channelDispatcher.Listener.Uri}");
                Console.WriteLine("Service is available. Press <ENTER> to exit.");
                Console.ReadLine();

                //Close service
                host.Close();
            }
        }

        private static void StartWcfNetPipe()
        {
            using (ServiceHost host = new ServiceHost(
                typeof(StringReverser), 
                new Uri("net.pipe://reverser")))
            {
                host.AddServiceEndpoint(typeof(IStringReverser),
                  new NetNamedPipeBinding(),
                  "PipeReverse");

                host.Open();

                foreach (var endpoint in host.Description.Endpoints)
                    Console.WriteLine($"Endpoint Uri: {endpoint.ListenUri}");

                Console.WriteLine("Service is available. Press <ENTER> to exit.");
                Console.ReadLine();

                host.Close();
            }
        }
    }
}

