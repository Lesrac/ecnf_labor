using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class City
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public WayPoint Location { get; set; }

        public City(string _name, string _country, int _population, double _latitude, double _longitude)
        {
            Name = _name;
            Country = _country;
            Population = _population;
            Location = new WayPoint(_name, _latitude, _longitude);
        }

        public bool Equals(City obj)
        public override int GetHashCode()
        {
            int result = 0;
            result += (Name == null ? 0 : Name.GetHashCode()) * 13;
            result += (Country == null ? 0 : Country.GetHashCode()) * 17;
            result += Population * 19;
            result += (Location == null ? 0 : Location.GetHashCode()) * 19;

            return result;
        }
            {
                return false;
            }
            return this.Name.Equals(obj.Name) && this.Country.Equals(obj.Country);
        }

    }
}
