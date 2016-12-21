using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class Altitude10MMeshRegisteredDao
    {
        private static readonly string TableName = "altitude_10m_mesh_registered_fixed";
        public static readonly string ColumnMeshId = "mesh_id";
        public static readonly string ColumnLowerLatitude = "lower_latitude";
        public static readonly string ColumnLowerLongitude = "lower_longitude";
        public static readonly string ColumnUpperLatitude = "upper_latitude";
        public static readonly string ColumnUpperLongitude = "upper_longitude";
        public static readonly string ColumnAltitude = "altitude";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static void Insert(int meshId, AltitudeDatum datum)
        {
            string query = $"INSERT INTO {TableName}({ColumnMeshId}, {ColumnLowerLatitude}, {ColumnLowerLongitude}, {ColumnUpperLatitude}, {ColumnUpperLongitude}, {ColumnAltitude}) ";
            query += $"VALUES('{meshId}', '{datum.LowerLatitude}', '{datum.LowerLongitude}', '{datum.UpperLatitude}', '{datum.UpperLongitude}', '{datum.Altitude}') ";

            DatabaseAccesser.Insert(query);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }
        public static DataTable GetAltitude(double latitude, double longitude)
        {
            string query = "SELECT * FROM " + TableName;
            query += " WHERE lower_latitude <= " + latitude + " AND upper_latitude > " + latitude + " AND lower_longitude <= " + longitude;
            query += " AND upper_longitude > " + longitude;


            return DatabaseAccesser.GetResult(query);
        }

        public static int GetMaxMeshId()
        {
            string query = "SELECT MAX(mesh_id) AS max_id ";
            query += $"FROM {TableName}";

            return DatabaseAccesser.GetResult(query).Rows[0].Field<int>("max_id");
        }
    }
}
