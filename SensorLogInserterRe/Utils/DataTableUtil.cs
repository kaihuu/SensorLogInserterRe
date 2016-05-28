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
        public static DataTable GetTripsRawTable()
        {
            DataTable tripsRawTable = new DataTable();

            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            tripsRawTable.Columns.Add(new DataColumn("DRIVER_ID"));
            tripsRawTable.Columns.Add(new DataColumn("CAR_ID"));
            tripsRawTable.Columns.Add(new DataColumn("SENSOR_ID"));
            tripsRawTable.Columns.Add(new DataColumn("START_TIME"));
            tripsRawTable.Columns.Add(new DataColumn("END_TIME"));
            tripsRawTable.Columns.Add(new DataColumn("START_LATITUDE"));
            tripsRawTable.Columns.Add(new DataColumn("START_LONGITUDE"));
            tripsRawTable.Columns.Add(new DataColumn("END_LATITUDE"));
            tripsRawTable.Columns.Add(new DataColumn("END_LONGITUDE"));
            tripsRawTable.Columns.Add(new DataColumn("CONSUMED_ENERGY"));
            tripsRawTable.Columns.Add(new DataColumn("TRIP_DIRECTION"));

            return tripsRawTable;
        }

        public static DataTable GetTripsTable()
        {
            DataTable tripsTable = new DataTable();

            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
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

        public static DataTable GetAndroidGpsRawTable()
        {
            // TODO 並び順をデータベース通りに
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            DataTable gpsRawTable = new DataTable();

            gpsRawTable.Columns.Add(new DataColumn("DRIVER_ID"));
            gpsRawTable.Columns.Add(new DataColumn("CAR_ID"));
            gpsRawTable.Columns.Add(new DataColumn("SENSOR_ID"));
            gpsRawTable.Columns.Add(new DataColumn("JST"));
            gpsRawTable.Columns.Add(new DataColumn("LATITUDE"));
            gpsRawTable.Columns.Add(new DataColumn("LONGITUDE"));
            gpsRawTable.Columns.Add(new DataColumn("ALTITUDE"));
            gpsRawTable.Columns.Add(new DataColumn("ANDROID_TIME"));

            return gpsRawTable;
        }

        public static DataTable GetAndroidAccRawTable()
        {
            // TODO 並び順をデータベース通りに
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            DataTable accRawTable = new DataTable();

            accRawTable.Columns.Add(new DataColumn("DRIVER_ID"));
            accRawTable.Columns.Add(new DataColumn("CAR_ID"));
            accRawTable.Columns.Add(new DataColumn("SENSOR_ID"));
            accRawTable.Columns.Add(new DataColumn("DATETIME"));
            accRawTable.Columns.Add(new DataColumn("ACC_X"));
            accRawTable.Columns.Add(new DataColumn("ACC_Y"));
            accRawTable.Columns.Add(new DataColumn("ACC_Z"));

            return accRawTable;
        }

        public static DataTable GetCorrectedGpsTable()
        {
            DataTable correctedGpsTable = new DataTable();
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            correctedGpsTable.Columns.Add(new DataColumn("DRIVER_ID"));
            correctedGpsTable.Columns.Add(new DataColumn("CAR_ID"));
            correctedGpsTable.Columns.Add(new DataColumn("SENSOR_ID"));
            correctedGpsTable.Columns.Add(new DataColumn("JST"));
            correctedGpsTable.Columns.Add(new DataColumn("LATITUDE"));
            correctedGpsTable.Columns.Add(new DataColumn("LONGITUDE"));
            correctedGpsTable.Columns.Add(new DataColumn("SPEED"));
            correctedGpsTable.Columns.Add(new DataColumn("HEADING"));
            correctedGpsTable.Columns.Add(new DataColumn("DISTANCE_DIFFERENCE"));

            return correctedGpsTable;
        }

        public static DataTable GetCorrectedAccTable()
        {
            DataTable correctedAccTable = new DataTable();
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            correctedAccTable.Columns.Add(new DataColumn("DRIVER_ID"));
            correctedAccTable.Columns.Add(new DataColumn("CAR_ID"));
            correctedAccTable.Columns.Add(new DataColumn("SENSOR_ID"));
            correctedAccTable.Columns.Add(new DataColumn("DATETIME"));
            correctedAccTable.Columns.Add(new DataColumn("ACC_X"));
            correctedAccTable.Columns.Add(new DataColumn("ACC_Y"));
            correctedAccTable.Columns.Add(new DataColumn("ACC_Z"));

            return correctedAccTable;
        }
    }
}
