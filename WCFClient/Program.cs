using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
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
            ChannelFactory<IStringDuplicator> pipeFactory = new ChannelFactory<IStringDuplicator>(
                new NetTcpBinding(), 
                new EndpointAddress("net.tcp://duplicator/TcpDuplicate"));

            IStringDuplicator pipeProxy = pipeFactory.CreateChannel();

            Console.WriteLine("Created Net Tcp channel on endpoint: \"net.tcp://duplicator/TcpDuplicate\"");

            while (true)
            {
                string str = Console.ReadLine();
                Console.WriteLine("Server: " + pipeProxy.MakeDuplicate(str));
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
