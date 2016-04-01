using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class AirResistanceCalculator
    {
        public static double CalcAirResistance(double myu,double vehicleMass,double theta)
        {
            return myu * vehicleMass * Math.Cos(theta) * 9.80665;
        }
    }
}
