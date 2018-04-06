using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class PlaceDao
    {
        public static readonly string ColumnPlaceID = "place_id";
        public static readonly string ColumnPlaceName = "place_name";
        public static readonly string ColumnStartLatitude = "start_latitude";
        public static readonly string ColumnEndLatitude = "end_latitude";
        public static readonly string ColumnStartLongitude = "start_longitude";
        public static readonly string ColumnEndLongitude = "end_longitude";
        public static readonly string ColumnStartDate = "start_date";
        public static readonly string ColumnEndDate = "end_date";
        public static readonly string ColumnProperty = "property";


        private static readonly string TableName = "place";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }
        public static int GetPlace(string placeName)
        {
            var query = new StringBuilder();
            query.AppendLine("SELECT * ");
            query.AppendLine($"FROM {TableName}");
            query.AppendLine($"WHERE place_name = '{placeName}'");

            return DatabaseAccesser.GetResult(query.ToString())
                .AsEnumerable()
                .Select(v => v.Field<int?>(EfficiencyDao.ColumnEfficiency)).FirstOrDefault() ?? -1;
        }

    }
}
