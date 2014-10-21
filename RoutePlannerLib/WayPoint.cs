using System;
namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class WayPoint
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public WayPoint()
        {

        }

        public WayPoint(string _name, double _latitude, double _longitude)
        {
            Name = _name;
            Latitude = _latitude;
            Longitude = _longitude;
        }

        public WayPoint()
        {

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
            double sinStart = Math.Sin(Latitude * (Math.PI / 180));
            double sinTarget = Math.Sin(target.Latitude * (Math.PI / 180));
            double cosStart = Math.Cos(Latitude * (Math.PI / 180));
            double cosTarget = Math.Cos(target.Latitude * (Math.PI / 180));
            double cosDifference = Math.Cos(Longitude * (Math.PI / 180) - target.Longitude * (Math.PI / 180));
            double distance = radius * Math.Acos(sinStart * sinTarget + cosStart * cosTarget * cosDifference);
            return distance;
        }

        public static WayPoint operator +(WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude + rhs.Latitude, lhs.Longitude + rhs.Longitude);
        }

        public static WayPoint operator -(WayPoint lhs, WayPoint rhs)
        {
            return new WayPoint(lhs.Name, lhs.Latitude - rhs.Latitude, lhs.Longitude - rhs.Longitude);
        }
    }
}
