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
        private static EfficiencyCalculator _instance;

        private DataTable _efficiencyTable;
        private DataTable _efficiencyMaxTable;
        private double _maxRev;
        private double _maxTorque;

        private EfficiencyCalculator()
        {
            // for Singleton pattern
        }

        public static EfficiencyCalculator GetInstance()
        {
            if(_instance == null)
            {
                InitInstance();
            }

            return _instance;
        }

        private static void InitInstance()
        {
            _instance = new EfficiencyCalculator
            {
                _efficiencyTable = EfficiencyDao.Get(),
                _efficiencyMaxTable = EfficiencyMaxDao.Get()
            };


            _instance._maxRev = (double) _instance._efficiencyTable
                .AsEnumerable()
                .Max(v => v[EfficiencyDao.ColumnRev]);

            _instance._maxTorque = (double)_instance._efficiencyTable
                .AsEnumerable()
                .Max(v => v[EfficiencyDao.ColumnTorque]);
        }

        public int GetEfficiency(Car car, float speed, float torque)
        {
            double rpm = MathUtil.ConvertSpeedToRev(car, speed);

            DataTable table;

            if(rpm > _maxRev || torque > _maxTorque)
            {
                table = this._efficiencyMaxTable;
            }
            else
            {
                table = this._efficiencyTable;
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
