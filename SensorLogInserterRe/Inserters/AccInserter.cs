using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Handlers.FileHandlers;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using SensorLogInserterRe.Inserters.Components;

namespace SensorLogInserterRe.Inserters
{
    class AccInserter
    {
        private static readonly int DriverIndex = 4;
        private static readonly int CarIndex = 5;
        private static readonly int SensorIndex = 6;

        public static void InsertAcc(List<string> insertFileList, InsertConfig config, List<InsertDatum> insertDatumList)
        {
            // foreach (var filePath in insertFileList)
            Parallel.For(0, insertFileList.Count, i =>
            {
                string[] word = insertFileList[i].Split('\\');

                // ACCファイルでない場合はcontinue
                if (System.Text.RegularExpressions.Regex.IsMatch(word[word.Length - 1], @"\d{14}Unsent16HzAccel.csv"))
                {

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

                    InsertAccRaw(insertFileList[i], datum);
                }
            });
        }

        private static void InsertAccRaw(string filePath, InsertDatum datum)
        {
            var accRawTable = AccFileHandler.ConvertCsvToDataTable(filePath, datum.DriverId, datum.CarId, datum.SensorId);
            accRawTable = SortTableByDateTime(accRawTable);
            if (accRawTable.Rows.Count != 0)
            {
                var normalizedAccTable = DataTableUtil.GetAndroidAccRawTable();

                #region インデックス 0 の場合

                var firstRow = normalizedAccTable.NewRow();

                firstRow.SetField(AndroidAccRawDao.ColumnDateTime, accRawTable.Rows[0].Field<DateTime>(AndroidAccRawDao.ColumnDateTime));
                firstRow.SetField(AndroidAccRawDao.ColumnDriverId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnDriverId));
                firstRow.SetField(AndroidAccRawDao.ColumnCarId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnCarId));
                firstRow.SetField(AndroidAccRawDao.ColumnSensorId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnSensorId));
                firstRow.SetField(AndroidAccRawDao.ColumnAccX, accRawTable.Rows[0].Field<Single>(AndroidAccRawDao.ColumnAccX));
                firstRow.SetField(AndroidAccRawDao.ColumnAccY, accRawTable.Rows[0].Field<Single>(AndroidAccRawDao.ColumnAccY));
                firstRow.SetField(AndroidAccRawDao.ColumnAccZ, accRawTable.Rows[0].Field<Single>(AndroidAccRawDao.ColumnAccZ));

                normalizedAccTable.Rows.Add(firstRow);

                #endregion

                for (int i = 1; i < accRawTable.Rows.Count; i++)
                {
                    //SQLSERVERのDATETIME型のミリ秒は0,3,7しか取らないため、5ミリ秒よりも短いデータが存在する場合は挿入時にエラーを出す可能性がある
                    if ((accRawTable.Rows[i].Field<DateTime>(AndroidAccRawDao.ColumnDateTime) -
                        accRawTable.Rows[i - 1].Field<DateTime>(AndroidAccRawDao.ColumnDateTime)).TotalMilliseconds >= 4)
                    {
                        var row = normalizedAccTable.NewRow();
                        row.SetField(AndroidAccRawDao.ColumnDateTime, accRawTable.Rows[i].Field<DateTime>(AndroidAccRawDao.ColumnDateTime));
                        row.SetField(AndroidAccRawDao.ColumnDriverId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnDriverId));
                        row.SetField(AndroidAccRawDao.ColumnCarId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnCarId));
                        row.SetField(AndroidAccRawDao.ColumnSensorId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnSensorId));
                        row.SetField(AndroidAccRawDao.ColumnAccX, accRawTable.Rows[i].Field<Single>(AndroidAccRawDao.ColumnAccX));
                        row.SetField(AndroidAccRawDao.ColumnAccY, accRawTable.Rows[i].Field<Single>(AndroidAccRawDao.ColumnAccY));
                        row.SetField(AndroidAccRawDao.ColumnAccZ, accRawTable.Rows[i].Field<Single>(AndroidAccRawDao.ColumnAccZ));

                        normalizedAccTable.Rows.Add(row);
                    }
                }

                // ファイルごとの処理なので主キー違反があっても挿入されないだけ
                AndroidAccRawDao.Insert(normalizedAccTable);
            }
        }

        private static DataTable SortTableByDateTime(DataTable table)
        {
            var view = new DataView(table) {Sort = AndroidAccRawDao.ColumnDateTime};

            return view.ToTable();
        }

        public static void InsertCorrectedAcc(InsertDatum datum, InsertConfig config)
        {
            Console.WriteLine("CALLED: InsertCorrectedAcc, " + datum);

            var tripsTable = new DataTable();
            //if (config.Correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
            //{
            //    tripsTable = TripsSpeedLPF005MMDao.Get(datum);
            //}
            //else
            //{
            //    tripsTable = TripsDao.Get(datum);
            //}

            foreach (DataRow row in tripsTable.Rows)
            {
                var correctedAccTable = AccCorrector.CorrectAcc(datum.StartTime, datum.EndTime, datum, row);

                // Tripsテーブルの1行ごろの処理なので主キー違反があっても挿入されないだけ
                // Trip単位で途中で挿入が異常終了した場合は、DELETEが必要
                CorrectedAccDao.Insert(correctedAccTable);
            }
        }
    }
}
