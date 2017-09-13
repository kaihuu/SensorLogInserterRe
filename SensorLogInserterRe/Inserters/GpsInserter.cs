using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Handlers.FileHandlers;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using SensorLogInserterRe.ViewModels;
using SensorLogInserterRe.Cleansers.Components;

namespace SensorLogInserterRe.Inserters
{
    class GpsInserter
    {
        private static readonly int DriverIndex = 4;
        private static readonly int CarIndex = 5;
        private static readonly int SensorIndex = 6;

        public static void InsertGps(List<string> insertFileList, InsertConfig config, int correctionIndex, List<InsertDatum> insertDatumList)
        {
            foreach (var filePath in insertFileList)
            {
                Console.WriteLine("GPSinserting:" + filePath);
                string[] word = filePath.Split('\\');

                // GPSファイルでない場合はcontinue
                if (! System.Text.RegularExpressions.Regex.IsMatch(word[word.Length - 1], @"\d{14}UnsentGPS.csv"))
                    continue;

                var datum = new InsertDatum()
                {
                    DriverId = DriverNames.GetDriverId(word[DriverIndex]),
                    CarId = CarNames.GetCarId(word[CarIndex]),
                    SensorId = SensorNames.GetSensorId(word[SensorIndex]),
                    StartTime = config.StartDate,
                    EndTime = config.EndDate,
                    EstimatedCarModel = EstimatedCarModel.GetModel(config.CarModel)
                };

                InsertDatum.AddDatumToList(insertDatumList, datum);

                LogWritter.WriteLog(LogWritter.LogMode.Gps, $"インサートデータ, FilePath: {filePath}, DriverId: {datum.DriverId}, CarId: {datum.CarId}, SensorId: {datum.SensorId}");

                // ファイルごとの処理なので主キー違反があっても挿入されないだけ
                var gpsRawTable = InsertGpsRaw(filePath, datum);
                if (config.Correction[correctionIndex] == InsertConfig.GpsCorrection.SpeedLPFMapMatching || config.Correction[correctionIndex] == InsertConfig.GpsCorrection.MapMatching)
                {
                    gpsRawTable = MapMatching.getResultMapMatching(gpsRawTable, datum);
                }
                if (gpsRawTable.Rows.Count != 0)
                {

                    InsertCorrectedGps(gpsRawTable, config.Correction[correctionIndex]);
                    TripInserter.InsertTripRaw(gpsRawTable, config.Correction[correctionIndex]);
                    
                }
                else
                {
                    LogWritter.WriteLog(LogWritter.LogMode.Gps, $"ファイルの行数が0行のためインサートを行いませんでした: {filePath}");
                }
            }
        }

        private static DataTable InsertGpsRaw(string filePath, InsertDatum datum)
        {
            var gpsRawTable = GpsFileHandler.ConvertCsvToDataTable(filePath, datum);

            if(gpsRawTable.Rows.Count != 0)
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
            correctedRow.SetField(CorrectedGpsDao.ColumnAltitude, rawRow.Field<double>(AndroidGpsRawDao.ColumnAltitude));
            correctedRow.SetField(CorrectedGpsDao.ColumnSpeed, rawRow.Field<double?>(AndroidGpsRawDao.ColumnSpeed));
            correctedRow.SetField(CorrectedGpsDao.ColumnBearing, rawRow.Field<double?>(AndroidGpsRawDao.ColumnBearing));
            correctedRow.SetField(CorrectedGpsDao.ColumnAccuracy, rawRow.Field<>(AndroidGpsRawDao.ColumnAccuracy));

        }

        private static void InsertCorrectedGps(DataTable gpsRawTable, InsertConfig.GpsCorrection correction)
        {
            DataTable correctedGpsTable = DataTableUtil.GetCorrectedGpsTable();

            #region インデックスが 0 の場合
            DataRow firstRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(firstRow, gpsRawTable.Rows[0]);
            firstRow.SetField(CorrectedGpsDao.ColumnDistanceDifference, 0);
            //firstRow.SetField(CorrectedGpsDao.ColumnSpeed, 0);
            firstRow.SetField(CorrectedGpsDao.ColumnHeading, 0);
            var meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
            gpsRawTable.Rows[0].Field<double>(CorrectedGpsDao.ColumnLatitude),
            gpsRawTable.Rows[0].Field<double>(CorrectedGpsDao.ColumnLongitude));

            firstRow.SetField(CorrectedGpsDao.ColumnTerrainAltitude, meshAndAltitude.Item2);
            


            var linkAndTheta = LinkMatcher.GetInstance().MatchLink(
            firstRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
            firstRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
            0f);

            firstRow.SetField(CorrectedGpsDao.ColumnLinkId, linkAndTheta.Item1);
            firstRow.SetField(CorrectedGpsDao.ColumnRoadTheta, linkAndTheta.Item2);

            correctedGpsTable.Rows.Add(firstRow);
            #endregion

            for (int i = 1; i < gpsRawTable.Rows.Count - 1; i++)
            {
                DataRow row = correctedGpsTable.NewRow();

                CopyRawDataToCorrectedRow(row, gpsRawTable.Rows[i]);

                // 距離の算出
                row.SetField(CorrectedGpsDao.ColumnDistanceDifference, DistanceCalculator.CalcDistance(
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLongitude)));

                meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
                gpsRawTable.Rows[i].Field<double>(CorrectedGpsDao.ColumnLatitude),
                gpsRawTable.Rows[i].Field<double>(CorrectedGpsDao.ColumnLongitude));

                row.SetField(CorrectedGpsDao.ColumnTerrainAltitude, meshAndAltitude.Item2);





                // 速度の算出
                //row.SetField(CorrectedGpsDao.ColumnSpeed, SpeedCalculator.CalcSpeed(
                //    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                //    gpsRawTable.Rows[i - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                //    gpsRawTable.Rows[i - 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst),
                //    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                //    gpsRawTable.Rows[i + 1].Field<double>(AndroidGpsRawDao.ColumnLongitude),
                //    gpsRawTable.Rows[i + 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst),
                //    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLatitude),
                //    gpsRawTable.Rows[i].Field<double>(AndroidGpsRawDao.ColumnLongitude)));

                //速度が1km以上になったらHEADINGを更新する(停止時に1つ1つ計算するとHEADINDが暴れるため)
                if (row.Field<Single?>(CorrectedGpsDao.ColumnSpeed) > 1.0)
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

                linkAndTheta = LinkMatcher.GetInstance().MatchLink(
                row.Field<double>(CorrectedGpsDao.ColumnLatitude),
                row.Field<double>(CorrectedGpsDao.ColumnLongitude),
                Convert.ToSingle(row.Field<double>(CorrectedGpsDao.ColumnHeading))
                );

                row.SetField(CorrectedGpsDao.ColumnLinkId, linkAndTheta.Item1);
                row.SetField(CorrectedGpsDao.ColumnRoadTheta, linkAndTheta.Item2);
                correctedGpsTable.Rows.Add(row);
            }

            #region インデックスが最後の場合
            DataRow lastRow = correctedGpsTable.NewRow();
            CopyRawDataToCorrectedRow(lastRow, gpsRawTable.Rows[gpsRawTable.Rows.Count - 1]);
            lastRow.SetField(CorrectedGpsDao.ColumnDistanceDifference, 0);
            lastRow.SetField(CorrectedGpsDao.ColumnSpeed, 0);
            lastRow.SetField(CorrectedGpsDao.ColumnHeading, 0);

            meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
            gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<double>(CorrectedGpsDao.ColumnLatitude),
            gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<double>(CorrectedGpsDao.ColumnLongitude));

            lastRow.SetField(CorrectedGpsDao.ColumnTerrainAltitude, meshAndAltitude.Item2);



            linkAndTheta = LinkMatcher.GetInstance().MatchLink(
            firstRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
            firstRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
            0f);

            lastRow.SetField(CorrectedGpsDao.ColumnLinkId, linkAndTheta.Item1);
            lastRow.SetField(CorrectedGpsDao.ColumnRoadTheta, linkAndTheta.Item2);

            correctedGpsTable.Rows.Add(lastRow);

            #endregion

            // ファイルごとの挿入なので主キー違反があっても挿入されないだけ
            if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)//速度にローパスフィルタを適用
            {
                
                DataTable correctedGpsSpeedLPFTable = LowPassFilter.speedLowPassFilter(correctedGpsTable, 0.05);
                CorrectedGpsSpeedLPF005MMDao.Insert(correctedGpsSpeedLPFTable);
            }
            else if (correction == InsertConfig.GpsCorrection.MapMatching)
            {
                CorrectedGPSMMDao.Insert(correctedGpsTable);
            }
            else
            {
                CorrectedGpsDao.Insert(correctedGpsTable);
            }
        }
    }
}
