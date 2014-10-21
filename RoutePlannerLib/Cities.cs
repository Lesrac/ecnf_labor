namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util;

    public class Cities
    {
        public List<City> CityList { get; set; }
        public int Count { get; set; }

        public int ReadCities(string filename)
        {
            CityList = new List<City>();
            CultureInfo info = CultureInfo.GetCultureInfo("en-GB");
            using (TextReader reader = new StreamReader(filename))
            {
                IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');
                foreach (string[] cs in citiesAsStrings)
                {
                    City city = new City(cs[0].Trim(), cs[1].Trim(), int.Parse(cs[2]), Convert.ToDouble(cs[3].Trim(), info), Convert.ToDouble(cs[4].Trim(), info));
                    this[CityList.Count] = city;
                }
            }
            return CityList.Count;
        }


        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            return CityList
                .Where(c => location.Distance(c.Location) <= distance)
                .OrderBy(c => location.Distance(c.Location))
                .ToList();
        }

        public City FindCity(string cityName)
        {
            return CityList.Find(delegate(City c)
            {
                return string.Compare(c.Name, cityName, true) == 0;
            });
        }

        public City this[int index]
        {
            get
            {
                if (index >= 0 && index < CityList.Count)
                {
                    return CityList[index];
                }
                return null;
            }
            set
            {
                if (index >= 0 && index < CityList.Count)
                {
                    CityList[index] = value;
                    Count++;
                }
                else if (index == CityList.Count)
                {
                    CityList.Add(value);
                    CityList[index] = value;
                    Count++;
                }
            }
        }

        #region
        /// <summary>
        /// Find all cities between 2 cities 
        /// </summary>
        /// <param name="from">source city</param>
        /// <param name="to">target city</param>
        /// <returns>list of cities</returns>
        public List<City> FindCitiesBetween(City from, City to)
        {
            var foundCities = new List<City>();
            if (from == null || to == null)
                return foundCities;

            foundCities.Add(from);

            var minLat = Math.Min(from.Location.Latitude, to.Location.Latitude);
            var maxLat = Math.Max(from.Location.Latitude, to.Location.Latitude);
            var minLon = Math.Min(from.Location.Longitude, to.Location.Longitude);
            var maxLon = Math.Max(from.Location.Longitude, to.Location.Longitude);

            // renames the name of the "cities" variable to your name of the internal City-List
            foundCities.AddRange(CityList.FindAll(c =>
                c.Location.Latitude > minLat && c.Location.Latitude < maxLat
                        && c.Location.Longitude > minLon && c.Location.Longitude < maxLon));

            foundCities.Add(to);
            return foundCities;
        }
        #endregion
    }
}
