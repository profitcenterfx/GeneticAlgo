using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    public static class GeneMath
    {
        public static float Log(double d)
        {
            if (d <= 0.0)
            {
                throw new ArgumentOutOfRangeException("d", "parameter value must be greater than zero");
            }
            return (float)Math.Log(d);
        }

        public static double Round(double f)
        {
            return (double) Math.Round(f);
        }

        public static double Round(double f, int decimals)
        {
            return (double)Math.Round(f, decimals);
        }

        public static float Exp(double f)
        {
            return (float)Math.Exp(f);
        }

        public static int Floor(double f)
        {
            return (int)Math.Floor(f);
        }

        public static int Ceiling(double f)
        {
            return (int)Math.Ceiling(f);
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt(f);
        }
    }
}
