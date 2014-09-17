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

            var wayPoint = new WayPoint("Windisch", 47.479319847061966, 8.212966918945312);
            Console.WriteLine("{0}: {1}/{2}", wayPoint.Name, wayPoint.Latitude, wayPoint.Longitude);

            Console.ReadKey();
        }
    }
}
