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
        public static void InsertEcolog(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction, out int count)
        {
            count = 0;
            var tripsTable = TripsDao.Get(datum);
            //int i = 1;

            //foreach (DataRow row in tripsTable.Rows)
            //{
            //    updateTextDelegate($"Insetring ECOLOG ... , {i} / {tripsTable.Rows.Count}");
            //    LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOG... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
            //    var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
            //    EcologDao.Insert(ecologTable);

            //    i++;
            //}
            int t = 0;
            Parallel.For(0, tripsTable.Rows.Count, i =>
           {
               if (tripsTable.Rows[i][(TripsDao.ColumnConsumedEnergy)] == DBNull.Value)
               {
                   updateTextDelegate($"Insetring ECOLOG ... , {i + 1} / {tripsTable.Rows.Count}");
                   LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOG... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                   var ecologTable = HagimotoEcologCalculator.CalcEcolog(tripsTable.Rows[i], datum, InsertConfig.GpsCorrection.Normal);
                   EcologSimulationDao.Insert(ecologTable);
                   t++;
               }
               
           });
            count = t;
            TripsDao.UpdateConsumedEnergy();
        }
        public static void InsertEcologSpeedLPF005MM(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = TripsSpeedLPF005MMDao.Get(datum);
            //int i = 1;

            //foreach (DataRow row in tripsTable.Rows)
            //{
            //    updateTextDelegate($"Insetring ECOLOGECOLOGSpeedLPF005MM ... , {i} / {tripsTable.Rows.Count}");
            //    LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGSpeedLPF005MM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
            //    var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
            //    EcologSpeedLPF005MMDao.Insert(ecologTable);

            //    i++;
            //}

            Parallel.For(0, tripsTable.Rows.Count, i =>
            {
                if (tripsTable.Rows[i][(TripsDao.ColumnConsumedEnergy)] == DBNull.Value)
                {
                    updateTextDelegate($"Insetring ECOLOGECOLOGSpeedLPF005MM ... , {i + 1} / {tripsTable.Rows.Count}");
                    LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGSpeedLPF005MM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                    var ecologTable = HagimotoEcologCalculator.CalcEcolog(tripsTable.Rows[i], datum, correction);
                    EcologSpeedLPF005MMDao.Insert(ecologTable);

                }
            });

            TripsSpeedLPF005MMDao.UpdateConsumedEnergy();
        }
        public static void InsertEcologMM(InsertDatum datum, MainWindowViewModel.UpdateTextDelegate updateTextDelegate, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = TripsMMDao.Get(datum);
            //int i = 1;

            //foreach (DataRow row in tripsTable.Rows)
            //{
            //    updateTextDelegate($"Insetring ECOLOGMM ... , {i} / {tripsTable.Rows.Count}");
            //    LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGMM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
            //    var ecologTable = HagimotoEcologCalculator.CalcEcolog(row, datum, correction);
            //    EcologMMDao.Insert(ecologTable);

            //    i++;
            //}
            Parallel.For(0, tripsTable.Rows.Count, i =>
            {
                if (tripsTable.Rows[i][(TripsDao.ColumnConsumedEnergy)] == DBNull.Value)
                {
                    updateTextDelegate($"Insetring ECOLOGMM ... , {i + 1} / {tripsTable.Rows.Count}");
                    LogWritter.WriteLog(LogWritter.LogMode.Ecolog, $"Insetring ECOLOGMM... , { i} / { tripsTable.Rows.Count}, Datum: {datum}");
                    var ecologTable = HagimotoEcologCalculator.CalcEcolog(tripsTable.Rows[i], datum, correction);
                    EcologMMDao.Insert(ecologTable);
                }

            });

            TripsMMDao.UpdateConsumedEnergy();
        }
    }
}
