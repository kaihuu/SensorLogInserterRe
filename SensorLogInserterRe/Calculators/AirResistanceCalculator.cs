using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class AirResistanceCalculator
    {
        public static double CalcForce(double rho, double Cd, double frontProjectedArea, double windSpeed)//空気抵抗力
        {
            return rho * Cd * frontProjectedArea * Math.Pow(windSpeed, 2) / 2;
        }

        public static double CalcPower(double rho, double Cd, double frontProjectedArea, double windSpeed, double vehicleSpeed)//空気抵抗による損失エネルギー，kWh/s
        {
            return CalcForce(rho, Cd, frontProjectedArea, windSpeed) * vehicleSpeed / 3600 / 1000;
        }
    }
}
