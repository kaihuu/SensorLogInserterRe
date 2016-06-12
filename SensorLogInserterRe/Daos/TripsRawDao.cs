using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class TripsRawDao
    {
        private static readonly string TableName = "trips_raw";
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

        public static DataTable Get(DateTime startDate, DateTime endDate, UserDatum datum)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT *");
            query.AppendLine($"FROM {TripsRawDao.TableName}");
            query.AppendLine($"WHERE {TripsRawDao.ColumnDriverId} = {datum.Driver.DriverId}");
            query.AppendLine($"AND {TripsRawDao.ColumnCarId} = {datum.Car.CarId}");
            query.AppendLine($"AND {TripsRawDao.ColumnSensorId} = {datum.Sensor.SensorId}");
            query.AppendLine($"AND {TripsRawDao.ColumnStartTime} >= {startDate}");
            query.AppendLine($"AND {TripsRawDao.ColumnStartTime} <= {endDate}");
            query.AppendLine($"ORDER BY {ColumnStartTime}");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
