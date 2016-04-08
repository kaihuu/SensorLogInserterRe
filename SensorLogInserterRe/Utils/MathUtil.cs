using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    class MathUtil
    {
        public static double ConvertDegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static double ConvertSpeedToRpm(Car car, double speed)
        {
            // 参照URL
            // http://detail.chiebukuro.yahoo.co.jp/qa/question_detail/q1249261627
            return speed * 3.6 * 60 / (car.TireRadius * 2 * Math.PI);
        }
    }
}
