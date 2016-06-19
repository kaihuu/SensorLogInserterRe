using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators.CalculatorComponents;

namespace SensorLogInserterRe.Calculators
{
   　static class SpeedCalculator
    {

        public static double CalcSpeed(double latitudeBefore, double longitudeBefore, double latitudeAfter, double longitudeAfter, double samplingSeconds)
        {
            //中間差分法を用いた導出
            return DistanceCalculator.CalcDistance(latitudeBefore, longitudeBefore, latitudeAfter, longitudeAfter) / 2 / samplingSeconds;
        }
    }
}
