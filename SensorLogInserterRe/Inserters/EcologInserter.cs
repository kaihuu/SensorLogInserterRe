using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Inserters.Components;
using SensorLogInserterRe.Models;

namespace SensorLogInserterRe.Inserters
{
    class EcologInserter
    {
        public static void InsertEcolog(InsertDatum datum)
        {
            var tripsTable = TripsDao.Get(datum);

            foreach (DataRow row in tripsTable.Rows)
            {
                // TODO 表示イベント
                // StateLabel = "データを挿入中(ECOLOG):" + i + "/" + tripTable.Rows.Count + "件挿入完了";

                HagimotoEcologCalculator.CalcEcolog(row, datum);
            }
        }

    }
}
