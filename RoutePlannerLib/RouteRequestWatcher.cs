using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestWatcher
    {
        public Dictionary<string, int> requests = new Dictionary<string, int>();

        public int GetCityRequests(string cityName)
        {
            try
            {
                return requests[cityName];
            }
            catch
            {
                return 0;
            }
        }

        public void LogRouteRequests(object sender, RouteRequestEventArgs args)
        {
            string toCity = args.ToCity.Name;
            if (requests.ContainsKey(toCity))
            {
                requests[toCity] += 1;
            } else {
                requests.Add(args.ToCity.Name, 1);
            }

            Console.WriteLine("Current Request state");
            Console.WriteLine("---------------------");
            foreach(string cityName in requests.Keys) {
                Console.WriteLine("ToCity: {0} has been requested {1} times", cityName, GetCityRequests(cityName));
            }
            Console.WriteLine();
        }
    }
}
