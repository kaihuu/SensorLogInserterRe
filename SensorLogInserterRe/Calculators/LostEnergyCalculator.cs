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
        public static double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, double rho, double windspeed, double myu, double theta)
        {
            double regeneEnergy = RegeneEnergyCalculator.CalcEnergy(drivingPower, vehicleSpeed, car);
            return Math.Abs(ConvertLossCaluculator.CalcEnergy(drivingPower ,car, vehicleSpeed))
                + RegeneLossCalculator.CalcEnergy(drivingPower,regeneEnergy,car,vehicleSpeed)
                + AirResistanceCalculator.CalcPower(rho, car.CdValue, car.FrontalProjectedArea, windspeed, vehicleSpeed)
                + RollingResistanceCalculator.CalcPower(myu, car.Weight, theta, vehicleSpeed);
        }
    }
}
