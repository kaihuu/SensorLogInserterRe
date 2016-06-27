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

        public static DataTable CalcEcolog(DataRow tripRow, InsertDatum datum)
        {
            var correctedGpsTable = CorrectedGpsDao.GetNormalized(tripRow.Field<DateTime>(TripsDao.ColumnStartTime),
                    tripRow.Field<DateTime>(TripsDao.ColumnEndTime), datum);

            var ecologTable = DataTableUtil.GetEcologTable();

            var firstRow = GenerateFirstEcologRow(
                ecologTable.NewRow(), tripRow, correctedGpsTable.Rows[0], datum, 0);

            ecologTable.Rows.Add(firstRow);

            var beforeRow = ecologTable.NewRow();
            beforeRow.ItemArray = firstRow.ItemArray;

            // correctedGpsTable.Rows.Count
            for (int i = 1; i < 20; i++)
            {
                var row = GenerateEcologRow(
                    ecologTable.NewRow(), beforeRow, tripRow, correctedGpsTable.Rows[i], datum, i);
                ecologTable.Rows.Add(row);

                beforeRow.ItemArray = row.ItemArray;
            }

            return ecologTable;
        }

        private static DataRow GenerateFirstEcologRow(DataRow newRow, DataRow tripRow, DataRow correctedGpsRow, InsertDatum datum, int i)
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

            var meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
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

            newRow.SetField(EcologDao.ColumnTripDirection, tripRow.Field<string>(TripsDao.ColumnTripDirection));

            var linkAndTheta = LinkMatcher.GetInstance().MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
                0f, tripRow.Field<string>(TripsDao.ColumnTripDirection), datum, i);

            newRow.SetField(EcologDao.ColumnLinkId, linkAndTheta.Item1);
            newRow.SetField(EcologDao.ColumnRoadTheta, linkAndTheta.Item2);

            return newRow;
        }

        private static DataRow GenerateEcologRow(DataRow newRow, DataRow beforeRow, DataRow tripRow, DataRow correctedGpsRow, InsertDatum datum, int i)
        {
            

            newRow.SetField(EcologDao.ColumnTripId, tripRow.Field<int>(TripsDao.ColumnTripId));
            newRow.SetField(EcologDao.ColumnDriverId, tripRow.Field<int>(TripsDao.ColumnDriverId));
            newRow.SetField(EcologDao.ColumnCarId, tripRow.Field<int>(TripsDao.ColumnCarId));
            newRow.SetField(EcologDao.ColumnSensorId, tripRow.Field<int>(TripsDao.ColumnSensorId));
            newRow.SetField(EcologDao.ColumnJst, correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst));
            newRow.SetField(EcologDao.ColumnLatitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude));
            newRow.SetField(EcologDao.ColumnLongitude, correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            double speed = correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnSpeed);

            Console.WriteLine("SPEED: " + speed);

            newRow.SetField(EcologDao.ColumnSpeed, speed);

            newRow.SetField(EcologDao.ColumnHeading,
                correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnHeading));

            double distanceDiff = correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnDistanceDifference);

            newRow.SetField(EcologDao.ColumnDistanceDifference, distanceDiff);

            var meshAndAltitude = AltitudeCalculator.GetInstance().CalcAltitude(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude));

            newRow.SetField(EcologDao.ColumnTerraubAltitude, meshAndAltitude.Item2);
            newRow.SetField(EcologDao.ColumnMeshId, meshAndAltitude.Item1);

            double terrainAltitudeDiff = meshAndAltitude.Item2 -
                                         beforeRow.Field<Single>(EcologDao.ColumnTerraubAltitude);

            newRow.SetField(EcologDao.ColumnTerrainAltitudeDiffarencce, terrainAltitudeDiff);

            // TODO 加速度ってどれで取るべきだ？Androidの加速度センサの値？
            newRow.SetField(EcologDao.ColumnLongitudinalAcc, 0);
            newRow.SetField(EcologDao.ColumnLateralAcc, 0);
            newRow.SetField(EcologDao.ColumnVerticalAcc, 0);

            double airResistancePower = AirResistanceCalculator.CalcPower(
                Rho, datum.EstimatedCarModel.CdValue, datum.EstimatedCarModel.FrontalProjectedArea, (speed + WindSpeed) / 3.6, speed / 3.6);

            Console.WriteLine("AIR: " + airResistancePower);

            newRow.SetField(
                EcologDao.ColumnEnergyByAirResistance,
                airResistancePower);

            double rollingResistancePower = RollingResistanceCalculator.CalcPower(
                Myu, datum.EstimatedCarModel.Weight, Math.Atan(terrainAltitudeDiff / distanceDiff), speed / 3.6);

            Console.WriteLine("ROLLING: " + rollingResistancePower);

            newRow.SetField(
                EcologDao.ColumnEnergyByRollingResistance,
                rollingResistancePower);

            double climbingResistancePower = ClimbingResistanceCalculator.CalcPower(
                datum.EstimatedCarModel.Weight, Math.Atan(terrainAltitudeDiff / distanceDiff), speed / 3.6);

            Console.WriteLine("CLIMBING: " + climbingResistancePower);

            newRow.SetField(
                EcologDao.ColumnEnergyByClimbingResistance,
                climbingResistancePower);

            double accResistancePower = AccResistanceCalculator.CalcPower(
                beforeRow.Field<Single>(EcologDao.ColumnSpeed),
                speed / 3.6, datum.EstimatedCarModel.Weight,
                (correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst) -
                 beforeRow.Field<DateTime>(EcologDao.ColumnJst)).TotalSeconds);

            Console.WriteLine("ACC: " + accResistancePower);

            newRow.SetField(
                EcologDao.ColumnEnergyByAccResistance,
                accResistancePower);

            double drivingResistancePower =
                airResistancePower + rollingResistancePower + climbingResistancePower + accResistancePower;

            int efficiency = EfficiencyCalculator.GetInstance().GetEfficiency(datum.EstimatedCarModel, speed,
                drivingResistancePower * 1000 * 3600 / speed * 3.6 * datum.EstimatedCarModel.TireRadius /
                datum.EstimatedCarModel.ReductionRatio);

            Console.WriteLine("EFFICIENCY: " + efficiency);

            newRow.SetField(EcologDao.ColumnEfficiency, efficiency);

            newRow.SetField(EcologDao.ColumnConvertLoss, ConvertLossCaluculator.CalcEnergy(
                drivingResistancePower, datum.EstimatedCarModel, speed / 3.6, efficiency));

            Console.WriteLine("CONVERTLOSS: " + ConvertLossCaluculator.CalcEnergy(
                drivingResistancePower, datum.EstimatedCarModel, speed / 3.6, efficiency));

            double regeneEnergy = RegeneEnergyCalculator.CalcEnergy(drivingResistancePower,
                speed / 3.6, datum.EstimatedCarModel, efficiency);

            newRow.SetField(EcologDao.ColumnRegeneLoss, RegeneLossCalculator.CalcEnergy(drivingResistancePower, regeneEnergy, datum.EstimatedCarModel, speed, efficiency));
            newRow.SetField(EcologDao.ColumnRegeneEnergy, regeneEnergy);
            newRow.SetField(EcologDao.ColumnLostEnergy, LostEnergyCalculator.CalcEnergy(drivingResistancePower, datum.EstimatedCarModel, speed, Rho, WindSpeed, Myu,
                Math.Atan(terrainAltitudeDiff / distanceDiff), efficiency));
            
            newRow.SetField(EcologDao.ColumnConsumedElectricEnergy, ConsumedEnergyCaluculator.CalcEnergy(drivingResistancePower, datum.EstimatedCarModel, speed, efficiency));

            newRow.SetField(EcologDao.ColumnLostEnergyByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnConsumedFuelByWellToWheel, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByEquipment,
                EquipmentEnergyCalculator.CalcEquipmentEnergy(correctedGpsRow.Field<DateTime>(CorrectedGpsDao.ColumnJst)));
            newRow.SetField(EcologDao.ColumnEnergyByCooling, DBNull.Value);
            newRow.SetField(EcologDao.ColumnEnergyByHeating, DBNull.Value);

            newRow.SetField(EcologDao.ColumnTripDirection, tripRow.Field<string>(TripsDao.ColumnTripDirection));

            var linkAndTheta = LinkMatcher.GetInstance().MatchLink(
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLatitude),
                correctedGpsRow.Field<double>(CorrectedGpsDao.ColumnLongitude),
                correctedGpsRow.Field<Single>(CorrectedGpsDao.ColumnHeading),
                tripRow.Field<string>(TripsDao.ColumnTripDirection), datum, i);

            newRow.SetField(EcologDao.ColumnLinkId, linkAndTheta.Item1);
            newRow.SetField(EcologDao.ColumnRoadTheta, linkAndTheta.Item2);

            return newRow;
        }
    }
}
