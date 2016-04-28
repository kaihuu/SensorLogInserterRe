using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class EfficiencyMaxDao
    {
        public static readonly string ColumnTorque = "torque";
        public static readonly string ColumnRev = "rev";
        public static readonly string ColumnEfficiency = "efficiency";

        private static readonly string TableName = "efficiency_map_max";

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
