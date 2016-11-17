using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    public interface IOptimizer
    {
        int OptimizeSymbol();

        float[] Steps { get; set; }

        int[] CountDecimal { get; set; }

        int[] ParametersRoundDigits { get; set; }

        float minOpSl { get; set; }

        float minOpSlGlobal { get; set; }

        int daysCount { get; set; }

        int daysCountWithPrediction { get; set; }

        int OpenSlipIndex { get; set; }

        void SetTrades(TradeInfo[] trades);

        void SetTradesCheck(TradeInfo[] trades);

        void SetTradesPrediction(TradeInfo[] trades);

        void SetTradesFull(TradeInfo[] trades);

        FitnessFunction FitnessFunction { get; set; }

        FitnessFunctionPrediction FitnessFunctionPrediction { get; set; }

        void CalcProfitSizeArb(int i, int countVariable, long[,,] numericGen, long[,] parameters,
            ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] arbPc, ref int countArb);

        void CalcProfitSizeArbPrediction(int i, int countVariable, long[,,] numericGen, long[,] parameters,
            ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] fitFunctionPredict,
            ref float[] arbPc, ref int countArb);

        void CalcProfitSizeArbStDev(int i, int countVariable, long[,,] numericGen, long[,] parameters,
            ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] arbPc, ref int countArb);

        void CalcProfitSizeArbStDevPrediction(int i, int countVariable, long[,,] numericGen, long[,] parameters,
            ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] fitFunctionPredict,
            ref float[] arbPc, ref int countArb);
    }
}