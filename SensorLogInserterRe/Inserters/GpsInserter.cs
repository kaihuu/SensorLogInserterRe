using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Handlers.FileHandlers;
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
            foreach (var filePath in insertFileList)
            {
                string[] word = filePath.Split('\\');

                int driverId = DriverNames.GetDriverId(word[DriverIndex]);
                int carId = CarNames.GetCarId(word[CarIndex]);
                int sensorId = SensorNames.GetSensorId(word[SensorIndex]);

                var gpsRawTable = GpsFileHandler.ConvertCsvToDataTable(filePath, driverId, carId, sensorId);
            }
        }

        private static void InsertConrrectedGps(DataTable gpsRawTable)
        {
            DataTable correctedGpsTable = DataTableUtil.GetCorrectedGpsTable();

            double latitudeNow;
            double longitudeNow;
            double latitudeBefore = 0;
            double longitudeBefore = 0;

            DateTime beforeJST = DateTime.Now;
            double beforeHeading = 0;
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            double startLatitude = 0;
            double startLongitude = 0;
            double endLatitude = 0;
            double endLongitude = 0;

            for (int i = 0; i < gpsRawTable.Rows.Count; i++)
            {
                DataRow dr = correctedGpsTable.NewRow();

                dr[CorrectedGpsDao.ColumnDriverId] = gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnDriverId];
                dr[CorrectedGpsDao.ColumnCarId] = gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnCarId];
                dr[CorrectedGpsDao.ColumnSensorId] = gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnSensorId];
                DateTime jstTime = DateTime.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnJst].ToString());
                dr[CorrectedGpsDao.ColumnJst] = jstTime.ToString(StringUtil.JstFormat);
                dr[CorrectedGpsDao.ColumnLatitude] = gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLatitude];
                dr[CorrectedGpsDao.ColumnLongitude] = gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLongitude];

                //トリップの最初の点(2点のデータから計算される値は0とする)
                if (i == 0)
                {
                    startTime = jstTime;
                    startLatitude = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLatitude].ToString());
                    startLongitude = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLongitude].ToString());

                    dr[CorrectedGpsDao.ColumnDistanceDifference] = 0;
                }
                else
                {
                    latitudeNow = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLatitude].ToString());
                    longitudeNow = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLatitude].ToString());
                    latitudeBefore = double.Parse(gpsRawTable.Rows[i - 1][AndroidGpsRawDao.ColumnLatitude].ToString());
                    longitudeBefore = double.Parse(gpsRawTable.Rows[i - 1][AndroidGpsRawDao.ColumnLongitude].ToString());

                    double distance = DistanceCalculator.CalcDistance(latitudeNow, longitudeNow, latitudeBefore, longitudeBefore);
                    dr["DISTANCE_DIFFERENCE"] = distance;
                }
            }
        }
    }
}
