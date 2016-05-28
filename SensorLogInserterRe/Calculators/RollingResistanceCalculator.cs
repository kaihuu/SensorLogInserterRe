using SensorLogInserterRe.Calculators.CalculatorComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class RollingResistanceCalculator
    {
        //転がり抵抗力
        public static double CalcForce(double myu, double vehicleMass, double theta)
        {
            return myu * vehicleMass * Math.Cos(theta) * Constants.GravityResistanceCoefficient;
        }

        //転がり抵抗による損失エネルギー, kWh/s
        public static double CalcPower(double myu, double vehicleMass, double theta, double vehicleSpeed)
        {
            return CalcForce(myu, vehicleMass, theta) * vehicleSpeed / 1000 / 3600;
        }
    }
}
