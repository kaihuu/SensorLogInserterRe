using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
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
            InsertGpsRaw(insertFileList);
        }

        private static void InsertGpsRaw(List<string> insertFileList)
        {
            foreach (var fileName in insertFileList)
            {
                string[] word = fileName.Split('\\');

                var gpsRawTable = DataTableUtil.GetAndroidGpsRawTable();

                int driverId = DriverNames.GetDriverId(word[DriverIndex]);
                int carId = CarNames.GetCarId(word[CarIndex]);
                int sensorId = SensorNames.GetSensorId(word[SensorIndex]);
            }
        }

        private static void InsertConrrectedGps()
        {
            
        }
    }
}
