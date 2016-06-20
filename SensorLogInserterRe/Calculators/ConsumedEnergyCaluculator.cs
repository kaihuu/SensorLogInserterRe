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
        public static double CalcEnergy(double drivingPower, Car car, double vehicleSpeed)
        {
            double consumedEnergy;
            double drivingForce = drivingPower * 1000 * 3600 / vehicleSpeed / 3.6;
            double drivingTorque = drivingForce * car.TireRadius / car.ReductionRatio;

            // 力行時
            if (drivingPower >= 0)
            {
                consumedEnergy = drivingPower / EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque) * 100 / car.InverterEfficiency;
            }
            // 回生時
            else
            {
                consumedEnergy = RegeneEnergyCalculator.CalcEnergy(drivingPower, vehicleSpeed, car);
            }
            return consumedEnergy;
        }
    }
}
