using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Inserters
{
    class GpsInserter
    {
        private static readonly int DriverIndex = 4;
        private static readonly int CarIndex = 5;
        private static readonly int SensorIndex = 6;

        public static void InsertGps(List<string> insertFileList)
        {
            foreach (var fileName in insertFileList)
            {
                string[] word = fileName.Split('\\');

                var gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();

                string driverName = word[DriverIndex];
                string carName = word[CarIndex];
                string sensorName = word[SensorIndex];
            }
        }

        private static void InsertGpsRaw()
        {
            
        }

        private static void InsertConrrectedGps()
        {
            
        }
    }
}
