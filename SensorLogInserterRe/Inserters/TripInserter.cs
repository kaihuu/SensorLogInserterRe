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

            row[TripsDao.ColumnDriverId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnDriverId].ToString());
            row[TripsDao.ColumnCarId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnCarId].ToString());
            row[TripsDao.ColumnSensorId] = Int32.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnSensorId].ToString());
            row[TripsDao.ColumnStartTime] = gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnJst].ToString();
            row[TripsDao.ColumnStartLatitude] = double.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnLatitude].ToString());
            row[TripsDao.ColumnStartLongitude] = double.Parse(gpsRawTable.Rows[0][AndroidGpsRawDao.ColumnLongitude].ToString());
            row[TripsDao.ColumnEndTime] = gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnJst].ToString();
            row[TripsDao.ColumnEndLatitude] = double.Parse(gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnLatitude].ToString());
            row[TripsDao.ColumnEndLongitude] = double.Parse(gpsRawTable.Rows[gpsRawTable.Rows.Count - 1][AndroidGpsRawDao.ColumnLongitude].ToString());

            tripsTable.Rows.Add(row);

            TripsRawDao.Insert(tripsTable);
        }

    }
}
