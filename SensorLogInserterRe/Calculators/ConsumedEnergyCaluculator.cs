using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class ConsumedEnergyCaluculator
    {
        public static double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, int efficiency)
        {
            double consumedEnergy;

            // 力行時
            if (drivingPower >= 0)
            {
                consumedEnergy = drivingPower / efficiency * 100 / car.InverterEfficiency;
            }
            // 回生時
            else
            {
                consumedEnergy = RegeneEnergyCalculator.CalcEnergy(drivingPower, vehicleSpeed, car, efficiency);
            }
            return consumedEnergy;
        }
    }
}
