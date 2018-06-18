using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class SensorNameDao
    {
        public static readonly string TableName = "sensor_name";

        public static readonly string ColumnSensorName = "sensor_name";
        public static readonly string ColumnSensorId = "sensor_id";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(string sensorName)
        {
            string query = $"SELECT * FROM {TableName} WHERE {ColumnSensorName} = '{sensorName}'";

            return DatabaseAccesser.GetResult(query);
        }
    }
}
