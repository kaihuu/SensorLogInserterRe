using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
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
            foreach (var filePath in insertFileList)
            {
                string[] word = filePath.Split('\\');

                int driverId = DriverNames.GetDriverId(word[DriverIndex]);
                int carId = CarNames.GetCarId(word[CarIndex]);
                int sensorId = SensorNames.GetSensorId(word[SensorIndex]);

                var accRawTable = InsertAccRaw(filePath, driverId, carId, sensorId);
                InsertCorrectedAcc(accRawTable);
                InsertTrip(gpsRawTable);
            }
        }

        private static DataTable InsertAccRaw(string filePath, int driverId, int carId, int sensorId)
        {
            var accRawTable = AccFileHandler.ConvertCsvToDataTable(filePath, driverId, carId, sensorId);
            AndroidAccRawDao.Insert(accRawTable);

            return accRawTable;
        }

        private static void InsertCorrectedAcc(DataTable rawTable)
        {

        }
    }
}
