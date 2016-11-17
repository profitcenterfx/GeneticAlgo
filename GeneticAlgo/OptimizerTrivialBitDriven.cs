using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeneticAlgo
{
    public enum FilterBoundState : int
    {
        Uncompressed = 0,
        Compressed = 1
    }

    public struct FiltersState
    {
        public FilterBoundState Lower;

        public FilterBoundState Upper;
    }

    public class BestIndividuum
    {
        public float MaxFitFuncValue;

        public int TradesCount;

        public float ArbitragePercent;

        public float Mutation;

        public float Indicator;

        public float MaxFitFunctionPredictionValue;

        public int[,] Indexes;

        public long[,] Values;

        public BestIndividuum(int varCount, float maxFitFuncValue, int tradesCount, float arbitragePercent, float mutation, float indicator, float maxFitFuncPredictionValue)
        {
            MaxFitFuncValue = maxFitFuncValue;
            TradesCount = tradesCount;
            ArbitragePercent = arbitragePercent;
            Mutation = mutation;
            Indicator = indicator;
            MaxFitFunctionPredictionValue = maxFitFuncPredictionValue;
            Indexes = new int[varCount,2];
            Values  = new long[varCount, 2];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"{MaxFitFuncValue}\t{TradesCount}\t{ArbitragePercent}");

            for (var varIndex = 0; varIndex < Values.GetLength(0); varIndex++)
            {
                sb.Append($"\t{Values[varIndex, 0]}\t{Values[varIndex, 1]}");
            }

            sb.Append($"\t{Mutation}\t{Indicator}\t{MaxFitFunctionPredictionValue}");
            return sb.ToString();
        }
    }

    public class OptimizerTrivialBitDriven
    {
        private int[] ind;

        private Filters filters;

        private int[][] initialPos;

        public TradeInfo[] Trades;

        public TradeInfo[] TradesCheck;
        public TradeInfo[] TradesPrediction;

        public TradeInfo[] TradesFull;

        private int inflectiveNumber;

        private double LogN2 = GeneMath.Log(2);

        private string decim = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        private Random random;
        private int[] size;
        private int[] arbSize;
        private float[] range;
        private float[] sumProfit;
        private float[] arbPc;
        private float[] arbProfit;
        private float[] fitFunction;
        private float[] fitFunctionPredict;
        private float sumCorrect2;
        private float minCorrect2;
        private int maxSize;
        private int minSize;
        private float minSp;
        private float minFitFunc;
        private float maxFitFunc;
        private float maxFitFuncPrediction;
        int maxCPF;
        float maxSP;

        private int countTrades;
        private int countArb = 0;

        public float IndentSL { get; set; }
        public float TslSL { get; set; }
        public float Commision { get; set; }

        public float[] Steps { get; set; }

        public int[] CountDecimal { get; set; }
        public int[] ParametersRoundDigits { get; set; }

        // индекс особь с максимальной прибылью
        private int globalIndex = 0;

        //текущее максимальное значение валовой прибыли среди всех генераций
        private float globalMax = 0;

        public float minOpSl { get; set; }

        public float minOpSlGlobal { get; set; }

        private AutoResetEvent taskCompletedEvent = new AutoResetEvent(false);

        public Parameters Parameters;

        public string Symbol { get; private set; }

        public string AgrEx { get; private set; }

        private int[] lowerBounds;

        private int[] upperBounds;

        public OptimizerTrivialBitDriven(Parameters parameters, string symbol, string agrEx, int[][] initialPosition,
            Filters steps, int inflectiveNumber)
        {
            Parameters = parameters;
            Symbol = symbol;
            AgrEx = agrEx;
            initialPos = initialPosition;
            this.filters = steps;
            this.inflectiveNumber = inflectiveNumber;
        }

        public void SetTrades(TradeInfo[] trades)
        {
            Trades = trades;
        }

        public void SetTradesCheck(TradeInfo[] trades)
        {
            TradesCheck = trades;
        }

        public void SetTradesPrediction(TradeInfo[] trades)
        {
            TradesPrediction = trades;
        }

        public void SetTradesFull(TradeInfo[] trades)
        {
            TradesFull = trades;
        }

        public void qSort2(ref float[] ar, int low, int high)
        {
            int i, j, msp;
            float m, wsp;

            i = low;
            j = high;
            m = ar[(i + j) / 2];

            while (i < j)
            {
                while (ar[i] > m) i++; //Инкремент i. Эквивалентно коду i:=i+1, но компилируется оптимальнее
                while (ar[j] < m) j--; //Декремент j. Эквивалентно коду j:=j-1;
                if (i <= j)
                {
                    wsp = ar[i];
                    msp = ind[i];
                    ar[i] = ar[j];
                    ind[i] = ind[j];
                    ar[j] = wsp;
                    ind[j] = msp;
                    i++;
                    j--;
                }
            }

            if (low < j) qSort2(ref ar, low, j);
            if (i < high) qSort2(ref ar, i, high);
        }

        public void InitDirectories()
        {
            string dir = Environment.CurrentDirectory;
            Directory.CreateDirectory(dir + "/Recomends/log/" + Symbol);
            Directory.CreateDirectory(dir + "/Recomends/" + Symbol);
            FileStream fs = File.Create(dir + "/Recomends/log/" + Symbol + "/" + AgrEx + ".txt");
            fs.Close();
            fs = File.Create(dir + "/Recomends/" + Symbol + "/" + AgrEx + ".txt");
            fs.Close();
        }

        public int[] InitRealVarIndexes()
        {
            List<int> fGenInd = new List<int>();
            //определяет параметры, которые будут постоянно изменяться при формировании стартовой популяции
            for (int j = 0; j < filters.Count; j++)
                if (filters[j].FirstPopulation)
                    fGenInd.Add(j);

            //определяет параметры, которые будут изменятся как 1 и чередоваться
            for (int j = 0; j < filters.Count; j++)
                if (!filters[j].FirstPopulation)
                    fGenInd.Add(j);

            return fGenInd.ToArray();
        }

        private long[] variableMinValue;

        private long[] variableMaxValue;

        private long[] variableLowerLimit;

        private long[] variableUpperLimit;

        private int[] variableLowerBitsCount;

        private int[] variableUpperBitsCount;

        private int[] variableLowerValuesCount;

        private int[] variableUpperValuesCount;

        private int[] variableValuesCount;

        private void InitParametersAndLoadFeedData()
        {
            var varCount = filters.Count;
            aggregated = GetAggregatedData($"{Symbol}_{AgrEx}");

            variableValuesCount      = new int[varCount];  // total variable unique values count
            variableMinValue         = new long[varCount]; // minimal variables values (absolute)
            variableMaxValue         = new long[varCount]; // maximal variables values (absolute)
            variableLowerLimit       = new long[varCount]; // variables lower limit (absolute)
            variableUpperLimit       = new long[varCount]; // variables upper limit (absolute)
            variableLowerValuesCount = new int[varCount];  // lower hromosome unique values count
            variableUpperValuesCount = new int[varCount];  // upper hromosome unique values count
            variableLowerBitsCount   = new int[varCount];  // variable lower hromosome bit count
            variableUpperBitsCount   = new int[varCount];  // variable upper hromosome bit count

            for (int varIndex = 0; varIndex < varCount; varIndex++)
            {
                var varAgr = aggregated[varIndex];
                var varValuesAligned = varAgr.Values.ToArray();

                variableValuesCount[varIndex] = varAgr.Count;
                variableMinValue[varIndex]    = varAgr.Values.Min();
                variableMaxValue[varIndex]    = varAgr.Values.Max();

                var minLimit = filters[varIndex].MinLimit;
                var maxLimit = filters[varIndex].MaxLimit;

                if (minLimit == "true")
                    variableLowerLimit[varIndex] = variableMaxValue[varIndex];
                else if (minLimit == "false")
                    variableLowerLimit[varIndex] = variableMinValue[varIndex];
                else
                    variableLowerLimit[varIndex] = filters[varIndex].MinLimitValue > variableMaxValue[varIndex] ? variableMaxValue[varIndex] : filters[varIndex].MinLimitValue;

                if (maxLimit == "true")
                    variableUpperLimit[varIndex] = variableMinValue[varIndex];
                else if (maxLimit == "false")
                    variableUpperLimit[varIndex] = variableMaxValue[varIndex];
                else
                    variableUpperLimit[varIndex] = filters[varIndex].MaxLimitValue < variableMinValue[varIndex] ? variableMinValue[varIndex] : filters[varIndex].MaxLimitValue;

                variableLowerValuesCount[varIndex] = 1;
                for (int valueIndex = 0; valueIndex < variableValuesCount[varIndex]; valueIndex++)
                    if (varValuesAligned[valueIndex] < variableLowerLimit[varIndex])
                        variableLowerValuesCount[varIndex]++;
                    else
                        break;

                variableUpperValuesCount[varIndex] = 1;
                for (int valueIndex = variableValuesCount[varIndex] - 1; valueIndex >= 0; valueIndex--)
                    if (varValuesAligned[valueIndex] > variableUpperLimit[varIndex])
                        variableUpperValuesCount[varIndex]++;
                    else
                        break;

                variableLowerBitsCount[varIndex] = GetStorageBitCount(variableLowerValuesCount[varIndex] - 2);
                variableUpperBitsCount[varIndex] = GetStorageBitCount(variableUpperValuesCount[varIndex] - 2);
            }
        }

        /// <summary>
        /// Calculates bit count needed to store specified quantity of values
        /// </summary>
        /// <param name="uniqueValuesCount">Quantity of values to encode</param>
        /// <returns>Bit count</returns>
        private int GetStorageBitCount(int uniqueValuesCount)
        {
            if (uniqueValuesCount <= 0)
                return 0;

            var n = uniqueValuesCount;
            var result = 0;
            while(n > 0)
            {
                n = n >> 1;
                result++;
            }
            return result;
        }

        private int[,,] GetInitialPopulationGens(int inflectiveNumber, int varCount, int[] inflectiveVarsIndexes, int slidingVarIndex, int[] upperUncompressed, int[] upperCompressed, int[] lowerCompressed)
        {
            var lastInflectiveVarIndex = inflectiveNumber - 2;
            var population = initialPos.GetLength(0);
            var result = new int[population, varCount, 2];
            var individuumIndex = 0;

            for (int i = 0; i < population; i++)
            {
                var isBreakedByLimits = false;
                for (int varIndex = 0; varIndex < lastInflectiveVarIndex; varIndex++)
                {
                    var realVarIndex = inflectiveVarsIndexes[varIndex];
                    var lower = initialPos[i][varIndex * 2];
                    var upper = initialPos[i][varIndex * 2 + 1];

                    if ((variableLowerLimit[realVarIndex] == variableMinValue[realVarIndex] && lower != 0) || (variableUpperLimit[realVarIndex] == variableMaxValue[realVarIndex] && upper != 0))
                    {
                        isBreakedByLimits = true;
                        break;
                    }

                    result[individuumIndex, realVarIndex, 0] = lower == 0 ? 0 : lowerCompressed[realVarIndex];
                    result[individuumIndex, realVarIndex, 1] = upper == 0 ? upperUncompressed[realVarIndex] : upperCompressed[realVarIndex];
                }

                if (isBreakedByLimits)
                    continue;

                // all another variables
                for (int varIndex = lastInflectiveVarIndex; varIndex < varCount; varIndex++)
                {
                    var realVarIndex = inflectiveVarsIndexes[varIndex];
                    var lower = initialPos[i][(lastInflectiveVarIndex + 1) * 2];
                    var upper = initialPos[i][(lastInflectiveVarIndex + 1) * 2 + 1];
                    result[individuumIndex, realVarIndex, 0] = lower == 0 ? 0 : lowerCompressed[realVarIndex];
                    result[individuumIndex, realVarIndex, 1] = upper == 0 ? upperUncompressed[realVarIndex] : upperCompressed[realVarIndex];
                }
                // sliding variable
                var realSlidingVarIndex = inflectiveVarsIndexes[lastInflectiveVarIndex + slidingVarIndex - 1];
                var slidingLower = initialPos[i][lastInflectiveVarIndex * 2];
                var slidingUpper = initialPos[i][lastInflectiveVarIndex * 2 + 1];

                if ((variableLowerLimit[realSlidingVarIndex] == variableMinValue[realSlidingVarIndex] && slidingLower != 0) || (variableUpperLimit[realSlidingVarIndex] == variableMaxValue[realSlidingVarIndex] && slidingUpper != 0))
                    continue;
                else
                {
                    result[individuumIndex, realSlidingVarIndex, 0] = slidingLower == 0 ? 0 : lowerCompressed[realSlidingVarIndex];
                    result[individuumIndex, realSlidingVarIndex, 1] = slidingUpper == 0 ? upperUncompressed[realSlidingVarIndex] : upperCompressed[realSlidingVarIndex];
                }

                individuumIndex++;
            }

            if(individuumIndex - 1 < population)
            {
                var realResult = new int[individuumIndex, varCount, 2];
                for(int i = 0; i < individuumIndex; i++)
                {
                    for (int varIndex = 0; varIndex < varCount; varIndex++)
                    {
                        realResult[i, varIndex, 0] = result[i, varIndex, 0];
                        realResult[i, varIndex, 1] = result[i, varIndex, 1];
                    }
                }
                return realResult;
            }
            return result;
        }

        public unsafe void CalcProfitSizeArb(int i, int countVariable, int[,,] numericGen, 
                                        ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] arbPc, ref int countArb)
        {
            float profitTotal = 0;
            float arbProfitTotal = 0;
            float fitFuncTotal = 0;
            int sizeTotal = 0;
            int countTrades = Trades.Length;
            int[] lowerBounds = new int[countVariable];
            int[] upperBounds = new int[countVariable];

            for (int variableIndex = 0; variableIndex < countVariable; variableIndex++)
            {
                lowerBounds[variableIndex] = numericGen[i, variableIndex, 0];
                upperBounds[variableIndex] = numericGen[i, variableIndex, 1];
            }

            fixed (int* varArrayPtr = &allVariables[0], lowerBoundsArrayPtr = &lowerBounds[0], upperBoundsArrayPtr = &upperBounds[0])
            {
                int lastBoundsIndex = countVariable - 1;
                int* currentVarPtr = varArrayPtr;
                int* currentLowerBoundsPtr = lowerBoundsArrayPtr;
                int* currentUpperBoundsPtr = upperBoundsArrayPtr;
                int currentBoundsIndex = 0;
                int currentTradeIndex = 0;
                int countAllVars = allVariables.Length;

                while (currentTradeIndex < countTrades)
                {
                    int curVar = *currentVarPtr;

                    if (curVar <= *currentLowerBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (curVar >= *currentUpperBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (currentBoundsIndex == lastBoundsIndex)
                    {
                        TradeInfo tradeInfo = Trades[currentTradeIndex];
                        bool isArbitrage = tradeInfo.isArbitrage;
                        float Profit = tradeInfo.Points;

                        profitTotal += Profit;
                        sizeTotal++;

                        if (isArbitrage)
                        {
                            arbProfitTotal += Profit;
                            countArb++;
                        }

                        if (Profit < 0)
                            fitFuncTotal += Profit;

                        currentTradeIndex++;
                        currentVarPtr++;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                    }
                    else
                    {
                        currentVarPtr++;
                        currentBoundsIndex++;
                        currentLowerBoundsPtr++;
                        currentUpperBoundsPtr++;
                    }
                }
            }

            fitFunction[i] += fitFuncTotal;
            sumProfit[i] += profitTotal;
            arbProfit[i] += arbProfitTotal;
            size[i] += sizeTotal;

            countTrades = TradesCheck.Length;
            for (int tradeIndex = 0; tradeIndex < countTrades; tradeIndex++)
            {
                float Profit = TradesCheck[tradeIndex].Points;
                int agregate = 0;

                // 
                //определяется количество фильтров, в которые прошла особь
                for (int j = 0; j < countVariable; j++)
                {
                    if (TradesCheck[tradeIndex].variables[j] > numericGen[i, j, 0] && TradesCheck[tradeIndex].variables[j] < numericGen[i, j, 1])
                        agregate++;
                    else
                    {
                        break;
                    }
                }

                //если прошла по всем фильтрам
                if (agregate == countVariable)
                {
                    //накопленная прибыль
                    sumProfit[i] = sumProfit[i] + Profit;

                    //количество сделок всего
                    size[i] = size[i] + 1;

                    //количество арбитражных сделок
                    countArb = countArb + (TradesCheck[tradeIndex].isArbitrage ? 1 : 0);

                    //арбитражная прибыль
                    if (TradesCheck[tradeIndex].isArbitrage) arbProfit[i] = arbProfit[i] + Profit;

                    //целеывая заполняется прибылью профильтрованных отрицательных сделок
                    if (Profit < 0) fitFunction[i] = fitFunction[i] + Profit;
                }
            }

            //формириуется значение целевой функции для i-ой особи
            fitFunction[i] = fitFunction[i] + arbProfit[i];
            if (size[i] == 0 || countArb == 0) arbPc[i] = 0;
            else arbPc[i] = countArb / (float)size[i];
        }

        public unsafe void CalcProfitSizeArbPrediction(int i, int countVariable, int[, ,] numericGen,
                                         ref float[] sumProfit, ref float[] arbProfit, ref float[] fitFunction, ref float[] fitFunctionPredict, ref float[] arbPc, ref int countArb)
        {
            float profitTotal = 0;
            float arbProfitTotal = 0;
            float fitFuncTotal = 0;
            int sizeTotal = 0;
            int countTrades = Trades.Length;

            for (int variableIndex = 0; variableIndex < countVariable; variableIndex++)
            {
                lowerBounds[variableIndex] = numericGen[i, variableIndex, 0];
                upperBounds[variableIndex] = numericGen[i, variableIndex, 1];
            }

            fixed (int* varArrayPtr = &allVariables[0], lowerBoundsArrayPtr = &lowerBounds[0], upperBoundsArrayPtr = &upperBounds[0])
            {
                int lastBoundsIndex = countVariable - 1;
                int* currentVarPtr = varArrayPtr;
                int* currentLowerBoundsPtr = lowerBoundsArrayPtr;
                int* currentUpperBoundsPtr = upperBoundsArrayPtr;
                int currentBoundsIndex = 0;
                int currentTradeIndex = 0;
                int countAllVars = allVariables.Length;

                while (currentTradeIndex < countTrades)
                {
                    int curVar = *currentVarPtr;

                    if (curVar <= *currentLowerBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (curVar >= *currentUpperBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (currentBoundsIndex == lastBoundsIndex)
                    {
                        TradeInfo tradeInfo = Trades[currentTradeIndex];
                        bool isArbitrage = tradeInfo.isArbitrage;
                        float Profit = tradeInfo.Points;

                        profitTotal += Profit;
                        sizeTotal++;

                        if (isArbitrage)
                        {
                            arbProfitTotal += Profit;
                            countArb++;
                        }

                        if (Profit < 0)
                            fitFuncTotal += Profit;

                        currentTradeIndex++;
                        currentVarPtr++;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                    }
                    else
                    {
                        currentVarPtr++;
                        currentBoundsIndex++;
                        currentLowerBoundsPtr++;
                        currentUpperBoundsPtr++;
                    }
                }
            }

            sumProfit[i] += profitTotal;
            arbProfit[i] += arbProfitTotal;
            size[i] += sizeTotal;
            fitFunction[i] += fitFuncTotal;

            countTrades = TradesCheck.Length;
            for (int tradeIndex = 0; tradeIndex < countTrades; tradeIndex++)
            {
                float Profit = TradesCheck[tradeIndex].Points;
                int agregate = 0;

                // 
                //определяется количество фильтров, в которые прошла особь
                for (int j = 0; j < countVariable; j++)
                {
                    if (TradesCheck[tradeIndex].variables[j] > numericGen[i, j, 0] && TradesCheck[tradeIndex].variables[j] < numericGen[i, j, 1])
                        agregate++;
                    else
                    {
                        break;
                    }
                }

                //если прошла по всем фильтрам
                if (agregate == countVariable)
                {
                    //накопленная прибыль
                    sumProfit[i] = sumProfit[i] + Profit;

                    //количество сделок всего
                    size[i] = size[i] + 1;

                    //количество арбитражных сделок
                    countArb = countArb + (TradesCheck[tradeIndex].isArbitrage ? 1 : 0);

                    //арбитражная прибыль
                    if (TradesCheck[tradeIndex].isArbitrage) arbProfit[i] = arbProfit[i] + Profit;

                    //целеывая заполняется прибылью профильтрованных отрицательных сделок
                    if (Profit < 0) fitFunction[i] = fitFunction[i] + Profit;
                }
            }

            //формириуется значение целевой функции для i-ой особи
            fitFunction[i] = fitFunction[i] + arbProfit[i];
            arbPc[i] = (size[i] == 0 || countArb == 0) ? 0 : countArb / (float)size[i];

            countTrades = TradesPrediction.Length;
            fitFunctionPredict[i] = fitFunction[i];
            for (int tradeIndex = 0; tradeIndex < countTrades; tradeIndex++)
            {
                float Profit = TradesPrediction[tradeIndex].Points;
                int agregate = 0;

                // 
                //определяется количество фильтров, в которые прошла особь
                for (int j = 0; j < countVariable; j++)
                {
                    if (TradesPrediction[tradeIndex].variables[j] > numericGen[i, j, 0] && TradesPrediction[tradeIndex].variables[j] < numericGen[i, j, 1])
                        agregate++;
                    else
                    {
                        break;
                    }
                }

                //если прошла по всем фильтрам
                if (agregate == countVariable)
                {
                    if (Profit < 0 || TradesPrediction[tradeIndex].isArbitrage) fitFunctionPredict[i] = fitFunctionPredict[i] + Profit;
                }
            }

        }

        public void FillInitialPopulationIndicies(int population, ref int[] crossing, ref float[,] correct)
        {
            int j = 0;
            //заполняем индексы особей
            while (j != Parameters.NumberOfTheBestIndividuals)
            {

                for (int i = 0; i < population; i++)
                {
                    if (j >= Parameters.NumberOfTheBestIndividuals)
                        break;
                    if (correct[i, 1] > 0)
                    {
                        crossing[j] = i;
                        correct[i, 1] = correct[i, 1] - 1;
                        j++;
                    }
                }
            }
        }

        public void FindInitialPopulationIndividualsForCrossing(int population, float minCorrect2, float sumCorrect2, ref float currentMargin, ref float[,] correct)
        {
            for (int i = 0; i < (int)GeneMath.Round(Parameters.NumberOfTheBestIndividuals * Parameters.CoefOfStochastickSelect); i++)
            {
                //определение особей для скрещивания
                float margin = (float)PseudoRandom.Next100000() / (float)100000;
                int j = -1;

                /*  выбирается случайное число от 0 до 1 и далее отбирается та особь, чей кумулятивный прирост
                    доли в сумме целевых функций превзошел выбранное число*/
                while (currentMargin <= margin)
                {
                    j++;
                    if (j == correct.GetLength(0))
                    {
                        j = PseudoRandom.Next(correct.GetLength(0) - 1);
                        break;
                    }

                    if (correct[j, 0] == 0)
                        continue;

                    currentMargin = currentMargin + (GeneMath.Exp(GeneMath.Log(correct[j, 0]) * (Parameters.SelectionPower1 * Parameters.SelectionPower2)) - minCorrect2) / (sumCorrect2 - population * minCorrect2);
                }

                //+1 к количеству представлений особи в отборе
                correct[j, 1]++;
                currentMargin = 0;
            }

            for (int i = 0; i < (int)(Parameters.NumberOfTheBestIndividuals - Math.Round(Parameters.NumberOfTheBestIndividuals * Parameters.CoefOfStochastickSelect)); i++)
                correct[ind[i], 1] = correct[ind[i], 1] + 1;
        }

        public float[,] CalculateFitnessFunctionTrivial(int population, float[] fitFunction, float minFitFunc)
        {
            var correct = new float[population, 2]; 
            sumCorrect2 = 0;
            range = new float[population];
            ind = new int[population];
            minCorrect2 = 0;

            //рассчитывается интегральная целевая функция
            for (int individuumIndex = 0; individuumIndex < population; individuumIndex++)
            {
                correct[individuumIndex, 0] = fitFunction[individuumIndex] - minFitFunc;

                if (correct[individuumIndex, 0] != 0)
                {
                    float logCorrecti0 = (float)GeneMath.Log(correct[individuumIndex, 0]);

                    // ищется общая сумма значений целевой функции по всем особям, усиленная возведением в степень
                    sumCorrect2 = sumCorrect2 + (float)GeneMath.Exp(logCorrecti0 * Parameters.SelectionPower1);
                    if (individuumIndex == 0)
                        minCorrect2 = (float)GeneMath.Exp(logCorrecti0 * Parameters.SelectionPower1);
                    else if (GeneMath.Exp(GeneMath.Log(correct[individuumIndex, 0]) * Parameters.SelectionPower1) < minCorrect2)
                        minCorrect2 = (float)GeneMath.Exp(logCorrecti0 * Parameters.SelectionPower1);
                }

                //заполняется массив значениями цел. функции для ранжирования по убыванию
                range[individuumIndex] = correct[individuumIndex, 0];
                ind[individuumIndex] = individuumIndex;
            }

            qSort2(ref range, 0, population - 1);
            return correct;
        }

        private void SwapIndividuals(int population, ref int[] crossing)
        {
            //тусуем особи
            for (int i = population; i >= 1; i--)
            {
                int k = PseudoRandom.Next(i);
                int j = crossing[i - 1];
                crossing[i - 1] = crossing[k];
                crossing[k] = j;
            }
        }

        private int InvertBit(int oldValue, int bit)
        {
            var mask = 1 << bit;
            return (oldValue & mask) > 0 ? oldValue & (~mask) : oldValue | mask;
        }

        private void Mutation(float mutation, int varCount, int population, ref int[,,] numericGen)
        {
            float mutationCoef = Parameters.MutationCoef1 * mutation;
            for (int individuumIndex = 0; individuumIndex < population; individuumIndex++)
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                {
                    // lower filter margin
                    var lowerBitsCount = variableLowerBitsCount[varIndex];
                    for (int bit = 0; bit < lowerBitsCount; bit++)
                        if ((float)(random.Next(101) + 1) / 100.0F <= mutationCoef * (1 + (bit + 1) / (float)lowerBitsCount))
                        {
                            var genIndex = lowerGensReversed[varIndex][numericGen[individuumIndex, varIndex, 0]];
                            var mutationResult = InvertBit(genIndex, lowerBitsCount - bit - 1);
                            numericGen[individuumIndex, varIndex, 0] = lowerGens[varIndex][mutationResult];
                        }
                    // upper filter margin
                    var upperBitsCount = variableUpperBitsCount[varIndex];
                    for (int bit = 0; bit < upperBitsCount; bit++)
                        if ((float)(random.Next(101) + 1) / 100.0F <= mutationCoef * (1 + (bit + 1) / (float)upperBitsCount))
                        {
                            var genIndex = upperGensReversed[varIndex][numericGen[individuumIndex, varIndex, 1]];
                            var mutationResult = InvertBit(genIndex, upperBitsCount - bit - 1);
                            numericGen[individuumIndex, varIndex, 1] = upperGens[varIndex][mutationResult];
                        }
                }
        }

        private int CopyBits(int source, int destination, int position, int count)
        {
            uint mask = (uint)((1 << count) - 1) << position;
            return (int)((destination & ~mask) | (source & mask));
        }

        private void Crossing(int varCount, int population, ref int[,,] numericGens)
        {
            int crossCount = (int)GeneMath.Round((double)population / 2, 0);
            for (int l = 1; l <= crossCount; l++)
            {
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                {
                    var lowerBits = variableLowerBitsCount[varIndex];
                    var upperBits = variableUpperBitsCount[varIndex];
                    if (lowerBits > 0)
                    {
                        int t = PseudoRandom.Next(lowerBits);
                        var left = lowerGensReversed[varIndex][numericGens[2 * l - 2, varIndex, 0]];
                        var right = lowerGensReversed[varIndex][numericGens[2 * l - 1, varIndex, 0]];
                        var crossedLeft = CopyBits(right, left, 0, lowerBits - t);
                        var crossedRight = CopyBits(left, right, 0, lowerBits - t);
                        numericGens[2 * l - 2, varIndex, 0] = lowerGens[varIndex][crossedLeft];
                        numericGens[2 * l - 1, varIndex, 0] = lowerGens[varIndex][crossedRight];
                    }
                    if (upperBits > 0)
                    {
                        int t = PseudoRandom.Next(upperBits);
                        var left = upperGensReversed[varIndex][numericGens[2 * l - 2, varIndex, 1]];
                        var right = upperGensReversed[varIndex][numericGens[2 * l - 1, varIndex, 1]];
                        var crossedLeft = CopyBits(right, left, 0, upperBits - t);
                        var crossedRight = CopyBits(left, right, 0, upperBits - t);
                        numericGens[2 * l - 2, varIndex, 1] = upperGens[varIndex][crossedLeft];
                        numericGens[2 * l - 1, varIndex, 1] = upperGens[varIndex][crossedRight];
                    }
                }
            }
        }

        private void FillCurrentPopulationIndicies(int population, ref int countGen0, ref int[] crossing, ref float[,] correct2)
        {
            int j = 0;
            //заполняем индексы особей
            while (j < population)
            {
                for (int individuumIndex = 0; individuumIndex < correct2.GetLength(0); individuumIndex++)
                {
                    if (correct2[individuumIndex, 1] > 0)
                    {
                        crossing[j] = individuumIndex;
                        correct2[individuumIndex, 1]--;
                        j++;
                    }
                }

                if (countGen0 > 0)
                {
                    crossing[j] = -1;
                    j++;
                    countGen0--;
                }
            }
        }

        private int k;
        private int[] allVariables;
        private float[] tradeProfits;
        private byte[] tradeArbitrage;
        private SortedDictionary<int, long>[] aggregated;

        private SortedDictionary<int, long>[] GetAggregatedData(string symbolExchange)
        {
            SortedDictionary<int, long>[] result = null;
            try {
                using (var stream = new FileStream($"bin\\{symbolExchange}.bin", FileMode.Open))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var tempResult = (AgrFeedData)formatter.Deserialize(stream);
                    result = tempResult.Data;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }

        PseudoRandom PseudoRandom = new PseudoRandom(false);

        Dictionary<int, Dictionary<int, int>> lowerGens;
        Dictionary<int, Dictionary<int, int>> upperGens;
        Dictionary<int, Dictionary<int, int>> lowerGensReversed;
        Dictionary<int, Dictionary<int, int>> upperGensReversed;

        public int OptimizeSymbol()
        {
            int Population = Parameters.Population;
            float SelectionPower1 = Parameters.SelectionPower1;
            float FrequencyOfOccurrenceNullIndividual = Parameters.FrequencyOfOccurrenceNullIndividual;
            int CountOfGenerations = Parameters.CountOfGenerations;
            float MutationCoef1 = Parameters.MutationCoef1;
            int MutationCoef2 = Parameters.MutationCoef2;
            float MutationCoef3 = Parameters.MutationCoef3;
            float MutationCoef4 = Parameters.MutationCoef4;
            int NumberOfTheBestIndividuals = Parameters.NumberOfTheBestIndividuals;
            int CountOfGenerationsWithTheBestInd = Parameters.CountOfGenerationsWithTheBestInd;
            float CoefOfStochastickSelect = Parameters.CoefOfStochastickSelect;

            string dir;
            // число оптимизируемых параметров
            int varCount;
            // количество особей в популяции
            int population;
            // количество особей с абсолютно разжатыми фильтрами в популяции
            int countGen0;
            // количество переменых состояние которых меняется массово 
            int nonInflNumb;
            // массив модулей разниц между предыдущей максимальной прибылью и текущей
            float[] indMut;
            ////переменная для установления границы случайным образом при селекции по методу рулетки
            //float margin;
            //переменная для накопления долей значений целевых особей в сумме целевых функций популяции для определения особи при скрещивании
            float currentMargin = 0;
            //коэффициент мутации
            float mutation = 0;
            //сумма модулей разниц прибыли indMut для определения границы усиления роли мутации 
            float indicator;
            //максимальное значение валовой прибыли в предыдущей генерации
            float bestProfitBefore;

            int optimizeSymbol;

            dir = Environment.CurrentDirectory;
            InitDirectories();

            if (Trades.Length < Parameters.MinNumberOfTradesAfterPreopting)
            {
                varCount = Parameters.CountOfVariable;
                var numericGen = new int[2, varCount, 4];
                for (int index = 0; index < varCount; index++)
                {
                    variableMinValue[index] = -1;
                    variableMaxValue[index] = -1;
                    numericGen[1, index, 0] = -1;
                    numericGen[1, index, 1] = -1;
                }
                
                numericGen[1, 8, 0] = (int)Math.Round(minOpSl);
                variableMinValue[8] = numericGen[1, 8, 0] - 1;

                if (MakeSet(dir, varCount, numericGen, out optimizeSymbol, new BestIndividuum(varCount, 0, 0, 0, 0, 0, 0)))
                    return optimizeSymbol;

                return TradesFull.Length;
            }

            random = new Random();

            countTrades = Trades.Length;
            varCount = filters.Count;
            InitParametersAndLoadFeedData();

            if (Parameters.UseOpslipInFF == true)
            {
                variableLowerLimit[8] = variableMinValue[8];
                variableLowerBitsCount[8] = 0;
                variableLowerValuesCount[8] = 1;
            }

            nonInflNumb = varCount - inflectiveNumber + 2;

            allVariables = new int[(Trades.Length + TradesCheck.Length) * varCount];
            tradeProfits = new float[Trades.Length + TradesCheck.Length];
            tradeArbitrage = new byte[Trades.Length + TradesCheck.Length];

            for (int tradeIndex = 0; tradeIndex < Trades.Length; tradeIndex++)
            {
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                    allVariables[tradeIndex * varCount + varIndex] = Trades[tradeIndex].variables[varIndex];
                tradeProfits[tradeIndex] = Trades[tradeIndex].Points;
                tradeArbitrage[tradeIndex] = (byte)(Trades[tradeIndex].isArbitrage ? 1 : 0);
            }
            
            for (int tradeIndex = Trades.Length; tradeIndex < Trades.Length + TradesCheck.Length; tradeIndex++)
            {
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                    allVariables[tradeIndex * varCount + varIndex] = TradesCheck[tradeIndex - Trades.Length].variables[varIndex];
                tradeProfits[tradeIndex] = TradesCheck[tradeIndex - Trades.Length].Points;
                tradeArbitrage[tradeIndex] = (byte)(TradesCheck[tradeIndex - Trades.Length].isArbitrage ? 1 : 0);
            }

            lowerBounds = new int[varCount];
            upperBounds = new int[varCount];

            var tradesTask = new CalcFFFullAndCheckTradesOpenCLTask(taskCompletedEvent);
            tradesTask.AllVariables = allVariables;
            if(Parameters.UseOpenCLDevice)
                tradesTask.allvarsBuffer = new Cloo.ComputeBuffer<int>(OpenCLService.context, Cloo.ComputeMemoryFlags.ReadOnly | Cloo.ComputeMemoryFlags.CopyHostPointer, tradesTask.AllVariables);

            var predictionTradesTask = new CalcFFPredictionOpenCLTask(taskCompletedEvent);

            var gensAccumulator = new int[NumberOfTheBestIndividuals*nonInflNumb, varCount, 2];

            #region Перебор начальной популяции

            /* запуск цикла по перебору стартовых параметров. С целью приемлимой минимизации количества вариантов
                начальной популяции постоянно изменяют свое значение 9 параметров, 10-й чередуется, а остальные изменяются, как 1.
                Таким образом, запускается цикл по чередованию 10-го параметра из 13-и оставшихся (22 всего - 9 постоянно)*/

            var crossing = new int[Parameters.NumberOfTheBestIndividuals];
            var upperCompressed = new int[varCount];
            var lowerCompressed = new int[varCount];
            var upperUncompressed = new int[varCount];

            lowerGens = new Dictionary<int, Dictionary<int, int>>();
            upperGens = new Dictionary<int, Dictionary<int, int>>();
            lowerGensReversed = new Dictionary<int, Dictionary<int, int>>();
            upperGensReversed = new Dictionary<int, Dictionary<int, int>>();

            for (int varIndex = 0; varIndex < varCount; varIndex++)
            {
                lowerGens[varIndex] = new Dictionary<int, int>();
                upperGens[varIndex] = new Dictionary<int, int>();
                lowerGensReversed[varIndex] = new Dictionary<int, int>();
                upperGensReversed[varIndex] = new Dictionary<int, int>();

                var maxIndex = aggregated[varIndex].Keys.Max();
                if (filters[varIndex].MinLimit != "false" && filters[varIndex].MinLimit != "true")
                {
                    var shift = aggregated[varIndex].Values.Count(v => v < filters[varIndex].MinLimitValue);
                    if (shift == 0)
                    {
                        lowerCompressed[varIndex] = 0;
                    }
                    else
                    {
                        var uniqueCount = aggregated[varIndex].Values.Distinct().Count();
                        var marginByValues = Math.Floor(aggregated[varIndex].Values.Distinct().Count(v => v < filters[varIndex].MinLimitValue) / 2.0);
                        var reverseCoef = aggregated[varIndex].Count / (float)uniqueCount;
                        lowerCompressed[varIndex] = aggregated[varIndex].Keys.ElementAt((int)Math.Round(marginByValues * reverseCoef));
                    }

                    var maxCorrectedIndex = TradeInfoCollection.GetNextPowerof2Dec1(shift > 0 ? shift - 1 : aggregated[varIndex].Count - 1);
                    var coef = shift == 1 ? 1 : (float)(shift > 0 ? shift - 1 : aggregated[varIndex].Count - 1) / maxCorrectedIndex;
                    for (var valIndex = 0; valIndex < maxCorrectedIndex + 1; valIndex++)
                        lowerGens[varIndex][valIndex] = (int)Math.Round(valIndex * coef);
                }
                else if (filters[varIndex].MinLimit == "true")
                {
                    lowerCompressed[varIndex] = aggregated[varIndex].Count / 2;
                    var maxCorrectedIndex = TradeInfoCollection.GetNextPowerof2Dec1(aggregated[varIndex].Keys.Count - 1);
                    var coef = aggregated[varIndex].Keys.Count == 1 ? 1 : (float)(aggregated[varIndex].Keys.Count - 1) / maxCorrectedIndex;
                    for (var valIndex = 0; valIndex < maxCorrectedIndex + 1; valIndex++)
                        lowerGens[varIndex][valIndex] = (int)Math.Round(valIndex * coef);
                }
                else
                    lowerCompressed[varIndex] = 0;

                upperUncompressed[varIndex] = maxIndex;

                if (filters[varIndex].MaxLimit != "false" && filters[varIndex].MaxLimit != "true")
                {
                    var shift = aggregated[varIndex].Values.Count(v => v < filters[varIndex].MaxLimitValue);
                    upperCompressed[varIndex] = aggregated[varIndex].Keys.ElementAt(aggregated[varIndex].Count - (aggregated[varIndex].Count - shift) / 2);
                    var maxCorrectedIndex = TradeInfoCollection.GetNextPowerof2Dec1(aggregated[varIndex].Count - shift - 1);
                    var maxOriginalIndex = aggregated[varIndex].Keys.Max();
                    var coef = shift == 1 ? 1 : (float)(aggregated[varIndex].Count - shift - 1) / maxCorrectedIndex;
                    for (var valIndex = 0; valIndex < maxCorrectedIndex + 1; valIndex++)
                        upperGens[varIndex][valIndex] = (int)Math.Round(maxOriginalIndex - shift - valIndex * coef) + shift;
                }
                else if (filters[varIndex].MaxLimit == "true")
                {
                    var uniqueValues = aggregated[varIndex].Values.Distinct().ToList();
                    var uniqueCount = uniqueValues.Count();
                    var compressedValue = uniqueValues[uniqueCount / 2];

                    foreach (var pair in aggregated[varIndex])
                    {
                        if (pair.Value == compressedValue)
                        {
                            upperCompressed[varIndex] = pair.Key;
                            break;
                        }
                    }

                    var maxCorrectedIndex = TradeInfoCollection.GetNextPowerof2Dec1(aggregated[varIndex].Count - 1);
                    var coef = aggregated[varIndex].Keys.Count == 1 ? 1 : (float)(aggregated[varIndex].Count - 1) / maxCorrectedIndex;
                    for (var valIndex = 0; valIndex < maxCorrectedIndex + 1; valIndex++)
                        upperGens[varIndex][valIndex] = (int)Math.Round(maxIndex - valIndex * coef);
                }
                else
                    upperCompressed[varIndex] = maxIndex;

                lowerGensReversed[varIndex] = lowerGens[varIndex].Reverse<int>();
                upperGensReversed[varIndex] = upperGens[varIndex].Reverse<int>();
            }


            var realVarIndexes = InitRealVarIndexes();

            for (var slidingVarNumber = 1; slidingVarNumber <= nonInflNumb; slidingVarNumber++)
            {
                PseudoRandom.Reset();

                var numericGen = GetInitialPopulationGens(inflectiveNumber, varCount, realVarIndexes, slidingVarNumber, upperUncompressed, upperCompressed, lowerCompressed);

                population = numericGen.GetLength(0);

                minFitFunc = 0;
                sumProfit = new float[population];
                size = new int[population];
                arbSize = new int[population];
                arbPc = new float[population];
                arbProfit = new float[population];
                fitFunction = new float[population];

                #region OpenCL FF calculation
                if (Parameters.UseOpenCLDevice && Trades.Length >= Parameters.MinTradesForUseOpenCL)
                {
                    // full trades
                    tradesTask.Dimensions = new long[] { population, Trades.Length };
                    tradesTask.AllVariables = allVariables;
                    tradesTask.TradeProfits = tradeProfits;
                    tradesTask.TradeArbitrage = tradeArbitrage;
                    tradesTask.ArbPc = new float[population];
                    tradesTask.ArbSize = new int[population];
                    tradesTask.ArbProfit = new float[population];
                    tradesTask.FitFunction = new float[population];
                    tradesTask.Size = new int[population];
                    tradesTask.SumProfit = new float[population];
                    tradesTask.VariablesCount = varCount;
                    tradesTask.LowerBounds = new int[varCount * population];
                    tradesTask.UpperBounds = new int[varCount * population];
                    tradesTask.CreateBuffers();

                    for (var index = 0; index < population; index++)
                        for (int varIndex = 0; varIndex < varCount; varIndex++)
                        {
                            tradesTask.LowerBounds[varCount * index + varIndex] = numericGen[index, varIndex, 0];
                            tradesTask.UpperBounds[varCount * index + varIndex] = numericGen[index, varIndex, 1];
                        }

                    OpenCLService.tasksQueue.Enqueue(tradesTask);
                    OpenCLService.TaskEnqueuedEvent.Set();
                    taskCompletedEvent.WaitOne();
                    size = tradesTask.Size;
                    arbProfit = tradesTask.ArbProfit;
                    sumProfit = tradesTask.SumProfit;
                    fitFunction = tradesTask.FitFunction;
                    arbSize = tradesTask.ArbSize;

                    for (int individualIndex = 0; individualIndex < population; individualIndex++)
                    {
                        fitFunction[individualIndex] += arbProfit[individualIndex];
                        if (size[individualIndex] == 0 || arbSize[individualIndex] == 0)
                            arbPc[individualIndex] = 0;
                        else
                            arbPc[individualIndex] = arbSize[individualIndex] / size[individualIndex];
                    }
                }
                #endregion

                //цикл по расчету значений целевой функции
                for (var index = 0; index < population; index++)
                {
                    countArb = 0;
                    if (!Parameters.UseOpenCLDevice || Trades.Length < Parameters.MinTradesForUseOpenCL)
                        CalcProfitSizeArb(index, varCount, numericGen, ref sumProfit, ref arbProfit, ref fitFunction, ref arbPc, ref countArb);

                    if (index == 0)
                        minFitFunc = fitFunction[index];
                    else
                        minFitFunc = minFitFunc > fitFunction[index] ? fitFunction[index] : minFitFunc;
                }

                var correct = CalculateFitnessFunctionTrivial(population, fitFunction, minFitFunc);

                FindInitialPopulationIndividualsForCrossing(population, minCorrect2, sumCorrect2, ref currentMargin, ref correct);

                FillInitialPopulationIndicies(population, ref crossing, ref correct);

                population = NumberOfTheBestIndividuals;

                for (int index = 0; index < NumberOfTheBestIndividuals; index++)
                {
                    for (int varIndex = 0; varIndex < varCount; varIndex++)
                    {
                        gensAccumulator[(slidingVarNumber - 1) * NumberOfTheBestIndividuals + index, varIndex, 0] = numericGen[crossing[index], varIndex, 0];
                        gensAccumulator[(slidingVarNumber - 1) * NumberOfTheBestIndividuals + index, varIndex, 1] = numericGen[crossing[index], varIndex, 1];
                    }
                }
            }

            #endregion


            #region Цикл по поколениям

            var bestIndividuumsOfAllGenerations = new List<BestIndividuum>();
            indMut = new float[MutationCoef2];
            bestProfitBefore = 0;
            indicator = 0;
            population = gensAccumulator.GetLength(0);
            var _numericGen = gensAccumulator;
            
            //старт непосредственно оптимизации
            while (bestIndividuumsOfAllGenerations.Count <= CountOfGenerations)
            {
                minSp = 0;
                maxSP = 0;
                maxSize = 0;
                minSize = 0;
                minFitFunc = 0;
                maxFitFunc = 0;
                maxFitFuncPrediction = 0;

                size = new int[population];
                arbPc = new float[population];
                arbSize = new int[population];
                arbProfit = new float[population];
                sumProfit = new float[population];
                fitFunction = new float[population];
                fitFunctionPredict = new float[population];
                crossing = new int[population];

                k = 0;

                #region OpenCL FF calculation
                if (Parameters.UseOpenCLDevice && Trades.Length >= Parameters.MinTradesForUseOpenCL)
                {
                    // full trades
                    tradesTask.Dimensions = new long[] { population, Trades.Length };
                    tradesTask.AllVariables = allVariables;
                    tradesTask.TradeProfits = tradeProfits;
                    tradesTask.TradeArbitrage = tradeArbitrage;
                    tradesTask.ArbPc = arbPc;
                    tradesTask.ArbSize = arbSize;
                    tradesTask.ArbProfit = arbProfit;
                    tradesTask.FitFunction = fitFunction;
                    tradesTask.Size = size;
                    tradesTask.SumProfit = sumProfit;
                    tradesTask.VariablesCount = varCount;
                    tradesTask.LowerBounds = new int[varCount * population];
                    tradesTask.UpperBounds = new int[varCount * population];

                    for (var index = 0; index < population; index++)
                        for (int varIndex = 0; varIndex < varCount; varIndex++)
                        {
                            tradesTask.LowerBounds[varCount * index + varIndex] = _numericGen[index, varIndex, 0];
                            tradesTask.UpperBounds[varCount * index + varIndex] = _numericGen[index, varIndex, 1];
                        }

                    tradesTask.CreateBuffers();

                    OpenCLService.tasksQueue.Enqueue(tradesTask);
                    OpenCLService.TaskEnqueuedEvent.Set();
                    taskCompletedEvent.WaitOne();
                    size = tradesTask.Size;
                    arbProfit = tradesTask.ArbProfit;
                    sumProfit = tradesTask.SumProfit;
                    fitFunction = tradesTask.FitFunction;
                    arbSize = tradesTask.ArbSize;

                    for (int individualIndex = 0; individualIndex < population; individualIndex++)
                    {
                        fitFunction[individualIndex] += arbProfit[individualIndex];
                        if (size[individualIndex] == 0 || arbSize[individualIndex] == 0)
                            arbPc[individualIndex] = 0;
                        else
                            arbPc[individualIndex] = arbSize[individualIndex] / size[individualIndex];

                        fitFunctionPredict[individualIndex] = fitFunction[individualIndex];
                    }

                    // prediction trades
                    if (TradesPrediction.Length > 0)
                    {
                        predictionTradesTask.Dimensions = new long[] { population, TradesCheck.Length };
                        predictionTradesTask.AllVariables = new int[TradesCheck.Length * varCount];
                        predictionTradesTask.TradeProfits = new short[TradesCheck.Length];
                        predictionTradesTask.TradeArbitrage = new byte[TradesCheck.Length];
                        predictionTradesTask.FitFunction = fitFunction;
                        predictionTradesTask.VariablesCount = varCount;
                        predictionTradesTask.LowerBounds = tradesTask.LowerBounds;
                        predictionTradesTask.UpperBounds = tradesTask.UpperBounds;

                        for (int tradeIndex = 0; tradeIndex < TradesPrediction.Length; tradeIndex++)
                        {
                            for (int varIndex = 0; varIndex < varCount; varIndex++)
                                predictionTradesTask.AllVariables[tradeIndex * varCount + varIndex] = TradesPrediction[tradeIndex].variables[varIndex];

                            predictionTradesTask.TradeProfits[tradeIndex] = (short)TradesPrediction[tradeIndex].Profit;
                            predictionTradesTask.TradeArbitrage[tradeIndex] = (byte)(TradesPrediction[tradeIndex].isArbitrage ? 1 : 0);
                        }

                        OpenCLService.tasksQueue.Enqueue(predictionTradesTask);
                        OpenCLService.TaskEnqueuedEvent.Set();
                        taskCompletedEvent.WaitOne();
                        fitFunctionPredict = predictionTradesTask.FitFunction;
                    }
                }
                #endregion

                for (var index = 0; index < population; index++)
                {
                    countArb = 0;

                    if (!Parameters.UseOpenCLDevice || Trades.Length < Parameters.MinTradesForUseOpenCL)
                        CalcProfitSizeArbPrediction(index, varCount, _numericGen, ref sumProfit, ref arbProfit, ref fitFunction, ref fitFunctionPredict, ref arbPc, ref countArb);

                    if (index == 0)
                    {
                        minSp = sumProfit[index];
                        maxSP = sumProfit[index];
                        maxSize = size[index];
                        minSize = size[index];
                        minFitFunc = fitFunction[index];
                        maxFitFunc = fitFunction[index];
                        maxFitFuncPrediction = fitFunctionPredict[index];
                    }
                    else
                    {
                        if (minSp > sumProfit[index]) minSp = sumProfit[index];
                        if (maxSP < sumProfit[index]) maxSP = sumProfit[index];
                        if (maxSize < size[index]) maxSize = size[index];
                        if (minSize > size[index]) minSize = size[index];
                        if (minFitFunc > fitFunction[index]) minFitFunc = fitFunction[index];
                        if (maxFitFunc < fitFunction[index]) maxFitFunc = fitFunction[index];
                        if (maxFitFuncPrediction < fitFunctionPredict[index]) { maxFitFuncPrediction = fitFunctionPredict[index]; k = index; }
                    }
                }

                if (bestIndividuumsOfAllGenerations.Count == 0)
                {
                    globalMax = maxFitFuncPrediction;
                    globalIndex = 0;
                }

                if (maxFitFuncPrediction > globalMax)
                {
                    globalMax = maxFitFuncPrediction;
                    globalIndex = bestIndividuumsOfAllGenerations.Count;
                }

                var correct2 = CalculateFitnessFunctionTrivial(population, fitFunction, minFitFunc);

                //усиление роли мутации если алгоритм "залип"=локальный экстремум
                if (bestIndividuumsOfAllGenerations.Count > MutationCoef2 - 1)
                    indicator = indicator - indMut[bestIndividuumsOfAllGenerations.Count % MutationCoef2];

                indMut[bestIndividuumsOfAllGenerations.Count % MutationCoef2] = Math.Abs(maxFitFunc - bestProfitBefore);
                indicator = indicator + indMut[bestIndividuumsOfAllGenerations.Count % MutationCoef2];
                bestProfitBefore = maxFitFunc;

                var individuum = new BestIndividuum(varCount, maxFitFunc, size[k], (float)GeneMath.Round(100 * arbPc[k]) / 100, mutation, indicator, maxFitFuncPrediction);
                for(var varIndex = 0; varIndex < varCount; varIndex++)
                {
                    var agrV = aggregated[varIndex];
                    var lowerIndex = agrV.ContainsKey(_numericGen[k, varIndex, 0]) ? _numericGen[k, varIndex, 0] : _numericGen[k, varIndex, 0] - 1;
                    var upperIndex = agrV.ContainsKey(_numericGen[k, varIndex, 1]) ? _numericGen[k, varIndex, 1] : _numericGen[k, varIndex, 1] - 1;
                    individuum.Indexes[varIndex, 0] = lowerIndex;
                    individuum.Indexes[varIndex, 1] = upperIndex;
                    individuum.Values[varIndex, 0] = agrV[lowerIndex];
                    individuum.Values[varIndex, 1] = agrV[upperIndex];
                }
                bestIndividuumsOfAllGenerations.Add(individuum);

                if ((mutation == 0) && (k > 0))
                    mutation = 0.0075F / (float)GeneMath.Exp(MutationCoef2 + 1) * (float)GeneMath.Log(MutationCoef4);

                if ((bestIndividuumsOfAllGenerations.Count > MutationCoef2) && (indicator / MutationCoef2 < MutationCoef3) && (maxFitFunc != 0))
                    mutation = mutation * MutationCoef4;
                else
                    mutation = 0.0075F;

                if ((bestIndividuumsOfAllGenerations.Count > MutationCoef2) && (maxFitFunc == 0) && (indicator == 0) && (k == 0))
                    mutation = 0;

                countGen0 = 0;

                if (bestIndividuumsOfAllGenerations.Count == CountOfGenerationsWithTheBestInd)
                    population = Parameters.Population;

                //определение особей для скрещивания
                for (var index = 0; index < (int)GeneMath.Round(population * CoefOfStochastickSelect, 0); index++)
                {
                    var margin = (float)PseudoRandom.Next100000() / (float)100000;
                    var individuumIndex = -1;
                    if ((PseudoRandom.Next((int)FrequencyOfOccurrenceNullIndividual) /
                            FrequencyOfOccurrenceNullIndividual ==
                            PseudoRandom.Next((int)FrequencyOfOccurrenceNullIndividual) / FrequencyOfOccurrenceNullIndividual))
                    {
                        countGen0++;
                    }
                    else
                    {
                        while (currentMargin <= margin)
                        {
                            individuumIndex++;
                            if (individuumIndex == correct2.GetLength(0))
                            {
                                individuumIndex = PseudoRandom.Next(correct2.GetLength(0) - 1);
                                break;
                            }
                            if (correct2[individuumIndex, 0] == 0)
                                continue;

                            currentMargin = currentMargin + (float)(GeneMath.Exp(GeneMath.Log(correct2[individuumIndex, 0]) * SelectionPower1) -
                                                    minCorrect2) / (sumCorrect2 - population * minCorrect2);

                        }
                        correct2[individuumIndex, 1]++;
                        currentMargin = 0;
                    }
                }

                for (var index = 0; index < population - (int)GeneMath.Round(population * CoefOfStochastickSelect, 0); index++)
                    correct2[ind[index], 1]++;

                FillCurrentPopulationIndicies(population, ref countGen0, ref crossing, ref correct2);

                SwapIndividuals(population, ref crossing);

                var tempGens = new int[_numericGen.GetLength(0), _numericGen.GetLength(1), _numericGen.GetLength(2)];
                Array.Copy(_numericGen, tempGens, _numericGen.Length);

                for (int individuumIndex = 0; individuumIndex < population; individuumIndex++)
                    for (int varIndex = 0; varIndex < varCount; varIndex++)
                    {
                        if (crossing[individuumIndex] == -1)
                        {
                            tempGens[individuumIndex, varIndex, 0] = 0;
                            tempGens[individuumIndex, varIndex, 1] = upperUncompressed[varIndex];
                        }
                        else
                        {
                            tempGens[individuumIndex, varIndex, 0] = _numericGen[crossing[individuumIndex], varIndex, 0];
                            tempGens[individuumIndex, varIndex, 1] = _numericGen[crossing[individuumIndex], varIndex, 1]; 
                        }
                    }

                Crossing(varCount, population, ref tempGens);

                Array.Copy(tempGens, _numericGen, _numericGen.Length);

                if (!Parameters.TurnOffMutation)
                    Mutation(mutation, varCount, population, ref _numericGen);
            }
            #endregion

            bestIndividuumsOfAllGenerations.SaveToFile(dir + "/Recomends/log/" + Symbol + "/" + AgrEx + ".txt");

            _numericGen = new int[2, varCount, 4];
            //j = 0;

            var bestOfAllGenerations = bestIndividuumsOfAllGenerations[globalIndex];

            for (var varIndex = 0; varIndex < varCount; varIndex++)
            {
                _numericGen[0, varIndex, 0] = (int)bestOfAllGenerations.Values[varIndex, 0];
                _numericGen[0, varIndex, 1] = (int)bestOfAllGenerations.Values[varIndex, 1];
                _numericGen[0, varIndex, 2] = (int)bestOfAllGenerations.Indexes[varIndex, 0];
                _numericGen[0, varIndex, 3] = (int)bestOfAllGenerations.Indexes[varIndex, 1];

                _numericGen[1, varIndex, 0] = (int)bestOfAllGenerations.Values[varIndex, 0];
                _numericGen[1, varIndex, 1] = (int)bestOfAllGenerations.Values[varIndex, 1];
                _numericGen[1, varIndex, 2] = (int)bestOfAllGenerations.Indexes[varIndex, 0];
                _numericGen[1, varIndex, 3] = (int)bestOfAllGenerations.Indexes[varIndex, 1];
            }

            if(MakeSet(dir, varCount, _numericGen, out optimizeSymbol, bestOfAllGenerations))
                return optimizeSymbol;

            return TradesFull.Length;
        }

        private string BitsToString(int lowerVal, int v)
        {
            var sb = new StringBuilder();
            for (int i = v - 1; i >= 0; i--)
                sb.Append((lowerVal & (1 << i)) > 0 ? "1" : "0");
            return sb.ToString();
        }

        private unsafe float CalcArbProfitPostOptimization(int countVariable, int[,,] numericGen)
        {
            float result = 0;

            for (int variableIndex = 0; variableIndex < countVariable; variableIndex++)
            {
                lowerBounds[variableIndex] = numericGen[1, variableIndex, 2];
                upperBounds[variableIndex] = numericGen[1, variableIndex, 3];
            }

            fixed (int* varArrayPtr = &allVariables[0], lowerBoundsArrayPtr = &lowerBounds[0], upperBoundsArrayPtr = &upperBounds[0])
            {
                int lastBoundsIndex = countVariable - 1;
                int* currentVarPtr = varArrayPtr;
                int* currentLowerBoundsPtr = lowerBoundsArrayPtr;
                int* currentUpperBoundsPtr = upperBoundsArrayPtr;
                int currentBoundsIndex = 0;
                int currentTradeIndex = 0;
                int countAllVars = allVariables.Length;

                while (currentTradeIndex < countTrades)
                {
                    int curVar = *currentVarPtr;

                    if (curVar <= *currentLowerBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (curVar >= *currentUpperBoundsPtr)
                    {
                        currentTradeIndex++;
                        currentVarPtr += countVariable - currentBoundsIndex;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                        continue;
                    }

                    if (currentBoundsIndex == lastBoundsIndex)
                    {
                        TradeInfo tradeInfo = Trades[currentTradeIndex];
                        bool isArbitrage = tradeInfo.isArbitrage;
                        float Profit = tradeInfo.Points;

                        if (isArbitrage && Profit < 0)
                            result += Profit;

                        currentTradeIndex++;
                        currentVarPtr++;
                        currentBoundsIndex = 0;
                        currentLowerBoundsPtr = lowerBoundsArrayPtr;
                        currentUpperBoundsPtr = upperBoundsArrayPtr;
                    }
                    else
                    {
                        currentVarPtr++;
                        currentBoundsIndex++;
                        currentLowerBoundsPtr++;
                        currentUpperBoundsPtr++;
                    }
                }
            }
            return result;
        }

        private bool MakeSet(string dir, int varCount, int[,,] numericGen, out int optimizeSymbol, BestIndividuum bestIndividuum)
        {
            var recom = new List<string>();
            recom.LoadFromFile(dir + "/Recomends/" + Symbol + "/" + AgrEx + ".txt");
            if (countTrades < Parameters.MinNumberOfTrades)
            {
                recom.Add("***");
            }
            else
            {
                recom.Add(bestIndividuum.ToString());
            }

            variableMinValue[8] = (long)minOpSlGlobal;
            string[,] isFiltered = new string[varCount, 2];
            //формирование файлов под зашивку файла настроек

            for (var varIndex = 0; varIndex < varCount; varIndex++)
            {
                double minMargin;
                double maxMargin;
                long downMarginCurrent, downMarginParameters;
                long upMarginCurrent, upMarginParameters;
                if (AgrEx[AgrEx.Length - 1] == 'd' && Parameters.OpenTrendIndex == varIndex)
                {
                    minMargin = Math.Round(-1*numericGen[1, varIndex, 1]*Steps[varIndex], CountDecimal[varIndex]);
                    maxMargin = Math.Round(-1*numericGen[1, varIndex, 0]*Steps[varIndex], CountDecimal[varIndex]);
                    downMarginCurrent = numericGen[1, varIndex, 1];
                    upMarginCurrent = numericGen[1, varIndex, 0];
                    downMarginParameters = variableMaxValue[varIndex];
                    upMarginParameters = variableMinValue[varIndex];
                }
                else
                {
                    minMargin = Math.Round(numericGen[1, varIndex, 0]*Steps[varIndex], CountDecimal[varIndex]);
                    maxMargin = Math.Round(numericGen[1, varIndex, 1]*Steps[varIndex], CountDecimal[varIndex]);
                    downMarginCurrent = numericGen[1, varIndex, 0];
                    upMarginCurrent = numericGen[1, varIndex, 1];
                    downMarginParameters = variableMinValue[varIndex];
                    upMarginParameters = variableMaxValue[varIndex];
                }

                if (downMarginCurrent == downMarginParameters && upMarginCurrent == upMarginParameters)
                {
                    recom.Add($"xxx\txxx\t{filters[varIndex].Name}");
                    isFiltered[varIndex, 0] = "xxx";
                    isFiltered[varIndex, 1] = "xxx";
                }
                if (downMarginCurrent == downMarginParameters && upMarginCurrent != upMarginParameters)
                {
                    recom.Add($"xxx\t{maxMargin}\t{filters[varIndex].Name}");
                    isFiltered[varIndex, 0] = "xxx";
                    isFiltered[varIndex, 1] = upMarginCurrent.ToString();
                }
                if (downMarginCurrent != downMarginParameters && upMarginCurrent == upMarginParameters)
                {
                    recom.Add($"{minMargin}\txxx\t{filters[varIndex].Name}");
                    isFiltered[varIndex, 0] = downMarginCurrent.ToString();
                    isFiltered[varIndex, 1] = "xxx";
                }
                if (downMarginCurrent != downMarginParameters && upMarginCurrent != upMarginParameters)
                {
                    recom.Add($"{minMargin}\t{maxMargin}\t{filters[varIndex].Name}");
                    isFiltered[varIndex, 0] = downMarginCurrent.ToString();
                    isFiltered[varIndex, 1] = upMarginCurrent.ToString();
                }
            }

            if (AgrEx[AgrEx.Length - 1] == 'd')
            {
                string tmp = isFiltered[Parameters.OpenTrendIndex, 0];
                isFiltered[Parameters.OpenTrendIndex, 0] = isFiltered[Parameters.OpenTrendIndex, 1];
                isFiltered[Parameters.OpenTrendIndex, 1] = tmp;
            }

            this.Trades = TradesFull;
            countTrades = Trades.Length;
            InitParametersAndLoadFeedData();

            allVariables = new int[(Trades.Length) * varCount];
            tradeProfits = new float[Trades.Length ];
            tradeArbitrage = new byte[Trades.Length];

            for (int tradeIndex = 0; tradeIndex < Trades.Length; tradeIndex++)
            {
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                    allVariables[tradeIndex * varCount + varIndex] = Trades[tradeIndex].variables[varIndex];
                tradeProfits[tradeIndex] = Trades[tradeIndex].Points;
                tradeArbitrage[tradeIndex] = (byte)(Trades[tradeIndex].isArbitrage ? 1 : 0);
            }

            for (var varIndex = 0; varIndex < varCount; varIndex++)
            {
                if (isFiltered[varIndex, 0] == "xxx")
                    numericGen[0, varIndex, 0] = (int)variableMinValue[varIndex];
                else
                    Utils.TryParse(isFiltered[varIndex, 0], out numericGen[0, varIndex, 0]);

                if (isFiltered[varIndex, 1] == "xxx")
                    numericGen[0, varIndex, 1] = (int)variableMaxValue[varIndex];
                else
                    Utils.TryParse(isFiltered[varIndex, 1], out numericGen[0, varIndex, 1]);
            }

            float SP = 0;
            var maxSizePostOptmization = 0;
            maxCPF = 0;
            globalMax = 0;

            int agregate = 0;

            //вычисление значения новой прибыли
            for (int tradeIndex = 0; tradeIndex < countTrades; tradeIndex++)
            {
                agregate = 0;
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                {
                    if (Trades[tradeIndex].variables[varIndex] <= numericGen[1, varIndex, 2])
                        break;

                    if (Trades[tradeIndex].variables[varIndex] >= numericGen[1, varIndex, 3])
                        break;

                    agregate++;
                }

                if (agregate == varCount)
                {
                    SP += Trades[tradeIndex].Points;
                    maxSizePostOptmization++;
                    if (Trades[tradeIndex].Points >= 0)
                        maxCPF++;
                    if (Trades[tradeIndex].isArbitrage || Trades[tradeIndex].Points < 0)
                        globalMax += Trades[tradeIndex].Points;
                }
            }

            if (recom[0] != "***")
            {
                int w3 = -1;
                while (w3 != 0)
                {
                    w3 = 0;
                    for (int varIndex = varCount - 1; varIndex >= 0; varIndex--)
                    {
                        var varAgr = aggregated[varIndex];
                        if (numericGen[1, varIndex, 2] > 0)
                        {
                            for (var valueIndex = numericGen[1, varIndex, 2] - 1; valueIndex >= 0; valueIndex--)
                            {
                                var currentLowerIndex = numericGen[1, varIndex, 2];
                                var newLowerIndex = varAgr.ContainsKey(numericGen[1, varIndex, 2] - 1) ? numericGen[1, varIndex, 2] - 1 : numericGen[1, varIndex, 2] - 2;
                                numericGen[1, varIndex, 2] = newLowerIndex; 

                                SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                if (SP < globalMax)
                                {
                                    numericGen[1, varIndex, 2] = currentLowerIndex;
                                    break;
                                }

                                numericGen[1, varIndex, 0] = (int)varAgr[newLowerIndex];
                                globalMax = SP;
                                w3++;
                            }
                        }

                        if (numericGen[1, varIndex, 3] < varAgr.Keys.Max())
                        {
                            for (var valueIndex = numericGen[1, varIndex, 3] + 1; valueIndex < variableValuesCount[varIndex]; valueIndex++)
                            {
                                var currentUpperIndex = numericGen[1, varIndex, 3];
                                var newUpperIndex = varAgr.ContainsKey(numericGen[1, varIndex, 3] + 1) ? numericGen[1, varIndex, 3] + 1 : numericGen[1, varIndex, 3] + 2;
                                numericGen[1, varIndex, 3] = newUpperIndex;
                                SP = 0;

                                SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                if (SP < globalMax)
                                {
                                    numericGen[1, varIndex, 3] = currentUpperIndex;
                                    break;
                                }

                                numericGen[1, varIndex, 1] = (int)varAgr[newUpperIndex];
                                globalMax = SP;
                                w3++;
                            }
                        }
                    }
                }

                float bestBefore = globalMax - 1;
                while (bestBefore < globalMax)
                {
                    bestBefore = globalMax;
                    w3 = -1;
                    while (w3 != 0)
                    {
                        w3 = 0;
                        for (int varIndex = varCount - 1; varIndex >= 0; varIndex--)
                        {
                            var varAgr = aggregated[varIndex];
                            if (numericGen[1, varIndex, 2] < variableValuesCount[varIndex] - 1)
                            {
                                for (var valueIndex = numericGen[1, varIndex, 2] + 1; valueIndex < variableValuesCount[varIndex]; valueIndex++)
                                {
                                    if (aggregated[varIndex][valueIndex] < variableLowerLimit[varIndex])
                                    {
                                        var currentLowerIndex = numericGen[1, varIndex, 2];
                                        var newLowerIndex = varAgr.ContainsKey(numericGen[1, varIndex, 2] + 1) ? numericGen[1, varIndex, 2] + 1 : numericGen[1, varIndex, 2] + 2;
                                        numericGen[1, varIndex, 2] = newLowerIndex;

                                        SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                        if (SP < globalMax)
                                        {
                                            numericGen[1, varIndex, 2] = currentLowerIndex;
                                            break;
                                        }

                                        numericGen[1, varIndex, 0] = (int)varAgr[newLowerIndex];
                                        globalMax = SP;
                                        w3++;
                                    }
                                    else break;
                                }
                            }

                            if (numericGen[1, varIndex, 3] > 0)
                            {
                                for (var valueIndex = numericGen[1, varIndex, 3] - 1; valueIndex >= 0; valueIndex--)
                                {
                                    if (aggregated[varIndex][valueIndex] > variableUpperLimit[varIndex])
                                    {
                                        var currentUpperIndex = numericGen[1, varIndex, 3];
                                        var newUpperIndex = varAgr.ContainsKey(numericGen[1, varIndex, 3] - 1) ? numericGen[1, varIndex, 3] - 1 : numericGen[1, varIndex, 3] - 2;
                                        numericGen[1, varIndex, 3] = newUpperIndex;

                                        SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                        if (SP < globalMax)
                                        {
                                            numericGen[1, varIndex, 3] = currentUpperIndex;
                                            break;
                                        }

                                        numericGen[1, varIndex, 1] = (int)varAgr[newUpperIndex];
                                        globalMax = SP;
                                        w3++;
                                    }
                                    else break;
                                }
                            }
                        }
                    }

                    w3 = -1;
                    while (w3 != 0)
                    {
                        w3 = 0;

                        for (int varIndex = varCount - 1; varIndex >= 0; varIndex--)
                        {
                            var varAgr = aggregated[varIndex];
                            if (numericGen[1, varIndex, 2] > 0)
                            {
                                for (var valueIndex = numericGen[1, varIndex, 2] - 1; valueIndex >= 0; valueIndex--)
                                {
                                    var currentLowerIndex = numericGen[1, varIndex, 2];
                                    var newLowerIndex = varAgr.ContainsKey(numericGen[1, varIndex, 2] - 1) ? numericGen[1, varIndex, 2] - 1 : numericGen[1, varIndex, 2] - 2;
                                    numericGen[1, varIndex, 2] = newLowerIndex;

                                    SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                    if (SP < globalMax)
                                    {
                                        numericGen[1, varIndex, 2] = currentLowerIndex;
                                        break;
                                    }

                                    numericGen[1, varIndex, 0] = (int)varAgr[newLowerIndex];
                                    globalMax = SP;
                                    w3++;
                                }
                            }

                            if (numericGen[1, varIndex, 3] < variableValuesCount[varIndex] - 1)
                            {
                                for (var valueIndex = numericGen[1, varIndex, 3] + 1; valueIndex < variableValuesCount[varIndex]; valueIndex++)
                                {
                                    var currentUpperIndex = numericGen[1, varIndex, 3];
                                    var newUpperIndex = varAgr.ContainsKey(numericGen[1, varIndex, 3] + 1) ? numericGen[1, varIndex, 3] + 1 : numericGen[1, varIndex, 3] + 2;
                                    numericGen[1, varIndex, 3] = newUpperIndex;

                                    SP = CalcArbProfitPostOptimization(varCount, numericGen);

                                    if (SP < globalMax)
                                    {
                                        numericGen[1, varIndex, 3] = currentUpperIndex;
                                        break;
                                    }

                                    numericGen[1, varIndex, 1] = (int)varAgr[newUpperIndex];
                                    globalMax = SP;
                                    w3++;
                                }
                            }
                        }
                    }
                }
            }

            SP = 0;
            maxSizePostOptmization = 0;
            maxCPF = 0;

            //вычисление значения новой прибыли
            for (int tradeIndex = 0; tradeIndex < countTrades; tradeIndex++)
            {
                agregate = 0;
                for (int varIndex = 0; varIndex < varCount; varIndex++)
                    if (Trades[tradeIndex].variables[varIndex] > numericGen[1, varIndex, 2] &&
                        Trades[tradeIndex].variables[varIndex] < numericGen[1, varIndex, 3])
                        agregate++;
                    else
                        break;

                if (agregate == varCount)
                {
                    SP = SP + Trades[tradeIndex].Points;
                    maxSizePostOptmization++;
                    if (Trades[tradeIndex].Points >= 0)
                        maxCPF++;
                }
            }

            recom.Add($"{SP}\t{maxSizePostOptmization}\t{maxCPF}");

            for (var varIndex = 0; varIndex < varCount; varIndex++)
            {
                string minMargin;
                string maxMargin;
                long downMarginCurrent, downMarginParameters;
                long upMarginCurrent, upMarginParameters;
                if (AgrEx[AgrEx.Length - 1] == 'd' && Parameters.OpenTrendIndex == varIndex)
                {
                    minMargin = Math.Round(-1*numericGen[1, varIndex, 1]*Steps[varIndex], CountDecimal[varIndex]).ToString();
                    maxMargin = Math.Round(-1*numericGen[1, varIndex, 0]*Steps[varIndex], CountDecimal[varIndex]).ToString();
                    downMarginCurrent = numericGen[1, varIndex, 1];
                    upMarginCurrent = numericGen[1, varIndex, 0];
                    downMarginParameters = variableMaxValue[varIndex];
                    upMarginParameters = variableMinValue[varIndex];
                }
                else
                {
                    minMargin = Math.Round(numericGen[1, varIndex, 0]*Steps[varIndex], CountDecimal[varIndex]).ToString();
                    maxMargin = Math.Round(numericGen[1, varIndex, 1]*Steps[varIndex], CountDecimal[varIndex]).ToString();
                    downMarginCurrent = numericGen[1, varIndex, 0];
                    upMarginCurrent = numericGen[1, varIndex, 1];
                    downMarginParameters = variableMinValue[varIndex];
                    upMarginParameters = variableMaxValue[varIndex];
                }

                if (downMarginCurrent == downMarginParameters && upMarginCurrent == upMarginParameters)
                    recom.Add("xxx\txxx\t" + filters[varIndex].Name);

                if (downMarginCurrent == downMarginParameters && upMarginCurrent != upMarginParameters)
                    recom.Add("xxx\t" + maxMargin + '\t' + filters[varIndex].Name);

                if (downMarginCurrent != downMarginParameters && upMarginCurrent == upMarginParameters)
                    recom.Add(minMargin + "\txxx\t" + filters[varIndex].Name);

                if (downMarginCurrent != downMarginParameters && upMarginCurrent != upMarginParameters)
                    recom.Add(minMargin + '\t' + maxMargin + '\t' + filters[varIndex].Name);
            }

            recom.Add("XML-settings:");

            for (var varIndex = 0; varIndex < varCount; varIndex++)
            {
                string minMargin;
                string maxMargin;
                long downMarginCurrent, downMarginParameters;
                long upMarginCurrent, upMarginParameters;
                if (AgrEx[AgrEx.Length - 1] == 'd' && Parameters.OpenTrendIndex == varIndex)
                {
                    int countDecim = Utils.GetDecimalDigitsCount(filters[varIndex].ParsingParameters.Step);
                    minMargin =
                        Math.Round(
                            -1*
                            (numericGen[1, varIndex, 1]*Steps[varIndex]/(float)filters[varIndex].ParsingParameters.Multiplier - (float)filters[varIndex].ParsingParameters.Step),
                            countDecim).ToString();
                    maxMargin =
                        Math.Round(
                            -1*
                            (numericGen[1, varIndex, 0]*Steps[varIndex]/(float)filters[varIndex].ParsingParameters.Multiplier + (float)filters[varIndex].ParsingParameters.Step),
                            countDecim).ToString();
                    downMarginCurrent = numericGen[1, varIndex, 1];
                    upMarginCurrent = numericGen[1, varIndex, 0];
                    downMarginParameters = variableMaxValue[varIndex];
                    upMarginParameters = variableMinValue[varIndex];
                }
                else
                {
                    int countDecim = Utils.GetDecimalDigitsCount(filters[varIndex].ParsingParameters.Step);
                    minMargin =
                        Math.Round(
                            (numericGen[1, varIndex, 0]*Steps[varIndex]/(float)filters[varIndex].ParsingParameters.Multiplier + (float)filters[varIndex].ParsingParameters.Step),
                            countDecim).ToString();
                    maxMargin =
                        Math.Round(
                            (numericGen[1, varIndex, 1]*Steps[varIndex]/ (float)filters[varIndex].ParsingParameters.Multiplier - (float)filters[varIndex].ParsingParameters.Step),
                            countDecim).ToString();
                    downMarginCurrent = numericGen[1, varIndex, 0];
                    upMarginCurrent = numericGen[1, varIndex, 1];
                    downMarginParameters = variableMinValue[varIndex];
                    upMarginParameters = variableMaxValue[varIndex];
                }

                if (downMarginCurrent == downMarginParameters && upMarginCurrent == upMarginParameters)
                    recom.Add("xxx\txxx\t" + filters[varIndex].ParsingParameters.Name);

                if (downMarginCurrent == downMarginParameters && upMarginCurrent != upMarginParameters)
                    recom.Add("xxx\t" + maxMargin + '\t' + filters[varIndex].ParsingParameters.Name);

                if (downMarginCurrent != downMarginParameters && upMarginCurrent == upMarginParameters)
                    recom.Add(minMargin + '\t' + "xxx\t" + filters[varIndex].ParsingParameters.Name);

                if (downMarginCurrent != downMarginParameters && upMarginCurrent != upMarginParameters)
                    recom.Add(minMargin + '\t' + maxMargin + '\t' + filters[varIndex].ParsingParameters.Name);
            }

            recom.SaveToFile(dir + "/Recomends/" + Symbol + "/" + AgrEx + ".txt");
            optimizeSymbol = 0;
            return false;
        }
    }
}
