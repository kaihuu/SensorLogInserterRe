using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Constant
{
    class SensorNames
    {
        public const string N06C = "N-06C";
        public const string At3S0_1 = "AT3S0-1";
        public const string At3S0_1_40 = "AT3S0-1_4.0";
        public const string Sc01C = "SC-01C";
        public const string Mz604_1_32 = "MZ604-1_3.2";
        public const string Mz604_1_40 = "MZ604-1_4.0";
        public const string Mz604_2_40 = "MZ604-2_4.0";
        public const string A1_07 = "A1_07";
        public const string SO_04D = "SO-04D";
        public const string At570_2 = "AT570-2";
        public const string At570_3 = "AT570-3";
        public const string At570_4 = "AT570-4";
        public const string At570_5 = "AT570-5";
        public const string Nexus7_2012_1 = "Nexus7(2012)-1";
        public const string Nexus7_2013_3 = "Nexus7(2013)-3";
        public const string Nexus7_2013_4 = "Nexus7(2013)-4";
        public const string Nexus7_2013_4_Small = "nexus7(2013)-4";
        public const string Nexus7_2013_5 = "nexus7(2013)-5LTE";
        public const string XperiaGX_SO_04D = "XperiaGX_SO-04D";
        public const string SO_02_F = "SO-02F";
        public const string Nexus6 = "nexus6";
        public const string Nexus7_2013_2 = "Nexus7(2013)-2";
        public const string Zenfone2_1 = "ZenFone2-1";
        public const string SKT01 = "SKT-01";

        public const string Simulation = "Simulation";
		public const string arisimu = "arisimu";

        public static int GetSensorId(string sensorName)
        {
            switch (sensorName)
            {
                case N06C:
                    return 3;
                case At3S0_1:
                    return 6;
                case At3S0_1_40:
                    return 6;
                case Sc01C:
                    return 7;
                case At570_2:
                    return 8;
                case Mz604_1_32:
                    return 9;
                case Mz604_1_40:
                    return 9;
                case Mz604_2_40:
                    return 10;
                case A1_07:
                    return 11;
                case At570_3:
                    return 12;
                case At570_4:
                    return 13;
                case SO_04D:
                    return 15;
                case At570_5:
                    return 16;
                case Nexus7_2013_3:
                    return 17;
                case Nexus7_2013_4:
                    return 18;
                case Nexus7_2013_4_Small:
                    return 18;
                case Nexus7_2013_5:
                    return 20;
                case XperiaGX_SO_04D:
                    return 21;
                case SO_02_F:
                    return 22;
                case Nexus6:
                    return 23;
                case Nexus7_2012_1:
                    return 24;
                case Nexus7_2013_2:
                    return 25;
                case Zenfone2_1:
                    return 26;
                case SKT01:
                    return 27;

                case Simulation:
                    return 98;
				case arisimu:
					return 101;
                default: 
                    return -1;
            }
        }
    }
}
