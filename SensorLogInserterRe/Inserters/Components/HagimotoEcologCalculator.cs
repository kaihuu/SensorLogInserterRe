using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Calculators;
using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;

namespace SensorLogInserterRe.Inserters.Components
{
    class HagimotoEcologCalculator
    {
        public static DataTable CalcEcolog(DataRow tripRow, UserDatum datum)
        {
            var correctedGpsTable = CorrectedGpsDao.Get(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                    tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);

            var ecologTable = DataTableUtil.GetEcologTable();

            var firstRow = GenerateFirstEcologRow(
                ecologTable.NewRow(), tripRow, correctedGpsTable.Rows[0]);

            ecologTable.Rows.Add(firstRow);

            var beforeRow = ecologTable.NewRow();
            beforeRow.ItemArray = firstRow.ItemArray;

            for (int i = 0; i < correctedGpsTable.Rows.Count; i++)
            {
                var row = GenerateEcologRow(
                    ecologTable.NewRow(), beforeRow, tripRow, correctedGpsTable.Rows[i]);
                ecologTable.Rows.Add(row);

                beforeRow.ItemArray = row.ItemArray;
            }

            return ecologTable;
        }

        private static DataRow GenerateFirstEcologRow(DataRow newRow, DataRow tripRow, DataRow correctedGpsRow)
        {
            newRow.SetField(EcologDao.ColumnTripId, tripRow.Field<int>(TripsDao.ColumnTripId));
            newRow.SetField(EcologDao.ColumnDriverId, tripRow.Field<int>(TripsDao.ColumnDriverId));
            newRow.SetField(EcologDao.ColumnCarId, tripRow.Field<int>(TripsDao.ColumnCarId));
            newRow.SetField(EcologDao.ColumnSensorId, tripRow.Field<int>(TripsDao.ColumnSensorId));
            newRow.SetField(EcologDao.ColumnJst, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            newRow.SetField(EcologDao.ColumnLatitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            newRow.SetField(EcologDao.ColumnLongitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));
            newRow.SetField(EcologDao.ColumnSpeed, 0);
            newRow.SetField(EcologDao.ColumnHeading, 0);
            newRow.SetField(EcologDao.ColumnDistanceDifference, 0);

            var meshAndAltitude = AltitudeCalculator.CalcAltitude(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnTerraubAltitude, meshAndAltitude.Item2);
            newRow.SetField(EcologDao.ColumnMeshId, meshAndAltitude.Item1);

            newRow.SetField(EcologDao.ColumnTerrainAltitudeDiffarencce, 0);
            newRow.SetField(EcologDao.ColumnLongitudinalAcc, 0);
            newRow.SetField(EcologDao.ColumnLateralAcc, 0);
            newRow.SetField(EcologDao.ColumnVerticalAcc, 0);
            newRow.SetField(EcologDao.ColumnEnergyByAirResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByRollingResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByClimbingResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByAccResistance, 0);
            newRow.SetField(EcologDao.ColumnConvertLoss, 0);
            newRow.SetField(EcologDao.ColumnRegeneLoss, 0);
            newRow.SetField(EcologDao.ColumnRegeneEnergy, 0);
            newRow.SetField(EcologDao.ColumnLostEnergy, 0);
            newRow.SetField(EcologDao.ColumnEfficiency, 0);
            newRow.SetField(EcologDao.ColumnLongitudinalAcc, 0);
            newRow.SetField(EcologDao.ColumnConsumedElectricEnergy, 0);
            newRow.SetField(EcologDao.ColumnLostEnergyByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuelByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByEquipment,
                EquipmentEnergyCalculator.CalcEquipmentEnergy(correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst)));
            newRow.SetField(EcologDao.ColumnEnergyByCooling, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByHeating, DBNull.Value);

            var linkAndTheta = LinkMatcher.MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnTripDirection, linkAndTheta.Item1);
            newRow.SetField(EcologDao.ColumnRoadTheta, linkAndTheta.Item2);

            return newRow;
        }

        private static DataRow GenerateEcologRow(DataRow newRow, DataRow boforeRow, DataRow tripRow, DataRow correctedGpsRow)
        {
            newRow.SetField(EcologDao.ColumnTripId, tripRow.Field<int>(TripsDao.ColumnTripId));
            newRow.SetField(EcologDao.ColumnDriverId, tripRow.Field<int>(TripsDao.ColumnDriverId));
            newRow.SetField(EcologDao.ColumnCarId, tripRow.Field<int>(TripsDao.ColumnCarId));
            newRow.SetField(EcologDao.ColumnSensorId, tripRow.Field<int>(TripsDao.ColumnSensorId));
            newRow.SetField(EcologDao.ColumnJst, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            newRow.SetField(EcologDao.ColumnLatitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            newRow.SetField(EcologDao.ColumnLongitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnSpeed, SpeedCalculator.CalcSpeed(
                boforeRow.Field<double>(EcologDao.ColumnLatitude),
                boforeRow.Field<double>(EcologDao.ColumnLongitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
                (correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst) - boforeRow.Field<DateTime>(EcologDao.ColumnJst)).TotalSeconds));

            newRow.SetField(EcologDao.ColumnHeading, HeadingCalculator.CalcHeading(
                boforeRow.Field<double>(EcologDao.ColumnLatitude),
                boforeRow.Field<double>(EcologDao.ColumnLongitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude)));

            newRow.SetField(EcologDao.ColumnDistanceDifference, DistanceCalculator.CalcDistance(
                boforeRow.Field<double>(EcologDao.ColumnLatitude),
                boforeRow.Field<double>(EcologDao.ColumnLongitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude)));

            var meshAndAltitude = AltitudeCalculator.CalcAltitude(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnTerraubAltitude, meshAndAltitude.Item2);
            newRow.SetField(EcologDao.ColumnMeshId, meshAndAltitude.Item1);

            newRow.SetField(EcologDao.ColumnTerrainAltitudeDiffarencce, meshAndAltitude.Item2 - boforeRow.Field<double>(EcologDao.ColumnTerraubAltitude));

            // TODO 加速度ってどれで取るべきだ？
            newRow.SetField(EcologDao.ColumnLongitudinalAcc, 0);
            newRow.SetField(EcologDao.ColumnLateralAcc, 0);
            newRow.SetField(EcologDao.ColumnVerticalAcc, 0);

            // ここから
            newRow.SetField(EcologDao.ColumnEnergyByAirResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByRollingResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByClimbingResistance, 0);
            newRow.SetField(EcologDao.ColumnEnergyByAccResistance, 0);
            newRow.SetField(EcologDao.ColumnConvertLoss, 0);
            newRow.SetField(EcologDao.ColumnRegeneLoss, 0);
            newRow.SetField(EcologDao.ColumnRegeneEnergy, 0);
            newRow.SetField(EcologDao.ColumnLostEnergy, 0);
            newRow.SetField(EcologDao.ColumnEfficiency, 0);
            newRow.SetField(EcologDao.ColumnLongitudinalAcc, 0);
            newRow.SetField(EcologDao.ColumnConsumedElectricEnergy, 0);
            newRow.SetField(EcologDao.ColumnLostEnergyByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuelByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByEquipment,
                EquipmentEnergyCalculator.CalcEquipmentEnergy(correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst)));
            newRow.SetField(EcologDao.ColumnEnergyByCooling, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByHeating, DBNull.Value);

            var linkAndTheta = LinkMatcher.MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnTripDirection, linkAndTheta.Item1);
            newRow.SetField(EcologDao.ColumnRoadTheta, linkAndTheta.Item2);

            return newRow;
        }
    }
}
