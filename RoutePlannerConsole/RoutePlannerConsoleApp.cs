namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerConsole
{
    using System;
    using System.Reflection;

    class RoutePlannerConsoleApp
    {
        static void Main(string[] args)
        {
            /* FILE VERSION
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.FileVersion;
            Console.WriteLine("Welcome to RoutePlanner ({0})", version);
             */
            Version version = Assembly.GetEntryAssembly().GetName().Version;
            Console.WriteLine("Welcome to RoutePlanner ({0})", version);
            Console.ReadKey();
        }
    }
}
