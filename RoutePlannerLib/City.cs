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

        public City()
        {

        }

        public City(string _name, string _country, int _population, double _latitude, double _longitude)
        {
            Name = _name;
            Country = _country;
            Population = _population;
            Location = new WayPoint(_name, _latitude, _longitude);
        }

        public override int GetHashCode()
        {
            int result = 0;
            result += (Name == null ? 0 : Name.GetHashCode()) * 13;
            result += (Country == null ? 0 : Country.GetHashCode()) * 17;
            result += Population * 19;
            result += (Location == null ? 0 : Location.GetHashCode()) * 19;

            return result;
        }

        public override bool Equals(object obj)
        {
            // if obj is null or not an instance of city
            // then c will be null
            City c = obj as City;
            return Equals(c);
        }

        public bool Equals(City other)
        {
            // If parameter is null return false:
            if (other == null)
            {
                return false;
            }

            return IsEqualName(other) && IsEqualCountry(other);
        }

        private bool IsEqualName(City other)
        {
            return (this.Name == null && other.Name == null)
                || (this.Name != null && this.Name.Equals(other.Name));
        }

        private Boolean IsEqualCountry(City other)
        {
            return (this.Country == null && other.Country == null)
                || (this.Country != null && this.Country.Equals(other.Country));
        }

    }
}
