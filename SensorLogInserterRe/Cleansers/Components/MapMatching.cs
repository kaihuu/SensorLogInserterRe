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
            if(gpsRawTable.Rows.Count == 0)
            {
                return new DataTable();
            }

            List<DataTable> dt = new List<DataTable>();
            if(datum.DriverId == 1)
            {
                int[] id = new int[] { 220 };
                DataTable tempTable = LinkDao.GetLinkTableforMM(id);//復路マップマッチング(代官山下ルート)
                tempTable.TableName = "復路,代官山下ルート";
                dt.Add(tempTable);
                dt[0].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 224 };
                tempTable = LinkDao.GetLinkTableforMM(id);//復路マップマッチング(代官山上ルート)
                tempTable.TableName = "復路,代官山上ルート";
                dt.Add(tempTable);
                dt[1].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 221 };
                tempTable = LinkDao.GetLinkTableforMM(id);//往路マップマッチング(小学校上ルート)
                tempTable.TableName = "往路,小学校上ルート";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 225 };
                tempTable = LinkDao.GetLinkTableforMM(id);//往路マップマッチング(小学校下ルート)
                tempTable.TableName = "往路,小学校下ルート";
                dt.Add(tempTable);
                dt[3].DefaultView.Sort = "START_LAT, " +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "START_LONG";
            }
            else if (datum.DriverId == 17)//マップマッチング道路リンクを取得
            {
                int[] id = new int[] { 328 };
                DataTable tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17復路ルート１";
                dt.Add(tempTable);
                dt[0].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 329 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート1";
                dt.Add(tempTable);
                dt[1].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 330 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート2";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 331 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17復路ルート2";
                dt.Add(tempTable);
                dt[3].DefaultView.Sort = "START_LAT, " +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "START_LONG";
            }
            //TODO マップマッチング処理
            double[] sumDist = new double[dt.Count];//GPS点をマッチングさせるのに移動させた距離の総和
            double[] maxDist = new double[dt.Count];//GPS点をマッチングさせるのに移動させた距離の最大値
            DataTable[] mapMatchedGpsTable = DataTableUtil.GetAndroidGpsRawTableArray(dt.Count);
                for (int i = 0; i < gpsRawTable.Rows.Count; i++)
                {

                    for (int n = 0; n < dt.Count; n++)
                    {
                        double tempDist = searchNearestLink(dt[n], gpsRawTable.Rows[i], ref mapMatchedGpsTable[n]);
                        sumDist[n] += tempDist;

                        if (tempDist > maxDist[n]) maxDist[n] = tempDist;
                    }
                }
                int element = getMinElement(sumDist);
                if(sumDist.Length == 0)
                {
                    return new DataTable();
                }
                if (sumDist[element] > 0.5 || maxDist[element] > 0.003)
                {
                    return new DataTable();
                }


            return mapMatchedGpsTable[element];

        }

        public static DataTable getResultMapMatchingDoppler(DataTable gpsRawTable, InsertDatum datum)
        {
            if (gpsRawTable.Rows.Count == 0)
            {
                return new DataTable();
            }

            List<DataTable> dt = new List<DataTable>();
            if (datum.DriverId == 1)//被験者１用のマップマッチング道路リンクを取得
            {
                int[] id = new int[] { 220 };
                DataTable tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "復路,代官山下ルート";
                dt.Add(tempTable);
                dt[0].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 224 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "復路,代官山上ルート";
                dt.Add(tempTable);
                dt[1].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 221 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "往路,小学校上ルート";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 225 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "往路,小学校下ルート";
                dt.Add(tempTable);
                dt[3].DefaultView.Sort = "START_LAT, " +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "START_LONG";
            }
            else if (datum.DriverId == 17)//マップマッチング道路リンクを取得
            {
                int[] id = new int[] { 328 };
                DataTable tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17復路ルート１";
                dt.Add(tempTable);
                dt[0].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 329 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート1";
                dt.Add(tempTable);
                dt[1].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 330 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート2";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 331 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート2";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 340 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17往路ルート3";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";

                id = new int[] { 341 };
                tempTable = LinkDao.GetLinkTableforMM(id);
                tempTable.TableName = "被験者17復路ルート3";
                dt.Add(tempTable);
                dt[2].DefaultView.Sort = "START_LAT, START_LONG";
            }
            //TODO マップマッチング処理
            double[] sumDist = new double[dt.Count];//GPS点をマッチングさせるのに移動させた距離の総和
            double[] maxDist = new double[dt.Count];//GPS点をマッチングさせるのに移動させた距離の最大値
            DataTable[] mapMatchedGpsTable = DataTableUtil.GetAndroidGpsRawDopplerTableArray(dt.Count);
            for (int i = 0; i < gpsRawTable.Rows.Count; i++)
            {

                for (int n = 0; n < dt.Count; n++)
                {
                    double tempDist = searchNearestLinkDoppler(dt[n], gpsRawTable.Rows[i], ref mapMatchedGpsTable[n]);
                    sumDist[n] += tempDist;

                    if (tempDist > maxDist[n]) maxDist[n] = tempDist;
                }
            }
            int element = getMinElement(sumDist);
            if (sumDist.Length == 0)
            {
                SlackUtil.noMapMatching(datum, gpsRawTable.Rows[0]);
                return new DataTable();
            }
            if (sumDist[element] > 0.5 || maxDist[element] > 0.003)
            {
                SlackUtil.noMapMatching(datum, gpsRawTable.Rows[0]);
                return new DataTable();
            }


            return mapMatchedGpsTable[element];

        }
        private static void CopyRawDataToCorrectedRow(DataRow correctedRow, DataRow rawRow)
        {
            correctedRow.SetField(AndroidGpsRawDao.ColumnDriverId, rawRow.Field<int>(AndroidGpsRawDao.ColumnDriverId));
            correctedRow.SetField(AndroidGpsRawDao.ColumnCarId, rawRow.Field<int>(AndroidGpsRawDao.ColumnCarId));
            correctedRow.SetField(AndroidGpsRawDao.ColumnSensorId, rawRow.Field<int>(AndroidGpsRawDao.ColumnSensorId));
            correctedRow.SetField(AndroidGpsRawDao.ColumnJst, rawRow.Field<DateTime>(AndroidGpsRawDao.ColumnJst));
            correctedRow.SetField(AndroidGpsRawDao.ColumnAltitude, rawRow.Field<double>(AndroidGpsRawDao.ColumnAltitude));
            correctedRow.SetField(AndroidGpsRawDao.ColumnAndroidTime, rawRow.Field<DateTime>(AndroidGpsRawDao.ColumnAndroidTime));
        }

        private static void CopyRawDataToCorrectedRowDoppler(DataRow correctedRow, DataRow rawRow)
        {
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnDriverId, rawRow.Field<int>(AndroidGpsRawDopplerDao.ColumnDriverId));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnCarId, rawRow.Field<int>(AndroidGpsRawDopplerDao.ColumnCarId));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnSensorId, rawRow.Field<int>(AndroidGpsRawDopplerDao.ColumnSensorId));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnJst, rawRow.Field<DateTime>(AndroidGpsRawDopplerDao.ColumnJst));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnAltitude, rawRow.Field<double>(AndroidGpsRawDopplerDao.ColumnAltitude));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnAndroidTime, rawRow.Field<DateTime>(AndroidGpsRawDopplerDao.ColumnAndroidTime));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnAccuracy, rawRow.Field<int>(AndroidGpsRawDopplerDao.ColumnAccuracy));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnSpeed, rawRow.Field<double?>(AndroidGpsRawDopplerDao.ColumnSpeed));
            correctedRow.SetField(AndroidGpsRawDopplerDao.ColumnBearing, rawRow.Field<double?>(AndroidGpsRawDopplerDao.ColumnBearing));

        }

        private static int getMinElement(double[] colle)
        {
            int temp = 0;



            for (int i = 1; i < colle.Length; i++)
            {
                if (colle[temp] > colle[i])
                    temp = i;
            }
            return temp;
        }
        private static double searchNearestLink(DataTable dt, DataRow gps, ref DataTable result)
        {
            //近傍リンクの絞込
            string query = "START_LAT > " + (gps.Field<double>(AndroidGpsRawDao.ColumnLatitude) - 0.05);
            query += " AND START_LAT < " + (gps.Field<double>(AndroidGpsRawDao.ColumnLatitude) + 0.05);
            query += " AND START_LONG > " + (gps.Field<double>(AndroidGpsRawDao.ColumnLongitude) - 0.05);
            query += " AND START_LONG < " + (gps.Field<double>(AndroidGpsRawDao.ColumnLongitude) + 0.05);
            DataRow[] dataRows = dt.Select(query);


            double minDist = 255;

            double tempLat = 0;
            double tempLong = 0;

            //各Homewardリンクセグメントに対して
            for (int j = 0; j < dataRows.Length; j++)
            {
                TwoDimensionalVector linkStartEdge = new TwoDimensionalVector((double)dataRows[j]["START_LAT"], (double)dataRows[j]["START_LONG"]);
                TwoDimensionalVector linkEndEdge = new TwoDimensionalVector((double)dataRows[j]["END_LAT"], (double)dataRows[j]["END_LONG"]);
                TwoDimensionalVector GPSPoint = new TwoDimensionalVector(gps.Field<double>(AndroidGpsRawDao.ColumnLatitude), gps.Field<double>(AndroidGpsRawDao.ColumnLongitude));

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

            DataRow row = result.NewRow();

            CopyRawDataToCorrectedRow(row, gps);

            row.SetField(AndroidGpsRawDao.ColumnLatitude, tempLat);
            row.SetField(AndroidGpsRawDao.ColumnLongitude, tempLong);

            result.Rows.Add(row);

            return minDist;
        }

        private static double searchNearestLinkDoppler(DataTable dt, DataRow gps, ref DataTable result)
        {
            //近傍リンクの絞込
            string query = "START_LAT > " + (gps.Field<double>(AndroidGpsRawDao.ColumnLatitude) - 0.05);
            query += " AND START_LAT < " + (gps.Field<double>(AndroidGpsRawDao.ColumnLatitude) + 0.05);
            query += " AND START_LONG > " + (gps.Field<double>(AndroidGpsRawDao.ColumnLongitude) - 0.05);
            query += " AND START_LONG < " + (gps.Field<double>(AndroidGpsRawDao.ColumnLongitude) + 0.05);
            DataRow[] dataRows = dt.Select(query);


            double minDist = 255;

            double tempLat = 0;
            double tempLong = 0;

            //各Homewardリンクセグメントに対して
            for (int j = 0; j < dataRows.Length; j++)
            {
                TwoDimensionalVector linkStartEdge = new TwoDimensionalVector((double)dataRows[j]["START_LAT"], (double)dataRows[j]["START_LONG"]);
                TwoDimensionalVector linkEndEdge = new TwoDimensionalVector((double)dataRows[j]["END_LAT"], (double)dataRows[j]["END_LONG"]);
                TwoDimensionalVector GPSPoint = new TwoDimensionalVector(gps.Field<double>(AndroidGpsRawDao.ColumnLatitude), gps.Field<double>(AndroidGpsRawDao.ColumnLongitude));

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

            DataRow row = result.NewRow();

            CopyRawDataToCorrectedRowDoppler(row, gps);

            row.SetField(AndroidGpsRawDopplerDao.ColumnLatitude, tempLat);
            row.SetField(AndroidGpsRawDopplerDao.ColumnLongitude, tempLong);

            result.Rows.Add(row);

            return minDist;
        }
    }
}
