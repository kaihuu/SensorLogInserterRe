using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Constant
{
    class DriverNames
    {
        public const string Tommy = "tommy";
        public const string Mori = "mori";
        public const string Tamura = "tam";
        public const string Uemura = "uemura";
        public const string Isobe = "isobe";

        public const string Simulation = "simu180215";
		public const string Arisimu = "Arisimu";
        // TODO 研究室メンバーの追加

        public static int GetDriverId(string driverName)
        {
            switch (driverName)
            {
                case DriverNames.Tommy:
                    return 1;
                case DriverNames.Mori:
                    return 4;
                case DriverNames.Tamura:
                    return 9;
                case DriverNames.Uemura:
                    return 17;
                case DriverNames.Isobe:
                    return 26;

                case DriverNames.Simulation:
                    return 98;

				case DriverNames.Arisimu:
					return 101;
				
                // TODO 研究室メンバーを追加
                default:
                    return -1;
            }
        }
    }
}
