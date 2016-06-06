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
        static public double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, double inverterEfficiency, double maxDrivingForce, double maxDrivingPower)
        {
            double consumedEnergy;
            double drivingForce = drivingPower * 1000 * 3600 / vehicleSpeed / 3.6;
            double drivingTorque = drivingForce * car.TireRadius / car.ReductionRatio;
            if (drivingPower >= 0)//力行時
            {
                consumedEnergy = drivingPower / EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque) * 100 / inverterEfficiency;
            }
            else//回生時
            {
                consumedEnergy = RegeneEnergyCalculator.CalcEnergy(drivingPower, vehicleSpeed, maxDrivingForce, maxDrivingPower, car, inverterEfficiency);
            }
            return consumedEnergy;
        }
    }
}
