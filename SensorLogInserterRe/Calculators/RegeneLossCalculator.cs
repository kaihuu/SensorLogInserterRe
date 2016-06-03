﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class RegeneLossCalculator
    {
        public static double CalcEnergy(double drivingPower,double regeneEnergy,Car car, double vehicleSpeed)
        {
            double drivingTorque = drivingPower * 1000 * 3600 / vehicleSpeed / 3.6 * car.TireRadius / car.ReductionRatio;
            double regeneLoss = Math.Abs(drivingPower - regeneEnergy / EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque));

            return regeneLoss;
        }
    }
}