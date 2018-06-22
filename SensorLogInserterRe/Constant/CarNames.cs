using System;
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
        public const string GRASSTRACKER = "GrassTracker";

        public const string LeafS01 = "LEAFS01";
        public const string LeafS02 = "LEAFS02";
        public const string LeafS03 = "LEAFS03";
        public const string LeafS04 = "LEAFS04";
        public const string LeafS05 = "LEAFS05";
        public const string LeafS06 = "LEAFS06";
        public const string LeafS07 = "LEAFS07";
        public const string LeafS08 = "LEAFS08";
        public const string LeafS09 = "LEAFS09";
        public const string LeafS10 = "LEAFS10";
		public const string leafsimu = "LEAFSIMU";

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
                case CarNames.GRASSTRACKER:
                    return 10;

                case CarNames.LeafS01:
                    return 901;
                case CarNames.LeafS02:
                    return 902;
                case CarNames.LeafS03:
                    return 903;
                case CarNames.LeafS04:
                    return 904;
                case CarNames.LeafS05:
                    return 905;
                case CarNames.LeafS06:
                    return 906;
                case CarNames.LeafS07:
                    return 907;
                case CarNames.LeafS08:
                    return 908;
                case CarNames.LeafS09:
                    return 909;
                case CarNames.LeafS10:
                    return 910;
				case CarNames.leafsimu:
					return 911;

                default:
                    return -1;
            }
        }
    }
}