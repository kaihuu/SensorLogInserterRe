using SensorLogInserterRe.Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators.CalculatorComponents
{
    class EfficiencyCalculator
    {
        private static EfficiencyCalculator Instance;

        private int[][] efficiency;

        private EfficiencyCalculator()
        {
            // newできないように
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

            var efficiencyTable = EfficiencyDao.Get();

            // DataTableをint配列に変える操作
            // 最後にefficiencyTable.Clear();を呼ぶこと

        }

        public int GetEfficiency(float speed, float torque)
        {
            // 配列番号用によしなにする
            return 0;
        } 
    }
}
