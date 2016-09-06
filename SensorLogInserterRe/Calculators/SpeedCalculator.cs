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
        /** km/h の速度を返す **/
        public static double CalcSpeed(double latitudeBefore, double longitudeBefore, DateTime timeBefore, double latitudeAfter, double longitudeAfter, DateTime timeAfter,
            double latitudeFocused, double longitudeFocused)
        {
            //中間差分法を用いた導出
            //return DistanceCalculator.CalcDistance(latitudeBefore, longitudeBefore, latitudeAfter, longitudeAfter) / 2 / samplingSeconds * 3.6;

            return (DistanceCalculator.CalcDistance(latitudeBefore, longitudeBefore, latitudeFocused, longitudeFocused) + 
                DistanceCalculator.CalcDistance(latitudeFocused, longitudeFocused, latitudeAfter, longitudeAfter)) /
                   (timeAfter - timeBefore).TotalSeconds * 3.6;
        }
    }
}
