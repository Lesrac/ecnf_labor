using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
    public class RouteRequestEventArgs : System.EventArgs
    {
        public readonly City FromCity;
        public readonly City ToCity;
        public readonly TransportModes Mode;

        public RouteRequestEventArgs(City fromCity, City toCity, TransportModes mode)
        {
            this.FromCity = fromCity;
            this.ToCity = toCity;
            this.Mode = mode;
        }
    }
}
