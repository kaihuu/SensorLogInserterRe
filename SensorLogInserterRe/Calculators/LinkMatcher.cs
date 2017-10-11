using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Calculators
{
    class LinkMatcher
    {
        private static LinkMatcher _instance;
        private DataTable _semanticLinkTable;
        private DataTable _linkTable;
        private DataTable _outwardHighwaySemanticLinkTable;
        private DataTable _homewardHighwaySemanticLinkTable;

        private static readonly int OutwardHighwaySemanticLinkId = 184;
        private static readonly int HomewardHighwaySemanticLinkId = 183;

        public static LinkMatcher GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LinkMatcher
                {
                    _semanticLinkTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine(),
               //     _linkTable = LinkDao.GetLinkTableWithHeadingAndLine(),
                    _outwardHighwaySemanticLinkTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine(OutwardHighwaySemanticLinkId),
                    _homewardHighwaySemanticLinkTable = SemanticLinkDao.GetSemanticLinkTableWithHeadingAndLine(HomewardHighwaySemanticLinkId)
                };
            }

          //  _instance._linkTable.DefaultView.Sort = "latitude, longitude";

            return _instance;
        }

        private LinkMatcher()
        {
            // for singleton
        }

        public Tuple<string, double?> MatchLink(double latitude, double longitude, Single heading, string direction,
            InsertDatum datum)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            // 探索コスト削減のため、Link ID と 道路勾配をいっぺんに返す
            int multiple = 10000; //緯度経度整数に変換用
            string linkId = null;
            double? roadTheta = null;
            DataRow[] selectedRows = null;
            int intlatitude =Convert.ToInt32(latitude * multiple);
            int intlongitude = Convert.ToInt32(longitude * multiple);

            #region リンクマッチング
            // TODO 以下のアルゴリズムを見直すとけっこうな高速化が計れそう
            // TODO 今すごくいい案思いついた、しかもデータベース屋っぽい案
            // TODO latitude, longitudeを引数とするテーブル関数定義して、
            // TODO 中身はじつは緯度、経度からリンクを返すルックアップテーブル
            // TODO これはけっこうクール

            if (datum.DriverId != 4 && datum.DriverId != 9)
           {
                if (direction == "outward")
                {
                    selectedRows = _outwardHighwaySemanticLinkTable
                        .AsEnumerable()
                        .Where(row => Math.Abs(row.Field<double>(2) - latitude) < 0.002 && Math.Abs(row.Field<double>(3) - longitude) < 0.002)
                        .ToArray();

                    if (selectedRows.Length != 0)
                        linkId = SelectLink(latitude, longitude, heading, selectedRows);

                }
                else if (direction == "homeward")
                {
                    selectedRows = _homewardHighwaySemanticLinkTable
                    .AsEnumerable()
                    .Where(row => Math.Abs(row.Field<double>(2) - latitude) < 0.002 && Math.Abs(row.Field<double>(3) - longitude) < 0.002)
                    .ToArray();

                    if (selectedRows.Length != 0)
                        linkId = SelectLink(latitude, longitude, heading, selectedRows);
                }

                else if (direction == "other")
                {
                    return new Tuple<string, double?>(null, null);
                }


                

                if (selectedRows == null || selectedRows.Length == 0)
                {
                    selectedRows = _semanticLinkTable
                        .AsEnumerable()
                        .Where(row => Math.Abs(row.Field<double>(2) - latitude) < 0.0001 && Math.Abs(row.Field<double>(3) - longitude) < 0.0001)
                        .ToArray();

                    if (selectedRows.Length != 0)
                        linkId = SelectLink(latitude, longitude, heading, selectedRows);
                }

                //if (selectedRows.Length == 0)
                //{
                //    selectedRows = _linkTable
                //        .AsEnumerable()
                //        .Where(row => Math.Abs(row.Field<double>(1) - latitude) < 0.002 && Math.Abs(row.Field<double>(2) - longitude) < 0.002)
                //        .ToArray();

                //    if (selectedRows.Length != 0)
                //        linkId = SelectLink(latitude, longitude, heading, selectedRows);
                //}
                if(selectedRows.Length == 0)
                {
                    selectedRows = LinksForSearchDao.GetLinkId(intlatitude, intlongitude).Select();
                    if (selectedRows.Length != 0)
                    {
                        linkId = SelectLink(latitude, longitude, heading, selectedRows);
                    }

                }
            }
            #endregion          

            if (linkId != null)
            {
                var linkDetail = LinkDetailDao.Get(linkId);

                if (linkDetail.Rows.Count == 1)
                {
                    Single linkHeading = linkDetail.Rows[0].Field<Single>("road_heading");
                    Single carHeading = heading;

                    double angle = Math.Abs(linkHeading - carHeading);

                    if (angle < 90 || angle > 270)
                    {
                        roadTheta = linkDetail.Rows[0].Field<Single>("road_theta");
                    }
                    else
                    {
                        roadTheta = -linkDetail.Rows[0].Field<Single>("road_theta");
                    }
                }
            }
            //sw.Stop();
            //LogWritter.WriteLog(LogWritter.LogMode.Elapsedtime, "One Link Match Calculaton Time:" + sw.Elapsed);
            return new Tuple<string, double?>(linkId, roadTheta);
        }

        private string SelectLink(double latitude, double longitude, double heading, DataRow[] selectedRows)
        {
            string matchedLink = "";
            double minDistance = double.PositiveInfinity;

            //車のHEADINGを計算
            double carHeading;
            if (heading > 180)
                carHeading = heading - 180;
            else
                carHeading = heading;

            //自動車の向きとリンクの向きのなす角
            double minAngle = double.PositiveInfinity;

            foreach (DataRow row in selectedRows)
            {
                double pointY;//リンク上のGPSの点との最近点の緯度
                double pointX;//リンク上のGPSの点との最近点の経度

                //リンクとGPS上の点との距離とリンク上の最近点を計算
                CalculateMinimumDistancePointOfLink(latitude, longitude, row, out pointY, out pointX);

                //リンクとの最短距離
                double distance = DistanceCalculator.CalcDistance(latitude, longitude, pointY, pointX);

                if (row.Field<double?>("heading") != null)
                {

                    double linkHeading = row.Field<double>("heading");
                    //リンクとHEADINGの角度を算出
                    double angle = Math.Abs(carHeading - linkHeading);

                    if (angle > 90)
                    {
                        angle = 180 - angle;
                    }

                    //リンクとの距離が10m以内でかつなす角が小さいものをマッチング
                    if (distance < 10 && angle < minAngle)
                    {
                        minDistance = distance;
                        minAngle = angle;
                        matchedLink = row.Field<string>("link_id").Trim();
                    }
                    else
                    {
                        //10m以内のリンクがないときは距離が短いものをマッチング
                        if (minDistance >= distance)
                        {
                            minDistance = distance;
                            matchedLink = row.Field<string>("link_id").Trim();
                        }
                    }
                }
                else
                {
                    //Headingが計算できないときは距離が短いものをマッチング
                    if (minDistance >= distance)
                    {
                        minDistance = distance;

                        matchedLink = row.Field<string>("link_id").Trim();
                    }
                }
            }

            return matchedLink;
        }

        private static void CalculateMinimumDistancePointOfLink(double latitude, double longitude, DataRow row, out double pointY, out double pointX)
        {
            try
            {

                double ay = double.Parse(row["LAT1"].ToString());//リンクの始点の緯度  リンクの始点を点A、終点を点B、GPSの点を点Pとすると
                double ax = double.Parse(row["LON1"].ToString());//リンクの始点の経度  点A=(Ax,Ay),点B=(Bx,By),点P=(Lng,Lat)と定義される
                double by = double.Parse(row["LAT2"].ToString());//リンクの終点の緯度
                double bx = double.Parse(row["LON2"].ToString());//リンクの終点の経度

                double ax2 = bx - ax;//経度の差  ベクトルAB=(ax2,ay2),ベクトルAP=(bx2,by2)
                double ay2 = by - ay;//緯度の差
                double bx2 = longitude - ax;//GPSの点とリンクの始点との経度の差
                double by2 = latitude - ay;//GPSの点とリンクの始点との緯度の差

                double r = (ax2 * bx2 + ay2 * by2) / (ax2 * ax2 + ay2 * ay2);//r=(ベクトルABとベクトルAPとの内積)/(ABの距離の2乗)=(|AP|cosθ)/|AB|

                if (r <= 0)//r<=0のとき点Pから線分ABへの垂線はA側の線分外の点になるため最近点は点A
                {
                    pointX = ax;
                    pointY = ay;
                }
                else if (r >= 1)//r>=1のとき点Pから線分ABへの垂線はB側の線分外の点になるため最近点は点B
                {
                    pointX = bx;
                    pointY = by;
                }
                else//0<r<1のとき点Pから線分ABへの垂線はAとBの間に存在するため最近点を計算する
                {
                    pointX = ax + r * ax2;
                    pointY = ay + r * ay2;
                }
            }
            catch (FormatException)
            {
                pointX = double.Parse(row["LONGITUDE"].ToString());
                pointY = double.Parse(row["LATITUDE"].ToString());
            }
        }

    }   
}
