using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class AndroidGpsRawDao
    {
        private static readonly string TableName = "android_gps_raw";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnAndroidTime = "android_time";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnLatitude = "latitude";
        public static readonly string ColumnLongitude = "longitude";
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
