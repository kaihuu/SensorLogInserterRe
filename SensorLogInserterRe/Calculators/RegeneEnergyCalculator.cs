using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class RegeneEnergyCalculator
    {
        public static double CalcRegeneEnergy(double drivingPower, double vehicleSpeed, double maxDrivingForce, double maxDrivingPower)
        {
            double regeneEnergy = 1000;
            if (drivingPower >= 0)
            {
               regeneEnergy = 0;
            }
            else
            {
               
            }
            return regeneEnergy;
        }
    }
}
