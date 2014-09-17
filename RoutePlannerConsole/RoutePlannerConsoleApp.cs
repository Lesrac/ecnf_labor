namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    using System;
    using System.Reflection;

    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("Welcome to RoutePlanner ({0})", version);
            Console.ReadKey();
        }
    }
}
