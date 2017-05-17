using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class TripsRawSpeedLPF005MMDao
    {
        private static readonly string TableName = "[trips_raw_speedlpf0.05_mm_links_lookup2]";
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
            query.AppendLine($"FROM {TripsRawSpeedLPF005MMDao.TableName}");
            query.AppendLine($"WHERE {TripsRawSpeedLPF005MMDao.ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"AND {TripsRawSpeedLPF005MMDao.ColumnCarId} = {datum.CarId}");
            query.AppendLine($"AND {TripsRawSpeedLPF005MMDao.ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"AND {TripsRawSpeedLPF005MMDao.ColumnStartTime} >= '{datum.StartTime}'");
            query.AppendLine($"AND {TripsRawSpeedLPF005MMDao.ColumnEndTime} <= '{datum.EndTime}'");
            query.AppendLine($"ORDER BY {ColumnStartTime}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}

