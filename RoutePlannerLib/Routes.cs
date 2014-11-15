
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

    public delegate void RouteRequestHandler(object sender, RouteRequestEventArgs e);
    /// <summary>
    /// Manages a routes from a city to another city.
    /// </summary>
    public class Routes : IRoutes
    {
        private static readonly TraceSource logger = new TraceSource("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Routes");
        public event RouteRequestHandler RouteRequestEvent;
        List<Link> routes = new List<Link>();
        Cities cities;

        public int Count
        {
            get { return routes.Count; }
        }

        /// <summary>
        /// Initializes the Routes with the cities.
        /// </summary>
        /// <param name="cities"></param>
        public Routes(Cities cities)
        {
            this.cities = cities;
        }

        /// <summary>
        /// Reads a list of links from the given file.
        /// Reads only links where the cities exist.
        /// </summary>
        /// <param name="filename">name of links file</param>
        /// <returns>number of read route</returns>
        public int ReadRoutes(string filename)
        {
            logger.TraceInformation("ReadRoutes started");
            using (TextReader reader = new StreamReader(filename))
            {
                List<string[]> existingCities = reader.GetSplittedLines('\t')
                    .Where(s => cities.FindCity(s[0]) != null && cities.FindCity(s[1]) != null)
                    .ToList();

                List<City[]> cits = existingCities.Select(str =>
                     str.Select(
                        s => cities.FindCity(s))
                        .ToArray()
                ).ToList();

                List<Link> links = cits.Select(
                    la => new Link(la[0], la[1], la[0].Location.Distance(la[1].Location), TransportModes.Rail))
                    .ToList();

                routes.AddRange(links);
            }
            logger.TraceInformation("ReadRoutes ended");
            return Count;
        }

        public City[] FindCities(TransportModes transportMode)
        {
            var fromCities = routes
                    .Where(l => l.TransportMode.Equals(transportMode))
                    .Select(m => m.FromCity);

            var toCities = routes
                    .Where(l => l.TransportMode.Equals(transportMode))
                    .Select(m => m.ToCity);

            return fromCities.Union(toCities).ToArray<City>();
        }

        private List<City> FindCitiesBetween(string fromCity, string toCity)
        {
            return cities.FindCitiesBetween(cities.FindCity(fromCity), cities.FindCity(toCity));
        }

        private List<Link> FindPath(List<City> cities, TransportModes mode)
        {
            City fromCity = cities.First();
            List<Link> links = cities
                .Skip(1)
                .Select(c => {
                    Link l = FindLink(fromCity, c, mode);
                    fromCity = c;
                    return l;
                })
                .Where(c => c != null)
                .ToList(); 

            return links;
        }

        #region Lab04: Dijkstra implementation
        public List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode)
        {
            var citiesBetween = FindCitiesBetween(fromCity, toCity);
            if (citiesBetween == null || citiesBetween.Count < 1 || routes == null || routes.Count < 1)
            {
                return null;
            }

            var source = citiesBetween[0];
            var target = citiesBetween[citiesBetween.Count - 1];
            if (RouteRequestEvent != null)
            {
                RouteRequestEvent(this, new RouteRequestEventArgs(source, target, mode));
            }
            Dictionary<City, double> dist;
            Dictionary<City, City> previous;
            var q = FillListOfNodes(citiesBetween, out dist, out previous);
            dist[source] = 0.0;

            // the actual algorithm
            previous = SearchShortestPath(mode, q, dist, previous);

            // create a list with all cities on the route
            var citiesOnRoute = GetCitiesOnRoute(source, target, previous);
            // prepare final list if links
            return FindPath(citiesOnRoute, mode);
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

        private Link FindLink(City fromC, City toC, TransportModes mode)
        {

            return (from l in this.routes
                    where mode.Equals(l.TransportMode)
                    && ((fromC.Equals(l.FromCity) && toC.Equals(l.ToCity))
                         || (toC.Equals(l.FromCity) && fromC.Equals(l.ToCity)))
                    select new Link(fromC, toC, l.Distance, TransportModes.Rail)).FirstOrDefault();
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
