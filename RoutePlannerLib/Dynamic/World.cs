namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Dynamic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Dynamic;

    public class World : DynamicObject
    {
        Cities cities;

        public World() : base()
        {
        }

        public World(Cities cities) : base()
        {
            this.cities = cities;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            string cityName = binder.Name;
            City c = cities.FindCity(cityName);
            if (c != null)
            {
                result = c;
            }
            else
            {
                result = string.Format("The city \"{0}\" does not exist!", cityName);
            }

            return true;
        }

    }
}
