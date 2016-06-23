using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Models
{
    class AltitudeDatum
    {
        public double? LowerLatitude { get; set; }
        public double? LowerLongitude { get; set; }
        public double? UpperLatitude { get; set; }
        public double? UpperLongitude { get; set; }
        public float? Altitude { get; set; }
    }
}
