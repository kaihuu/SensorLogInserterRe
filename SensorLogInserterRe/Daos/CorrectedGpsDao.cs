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
        private static readonly string TableName = "corrected_gps_simulation";
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

        public static DataTable GetNormalized(DateTime startTime, DateTime endTime, InsertDatum datum)
        {
            var query = new StringBuilder();
            query.AppendLine($"WITH convert_gps");
            query.AppendLine($"AS (");
            query.AppendLine($"	SELECT {ColumnDriverId}");
            query.AppendLine($"		,{ColumnCarId}");
            query.AppendLine($"		,{ColumnSensorId}");
            query.AppendLine($"		,CONVERT(DATETIME, CONVERT(VARCHAR(30), CONVERT(DATETIME, (");
            query.AppendLine($"						CASE ");
            query.AppendLine($"							WHEN DATEPART(Ms, {ColumnJst}) >= 500");
            query.AppendLine($"								THEN DATEADD(SECOND, 1, {ColumnJst})");
            query.AppendLine($"							ELSE {ColumnJst}");
            query.AppendLine($"							END");
            query.AppendLine($"						)), 20)) AS {ColumnJst}");
            query.AppendLine($"		,{ColumnLatitude}");
            query.AppendLine($"		,{ColumnLongitude}");
            query.AppendLine($"		,{ColumnSpeed}");
            query.AppendLine($"		,{ColumnHeading}");
            query.AppendLine($"		,{ColumnDistanceDifference}");
            query.AppendLine($"	FROM {TableName}");
            query.AppendLine($"	WHERE {ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"		AND {ColumnCarId} = {datum.CarId}");
            query.AppendLine($"		AND {ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"     AND {ColumnJst} >= '{startTime}'");
            query.AppendLine($"     AND {ColumnJst} <= '{endTime}'");
            query.AppendLine($"	)");
            query.AppendLine($"SELECT {ColumnDriverId}");
            query.AppendLine($"	,{ColumnCarId}");
            query.AppendLine($"	,{ColumnSensorId}");
            query.AppendLine($"	,{ColumnJst}");
            query.AppendLine($"	,AVG({ColumnLatitude}) AS {ColumnLatitude}");
            query.AppendLine($"	,AVG({ColumnLongitude}) AS {ColumnLongitude}");
            query.AppendLine($"	,CAST(AVG({ColumnSpeed}) AS real) AS {ColumnSpeed}");
            query.AppendLine($"	,CAST(AVG({ColumnHeading}) AS real) AS {ColumnHeading}");
            query.AppendLine($"	,CAST(SUM({ColumnDistanceDifference}) AS real) AS {ColumnDistanceDifference}");
            query.AppendLine($"FROM convert_gps");
            query.AppendLine($"GROUP BY {ColumnDriverId}");
            query.AppendLine($"	,{ColumnCarId}");
            query.AppendLine($"	,{ColumnSensorId}");
            query.AppendLine($"	,{ColumnJst}");
            query.AppendLine($"ORDER BY {ColumnJst}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
