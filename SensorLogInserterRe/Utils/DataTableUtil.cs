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
            tripsRawTable.Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
            tripsRawTable.Columns.Add(new DataColumn("CAR_ID", typeof(int)));
            tripsRawTable.Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
            tripsRawTable.Columns.Add(new DataColumn("START_TIME", typeof(DateTime)));
            tripsRawTable.Columns.Add(new DataColumn("END_TIME", typeof(DateTime)));
            tripsRawTable.Columns.Add(new DataColumn("START_LATITUDE", typeof(double)));
            tripsRawTable.Columns.Add(new DataColumn("START_LONGITUDE", typeof(double)));
            tripsRawTable.Columns.Add(new DataColumn("END_LATITUDE", typeof(double)));
            tripsRawTable.Columns.Add(new DataColumn("END_LONGITUDE", typeof(double)));

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
            tripsTable.Columns.Add(new DataColumn("CONSUMED_ENERGY", typeof(Single)));
            tripsTable.Columns.Add(new DataColumn("TRIP_DIRECTION", typeof(string)));
            tripsTable.Columns.Add(new DataColumn("VALIDATION", typeof(string)));

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

        public static DataTable[] GetAndroidGpsRawTableArray(int n)
        {
            // TODO 並び順をデータベース通りに
            // TODO string 直書きでなく、Daoのstaticフィールドを参照に
            DataTable[] gpsRawTable = new DataTable[n];
            for (int i = 0; i < n; i++)
            {
                gpsRawTable[i] = new DataTable();
                gpsRawTable[i].Columns.Add(new DataColumn("DRIVER_ID", typeof(int)));
                gpsRawTable[i].Columns.Add(new DataColumn("CAR_ID", typeof(int)));
                gpsRawTable[i].Columns.Add(new DataColumn("SENSOR_ID", typeof(int)));
                gpsRawTable[i].Columns.Add(new DataColumn("JST", typeof(DateTime)));
                gpsRawTable[i].Columns.Add(new DataColumn("LATITUDE", typeof(double)));
                gpsRawTable[i].Columns.Add(new DataColumn("LONGITUDE", typeof(double)));
                gpsRawTable[i].Columns.Add(new DataColumn("ALTITUDE", typeof(double)));
                gpsRawTable[i].Columns.Add(new DataColumn("ANDROID_TIME", typeof(DateTime)));
            }
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
            accRawTable.Columns.Add(new DataColumn("ACC_X", typeof(Single)));
            accRawTable.Columns.Add(new DataColumn("ACC_Y", typeof(Single)));
            accRawTable.Columns.Add(new DataColumn("ACC_Z", typeof(Single)));

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
            correctedGpsTable.Columns.Add(new DataColumn("SPEED", typeof(Single)));
            correctedGpsTable.Columns.Add(new DataColumn("HEADING", typeof(Single)));
            correctedGpsTable.Columns.Add(new DataColumn("DISTANCE_DIFFERENCE", typeof(Single)));

            return correctedGpsTable;
        }

        public static DataTable GetTempCorrectedAccTable(DataTable table)
        {
            table.Columns.Add(new DataColumn("ROLL", typeof(Single)));
            table.Columns.Add(new DataColumn("PITCH", typeof(Single)));
            table.Columns.Add(new DataColumn("YAW", typeof(Single)));

            //補正データを記録するためのカラムを追加
            table.Columns.Add(new DataColumn("ALPHA", typeof(Single)));
            table.Columns.Add(new DataColumn("VECTOR_X", typeof(Single)));
            table.Columns.Add(new DataColumn("VECTOR_Y", typeof(Single)));
            table.Columns.Add(new DataColumn("BETA", typeof(Single)));
            table.Columns.Add(new DataColumn("GAMMA", typeof(Single)));

            return table;
        }

        public static DataTable GetEcologTable()
        {
            var ecologTable = new DataTable();

            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnTripId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnDriverId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnCarId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnSensorId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnJst, typeof(DateTime)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLatitude, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLongitude, typeof(double)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnSpeed, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnHeading, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnDistanceDifference, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnTerraubAltitude, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnTerrainAltitudeDiffarencce, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLongitudinalAcc, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLateralAcc, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnVerticalAcc, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByAirResistance, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByRollingResistance, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByClimbingResistance, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByAccResistance, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnConvertLoss, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnRegeneLoss, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnRegeneEnergy, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLostEnergy, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEfficiency, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnConsumedElectricEnergy, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLostEnergyByWellToWheel, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnConsumedFuel, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnConsumedFuelByWellToWheel, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByEquipment, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByCooling, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnEnergyByHeating, typeof(Single)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnTripDirection, typeof(string)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnMeshId, typeof(int)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnLinkId, typeof(string)));
            ecologTable.Columns.Add(new DataColumn(EcologSimulationDao.ColumnRoadTheta, typeof(Single)));

            return ecologTable;
        }
    }
}
