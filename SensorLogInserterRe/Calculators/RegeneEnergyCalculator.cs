using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Calculators
{
    static class RegeneEnergyCalculator
    {
        public static double CalcEnergy(double drivingPower, double vehicleSpeed, Car car)
        {
            //制動力[N]
            double drivingForce = drivingPower * 1000 * 3600 / vehicleSpeed / 3.6;
            //限界回生力と限界回生エネルギーの時の回生力の低い方が変わるときの車速[km/h] 
            double speedC = car.MaxDrivingPower * 1000 / car.MaxDrivingForce / 3.6;
            double regeneEnergy = 0;
            double drivingTorque = drivingForce * car.TireRadius / car.ReductionRatio;

            //力行時
            if (drivingPower >= 0)
            {
               regeneEnergy = 0;
            }
            //回生時
            else
            {
                //車速が7km/hより小さいとき
                if (vehicleSpeed < 7)
                {
                    regeneEnergy = 0;
                }
                else if(vehicleSpeed >= 7 && drivingPower * 3600 >= car.MaxDrivingPower &&
                    drivingForce >= car.MaxDrivingForce)//車速が7km/hより大きく，回生エネルギー限界[kW]を制動エネルギー[kW]が超えず，
                   //回生による制動力[N]の限界を制動力[N]が超えない場合（負のため不等号逆転）
                {
                    regeneEnergy = drivingPower * EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque);
                }
                //限界回生力[N]を超えている場合
                else if (vehicleSpeed >=7 && vehicleSpeed <= speedC && drivingForce < car.MaxDrivingForce)
                {
                    regeneEnergy = car.MaxDrivingForce * vehicleSpeed * 3.6 * EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque); 
                }
                //回生エネルギー限界を超えている場合
                else if (vehicleSpeed > speedC && drivingPower * 3600 < car.MaxDrivingPower)
                {
                    regeneEnergy = car.MaxDrivingPower / 1000 * EfficiencyCalculator.GetInstance().GetEfficiency(car, vehicleSpeed, drivingTorque);
                }
                regeneEnergy = regeneEnergy / 100 * car.InverterEfficiency;
            }
            return regeneEnergy;
        }
    }
}
