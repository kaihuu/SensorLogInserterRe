using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    static class AccResistanceCaluculator
    {
        public static double CalcAccRessistanceForce(double acc, double vehicleMass, double vehicleRollingMass)//加速抵抗力
        {
            return (vehicleMass + vehicleRollingMass) * acc;
        }
        public static double CalcAccResistancePower(double vehicleSpeedBefore, double vehicleSpeedThis, double vehicleMass, double samplingTime)//加速抵抗による損失エネルギー，kWh/s,萩本モデル
        {
            return vehicleMass * (Math.Pow(vehicleSpeedThis, 2) - Math.Pow(vehicleSpeedBefore, 2)) / 2 / samplingTime / 3600 / 1000;
        }
    }
}
