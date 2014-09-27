namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    using System;
    using System.Reflection;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib;

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

            Console.ReadKey();
        }
    }
}
