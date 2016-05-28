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
using SensorLogInserterRe.Models;
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
            foreach (var filePath in insertFileList)
            {
                string[] word = filePath.Split('\\');

                var gpsRawTable = InsertGpsRaw(filePath, new UserDatum()
                {
                    DriverId = DriverNames.GetDriverId(word[DriverIndex]),
                    CarId = CarNames.GetCarId(word[CarIndex]),
                    SensorId = SensorNames.GetSensorId(word[SensorIndex])
            });
                InsertConrrectedGps(gpsRawTable);
                TripInserter.InsertTripRaw(gpsRawTable);
            }
        }

        private static DataTable InsertGpsRaw(string filePath, UserDatum datum)
        {
            var gpsRawTable = GpsFileHandler.ConvertCsvToDataTable(filePath, datum);
            AndroidGpsRawDao.Insert(gpsRawTable);

            return gpsRawTable;
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

        private static void InsertConrrectedGps(DataTable gpsRawTable)
        {
            DataTable correctedGpsTable = DataTableUtil.GetCorrectedGpsTable();

            #region インデックスが 0 の場合
            DataRow firstRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(firstRow, gpsRawTable.Rows[0]);
            firstRow.SetField(CorrectedGpsDao.ColumnDistanceDifference, 0);
            firstRow.SetField(CorrectedGpsDao.ColumnSpeed, 0);
            firstRow.SetField(CorrectedGpsDao.ColumnHeading, 0);

            correctedGpsTable.Rows.Add(firstRow);
            #endregion

            for (int i = 1; i < gpsRawTable.Rows.Count - 1; i++)
            {
                DataRow row = correctedGpsTable.NewRow();

                CopyRawDataToCorrectedRow(row, gpsRawTable.Rows[i]);

                // 距離の算出
                row.SetField<double>(CorrectedGpsDao.ColumnDistanceDifference, DistanceCalculator.CalcDistance(
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLongitude)));

                // 速度の算出
                row.SetField(CorrectedGpsDao.ColumnSpeed, SpeedCalculator.CalcSpeed(
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    (gpsRawTable.Rows[i + 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst) -
                     gpsRawTable.Rows[i - 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst)).TotalSeconds));

                //速度が1km以上になったらHEADINGを更新する(停止時に1つ1つ計算するとHEADINDが暴れるため)
                if (row.Field<double>(CorrectedGpsDao.ColumnSpeed) > 1.0)
                {
                    row.SetField(CorrectedGpsDao.ColumnHeading, HeadingCalculator.CalcHeading(
                        gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                        gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                        gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                        gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLongitude)));
                }
                else
                {
                    row.SetField(CorrectedGpsDao.ColumnHeading, correctedGpsTable.Rows[i - 1].Field<double>(CorrectedGpsDao.ColumnHeading));
                }

                correctedGpsTable.Rows.Add(row);
            }

            #region インデックスが最後の場合
            DataRow lastRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(lastRow, gpsRawTable.Rows[gpsRawTable.Rows.Count - 1]);
            lastRow.SetField(CorrectedGpsDao.ColumnDistanceDifference, 0);
            lastRow.SetField(CorrectedGpsDao.ColumnSpeed, 0);
            lastRow.SetField(CorrectedGpsDao.ColumnHeading, 0);

            correctedGpsTable.Rows.Add(lastRow);

            #endregion
        }
    }
}
