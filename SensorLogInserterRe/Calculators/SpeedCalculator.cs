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

        public static GeoCoordinate CalcSpeed(GeoCoordinate geoBefore, GeoCoordinate geoThis, GeoCoordinate geoAfter, double samplingTime)
        {
            geoThis.Speed = HubenyDistanceCalculator.CalcHubenyFormula(geoBefore, geoAfter) / 2 / samplingTime; //中間差分法を用いた導出
            return geoThis;
        }
    }
}
