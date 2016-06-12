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

        public static DataTable GetTempCorrectedAccTable(DataTable table)
        {
            // TODO これけっこうやばい、合ってない可能性大

            table.Columns.Add(new DataColumn("ROLL"));
            table.Columns.Add(new DataColumn("PITCH"));
            table.Columns.Add(new DataColumn("YAW"));

            //補正データを記録するためのカラムを追加
            table.Columns.Add(new DataColumn("ALPHA"));
            table.Columns.Add(new DataColumn("VECTOR_X"));
            table.Columns.Add(new DataColumn("VECTOR_Y"));
            table.Columns.Add(new DataColumn("BETA"));
            table.Columns.Add(new DataColumn("GAMMA"));

            return table;
        }

        public static DataTable GetEcologTable()
        {
            var ecologTable = new DataTable();

            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTripId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnDriverId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnCarId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnSensorId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnJst));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLatitude));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLongitude));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnSpeed));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnHeading));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnDistanceDifference));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTerraubAltitude));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTerrainAltitudeDiffarencce));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLongitudinalAcc));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLateralAcc));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnVerticalAcc));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByAirResistance));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByRollingResistance));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByClimbingResistance));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByAccResistance));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTripId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConvertLoss));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRegeneLoss));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRegeneEnergy));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLostEnergy));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEfficiency));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedElectricEnergy));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLostEnergyByWellToWheel));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedFuel));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnConsumedFuelByWellToWheel));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByEquipment));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByCooling));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnEnergyByHeating));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnTripDirection));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnMeshId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnLinkId));
            ecologTable.Columns.Add(new DataColumn(EcologDao.ColumnRoadTheta));

            return ecologTable;
        }
    }
}
