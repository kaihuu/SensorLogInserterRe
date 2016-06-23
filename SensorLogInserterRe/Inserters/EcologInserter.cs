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
            Console.WriteLine("CALLED: InsertEcolog" + datum);

            var tripsTable = TripsDao.Get(datum);

            foreach (DataRow row in tripsTable.Rows)
            {
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum);
                EcologDao.Insert(ecologTable);
            }
        }

    }
}
