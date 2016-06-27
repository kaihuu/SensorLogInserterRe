using SensorLogInserterRe.Daos;
using SensorLogInserterRe.Models;
using SensorLogInserterRe.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators
{
    class EfficiencyCalculator
    {
        private static EfficiencyCalculator _instance;

        private DataTable _efficiencyTable;
        private DataTable _efficiencyMaxTable;
        private int _maxRev;
        private int _maxTorque;
        private int _minTorque;

        private EfficiencyCalculator()
        {
            // for Singleton pattern
        }

        public static EfficiencyCalculator GetInstance()
        {
            if (_instance == null)
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


            _instance._maxRev = _instance._efficiencyTable
                .AsEnumerable()
                .Max(v => v.Field<int>(EfficiencyDao.ColumnRev));

            _instance._maxTorque = (int)_instance._efficiencyTable
                .AsEnumerable()
                .Max(v => v.Field<int>(EfficiencyDao.ColumnTorque));

            _instance._minTorque = (int) _instance._efficiencyTable
                .AsEnumerable()
                .Min(v => v.Field<int>(EfficiencyDao.ColumnTorque));
        }

        public int GetEfficiency(Car car, double speed, double torque)
        {
            //Console.WriteLine($"TORQUE: {torque}");

            double rpm = MathUtil.ConvertSpeedToRev(car, speed);

            DataTable table;

            if(rpm > _maxRev || torque > _maxTorque || torque < _minTorque)
            {
                Console.WriteLine("SCOPE: EfficiencyMaxTable");
                table = this._efficiencyMaxTable;
            }
            else
            {
                Console.WriteLine("SCOPE: EfficiencyTable");
                table = this._efficiencyTable;
            }

            return table.AsEnumerable()
                .Where(v => v.Field<int>(EfficiencyDao.ColumnRev) == (int) Math.Round(rpm / 10) * 10 )
                .Where(v => v.Field<int>(EfficiencyDao.ColumnTorque) == (int) Math.Round(torque) )
                .Select(v => v.Field<int?>(EfficiencyDao.ColumnEfficiency)).FirstOrDefault() ?? 70;
        }
    }
}
