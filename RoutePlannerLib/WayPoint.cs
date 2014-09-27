using System;
namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public override string ToString()
        {
            string val = String.Format("WayPoint: {0}/{1}", String.Format("{0:0.00}", Latitude), String.Format("{0:0.00}", Longitude));
            if (!String.IsNullOrWhiteSpace(Name))
            {
                val = String.Format("WayPoint: {0} {1}/{2}", Name, String.Format("{0:0.00}", Latitude), String.Format("{0:0.00}", Longitude));
            }
            return val;
        }

        public double Distance(WayPoint target)
        {
            int radius = 6371;
            double sinStart = Math.Sin(Latitude);
            double sinTarget = Math.Sin(target.Latitude);
            double cosStart = Math.Cos(Latitude);
            double cosTarget = Math.Cos(target.Latitude);
            double cosDifference = Math.Cos(Longitude - target.Longitude);
            double distance = radius * Math.Acos(sinStart * sinTarget + cosStart * cosTarget * cosDifference);
            return distance;
        }
    }
}
