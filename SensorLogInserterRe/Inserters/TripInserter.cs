using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators;
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
            //Console.WriteLine("GPSSpeed:");
             //Console.WriteLine(gpsRawTable.Rows[0].Field<double?>(AndroidGpsRawDopplerDao.ColumnSpeed));
            
            
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
            else if(correction == InsertConfig.GpsCorrection.MapMatching)
            {
                TripsRawMMDao.Insert(tripsTable);
            }
            else if(correction == InsertConfig.GpsCorrection.DopplerSpeed)
            {
                TripsRawDopplerDao.Insert(tripsTable);
            }
            else if(correction == InsertConfig.GpsCorrection.DopplerNotMM)
            {
                TripsRawDopplerNotMMDao.Insert(tripsTable);
            }
            else {
                TripsRawDao.Insert(tripsTable);
            }
        }

        public static void InsertTrip(InsertDatum datum, InsertConfig.GpsCorrection correction, bool isCheckedSightseeingInsert)
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
            else if (correction == InsertConfig.GpsCorrection.Normal)
            {
                tripsRawTable = TripsRawDao.Get(datum);
                TripsDao.DeleteTrips(); //途中中断された際に作成したトリップを削除
            }
            else if (correction == InsertConfig.GpsCorrection.DopplerSpeed)
            {
                tripsRawTable = TripsRawDopplerDao.Get(datum);
                TripsDopplerDao.DeleteTrips(); //途中中断された際に作成したトリップを削除
            }
            else if (correction == InsertConfig.GpsCorrection.DopplerNotMM)
            {
                tripsRawTable = TripsRawDopplerNotMMDao.Get(datum);
                TripsDopplerNotMMDao.DeleteTrips();
            }


                LogWritter.WriteLog(LogWritter.LogMode.Trip, $"挿入対象のRAWデータ: {tripsRawTable.Rows.Count}");

            
            for (int i = 0; i < tripsRawTable.Rows.Count; i++)
            {
                DataTable tripsTable = DataTableUtil.GetTripsTable();


                // 観光オプションによるインサート処理はあらかじめ切り分ける。
                if (isCheckedSightseeingInsert)
                {
                    InsertSightSeeingTrip(tripsRawTable, tripsTable, datum, i, correction);
                }

                // 自宅出発
                else if (IsHome(tripsRawTable.Rows[i].Field<double>(TripsRawDao.ColumnStartLatitude),
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

                // 1トリップごとなので主キー違反があっても挿入されないだけ
                if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
                {
                    TripsSpeedLPF005MMDao.Insert(tripsTable);
                }
                else if (correction == InsertConfig.GpsCorrection.MapMatching)
                {
                    TripsMMDao.Insert(tripsTable);
                }
                else if(correction == InsertConfig.GpsCorrection.Normal){
                    TripsDao.Insert(tripsTable);
                }
                else if(correction == InsertConfig.GpsCorrection.DopplerSpeed)
                {
                    if (tripsTable.Rows.Count != 0)
                    {
                        var gpsCorrectedTable = CorrectedGPSDopplerDao.GetNormalized(
                            tripsTable.Rows[0].Field<DateTime>(TripsDopplerDao.ColumnStartTime),
                            tripsTable.Rows[0].Field<DateTime>(TripsDopplerDao.ColumnEndTime),
                            datum);
                        if (gpsCorrectedTable.Rows.Count != 0)
                        {
                                TripsDopplerDao.Insert(tripsTable);
                            
                        }
                    }
                }
                else if(correction == InsertConfig.GpsCorrection.DopplerNotMM)
                {
                    if (tripsTable.Rows.Count != 0)
                    {
                        var gpsCorrectedTable = CorrectedGpsDopplerNotMMDao.GetNormalized(
                            tripsTable.Rows[0].Field<DateTime>(TripsDopplerDao.ColumnStartTime),
                            tripsTable.Rows[0].Field<DateTime>(TripsDopplerDao.ColumnEndTime),
                            datum);
                        if (gpsCorrectedTable.Rows.Count != 0)
                        {
                            TripsDopplerNotMMDao.Insert(tripsTable);

                        }
                    }
                }
                
            }
        }

        private static bool IsHome(double latitude, double longitude, DateTime date, InsertDatum datum)
        {
            DataTable dataTable = PlaceGetter.GetInstance().getDataTable();
            DataRow[] dataRows = dataTable.Select("property = 'home'");

            for(int i = 0; i < dataRows.Length; i++)
            {
                if(dataRows[i][PlaceDao.ColumnStartDate] == DBNull.Value && dataRows[i][PlaceDao.ColumnEndDate] == DBNull.Value)
                {
                    if (latitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLatitude)
                        && latitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLatitude)
                        && longitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLongitude)
                        && longitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLongitude))
                        return true;
                }
                else if (dataRows[i][PlaceDao.ColumnStartDate] != DBNull.Value && dataRows[i][PlaceDao.ColumnEndDate] == DBNull.Value)
                {
                    if (latitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLatitude)
                        && latitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLatitude)
                        && longitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLongitude)
                        && longitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLongitude)
                        && date > dataRows[i].Field<DateTime>(PlaceDao.ColumnStartDate))
                        return true;
                }
                else if (dataRows[i][PlaceDao.ColumnStartDate] == DBNull.Value && dataRows[i][PlaceDao.ColumnEndDate] != DBNull.Value)
                {
                    if (latitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLatitude)
                        && latitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLatitude)
                        && longitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLongitude)
                        && longitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLongitude)
                        && date < dataRows[i].Field<DateTime>(PlaceDao.ColumnEndDate))
                        return true;
                }
                else
                {
                    if (latitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLatitude)
                        && latitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLatitude)
                        && longitude > dataRows[i].Field<double>(PlaceDao.ColumnStartLongitude)
                        && longitude < dataRows[i].Field<double>(PlaceDao.ColumnEndLongitude)
                        && date > dataRows[i].Field<DateTime>(PlaceDao.ColumnStartDate)
                        && date < dataRows[i].Field<DateTime>(PlaceDao.ColumnEndDate))
                        return true;
                }

            }

            return false;
        }

        private static bool IsYnu(double latitude, double longitude)
        {
            GeoCoordinate startCoordinate = PlaceGetter.GetInstance().Get("YNU").Item1;
            GeoCoordinate endCoordinate = PlaceGetter.GetInstance().Get("YNU").Item2;
            return latitude > startCoordinate.Latitude && latitude < endCoordinate.Latitude && longitude > startCoordinate.Longitude && longitude < endCoordinate.Longitude;
        }


        /*  
         *  IsSightseeing()
         *  2018leaf実験用に追加した観光オプション判定用のメソッド
         *  引数で与えられた座標がPLACE.PROPERTY==sightseeingとなるようなレコード群に含まれるかを判定する。
         */
        private static bool IsSightseeing(double latitude, double longitude)
        {
            GeoCoordinate startCoordinate;
            GeoCoordinate endCoordinate;

            // property値がsightseeingである範囲群を取得
            List<Tuple<GeoCoordinate, GeoCoordinate>> sightseeingPlaces = PlaceGetter.GetInstance().GetRowsByProperty("sightseeing");

            // 引数で与えられた座標が範囲群の中の1つの範囲内に存在する場合はTrue
            foreach(Tuple<GeoCoordinate, GeoCoordinate> sightseeingPlace in sightseeingPlaces)
            {
                startCoordinate = sightseeingPlace.Item1;
                endCoordinate = sightseeingPlace.Item2;

                if (latitude > startCoordinate.Latitude
                    && latitude < endCoordinate.Latitude
                    && longitude > startCoordinate.Longitude
                    && longitude < endCoordinate.Longitude)
                {
                    return true;
                }
            }

            // ここに到達した場合には、引数で与えられた座標はsightseeingプロパティを持つ場所群に含まれない
            return false;
        }

        private static int GetMaxTripId(InsertConfig.GpsCorrection correction)
        {
            int tripid = -1;
            if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching)
            {
                tripid = TripsSpeedLPF005MMDao.GetMaxTripId() + 1;
            }
            else if (correction == InsertConfig.GpsCorrection.MapMatching)
            {
                tripid = TripsMMDao.GetMaxTripId() + 1;
            }
            else if(correction == InsertConfig.GpsCorrection.Normal)
            {
                tripid = TripsDao.GetMaxTripId() + 1;
            }
            else if(correction == InsertConfig.GpsCorrection.DopplerSpeed)
            {
                tripid = TripsDopplerDao.GetMaxTripId() + 1;
            }
            else if(correction == InsertConfig.GpsCorrection.DopplerNotMM)
            {
                tripid = TripsDopplerNotMMDao.GetMaxTripId() + 1;
            }
            return tripid;
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


                    row.SetField(TripsDao.ColumnTripId, GetMaxTripId(correction));

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
                    else if (correction == InsertConfig.GpsCorrection.DopplerSpeed && !TripsDopplerDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.DopplerNotMM && !TripsDopplerNotMMDao.IsExsistsTrip(row))
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
                    row.SetField(TripsDao.ColumnTripId, GetMaxTripId(correction));
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
                    else if (correction == InsertConfig.GpsCorrection.DopplerSpeed && !TripsDopplerDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.DopplerNotMM && !TripsDopplerNotMMDao.IsExsistsTrip(row))
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

        /*  
         *  InsertSightSeeingTrip()
         *  観光用トリップを挿入するためのメソッド
         *  YNU出発と観光地出発の2種類があることに注意が必要になる。
         */
        private static void InsertSightSeeingTrip(DataTable tripsRawTable, DataTable tripsTable, InsertDatum datum, int startIndex, InsertConfig.GpsCorrection correction)
        {
            // InsertHomewardTripと同じような処理を記述
            // tripのスタートのtripsRawのインデックスをstartIndex
            // 現在注目しているtripsRawのインデックスをcurrentIndex
            int currentIndex = startIndex;

            // tripChangeFlagはループを抜けるためのフラグ
            // このフラグが立つと、異なるトリップに到達したことを示す
            bool tripChangeFlag = false;

            // TripsRawを結合してTripsを生成する。
            while (currentIndex < tripsRawTable.Rows.Count && tripChangeFlag == false)
            {
                // スタートがynuか観光地 かつ ゴールがynuか観光地
                // でもynuからynuのトリップは考えない

                // スタート,ゴールがynuであるか
                bool isStartYnu = IsYnu(tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLatitude),
                                        tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLongitude));
                bool isEndYnu = IsYnu(tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLatitude),
                                      tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLongitude));
                // スタートゴールが観光地であるか
                bool isStartSightseeingSpot = IsSightseeing(tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLatitude),
                                                            tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLongitude));
                bool isEndSightseeingSpot = IsSightseeing(tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLatitude),
                                                          tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLongitude));
                
                // そもそもスタートがynuでも観光地でもない場合
                if (!(isStartSightseeingSpot || isStartYnu)) {
                    tripChangeFlag = true;
                }
                // YNU発YNU着はない
                else if (isStartYnu && isEndYnu)
                {
                    LogWritter.WriteLog(LogWritter.LogMode.Trip, "YNU⇒YNUトリップなので挿入しません。"
                                                                 + ConvertRowToString(tripsRawTable.Rows[startIndex], 
                                                                                      tripsRawTable.Rows[currentIndex]));
                    tripChangeFlag = true;
                }
                // YNUまたは観光地が出発地AND到着地
                else if ( (isStartYnu || isStartSightseeingSpot)
                       && (isEndYnu || isEndSightseeingSpot) )
                {
                    var row = tripsTable.NewRow();
                    row.SetField(TripsDao.ColumnTripId, GetMaxTripId(correction));
                    row.SetField(TripsDao.ColumnDriverId, tripsRawTable.Rows[startIndex].Field<int>(TripsRawDao.ColumnDriverId));
                    row.SetField(TripsDao.ColumnCarId, tripsRawTable.Rows[startIndex].Field<int>(TripsRawDao.ColumnCarId));
                    row.SetField(TripsDao.ColumnSensorId, tripsRawTable.Rows[startIndex].Field<int>(TripsRawDao.ColumnSensorId));
                    row.SetField(TripsDao.ColumnStartTime, tripsRawTable.Rows[startIndex].Field<DateTime>(TripsRawDao.ColumnStartTime));
                    row.SetField(TripsDao.ColumnEndTime, tripsRawTable.Rows[currentIndex].Field<DateTime>(TripsRawDao.ColumnEndTime));
                    row.SetField(TripsDao.ColumnStartLatitude, tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLatitude));
                    row.SetField(TripsDao.ColumnStartLongitude, tripsRawTable.Rows[startIndex].Field<double>(TripsRawDao.ColumnStartLongitude));
                    row.SetField(TripsDao.ColumnEndLatitude, tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLatitude));
                    row.SetField(TripsDao.ColumnEndLongitude, tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnEndLongitude));
                    row.SetField(TripsDao.ColumnConsumedEnergy, DBNull.Value);
                    row.SetField(TripsDao.ColumnTripDirection, "tourism");
                    row.SetField(TripsDao.ColumnValidation, DBNull.Value);

                    TimeSpan span = tripsRawTable.Rows[currentIndex].Field<DateTime>(TripsRawDao.ColumnEndTime)
                                    - tripsRawTable.Rows[startIndex].Field<DateTime>(TripsRawDao.ColumnStartTime);

                    if (span.TotalHours > 12)
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "別々のトリップを結合する可能性があるので挿入しません "
                                                                     + ConvertRowToString(tripsRawTable.Rows[startIndex],
                                                                                          tripsRawTable.Rows[currentIndex]));
                        break;
                    }

                    if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching
                        && !TripsSpeedLPF005MMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.MapMatching
                             && !TripsMMDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else if (correction == InsertConfig.GpsCorrection.Normal
                             && !TripsDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    } 
                    else if (correction == InsertConfig.GpsCorrection.DopplerSpeed
                             && !TripsDopplerDao.IsExsistsTrip(row))
                    {
                        tripsTable.Rows.Add(row);
                    }
                    else
                    {
                        LogWritter.WriteLog(LogWritter.LogMode.Trip, "既にこのトリップは挿入されているので挿入しません "
                                                                     + ConvertRowToString(tripsRawTable.Rows[startIndex],
                                                                                          tripsRawTable.Rows[currentIndex]));
                    }

                    tripChangeFlag = true;
                }

                currentIndex++;
                // IndexOutOfBoundsを防止
                if(currentIndex >= tripsRawTable.Rows.Count)
                {
                    return;
                }

                // YNUにも観光地にも到着しないまま、開始地点がYNUか観光地になった場合
                if (IsYnu(tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnStartLatitude),
                          tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnStartLongitude))
                    || IsSightseeing(tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnStartLongitude),
                                     tripsRawTable.Rows[currentIndex].Field<double>(TripsRawDao.ColumnStartLongitude))
                   )
                {
                    tripChangeFlag = true;
                    LogWritter.WriteLog(LogWritter.LogMode.Trip, "YNU⇒？トリップなので挿入しません "
                              + ConvertRowToString(tripsRawTable.Rows[startIndex], tripsRawTable.Rows[currentIndex]));
                }
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
