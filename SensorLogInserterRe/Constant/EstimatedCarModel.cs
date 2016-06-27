using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using SensorLogInserterRe.Models;
using Constants = SensorLogInserterRe.Calculators.CalculatorComponents.Constants;

namespace SensorLogInserterRe.Constant
{
    class EstimatedCarModel
    {
        public static Car GetModel(InsertConfig.EstimatedCarModel carModel)
        {
            switch (carModel)
            {
                case InsertConfig.EstimatedCarModel.LeafEarlyModel:
                    // TODO LEAF前期型のスペックに
                    return new Car
                    {
                        Battery = 24,
                        Weight = 1600,
                        TireRadius = 0.3155f,
                        ReductionRatio = 7.9377f,
                        CdValue = 0.28f,
                        FrontalProjectedArea = 2.19f,
                        InverterEfficiency = 0.95,
                        MaxDrivingPower = -30,
                        MaxDrivingForce = -0.15 * Constants.GravityResistanceCoefficient * 1600
                    };
                default:
                    return null;
            }

        }

    }
}
