using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class Altitude10MMeshDao
    {
        private static readonly string TableName = "altitude_10m_mesh";
        public static readonly string ColumnLowerLatitude = "lower_latitude";
        public static readonly string ColumnLowerLongitude = "lower_longitude";
        public static readonly string ColumnUpperLatitude = "upper_latitude";
        public static readonly string ColumnUpperLongitude = "upper_longitude";
        public static readonly string ColumnAltitude = "altitude";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static AltitudeDatum Get(double latitude, double longitude)
        {
            string query = "select * ";
            query += $"FROM {TableName} ";
            query += $"WHERE {ColumnLowerLatitude} <= " + latitude + " ";
            query += $"AND {ColumnUpperLatitude} > " + latitude + " ";
            query += $"AND {ColumnLowerLongitude} <= " + longitude + " ";
            query += $"AND {ColumnUpperLongitude} > " + longitude + " ";

            var result = DatabaseAccesser.GetResult(query);
            AltitudeDatum resultDatum = new AltitudeDatum();
            if (result.Rows.Count > 0)
            {
                resultDatum = new AltitudeDatum
                {
                    LowerLatitude = result.Rows[0].Field<double?>(ColumnLowerLatitude),
                    LowerLongitude = result.Rows[0].Field<double?>(ColumnLowerLongitude),
                    UpperLatitude = result.Rows[0].Field<double?>(ColumnUpperLatitude),
                    UpperLongitude = result.Rows[0].Field<double?>(ColumnUpperLongitude),
                    Altitude = result.Rows[0].Field<float?>(ColumnAltitude)
                };
            }
            return resultDatum;
        }
    }
}
