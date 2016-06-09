using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class LostEnergyCalculator
    {
        static public double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, double inverterEfficiency, double maxDrivingForce, double maxDrivingPower,
            double rho, double windspeed, double myu, double theta)
        {
            double regeneEnergy = RegeneEnergyCalculator.CalcEnergy(drivingPower, vehicleSpeed, maxDrivingForce, maxDrivingPower, car, inverterEfficiency);
            return Math.Abs(ConvertLossCaluculator.CalcEnergy(drivingPower ,car, vehicleSpeed, inverterEfficiency, maxDrivingForce, maxDrivingPower))
                + RegeneLossCalculator.CalcEnergy(drivingPower,regeneEnergy,car,vehicleSpeed,inverterEfficiency)
                + AirResistanceCalculator.CalcPower(rho, car.CdValue, car.FrontalProjectedArea, windspeed, vehicleSpeed)
                + RollingResistanceCalculator.CalcPower(myu, car.Weight, theta, vehicleSpeed);
        }
    }
}
