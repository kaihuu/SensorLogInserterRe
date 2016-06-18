using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;

namespace SensorLogInserterRe.Calculators
{
    class AltitudeCalculator
    {
        public static Tuple<string, double> CalcAltitude(double latitude, double longitude)
        {
            // TODO 実装
            // 探索コストを下げるために、Mesh ID と Altitude をいっぺんに返す

            var registeredMeshTable = Altitude10MMeshRegisteredDao.Get();

            var semanticLinkTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine();
            var linkTable = LinkDao.GetLinkTableWithHeadingAndLine();
            var outwardHighwaySemanticTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine(184);
            var homewardHighwaySemanticTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine(183);

            foreach (DataRow dr in dt.Rows)
            {

                count++;

                //緯度経度取得
                double Lat = double.Parse(dr["LATITUDE"].ToString().Trim());
                double Lng = double.Parse(dr["LONGITUDE"].ToString().Trim());

                DataRow[] selectedRow = null;

                selectedRow = null;


                #region 最短距離計算
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //if (double.Parse(dr["SPEED"].ToString()) >= 60 && Int32.Parse(dr["DRIVER_ID"].ToString()) != 4 && Int32.Parse(dr["DRIVER_ID"].ToString()) != 9)
                if (Int32.Parse(dr["DRIVER_ID"].ToString()) != 4 && Int32.Parse(dr["DRIVER_ID"].ToString()) != 9)
                {
                    if (direction == "outward")
                    {
                        selectedRow = outwardHighwaySemanticTable.AsEnumerable().Where(row => (double)row["LATITUDE"] > (Lat - 0.002) && (double)row["LATITUDE"] < (Lat + 0.002) && (double)row["LONGITUDE"] > (Lng - 0.002) && (double)row["LONGITUDE"] < (Lng + 0.002)).ToArray();
                        if (selectedRow.Length != 0)
                        {
                            dr["LINK_ID"] = LinkMatching(dr, Lat, Lng, selectedRow);
                        }
                    }
                    else if (direction == "homeward")
                    {
                        selectedRow = homewardHighwaySemanticTable.AsEnumerable().Where(row => (double)row["LATITUDE"] > (Lat - 0.002) && (double)row["LATITUDE"] < (Lat + 0.002) && (double)row["LONGITUDE"] > (Lng - 0.002) && (double)row["LONGITUDE"] < (Lng + 0.002)).ToArray();
                        if (selectedRow.Length != 0)
                        {
                            dr["LINK_ID"] = LinkMatching(dr, Lat, Lng, selectedRow);
                        }
                    }
                }

                selectedRow = null;
                selectedRow = semanticLinkTable.AsEnumerable().Where(row => (double)row["LATITUDE"] > (Lat - 0.0001) && (double)row["LATITUDE"] < (Lat + 0.0001) && (double)row["LONGITUDE"] > (Lng - 0.0001) && (double)row["LONGITUDE"] < (Lng + 0.0001)).ToArray();

                if (selectedRow.Length != 0 && dr["LINK_ID"].ToString() == "")
                {
                    dr["LINK_ID"] = LinkMatching(dr, Lat, Lng, selectedRow);
                }


                //セマンティックリンクでマッチングされないとき
                if (dr["LINK_ID"].ToString() == "")
                {
                    selectedRow = null;

                    selectedRow = linkTable.AsEnumerable().Where(row => (double)row["LATITUDE"] > (Lat - 0.002) && (double)row["LATITUDE"] < (Lat + 0.002) && (double)row["LONGITUDE"] > (Lng - 0.002) && (double)row["LONGITUDE"] < (Lng + 0.002)).ToArray();

                    dr["LINK_ID"] = LinkMatching(dr, Lat, Lng, selectedRow);
                }

                dtlink.Clear();

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion

                if (dr["LINK_ID"] != null)
                {
                    DataTable linkDetail = GetLinksDetail(dr["LINK_ID"].ToString());

                    if (linkDetail.Rows.Count == 1)
                    {
                        double linkHeading = double.Parse(linkDetail.Rows[0]["ROAD_HEADING"].ToString());
                        double carHeading = double.Parse(dr["HEADING"].ToString());

                        double angle = Math.Abs(linkHeading - carHeading);

                        if (angle < 90 || angle > 270)
                        {
                            dr["ROAD_THETA"] = linkDetail.Rows[0]["ROAD_THETA"];
                        }
                        else
                        {
                            dr["ROAD_THETA"] = -double.Parse(linkDetail.Rows[0]["ROAD_THETA"].ToString());
                        }
                    }
                }
                else
                {
                    dt.Clear();
                    break;
                }

                #region メッシュ作成
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (!(lowerLatitude <= Lat && upperLatitude > Lat && lowerLongitude <= Lng && upperLongitude > Lng))
                {
                    //メッシュ検索
                    // = registeredTable.Select("LOWER_LATITUDE <= " + Lat + "AND UPPER_LATITUDE > " + Lat + "AND LOWER_LONGITUDE <= " + Lng + "AND UPPER_LONGITUDE > " + Lng);

                    selectedRow = registeredTable.AsEnumerable().Where(row => (double)row["LOWER_LATITUDE"] <= Lat && (double)row["UPPER_LATITUDE"] > Lat && (double)row["LOWER_LONGITUDE"] <= Lng && (double)row["UPPER_LONGITUDE"] > Lng).ToArray();

                    if (selectedRow.Length > 0)  //メッシュ登録済み
                    {
                        foreach (DataRow row in selectedRow)
                        {
                            //地点の地理データの標高＆メッシュIDを取得
                            altitudeNow_TERRAIN = double.Parse(row["ALTITUDE"].ToString());

                            meshID = Int32.Parse(row["MESH_ID"].ToString());

                            lowerLatitude = double.Parse(row["LOWER_LATITUDE"].ToString());
                            lowerLongitude = double.Parse(row["LOWER_LONGITUDE"].ToString());
                            upperLatitude = double.Parse(row["UPPER_LATITUDE"].ToString());
                            upperLongitude = double.Parse(row["UPPER_LONGITUDE"].ToString());
                        }
                    }
                    else  //登録されていないメッシュ
                    {
                        //登録用のIDを取得
                        meshID = GetMaxMeshID();
                        //全標高データを取得

                        double[] aldata = { 0, 0, 0, 0, 0 };
                        aldata = GetAltitude(Lat, Lng);

                        if (aldata[0] == 0)  //標高データが存在しなかった場合
                        {
                            //登録せず標高を-999とする
                            altitudeNow_TERRAIN = -999;
                            meshID = -1;
                        }
                        else
                        {
                            altitudeNow_TERRAIN = aldata[4];  //標高データを取得

                            //標高データを登録
                            //標高データ修正前
                            if (main.CheckInsertBeforeAltitude())
                            {
                                InsertAltitudeRegistered(aldata, meshID);
                            }
                            //標高データ修正テーブル
                            else
                            {
                                InsertAltitudeRegisteredFixed(aldata, meshID);
                            }


                            //データテーブルに新しい標高データを登録
                            DataRow newrow = registeredTable.NewRow();
                            newrow["MESH_ID"] = meshID;
                            newrow["LOWER_LATITUDE"] = aldata[0];
                            newrow["LOWER_LONGITUDE"] = aldata[1];
                            newrow["UPPER_LATITUDE"] = aldata[2];
                            newrow["UPPER_LONGITUDE"] = aldata[3];

                            newrow["ALTITUDE"] = aldata[4];
                            //newrow["ALTITUDE"] = GetAltitudeAverage(Lat, Lng);
                            registeredTable.Rows.Add(newrow);
                        }
                    }
                }

                if (flag == false)
                {
                    dr["TERRAIN_ALTITUDE_DIFFERENCE"] = 0;
                    flag = true;
                }
                else
                {
                    dr["TERRAIN_ALTITUDE_DIFFERENCE"] = altitudeNow_TERRAIN - altitudeBefore_TERRAIN;
                }

                dr["TERRAIN_ALTITUDE"] = altitudeNow_TERRAIN;

                dr["MESH_ID"] = meshID;

                altitudeBefore_TERRAIN = altitudeNow_TERRAIN;

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                #endregion
            }

            return dt;
        }
    }
}
