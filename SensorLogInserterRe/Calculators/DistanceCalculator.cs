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
        public static float CalcDistance(double latitudeFirst, double longitudeFirst, double latitudeSecond, double longitudeSecond)
        {
            //ヒュベニの公式で距離を計算
            double result = HubenyDistanceCalculator.CalcHubenyFormula(geoFirst, geoSecond);

            return (float)result;
        }
    }
}
