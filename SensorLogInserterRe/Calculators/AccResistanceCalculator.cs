using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class AccResistanceCalculator
    {
        //加速抵抗力
        public static double CalcForce(double acc, double vehicleMass, double vehicleRollingMass)
        {
            return (vehicleMass + vehicleRollingMass) * acc;
        }

        //加速抵抗による損失エネルギー，kWh/s, 萩本モデル
        public static double CalcPower(double vehicleSpeedBefore, double vehicleSpeedThis, double vehicleMass, double samplingTime)
        {
            return vehicleMass * (Math.Pow(vehicleSpeedThis, 2) - Math.Pow(vehicleSpeedBefore, 2)) / 2 / samplingTime / 3600 / 1000;
        }
    }
}
