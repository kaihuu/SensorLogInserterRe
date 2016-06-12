using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

namespace SensorLogInserterRe.Models
{
    public class UserDatum : NotificationObject
    {
        public Driver Driver { get; set; }

        public Car Car { get; set; }

        public Sensor Sensor { get; set; }
    }
}
