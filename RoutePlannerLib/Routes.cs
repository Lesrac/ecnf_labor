
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
    public abstract class Routes : IRoutes
    {
        private static readonly TraceSource logger = new TraceSource("Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Routes");
        protected List<Link> routes = new List<Link>();
        private Cities cities;
        public event RouteRequestHandler RouteRequestEvent;

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

        protected List<City> FindCitiesBetween(string fromCity, string toCity)
        {
            return cities.FindCitiesBetween(cities.FindCity(fromCity), cities.FindCity(toCity));
        }

        protected List<Link> FindPath(List<City> cities, TransportModes mode)
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

        protected abstract Link FindLink(City fromC, City toC, TransportModes mode);

        public abstract List<Link> FindShortestRouteBetween(string fromCity, string toCity, TransportModes mode);

        protected void NotifyObservers(City fromCity,City toCity,TransportModes mode) {
            if (RouteRequestEvent != null)
            {
                RouteRequestEvent(this, new RouteRequestEventArgs(fromCity, toCity, mode));
            }
        }
    }
}
