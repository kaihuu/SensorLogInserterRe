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
        public static void InsertEcolog(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = TripsDao.Get(datum);
            int i = 1;

            foreach (DataRow row in tripsTable.Rows)
            {
                updateTextDelegate($"Insetring ECOLOG ... , {i} / {tripsTable.Rows.Count}");
                LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOG... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
                EcologDao.Insert(ecologTable);

                i++;
            }

            TripsDao.UpdateConsumedEnergy();
        }
        public static void InsertEcologSpeedLPF005MM(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = TripsSpeedLPF005MMDao.Get(datum);
            int i = 1;

            foreach (DataRow row in tripsTable.Rows)
            {
                updateTextDelegate($"Insetring ECOLOGECOLOGSpeedLPF005MM ... , {i} / {tripsTable.Rows.Count}");
                LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGSpeedLPF005MM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
                EcologSpeedLPF005MMDao.Insert(ecologTable);

                i++;
            }

            TripsSpeedLPF005MMDao.UpdateConsumedEnergy();
        }
        public static void InsertEcologMM(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = TripsMMDao.Get(datum);
            int i = 1;

            foreach (DataRow row in tripsTable.Rows)
            {
                updateTextDelegate($"Insetring ECOLOGMM ... , {i} / {tripsTable.Rows.Count}");
                LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGMM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
                EcologMMDao.Insert(ecologTable);

                i++;
            }

            TripsMMDao.UpdateConsumedEnergy();
        }
    }
}
