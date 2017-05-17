using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class TripsRawMMDao
    {
        private static readonly string TableName = "[trips_raw_mm_links_lookup2]";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnStartTime = "start_time";
        public static readonly string ColumnEndTime = "end_time";
        public static readonly string ColumnStartLatitude = "start_latitude";
        public static readonly string ColumnStartLongitude = "start_longitude";
        public static readonly string ColumnEndLatitude = "end_latitude";
        public static readonly string ColumnEndLongitude = "end_longitude";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(InsertDatum datum)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT *");
            query.AppendLine($"FROM {TripsRawMMDao.TableName}");
            query.AppendLine($"WHERE {TripsRawMMDao.ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"AND {TripsRawMMDao.ColumnCarId} = {datum.CarId}");
            query.AppendLine($"AND {TripsRawMMDao.ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"AND {TripsRawMMDao.ColumnStartTime} >= '{datum.StartTime}'");
            query.AppendLine($"AND {TripsRawMMDao.ColumnEndTime} <= '{datum.EndTime}'");
            query.AppendLine($"ORDER BY {ColumnStartTime}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
