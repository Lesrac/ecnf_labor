namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    public class RoutesFactory
    {
        static public IRoutes Create(Cities cities)
        {
            return Create(cities, Properties.Settings.Default.RouteAlgorithm);
        }
        static public IRoutes Create(Cities cities, string algorithmClassName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = assembly.GetType(algorithmClassName);
            object instance = null;
            if (type == null)
            {
                return null;
            }
            instance = Activator.CreateInstance(type, cities);
            if (instance is IRoutes)
            {
                return (IRoutes)instance;
            }
            return null;
        }
    }
}
