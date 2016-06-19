using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class CorrectedGpsDao
    {
        private static readonly string TableName = "corrected_gps";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnLatitude = "latitude";
        public static readonly string ColumnLongitude = "longitude";
        public static readonly string ColumnSpeed = "speed";
        public static readonly string ColumnHeading = "heading";
        public static readonly string ColumnDistanceDifference = "distance_difference";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(DateTime startTime, DateTime endTime, InsertDatum datum)
        {
            var query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"  FROM {TableName}");
            query.AppendLine($" WHERE {ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"   AND {ColumnCarId} = {datum.CarId}");
            query.AppendLine($"   AND {ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"   AND {ColumnJst} >= '{startTime}'");
            query.AppendLine($"   AND {ColumnJst} <= '{endTime}'");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
