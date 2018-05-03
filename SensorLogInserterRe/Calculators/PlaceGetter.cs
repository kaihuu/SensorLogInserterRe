using SensorLogInserterRe.Daos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class PlaceGetter
    {
        private static PlaceGetter _instance;

        private DataTable _placeTable;

        private PlaceGetter()
        {
            // for Singleton pattern
        }

        public static PlaceGetter GetInstance()
        {
            if (_instance == null)
            {
                InitInstance();
            }

            return _instance;
        }

        private static void InitInstance()
        {
            _instance = new PlaceGetter
            {
                _placeTable = PlaceDao.Get()
            };

        }

        public DataTable getDataTable()
        {
            return _placeTable;
        }

        public Tuple<GeoCoordinate, GeoCoordinate> Get(String placeName)
        {
            GeoCoordinate startCoordinate = new GeoCoordinate();
            GeoCoordinate endCoordinate = new GeoCoordinate();
            DataRow dataRow = _placeTable.Select("place_name = '"+ placeName + "'")[0];

            startCoordinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnStartLatitude);
            startCoordinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnStartLongitude);
            endCoordinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnEndLatitude);
            endCoordinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnEndLongitude);


            return Tuple.Create(startCoordinate, endCoordinate);
        }

        /*  
         *  getByProperty()
         *  特定Property値に合致するPLACEを取得する。
         *  複数存在するので、タプルのリスト形式で返す。
         *  (上記Get()メソッドも同様の実装にするべきだと推察される。
         */
        public List<Tuple<GeoCoordinate, GeoCoordinate>> GetRowsByProperty(string property)
        {
            GeoCoordinate startCoodinate;
            GeoCoordinate endCoordinate;

            // 引数で指定したproperty値を持つレコード群を取得
            DataRow[] dataRows = _placeTable.Select("property = '" + property + "'");

            List<Tuple<GeoCoordinate, GeoCoordinate>> retTuples = new List<Tuple<GeoCoordinate, GeoCoordinate>>();

            // 全てのレコードに対して上記のGet()の戻り値と同じ形式のTupleを作成して、リストに追加
            foreach(DataRow dataRow in dataRows)
            {
                startCoodinate = new GeoCoordinate();
                endCoordinate = new GeoCoordinate();
                startCoodinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnStartLatitude);
                startCoodinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnStartLongitude);
                endCoordinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnEndLatitude);
                endCoordinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnEndLongitude);

               // Tuple<GeoCoordinate, GeoCoordinate> tuple = new Tuple<GeoCoordinate, GeoCoordinate>(startCoodinate, endCoordinate);

                retTuples.Add(new Tuple<GeoCoordinate, GeoCoordinate>(startCoodinate, endCoordinate));
            }

            return retTuples;
        }
    }
}
