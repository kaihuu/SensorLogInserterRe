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
        public const string Ibaraki = "ibaraki";
        public const string Hamazaki = "hamazaki";
        public const string Koike = "koike";
        public const string Isobe = "isobe";
        public const string Arinaga = "arinaga";
        public const string Kichise = "kichise";
        public const string Itani = "itani";
        public const string Katsumura = "katsumura";
        public const string Watanabe = "watanabe";
        public const string Ishida = "ishida";
        public const string Fukano = "fukano";
        public const string Iida = "iida";
        public const string Murakami = "murakami";
        public const string Ohashi = "ohashi";
        public const string Yoshida = "yoshida";
        // TODO 研究室メンバーの追加

        // TODO ECOLOGDB2016.DRIVERSと内容が重複するので一本化する

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

                case DriverNames.Ibaraki:
                    return 20;
                case DriverNames.Hamazaki:
                    return 21;
                case DriverNames.Koike:
                    return 24;
                case DriverNames.Isobe:
                    return 26;
                case DriverNames.Arinaga:
                    return 27;
                case DriverNames.Kichise:
                    return 28;
                case DriverNames.Itani:
                    return 29;
                case DriverNames.Katsumura:
                    return 30;
                case DriverNames.Watanabe:
                    return 31;
                case DriverNames.Ishida:
                    return 32;
                case DriverNames.Fukano:
                    return 33;
                case DriverNames.Iida:
                    return 34;
                case DriverNames.Murakami:
                    return 35;
                case DriverNames.Ohashi:
                    return 36;
                case DriverNames.Yoshida:
                    return 37;
                default:
                    return -1;
            }
        }
    }
}
