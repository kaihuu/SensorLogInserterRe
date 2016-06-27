using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Inserters.Components;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Inserters
{
    class EcologInserter
    {
        public static void InsertEcolog(InsertDatum datum)
        {
            var tripsTable = TripsDao.Get(datum);
            int i = 1;

            foreach (DataRow row in tripsTable.Rows)
            {
                Console.WriteLine($"TRIP INDEX: {i}, START: {DateTime.Now}");
                //LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"TRIP INDEX: {i}, START: {DateTime.Now}");
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum);
                EcologDao.Insert(ecologTable);
                //LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"TRIP INDEX: {i}, END: {DateTime.Now}");

                i++;
            }
        }

    }
}
