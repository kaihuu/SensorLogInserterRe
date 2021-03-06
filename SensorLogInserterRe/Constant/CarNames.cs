﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Constant
{
    class CarNames
    {
        public const string S2000 = "s2000";
        public const string Leaf = "LEAF";
        public const string Rav4 = "RAV4";
        public const string Prius = "PRIUS";
        public const string E350 = "E350";
        public const string Leaf000143 = "LEAF_000143";
        public const string YZFR15 = "YZF-R15";
        public const string Leaf_ZE1_20181005 = "LEAF_ZE1_20181005";
        public const string Leafnew = "LEAFnew";
        public const string CB400SF = "CB400SF";
        public const string LEAF2020 = "LEAF2020";

        public static int GetCarId(string carName)
        {
            switch (carName)
            {
                case CarNames.S2000:
                    return 1;
                case CarNames.Rav4:
                    return 2;
                case CarNames.Leaf:
                    return 3;
                case CarNames.Prius:
                    return 4;
                case CarNames.E350:
                    return 5;
                case CarNames.Leaf000143:
                    return 6;
                case CarNames.YZFR15:
                    return 9;
                case CarNames.Leaf_ZE1_20181005:
                    return 13;
                case CarNames.Leafnew:
                    return 14;
                case CarNames.CB400SF:
                    return 15;
                case CarNames.LEAF2020:
                    return 22;
                default:
                    return -1;
            }
        }
    }
}
