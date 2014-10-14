namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;

    public class RoutesFactory
    {
        static public IRoutes Create(Cities cities)
        {
            return Create(cities,Properties.Settings.Default.RouteAlgorithm);
        }

        static public IRoutes Create(Cities cities, string algorithmClassName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Module module = assembly.GetModules()[0];
            Type type = module.GetType(algorithmClassName);
            if (type == null)
            {
                return null;
            }
            ConstructorInfo ci = type.GetConstructor(new Type[] { typeof(Cities) });
            if(ci == null) {
                return null;
            }
            object factory = ci.Invoke(new object[]{cities});
            return factory as IRoutes;
        }
    }
}
