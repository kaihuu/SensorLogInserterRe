using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class HeadingCalculator
    {
        public static double CalcHeading(double latitudeFirst, double longitudeFirst, double latitudeSecond, double longitudeSecond)
        {
            double heading = 0;
            double convertedLatFirst = latitudeFirst * Math.PI / 180;
            double convertedLatSecond = latitudeSecond * Math.PI / 180;
            double convertedLongFirst = longitudeFirst * Math.PI / 180;
            double convertedLongSecond = longitudeSecond * Math.PI / 180;

            double y = Math.Cos(convertedLatSecond) * Math.Sin(convertedLongSecond - convertedLongFirst);
            double x = Math.Cos(convertedLatFirst) * Math.Sin(convertedLatSecond) - Math.Sin(convertedLatFirst) * Math.Cos(convertedLatSecond) * Math.Cos(convertedLongSecond - convertedLongFirst);

            heading = Math.Atan2(y, x) * 180 / Math.PI;

            if (heading < 0)
            {
                heading = heading + 360;
            }

            return heading;
        }
    }
}
