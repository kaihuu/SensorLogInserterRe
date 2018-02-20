using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOLOGCarSimulator.Daos
{
    class LinkDataGetterForMM
    {
        public static DataTable LinkTableGetter(int[] id)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver3;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //没クエリ
            //string query = "select l1.LINK_ID as LINK_ID ,l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            //query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS ";
            //query += "where l1.NUM = l2.NUM - 1 ";
            //query += "and l1.LINK_ID = l2.LINK_ID ";
            //query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            //query += "and SEMANTIC_LINK_ID in ( " + id[0];

            //for (int i = 1; i < id.Length; i++)
            //{
            //    query += ", " + id;
            //}

            //query += ") ";
            //query += "order by l1.NUM ";


            string query = "select l1.LINK_ID as LINK_ID , l1.NUM, l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            query += ",SQRT((l1.LATITUDE - l2.LATITUDE) * (l1.LATITUDE - l2.LATITUDE) + (l1.LONGITUDE - l2.LONGITUDE) * (l1.LONGITUDE - l2.LONGITUDE)) as DISTANCE  ";
            query += "from LINKS as l1,LINKS as l2,( ";
            query += "select l1.NUM,MIN(l2.NUM - l1.NUM) as diff ";
            query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS  ";
            query += "where l1.NUM < l2.NUM  ";
            query += "and l1.LINK_ID = l2.LINK_ID  ";
            query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID  ";
            query += "and SEMANTIC_LINK_ID in ( " + id[0];

            for (int i = 1; i < id.Length; i++)
            {
                query += ", " + id;
            }

            query += ") ";
            query += "group by l1.NUM) as Corres ";
            query += "where l1.NUM = Corres.NUM ";
            query += "and l2.NUM = l1.NUM + Corres.diff ";
            query += "order by NUM ";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable LinkTableGetterforvMM(int[] id)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //没クエリ
            //string query = "select l1.LINK_ID as LINK_ID ,l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            //query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS ";
            //query += "where l1.NUM = l2.NUM - 1 ";
            //query += "and l1.LINK_ID = l2.LINK_ID ";
            //query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            //query += "and SEMANTIC_LINK_ID in ( " + id[0];

            //for (int i = 1; i < id.Length; i++)
            //{
            //    query += ", " + id;
            //}

            //query += ") ";
            //query += "order by l1.NUM ";


            string query = "select l1.LINK_ID as LINK_ID , l1.NUM, l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            query += ",SQRT((l1.LATITUDE - l2.LATITUDE) * (l1.LATITUDE - l2.LATITUDE) + (l1.LONGITUDE - l2.LONGITUDE) * (l1.LONGITUDE - l2.LONGITUDE)) as DISTANCE  ";
            query += "from LINKS as l1,LINKS as l2,( ";
            query += "select l1.NUM,MIN(l2.NUM - l1.NUM) as diff ";
            query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS  ";
            query += "where l1.NUM != l2.NUM and ";
            query += "l1.LINK_ID = l2.LINK_ID  ";
            query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID  and l2.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            query += "and SEMANTIC_LINK_ID in ( " + id[0];

            for (int i = 1; i < id.Length; i++)
            {
                query += ", " + id;
            }

            query += ") ";
            query += "group by l1.NUM) as Corres ";
            query += "where l1.NUM = Corres.NUM and l1.LINK_ID = l2.LINK_ID ";
            query += "and l2.NUM = l1.NUM + Corres.diff ";
            query += "order by NUM ";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable LinkTableGetter2(int id)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //没クエリ
            //string query = "select l1.LINK_ID as LINK_ID ,l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            //query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS ";
            //query += "where l1.NUM = l2.NUM - 1 ";
            //query += "and l1.LINK_ID = l2.LINK_ID ";
            //query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            //query += "and SEMANTIC_LINK_ID in ( " + id[0];

            //for (int i = 1; i < id.Length; i++)
            //{
            //    query += ", " + id;
            //}

            //query += ") ";
            //query += "order by l1.NUM ";


            string query = "select l1.LINK_ID as LINK_ID , l1.NUM, l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            query += ",SQRT((l1.LATITUDE - l2.LATITUDE) * (l1.LATITUDE - l2.LATITUDE) + (l1.LONGITUDE - l2.LONGITUDE) * (l1.LONGITUDE - l2.LONGITUDE)) as DISTANCE  ";
            query += "from LINKS as l1,LINKS as l2,( ";
            query += "select l1.NUM,MIN(ABS(l2.NUM - l1.NUM)) as diff ";
            query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS  ";
            query += "where l1.NUM != l2.NUM and ";
            query += " l1.LINK_ID = l2.LINK_ID  ";
            query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID and l2.LINK_ID = SEMANTIC_LINKS.LINK_ID ";
            query += "and SEMANTIC_LINK_ID in ( " + id;


            query += ") ";
            query += "group by l1.NUM) as Corres ";
            query += "where l1.NUM = Corres.NUM and l1.LINK_ID = l2.LINK_ID ";
            query += "and ABS(l2.NUM-l1.NUM) =  Corres.diff ";
            query += "order by NUM ";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }

        public static int LinkNumGetter(int[] id, Object latitude, Object longitude)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();
            int num = 0;

            String query = "WITH LINE ( LINK_ID , NUM, START_LAT, START_LONG, END_LAT, END_LONG, DISTANCE) as ( ";
            query += "select l1.LINK_ID as LINK_ID , l1.NUM, l1.LATITUDE as START_LAT, l1.LONGITUDE as START_LONG,l2.LATITUDE as END_LAT, l2.LONGITUDE as END_LONG ";
            query += ",SQRT((l1.LATITUDE - l2.LATITUDE) * (l1.LATITUDE - l2.LATITUDE) + (l1.LONGITUDE - l2.LONGITUDE) * (l1.LONGITUDE - l2.LONGITUDE)) as DISTANCE  ";
            query += "from LINKS as l1,LINKS as l2,( ";
            query += "select l1.NUM,MIN(l2.NUM - l1.NUM) as diff ";
            query += "from LINKS as l1,LINKS as l2,SEMANTIC_LINKS  ";
            query += "where l1.NUM < l2.NUM  ";
            query += "and l1.LINK_ID = l2.LINK_ID  ";
            query += "and l1.LINK_ID = SEMANTIC_LINKS.LINK_ID  ";
            query += "and SEMANTIC_LINK_ID in ( " + id[0];

            for (int i = 1; i < id.Length; i++)
            {
                query += ", " + id;
            }

            query += ") ";
            query += "group by l1.NUM) as Corres ";
            query += "where l1.NUM = Corres.NUM ";
            query += "and l2.NUM = l1.NUM + Corres.diff )";
            query += "SELECT A.NUM FROM LINE AS A WHERE NOT EXISTS(SELECT * FROM LINE AS B WHERE SQRT((" + latitude + " - A.START_LAT) * ";
            query += "(" + latitude + " - A.START_LAT) + (" + longitude + " - A.START_LONG) * (" + longitude + " - A.START_LONG)) > ";
            query += "SQRT((" + latitude + " - B.START_LAT) * (" + latitude + " - B.START_LAT) + (" + longitude + " - B.START_LONG) * (" + longitude + " - B.START_LONG)))";


            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            DataRow[] row = dt.Select();

            if (row.Length == 1)
            {
                num = Convert.ToInt32(row[0]["NUM"]);
            }

            return num;
        }
        public static DataTable TripGPSGetterfromTRIPID(int TRIP_ID)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "SELECT ANDROID_GPS_RAW.* FROM ANDROID_GPS_RAW INNER JOIN (SELECT * FROM ECOLOG WHERE TRIP_ID =" + TRIP_ID + ") AS ECOLOGTABLE ON ANDROID_GPS_RAW.JST = ECOLOGTABLE.JST AND ANDROID_GPS_RAW.SENSOR_ID = '24'";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable TripGidsGetterfromTRIPID(int TRIP_ID)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "WITH LEAFSPYNOAIRCON AS (SELECT TRIP_ID FROM LEAFSPY_RAW GROUP BY TRIP_ID HAVING SUM(LEAFSPY_RAW.AC_PWR_250W) = 0)";
            query += " SELECT ECOLOG.TRIP_ID,ECOLOG.JST,ECOLOG.LATITUDE,ECOLOG.LONGITUDE,NOWLEAFSPY.DATETIME,-BEFORELEAFSPY.GIDS + NOWLEAFSPY.GIDS AS DELTA_GIDS";
            query += " FROM ECOLOG, LEAFSPY_RAW AS NOWLEAFSPY, LEAFSPYNOAIRCON, LEAFSPY_RAW AS BEFORELEAFSPY WHERE ECOLOG.DRIVER_ID =NOWLEAFSPY.DRIVER_ID AND ECOLOG.CAR_ID =NOWLEAFSPY.CAR_ID AND ";
            query += "ECOLOG.JST = NOWLEAFSPY.DATETIME AND NOWLEAFSPY.TRIP_ID = LEAFSPYNOAIRCON.TRIP_ID AND ECOLOG.TRIP_ID = " + TRIP_ID;
            query += " AND NOWLEAFSPY.TRIP_ID = BEFORELEAFSPY.TRIP_ID AND DATEDIFF(second, BEFORELEAFSPY.DATETIME, NOWLEAFSPY.DATETIME) < -2 AND ";
            query += " DATEDIFF(second, BEFORELEAFSPY.DATETIME, NOWLEAFSPY.DATETIME) > -6";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable TripECOLOGGetterfromTRIPID(int TRIP_ID)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "SELECT * FROM ECOLOG WHERE TRIP_ID = " + TRIP_ID;

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable SegmentGetter(int semanticLinkId)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "SELECT *　FROM　[100M_SEGMENT]　WHERE SEMANTIC_LINK_ID = " + semanticLinkId;

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }
        public static DataTable LinkListGetter(int semanticLinkId)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "SELECT *　FROM　LINK_LIST　WHERE SEMANTIC_LINK_ID = " + semanticLinkId;

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }

        public static DataTable TripGPSGetter(String START_TIME, String END_TIME)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "select * ";
            query += "from ANDROID_GPS_RAW ";
            query += "where JST between '" + START_TIME + "' and '" + END_TIME + "' ";
            query += "and SENSOR_ID = 24";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }

        public static DataTable TripACCGetter(String START_TIME, String END_TIME)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "SELECT CONVERT(VARCHAR,DATETIME,121) AS DATETIME,ACC_X,ACC_Y,ACC_Z FROM ANDROID_ACC_RAW WHERE DATETIME >= DATEADD(second, -1, '" + START_TIME + "') AND DATETIME <= DATEADD(second, 1, '" + END_TIME + "') AND SENSOR_ID = '24'";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }
            return dt;
        }


        //START_TIMEの1秒前とEND_TIMEの1秒後のGPSを取ってくる
        public static DataTable seGPSGetter(String START_TIME, String END_TIME)
        {
            string cn = @"Data Source=ECOLOGDB;Initial Catalog=ECOLOGDBver2;Integrated Security=True";//接続DB

            DataTable dt = new DataTable();

            //クエリ編集
            string query = "select * ";
            query += "from ANDROID_GPS_RAW ";
            query += "where (JST = DATEADD(second, -1, '" + START_TIME + "') ";
            query += "or JST = DATEADD(second, 1, '" + END_TIME + "')) ";
            query += "and SENSOR_ID = 24";

            using (SqlConnection SQLConn = new SqlConnection(cn))
            {
                SQLConn.FireInfoMessageEventOnUserErrors = false;

                SqlDataAdapter da = new SqlDataAdapter(query, cn);

                //DBからデータを取得しDataTableへ格納
                try
                {
                    SQLConn.Open();
                    SqlCommand cmd = new SqlCommand(query, SQLConn);
                    cmd.CommandTimeout = 600;
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    //エラー(いじった方がいいか？)
                    //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    SQLConn.Close();
                }
            }

            return dt;
        }

    }
}
