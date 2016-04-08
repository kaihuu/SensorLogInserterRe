using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class EfficiencyDao
    {
        public static readonly string ColumnTorque = "torque";
        public static readonly string ColumnRev = "rev";
        public static readonly string ColumnEfficiency = "efficiency";

        private static readonly string EfficiencyTableName = "efficiency_map";
        private static readonly string EfficiencyMaxTableName = "efficiency_map_max";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(EfficiencyDao.EfficiencyTableName, dataTable);
        }

        public static void InsertMax(DataTable dataTable)
        {
            DatabaseAccesser.Insert(EfficiencyDao.EfficiencyMaxTableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + EfficiencyTableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable GetMax()
        {
            string query = "SELECT * FROM " + EfficiencyMaxTableName;

            return DatabaseAccesser.GetResult(query);
        }

    }
}
