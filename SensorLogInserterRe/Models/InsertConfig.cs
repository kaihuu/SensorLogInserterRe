using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Models
{
    class InsertConfig
    {
        public enum EstimatedCarModel
        {
            LeafEarlyModel
        }

        public enum EstimationModel
        {
            EvEnergyConsumptionModel,
            MachineLearningModel
        }

        public enum GpsCorrection
        {
            Normal,
            MapMatching,
            DeadReckoning,
            SpeedLPFMapMatching
        }

        public List<string> CheckeDrivers { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EstimationModel EstModel { get; set; }

        public List<GpsCorrection> Correction { get; set; }

        public EstimatedCarModel CarModel { get; set; }

        private InsertConfig()
        {
            this.CheckeDrivers = new List<string>();
            this.Correction = new List<GpsCorrection>();
        }

        public static InsertConfig GetInstance()
        {
            var instance = new InsertConfig();
            return instance;
        }

        public override string ToString()
        {
            var ret = new StringBuilder();

            string drivers = "Driver ID: ";
            this.CheckeDrivers.ForEach(x => { drivers += x + ","; });

            ret.Append(drivers + " ");
            ret.Append("StartDate: " + this.StartDate + " ");
            ret.Append("EndDate: " + this.EndDate + " ");
            ret.Append("EstModel: " + this.EstModel + " ");
            ret.Append("Correction: " + this.Correction + " ");
            ret.Append("CarModel: " + this.CarModel);
            return ret.ToString();
        }
    }
}
