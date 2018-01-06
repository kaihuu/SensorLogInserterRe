using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class CorrectedGPSDopplerDao
    {
        private static readonly string TableName = "CORRECTED_GPS_Doppler";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnLatitude = "latitude";
        public static readonly string ColumnLongitude = "longitude";
        public static readonly string ColumnAltitude = "altitude";
        public static readonly string ColumnHeading = "heading";
        public static readonly string ColumnSpeed = "speed";
        public static readonly string ColumnBearing = "bearing";
        public static readonly string ColumnDistanceDifference = "distance_difference";
        public static readonly string ColumnAccuracy = "accuracy";
        public static readonly string ColumnLinkId = "link_id";
        public static readonly string ColumnTerrainAltitude = "terrain_altitude";
        public static readonly string ColumnRoadTheta = "road_theta";

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
            //GPRMC車速から参照するように改造済み
            var query = new StringBuilder();
            query.AppendLine($"WITH convert_gps");
            query.AppendLine($"AS (");
            query.AppendLine($"	SELECT GPS.{ColumnDriverId}");
            query.AppendLine($"		,GPS.{ColumnCarId}");
            query.AppendLine($"		,GPS.{ColumnSensorId}");
            query.AppendLine($"		,CONVERT(DATETIME, CONVERT(VARCHAR(30), CONVERT(DATETIME, (");
            query.AppendLine($"						CASE ");
            query.AppendLine($"							WHEN DATEPART(Ms, GPS.{ColumnJst}) >= 500");
            query.AppendLine($"								THEN DATEADD(SECOND, 1, GPS.{ColumnJst})");
            query.AppendLine($"							ELSE GPS.{ColumnJst}");
            query.AppendLine($"							END");
            query.AppendLine($"						)), 20)) AS {ColumnJst}");
            query.AppendLine($"		,GPS.{ColumnLatitude}");
            query.AppendLine($"		,GPS.{ColumnLongitude}");
            query.AppendLine($"						,CASE ");
            query.AppendLine($"							WHEN GPS.{ColumnBearing} IS NULL");
            query.AppendLine($"								THEN RMC.MOVING_SPEED * 1.852");
            query.AppendLine($"							ELSE GPS.{ColumnSpeed}");
            query.AppendLine($"							END AS {ColumnSpeed}");
            query.AppendLine($"		,{ColumnHeading}");
            query.AppendLine($"		,{ColumnDistanceDifference}");
            query.AppendLine($"		,{ColumnLinkId}");
            query.AppendLine($"		,{ColumnRoadTheta}");
            query.AppendLine($"	FROM {TableName} AS GPS");
            query.AppendLine($"	LEFT JOIN GPRMC_RAW AS RMC");
            query.AppendLine($"	ON GPS.{ColumnSensorId} = RMC.{ColumnSensorId}");
            query.AppendLine($"	    AND GPS.{ColumnJst} = RMC.{ColumnJst}");
            query.AppendLine($"	WHERE GPS.{ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"		AND GPS.{ColumnCarId} = {datum.CarId}");
            query.AppendLine($"		AND GPS.{ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"     AND GPS.{ColumnJst} >= '{startTime}'");
            query.AppendLine($"     AND GPS.{ColumnJst} <= '{endTime}'");
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
            query.AppendLine($"	,MIN({ColumnLinkId}) AS {ColumnLinkId}");
            query.AppendLine($"	,CAST(AVG({ColumnRoadTheta}) AS real) AS {ColumnRoadTheta}");
            query.AppendLine($"FROM convert_gps");
            query.AppendLine($"WHERE speed IS NOT NULL");
            query.AppendLine($"GROUP BY {ColumnDriverId}");
            query.AppendLine($"	,{ColumnCarId}");
            query.AppendLine($"	,{ColumnSensorId}");
            query.AppendLine($"	,{ColumnJst}");
            query.AppendLine($"ORDER BY {ColumnJst}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
