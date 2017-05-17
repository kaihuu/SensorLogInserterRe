using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class TripsSpeedLPF005MMDao
    {
        private static readonly string TableName = "[trips_speedlpf0.05_MM_links_lookup2]";
   //     private static readonly string EcologTableName = "[ecolog_speedlpf0.05_MM_linkf_lookup]";
        public static readonly string ColumnTripId = "trip_id";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnStartTime = "start_time";
        public static readonly string ColumnEndTime = "end_time";
        public static readonly string ColumnStartLatitude = "start_latitude";
        public static readonly string ColumnStartLongitude = "start_longitude";
        public static readonly string ColumnEndLatitude = "end_latitude";
        public static readonly string ColumnEndLongitude = "end_longitude";
        public static readonly string ColumnConsumedEnergy = "consumed_energy";
        public static readonly string ColumnTripDirection = "trip_direction";
        public static readonly string ColumnValidation = "validation";

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
            query.AppendLine($"FROM {TripsSpeedLPF005MMDao.TableName}");
            query.AppendLine($"WHERE {TripsSpeedLPF005MMDao.ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"AND {TripsSpeedLPF005MMDao.ColumnCarId} = {datum.CarId}");
            query.AppendLine($"AND {TripsSpeedLPF005MMDao.ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"AND {TripsSpeedLPF005MMDao.ColumnStartTime} >= '{datum.StartTime}'");
            query.AppendLine($"AND {TripsSpeedLPF005MMDao.ColumnEndTime} <= '{datum.EndTime}'");
            query.AppendLine($"ORDER BY {ColumnStartTime}");

            return DatabaseAccesser.GetResult(query.ToString());
        }

        public static void DeleteTrips()
        {
            var query = new StringBuilder();

            query.AppendLine("DELETE");
            query.AppendLine($"FROM {TripsSpeedLPF005MMDao.TableName}");
            query.AppendLine("WHERE NOT EXISTS");
            query.AppendLine("(SELECT *");
            query.AppendLine($"FROM {EcologSpeedLPF005MMDao.TableName}");
            query.AppendLine($"WHERE {EcologSpeedLPF005MMDao.ColumnTripId} = {TripsSpeedLPF005MMDao.ColumnTripId}");

            DatabaseAccesser.Delete(query.ToString());
        }


        public static int GetMaxTripId()
        {
            string query = $"SELECT MAX({ColumnTripId}) AS max_id FROM {TableName}";

            return DatabaseAccesser.GetResult(query).Rows[0].Field<int?>("max_id") ?? 0;
        }

        public static bool IsExsistsTrip(DataRow row)
        {
            var query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"FROM {TableName}");
            query.AppendLine($"WHERE {ColumnDriverId} = {row.Field<int>(ColumnDriverId)}");
            query.AppendLine($"  AND {ColumnCarId} = {row.Field<int>(ColumnCarId)}");
            query.AppendLine($"  AND {ColumnSensorId} = {row.Field<int>(ColumnSensorId)}");
            // SQL ServerではDateTime(1)型のミリ秒を切り上げするので±１秒の間をもうける
            query.AppendLine($"  AND {ColumnStartTime} > '{row.Field<DateTime>(ColumnStartTime).AddSeconds(-1)}'");
            query.AppendLine($"  AND {ColumnEndTime} < '{row.Field<DateTime>(ColumnEndTime).AddSeconds(1)}'");

            return DatabaseAccesser.GetResult(query.ToString()).AsEnumerable().Count() != 0;
        }

        public static void UpdateConsumedEnergy()
        {
            var selectQuery = new StringBuilder();
            selectQuery.AppendLine("SELECT trip.trip_id, SUM(consumed_electric_energy) AS consumed_energy");
            selectQuery.AppendLine($"FROM {TableName} AS trip, {EcologSpeedLPF005MMDao.TableName} AS ecolog");
            selectQuery.AppendLine("WHERE consumed_energy IS NULL");
            selectQuery.AppendLine("  AND trip.trip_id = ecolog.trip_id");
            selectQuery.AppendLine("GROUP BY trip.trip_id");

            var resultTable = DatabaseAccesser.GetResult(selectQuery.ToString());

            foreach (DataRow row in resultTable.Rows)
            {
                var updateQuery = new StringBuilder();
                updateQuery.AppendLine($"UPDATE {TableName} ");
                updateQuery.AppendLine($"SET consumed_energy = '{row.Field<double>(1)}'");
                updateQuery.AppendLine($"WHERE trip_id = {row.Field<int>(0)}");

                DatabaseAccesser.Update(updateQuery.ToString());
            }
        }
    }
}
