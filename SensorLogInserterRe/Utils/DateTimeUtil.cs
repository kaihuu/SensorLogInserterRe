using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    static class DateTimeUtil
    {
        public static long ConvertDateTimeToLongFormatted(DateTime dateTime)
        {
            return dateTime.Year * 10000000000 + dateTime.Month * 100000000 + dateTime.Day * 1000000;
        }

    }
}
