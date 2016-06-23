using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

/***
 * クソみたいなコードだけどこれ以上無理だわ加速度使う人あとはこれを何とかしてくれ元のコードですら合ってるかわからんリファクタリングして更にわけわかんなくなってる
 ***/

namespace SensorLogInserterRe.Inserters.Components
{
    class AccCorrector
    {
        public static DataTable CorrectAcc(DateTime startTime, DateTime endTime, InsertDatum datum, DataRow tripRow)
        {
            /*** LocationListenerのtimestampとSystem.currentTimeMillis()の差分を計算している
            LocationListenerのtimestampはおそらく、システム時間を参照しているからこの処理は意味ないかも ***/
            int millisecTimeDiff = AndroidGpsRawDao.GetMilliSencodTimeDiffBetweenJstAndAndroidTime(
                    tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                    tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);

            var accRawTable = AndroidAccRawDao.Get(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                tripRow.Field<DateTime>(TripsDao.ColumnEndTime), millisecTimeDiff, datum);

            accRawTable = DataTableUtil.GetTempCorrectedAccTable(accRawTable);

            if (accRawTable.Rows.Count == 0)
            {
                // TODO ログ書き込み
                //エラー期間をログファイルに記録
                // WriteLog("DRIVER_ID:" + userID.DriverID + "SENSOR_ID:" + userID.SensorID + " " + tripStart + "～" + tripEnd + "加速度データなし", LogMode.acc);
                return null;
            }
            else if (false) // TODO 挿入済みチェックをすべきか？
            {
                // WriteLog("DRIVER_ID:" + userID.DriverID + "SENSOR_ID:" + userID.SensorID + " " + tripStart.ToString() + "～" + tripEnd.ToString() + "はすでに挿入済みです\r\n", LogMode.acc);
                return null;
            }

            // TODO ファイルに記録
            //WriteLog("X方向平均(補正前)＝" + dtACC.Compute("AVG(LONGITUDINAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString(), LogMode.acc);
            //WriteLog("Y方向平均(補正前)＝" + dtACC.Compute("AVG(LATERAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString(), LogMode.acc);
            //WriteLog("Z方向平均(補正前)＝" + dtACC.Compute("AVG(VERTICAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString(), LogMode.acc);   

            var accurateStoppingAccRawTable = CorrectedAccDao.GetAccurateStoppingAccRaw(millisecTimeDiff, datum);

            if (accurateStoppingAccRawTable.Rows.Count == 0)
            {
                // TODO ログ出力
                //WriteLog("DRIVER_ID:" + userID.DriverID + "SENSOR_ID:" + userID.SensorID + " " + tripStart.ToString() + "～" + tripEnd.ToString() + "停止時の加速度データなし or タイムアウトエラーが発生 + \r\n", LogMode.acc);
                return null;
            }

            // TODO テーブル関数定義の場合、staticフィールドで参照するがよろし
            double avgX = double.Parse(accurateStoppingAccRawTable.Rows[0]["ACC_X"].ToString());
            double avgY = double.Parse(accurateStoppingAccRawTable.Rows[0]["ACC_Y"].ToString());
            double avgZ = double.Parse(accurateStoppingAccRawTable.Rows[0]["ACC_Z"].ToString());

            var vector1 = new ThreeDimensionalVector(0, 0, 9.8);
            var vector2 = new ThreeDimensionalVector(avgX, avgY, avgZ);

            var vectorProduct = new ThreeDimensionalVector(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
            var vasisVP = new ThreeDimensionalVector(vectorProduct.X / MathUtil.CalcVectorAbsoluteValue(vectorProduct),
                vectorProduct.Y / MathUtil.CalcVectorAbsoluteValue(vectorProduct),
                vectorProduct.Z / MathUtil.CalcVectorAbsoluteValue(vectorProduct));

            var angle = Math.Asin(MathUtil.CalcVectorAbsoluteValue(vectorProduct) / (MathUtil.CalcVectorAbsoluteValue(vector1) * MathUtil.CalcVectorAbsoluteValue(vector2)));

            // TODO ログ出力
            //WriteLog("角度(Z軸補正)=" + angle * 360 / (2 * Math.PI), LogMode.acc);
            //WriteLog("回転軸 = (" + vasisVP.X + ", " + vasisVP.Y + ", " + vasisVP.Z + ")", LogMode.acc);

            Quaternion p = new Quaternion(0, vector2.X, vector2.Y, vector2.Z);
            Quaternion q = new Quaternion(Math.Cos(angle / 2), vasisVP.X * Math.Sin(angle / 2), vasisVP.Y * Math.Sin(angle / 2), vasisVP.Z * Math.Sin(angle / 2));
            Quaternion r = new Quaternion(Math.Cos(angle / 2), -vasisVP.X * Math.Sin(angle / 2), -vasisVP.Y * Math.Sin(angle / 2), -vasisVP.Z * Math.Sin(angle / 2));

            Quaternion rp = MathUtil.MultiplyQuaternion(r, p);
            Quaternion rpq = MathUtil.MultiplyQuaternion(rp, q);

            for (int i = 0; i < accRawTable.Rows.Count; i++)
            {
                Quaternion p1 = new Quaternion(0,
                    accRawTable.Rows[i].Field<Single>("LONGITUDINAL_ACC"),
                    accRawTable.Rows[i].Field<Single>("LATERAL_ACC"),
                    accRawTable.Rows[i].Field<Single>("VERTICAL_ACC"));
                Quaternion rp1 = MathUtil.MultiplyQuaternion(r, p1);
                Quaternion rpq1 = MathUtil.MultiplyQuaternion(rp1, q);

                // TODO SetFieldに書き直し、する価値ないか
                accRawTable.Rows[i]["LONGITUDINAL_ACC"] = Single.Parse(rpq1.X.ToString());
                accRawTable.Rows[i]["LATERAL_ACC"] = Single.Parse(rpq1.Y.ToString());
                accRawTable.Rows[i]["VERTICAL_ACC"] = Single.Parse(rpq1.Z.ToString());
                accRawTable.Rows[i]["ALPHA"] = (Single)(angle * 360 / (2 * Math.PI));
                accRawTable.Rows[i]["VECTOR_X"] = (Single)vasisVP.X;
                accRawTable.Rows[i]["VECTOR_Y"] = (Single)vasisVP.Y;

                accRawTable.Rows[i]["ROLL"] = DBNull.Value;
                accRawTable.Rows[i]["PITCH"] = DBNull.Value;
                accRawTable.Rows[i]["YAW"] = DBNull.Value;
            }

            // TODO ログ出力
            //WriteLog("X方向平均(補正後)＝" + dtACC.Compute("AVG(LONGITUDINAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString(), LogMode.acc);
            //WriteLog("Y方向平均(補正後)＝" + dtACC.Compute("AVG(LATERAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString(), LogMode.acc);
            //WriteLog("Z方向平均(補正後)＝" + dtACC.Compute("AVG(VERTICAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString() + "\r\n", LogMode.acc);

            var tableBreaking = CorrectedAccDao.GetAccurateAccBreakingRaw(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                tripRow.Field<DateTime>(TripsDao.ColumnEndTime),
                datum);

            double xsum = 0;
            double ysum = 0;
            int count = 0;

            for (int i = 0; i < tableBreaking.Rows.Count; i++)
            {
                DateTime end = DateTime.Parse(tableBreaking.Rows[i]["JST"].ToString());
                DateTime start = end.AddSeconds(-10);

                DataRow[] rows = accRawTable.Select("JST >= #" + start + "# AND JST <= #" + end + "#");

                foreach (DataRow row in rows)
                {
                    xsum = xsum + double.Parse(row["LONGITUDINAL_ACC"].ToString());
                    ysum = ysum + double.Parse(row["LATERAL_ACC"].ToString());

                    count++;
                }
            }

            if (count == 0)
            {
                // TODO ログ出力
                //WriteLog("DRIVER_ID:" + userID.DriverID + "SENSOR_ID:" + userID.SensorID + " " + "減速データがありません\r\n", LogMode.acc);
                return null;
            }

            double bx = xsum / count;
            double by = ysum / count;

            var v3 = new ThreeDimensionalVector(-1, 0, 0);
            var v4 = new ThreeDimensionalVector(bx, by, 0);
            double innerProduct = v3.X * v4.X + v3.Y * v4.Y;
            double cos = innerProduct / (MathUtil.CalcVectorAbsoluteValue(v3) * MathUtil.CalcVectorAbsoluteValue(v4));

            double anglexy = Math.Acos(cos);//ラジアン(0～180)


            for (int j = 0; j < accRawTable.Rows.Count; j++)
            {
                double x = double.Parse(accRawTable.Rows[j]["LONGITUDINAL_ACC"].ToString());
                double y = double.Parse(accRawTable.Rows[j]["LATERAL_ACC"].ToString());
                double z = double.Parse(accRawTable.Rows[j]["VERTICAL_ACC"].ToString());
                double x2;
                double y2;
                double z2;

                if (by >= 0)
                {
                    x2 = Math.Cos(anglexy) * x - Math.Sin(anglexy) * y;
                    y2 = Math.Sin(anglexy) * x + Math.Cos(anglexy) * y;
                    z2 = z;
                }
                else
                {
                    x2 = Math.Cos(-anglexy) * x - Math.Sin(-anglexy) * y;
                    y2 = Math.Sin(-anglexy) * x + Math.Cos(-anglexy) * y;
                    z2 = z;
                }

                accRawTable.Rows[j]["LONGITUDINAL_ACC"] = Single.Parse(x2.ToString());
                accRawTable.Rows[j]["LATERAL_ACC"] = Single.Parse(y2.ToString());
                accRawTable.Rows[j]["VERTICAL_ACC"] = Single.Parse(z2.ToString());
                accRawTable.Rows[j]["BETA"] = (Single)(anglexy * 360 / (2 * Math.PI));
            }

            avgX = double.Parse(accRawTable.Compute("AVG(LONGITUDINAL_ACC)", "JST > #" + tripRow.Field<DateTime>(TripsDao.ColumnStartTime) + "# AND JST < #" + tripRow.Field<DateTime>(TripsDao.ColumnEndTime) + "#").ToString());
            avgZ = double.Parse(accRawTable.Compute("AVG(VERTICAL_ACC)", "JST > #" + tripRow.Field<DateTime>(TripsDao.ColumnStartTime) + "# AND JST < #" + tripRow.Field<DateTime>(TripsDao.ColumnEndTime) + "#").ToString());

            //WriteLog("平均値 x = " + v4.X + ", y = " + v4.Y, LogMode.acc);
            //WriteLog("XY回転角度＝" + "xy = " + anglexy * 360 / (2 * Math.PI), LogMode.acc);
            //WriteLog("X方向平均(補正後)＝" + averagex.ToString(), LogMode.acc);
            //WriteLog("Y方向平均(補正後)＝" + (double.Parse(dtACC.Compute("AVG(LATERAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString())).ToString(), LogMode.acc);
            //WriteLog("Z方向平均(補正後)＝" + averagez.ToString() + "\r\n", LogMode.acc);

            avgX = double.Parse(accRawTable.Compute("AVG(LONGITUDINAL_ACC)", "JST > #" + tripRow.Field<DateTime>(TripsDao.ColumnStartTime) + "# AND JST < #" + tripRow.Field<DateTime>(TripsDao.ColumnEndTime) + "#").ToString());

            var v5 = new ThreeDimensionalVector(0, 0, 9.8);
            var v6 = new ThreeDimensionalVector(avgX, 0, avgZ);

            innerProduct = v5.X * v6.X + v5.Z * v6.Z;
            cos = innerProduct / (MathUtil.CalcVectorAbsoluteValue(v5) * MathUtil.CalcVectorAbsoluteValue(v6));

            anglexy = Math.Acos(cos);//ラジアン(0～180)

            for (int j = 0; j < accRawTable.Rows.Count; j++)
            {
                double x = double.Parse(accRawTable.Rows[j]["LONGITUDINAL_ACC"].ToString());
                double y = double.Parse(accRawTable.Rows[j]["LATERAL_ACC"].ToString());
                double z = double.Parse(accRawTable.Rows[j]["VERTICAL_ACC"].ToString());
                double x2;
                double y2;
                double z2;

                if (avgX >= 0)
                {
                    x2 = Math.Cos(anglexy) * x - Math.Sin(anglexy) * z;
                    y2 = y;
                    z2 = Math.Sin(anglexy) * x + Math.Cos(anglexy) * z;
                }
                else
                {
                    x2 = Math.Cos(-anglexy) * x - Math.Sin(-anglexy) * z;
                    y2 = y;
                    z2 = Math.Sin(-anglexy) * x + Math.Cos(-anglexy) * z;
                }

                accRawTable.Rows[j]["LONGITUDINAL_ACC"] = Single.Parse(x2.ToString());
                accRawTable.Rows[j]["LATERAL_ACC"] = Single.Parse(y2.ToString());
                accRawTable.Rows[j]["VERTICAL_ACC"] = Single.Parse(z2.ToString());
                accRawTable.Rows[j]["GAMMA"] = (Single)(anglexy * 360 / (2 * Math.PI));
            }

            // TODO ログ出力
            //WriteLog("平均値 x = " + v6.X + ", z = " + v6.Z, LogMode.acc);
            //WriteLog("XZ回転角度＝" + "xz = " + anglexy * 360 / (2 * Math.PI), LogMode.acc);
            //WriteLog("X方向平均(補正後)＝" + double.Parse(dtACC.Compute("AVG(LONGITUDINAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString()).ToString(), LogMode.acc);
            //WriteLog("Y方向平均(補正後)＝" + double.Parse(dtACC.Compute("AVG(LATERAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString()).ToString(), LogMode.acc);
            //WriteLog("Z方向平均(補正後)＝" + double.Parse(dtACC.Compute("AVG(VERTICAL_ACC)", "JST > #" + tripStart + "# AND JST < #" + tripEnd + "#").ToString()).ToString() + "\r\n", LogMode.acc);

            return accRawTable;
        }
    }

}
