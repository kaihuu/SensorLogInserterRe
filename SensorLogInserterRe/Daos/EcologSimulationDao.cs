using System;
using System.Data;
using System.Text;

namespace SensorLogInserterRe.Daos
{
    class EcologSimulationDao
    {
        public static readonly string TableName = "ecolog_simulation";//シミュレーション用のテーブル　構造はECOLOGテーブルに倣う
        public static readonly string ColumnTripId = "trip_id";
        public static readonly string ColumnDriverId = "driver_id";
        public static readonly string ColumnCarId = "car_id";
        public static readonly string ColumnSensorId = "sensor_id";
        public static readonly string ColumnJst = "jst";
        public static readonly string ColumnLatitude = "latitude";
        public static readonly string ColumnLongitude = "longitude";
        public static readonly string ColumnSpeed = "speed";
        public static readonly string ColumnHeading = "heading";
        public static readonly string ColumnDistanceDifference = "distance_difference";
        public static readonly string ColumnTerraubAltitude = "terrain_altitude";
        public static readonly string ColumnTerrainAltitudeDiffarencce = "terrain_altitude_difference";
        public static readonly string ColumnLongitudinalAcc = "longitudinal_acc";
        public static readonly string ColumnLateralAcc = "lateral_acc";
        public static readonly string ColumnVerticalAcc = "vertical_acc";
        public static readonly string ColumnEnergyByAirResistance = "energy_by_air_resistance";
        public static readonly string ColumnEnergyByRollingResistance = "energy_by_rolling_resistance";
        public static readonly string ColumnEnergyByClimbingResistance = "energy_by_climbing_resistance";
        public static readonly string ColumnEnergyByAccResistance = "energy_by_acc_resistance";
        public static readonly string ColumnConvertLoss = "convert_loss";
        public static readonly string ColumnRegeneLoss = "regene_loss";
        public static readonly string ColumnRegeneEnergy = "regene_energy";
        public static readonly string ColumnLostEnergy = "lost_energy";
        public static readonly string ColumnEfficiency = "efficiency";
        public static readonly string ColumnConsumedElectricEnergy = "consumed_electric_energy";
        public static readonly string ColumnLostEnergyByWellToWheel = "lost_energy_by_well_to_wheel";
        public static readonly string ColumnConsumedFuel = "consumed_fuel";
        public static readonly string ColumnConsumedFuelByWellToWheel = "consumed_fuel_by_well_to_wheel";
        public static readonly string ColumnEnergyByEquipment = "energy_by_equipment";
        public static readonly string ColumnEnergyByCooling = "energy_by_cooling";
        public static readonly string ColumnEnergyByHeating = "energy_by_heating";
        public static readonly string ColumnTripDirection = "trip_direction";
        public static readonly string ColumnMeshId = "mesh_id";
        public static readonly string ColumnLinkId = "link_id";
        public static readonly string ColumnRoadTheta = "road_theta";

        public static void Insert(DataTable dataTable)
        {
            DatabaseAccesser.Insert(EcologSimulationDao.TableName, dataTable);
        }

        public static DataTable Get()
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"FROM " + TableName);
            /*query.AppendLine($"WHERE link_id = 'RB140900511749'"); //シミュレーションの対象とする地点（サグ地点）の道路リンクID（ここから）
            query.AppendLine($" OR link_id = 'RB140900511750'");
            query.AppendLine($" OR link_id = 'RB140900511751'");
            query.AppendLine($" OR link_id = 'RB140900511752'");
            query.AppendLine($" OR link_id = 'RB140900511753'");
            query.AppendLine($" OR link_id = 'RB140900511754'");
            query.AppendLine($" OR link_id = 'RB140900511756'");
            query.AppendLine($" OR link_id = 'RB140900519680'");
            query.AppendLine($" OR link_id = 'RB140900519662'");
            query.AppendLine($" OR link_id = 'RB140900519666'");
            query.AppendLine($" OR link_id = 'RB140900519692'");
            query.AppendLine($" OR link_id = 'RB140900519711'");
            query.AppendLine($" OR link_id = 'RB140900714459'");
            query.AppendLine($" OR link_id = 'RB140900714494'");
            query.AppendLine($" OR link_id = 'RB140900714538'");
            query.AppendLine($" OR link_id = 'RB140900714273'");//（ここまで）*/

            return DatabaseAccesser.GetResult(query.ToString());
        }

        public static DataTable GetSelectedData()
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"FROM " + TableName);
            /*query.AppendLine($"WHERE link_id = 'RB140900511749'"); //シミュレーションの対象とする地点（サグ地点）の道路リンクID（ここから）
            query.AppendLine($" OR link_id = 'RB140900511750'");
            query.AppendLine($" OR link_id = 'RB140900511751'");
            query.AppendLine($" OR link_id = 'RB140900511752'");
            query.AppendLine($" OR link_id = 'RB140900511753'");
            query.AppendLine($" OR link_id = 'RB140900511754'");
            query.AppendLine($" OR link_id = 'RB140900511756'");
            query.AppendLine($" OR link_id = 'RB140900519680'");
            query.AppendLine($" OR link_id = 'RB140900519662'");
            query.AppendLine($" OR link_id = 'RB140900519666'");
            query.AppendLine($" OR link_id = 'RB140900519692'");
            query.AppendLine($" OR link_id = 'RB140900519711'");
            query.AppendLine($" OR link_id = 'RB140900714459'");
            query.AppendLine($" OR link_id = 'RB140900714494'");
            query.AppendLine($" OR link_id = 'RB140900714538'");
            query.AppendLine($" OR link_id = 'RB140900714273'");//（ここまで）*/
            //TODO SQL書き換え
            return DatabaseAccesser.GetResult(query.ToString());
        }

        public static DataTable Get(DateTime startPeriod, DateTime endPeriod)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"FROM " + TableName);
            query.AppendLine($"WHERE jst >= '{startPeriod}'");
            query.AppendLine($" AND jst <= '{endPeriod}'");
            /*query.AppendLine($" AND link_id = 'RB140900511749'");//シミュレーションの対象とする地点（サグ地点）の道路リンクID（ここから）
            query.AppendLine($" OR link_id = 'RB140900511750'");
            query.AppendLine($" OR link_id = 'RB140900511751'");
            query.AppendLine($" OR link_id = 'RB140900511752'");
            query.AppendLine($" OR link_id = 'RB140900511753'");
            query.AppendLine($" OR link_id = 'RB140900511754'");
            query.AppendLine($" OR link_id = 'RB140900511756'");
            query.AppendLine($" OR link_id = 'RB140900519680'");
            query.AppendLine($" OR link_id = 'RB140900519662'");
            query.AppendLine($" OR link_id = 'RB140900519666'");
            query.AppendLine($" OR link_id = 'RB140900519692'");
            query.AppendLine($" OR link_id = 'RB140900519711'");
            query.AppendLine($" OR link_id = 'RB140900714459'");
            query.AppendLine($" OR link_id = 'RB140900714494'");
            query.AppendLine($" OR link_id = 'RB140900714538'");
            query.AppendLine($" OR link_id = 'RB140900714273'");//（ここまで）*/

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
