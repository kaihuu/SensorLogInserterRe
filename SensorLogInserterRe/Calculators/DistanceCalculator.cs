using System;
using System.Device.Location;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators.CalculatorComponents;

namespace SensorLogInserterRe.Calculators
{
    static class DistanceCalculator
    {
        public static double CalcDistance(double latitudeFirst, double longitudeFirst, double latitudeSecond, double longitudeSecond)
        {
            //ヒュベニの公式で距離を計算
           return HubenyDistanceCalculator.CalcHubenyFormula(latitudeFirst, longitudeFirst, latitudeSecond, longitudeSecond);
        }
    }
}
