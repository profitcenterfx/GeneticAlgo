using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace GeneticAlgo
{
    public enum IndexType
    {
        Absolute,
        Relative
    }

    [Serializable]
    public class AgrFeedData
    {
        public SortedDictionary<int, long>[] Data { get; set; }

        public int[] VariablesBitLength { get; set; }
    }
    
    public struct Index
    {
        public IndexType Type;

        public string IndexName;

        public string DivisorIndexName;

        public int Column;

        public int DivisorColumn;

        public Index(IndexType indexType, string indexName, string divisorIndexName, int column, int divisorColumn)
        {
            Type = indexType;
            IndexName = indexName;
            DivisorIndexName = divisorIndexName;
            Column = column;
            DivisorColumn = divisorColumn;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1}", IndexName, Column);
        }
    }



    public class TradeInfoCollection
    {

        public ConcurrentDictionary<string, ConcurrentDictionary<string, List<TradeInfo>>> Trades { get; private set; }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<DateTime, float>>> DaysPoints { get; private set; }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, List<float>>>> DaysProfitExpectancy { get; private set; }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, SortedDictionary<double, List<float>>>>> DealsPoints { get; private set; }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, float>> DealsPointsPercent { get; private set; }

        public SortedDictionary<DateTime, SortedDictionary<double, List<float>>> DealsPointsGlobal { get; private set; }

        public int totalDealsCount;

        public Parameters Parameters { get; private set; }

        private float[] steps;

        public int[] ParametersRoundDigits { get; private set; }

        public Dictionary<DateTime, float> allDates;

        private Dictionary<string, int> parametersIndexes;

        private Filters filters;

        // some key column indexes
        private int symbol = 4;
        private int AgrEx = 18;
        private int Points = 16;
        private int Profit = 15;
        private int Commission = 14;
        private int MAE = 31;
        private int MFE = 32;
        private int OpenTime = 2;
        private int OpenMass = 52;
        private int OpenAgrExMass = 64;
        private int BBOAgrMass = 66;
        private int SumAgrMass = 68;
        private int OpenAccel = 53;
        private int OpenAgrExAccel = 65;
        private int BBOAgrAccel = 67;
        private int AveAgrAccel = 69;
        private int OpenTrendMid = 62;

        public TradeInfoCollection(string tradeLogFilename, Parameters parameters, float[] steps,
            Dictionary<string, int> parametersIndexes, Filters filtersValue)
        {
            Trades = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<TradeInfo>>>();
            DaysPoints = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<DateTime, float>>>();
            DaysProfitExpectancy = new ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, List<float>>>>();
            DealsPoints = new ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, SortedDictionary<double, List<float>>>>>();
            DealsPointsGlobal = new SortedDictionary<DateTime, SortedDictionary<double, List<float>>>();
            DealsPointsPercent = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            Parameters = parameters;
            this.steps = steps;
            this.parametersIndexes = parametersIndexes;
            this.filters = filtersValue;
            LoadTradeData(tradeLogFilename);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        public List<string> GetArgExchanges()
        {
            List<string> result = new List<string>();
            foreach (ConcurrentDictionary<string, List<TradeInfo>> agr in Trades.Values)
            {
                result.AddRange(agr.Keys);
            }
            
            return result.Distinct().ToList();
        }

        public List<TradeInfo> GetTradesList(string symbol, string exchange)
        {
            ConcurrentDictionary<string, List<TradeInfo>> symbolTrades;
            if (!Trades.TryGetValue(symbol, out symbolTrades))
            {
                symbolTrades = new ConcurrentDictionary<string, List<TradeInfo>>();
                Trades[symbol] = symbolTrades;
            }

            List<TradeInfo> agrTrades;
            if (!symbolTrades.TryGetValue(exchange, out agrTrades))
            {
                agrTrades = new List<TradeInfo>();
                symbolTrades[exchange] = agrTrades;
            }
            return agrTrades;
        }

        public void GetDayPoints(string symbol, string exchange, DateTime currentDay, float points, bool isLastDeal)
        {
            if (!DaysPoints.ContainsKey(symbol))
            {
                DaysPoints[symbol] = new ConcurrentDictionary<string, ConcurrentDictionary<DateTime, float>>();
            }

            if (!DaysPoints[symbol].ContainsKey(exchange))
            {
                DaysPoints[symbol][exchange] = new ConcurrentDictionary<DateTime, float>();
                foreach (DateTime Day in allDates.Keys)
                {
                    DaysPoints[symbol][exchange][Day] = 0;
                }
            }

            if (!DaysPoints[symbol][exchange].ContainsKey(currentDay))
            {
                DaysPoints[symbol][exchange][currentDay] = points;
            }
            else
            {
                DaysPoints[symbol][exchange][currentDay] += points;
            }
            if (isLastDeal)
            {
                List<string> exchanges = new List<string>();
                exchanges = GetArgExchanges();

                foreach(string symbol_ in DaysPoints.Keys)
                {
                    for (int index = 0; index < exchanges.Count; index++)
                    {
                        if (!DaysPoints[symbol_].ContainsKey(exchanges[index]))
                        {
                            DaysPoints[symbol_][exchanges[index]] = new ConcurrentDictionary<DateTime, float>();
                            foreach (DateTime Day in allDates.Keys)
                            {
                                DaysPoints[symbol_][exchanges[index]][Day] = 0;
                            }
                        }
                    }
                }
            }
        }

        public void GetDayProfitExpectancy(string symbol, string exchange, DateTime currentDay, float MAE, float MFE,  float points, bool isLastDeal)
        {
            if (!DaysProfitExpectancy.ContainsKey(symbol))
            {
                DaysProfitExpectancy[symbol] = new ConcurrentDictionary<string, SortedDictionary<DateTime, List<float>>>();
            }

            if (!DaysProfitExpectancy[symbol].ContainsKey(exchange))
            {
                DaysProfitExpectancy[symbol][exchange] = new SortedDictionary<DateTime, List<float>>();
            }
            
            if (!DaysProfitExpectancy[symbol][exchange].ContainsKey(currentDay))
            {
                DaysProfitExpectancy[symbol][exchange][currentDay] = new List<float>(8);
                Utils.AddEmptyValues(DaysProfitExpectancy[symbol][exchange][currentDay], 8);
                DaysProfitExpectancy[symbol][exchange][currentDay][0] = MAE;
                DaysProfitExpectancy[symbol][exchange][currentDay][1] = MFE;
                if (points > 0) DaysProfitExpectancy[symbol][exchange][currentDay][2] = 1;
                else DaysProfitExpectancy[symbol][exchange][currentDay][3] = 1;
                DaysProfitExpectancy[symbol][exchange][currentDay][4] = 1;
                DaysProfitExpectancy[symbol][exchange][currentDay][7] += points;
            }
            else
            {
                DaysProfitExpectancy[symbol][exchange][currentDay][0] += MAE;
                DaysProfitExpectancy[symbol][exchange][currentDay][1] += MFE;
                if (points > 0) DaysProfitExpectancy[symbol][exchange][currentDay][2] += 1;
                else DaysProfitExpectancy[symbol][exchange][currentDay][3] += 1;
                DaysProfitExpectancy[symbol][exchange][currentDay][4] += 1;
                DaysProfitExpectancy[symbol][exchange][currentDay][7] += points;
            }
            if (isLastDeal)
            {
                foreach (string symbol_ in DaysProfitExpectancy.Keys)
                {
                    foreach (string agrEx in DaysProfitExpectancy[symbol_].Keys)
                    {
                        foreach (DateTime Day in DaysProfitExpectancy[symbol_][agrEx].Keys)
                        {
                            DaysProfitExpectancy[symbol_][agrEx][Day][0] /= DaysProfitExpectancy[symbol_][agrEx][Day][4];
                            DaysProfitExpectancy[symbol_][agrEx][Day][1] /= DaysProfitExpectancy[symbol_][agrEx][Day][4];
                            DaysProfitExpectancy[symbol_][agrEx][Day][2] /= DaysProfitExpectancy[symbol_][agrEx][Day][4];
                            DaysProfitExpectancy[symbol_][agrEx][Day][3] /= DaysProfitExpectancy[symbol_][agrEx][Day][4];
                            DaysProfitExpectancy[symbol_][agrEx][Day][5] = DaysProfitExpectancy[symbol_][agrEx][Day][0] * DaysProfitExpectancy[symbol_][agrEx][Day][3] +
                            DaysProfitExpectancy[symbol_][agrEx][Day][1] * DaysProfitExpectancy[symbol_][agrEx][Day][2];
                        }
                        int daysCount = 0;
                        float dealsCount = 0;
                        foreach (DateTime Day in allDates.Keys)
                        {
                            if (!DaysProfitExpectancy[symbol_][agrEx].ContainsKey(Day))
                            {
                                DaysProfitExpectancy[symbol_][agrEx][Day] = new List<float>(8);
                                Utils.AddEmptyValues(DaysProfitExpectancy[symbol_][agrEx][Day], 8);
                                if (daysCount > 0) DaysProfitExpectancy[symbol_][agrEx][Day][6] = dealsCount / daysCount;
                            }
                            else
                            {
                                if (daysCount>0) DaysProfitExpectancy[symbol_][agrEx][Day][6] = dealsCount / daysCount;                                
                                dealsCount += DaysProfitExpectancy[symbol_][agrEx][Day][4];
                            }
                            daysCount++; 
                        }
                    }
                }
            }
        }

        public void GetDealPoints(string symbol, string exchangeUD, DateTime closeDealTime, float points)
        {
            string exchange = exchangeUD.Remove(exchangeUD.Length - 1);
            if (!DealsPoints.ContainsKey(symbol))
            {
                DealsPoints[symbol] = new ConcurrentDictionary<string, SortedDictionary<DateTime, SortedDictionary<double, List<float>>>>();
                DealsPointsPercent[symbol] = new ConcurrentDictionary<string, float>();
            }

            if (!DealsPoints[symbol].ContainsKey(exchange))
            {
                DealsPoints[symbol][exchange] = new SortedDictionary<DateTime, SortedDictionary<double, List<float>>>();
            }
            if (!DealsPointsPercent[symbol].ContainsKey(exchangeUD))
            {
                DealsPointsPercent[symbol][exchangeUD] = 0;
            }
            DealsPointsPercent[symbol][exchangeUD]++;
            totalDealsCount++;

            if (!DealsPoints[symbol][exchange].ContainsKey(closeDealTime.Date))
            {
                DealsPoints[symbol][exchange][closeDealTime.Date] = new SortedDictionary<double, List<float>>();
            }
            if (!DealsPointsGlobal.ContainsKey(closeDealTime.Date))
            {
                DealsPointsGlobal[closeDealTime.Date] = new SortedDictionary<double, List<float>>();
            }

            if (!DealsPoints[symbol][exchange][closeDealTime.Date].ContainsKey(closeDealTime.TimeOfDay.TotalSeconds))
            {
                DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds] = new List<float>(2);
                Utils.AddEmptyValues(DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds], 2);
                DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][0] = points;
                DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][1]++;
            }
            else
            {
                DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][0] += points;
                DealsPoints[symbol][exchange][closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][1]++;
            }

            if (!DealsPointsGlobal[closeDealTime.Date].ContainsKey(closeDealTime.TimeOfDay.TotalSeconds))
            {
                DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds] = new List<float>(2);
                Utils.AddEmptyValues(DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds], 2);
                DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][0] = points;
                DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][1]++;
            }
            else
            {
                DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][0] += points;
                DealsPointsGlobal[closeDealTime.Date][closeDealTime.TimeOfDay.TotalSeconds][1]++;
            }
        }

        public void GetTradesListFiltered(string symbol, string exchange, out List<TradeInfo> agrTradesCut, out List<TradeInfo> TradesCutCheck, out List<TradeInfo> TradesCutPrediction)
        {
            float advanceMargin = Parameters.ArbPercentForAdvancePreopting;
            agrTradesCut = new List<TradeInfo>();
            TradesCutCheck = new List<TradeInfo>();
            TradesCutPrediction = new List<TradeInfo>();

            ConcurrentDictionary<string, List<TradeInfo>> symbolTrades;
            if(!Trades.TryGetValue(symbol, out symbolTrades))
                return;

            List<TradeInfo> agrTrades;
            if(!symbolTrades.TryGetValue(exchange, out agrTrades))
                return;

            var agrValue = agrTrades.Select(trade => trade.OpenSlip).Distinct().OrderBy(v => v).ToArray();
            var maxLength = agrValue.Length;
            float arbCount = agrTrades.Sum(trade => trade.isArbitrage ? 1 : 0);
            var openSlipMargins = new List<string>();
            var corFil = new float[maxLength, 6];

            for (int i = 0; i < maxLength; i++)
            {
                corFil[i, 0] = agrValue[i];
                for (int tradeIndex = 0; tradeIndex < agrTrades.Count; tradeIndex++)
                {
                    if (agrTrades[tradeIndex].OpenSlip == corFil[i, 0])
                    {
                        if (agrTrades[tradeIndex].isArbitrage)
                            corFil[i, 1]++;

                        corFil[i, 3]++;
                    }
                }

                if (i > 0)
                {
                    corFil[i, 2] = corFil[i - 1, 2] + corFil[i, 1];
                    corFil[i, 4] = corFil[i - 1, 4] + corFil[i, 3];
                }
                else
                {
                    corFil[i, 2] = corFil[i, 1];
                    corFil[i, 4] = corFil[i, 3];
                }

                if (i == maxLength - 1)
                {
                    corFil[i, 5] = 0;
                    openSlipMargins.Add(symbol + "\t" + exchange + "\t" + Convert.ToString(corFil[i, 0]) + "\t" + Convert.ToString(corFil[i, 5]));
                }
                else
                {
                    corFil[i, 5] = 1 - corFil[i, 2] / corFil[i, 4];
                    openSlipMargins.Add(symbol + "\t" + exchange + "\t" + Convert.ToString(corFil[i, 0]) + "\t" + Convert.ToString(corFil[i, 5]));
                }
            }

            Parameters.MinAdvance = corFil[0, 0] - 1;
            Parameters.MinAdvanceGlobal = corFil[0, 0] - 1;

            for (int i = maxLength - 1; i >= 0; i--)
            {
                if (corFil[i, 5] >= advanceMargin)
                {
                    Parameters.MinAdvance = corFil[i, 0];
                    break;
                }
            }
            
            if (Parameters.MinAdvance > -1) Parameters.MinAdvance = -1;//openSlipetch protect 

            var tempIndexes = new int[agrTrades.Count];
            int counter = -1;
            for (int i = 0; i < agrTrades.Count; i++)
            {
                if (agrTrades[i].OpenSlip > Parameters.MinAdvance)
                {
                    counter++;
                    tempIndexes[counter] = i;
                }
            }

            var indexes = new int[counter + 1];
            for (int i = 0; i < indexes.Length; i++)
                indexes[i] = tempIndexes[i];


            Parameters.CountOfVariable = steps.Length-1;
            Dictionary<DateTime, List<int>> StDev = new Dictionary<DateTime, List<int>>();
            for (int i = 0; i < (int)Math.Round(Parameters.PercentOfOptLog*indexes.Length); i++)
            {
                agrTradesCut.Add(agrTrades[indexes[i]]);
                DateTime currentLogDay = new DateTime();
                currentLogDay = agrTrades[indexes[i]].openDate.Date;
                if (!StDev.ContainsKey(currentLogDay))
                {
                    StDev.Add(currentLogDay, new List<int>());
                }
            }

            for (int i = (int)Math.Round(Parameters.PercentOfOptLog * indexes.Length); i < (int)Math.Round((Parameters.PercentOfOptLog + Parameters.PercentOfCheckLog) * indexes.Length); i++)
            {
                TradesCutCheck.Add(agrTrades[indexes[i]]);
                DateTime currentLogDay = new DateTime();
                currentLogDay = agrTrades[indexes[i]].openDate.Date;
                if (!StDev.ContainsKey(currentLogDay))
                {
                    StDev.Add(currentLogDay, new List<int>());
                }
            }
            Parameters.daysCount = StDev.Count;
            for (int i = (int)Math.Round((Parameters.PercentOfOptLog + Parameters.PercentOfCheckLog) * indexes.Length); i < indexes.Length; i++)
            {
                TradesCutPrediction.Add(agrTrades[indexes[i]]);
                DateTime currentLogDay = new DateTime();
                currentLogDay = agrTrades[indexes[i]].openDate.Date;
                if (!StDev.ContainsKey(currentLogDay))
                {
                    StDev.Add(currentLogDay, new List<int>());
                }
            }
            openSlipMargins.Add(Convert.ToString(Parameters.MinAdvance));
            string path = Directory.GetCurrentDirectory();
            System.IO.File.AppendAllLines(path+"/Recomends/Preopting.txt", openSlipMargins);
        }

        private string GetPower(string mass, string accel)
        {
            double m;
            Utils.TryParse(mass, out m);
            m *= Parameters.mass_multiplier;
            double a;
            Utils.TryParse(accel, out a);
            a *= Parameters.accel_multiplier;
            string s = Convert.ToString(Math.Round(Math.Sqrt(a * a + m * m), 2));
            return s;
        }

        private long GetPowerLong(string mass, string accel, double step)
        {
            double m;
            Utils.TryParse(mass, out m);
            m *= Parameters.mass_multiplier;
            double a;
            Utils.TryParse(accel, out a);
            a *= Parameters.accel_multiplier;
            
            return (long)Math.Round(Math.Sqrt(a * a + m * m) / step, 0);
        }

        private string GetAngle(string mass, string accel)
        {
            double m;
            Utils.TryParse(mass, out m);
            m *= Parameters.mass_multiplier;
            double a;
            Utils.TryParse(accel, out a);
            a *= Parameters.accel_multiplier;
            double angle = (a != 0 || m != 0) ? Math.Atan(m / a) * 180 / Math.PI - 45 : 45;
            if (a < 0) angle += 180;
            if (angle > 180) angle -= 360;
            return Convert.ToString(Math.Round(angle, 2));
        }

        private long GetAngleLong(string mass, string accel, double step)
        {
            double m;
            Utils.TryParse(mass, out m);
            m *= Parameters.mass_multiplier;
            double a;
            Utils.TryParse(accel, out a);
            a *= Parameters.accel_multiplier;
            double angle = (a != 0 || m != 0) ? Math.Atan(m / a) * 180 / Math.PI - 45 : 45;
            if (a < 0) angle += 180;
            if (angle > 180) angle -= 360;
            return (long)Math.Round(angle / step, 0);
        }

        private void LoadTradeData(string tradeLogFilename)
        {
            InitParametersRoundDigits();

            if (!File.Exists(tradeLogFilename))
                return;

            if (!Directory.Exists("bin"))
                Directory.CreateDirectory("bin");

            var indexes = GetIndexes();
            var cultureInfo = CultureInfo.InvariantCulture;
            var firstDealDate = DateTime.MinValue;
            var lastDealDate = DateTime.MinValue;
            allDates = new Dictionary<DateTime, float>();

            {
                #region Collecting aggregated variables values

                var tempAgr = new ConcurrentDictionary<string, SortedDictionary<long, int>[]>();

                using (StreamReader reader = new StreamReader(new FileStream(tradeLogFilename, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024 * 512)))
                {
                    var first = true;
                    reader.ReadLine();
                    var header = reader.ReadLine().Split(new char[] { ';' });
                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine().Split(new char[] { ';' });
                        DateTime date;
                        // if trade date/time can't be parsed correctly, than skip that trade
                        if (!DateTime.TryParse(data[OpenTime], cultureInfo, DateTimeStyles.AssumeLocal, out date))
                            continue;

                        if (!allDates.ContainsKey(date.Date))
                            allDates.Add(date.Date, 0);

                        // skip trade when it's open date/time not within specified interval  
                        if (Parameters.UseTradeLogDateTimeFilter && (date < Parameters.TradeLogFilterStartDateTime || date > Parameters.TradeLogFilterEndDateTime))
                            continue;
                        if (first)
                        {
                            firstDealDate = date;
                            first = false;
                        }

                        lastDealDate = date;

                        var variables = new long[indexes.Length];//points является отдельной переменной и не входит в группу переменных variables
                        string agrExName = "";

                        for (int index = 0; index < indexes.Length; index++)
                        {
                            if (indexes[index].Type == IndexType.Absolute)
                            {
                                switch (indexes[index].IndexName)
                                {
                                    case "Points":
                                        break;
                                    case "FPwr":
                                        variables[index] = GetPowerLong(data[OpenMass], data[OpenAccel], steps[index]);
                                        break;
                                    case "APwr":
                                        variables[index] = GetPowerLong(data[OpenAgrExMass], data[OpenAgrExAccel],
                                            steps[index]);
                                        break;
                                    case "BPwr":
                                        variables[index] = GetPowerLong(data[BBOAgrMass], data[BBOAgrAccel],
                                            steps[index]);
                                        break;
                                    case "SPwr":
                                        variables[index] = GetPowerLong(data[SumAgrMass], data[AveAgrAccel],
                                            steps[index]);
                                        break;
                                    case "FAng":
                                        variables[index] = GetAngleLong(data[OpenMass], data[OpenAccel], steps[index]);
                                        break;
                                    case "AAng":
                                        variables[index] = GetAngleLong(data[OpenAgrExMass], data[OpenAgrExAccel],
                                            steps[index]);
                                        break;
                                    case "BAng":
                                        variables[index] = GetAngleLong(data[BBOAgrMass], data[BBOAgrAccel],
                                            steps[index]);
                                        break;
                                    case "SAng":
                                        variables[index] = GetAngleLong(data[SumAgrMass], data[AveAgrAccel],
                                            steps[index]);
                                        break;
                                    case "OpenTrendMid":
                                        double val;
                                        Utils.TryParse(data[OpenTrendMid], out val);
                                        if (val < 0)
                                        {
                                            agrExName = data[AgrEx] + 'd';
                                            string makePositiveTrend = data[indexes[index].Column].Remove(0, 1);
                                            Utils.ParseLongDualCulture(makePositiveTrend, steps[index],
                                                out variables[index]);
                                        }
                                        else
                                        {
                                            agrExName = data[AgrEx] + 'u';
                                            Utils.ParseLongDualCulture(data[indexes[index].Column], steps[index],
                                                out variables[index]);
                                        }

                                        break;
                                    default:
                                        Utils.ParseLongDualCulture(data[indexes[index].Column], steps[index], out variables[index]);
                                        break;
                                }
                            }
                            else
                            {
                                float val, divisorVal;
                                Utils.ParseFloatDualCulture(data[indexes[index].Column], out val);
                                Utils.ParseFloatDualCulture(data[indexes[index].DivisorColumn], out divisorVal);
                                if (divisorVal != 0)
                                {
                                    variables[index] = (int)GeneMath.Round(val / divisorVal * (1 / steps[index]));
                                }
                                else
                                {
                                    variables[index] = (int)GeneMath.Round(val * 100 * (1 / steps[index]));
                                }
                            }
                        }

                        var symbolExchenge = data[symbol].Replace('/', '_') + "_" + agrExName;

                        SortedDictionary<long, int>[] symAgr;

                        if (!tempAgr.ContainsKey(symbolExchenge))
                        {
                            symAgr = new SortedDictionary<long, int>[indexes.Length];
                            tempAgr[symbolExchenge] = symAgr;
                            for (int i = 0; i < variables.Length; i++)
                                symAgr[i] = new SortedDictionary<long, int>();
                        }
                        else
                            symAgr = tempAgr[symbolExchenge];

                        for (int i = 0; i < variables.Length; i++)
                        {
                            var varAgr = symAgr[i];
                            if (!varAgr.ContainsKey(variables[i]))
                                varAgr[variables[i]] = 1;
                            else
                                varAgr[variables[i]]++;
                        }
                    }
                }

                #endregion

                #region Aggregated values serialization

                var serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                foreach (var symExPair in tempAgr)
                {
                    var symAgr = new AgrFeedData();
                    symAgr.Data = new SortedDictionary<int, long>[indexes.Length];
                    symAgr.VariablesBitLength = new int[symExPair.Value.Length];
                    var varIndex = 0;
                    foreach (var varValues in symExPair.Value)
                    {
                        var varAgr = new SortedDictionary<int, long>();
                        symAgr.Data[varIndex] = varAgr;
                        varValues.Add(varValues.Keys.ElementAt(0) - 1, 0);
                        varValues.Add(varValues.Keys.ElementAt(varValues.Keys.Count - 1) + 1, 0);
                        int i = 0;
                        var coef = varValues.Count == 1 ? 1 : GetNextPowerof2Dec1(varValues.Count - 1) / (float)(varValues.Count - 1);
                        var maxKey = 0;
                        var lastIndex = -1;
                        var lastValue = 0L;
                        foreach (var valuePair in varValues)
                        {
                            var index = (int)Math.Round(i * coef);
                            if(index - lastIndex > 1)
                            {
                                varAgr[index - 1] = lastValue;
                            }
                            varAgr[index] = valuePair.Key;
                            i++;
                            maxKey = index;
                            lastIndex = index;
                            lastValue = valuePair.Key;
                        }
                        foreach(var indexedPair in varAgr)
                        {
                            varValues[indexedPair.Value] = indexedPair.Key;
                        }
                        
                        var bitsCount = GetBitsCount(maxKey);
                        symAgr.VariablesBitLength[varIndex] = (int)bitsCount;
                        varIndex++;
                    }
                    try
                    {
                        using (var stream = new FileStream($"bin\\{symExPair.Key}.bin", FileMode.Create))
                        {
                            serializer.Serialize(stream, symAgr);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
                #endregion

                var logDaysCount = lastDealDate.Date.Subtract(firstDealDate.Date).Days;
                var pointsDivizor = logDaysCount * ((Parameters.DatePriority - 1) == 0 ? 1 : Parameters.DatePriority - 1);

                #region Tradelog parsing

                using (StreamReader reader = new StreamReader(new FileStream(tradeLogFilename, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024 * 512)))
                {
                    reader.ReadLine();
                    var header = reader.ReadLine().Split(new char[] { ';' });
                    while (!reader.EndOfStream)
                    {
                        var data = reader.ReadLine().Split(new char[] { ';' });
                        DateTime date;
                        // if trade date/time can't be parsed correctly, than skip that trade
                        if (!DateTime.TryParse(data[OpenTime], cultureInfo, DateTimeStyles.AssumeLocal, out date))
                            continue;

                        // skip trade when it's open date/time not within specified interval  
                        if (Parameters.UseTradeLogDateTimeFilter && (date < Parameters.TradeLogFilterStartDateTime || date > Parameters.TradeLogFilterEndDateTime))
                            continue;

                        TradeInfo info = new TradeInfo();
                        info.openDate = date;
                        var pointsDays = date.Date.Subtract(firstDealDate.Date).Days;
                        var pointsMultiplier = (pointsDays / pointsDivizor + 1);
                        Utils.ParseFloatDualCulture(data[MAE], out info.MAE);
                        Utils.ParseFloatDualCulture(data[MFE], out info.MFE);

                        info.variables = new int[indexes.Length];//points является отдельной переменной и не входит в группу переменных variables
                        var variables = new long[indexes.Length];
                        string agrExName = "";

                        float dealPoints = 0;

                        for (int index = 0; index < indexes.Length; index++)
                        {
                            if (indexes[index].Type == IndexType.Absolute)
                            {
                                switch (indexes[index].IndexName)
                                {
                                    case "Points":
                                        float commission;
                                        Utils.ParseFloatDualCulture(data[indexes[index].Column], out info.Points);
                                        Utils.ParseFloatDualCulture(data[Profit], out info.Profit);
                                        Utils.ParseFloatDualCulture(data[Commission], out commission);
                                        if (info.Profit != 0)
                                        {
                                            info.PointsWithCommision = info.Points;
                                            if (!Parameters.BTlog)
                                            {
                                                dealPoints = info.Points * (1 + commission / info.Profit) > 50 && Parameters.ProfitLimit ? 50 : info.Points * (1 + commission / info.Profit);
                                            }
                                            else
                                            {
                                                dealPoints = info.Points + commission > 50 && Parameters.ProfitLimit ? 50 : info.Points + commission;
                                            }
                                            info.Points = dealPoints * pointsMultiplier;
                                        }
                                        break;
                                    case "FPwr":
                                        variables[index] = GetPowerLong(data[OpenMass], data[OpenAccel], steps[index]);
                                        break;
                                    case "APwr":
                                        variables[index] = GetPowerLong(data[OpenAgrExMass], data[OpenAgrExAccel], steps[index]);
                                        break;
                                    case "BPwr":
                                        variables[index] = GetPowerLong(data[BBOAgrMass], data[BBOAgrAccel], steps[index]);
                                        break;
                                    case "SPwr":
                                        variables[index] = GetPowerLong(data[SumAgrMass], data[AveAgrAccel], steps[index]);
                                        break;
                                    case "FAng":
                                        variables[index] = GetAngleLong(data[OpenMass], data[OpenAccel], steps[index]);
                                        break;
                                    case "AAng":
                                        variables[index] = GetAngleLong(data[OpenAgrExMass], data[OpenAgrExAccel], steps[index]);
                                        break;
                                    case "BAng":
                                        variables[index] = GetAngleLong(data[BBOAgrMass], data[BBOAgrAccel], steps[index]);
                                        break;
                                    case "SAng":
                                        variables[index] = GetAngleLong(data[SumAgrMass], data[AveAgrAccel], steps[index]);
                                        break;
                                    case "OpenTrendMid":
                                        double val;
                                        Utils.TryParse(data[OpenTrendMid], out val);
                                        if (val < 0)
                                        {
                                            agrExName = data[AgrEx] + 'd';
                                            string makePositiveTrend = data[indexes[index].Column].Remove(0, 1);
                                            Utils.ParseLongDualCulture(makePositiveTrend, steps[index], out variables[index]);
                                        }
                                        else
                                        {
                                            agrExName = data[AgrEx] + 'u';
                                            Utils.ParseLongDualCulture(data[indexes[index].Column], steps[index], out variables[index]);
                                        }

                                        break;
                                    case "OpenSlip":
                                        Utils.ParseFloatDualCulture(data[indexes[index].Column], out info.OpenSlip);
                                        Utils.ParseLongDualCulture(data[indexes[index].Column], steps[index], out variables[index]);
                                        break;
                                    default:
                                        Utils.ParseLongDualCulture(data[indexes[index].Column], steps[index], out variables[index]);
                                        break;
                                }
                            }
                            else
                            {
                                float val, divisorVal;
                                Utils.ParseFloatDualCulture(data[indexes[index].Column], out val);
                                Utils.ParseFloatDualCulture(data[indexes[index].DivisorColumn], out divisorVal);
                                if (divisorVal != 0)
                                {
                                    variables[index] = (int)GeneMath.Round(val / divisorVal * (1 / steps[index]));
                                }
                                else
                                {
                                    variables[index] = (int)GeneMath.Round(val * 100 * (1 / steps[index]));
                                }
                            }
                        }

                        var symbolExchenge = data[symbol].Replace('/', '_') + "_" + agrExName;
                        var symAgr = tempAgr[symbolExchenge];

                        for (int index = 0; index < info.variables.Length; index++)
                        {
                            info.variables[index] = symAgr[index][variables[index]];
                        }

                        info.isArbitrage = Parameters.UseAllFilters ? info.Points > 0 : (((float)Math.Abs(info.MFE / info.MAE) > 2 || info.MAE == 0) && info.Points > 0);

                        List<TradeInfo> agrTrades = GetTradesList(data[symbol], agrExName);
                        agrTrades.Add(info);

                        bool isLastDeal = false;
                        if (reader.EndOfStream) isLastDeal = true;
                        if (Parameters.isVolumeSet)
                        {
                            GetDayPoints(data[symbol], agrExName, date.Date, dealPoints, isLastDeal);
                            GetDayProfitExpectancy(data[symbol], agrExName, date.Date, info.MAE, info.MFE, dealPoints, isLastDeal);
                        }
                        if (Parameters.isMaxDDSet)
                        {
                            DateTime tradeClosed;
                            // if trade date/time can't be parsed correctly, than skip that trade
                            if (DateTime.TryParse(data[9], cultureInfo, DateTimeStyles.AssumeLocal, out tradeClosed))
                            {
                                GetDealPoints(data[symbol], agrExName, tradeClosed, dealPoints);
                            }
                        }
                    }
                }
                #endregion

            }

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        public static float GetNextPowerof2Dec1(int n)
        {
            var result = n;
            var cnt = 0;
            while (result > 0)
            {
                result = result >> 1;
                cnt++;
            }
            result = (1 << cnt) - 1;
            return result;
        }

        private int GetBitsCount(long n)
        {
            var result = n;
            var cnt = 0;
            while (result > 0)
            {
                result = result >> 1;
                cnt++;
            }
            return cnt;
        }

        private Index[] GetIndexes()
        {
            // find index/column mappings
            Index[] indexes = new Index[filters.Count + 1];
            for (int i = 0; i < filters.Count; i++)
            {
                string indexName, divisorIndexName;
                IndexType indexType = filters.filters[i].Name.Contains("\\") ? IndexType.Relative : IndexType.Absolute;

                if (indexType == IndexType.Absolute)
                {
                    indexName = filters.filters[i].Name;
                    divisorIndexName = "";
                }
                else
                {
                    string[] tmp = filters.filters[i].Name.Split(new char[] {'\\'});
                    indexName = tmp[0];
                    divisorIndexName = tmp[1];
                }

                int dividentIndex, dividerIndex;
                if (!parametersIndexes.TryGetValue(indexName, out dividentIndex))
                    dividentIndex = -1;
                if (!parametersIndexes.TryGetValue(divisorIndexName, out dividerIndex))
                    dividerIndex = -1;

                indexes[i] = new Index(indexType, indexName, divisorIndexName, dividentIndex, dividerIndex);
            }

            indexes[filters.Count] = new Index(IndexType.Absolute, "Points", "", Points, -1);

            for (int index = 0; index < indexes.Length; index++)
            {
                if (indexes[index].IndexName == "OpenTrendMid") Parameters.OpenTrendIndex = index;
            }
            return indexes;
        }

        private void InitParametersRoundDigits()
        {
            ParametersRoundDigits = new int[steps.Length];
            for (int i = 0; i < ParametersRoundDigits.Length; i++)
                ParametersRoundDigits[i] = (int) GeneMath.Round(1/(steps[i]*10), 0).ToString().Length;
        }
    }
}
