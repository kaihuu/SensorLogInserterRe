using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class AirResistanceCalculator
    {
        //空気抵抗力
        public static double CalcForce(double rho, double Cd, double frontProjectedArea, double windSpeed)
        {
            return rho * Cd * frontProjectedArea * Math.Pow(windSpeed, 2) / 2;
        }

        //空気抵抗による損失エネルギー，kWh/s
        public static double CalcPower(double rho, double Cd, double frontProjectedArea, double windSpeed, double vehicleSpeed)
        {
            return CalcForce(rho, Cd, frontProjectedArea, windSpeed) * vehicleSpeed / 3600 / 1000;
        }
    }
}
