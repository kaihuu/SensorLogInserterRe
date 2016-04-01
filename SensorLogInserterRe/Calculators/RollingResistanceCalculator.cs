using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class RollingResistanceCalculator
    {
        public static double CalcRollingResistance(double myu,double vehicleMass,double theta)
        {
            return myu * vehicleMass * Math.Cos(theta)* 9.80665;
        }
    }
}
