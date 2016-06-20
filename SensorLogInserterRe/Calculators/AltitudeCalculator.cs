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
        private static AltitudeCalculator Instance { get; set; }

        private DataTable RegisteredTable { get; set; }

        public static AltitudeCalculator GetInstance()
        {
            if (Instance == null)
                Instance = new AltitudeCalculator
                {
                    RegisteredTable = Altitude10MMeshRegisteredDao.Get()
                };

            return Instance;
        }

        private AltitudeCalculator()
        {
            // for singleton
        }

        public Tuple<int, double> CalcAltitude(double latitude, double longitude)
        {
            int meshId;
            double altitude;

            var selectedRows = RegisteredTable.AsEnumerable()
                .Where(row => row.Field<double>("lower_latitude") <= latitude
                    && row.Field<double>("upper_latitude") > latitude
                    && row.Field<double>("lower_longitude") <= longitude
                    && row.Field<double>("upper_longitude") > longitude)
                .ToArray();

            //メッシュ登録済み
            if (selectedRows.Length > 0)
            {
                altitude = selectedRows[0].Field<double>(Altitude10MMeshRegisteredDao.ColumnAltitude);
                meshId = selectedRows[0].Field<int>(Altitude10MMeshRegisteredDao.ColumnMeshId);
            }
            else  //登録されていないメッシュ
            {
                //登録用のIDを取得
                meshId = Altitude10MMeshRegisteredDao.GetMaxMeshId();
                var altitudeDatum = Altitude10MMeshDao.Get(latitude, longitude);
                
                //標高データが存在しなかった場合
                if (altitudeDatum.Altitude == null)
                {
                    //登録せず標高を-999とする
                    altitude = -999;
                    meshId = -1;
                }
                else
                {
                    altitude = altitudeDatum.Altitude.Value;

                    //標高データ修正テーブル
                    Altitude10MMeshRegisteredDao.Insert(meshId, altitudeDatum);

                    //データテーブルに新しい標高データを登録
                    DataRow newrow = RegisteredTable.NewRow();
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnMeshId, meshId);
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnLowerLatitude, altitudeDatum.LowerLatitude);
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnLowerLongitude, altitudeDatum.LowerLongitude);
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnUpperLatitude, altitudeDatum.UpperLatitude);
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnUpperLongitude, altitudeDatum.UpperLongitude);
                    newrow.SetField(Altitude10MMeshRegisteredDao.ColumnAltitude, altitudeDatum.Altitude);

                    RegisteredTable.Rows.Add(newrow);
                }
            }

            return new Tuple<int, double>(meshId, altitude);
        }
    }
}
