namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    using System;
    using System.Reflection;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;
    using System.Collections.Generic;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Export;
    using System.IO;

    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("Welcome to RoutePlanner ({0})", version);


            WayPoint wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine(wayPoint.ToString());
            WayPoint wpBern = new WayPoint("Bern", 46.9472221, 7.451202500000022);
            WayPoint wpTripolis = new WayPoint("Tripolis", 59.86062519999999, 17.650885199999948);
            Console.WriteLine("Distanz Bern-Tripolis: {0}km", wpBern.Distance(wpTripolis));

            City cBern = new City("Bern", "Schweiz", 75000, 47.479319847061966, 8.212966918945312);
            City c0 = new City("Mumbai", "India", 12383146, 18.96, 72.82);

            string serializedCity = string.Empty;
            using (StringWriter outstream = new StringWriter())
            {
                SimpleObjectWriter writer = new SimpleObjectWriter(outstream);
                writer.Next(cBern);
                serializedCity = outstream.ToString();
            }
            Console.WriteLine(serializedCity);

            using(StringReader inStream = new StringReader(serializedCity)) {
                SimpleObjectReader reader = new SimpleObjectReader(inStream);
                object o = reader.Next();
            }
        

            WayPoint wp = c0.Location;
            Cities c = new Cities();
            c.ReadCities("citiesTestDataLab2.txt");
            c.FindNeighbours(wp,2000);
            c.ReadCities("citiesTestDataLab2.txt");

            var routes = new Routes(c);
            var reqWatch = new RouteRequestWatcher();
            routes.RouteRequestEvent += reqWatch.LogRouteRequests;
            routes.FindShortestRouteBetween("Mumbai", "India", TransportModes.Rail);
            routes.FindShortestRouteBetween("Mumbai", "India", TransportModes.Rail);
            routes.FindShortestRouteBetween("India", "Mumbai", TransportModes.Rail);

            Console.WriteLine("City found: {0}", c.FindCity("Mumbai").Name);

            c.ReadCities("citiesTestDataLab4.txt");
            Routes r = new Routes(c);
            r.RouteRequestEvent += reqWatch.LogRouteRequests;
            r.ReadRoutes("linksTestDataLab4.txt");
            List<Link> l = r.FindShortestRouteBetween("Zürich", "Winterthur", TransportModes.Rail);
            foreach (Link link in l)
            {
                Console.WriteLine("from {0} to {1} in {2}", link.FromCity.Name, link.ToCity.Name, link.Distance);
            }
            Console.ReadKey();

            City zurich = c.FindCity("Zürich");
            City winterthur = c.FindCity("Winterthur");
            ExcelExchange export = new ExcelExchange();
            export.WriteToFile("Test.xls", zurich, winterthur,l);
            
        }
    }
}
