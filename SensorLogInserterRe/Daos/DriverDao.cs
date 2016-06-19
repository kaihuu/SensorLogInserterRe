using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Daos
{
    class DriverDao
    {
        public static readonly string TableName = "drivers";

        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnName = "name";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(string driverName)
        {
            string query = $"SELECT * FROM {TableName} WHERE {ColumnName} = '{driverName}'";

            return DatabaseAccesser.GetResult(query);
        } 
    }
}
