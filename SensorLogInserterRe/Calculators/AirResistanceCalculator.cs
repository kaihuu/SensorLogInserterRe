using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class AirResistanceCalculator
    {
        public static double CalcAirResistance(double rho, double Cd, double FrontProjectedArea, double windSpeed)
        {
            return rho * Cd* FrontProjectedArea * Math.Pow(windSpeed, 2) / 2;
        }
    }
}
