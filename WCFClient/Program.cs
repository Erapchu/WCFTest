using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Text;
using System.Threading.Tasks;
using WCFCommon.Helpers;
using WCFCommon.WCF.NetPipe;
using WCFCommon.WCF.NetTcp;

namespace WCFClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type 1 to start Net.Pipe WCF service client.");
            Console.WriteLine("Type 2 to start Net.Tcp WCF service client.");

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
            //Find service host
            var endpointDiscoveryMetadata = FindService();
            if (endpointDiscoveryMetadata is null)
            {
                Console.WriteLine("Please, firstly start a host.");
                Console.ReadLine();
                return;
            }
            foreach(var listenUri in endpointDiscoveryMetadata.ListenUris)
                Console.WriteLine($"Finded address: {listenUri}");

            //Endpoint address
            var availableEndpointAddress = endpointDiscoveryMetadata.ListenUris.FirstOrDefault();
            var endpointAddress = new EndpointAddress(availableEndpointAddress);

            //Net Tcp Binding
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;

            //Channel factory
            var tcpFactory = new ChannelFactory<IStringDuplicator>(binding, endpointAddress);
            tcpFactory.Endpoint.ListenUriMode = System.ServiceModel.Description.ListenUriMode.Unique;
            IStringDuplicator tcpProxy = tcpFactory.CreateChannel();

            Console.WriteLine($"Created Net Tcp channel on endpoint: \"{tcpFactory.Endpoint.ListenUri}\"");
            Console.WriteLine("Enter some text to send it on server:");

            while (true)
            {
                string str = Console.ReadLine();
                Console.WriteLine("Server: " + tcpProxy.MakeDuplicate(str));
            }
        }

        private static EndpointDiscoveryMetadata FindService()
        {
            //Create Discovery Client
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());

            //Create Find Criteria
            var findCriteria = new FindCriteria(typeof(IStringDuplicator))
            {
                Duration = TimeSpan.FromSeconds(5)
            };

            //Searching
            FindResponse findResponse = null;
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{i + 1} attempt to find server");
                findResponse = discoveryClient.Find(findCriteria);
                if (findResponse.Endpoints.Count > 0)
                    break;
            }
            discoveryClient.Close();

            if (findResponse.Endpoints.Count > 0)
            {
                return findResponse.Endpoints[0];
            }
            else
            {
                return null;
            }
        }

        private static void StartWcfNetPipe()
        {
            ChannelFactory<IStringReverser> pipeFactory = new ChannelFactory<IStringReverser>(
                new NetNamedPipeBinding(), 
                new EndpointAddress("net.pipe://localhost/PipeReverse"));

            IStringReverser pipeProxy = pipeFactory.CreateChannel();

            Console.WriteLine("Created Name Pipe channel on endpoint: \"net.pipe://localhost/PipeReverse\"");

            while (true)
            {
                string str = Console.ReadLine();
                Console.WriteLine("Server: " + pipeProxy.ReverseString(str));
            }
        }
    }
}
