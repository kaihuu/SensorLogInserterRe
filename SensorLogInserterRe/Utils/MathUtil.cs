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

    }
}
