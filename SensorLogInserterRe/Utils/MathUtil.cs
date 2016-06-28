using SensorLogInserterRe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Utils
{
    static class MathUtil
    {
        public static double ConvertDegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static double ConvertSpeedToRev(Car car, double speed)
        {
            return speed * 60 / (car.TireRadius * 2 * Math.PI) * car.ReductionRatio;
        }

        public static double CalcVectorAbsoluteValue(ThreeDimensionalVector vector)
        {
            return  Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);
        }

        public static Quaternion MultiplyQuaternion(Quaternion a, Quaternion b)
        {
            ThreeDimensionalVector vp = new ThreeDimensionalVector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
            Quaternion ab = new Quaternion(a.T * b.T, a.T * b.X + b.T * a.X + vp.X, a.T * b.Y + b.T * a.Y + vp.Y, a.T * b.Z + b.T * a.Z + vp.Z);

            return ab;
        }
    }
}
