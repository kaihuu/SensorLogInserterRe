using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Constant
{
    class Coordinate
    {
        public class Ynu
        {
            public static readonly double LatitudeStart = 35.47;
            public static readonly double LatitudeEnd = 35.476;
            public static readonly double LongitudeStart = 139.58;
            public static readonly double LongitudeEnd = 139.60;
        }

        public class TommyHome
        {
            public static readonly double LatitudeStart = 35.43;
            public static readonly double LatitudeEnd = 35.435;
            public static readonly double LongitudeStart = 139.40;
            public static readonly double LongitudeEnd = 139.42;
        }

        public class MoriHome
        {
            public static readonly double LatitudeStart = 35.527;
            public static readonly double LatitudeEnd = 35.538;
            public static readonly double LongitudeStart = 139.428;
            public static readonly double LongitudeEnd = 139.443;
        }

        public class TamuraHomeBefore
        {
            public static readonly double LatitudeStart = 35.58;
            public static readonly double LatitudeEnd = 35.59;
            public static readonly double LongitudeStart = 139.65;
            public static readonly double LongitudeEnd = 139.668;
            public static readonly DateTime EndDate = DateTime.Parse("2013-09-10");
        }

        public class TamuraHomeAfter
        {
            public static readonly double LatitudeStart = 35.342;
            public static readonly double LatitudeEnd = 35.35;
            public static readonly double LongitudeStart = 139.51;
            public static readonly double LongitudeEnd = 139.525;
            public static readonly DateTime StartDate = DateTime.Parse("2013-09-10");
        }

        public class AyaseCityHall
        {
            public static readonly double LatitudeStart = 35.43;
            public static readonly double LatitudeEnd = 35.445;
            public static readonly double LongitudeStart = 139.42;
            public static readonly double LongitudeEnd = 139.435;
            //public static readonly DateTime StartDate = DateTime.Parse("2013-11-22");
            //public static readonly DateTime EndDate = DateTime.Parse("2013-11-23");
            //public static readonly int SensorId = 14;
        }

    }
}
