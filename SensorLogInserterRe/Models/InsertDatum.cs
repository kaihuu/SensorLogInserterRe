using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace SensorLogInserterRe.Models
{
    public class InsertDatum : NotificationObject
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int DriverId { get; set; }

        public int CarId { get; set; }

        public int SensorId { get; set; }

        public Car EstimatedCarModel { get; set; }

        public static void AddDatumToList(List<InsertDatum> list, InsertDatum datum)
        {
            int count = list.Count(v => v.DriverId == datum.DriverId && v.CarId == datum.CarId && v.SensorId == datum.SensorId);

            if(count == 0)
                list.Add(datum);
        }

        public override string ToString()
        {
            return $"START_TIME:{StartTime}, END_TIME:{EndTime}, DRIVER_ID:{DriverId}, CAR_ID:{CarId}, SENSOR_ID:{SensorId}";
        }
    }
}
