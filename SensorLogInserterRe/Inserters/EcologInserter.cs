using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Cleansers;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Inserters
{
    class EcologInserter
    {
        public static void InsertEcolog(DateTime startDate, DateTime endDate, UserDatum datum)
        {
            var tripsTable = TripsDao.Get(startDate, endDate, datum);

            foreach (DataRow row in tripsTable.Rows)
            {
                // TODO 表示イベント
                // StateLabel = "データを挿入中(ECOLOG):" + i + "/" + tripTable.Rows.Count + "件挿入完了";
            }
        }

    }
}
