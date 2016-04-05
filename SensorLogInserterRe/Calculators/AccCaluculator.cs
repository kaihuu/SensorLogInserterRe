using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using SensorLogInserterRe.Calculators.CalculatorComponents;

namespace SensorLogInserterRe.Calculators
{
    static class AccCaluculator
    {
        public static double CalcAcc(GeoCoordinate geoBefore, GeoCoordinate geoThis, GeoCoordinate geoAfter, double samplingTime)
        {
            return (HubenyDistanceCalculator.CalcHubenyFormula(geoThis, geoAfter) - HubenyDistanceCalculator.CalcHubenyFormula(geoBefore, geoThis)) / Math.Pow(samplingTime, 2); //中間差分法による導出
        }
    }
}
