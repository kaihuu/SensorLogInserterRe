using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class SpeedCalculator
    {

        public static Tuple<GeoCoordinate, GeoCoordinate> CalcSpeed(GeoCoordinate geoFirst, GeoCoordinate geoSecond)
        {
            return new Tuple<GeoCoordinate, GeoCoordinate>(new GeoCoordinate(), new GeoCoordinate());
        }

    }
}
