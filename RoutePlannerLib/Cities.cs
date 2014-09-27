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
            TextReader reader = File.OpenText(filename);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] splitted = line.Split('\t');
                City city = new City(splitted[0], splitted[1], Convert.ToInt32(splitted[2].Trim()), Convert.ToDouble(splitted[3].Trim(), info), Convert.ToDouble(splitted[4].Trim(), info));
                CityList.Add(city);
            }
            return CityList.Count;
        }

        public List<City> FindNeighbours(WayPoint location, double distance)
        {
            List<City> neighbours = new List<City>();
            foreach (City city in CityList)
            {
                if (location.Distance(city.Location) <= distance)
                {
                    neighbours.Add(city);
                }
            }
            return neighbours;
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
            }
        }
    }
}
