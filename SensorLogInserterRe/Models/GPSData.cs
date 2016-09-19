using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Models
{
    class GPSData
    {
        public DateTime GPSTime { get; set; }
        public DateTime androidTime { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }
        public double accuracy { get; set; }

        public GPSData(DateTime time1, DateTime time2, double la, double lo, double al, double ac)
        {
            GPSTime = time1;
            androidTime = time2;
            latitude = la;
            longitude = lo;
            altitude = al;
            accuracy = ac;
        }
    }
}
