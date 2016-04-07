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

        private static readonly string TableName = "efficiency_map";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(EfficiencyDao.TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

    }
}
