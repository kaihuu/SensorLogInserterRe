using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    static class DataTableUtil
    {
        public static DataTable GetTripsTable()
        {
            DataTable tripsTable = new DataTable();
            tripsTable.Columns.Add(new DataColumn("TRIP_ID"));
            tripsTable.Columns.Add(new DataColumn("DRIVER_ID"));
            tripsTable.Columns.Add(new DataColumn("CAR_ID"));
            tripsTable.Columns.Add(new DataColumn("SENSOR_ID"));
            tripsTable.Columns.Add(new DataColumn("START_TIME"));
            tripsTable.Columns.Add(new DataColumn("END_TIME"));
            tripsTable.Columns.Add(new DataColumn("START_LATITUDE"));
            tripsTable.Columns.Add(new DataColumn("START_LONGITUDE"));
            tripsTable.Columns.Add(new DataColumn("END_LATITUDE"));
            tripsTable.Columns.Add(new DataColumn("END_LONGITUDE"));
            tripsTable.Columns.Add(new DataColumn("CONSUMED_ENERGY"));
            tripsTable.Columns.Add(new DataColumn("TRIP_DIRECTION"));

            return tripsTable;
        }

    }
}
