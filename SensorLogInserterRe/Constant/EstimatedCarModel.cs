using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SensorLogInserterRe.Models;

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
                    return new Car()
                    {
                        Battery = 1000,
                        FrontalProjectedArea = 1000
                    };
                default:
                    return null;
            }

        }

    }
}
