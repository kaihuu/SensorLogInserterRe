using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class AndroidGpsRawDao
    {
        private static readonly string TableName = "android_gps_raw_simulation";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnAndroidTime = "android_time";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnLatitude = "latitude";
        public static readonly string ColumnLongitude = "longitude";
        public static readonly string ColumnAltitude = "altitude";
        public static readonly string ColumnSpeed = "speed";
        public static readonly string ColumnBearing = "bearing";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static int GetMilliSencodTimeDiffBetweenJstAndAndroidTime(DateTime startTime, DateTime endTime, InsertDatum datum)
        {
            var query = new StringBuilder();
            query.AppendLine($"SELECT AVG(DATEDIFF(MILLISECOND, {ColumnAndroidTime}, {ColumnJst})) AS time_diff");
            query.AppendLine($"FROM {TableName}");
            query.AppendLine($"WHERE {ColumnJst} >= '{startTime}'");
            query.AppendLine($" AND {ColumnJst} <= '{endTime}'");
            query.AppendLine($" AND {ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($" AND {ColumnSensorId} = {datum.SensorId}");

            return DatabaseAccesser.GetResult(query.ToString()).Rows[0].Field<int?>("time_diff") ?? 0;
        }
    }
}
