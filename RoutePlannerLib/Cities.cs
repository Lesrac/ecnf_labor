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
        public int Count {
            get
            {
                return CityList.Count;
            }
        }
        }

        public int ReadCities(string filename)
        {
            if (CityList == null)
            {
                CityList = new List<City>();
            }
            CultureInfo info = CultureInfo.GetCultureInfo("en-GB");
            using (TextReader reader = new StreamReader(filename))
            {
                int oldCount = CityList.Count();
                IEnumerable<string[]> citiesAsStrings = reader.GetSplittedLines('\t');
                CityList.AddRange(
                    citiesAsStrings
                    .Select(cs => new City (
                            cs[0].Trim(), // City Name
                            cs[1].Trim(), // Country
                            int.Parse(cs[2]), // population
                            double.Parse(cs[3].Trim(), info),    // latitude
                            double.Parse(cs[4].Trim(), info)    // longitued
                        )
                    ));
                return CityList.Count - oldCount;
            }
            
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
            return CityList.Find(c => string.Compare(c.Name, cityName, true) == 0);
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
                }
                else if (index == CityList.Count)
                {
                    CityList.Add(value);
                    CityList[index] = value;
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
