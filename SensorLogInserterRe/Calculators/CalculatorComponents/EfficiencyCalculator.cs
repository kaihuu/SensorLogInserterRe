using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators.CalculatorComponents
{
    class EfficiencyCalculator
    {
        private static EfficiencyCalculator Instance;

        private DataTable efficiencyTable;
        private DataTable efficiencyMaxTable;
        private double maxRev;
        private double maxTorque;

        private EfficiencyCalculator()
        {
            // for Singleton pattern
        }

        public static EfficiencyCalculator GetInstance()
        {
            if(Instance == null)
            {
                initInstance();
            }

            return Instance;
        }

        private static void initInstance()
        {
            Instance = new EfficiencyCalculator();

            Instance.efficiencyTable = EfficiencyDao.Get();
            Instance.efficiencyMaxTable = EfficiencyDao.GetMax();

            Instance.maxRev = (double) Instance.efficiencyTable
                .AsEnumerable()
                .Max(v => v[EfficiencyDao.ColumnRev]);

            Instance.maxTorque = (double)Instance.efficiencyTable
                .AsEnumerable()
                .Max(v => v[EfficiencyDao.ColumnTorque]);
        }

        public int GetEfficiency(Car car, float speed, float torque)
        {
            double rpm = MathUtil.ConvertSpeedToRpm(car, speed);

            DataTable table;

            if(rpm > maxRev || torque > maxTorque)
            {
                table = this.efficiencyMaxTable;
            }
            else
            {
                table = this.efficiencyTable;
            }

            return table.AsEnumerable()
                   .Where(v => (int)v[EfficiencyDao.ColumnRev] == (int)Math.Round(rpm / 10) * 10)
                   .Where(v => (int)v[EfficiencyDao.ColumnTorque] == (int)Math.Round(torque))
                   .Select(v => v[EfficiencyDao.ColumnEfficiency])
                   .Cast<int>()
                   .ElementAt(0);
        } 
    }
}
