using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class EquipmentEnergyCalculator
    {
        private static double OutwardEquipmentPower = 0.2;
        private static double HomewardEquipmentPower = 0.3;

        public static double CalcEquipmentEnergy(DateTime time)
        {
            // TODO サマータイムとか入れれば更にいいかもしれないけど、誤差の範囲内でしょうな

            if (time.Hour < 18)
                // [kWh/s]
                return OutwardEquipmentPower / 3600;
            else
                return HomewardEquipmentPower / 3600;
        }
    }
}
