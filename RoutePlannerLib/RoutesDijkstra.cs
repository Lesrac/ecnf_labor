namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Threading;
    using System.Linq;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
    using System.Diagnostics;

    public class RoutesDijkstra : Routes 
    {
        private static readonly TraceSource logger = new TraceSource("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.RoutesDijkstra");
        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public RoutesDijkstra(Cities cities) : base(cities)
        {
        }

        #region Lab04: Dijkstra implementation
        public override List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode, IProgress<string> progress = null)
        {
            var citiesBetween = FindCitiesBetween(fromCity, toCity);
            NotifyProgress(progress, "FindCitiesBetween done");
            if (citiesBetween == null || citiesBetween.Count < 1 || routes == null || routes.Count < 1)
            {
                return null;
            }

            var source = citiesBetween[0];
            var target = citiesBetween[citiesBetween.Count - 1];
            NotifyObservers(source, target, mode);
            Dictionary<City, double> dist;
            Dictionary<City, City> previous;
            var q = FillListOfNodes(citiesBetween, out dist, out previous);
            NotifyProgress(progress, "FillListOfNodes done");
            dist[source] = 0.0;

            // the actual algorithm
            previous = SearchShortestPath(mode, q, dist, previous);
            NotifyProgress(progress, "Searching shortest path done");

            // create a list with all cities on the route
            var citiesOnRoute = GetCitiesOnRoute(source, target, previous);
            NotifyProgress(progress, "GetCitiesOnRoute done");
            // prepare final list if links
            List<Link> result = FindPath(citiesOnRoute, mode);
            NotifyProgress(progress, "FindPath done");

            return result;
        }

        private static List<City> FillListOfNodes(List<City> cities, out Dictionary<City, double> dist, out Dictionary<City, City> previous)
        {
            var q = new List<City>(); // the set of all nodes (cities) in Graph ;
            dist = new Dictionary<City, double>();
            previous = new Dictionary<City, City>();
            foreach (var v in cities)
            {
                dist[v] = double.MaxValue;
                previous[v] = null;
                q.Add(v);
            }

            return q;
        }

        /// <summary>
        /// Searches the shortest path for cities and the given links
        /// </summary>
        /// <param name="mode">transportation mode</param>
        /// <param name="q"></param>
        /// <param name="dist"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        private Dictionary<City, City> SearchShortestPath(TransportModes mode, List<City> q, Dictionary<City, double> dist, Dictionary<City, City> previous)
        {
            while (q.Count > 0)
            {
                City u = null;
                var minDist = double.MaxValue;
                // find city u with smallest dist
                foreach (var c in q)
                    if (dist[c] < minDist)
                    {
                        u = c;
                        minDist = dist[c];
                    }

                if (u != null)
                {
                    q.Remove(u);
                    foreach (var n in FindNeighbours(u, mode))
                    {
                        var l = FindLink(u, n, mode);
                        var d = dist[u];
                        if (l != null)
                            d += l.Distance;
                        else
                            d += double.MaxValue;

                        if (dist.ContainsKey(n) && d < dist[n])
                        {
                            dist[n] = d;
                            previous[n] = u;
                        }
                    }
                }
                else
                    break;

            }

            return previous;
        }


        /// <summary>
        /// Finds all neighbor cities of a city. 
        /// </summary>
        /// <param name="city">source city</param>
        /// <param name="mode">transportation mode</param>
        /// <returns>list of neighbor cities</returns>
        private List<City> FindNeighbours(City city, TransportModes mode)
        {
            var neighbors = new List<City>();
            foreach (var r in routes)
                if (mode.Equals(r.TransportMode))
                {
                    if (city.Equals(r.FromCity))
                        neighbors.Add(r.ToCity);
                    else if (city.Equals(r.ToCity))
                        neighbors.Add(r.FromCity);
                }

            return neighbors;
        }

        private List<City> GetCitiesOnRoute(City source, City target, Dictionary<City, City> previous)
        {
            var citiesOnRoute = new List<City>();
            var cr = target;
            while (previous[cr] != null)
            {
                citiesOnRoute.Add(cr);
                cr = previous[cr];
            }
            citiesOnRoute.Add(source);

            citiesOnRoute.Reverse();
            return citiesOnRoute;
        }
        #endregion

    }
}
