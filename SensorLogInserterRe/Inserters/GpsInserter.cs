using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
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

            DateTime beforeJst = DateTime.Now;
            double beforeHeading = 0;

            #region CorrectedGps テーブルの生成

            #region インデックスが 0 の場合
            DataRow firstRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(firstRow, gpsRawTable.Rows[0]);
            firstRow.SetField(CorrectedGpsDao.ColumnDistanceDifference, 0);
            firstRow.SetField(CorrectedGpsDao.ColumnSpeed, 0);
            firstRow.SetField(CorrectedGpsDao.ColumnHeading, 0);
            #endregion

            for (int i = 1; i < gpsRawTable.Rows.Count - 1; i++)
            {
                DataRow row = correctedGpsTable.NewRow();

                CopyRawDataToCorrectedRow(row, gpsRawTable.Rows[i]);

                latitudeNow = gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLatitude);
                longitudeNow = gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLongitude);
                latitudeBefore = gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude);
                longitudeBefore = gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude);

                // 距離の算出
                row[CorrectedGpsDao.ColumnDistanceDifference] = DistanceCalculator.CalcDistance(
                    new GeoCoordinate(latitudeBefore, longitudeBefore), new GeoCoordinate(latitudeNow, longitudeNow));

                // 速度の算出
                row[CorrectedGpsDao.ColumnSpeed] = SpeedCalculator.CalcSpeed(
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    (gpsRawTable.Rows[i + 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst) - gpsRawTable.Rows[i - 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst)).TotalSeconds);

                //速度が1km以上になったらHEADINGを更新する(停止時に1つ1つ計算するとHEADINDが暴れるため)
                if ()
                {
                    double heading = HeadingCalculator.CalcHeading(latitudeBefore, longitudeBefore, latitudeNow,
                        longitudeNow);

                    row[CorrectedGpsDao.ColumnHeading] = heading;
                    beforeHeading = heading;
                }
                else
                {
                    row[CorrectedGpsDao.ColumnHeading] = beforeHeading;
                }

            }
            #endregion

            #region Trips テーブルの挿入

            var tripsTable = DataTableUtil.GetTripsTable();
            DataRow tripsRow = tripsTable.NewRow();

            tripsRow[TripsDao.ColumnDriverId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnDriverId].ToString());
            tripsRow[TripsDao.ColumnCarId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnCarId].ToString());
            tripsRow[TripsDao.ColumnSensorId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnSensorId].ToString());
            tripsRow[TripsDao.ColumnStartTime] = gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnJst].ToString();
            tripsRow[TripsDao.ColumnStartLatitude] = double.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnLatitude].ToString());
            tripsRow[TripsDao.ColumnStartLongitude] = double.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnLongitude].ToString());
            tripsRow[TripsDao.ColumnEndTime] = gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnJst].ToString();
            tripsRow[TripsDao.ColumnEndLatitude] = double.Parse(gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnLatitude].ToString());
            tripsRow[TripsDao.ColumnEndLongitude] = double.Parse(gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnLongitude].ToString());

            TripsRawDao.Insert(tripsTable);

            #endregion
        }

        private static void CopyRawDataToCorrectedRow(DataRow correctedRow, DataRow rawRow)
        {
            correctedRow.SetField(CorrectedGpsDao.ColumnDriverId, rawRow.Field<int>(AndroidGpsRawDao.ColumnDriverId));
            correctedRow.SetField(CorrectedGpsDao.ColumnCarId, rawRow.Field<int>(AndroidGpsRawDao.ColumnCarId));
            correctedRow.SetField(CorrectedGpsDao.ColumnSensorId, rawRow.Field<int>(AndroidGpsRawDao.ColumnSensorId));
            correctedRow.SetField(CorrectedGpsDao.ColumnJst, rawRow.Field<DateTime>(AndroidGpsRawDao.ColumnJst));
            correctedRow.SetField(CorrectedGpsDao.ColumnLatitude, rawRow.Field<double>(AndroidGpsRawDao.ColumnLatitude));
            correctedRow.SetField(CorrectedGpsDao.ColumnLongitude, rawRow.Field<double>(AndroidGpsRawDao.ColumnLongitude));
        }
    }
}
