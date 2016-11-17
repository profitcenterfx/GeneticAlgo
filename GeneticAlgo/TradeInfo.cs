using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    public struct TradeInfo
    {
        public int[] variables;

        public float Profit;

        public float OpenSlip;

        public float PointsWithCommision;

        public float Points;

        public float MAE;

        public float MFE;

        public bool isArbitrage;
       
        public DateTime openDate;

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < variables.Length; i++)
            {
                result += variables[i].ToString() + "\t";
            }
            return result;
        }
    }
}
