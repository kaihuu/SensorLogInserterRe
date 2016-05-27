﻿using System;
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

            double latitudeNow;
            double longitudeNow;
            double latitudeBefore = 0;
            double longitudeBefore = 0;

            DateTime beforeJst = DateTime.Now;
            double beforeHeading = 0;

            #region CorrectedGps テーブルの生成

            #region インデックスが 0 の場合
            DataRow firstRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(firstRow, gpsRawTable.Rows[0]);
            firstRow[CorrectedGpsDao.ColumnDistanceDifference] = 0;
            firstRow[CorrectedGpsDao.ColumnSpeed] = 0;
            firstRow[CorrectedGpsDao.ColumnHeading] = 0;
            beforeJst = DateTime.Parse(firstRow[CorrectedGpsDao.ColumnJst].ToString());
            #endregion

            for (int i = 1; i < gpsRawTable.Rows.Count - 1; i++)
            {
                DataRow row = correctedGpsTable.NewRow();

                CopyRawDataToCorrectedRow(row, gpsRawTable.Rows[i]);
                DateTime jstTime = DateTime.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnJst].ToString());

                latitudeNow = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLatitude].ToString());
                longitudeNow = double.Parse(gpsRawTable.Rows[i][AndroidGpsRawDao.ColumnLongitude].ToString());
                latitudeBefore = double.Parse(gpsRawTable.Rows[i - 1][AndroidGpsRawDao.ColumnLatitude].ToString());
                longitudeBefore = double.Parse(gpsRawTable.Rows[i - 1][AndroidGpsRawDao.ColumnLongitude].ToString());

                // 距離の算出
                row[CorrectedGpsDao.ColumnDistanceDifference] = DistanceCalculator.CalcDistance(
                    new GeoCoordinate(latitudeBefore, longitudeBefore), new GeoCoordinate(latitudeNow, longitudeNow));

                // 速度の算出
                row[CorrectedGpsDao.ColumnSpeed] = SpeedCalculator.CalcSpeed(
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude), );

                //1つ前のデータとの時間差を取得(second)
                TimeSpan span;
                span = jstTime - beforeJst;
                double spanTime = span.TotalSeconds;

                //速度が1km以上になったらHEADINGを更新する(停止時に1つ1つ計算するとHEADINDが暴れるため)
                if (distance * 3.6 / spanTime >= 1)
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
            correctedRow[CorrectedGpsDao.ColumnDriverId] = rawRow[AndroidGpsRawDao.ColumnDriverId];
            correctedRow[CorrectedGpsDao.ColumnCarId] = rawRow[AndroidGpsRawDao.ColumnCarId];
            correctedRow[CorrectedGpsDao.ColumnSensorId] = rawRow[AndroidGpsRawDao.ColumnSensorId];
            DateTime jstTime = DateTime.Parse(rawRow[AndroidGpsRawDao.ColumnJst].ToString());
            correctedRow[CorrectedGpsDao.ColumnJst] = jstTime.ToString(StringUtil.JstFormat);
            correctedRow[CorrectedGpsDao.ColumnLatitude] = rawRow[AndroidGpsRawDao.ColumnLatitude];
            correctedRow[CorrectedGpsDao.ColumnLongitude] = rawRow[AndroidGpsRawDao.ColumnLongitude];
        }
    }
}
