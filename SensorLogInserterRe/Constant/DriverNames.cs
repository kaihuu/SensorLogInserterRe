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
                // TODO 研究室メンバーを追加
                default:
                    return -1;
            }
        }
    }
}
