using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class AccResistanceCaluculator
    {
        public static double CalcAccRessistance(double acc,double vehicleMass,double vehicleRollingMass)
        {
            return (vehicleMass+vehicleRollingMass)*acc;
        }
    }
}
