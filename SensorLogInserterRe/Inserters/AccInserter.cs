using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Handlers.FileHandlers;

namespace SensorLogInserterRe.Inserters
{
    class AccInserter
    {
        private static readonly int DriverIndex = 4;
        private static readonly int CarIndex = 5;
        private static readonly int SensorIndex = 6;

        public static void InsertAcc(List<string> insertFileList)
        {
            InsertAccRaw(insertFileList);
        }

        private static void InsertAccRaw(List<string> insertFileList)
        {
            foreach (var filePath in insertFileList)
            {
                string[] word = filePath.Split('\\');

                int driverId = DriverNames.GetDriverId(word[DriverIndex]);
                int carId = CarNames.GetCarId(word[CarIndex]);
                int sensorId = SensorNames.GetSensorId(word[SensorIndex]);

                var gpsRawTable = AccFileHandler.ConvertCsvToDataTable(filePath, driverId, carId, sensorId);
            }
        }

        private static void InsertCorrectedAcc()
        {

        }
    }
}
