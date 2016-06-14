using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;

namespace SensorLogInserterRe.Calculators
{
    class AltitudeCalculator
    {
        public static Tuple<string, double> CalcAltitude(double latitude, double longitude)
        {
            // TODO 実装
            // 探索コストを下げるために、Mesh ID と Altitude をいっぺんに返す

            var registeredMeshTable = Altitude10MMeshRegisteredDao.Get();


        }
    }
}
