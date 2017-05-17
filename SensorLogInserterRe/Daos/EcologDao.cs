using ECOLOGSemanticViewer.Models.EcologModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Daos
{
    class EcologDao
    {
        public static readonly string TableName = "ecolog_links_lookup2";
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
            DatabaseAccesser.Insert(EcologDao.TableName, dataTable);
        }

        public static DataTable Get()
        {
            string query = "SELECT * FROM " + TableName;

            return DatabaseAccesser.GetResult(query);
        }

        public static DataTable Get(DateTime startPeriod, DateTime endPeriod)
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine($"SELECT *");
            query.AppendLine($"FROM " + TableName);
            query.AppendLine($"WHERE jst >= '{startPeriod}'");
            query.AppendLine($" AND jst <= '{endPeriod}'");

            return DatabaseAccesser.GetResult(query.ToString());
        }
    }
}
