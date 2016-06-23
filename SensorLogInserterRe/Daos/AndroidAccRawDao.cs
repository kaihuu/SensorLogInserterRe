using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class AndroidAccRawDao
    {
        private static readonly string TableName = "android_acc_raw";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnDateTime = "datetime";
        public static readonly string ColumnAccX = "acc_x";
        public static readonly string ColumnAccY = "acc_y";
        public static readonly string ColumnAccZ = "acc_z";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(DateTime startTime, DateTime endTime, int timeDiff, InsertDatum datum)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT");
            query.AppendLine($"   {ColumnDriverId},");
            query.AppendLine($"   {ColumnCarId},");
            query.AppendLine($"   {ColumnSensorId},");
            query.AppendLine($"   CONVERT(varchar,DATEADD(MILLISECOND, {timeDiff} ,{ColumnDateTime}),121) AS jst,");
            query.AppendLine($"   {ColumnAccX} AS {CorrectedAccDao.ColumnLongitudinalAcc},");
            query.AppendLine($"   {ColumnAccY} AS {CorrectedAccDao.ColumnLateralAcc},");
            query.AppendLine($"   {ColumnAccZ} AS {CorrectedAccDao.ColumnVerticalAcc}");
            query.AppendLine($"FROM {TableName}");
            query.AppendLine($"WHERE {ColumnDateTime} >= '{startTime}'");
            query.AppendLine($"   AND {ColumnDateTime} <= '{endTime}'");
            query.AppendLine($"   AND {ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"   AND {ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"ORDER BY {ColumnDateTime}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
