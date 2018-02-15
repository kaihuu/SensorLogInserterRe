using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class TripsDao
    {
        private static readonly string TableName = "trips_simulation";
      //  private static readonly string EcologTableName = "ecolog_links_lookup";
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

        public static void DeleteTrips()
        {
            var query = new StringBuilder();

            query.AppendLine("DELETE");
            query.AppendLine($"FROM {TripsDao.TableName}");
            query.AppendLine("WHERE NOT EXISTS");
            query.AppendLine("(SELECT *");
            query.AppendLine($"FROM {EcologSimulationDao.TableName}");
            query.AppendLine($"WHERE {EcologSimulationDao.ColumnTripId} = {TripsDao.ColumnTripId}");

            DatabaseAccesser.Delete(query.ToString());
        }

        public static DataTable Get(InsertDatum datum)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT *");
            query.AppendLine($"FROM {TripsDao.TableName}");
            query.AppendLine($"WHERE {TripsDao.ColumnDriverId} = {datum.DriverId}");
            query.AppendLine($"AND {TripsDao.ColumnCarId} = {datum.CarId}");
            query.AppendLine($"AND {TripsDao.ColumnSensorId} = {datum.SensorId}");
            query.AppendLine($"AND {TripsDao.ColumnStartTime} >= '{datum.StartTime}'");
            query.AppendLine($"AND {TripsDao.ColumnEndTime} <= '{datum.EndTime}'");
            query.AppendLine($"ORDER BY {ColumnStartTime}");

            return DatabaseAccesser.GetResult(query.ToString());
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
            selectQuery.AppendLine($"FROM {TripsDao.TableName} AS trip, {EcologSimulationDao.TableName} AS ecolog");
            selectQuery.AppendLine("WHERE consumed_energy IS NULL");
            selectQuery.AppendLine("  AND trip.trip_id = ecolog.trip_id");
            selectQuery.AppendLine("GROUP BY trip.trip_id");

            var resultTable = DatabaseAccesser.GetResult(selectQuery.ToString());

            foreach (DataRow row in resultTable.Rows)
            {
                var updateQuery = new StringBuilder();
                updateQuery.AppendLine($"UPDATE {TripsDao.TableName}");
                updateQuery.AppendLine($"SET consumed_energy = '{row.Field<double>(1)}'");
                updateQuery.AppendLine($"WHERE trip_id = {row.Field<int>(0)}");

                DatabaseAccesser.Update(updateQuery.ToString());
            }
        }
    }
}
