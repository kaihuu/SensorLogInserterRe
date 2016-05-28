using SensorLogInserterRe.Calculators.CalculatorComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class ClimbingResistanceCalculator
    {
        //登坂抵抗力
        public static double CalcForce(double vehicleMass, double theta)
        {
            return vehicleMass * Math.Sin(theta) * Constants.GravityResistanceCoefficient;
        }

        //登坂抵抗による損失エネルギー, kWh/s
        public static double CalcPower(double vehicleMass, double theta, double vehicleSpeed)
        {
            return CalcForce(vehicleMass, theta) * vehicleSpeed / 3600 / 1000;
        }
    }
}
