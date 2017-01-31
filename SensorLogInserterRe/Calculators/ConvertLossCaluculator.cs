using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class ConvertLossCaluculator
    {
        public static double CalcEnergy(double drivingPower, Car car, double vehicleSpeed, int efficiency)
        {
            double convertLoss;

            if(drivingPower >= 0)
            {
                convertLoss = ConsumedEnergyCaluculator.CalcEnergy(drivingPower, car, vehicleSpeed, efficiency)
                    * ( (1 - (efficiency + 0.0f) / 100 * car.InverterEfficiency) );
            }
            else
            {
                convertLoss = ConsumedEnergyCaluculator.CalcEnergy(drivingPower, car, vehicleSpeed, efficiency)
                    * ( (1 / (efficiency + 0.0f) * 100 / car.InverterEfficiency - 1) );
            }
            return convertLoss;
        }
    }
}
