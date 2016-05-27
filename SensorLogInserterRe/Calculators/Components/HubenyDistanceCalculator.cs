using SensorLogInserterRe.Utils;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Calculators.CalculatorComponents
{
    static class HubenyDistanceCalculator
    {
        private static double LongRadius = 6378137; // 長半径
        private static double ShortRadius = 6356752.314245; // 短半径

        //ヒュベニの公式
        public static double CalcHubenyFormula(double latitudeFirst, double longitudeFirst, double latitudeSecond, double longitudeSecond)
        {
            double differenceLattitude = MathUtil.ConvertDegreeToRadian(latitudeFirst - latitudeSecond); // 緯度の差
            double differenceLongitude = MathUtil.ConvertDegreeToRadian(longitudeFirst - longitudeSecond); // 経度の差

            double M = CalcMeridianCurvature(latitudeFirst, latitudeSecond); // 子午線曲率半径
            double N = CalcPrimeVerticalCircleCurvature(latitudeFirst, latitudeSecond); // 卯酉線曲率半径

            double yFirst = MathUtil.ConvertDegreeToRadian(latitudeFirst); // 緯度をラジアンに変換
            double ySecond = MathUtil.ConvertDegreeToRadian(latitudeSecond); // 緯度をラジアンに変換

            double cosMyuY = Math.Cos((yFirst + ySecond) / 2); // 緯度の平均値のコサイン

            double distance = Math.Sqrt(Math.Pow(differenceLattitude * M, 2) + Math.Pow(differenceLongitude * N * cosMyuY, 2));//距離の計算

            return distance;
        }

        //子午線曲率半径
        private static double CalcMeridianCurvature(double lattitudeFirst, double lattitudeSecond)
        {

            double W = CalcW(lattitudeFirst, lattitudeSecond); // Wを計算
            double e2 = CalcE2(); // 第一離心率の２乗を計算
            double M = LongRadius * (1 - e2) / Math.Pow(W, 3); // 子午線曲率半径を計算

            return M;
        }

        //卯酉線曲率半径
        private static double CalcPrimeVerticalCircleCurvature(double lattitudeFirst, double lattitudeSecond)
        {
            double W = CalcW(lattitudeFirst, lattitudeSecond); // Wを計算
            double N = LongRadius / W; // 卯酉線曲率半径を計算

            return N;
        }

        //ヒュベニの公式のWを導出
        private static double CalcW(double lattitudeFirst, double lattitudeSecond)
        {

            double yFirst = MathUtil.ConvertDegreeToRadian(lattitudeFirst); // 緯度をラジアンに変換
            double ySecond = MathUtil.ConvertDegreeToRadian(lattitudeSecond); // 緯度をラジアンに変換

            double sinMyuY2 = Math.Pow(Math.Sin((yFirst + ySecond) / 2), 2); // 緯度の平均値のサインの２乗
            double e2 = CalcE2(); //第一離心率の２乗

            double W = Math.Sqrt(1 - e2 * sinMyuY2);//Wを計算

            return W;
        }

        //第一離心率の２乗
        private static double CalcE2()
        {
            return (Math.Pow(LongRadius, 2) - Math.Pow(ShortRadius, 2)) / Math.Pow(LongRadius, 2);
        }

    }
}
