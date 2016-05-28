using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Inserters
{
    class TripInserter
    {
        public static void InsertTripRaw(DataTable gpsRawTable)
        {
            var tripsTable = DataTableUtil.GetTripsTable();
            DataRow row = tripsTable.NewRow();

            row.SetField(TripsRawDao.ColumnDriverId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnDriverId));
            row.SetField(TripsRawDao.ColumnCarId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnCarId));
            row.SetField(TripsRawDao.ColumnSensorId, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnSensorId));
            row.SetField(TripsRawDao.ColumnStartTime, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnStartLatitude, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnStartLongitude, gpsRawTable.Rows[0].Field<int>(AndroidGpsRawDao.ColumnLongitude));
            row.SetField(TripsRawDao.ColumnEndTime, gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnJst));
            row.SetField(TripsRawDao.ColumnEndLatitude, gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnLatitude));
            row.SetField(TripsRawDao.ColumnEndLongitude, gpsRawTable.Rows[gpsRawTable.Rows.Count - 1].Field<int>(AndroidGpsRawDao.ColumnLongitude));
            tripsTable.Rows.Add(row);

            TripsRawDao.Insert(tripsTable);
        }

        public static void InsertTrip()
        {
            
        }
    }
}
