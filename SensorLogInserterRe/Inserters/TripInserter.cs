using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Constant;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using SensorLogInserterRe.ViewModels;

namespace SensorLogInserterRe.Inserters
{
    class TripInserter
    {
        public static void InsertTripRaw(DataTable gpsRawTable, InsertConfig.GpsCorrection correction)
        {
            var tripsTable = DataTableUtil.GetTripsRawTable();
            DataRow row = tripsTable.NewRow();

            row.SetField(TripsRawDao.ColumnDriverId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnDriverId));
            row.SetField(TripsRawDao.ColumnCarId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnCarId));
            row.SetField(TripsRawDao.ColumnSensorId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnSensorId));
            row.SetField(TripsRawDao.ColumnStartTime, gpsRawTable.Rows[0].Field<DateTime>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnStartLatitude,
                gpsRawTable.Rows[0].Field<double>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnStartLongitude,
                gpsRawTable.Rows[0].Field<double>(AndroidGpsRawDao.ColumnLongitude));
            row.SetField(TripsRawDao.ColumnEndTime,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<DateTime>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnEndLatitude,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<double>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnEndLongitude,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<double>(AndroidGpsRawDao.ColumnLongitude));
            tripsTable.Rows.Add(row);

            // GPSファイルごとの処理なので主キー違反があっても挿入されないだけ
            if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
            {
                TripsRawSpeedLPF005MMDao.Insert(tripsTable);
            }
            else if (correction == InsertConfig.GpsCorrection.MapMatching)
            {
                TripsRawMMDao.Insert(tripsTable);
            }
            else
            {
                TripsRawDao.Insert(tripsTable);
            }
        }

        public static void InsertTrip(InsertDatum datum, InsertConfig.GpsCorrection correction)
        {
            LogWritter.WriteLog(LogWritter.LogMode.Trip, $"TRIP挿入開始, DRIVER_ID: {datum.DriverId}, CAR_ID: {datum.CarId}, SENSOR_ID: {datum.SensorId}");
            var tripsRawTable = new DataTable();
            if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
            {
                tripsRawTable = TripsRawSpeedLPF005MMDao.Get(datum);
                TripsSpeedLPF005MMDao.DeleteTrips(); //途中中断された際に作成したトリップを削除
            }
            else if (correction == InsertConfig.GpsCorrection.MapMatching)
            {
                tripsRawTable = TripsRawMMDao.Get(datum);
                TripsMMDao.DeleteTrips(); //途中中断された際に作成したトリップを削除
            }
            else
            {
                tripsRawTable = TripsRawDao.Get(datum);
                TripsDao.DeleteTrips(); //途中中断された際に作成したトリップを削除
            }


            LogWritter.WriteLog(LogWritter.LogMode.Trip, $"挿入対象のRAWデータ: {tripsRawTable.Rows.Count}");


            for (int i = 0; i < tripsRawTable.Rows.Count; i++)
            {
                DataTable tripsTable = DataTableUtil.GetTripsTable();

                // 自宅出発
                if (IsHome(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
                    tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude),
                    tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime),
                    datum))
                {
                    InsertOutwardTrip(tripsRawTable, tripsTable, datum, i, correction);
                }
                // YNU出発
                else if (IsYnu(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
                    tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude)))
                {
                    InsertHomewardTrip(tripsRawTable, tripsTable, datum, i, correction);
                }

                //自宅・YNU以外出発
                else if (IsOther(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
                     tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude)))
                {
                    InsertOtherTrip(tripsRawTable, tripsTable, datum, i, correction);
                }

                // 1トリップごとなので主キー違反があっても挿入されないだけ
                if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                {
                    TripsSpeedLPF005MMDao.Insert(tripsTable);
                }
                else if (correction == InsertConfig.GpsCorrection.MapMatching)
                {
                    TripsMMDao.Insert(tripsTable);
                }
                else
                {
                    TripsDao.Insert(tripsTable);
                }

            }
        }

        private static bool IsHome(double latitude, double longitude, DateTime date, InsertDatum datum)
        {
            if (latitude > Coordinate.TommyHome.LatitudeStart
                && latitude < Coordinate.TommyHome.LatitudeEnd
                && longitude > Coordinate.TommyHome.LongitudeStart
                && longitude < Coordinate.TommyHome.LongitudeEnd)
                return true;

            if (latitude > Coordinate.MoriHome.LatitudeStart
                && latitude < Coordinate.MoriHome.LatitudeEnd
                && longitude > Coordinate.MoriHome.LongitudeStart
                && longitude < Coordinate.MoriHome.LongitudeEnd)
                return true;

            if (latitude > Coordinate.TamuraHomeBefore.LatitudeStart
                && latitude < Coordinate.TamuraHomeBefore.LatitudeEnd
                && longitude > Coordinate.TamuraHomeBefore.LongitudeStart
                && longitude < Coordinate.TamuraHomeBefore.LongitudeEnd
                && date < Coordinate.TamuraHomeBefore.EndDate)
                return true;

            if (latitude > Coordinate.TamuraHomeAfter.LatitudeStart
                && latitude < Coordinate.TamuraHomeAfter.LatitudeEnd
                && longitude > Coordinate.TamuraHomeAfter.LongitudeStart
                && longitude < Coordinate.TamuraHomeAfter.LongitudeEnd
                && date > Coordinate.TamuraHomeAfter.StartDate)
                return true;

            if (latitude > Coordinate.AyaseCityHall.LatitudeStart
                && latitude < Coordinate.AyaseCityHall.LatitudeEnd
                && longitude > Coordinate.AyaseCityHall.LongitudeStart
                && longitude < Coordinate.AyaseCityHall.LongitudeEnd)
                return true;

            if (latitude > Coordinate.UemuraHome.LatitudeStart
                && latitude < Coordinate.UemuraHome.LatitudeEnd
                && longitude > Coordinate.UemuraHome.LongitudeStart
                && longitude < Coordinate.UemuraHome.LongitudeEnd)
                return true;

            return false;
        }

        private static bool IsYnu(double latitude, double longitude)
        {
            return latitude > Coordinate.Ynu.LatitudeStart && latitude < Coordinate.Ynu.LatitudeEnd && longitude > Coordinate.Ynu.LongitudeStart && longitude < Coordinate.Ynu.LongitudeEnd;
        }

        private static bool IsOther(double latitude, double longitude)
        {
            //その他登録地点
            if (latitude > Coordinate.OaraiTerminal.LatitudeStart       //大洗フェリーターミナル
                && latitude < Coordinate.OaraiTerminal.LatitudeEnd
                && longitude > Coordinate.OaraiTerminal.LongitudeStart
                && longitude < Coordinate.OaraiTerminal.LongitudeEnd)
                return true;

            if (latitude > Coordinate.TomakomaiTerminal.LatitudeStart   //苫小牧フェリーターミナル
                && latitude < Coordinate.TomakomaiTerminal.LatitudeEnd
                && longitude > Coordinate.TomakomaiTerminal.LongitudeStart
                && longitude < Coordinate.TomakomaiTerminal.LongitudeEnd)
                return true;

            if (latitude > Coordinate.RoytonSapporo.LatitudeStart       //ロイトン札幌
                && latitude < Coordinate.RoytonSapporo.LatitudeEnd
                && longitude > Coordinate.RoytonSapporo.LongitudeStart
                && longitude < Coordinate.RoytonSapporo.LongitudeEnd)
                return true;

            if (latitude > Coordinate.HotelTaisetsu.LatitudeStart       //ホテル大雪（たいせつ）
                && latitude < Coordinate.HotelTaisetsu.LatitudeEnd
                && longitude > Coordinate.HotelTaisetsu.LongitudeStart
                && longitude < Coordinate.HotelTaisetsu.LongitudeEnd)
                return true;

            if (latitude > Coordinate.Oehonke.LatitudeStart             //大江本家
                && latitude < Coordinate.Oehonke.LatitudeEnd
                && longitude > Coordinate.Oehonke.LongitudeStart
                && longitude < Coordinate.Oehonke.LongitudeEnd)
                return true;

            if (latitude > Coordinate.KKRKawayu.LatitudeStart           //KKRかわゆ
                && latitude < Coordinate.KKRKawayu.LatitudeEnd
                && longitude > Coordinate.KKRKawayu.LongitudeStart
                && longitude < Coordinate.KKRKawayu.LongitudeEnd)
                return true;

            if (latitude > Coordinate.MichinoEkiMashu.LatitudeStart      //道の駅摩周温泉
                && latitude < Coordinate.MichinoEkiMashu.LatitudeEnd
                && longitude > Coordinate.MichinoEkiMashu.LongitudeStart
                && longitude < Coordinate.MichinoEkiMashu.LongitudeEnd)
                return true;

            if (latitude > Coordinate.ToujiIkkyu.LatitudeStart      //かよい湯治一休
                && latitude < Coordinate.ToujiIkkyu.LatitudeEnd
                && longitude > Coordinate.ToujiIkkyu.LongitudeStart
                && longitude < Coordinate.ToujiIkkyu.LongitudeEnd)
                return true;

            if (latitude > Coordinate.Inubozaki.LatitudeStart      //犬吠埼灯台
                && latitude < Coordinate.Inubozaki.LatitudeEnd
                && longitude > Coordinate.Inubozaki.LongitudeStart
                && longitude < Coordinate.Inubozaki.LongitudeEnd)
                return true;

            /***シミュレーション用***/
            if (latitude > Coordinate.S01S.LatitudeStart 
                && latitude < Coordinate.S01S.LatitudeEnd
                && longitude > Coordinate.S01S.LongitudeStart
                && longitude < Coordinate.S01S.LongitudeEnd)
                return true;

            /*if (latitude > Coordinate.S02S.LatitudeStart
                && latitude < Coordinate.S02S.LatitudeEnd
                && longitude > Coordinate.S02S.LongitudeStart
                && longitude < Coordinate.S02S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S03S.LatitudeStart
               && latitude < Coordinate.S03S.LatitudeEnd
               && longitude > Coordinate.S03S.LongitudeStart
               && longitude < Coordinate.S03S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S04S.LatitudeStart
                && latitude < Coordinate.S04S.LatitudeEnd
                && longitude > Coordinate.S04S.LongitudeStart
                && longitude < Coordinate.S04S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S05S.LatitudeStart
               && latitude < Coordinate.S05S.LatitudeEnd
               && longitude > Coordinate.S05S.LongitudeStart
               && longitude < Coordinate.S05S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S06S.LatitudeStart
                && latitude < Coordinate.S06S.LatitudeEnd
                && longitude > Coordinate.S06S.LongitudeStart
                && longitude < Coordinate.S06S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S07S.LatitudeStart
               && latitude < Coordinate.S07S.LatitudeEnd
               && longitude > Coordinate.S07S.LongitudeStart
               && longitude < Coordinate.S07S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S08S.LatitudeStart
                && latitude < Coordinate.S08S.LatitudeEnd
                && longitude > Coordinate.S08S.LongitudeStart
                && longitude < Coordinate.S08S.LongitudeEnd)
                return true;
            if (latitude > Coordinate.S09S.LatitudeStart
                && latitude < Coordinate.S09S.LatitudeEnd
                && longitude > Coordinate.S09S.LongitudeStart
                && longitude < Coordinate.S09S.LongitudeEnd)
                return true;

            if (latitude > Coordinate.S10S.LatitudeStart
               && latitude < Coordinate.S10S.LatitudeEnd
               && longitude > Coordinate.S10S.LongitudeStart
               && longitude < Coordinate.S10S.LongitudeEnd)
                return true;*/

            if (latitude > Coordinate.ShimoSeyaNichome.LatitudeStart
                && latitude < Coordinate.ShimoSeyaNichome.LatitudeEnd
                && longitude > Coordinate.ShimoSeyaNichome.LongitudeStart
                && longitude < Coordinate.ShimoSeyaNichome.LongitudeEnd)
                return true;

            return false;
        }

        private static void InsertOutwardTrip(DataTable tripsRawTable, DataTable tripsTable, InsertDatum datum, int i, InsertConfig.GpsCorrection correction)
        {
            int j = i;
            bool tripChangeFlag = false;

            // TripsRaw を結合して Trips を生成するループ
            while (j < tripsRawTable.Rows.Count && tripChangeFlag == false)
            {
                // 自宅 ⇒ YNU
                if (IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    var row = tripsTable.NewRow();

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnConsumedEnergy, DBNull.Value);
                    row.SetField(TripsDao.ColumnTripDirection, "outward");
                    row.SetField(TripsDao.ColumnValidation, DBNull.Value);

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }

                //自宅　⇒　その他登録地点
                else if (IsOther(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "other");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }

                // 自宅 ⇒ 自宅
                else if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude),
                    tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime), datum))
                {
                    LogWritter.WriteLog(LogWritter.LogMode.Trip, "自宅⇒自宅トリップなので挿入しません "
                          + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    // Trip の挿入は行わない
                    // ループの初期化
                    tripChangeFlag = true;
                }

                j++;

                // 自宅 ⇒ ？
                if (j < tripsRawTable.Rows.Count && tripChangeFlag != true)
                {
                    // 自宅にも、学校にも到着しないまま、開始地点が自宅か学校になった場合
                    if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude),
                            tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLongitude),
                            tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnStartTime), datum)
                        || IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLongitude)))
                    {
                        tripChangeFlag = true;
                        // TODO ログ出力
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "自宅⇒？トリップなので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }
                }
            }
        }

        private static void InsertHomewardTrip(DataTable tripsRawTable, DataTable tripsTable, InsertDatum datum, int i, InsertConfig.GpsCorrection correction)
        {
            int j = i;
            bool tripChangeFlag = false;

            // TripsRaw を結合して Trips を生成するループ
            while (j < tripsRawTable.Rows.Count && tripChangeFlag == false)
            {
                // YNU ⇒ 自宅
                if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude),
                    tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime),
                    datum))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "homeward");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }

                //YNU ⇒ その他登録地点
                else if (IsOther(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "other");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }

                // YNU ⇒ YNU
                else if (IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    LogWritter.WriteLog(LogWritter.LogMode.Trip, "YNU⇒YNUトリップなので挿入しません "
                          + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));

                    // Trip の挿入は行わない
                    // ループの初期化
                    tripChangeFlag = true;
                }

                j++;

                // YNU ⇒ ？
                if (j < tripsRawTable.Rows.Count && tripChangeFlag != true)
                {
                    // 自宅にも、学校にも到着しないまま、開始地点が自宅か学校になった場合
                    if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude),
                            tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLongitude),
                            tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnStartTime), datum)
                        || IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLongitude)))
                    {
                        tripChangeFlag = true;
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "YNU⇒？トリップなので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }
                }
            }
        }

        //TRIP_DIRECTION = 'other' のインサート
        private static void InsertOtherTrip(DataTable tripsRawTable, DataTable tripsTable, InsertDatum datum, int i, InsertConfig.GpsCorrection correction)
        {
            int j = i;
            bool tripChangeFlag = false;

            // TripsRaw を結合して Trips を生成するループ
            while (j < tripsRawTable.Rows.Count && tripChangeFlag == false)
            {
                // その他登録地点　⇒　その他登録地点
                if (IsOther(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "other");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }


                // その他登録地点　⇒　YNU
                if (IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "other");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }

                //その他登録地点　⇒　自宅
                else if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude),
                    tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime),
                    datum))
                {
                    var row = tripsTable.NewRow();
                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsSpeedLPF005MMDao.GetMaxTripId() + 1);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching)
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsMMDao.GetMaxTripId() + 1);
                    }
                    else
                    {
                        row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId() + 1);
                    }
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnTripDirection, "other");

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                            + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[i], tripsRawTable.Rows[j]));
                    }

                    tripChangeFlag = true;
                }


                j++;

            }
        }
        private static string ConvertRowToString(DataRow firstRow, DataRow lastRow)
        {
            return $"DRIVER_ID: {firstRow.Field<int>(TripsDao.ColumnDriverId)}, " +
                   $"CAR_ID: {firstRow.Field<int>(TripsDao.ColumnCarId)}, " +
                   $"SENSOR_ID: {firstRow.Field<int>(TripsDao.ColumnSensorId)}, " +
                   $"START_TIME:{firstRow.Field<DateTime>(TripsDao.ColumnStartTime)}, " +
                   $"END_TIME: {lastRow.Field<DateTime>(TripsDao.ColumnEndTime)}";
        }


    }
}
