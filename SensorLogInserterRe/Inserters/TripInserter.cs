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

namespace SensorLogInserterRe.Inserters
{
    class TripInserter
    {
        public static void InsertTripRaw(DataTable gpsRawTable)
        {
            var tripsTable = DataTableUtil.GetTripsTable();
            DataRow row = tripsTable.NewRow();

            row.SetField(TripsRawDao.ColumnDriverId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnDriverId));
            row.SetField(TripsRawDao.ColumnCarId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnCarId));
            row.SetField(TripsRawDao.ColumnSensorId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnSensorId));
            row.SetField(TripsRawDao.ColumnStartTime, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnStartLatitude,
                gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnStartLongitude,
                gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnLongitude));
            row.SetField(TripsRawDao.ColumnEndTime,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnEndLatitude,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnEndLongitude,
                gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnLongitude));
            tripsTable.Rows.Add(row);

            TripsRawDao.Insert(tripsTable);
        }

        public static void InsertTrip(DateTime startDate, DateTime endDate, UserDatum datum)
        {
            var tripsRawTable = TripsRawDao.Get(startDate, endDate, datum);

            for (int i = 0; i < tripsRawTable.Rows.Count; i++)
            {
                DataTable tripsTable = DataTableUtil.GetTripsTable();

                // 自宅出発
                if (IsHome(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
                    tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude),
                    tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime),
                    datum))
                {
                    i = InsertOutwardTrip(tripsRawTable, tripsTable, datum, i);
                }
                // YNU出発
                else if (IsYnu(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
                    tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude)))
                {
                    i = InsertHomewardTrip(tripsRawTable, tripsTable, datum, i);
                }
            }
        }

        private static bool IsHome(double latitude, double longitude, DateTime date, UserDatum datum)
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
                && longitude < Coordinate.AyaseCityHall.LongitudeEnd
                && date > Coordinate.AyaseCityHall.StartDate
                && date < Coordinate.AyaseCityHall.EndDate
                && datum.SensorId == Coordinate.AyaseCityHall.SensorId)
                return true;

            return false;
        }

        private static bool IsYnu(double latitude, double longitude)
        {
            return latitude > Coordinate.Ynu.LatitudeStart && latitude < Coordinate.Ynu.LatitudeEnd && longitude > Coordinate.Ynu.LongitudeStart && longitude < Coordinate.Ynu.LongitudeEnd;
        }

        private static int InsertOutwardTrip(DataTable tripsRawTable, DataTable tripsTable, UserDatum datum, int i)
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

                    row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId());
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));

                    // TODO ログ出力
                    // WriteLog("DRIVER_ID:" + tripRawTable.Rows[i]["DRIVER_ID"] + " CAR_ID:" + tripRawTable.Rows[i]["CAR_ID"] + " SENSOR_ID:" + tripRawTable.Rows[i]["SENSOR_ID"] + "期間:" + tripRawTable.Rows[i]["START_TIME"] + "～" + tripRawTable.Rows[j]["END_TIME"], LogMode.trip);
                    // WriteLog("自宅→横国", LogMode.trip);

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        // TODO ログ出力
                        //WriteLog("別々のトリップを結合する可能性があるので挿入しません", LogMode.trip);
                        //WriteLog("TRIP:トリップ結合でエラー発生", LogMode.error);
                        //errorCount++;
                        break;
                    }
                    else
                    {
                        tripsTable.Rows.Add(row);
                    }

                    tripChangeFlag = true;
                }

                // 自宅 ⇒ 自宅
                else if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude),
                    tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime), datum))
                {
                    // TODO ログ出力
                    // WriteLog("DRIVER_ID:" + tripRawTable.Rows[i]["DRIVER_ID"] + " CAR_ID:" + tripRawTable.Rows[i]["CAR_ID"] + " SENSOR_ID:" + tripRawTable.Rows[i]["SENSOR_ID"] + "期間:" + tripRawTable.Rows[i]["START_TIME"] + "～" + tripRawTable.Rows[j]["END_TIME"], LogMode.trip);
                    // WriteLog("自宅→自宅\r\n", LogMode.trip);

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
                        || IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude)))
                    {
                        tripChangeFlag = true;
                        // TODO ログ出力
                        // WriteLog("自宅→?\r\n", LogMode.trip);
                    }
                }
            }

            return j;
        }

        private static int InsertHomewardTrip(DataTable tripsRawTable, DataTable tripsTable, UserDatum datum, int i)
        {
            int j = i;
            bool tripChangeFlag = false;

            // TripsRaw を結合して Trips を生成するループ
            while (j < tripsRawTable.Rows.Count && tripChangeFlag == false)
            {
                // YNU ⇒ 自宅
                if (IsHome(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude),
                    tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime),
                    datum))
                {
                    var row = tripsTable.NewRow();

                    row.SetField(TripsDao.ColumnTripId, TripsDao.GetMaxTripId());
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[i].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude));

                    // TODO ログ出力
                    // WriteLog("DRIVER_ID:" + tripRawTable.Rows[i]["DRIVER_ID"] + " CAR_ID:" + tripRawTable.Rows[i]["CAR_ID"] + " SENSOR_ID:" + tripRawTable.Rows[i]["SENSOR_ID"] + "期間:" + tripRawTable.Rows[i]["START_TIME"] + "～" + tripRawTable.Rows[j]["END_TIME"], LogMode.trip);
                    // WriteLog("YNU→自宅", LogMode.trip);

                    TimeSpan span = tripsRawTable.Rows[j].Field<DateTime>(TripsRawDao.ColumnEndTime)
                        - tripsRawTable.Rows[i].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        // TODO ログ出力
                        //WriteLog("別々のトリップを結合する可能性があるので挿入しません", LogMode.trip);
                        //WriteLog("TRIP:トリップ結合でエラー発生", LogMode.error);
                        //errorCount++;
                        break;
                    }
                    else
                    {
                        tripsTable.Rows.Add(row);
                    }

                    tripChangeFlag = true;
                }

                // YNU ⇒ YNU
                else if (IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLatitude),
                    tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnEndLongitude)))
                {
                    // TODO ログ出力
                    // WriteLog("DRIVER_ID:" + tripRawTable.Rows[i]["DRIVER_ID"] + " CAR_ID:" + tripRawTable.Rows[i]["CAR_ID"] + " SENSOR_ID:" + tripRawTable.Rows[i]["SENSOR_ID"] + "期間:" + tripRawTable.Rows[i]["START_TIME"] + "～" + tripRawTable.Rows[j]["END_TIME"], LogMode.trip);
                    // WriteLog("自宅→自宅\r\n", LogMode.trip);

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
                        || IsYnu(tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude), tripsRawTable.Rows[j].Field<double>(TripsRawDao.ColumnStartLatitude)))
                    {
                        tripChangeFlag = true;
                        // TODO ログ出力
                        // WriteLog("YNU→?\r\n", LogMode.trip);
                    }
                }
            }

            return j;
        }
    }
}
