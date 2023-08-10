using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using WCFCommon.WCF.NetPipe;
using WCFCommon.WCF.NetTcp;
using WCFServer.NetPipe;
using WCFServer.NetTcp;

namespace WCFServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Type 1 to start Net.Pipe {nameof(IStringReverser)} WCF service host.");
            Console.WriteLine($"Type 2 to start Net.Tcp {nameof(IStringDuplicator)} WCF service host.");
            Console.WriteLine($"Type 3 to start Net.Tcp {nameof(ILargeObjectService)} WCF buffered service host.");
            Console.WriteLine($"Type 4 to start Net.Tcp {nameof(ILargeObjectService)} WCF streaming service host.");

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
                case 3:
                    StartWcfNetTcpStreaming(TransferMode.Buffered);
                    break;
                case 4:
                    StartWcfNetTcpStreaming(TransferMode.Streamed);
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
                /*var discoveryBehavior = new ServiceDiscoveryBehavior();
                discoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());
                host.Description.Behaviors.Add(discoveryBehavior);*/
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                //Open service
                host.Open();

                foreach (var channelDispatcher in host.ChannelDispatchers)
                    Console.WriteLine($"Endpoint Uri: {channelDispatcher.Listener?.Uri}");
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

        private static void StartWcfNetTcpStreaming(TransferMode transferMode)
        {
            using (var host = new ServiceHost(typeof(LargeObjectService), new Uri($"net.tcp://{Dns.GetHostName()}")))
            {
                //Net Tcp Binding
                var binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                binding.TransferMode = transferMode;
                // Not required for server (it can receive 64 KB by default)
                //if (transferMode == TransferMode.Streamed)
                //{
                //    binding.MaxReceivedMessageSize = 4_294_967_296L;
                //}

                //Endpoint (target)
                var duplicatorEndpoint = host.AddServiceEndpoint(typeof(ILargeObjectService), binding, "TcpLargeObject");
                duplicatorEndpoint.ListenUriMode = System.ServiceModel.Description.ListenUriMode.Unique;

                //Discovery behavior and endpoint
                host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
                /*var discoveryBehavior = new ServiceDiscoveryBehavior();
                discoveryBehavior.AnnouncementEndpoints.Add(new UdpAnnouncementEndpoint());
                host.Description.Behaviors.Add(discoveryBehavior);*/
                host.AddServiceEndpoint(new UdpDiscoveryEndpoint());

                //Open service
                host.Open();

                foreach (var channelDispatcher in host.ChannelDispatchers)
                    Console.WriteLine($"Endpoint Uri: {channelDispatcher.Listener?.Uri}");
                Console.WriteLine("Service is available. Press <ENTER> to exit.");
                Console.ReadLine();

                //Close service
                host.Close();
            }
        }
    }
}

