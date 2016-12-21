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
        private static AltitudeCalculator _instance;

        private DataTable _registeredTable;

        public static AltitudeCalculator GetInstance()
        {
            if (_instance == null)
                _instance = new AltitudeCalculator
                {
                    _registeredTable = Altitude10MMeshRegisteredDao.Get()
                };

            return _instance;
        }

        private AltitudeCalculator()
        {
            // for singleton
        }

        public Tuple<int, double> CalcAltitude(double latitude, double longitude)
        {
            int meshId;
            float altitude;

            var selectedRows = Altitude10MMeshRegisteredDao.GetAltitude(latitude, longitude).Select(null);
            //_registeredTable.AsEnumerable()
            //.Where(row => row.Field<double>("lower_latitude") <= latitude
            //    && row.Field<double>("upper_latitude") > latitude
            //    && row.Field<double>("lower_longitude") <= longitude
            //    && row.Field<double>("upper_longitude") > longitude)
            //.ToArray();

            //メッシュ登録済み
            if (selectedRows.Length > 0)
            {
                altitude = selectedRows[0].Field<Single>(Altitude10MMeshRegisteredDao.ColumnAltitude);
                meshId = selectedRows[0].Field<int>(Altitude10MMeshRegisteredDao.ColumnMeshId);
            }
            else  //登録されていないメッシュ
            {
                //登録用のIDを取得
                meshId = Altitude10MMeshRegisteredDao.GetMaxMeshId() + 1;
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

                    ////データテーブルに新しい標高データを登録
                    //DataRow newrow = _registeredTable.NewRow();
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnMeshId, meshId);
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnLowerLatitude, altitudeDatum.LowerLatitude);
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnLowerLongitude, altitudeDatum.LowerLongitude);
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnUpperLatitude, altitudeDatum.UpperLatitude);
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnUpperLongitude, altitudeDatum.UpperLongitude);
                    //newrow.SetField(Altitude10MMeshRegisteredDao.ColumnAltitude, altitudeDatum.Altitude);

                    //_registeredTable.Rows.Add(newrow);
                }
            }

            return new Tuple<int, double>(meshId, altitude);
        }
    }
}
