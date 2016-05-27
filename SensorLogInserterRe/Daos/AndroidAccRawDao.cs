using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class AndroidAccRawDao
    {
        private static readonly string TableName = "android_acc_raw";
        public static readonly string ColumnDateTime = "datetime";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnAccX = "acc_x";
        public static readonly string ColumnAccY = "acc_y";
        public static readonly string ColumnAccZ = "acc_z";

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
