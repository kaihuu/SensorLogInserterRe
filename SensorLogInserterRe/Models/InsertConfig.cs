using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Models
{
    class InsertConfig
    {
        public enum EstimationModel
        {
            EvEnergyConsumptionModel,
            MachineLearningModel
        }

        public enum GpsCorrection
        {
            MapMatching,
            DeadReckoning
        }

        public List<string> CheckeDrivers { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EstimationModel Model { get; set; }

        public GpsCorrection Correction { get; set; }

        public InsertConfig()
        {
            this.CheckeDrivers = new List<string>();
        }
    }
}
