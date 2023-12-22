using System;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Threading.Tasks;
using WCFCommon.WCF.NetPipe;
using WCFCommon.WCF.NetTcp;

namespace WCFClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Type 1 to start Net.Pipe WCF {nameof(IStringReverser)} service client.");
            Console.WriteLine($"Type 2 to start Net.Tcp WCF {nameof(IStringDuplicator)} service client.");
            Console.WriteLine($"Type 3 to start Net.Tcp WCF {nameof(ILargeObjectService)} buffered service client.");
            Console.WriteLine($"Type 4 to start Net.Tcp WCF {nameof(ILargeObjectService)} streamed service client.");

            int result;
            while (!int.TryParse(Console.ReadLine(), out result)) { }
            switch (result)
            {
                case 1:
                    StartWcfNetPipe();
                    break;
                case 2:
                    {
                        var tcpProxy = StartWcfNetTcp<IStringDuplicator>(TransferMode.Buffered);
                        WorkWithDuplicator(tcpProxy);
                    }
                    break;
                case 3:
                    {
                        var tcpProxy = StartWcfNetTcp<ILargeObjectService>(TransferMode.Buffered, 4_294_967_296L);
                        WorkWithLargeDataStreaming(tcpProxy);
                    }
                    break;
                case 4:
                    {
                        var tcpProxy = StartWcfNetTcp<ILargeObjectService>(TransferMode.Streamed, 4_294_967_296L);
                        WorkWithLargeDataStreaming(tcpProxy);
                    }
                    break;
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
                var response = pipeProxy.TestMethod(new WCFCommon.Class1() { Test = str });
            }
        }

        private static T StartWcfNetTcp<T>(TransferMode transferMode, long maxReceivedMessageSize = 65536L)
        {
            //Find service host
            var endpointDiscoveryMetadata = FindServiceHelper.FindService<T>();
            if (endpointDiscoveryMetadata is null)
            {
                Console.WriteLine("Please, firstly start a host.");
                Console.ReadLine();
                return default;
            }
            foreach (var listenUri in endpointDiscoveryMetadata.ListenUris)
                Console.WriteLine($"Finded address: {listenUri}");

            //Endpoint address
            var availableEndpointAddress = endpointDiscoveryMetadata.ListenUris.FirstOrDefault();
            var endpointAddress = new EndpointAddress(availableEndpointAddress);

            //Net Tcp Binding
            var binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            binding.TransferMode = transferMode;
            binding.MaxReceivedMessageSize = maxReceivedMessageSize;

            //Channel factory
            var tcpFactory = new ChannelFactory<T>(binding, endpointAddress);
            tcpFactory.Endpoint.ListenUriMode = System.ServiceModel.Description.ListenUriMode.Unique;
            T tcpProxy = tcpFactory.CreateChannel();

            Console.WriteLine($"Created Net Tcp channel on endpoint: \"{tcpFactory.Endpoint.ListenUri}\"");

            return tcpProxy;
        }

        private static void WorkWithDuplicator(IStringDuplicator tcpProxy)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter some text to send it on server:");

                    string str = Console.ReadLine();
                    Console.WriteLine("Sync response: " + tcpProxy.MakeDuplicate(str));

                    var res = Task.Factory.FromAsync(tcpProxy.BeginServiceAsyncMethod(str, (a) => { }, null), (a) => tcpProxy.EndServiceAsyncMethod(a)).Result;
                    Console.WriteLine("Async response: " + res);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void WorkWithLargeDataStreaming(ILargeObjectService tcpProxy)
        {
            var largeObject = tcpProxy.GetLargeObject();
            var ms = new MemoryStream();
            //largeObject.CopyTo(ms);
            byte[] array = new byte[81920];
            int count;
            while ((count = largeObject.Read(array, 0, array.Length)) != 0)
            {
                ms.Write(array, 0, count);
            }
            var result = ms.ToArray();
        }
    }
}
