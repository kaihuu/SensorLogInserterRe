using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Handlers.FileHandlers;
using SensorLogInserterRe.Utils;

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
            }
        }

        private static DataTable InsertAccRaw(string filePath, int driverId, int carId, int sensorId)
        {
            var accRawTable = AccFileHandler.ConvertCsvToDataTable(filePath, driverId, carId, sensorId);
            accRawTable = SortTableByDateTime(accRawTable);

            var normalizedAccTable = DataTableUtil.GetAndroidAccRawTable();

            #region インデックス 0 の場合

            var firstRow = normalizedAccTable.NewRow();

            firstRow.SetField(AndroidAccRawDao.ColumnDateTime, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnDateTime));
            firstRow.SetField(AndroidAccRawDao.ColumnDriverId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnDriverId));
            firstRow.SetField(AndroidAccRawDao.ColumnCarId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnCarId));
            firstRow.SetField(AndroidAccRawDao.ColumnSensorId, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnSensorId));
            firstRow.SetField(AndroidAccRawDao.ColumnAccX, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnAccX));
            firstRow.SetField(AndroidAccRawDao.ColumnAccY, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnAccY));
            firstRow.SetField(AndroidAccRawDao.ColumnAccZ, accRawTable.Rows[0].Field<int>(AndroidAccRawDao.ColumnAccZ));

            normalizedAccTable.Rows.Add(firstRow);

            #endregion

            int removedCount = 0;

            for (int i = 1; i < accRawTable.Rows.Count; i++)
            {
                //SQLSERVERのDATETIME型のミリ秒は0,3,7しか取らないため、5ミリ秒よりも短いデータが存在する場合は挿入時にエラーを出す可能性がある
                if ( (accRawTable.Rows[i].Field<DateTime>(AndroidAccRawDao.ColumnDateTime) - 
                    accRawTable.Rows[i - 1].Field<DateTime>(AndroidAccRawDao.ColumnDateTime)).TotalMilliseconds >= 4)
                {
                    var row = normalizedAccTable.NewRow();
                    row.SetField(AndroidAccRawDao.ColumnDateTime, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnDateTime));
                    row.SetField(AndroidAccRawDao.ColumnDriverId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnDriverId));
                    row.SetField(AndroidAccRawDao.ColumnCarId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnCarId));
                    row.SetField(AndroidAccRawDao.ColumnSensorId, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnSensorId));
                    row.SetField(AndroidAccRawDao.ColumnDateTime, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnDateTime));
                    row.SetField(AndroidAccRawDao.ColumnAccX, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnAccX));
                    row.SetField(AndroidAccRawDao.ColumnAccY, accRawTable.Rows[i].Field<int>(AndroidAccRawDao.ColumnAccY));

                    normalizedAccTable.Rows.Add(row);
                }
                else
                {
                    removedCount++;
                }
            }

            // TODO 除去したデータ件数をログに出力
            AndroidAccRawDao.Insert(normalizedAccTable);

            return normalizedAccTable;
        }

        private static void InsertCorrectedAcc(DataTable accRawTable)
        {
            accRawTable = SortTableByDateTime(accRawTable);

            var correctedAccTable = DataTableUtil.GetCorrectedAccTable();

            

        }

        private static DataTable SortTableByDateTime(DataTable table)
        {
            var view = new DataView(table);

            view.Sort = CorrectedAccDao.ColumnDateTime;

            return view.ToTable();
        }
    }
}
