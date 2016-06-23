using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Daos;

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
            tripsTable.Columns.Add(new DataColumn("TRIP_ID", typeof(int)));
            tripsTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            tripsTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            tripsTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            tripsTable.Columns.Add(new DataColumn("START_TIME", typeof(DateTime)));
            tripsTable.Columns.Add(new DataColumn("END_TIME", typeof(DateTime)));
            tripsTable.Columns.Add(new DataColumn("START_LATITUDE", typeof(double)));
            tripsTable.Columns.Add(new DataColumn("START_LONGITUDE", typeof(double)));
            tripsTable.Columns.Add(new DataColumn("END_LATITUDE", typeof(double)));
            tripsTable.Columns.Add(new DataColumn("END_LONGITUDE", typeof(double)));
            tripsTable.Columns.Add(new DataColumn("CONSUMED_ENERGY", typeof(double)));
            tripsTable.Columns.Add(new DataColumn("TRIP_DIRECTION", typeof(string)));

            return tripsTable;
        }

        public static DataTable GetAndroidGpsRawTable()
        {
            // TODO 並び順をデータベース通りに
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            DataTable gpsRawTable = new DataTable();

            gpsRawTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            gpsRawTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            gpsRawTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            gpsRawTable.Columns.Add(new DataColumn("JST", typeof(DateTime)));
            gpsRawTable.Columns.Add(new DataColumn("LATITUDE", typeof(double)));
            gpsRawTable.Columns.Add(new DataColumn("LONGITUDE", typeof(double)));
            gpsRawTable.Columns.Add(new DataColumn("ALTITUDE", typeof(double)));
            gpsRawTable.Columns.Add(new DataColumn("ANDROID_TIME", typeof(DateTime)));

            return gpsRawTable;
        }

        public static DataTable GetAndroidAccRawTable()
        {
            // TODO 並び順をデータベース通りに
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            DataTable accRawTable = new DataTable();

            accRawTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            accRawTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            accRawTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            accRawTable.Columns.Add(new DataColumn("DATETIME", typeof(DateTime)));
            accRawTable.Columns.Add(new DataColumn("ACC_X", typeof(double)));
            accRawTable.Columns.Add(new DataColumn("ACC_Y", typeof(double)));
            accRawTable.Columns.Add(new DataColumn("ACC_Z", typeof(double)));

            return accRawTable;
        }

        public static DataTable GetCorrectedGpsTable()
        {
            DataTable correctedGpsTable = new DataTable();
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            correctedGpsTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            correctedGpsTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            correctedGpsTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            correctedGpsTable.Columns.Add(new DataColumn("JST", typeof(DateTime)));
            correctedGpsTable.Columns.Add(new DataColumn("LATITUDE", typeof(double)));
            correctedGpsTable.Columns.Add(new DataColumn("LONGITUDE", typeof(double)));
            correctedGpsTable.Columns.Add(new DataColumn("SPEED", typeof(double)));
            correctedGpsTable.Columns.Add(new DataColumn("HEADING", typeof(double)));
            correctedGpsTable.Columns.Add(new DataColumn("DISTANCE_DIFFERENCE", typeof(double)));

            return correctedGpsTable;
        }

        public static DataTable GetCorrectedAccTable()
        {
            DataTable correctedAccTable = new DataTable();
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            correctedAccTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            correctedAccTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            correctedAccTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            correctedAccTable.Columns.Add(new DataColumn("DATETIME", typeof(DateTime)));
            correctedAccTable.Columns.Add(new DataColumn("ACC_X", typeof(double)));
            correctedAccTable.Columns.Add(new DataColumn("ACC_Y", typeof(double)));
            correctedAccTable.Columns.Add(new DataColumn("ACC_Z", typeof(double)));

            return correctedAccTable;
        }

        public static DataTable GetTempCorrectedAccTable(DataTable table)
        {
            table.Columns.Add(new DataColumn("ROLL", typeof(double)));
            table.Columns.Add(new DataColumn("PITCH", typeof(double)));
            table.Columns.Add(new DataColumn("YAW", typeof(double)));

            //補正データを記録するためのカラムを追加
            table.Columns.Add(new DataColumn("ALPHA", typeof(double)));
            table.Columns.Add(new DataColumn("VECTOR_X", typeof(double)));
            table.Columns.Add(new DataColumn("VECTOR_Y", typeof(double)));
            table.Columns.Add(new DataColumn("BETA", typeof(double)));
            table.Columns.Add(new DataColumn("GAMMA", typeof(double)));

            return table;
        }

        public static DataTable GetEcologTable()
        {
            var ecologTable = new DataTable();

            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTripId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnDriverId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnCarId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnSensorId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnJst, typeof(DateTime)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLatitude, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLongitude, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnSpeed, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnHeading, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnDistanceDifference, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTerraubAltitude, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTerrainAltitudeDiffarencce, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLongitudinalAcc, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLateralAcc, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnVerticalAcc, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByAirResistance, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByRollingResistance, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByClimbingResistance, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByAccResistance, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConvertLoss, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRegeneLoss, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRegeneEnergy, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLostEnergy, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEfficiency, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedElectricEnergy, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLostEnergyByWellToWheel, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedFuel, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedFuelByWellToWheel, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByEquipment, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByCooling, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByHeating, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTripDirection, typeof(string)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnMeshId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLinkId, typeof(string)));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRoadTheta, typeof(double)));

            return ecologTable;
        }
    }
}
