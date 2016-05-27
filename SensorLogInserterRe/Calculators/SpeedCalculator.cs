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
            double speed = HubenyDistanceCalculator.CalcHubenyFormula(new GeoCoordinate(latitudeBefore, longitudeBefore), new GeoCoordinate(latitudeAfter, longitudeAfter)) / 2 / samplingSeconds; //中間差分法を用いた導出
            return speed;
        }
    }
}
