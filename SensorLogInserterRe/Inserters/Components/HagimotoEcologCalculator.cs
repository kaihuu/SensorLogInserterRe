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
        private static readonly double WindSpeed = 0;
        private static readonly double Rho = 1.22;
        private static readonly double Myu = 0.015;

        public static DataTable CalcEcolog(DataRow tripRow, InsertDatum datum, InsertConfig.GpsCorrection correction)
        {
            var correctedGpsTable = new DataTable();
            if (correction == InsertConfig.GpsCorrection.SpeedLPFMapMatching) //補正GPS取得元変更
            {
                correctedGpsTable = CorrectedGpsSpeedLPF005MMDao.GetNormalized(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);
            }
            else if (correction == InsertConfig.GpsCorrection.MapMatching)
            {
                correctedGpsTable = CorrectedGPSMMDao.GetNormalized(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);
            }
            else
            {
                correctedGpsTable = CorrectedGpsDao.GetNormalized(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                        tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);
            }


            var ecologTable = DataTableUtil.GetEcologTable();
            if (correctedGpsTable.Rows.Count == 0)
            {
                return ecologTable;
            }
            var firstRow = GenerateFirstEcologRow(
                ecologTable.NewRow(), tripRow, correctedGpsTable.Rows[0], datum);

            ecologTable.Rows.Add(firstRow);

            var beforeRow = ecologTable.NewRow();
            beforeRow.ItemArray = firstRow.ItemArray;

            for (int i = 1; i < correctedGpsTable.Rows.Count; i++)
            {
                var row = GenerateEcologRow(
                    ecologTable.NewRow(), beforeRow, tripRow, correctedGpsTable.Rows[i], datum);

                ecologTable.Rows.Add(row);

                beforeRow.ItemArray = row.ItemArray;
            }

            return ecologTable;
        }

        private static DataRow GenerateFirstEcologRow(DataRow newRow, DataRow tripRow, DataRow correctedGpsRow, InsertDatum datum)
        {
            newRow.SetField(EcologSimulationDao.ColumnTripId, tripRow.Field<int>(TripsDao.ColumnTripId));
            newRow.SetField(EcologSimulationDao.ColumnDriverId, tripRow.Field<int>(TripsDao.ColumnDriverId));
            newRow.SetField(EcologSimulationDao.ColumnCarId, tripRow.Field<int>(TripsDao.ColumnCarId));
            newRow.SetField(EcologSimulationDao.ColumnSensorId, tripRow.Field<int>(TripsDao.ColumnSensorId));
            newRow.SetField(EcologSimulationDao.ColumnJst, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            newRow.SetField(EcologSimulationDao.ColumnLatitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            newRow.SetField(EcologSimulationDao.ColumnLongitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));
            newRow.SetField(EcologSimulationDao.ColumnSpeed, 0);
            newRow.SetField(EcologSimulationDao.ColumnHeading, 0);
            newRow.SetField(EcologSimulationDao.ColumnDistanceDifference, 0);

            var meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologSimulationDao.ColumnTerraubAltitude, meshAndAltitude.Item2);
            newRow.SetField(EcologSimulationDao.ColumnMeshId, meshAndAltitude.Item1);

            newRow.SetField(EcologSimulationDao.ColumnTerrainAltitudeDiffarencce, 0);

            // TODO 加速度を挿入する場合はここへ
            newRow.SetField(EcologSimulationDao.ColumnLongitudinalAcc, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnLateralAcc, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnVerticalAcc, DBNull.Value);

            newRow.SetField(EcologSimulationDao.ColumnEnergyByAirResistance, 0);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByRollingResistance, 0);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByClimbingResistance, 0);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByAccResistance, 0);
            newRow.SetField(EcologSimulationDao.ColumnConvertLoss, 0);
            newRow.SetField(EcologSimulationDao.ColumnRegeneLoss, 0);
            newRow.SetField(EcologSimulationDao.ColumnRegeneEnergy, 0);
            newRow.SetField(EcologSimulationDao.ColumnLostEnergy, 0);
            newRow.SetField(EcologSimulationDao.ColumnEfficiency, 0);
            newRow.SetField(EcologSimulationDao.ColumnConsumedElectricEnergy, 0);
            newRow.SetField(EcologSimulationDao.ColumnLostEnergyByWellToWheel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnConsumedFuel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnConsumedFuelByWellToWheel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByEquipment,
                EquipmentEnergyCalculator.CalcEquipmentEnergy(correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst)));
            newRow.SetField(EcologSimulationDao.ColumnEnergyByCooling, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByHeating, DBNull.Value);

            newRow.SetField(EcologSimulationDao.ColumnTripDirection, tripRow.Field<string>(TripsDao.ColumnTripDirection));

            var linkAndTheta = LinkMatcher.GetInstance().MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
                0f, tripRow.Field<string>(TripsDao.ColumnTripDirection), datum);
            if (linkAndTheta.Item1 == null)
            {
                newRow.SetField(EcologSimulationDao.ColumnLinkId, DBNull.Value);
                newRow.SetField(EcologSimulationDao.ColumnRoadTheta, DBNull.Value);
            }

            else
            {
                newRow.SetField(EcologSimulationDao.ColumnLinkId, linkAndTheta.Item1);
                newRow.SetField(EcologSimulationDao.ColumnRoadTheta, linkAndTheta.Item2);
            }

            return newRow;
        }

        private static DataRow GenerateEcologRow(DataRow newRow, DataRow beforeRow, DataRow tripRow, DataRow correctedGpsRow, InsertDatum datum)
        {
            newRow.SetField(EcologSimulationDao.ColumnTripId, tripRow.Field<int>(TripsDao.ColumnTripId));
            newRow.SetField(EcologSimulationDao.ColumnDriverId, tripRow.Field<int>(TripsDao.ColumnDriverId));
            newRow.SetField(EcologSimulationDao.ColumnCarId, tripRow.Field<int>(TripsDao.ColumnCarId));
            newRow.SetField(EcologSimulationDao.ColumnSensorId, tripRow.Field<int>(TripsDao.ColumnSensorId));
            newRow.SetField(EcologSimulationDao.ColumnJst, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            newRow.SetField(EcologSimulationDao.ColumnLatitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            newRow.SetField(EcologSimulationDao.ColumnLongitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            double speed = correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnSpeed);
            double speedMeterPerSec = speed / 3.6;
            //Console.WriteLine("SPEED: " + speed);

            newRow.SetField(EcologSimulationDao.ColumnSpeed, speed);

            newRow.SetField(EcologSimulationDao.ColumnHeading,
                correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnHeading));

            double distanceDiff = correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnDistanceDifference);

            //Console.WriteLine("DISTANCE_DIFF: " + distanceDiff);

            newRow.SetField(EcologSimulationDao.ColumnDistanceDifference, distanceDiff);

            var meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologSimulationDao.ColumnTerraubAltitude, meshAndAltitude.Item2);
            newRow.SetField(EcologSimulationDao.ColumnMeshId, meshAndAltitude.Item1);

            double terrainAltitudeDiff = meshAndAltitude.Item2 -
                                         beforeRow.Field<Single>(EcologSimulationDao.ColumnTerraubAltitude);

            //Console.WriteLine("ALTITUDE_DIFF: " + terrainAltitudeDiff);

            newRow.SetField(EcologSimulationDao.ColumnTerrainAltitudeDiffarencce, terrainAltitudeDiff);

            // TODO 加速度を追加する場合はここへ
            newRow.SetField(EcologSimulationDao.ColumnLongitudinalAcc, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnLateralAcc, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnVerticalAcc, DBNull.Value);

            double airResistancePower = 0;
            if (speed > 1 && distanceDiff > 0)
                airResistancePower = AirResistanceCalculator.CalcPower(
                    Rho, datum.EstimatedCarModel.CdValue, datum.EstimatedCarModel.FrontalProjectedArea, (speed + WindSpeed) / 3.6, speedMeterPerSec);

            //Console.WriteLine("AIR: " + airResistancePower);

            newRow.SetField(
                EcologSimulationDao.ColumnEnergyByAirResistance,
                airResistancePower);

            double rollingResistancePower = 0;
            if (speed > 1 && distanceDiff > 0)
                rollingResistancePower = RollingResistanceCalculator.CalcPower(
                    Myu, datum.EstimatedCarModel.Weight, Math.Atan(terrainAltitudeDiff / distanceDiff), speedMeterPerSec);

            //Console.WriteLine("ROLLING: " + rollingResistancePower);

            newRow.SetField(
                EcologSimulationDao.ColumnEnergyByRollingResistance,
                rollingResistancePower);

            double climbingResistancePower = 0;
            if (speed > 1 && distanceDiff > 0)
                climbingResistancePower = ClimbingResistanceCalculator.CalcPower(
                    datum.EstimatedCarModel.Weight, Math.Atan(terrainAltitudeDiff / distanceDiff), speedMeterPerSec);

            //Console.WriteLine("CLIMBING: " + climbingResistancePower);

            newRow.SetField(
                EcologSimulationDao.ColumnEnergyByClimbingResistance,
                climbingResistancePower);

            double accResistancePower = 0;
            if (speed > 1 && distanceDiff > 0)
                accResistancePower = AccResistanceCalculator.CalcPower(
                    beforeRow.Field<Single>(EcologSimulationDao.ColumnSpeed) / 3.6,
                    beforeRow.Field<DateTime>(EcologSimulationDao.ColumnJst),
                    speedMeterPerSec, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst),
                    datum.EstimatedCarModel.Weight);

            //Console.WriteLine("ACC: " + accResistancePower);

            newRow.SetField(
                EcologSimulationDao.ColumnEnergyByAccResistance,
                accResistancePower);

            double drivingResistancePower =
                airResistancePower + rollingResistancePower + climbingResistancePower + accResistancePower;

            double torque = 0;
            if (drivingResistancePower > 0 && speed > 0)
                torque = drivingResistancePower * 1000 * 3600 / speedMeterPerSec * datum.EstimatedCarModel.TireRadius /
                         datum.EstimatedCarModel.ReductionRatio;

            int efficiency = EfficiencyCalculator.GetInstance().GetEfficiency(datum.EstimatedCarModel, speedMeterPerSec, torque);

            //Console.WriteLine("EFFICIENCY: " + efficiency);

            newRow.SetField(EcologSimulationDao.ColumnEfficiency, efficiency);

            double convertLoss = ConvertLossCaluculator.CalcEnergy(
                drivingResistancePower, datum.EstimatedCarModel, speedMeterPerSec, efficiency);

            newRow.SetField(EcologSimulationDao.ColumnConvertLoss, convertLoss);

            //Console.WriteLine("CONVERTLOSS: " + convertLoss);

            double regeneEnergy = RegeneEnergyCalculator.CalcEnergy(drivingResistancePower,
                speedMeterPerSec, datum.EstimatedCarModel, efficiency);

            newRow.SetField(EcologSimulationDao.ColumnRegeneEnergy, regeneEnergy);

            double regeneLoss = RegeneLossCalculator.CalcEnergy(drivingResistancePower, regeneEnergy,
                datum.EstimatedCarModel, speedMeterPerSec, efficiency);

            newRow.SetField(EcologSimulationDao.ColumnRegeneLoss, regeneLoss);

            double lostEnergy = LostEnergyCalculator.CalcEnergy(convertLoss, regeneLoss, airResistancePower,
                rollingResistancePower);

            newRow.SetField(EcologSimulationDao.ColumnLostEnergy, lostEnergy);

            newRow.SetField(EcologSimulationDao.ColumnConsumedElectricEnergy, ConsumedEnergyCaluculator.CalcEnergy(drivingResistancePower, datum.EstimatedCarModel, speedMeterPerSec, efficiency));

            newRow.SetField(EcologSimulationDao.ColumnLostEnergyByWellToWheel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnConsumedFuel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnConsumedFuelByWellToWheel, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByEquipment,
                EquipmentEnergyCalculator.CalcEquipmentEnergy(correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst)));
            newRow.SetField(EcologSimulationDao.ColumnEnergyByCooling, DBNull.Value);
            newRow.SetField(EcologSimulationDao.ColumnEnergyByHeating, DBNull.Value);

            newRow.SetField(EcologSimulationDao.ColumnTripDirection, tripRow.Field<string>(TripsDao.ColumnTripDirection));

            var linkAndTheta = LinkMatcher.GetInstance().MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
                correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnHeading),
                tripRow.Field<string>(TripsDao.ColumnTripDirection), datum);

            if (linkAndTheta.Item1 == null)
            {
                newRow.SetField(EcologSimulationDao.ColumnLinkId, DBNull.Value);
                newRow.SetField(EcologSimulationDao.ColumnRoadTheta, DBNull.Value);
            }

            else
            {
                newRow.SetField(EcologSimulationDao.ColumnLinkId, linkAndTheta.Item1);
                newRow.SetField(EcologSimulationDao.ColumnRoadTheta, linkAndTheta.Item2);
            }

            return newRow;
        }
    }
}
