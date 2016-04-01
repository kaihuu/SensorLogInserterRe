using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class ClimbingResistanceCaluculator
    {
        public static double CalcClimbingResistance(double vehicleMass, double theta)
        {
            return vehicleMass * Math.Sin(theta) * 9.80665;
        }
    }
}
