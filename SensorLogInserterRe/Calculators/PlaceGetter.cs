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
            DataRow dataRow = _placeTable.Select("place_name = "+ placeName)[0];

            startCoordinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnStartLatitude);
            startCoordinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnStartLongitude);
            endCoordinate.Latitude = dataRow.Field<double>(PlaceDao.ColumnEndLatitude);
            endCoordinate.Longitude = dataRow.Field<double>(PlaceDao.ColumnEndLongitude);


            return Tuple.Create(startCoordinate, endCoordinate);
        }
    }
}
