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
    class CorrectedAccInserter
    {
        public static void InsertCorrectedAcc(DateTime startDate, DateTime endDate, UserDatum datum)
        {
            var tripsTable = TripsDao.Get(startDate, endDate, datum);

            foreach (DataRow row in tripsTable.Rows)
            {
                var correctedAccTable = AccCorrector.CorrectAcc(startDate, endDate, datum, row);
                CorrectedAccDao.Insert(correctedAccTable);
            }
        }
    }
}
