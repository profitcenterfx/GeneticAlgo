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
    class VolumesSetter
    {
        private Parameters parameters;

        private float baseExpVar = -4f;
               
        public VolumesSetter(Parameters Parameters)
        {
            parameters = Parameters;
        }


        public ConcurrentDictionary<string, ConcurrentDictionary<string, float>> CalculateAveragePointsPerDay(ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<DateTime, float>>> DaysPoints,
            int daysCount)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> averagePoints;
            averagePoints = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            foreach (string symbol in DaysPoints.Keys)
            {
                foreach (string agrEx in DaysPoints[symbol].Keys)
                {
                    float sumPoints = 0;
                    for (int index = 0; index < daysCount; index++)
                    {
                        sumPoints += DaysPoints[symbol][agrEx].ElementAt(index).Value;
                    }
                    if (!averagePoints.ContainsKey(symbol)) averagePoints[symbol] = new ConcurrentDictionary<string, float>();
                    averagePoints[symbol][agrEx] = sumPoints / daysCount;
                }
            }
            return averagePoints;

        }
        
        public ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>> GetCovariationMatrix
            (ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<DateTime, float>>> DaysPoints,
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> averagePoints, int daysCount)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>> CovariationMatrix;
            CovariationMatrix = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>>();
            foreach (string symbol in DaysPoints.Keys)
            {              
                if (!CovariationMatrix.ContainsKey(symbol)) CovariationMatrix[symbol] = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>();
                
                foreach (string agrEx in DaysPoints[symbol].Keys)
                {
                    if (!CovariationMatrix[symbol].ContainsKey(agrEx)) CovariationMatrix[symbol][agrEx] = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
                    
                    foreach (string symbol2 in DaysPoints.Keys)
                    {
                        if (!CovariationMatrix[symbol][agrEx].ContainsKey(symbol2)) CovariationMatrix[symbol][agrEx][symbol2] = new ConcurrentDictionary<string, float>();
                        if (!CovariationMatrix.ContainsKey(symbol2)) CovariationMatrix[symbol2] = new ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>();
                        
                        foreach (string agrEx2 in DaysPoints[symbol].Keys)
                        {
                            if (!CovariationMatrix[symbol2].ContainsKey(agrEx2)) CovariationMatrix[symbol2][agrEx2] = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
                            if (!CovariationMatrix[symbol2][agrEx2].ContainsKey(symbol)) CovariationMatrix[symbol2][agrEx2][symbol] = new ConcurrentDictionary<string, float>();

                            if (!CovariationMatrix[symbol][agrEx][symbol2].ContainsKey(agrEx2) || !CovariationMatrix[symbol2][agrEx2][symbol].ContainsKey(agrEx))
                            {
                                float currentCovariation = 0;
                                for (int index = 0; index < daysCount; index++)
                                {
                                    float dayPointsFirstFeed = DaysPoints[symbol].ContainsKey(agrEx) ? DaysPoints[symbol][agrEx].ElementAt(index).Value : 0;
                                    float dayPointsSecondFeed = DaysPoints[symbol2].ContainsKey(agrEx2) ? DaysPoints[symbol2][agrEx2].ElementAt(index).Value : 0;
                                    float averagePointsFirstFeed = averagePoints[symbol].ContainsKey(agrEx) ? averagePoints[symbol][agrEx] : 0;
                                    float averagePointsSecondFeed = averagePoints[symbol2].ContainsKey(agrEx2) ? averagePoints[symbol2][agrEx2] : 0;
                                    currentCovariation += (dayPointsFirstFeed - averagePointsFirstFeed) * (dayPointsSecondFeed - averagePointsSecondFeed);
                                }
                                currentCovariation /= daysCount;
                                CovariationMatrix[symbol][agrEx][symbol2][agrEx2] = currentCovariation;
                                CovariationMatrix[symbol2][agrEx2][symbol][agrEx] = currentCovariation;
                            }
                        }                   
                    }
                }
            }
            return CovariationMatrix;
        }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, int>> GetVolumes 
            (ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>> covariationMatrix,
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> averagePoints)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, int>> feedVolumes;
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables;
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> weights;
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariablesSteps;
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> lambda;
            feedVolumes = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
            expVariables = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            weights = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            expVariablesSteps = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            lambda = new ConcurrentDictionary<string,ConcurrentDictionary<string,float>>();
            float epsilon = parameters.Epsilon;
         
            foreach (string symbol in averagePoints.Keys)
            {
                if (!expVariables.ContainsKey(symbol)) expVariables[symbol] = new ConcurrentDictionary<string, float>();
                if (!expVariablesSteps.ContainsKey(symbol)) expVariablesSteps[symbol] = new ConcurrentDictionary<string, float>();
                if (!lambda.ContainsKey(symbol)) lambda[symbol] = new ConcurrentDictionary<string, float>();
                foreach (string agrEx in averagePoints[symbol].Keys) 
                {
                    expVariables[symbol][agrEx] = baseExpVar;
                    lambda[symbol][agrEx] = parameters.Lambda;
                }
            }
            float FF = 0;
            float tempFF = 0;
            float incFF = epsilon;
            float sumExp = GetSumExp(expVariables);
            float sumPointsExp = GetSumPointsExp(expVariables, averagePoints);
            float sumCovariationsByWeights = GetSumCovariationsByWeights(expVariables, covariationMatrix);
            int index = 0;
            while (incFF >= epsilon && incFF > 0)
            {
                if (index == 0) FF = (sumPointsExp - Convert.ToSingle(Math.Sqrt(sumCovariationsByWeights))) / sumExp;
                else FF = tempFF;
                foreach (string symbol in averagePoints.Keys)
                {
                    foreach (string agrEx in averagePoints[symbol].Keys)
                    {
                        float sumCurrentCovariationsByWeights = GetSumCurrentCovariationsByWeights(expVariables, covariationMatrix, symbol, agrEx);
                        expVariablesSteps[symbol][agrEx] = expVariables[symbol][agrEx] * (averagePoints[symbol][agrEx] * sumExp - sumPointsExp +
                            (sumCovariationsByWeights - sumExp * sumCurrentCovariationsByWeights) / Convert.ToSingle(Math.Sqrt(sumCovariationsByWeights))) /
                            (sumExp * sumExp);
                    }
                }
                foreach (string symbol in averagePoints.Keys)
                {
                    foreach (string agrEx in averagePoints[symbol].Keys)
                    {
                        expVariables[symbol][agrEx] -= lambda[symbol][agrEx] * expVariablesSteps[symbol][agrEx];
                    }
                }
                sumExp = GetSumExp(expVariables);
                sumPointsExp = GetSumPointsExp(expVariables, averagePoints);
                sumCovariationsByWeights = GetSumCovariationsByWeights(expVariables, covariationMatrix);
                tempFF = (sumPointsExp - Convert.ToSingle(Math.Sqrt(sumCovariationsByWeights))) / sumExp;
                if (index > 0) incFF = tempFF - FF;
                index++;
                if (true) lambda = GetCurrentLambda(expVariables);
            }

            weights = GetWeights(expVariables);
            feedVolumes = CalculateVolumes(weights);
            return feedVolumes;
        }

        private float GetSumExp(ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables)
        {
            float sumExp = 0;
            foreach (string symbol in expVariables.Keys)
            {
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    sumExp += Convert.ToSingle(Math.Exp(expVariables[symbol][agrEx]));
                }
            }
            return sumExp;
        }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, float>> GetCurrentLambda(ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables)
        {
            float minDev = 0;
            float maxDev = 0;
            float lambda = parameters.Lambda;
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> currentLambda = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            foreach (string symbol in expVariables.Keys)
            {
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    if (maxDev == 0 && minDev == 0)
                    {
                        maxDev = Math.Abs(expVariables[symbol][agrEx] - baseExpVar);
                        minDev = Math.Abs(expVariables[symbol][agrEx] - baseExpVar);
                    }
                    else
                    {
                        if (Math.Abs(expVariables[symbol][agrEx] - baseExpVar) > maxDev) maxDev = Math.Abs(expVariables[symbol][agrEx] - baseExpVar);
                        if (Math.Abs(expVariables[symbol][agrEx] - baseExpVar) < minDev) minDev = Math.Abs(expVariables[symbol][agrEx] - baseExpVar);
                    }
                }
            }
            float MaxMinSum = maxDev + minDev;
            foreach (string symbol in expVariables.Keys)
            {
                if (!currentLambda.ContainsKey(symbol)) currentLambda[symbol] = new ConcurrentDictionary<string, float>();
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    currentLambda[symbol][agrEx] = lambda * (MaxMinSum - Math.Abs(expVariables[symbol][agrEx] - baseExpVar))/maxDev;
                }
            }
            return currentLambda;
        }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, float>> GetWeights
            (ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> weights;
            weights = new ConcurrentDictionary<string, ConcurrentDictionary<string, float>>();
            float sumExp = GetSumExp(expVariables);
            //float sumWeights = 0;
            foreach (string symbol in expVariables.Keys)
            {
                if (!weights.ContainsKey(symbol)) weights[symbol] = new ConcurrentDictionary<string, float>();
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    weights[symbol][agrEx] = Convert.ToSingle(Math.Exp(expVariables[symbol][agrEx]) / sumExp);
                    //sumWeights += weights[symbol][agrEx];
                }
            }
            return weights;
        }


        private float GetSumPointsExp(ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables,
            ConcurrentDictionary<string, ConcurrentDictionary<string, float>> averagePoints)
        {
            float sumPointsExp = 0;
            foreach (string symbol in expVariables.Keys)
            {
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    sumPointsExp += Convert.ToSingle(Math.Exp(expVariables[symbol][agrEx])*averagePoints[symbol][agrEx]);
                }
            }
            return sumPointsExp;
        }

        private float GetSumCovariationsByWeights(ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables,
            ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>> covariationMatrix)
        {
            float SumCovariationsByWeights = 0;
            foreach (string symbol in expVariables.Keys)
            {
                foreach (string agrEx in expVariables[symbol].Keys)
                {
                    foreach (string symbol_ in expVariables.Keys)
                    {
                        foreach (string agrEx_ in expVariables[symbol_].Keys)
                        {
                            SumCovariationsByWeights += Convert.ToSingle(Math.Exp(expVariables[symbol][agrEx]) * Math.Exp(expVariables[symbol_][agrEx_]) *
                                covariationMatrix[symbol][agrEx][symbol_][agrEx_]);
                        }
                    }                 
                }
            }
            return SumCovariationsByWeights;
        }

        private float GetSumCurrentCovariationsByWeights(ConcurrentDictionary<string, ConcurrentDictionary<string, float>> expVariables,
            ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, ConcurrentDictionary<string, float>>>> covariationMatrix,
            string symbol, string agrEx)
        {
            float SumCurrentCovariationsByWeights = 0;
            foreach (string symbol_ in expVariables.Keys)
            {
                foreach (string agrEx_ in expVariables[symbol].Keys)
                {
                    SumCurrentCovariationsByWeights += Convert.ToSingle(Math.Exp(expVariables[symbol_][agrEx_]) *
                        covariationMatrix[symbol][agrEx][symbol_][agrEx_]);
                }
            }
            return SumCurrentCovariationsByWeights;
        }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, int>> CalculateVolumes
            (ConcurrentDictionary<string, ConcurrentDictionary<string, float>> Weights)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, int>> volumes;
            volumes = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
            int maxDealVolume = parameters.MaxDealVolume;
            SortedList<int, float> volumeLevels;
            volumeLevels = new SortedList<int, float>();
            volumeLevels[150000] = 0;
            volumeLevels[100000] = 25000;
            volumeLevels[50000] = 25000;
            volumeLevels[25000] =12500;
            volumeLevels[10000] = 7500;
            volumeLevels[5000] = 2500;
            volumeLevels[1000] = 2000;

            float minWeight = 1f;
            float maxWeight = 0f;

            foreach (string symbol in Weights.Keys)
            {
                foreach (string agrEx in Weights[symbol].Keys)
                {
                    if (Weights[symbol][agrEx] < minWeight) minWeight = Weights[symbol][agrEx];
                    if (Weights[symbol][agrEx] > maxWeight) maxWeight = Weights[symbol][agrEx];
                }
            }

            float deltaWeight = maxWeight - minWeight;
            float volumeKoef = (maxDealVolume - 1000)/deltaWeight;

            foreach (string symbol in Weights.Keys)
            {
                if (!volumes.ContainsKey(symbol)) volumes[symbol] = new ConcurrentDictionary<string, int>();
                foreach (string agrEx in Weights[symbol].Keys)
                {
                    float currentVolume = (Weights[symbol][agrEx] - minWeight) * volumeKoef + 1000;
                    for (int index = volumeLevels.Count - 1; index >= 0; index--)
                    {
                        if (currentVolume >= volumeLevels.ElementAt(index).Key)
                        {
                            if (currentVolume - volumeLevels.ElementAt(index).Key > volumeLevels.ElementAt(index).Value)
                                volumes[symbol][agrEx] = index != volumeLevels.Count - 1 ? volumeLevels.ElementAt(index + 1).Key : volumeLevels.ElementAt(index).Key;
                            else volumes[symbol][agrEx] = volumeLevels.ElementAt(index).Key;
                            break;
                        }
                    }
                }
            }

            return volumes;
        }

        public ConcurrentDictionary<string, ConcurrentDictionary<string, int>> CalculateVolumes322
            (ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, List<float>>>> DaysProfitExpectancy)
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, int>> volumes;
            volumes = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
            List<int> volumeLevels;
            volumeLevels = new List<int>();
            volumeLevels.Add(1000);
            volumeLevels.Add(5000);
            volumeLevels.Add(10000);
            volumeLevels.Add(25000);
            volumeLevels.Add(50000);
            volumeLevels.Add(100000);
            volumeLevels.Add(150000);
            volumeLevels.Add(200000);
            volumeLevels.Add(250000);
            volumeLevels.Add(500000);
            volumeLevels.Add(750000);
            volumeLevels.Add(1000000);
            
            int maxDealVolume = parameters.MaxDealVolume322;
            int minDealVolume = parameters.MinDealVolume322;
            float ImportanceDealPercent = parameters.ImportanceDealPercent;
            int ImportanceDealCount = parameters.ImportanceDealCount;
            int ZeroDaysLimit = parameters.ZeroDaysForDecrVol;
            bool NetPointsProfitDay = parameters.NetPointsProfitDay;

            for (int i = 0; i < volumeLevels.Count; i++)
            {
                if (volumeLevels[i] < minDealVolume && volumeLevels[i + 1] < minDealVolume)
                {
                    volumeLevels.RemoveAt(i);
                    i--;
                }
                else
                {
                    volumeLevels[i] = minDealVolume;
                    break;
                }
            }

            DateTime firstDay = parameters.SetVolumeStartDateTime.Date;
            DateTime lastDay = parameters.SetVolumeEndDateTime.Date;
            float PEmargin = parameters.MinPE;
            int dealsCount = parameters.DealsForVolChange322;
            int daysCount = parameters.DaysForVolChange322;

            foreach (string symbol in DaysProfitExpectancy.Keys)
            {
                if (!volumes.ContainsKey(symbol)) volumes[symbol] = new ConcurrentDictionary<string, int>();
                foreach (string agrEx in DaysProfitExpectancy[symbol].Keys)
                {
                    int index = 0;
                    int volumeLevel = 0;
                    int zeroDaysCount = 0;
                    foreach (DateTime day in DaysProfitExpectancy[symbol][agrEx].Keys)
                    {
                        if (day >= firstDay && day <= lastDay)
                        {
                            /*новые условия повышения/понижения лотажа*/
                            float ProfitDay = NetPointsProfitDay ? DaysProfitExpectancy[symbol][agrEx][day][7] : DaysProfitExpectancy[symbol][agrEx][day][5];

                            if (DaysProfitExpectancy[symbol][agrEx][day][4] == 0)
                            {
                                zeroDaysCount++;
                            }
                            else
                            {
                                if (zeroDaysCount != 0) zeroDaysCount = 0;
                            }

                            //считаем количество последовательных дней для повышения лотажа
                            if (ProfitDay > PEmargin 
                                && DaysProfitExpectancy[symbol][agrEx][day][4] > ImportanceDealPercent * DaysProfitExpectancy[symbol][agrEx][day][6] / 100
                                && DaysProfitExpectancy[symbol][agrEx][day][4] > ImportanceDealCount)
                            {
                                if (index < 0) index = 0;
                                index++;
                            }
                            //обнуляем счетчик на "слабом" прибыльном дне
                            if (parameters.UseImportanceDeals && ProfitDay > PEmargin 
                                && (DaysProfitExpectancy[symbol][agrEx][day][4] <= ImportanceDealPercent * DaysProfitExpectancy[symbol][agrEx][day][6] / 100
                                || DaysProfitExpectancy[symbol][agrEx][day][4] <= ImportanceDealCount))
                            {
                                index = 0;
                            }
                            //считаем количество последовательных дней для понижения лотажа
                            if (ProfitDay <= PEmargin && DaysProfitExpectancy[symbol][agrEx][day][4] != 0)
                            {
                                if (index > 0) index = 0;
                                index--;
                            }
                            /*новые условия повышения/понижения лотажа (конец)*/

                            //проверяем условия для повышения лотажа с "нуля" отсчета
                            if (volumeLevel == 0 && index == Math.Round(daysCount * 1.5))
                            {
                                volumeLevel++;
                                index = 0;
                            }
                            //проверяем условия для очередного повышения лотажа
                            if (volumeLevel > 0 && index == daysCount && volumeLevels[volumeLevel] < maxDealVolume)
                            {
                                volumeLevel++;
                                index = 0;
                            }
                            //проверяем условия для очередного понижения лотажа
                            if (volumeLevel > 0 && (index == -daysCount || zeroDaysCount == ZeroDaysLimit))
                            {
                                volumeLevel--;
                                index = 0;
                            }
                        }
                    }
                    volumes[symbol][agrEx] = volumeLevels[volumeLevel];
                }
            }

            return volumes;
        }
        public void GetXMLwithVolumes(string currentSettings, ConcurrentDictionary<string, ConcurrentDictionary<string, int>> volumes)
        {
            string dir, settingsPath;
            
            dir = Environment.CurrentDirectory;
            settingsPath = currentSettings.Substring(0, currentSettings.IndexOf(".xml")) + "_volume.xml";
            FileStream fs = File.Create(settingsPath);
            fs.Close();
            StreamReader sr;
            sr = new StreamReader(currentSettings);
            List<string> source = new List<string>();
            source.AddRange(sr.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            sr.Close();

            List<string> xmlTxt = new List<string>();
            
            bool isAggr = true;
            string symbol="", agrEx="";
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].IndexOf("<symbols>") != -1) isAggr = false;
                if (source[i].IndexOf("symbol name") != -1 && isAggr)
                {
                    symbol = source[i].Substring(source[i].IndexOf("<symbol name=\"") + "<symbol name=\"".Length,
                            source[i].IndexOf("\">") - (source[i].IndexOf("<symbol name=\"") + "<symbol name=\"".Length));
                }
                if (source[i].IndexOf("exchange name") != -1 && isAggr)
                {
                    agrEx = source[i].Substring(source[i].IndexOf("<exchange name=\"") + "<exchange name=\"".Length,
                            source[i].IndexOf("\">") - (source[i].IndexOf("<exchange name=\"") + "<exchange name=\"".Length));
                }
                if (source[i].IndexOf("<volume>") != -1 && isAggr)
                {
                    if (volumes.ContainsKey(symbol) && volumes[symbol].ContainsKey(agrEx))
                    {
                        string firstPart = source[i].Substring(0, source[i].IndexOf("<volume>") + "<volume>".Length);
                        string secondPart = source[i].Substring(source[i].IndexOf("</volume>"));
                        source[i] = firstPart + Convert.ToString(volumes[symbol][agrEx]) + secondPart;
                    }

                }
                xmlTxt.Add(source[i]);
            }
            xmlTxt.SaveToFile(settingsPath);
        }
    
    
    }
}
