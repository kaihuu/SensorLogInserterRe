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
using SensorLogInserterRe.ViewModels;

namespace SensorLogInserterRe.Inserters
{
    class EcologInserter
    {
        public static void InsertEcolog(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate)
        {
            var tripsTable = TripsDao.Get(datum);
            int i = 1;

            foreach (DataRow row in tripsTable.Rows)
            {
                updateTextDelegate($"Insetring ECOLOG ... , {i} / {tripsTable.Rows.Count}");
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum);
                EcologDao.Insert(ecologTable);

                i++;
            }

            TripsDao.UpdateConsumedEnergy();
        }
    }
}
