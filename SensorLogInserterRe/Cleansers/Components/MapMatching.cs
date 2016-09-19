using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Cleansers.Components
{
    static class MapMatching
    {
        public static DataTable getResultMapMatching(DataTable gpsRawTable, InsertDatum datum)
        {
            DataTable correctedGpsTable = DataTableUtil.GetCorrectedGpsTable();

            List<DataTable> dt = new List<DataTable>();
            if(datum.DriverId == 1)//富井先生用のマップマッチング道路リンクを取得
            {
                int[] id = new int[] { 220 };
                DataTable tempTable = LinkDao.GetLinkTableforMM(id);//復路マップマッチング(富井先生：代官山下ルート)
                tempTable.TableName = "富井先生,復路,代官山下ルート";
                dt.Add(tempTable);

                id = new int[] { 224 };
                tempTable = LinkDao.GetLinkTableforMM(id);//復路マップマッチング(富井先生：代官山上ルート)
                tempTable.TableName = "富井先生,復路,代官山上ルート";
                dt.Add(tempTable);

                id = new int[] { 221 };
                tempTable = LinkDao.GetLinkTableforMM(id);//往路マップマッチング(富井先生：小学校上ルート)
                tempTable.TableName = "富井先生,往路,小学校上ルート";
                dt.Add(tempTable);

                id = new int[] { 225 };
                tempTable = LinkDao.GetLinkTableforMM(id);//往路マップマッチング(富井先生：小学校下ルート)
                tempTable.TableName = "富井先生,往路,小学校下ルート";
                dt.Add(tempTable);
            }
            //TODO マップマッチング処理


            return correctedGpsTable;

        }
        private static void CopyRawDataToCorrectedRow(DataRow correctedRow, DataRow rawRow)
        {
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnDriverId, rawRow.Field<int>(CorrectedGpsDao.ColumnDriverId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnCarId, rawRow.Field<int>(CorrectedGpsDao.ColumnCarId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnSensorId, rawRow.Field<int>(CorrectedGpsDao.ColumnSensorId));
            correctedRow.SetField(CorrectedGpsSpeedLPF005MMDao.ColumnJst, rawRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
        }


        private static double searchNearestLink(DataTable dt, GPSData gps, List<GPSData> GPSArray)
        {
            //近傍リンクの絞込
            string query = "START_LAT > " + (gps.latitude - 0.05);
            query += " AND START_LAT < " + (gps.latitude + 0.05);
            query += " AND START_LONG > " + (gps.longitude - 0.05);
            query += " AND START_LONG < " + (gps.longitude + 0.05);
            DataRow[] dataRows = dt.Select(query);


            double minDist = 255;

            double tempLat = 0;
            double tempLong = 0;

            //各Homewardリンクセグメントに対して
            for (int j = 0; j < dataRows.Length; j++)
            {
                TwoDimensionalVector linkStartEdge = new TwoDimensionalVector((double)dataRows[j]["START_LAT"], (double)dataRows[j]["START_LONG"]);
                TwoDimensionalVector linkEndEdge = new TwoDimensionalVector((double)dataRows[j]["END_LAT"], (double)dataRows[j]["END_LONG"]);
                TwoDimensionalVector GPSPoint = new TwoDimensionalVector(gps.latitude, gps.longitude);

                //線分内の最近傍点を探す
                TwoDimensionalVector matchedPoint = TwoDimensionalVector.nearest(linkStartEdge, linkEndEdge, GPSPoint);

                //最近傍点との距離
                double tempDist = TwoDimensionalVector.distance(GPSPoint, matchedPoint);
                //リンク集合の中での距離最小を探す
                if (tempDist < minDist)
                {
                    minDist = tempDist;
                    tempLat = matchedPoint.x;
                    tempLong = matchedPoint.y;

                }
            }

            GPSData resultGPS = new GPSData(gps.GPSTime, gps.androidTime, tempLat, tempLong, gps.altitude, gps.accuracy);
            GPSArray.Add(resultGPS);

            return minDist;
        }
    }
}
