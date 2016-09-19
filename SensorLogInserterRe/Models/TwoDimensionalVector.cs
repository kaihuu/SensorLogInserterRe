using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorLogInserterRe.Models
{
    class TwoDimensionalVector
    {
        public double x { get; set; }
        public double y { get; set; }

        public TwoDimensionalVector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //点Pから線分ABへの最も近い点を探索する
        public static TwoDimensionalVector nearest(TwoDimensionalVector A, TwoDimensionalVector B, TwoDimensionalVector P)
        {
            TwoDimensionalVector a = new TwoDimensionalVector(B.x - A.x, B.y - A.y);
            TwoDimensionalVector b = new TwoDimensionalVector(P.x - A.x, P.y - A.y);
            double r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

            if (r <= 0)
            {
                return A;
            }
            else if (r >= 1)
            {
                return B;
            }
            else
            {
                TwoDimensionalVector result = new TwoDimensionalVector(A.x + r * a.x, A.y + r * a.y);
                return result;
            }
        }

        //線分ABの長さ
        public static double distance(TwoDimensionalVector A, TwoDimensionalVector B)
        {
            return Math.Sqrt((A.x - B.x) * (A.x - B.x) + (A.y - B.y) * (A.y - B.y));
        }
    }
}
