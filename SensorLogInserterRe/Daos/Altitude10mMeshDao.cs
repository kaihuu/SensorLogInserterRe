using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;

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
    }
}
