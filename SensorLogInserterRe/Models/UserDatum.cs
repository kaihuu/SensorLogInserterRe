using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace SensorLogInserterRe.Models
{
    public class UserDatum : NotificationObject
    {
        public int DriverId { get; set; }

        public int CarId { get; set; }

        public int SensorId { get; set; }
    }
}
