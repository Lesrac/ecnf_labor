using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class Cities
    {
        public List<City> CityList { get; set; }
        public int Count { get; set; }
        public int ReadCities(string filename)
        {
            CityList = new List<City>();
            CultureInfo info = CultureInfo.GetCultureInfo("en-GB");
            using (TextReader reader = File.OpenText(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] splitted = line.Split('\t');
                    City city = new City(splitted[0], splitted[1], Convert.ToInt32(splitted[2].Trim()), Convert.ToDouble(splitted[3].Trim(), info), Convert.ToDouble(splitted[4].Trim(), info));
                    this[CityList.Count] = city;
                }
            }
            return CityList.Count;
        }

        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            /*var query = from city in CityList
                        where location.Distance(city.Location) <= distance
                        orderby location.Distance(city.Location)
                        select city;
            return new List<City>(query);
            */
            List<City> neighbours = new List<City>();
            foreach (City city in CityList)
            {
                double dist = location.Distance(city.Location);
                Console.WriteLine("Distance: {0}", dist);
                if (dist <= distance)
                {
                    neighbours.Add(city);
                }
            }
            return (neighbours.OrderBy( city => location.Distance(city.Location))).ToList();
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
    }
}
