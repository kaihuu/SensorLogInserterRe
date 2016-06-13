﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class ConvertLossCaluculator
    {
        public static double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, double inverterEfficiency, double maxDrivingForce, double maxDrivingPower)
        {
            double convertLoss;
            double drivingForce = drivingPower * 1000 * 3600 / vehicleSpeed / 3.6;
            double drivingTorque = drivingForce * car.TireRadius / car.ReductionRatio;
            if(drivingPower >= 0)
            {
                convertLoss = ConsumedEnergyCaluculator.CalcEnergy(drivingPower, car, vehicleSpeed, inverterEfficiency, maxDrivingForce, maxDrivingPower)
                    * (1 - EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque) / 100 * inverterEfficiency);
            }
            else
            {
                convertLoss = ConsumedEnergyCaluculator.CalcEnergy(drivingPower, car, vehicleSpeed, inverterEfficiency, maxDrivingForce, maxDrivingPower)
                    * (1 - 1 / EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque) * 100 / inverterEfficiency);
            }
            return convertLoss;
        }
    }
}