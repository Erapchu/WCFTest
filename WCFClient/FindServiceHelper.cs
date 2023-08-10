using System;
using System.ServiceModel.Discovery;

namespace WCFClient
{
    internal static class FindServiceHelper
    {
        internal static EndpointDiscoveryMetadata FindService<T>()
        {
            //Create Discovery Client
            DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());

            //Create Find Criteria
            var findCriteria = new FindCriteria(typeof(T))
            {
                Duration = TimeSpan.FromSeconds(1)
            };

            //Searching
            FindResponse findResponse = null;
            for (int i = 0; i < 11; i++)
            {
                Console.WriteLine($"{i + 1} attempt to find server with duration {findCriteria.Duration}");
                findResponse = discoveryClient.Find(findCriteria);
                if (findResponse.Endpoints.Count > 0)
                    break;
                findCriteria.Duration += TimeSpan.FromSeconds(1);
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
    }
}
