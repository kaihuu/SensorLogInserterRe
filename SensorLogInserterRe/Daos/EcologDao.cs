using ECOLOGSemanticViewer.Models.EcologModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class EcologDao
    {
        private static readonly string TableName = "ecolog";

        private static EcologDao Instance;

        public static EcologDao GetInstance()
        {
            if(Instance == null) Instance = new EcologDao();

            return Instance;
        }

        private EcologDao(){

        }

        public void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(EcologDao.TableName, dataTable);
        }

        public DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public DataTable Get(DateTime startPeriod, DateTime endPeriod)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT *");
            sb.AppendLine("FROM " + TableName);
            sb.AppendLine("WHERE jst >= " + startPeriod);
            sb.AppendLine(" AND jst <= " + endPeriod);

            return DatabaseAccesser.GetResult(sb.ToString());
        }
    }
}
