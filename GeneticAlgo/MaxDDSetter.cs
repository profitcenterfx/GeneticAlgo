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
    class MaxDDSetter
    {

        private Parameters parameters;

        private string currentSettings;

        private int minDD = 0;

        private int minDDdelay;

        private int TotalDeals;

        private ConcurrentDictionary<string, ConcurrentDictionary<string, float>> DealsPointsPersent;

        private ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, SortedDictionary<double, List<float>>>>> DealsPoints;

        private SortedDictionary<DateTime, SortedDictionary<double, List<float>>> GlobalDealsPoints;

        private ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<List<double>>>>> secondsSeries;

        public MaxDDSetter(Parameters Parameters, ConcurrentDictionary<string, ConcurrentDictionary<string, SortedDictionary<DateTime, SortedDictionary<double, List<float>>>>> dealsPoints,
            string currentSettingsFile, SortedDictionary<DateTime, SortedDictionary<double, List<float>>> globalDealsPoints, ConcurrentDictionary<string, ConcurrentDictionary<string, float>> dealsPointsPercent,
            int totalDeals)
        {
            parameters = Parameters;
            DealsPoints = dealsPoints;
            currentSettings = currentSettingsFile;
            GlobalDealsPoints = globalDealsPoints;
            DealsPointsPersent = dealsPointsPercent;
            TotalDeals = totalDeals;
        }

        private float GetAverageTradeVolume()
        {
            float volume=0;
            foreach (string symbol in DealsPointsPersent.Keys)
            {
                foreach (string agrEx in DealsPointsPersent[symbol].Keys)
                {
                    try
                    {
                        volume += Convert.ToSingle(DealsPointsPersent[symbol][agrEx] * GeneticAlgo.Settings.aggregator[symbol][agrEx].volume);
                    }
                    catch { }
                }
            }
            volume /= TotalDeals;
            return volume;
        }
        private void CalculateCumulativeSeries(ref ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>> cumulativesSeries,
            SortedDictionary<DateTime, SortedDictionary<double, List<float>>> dealsPoints, string symbol, string agrEx)
        {
            int counter = 0;
            bool addSeries = true;
            
            foreach (DateTime day in dealsPoints.Keys)
            {
                if (day < parameters.SetVolumeStartDateTime || day > parameters.SetVolumeEndDateTime)
                {
                    continue;
                }
                float cumulative = 0;
                float cumulative2 = 0;
                float cumulativeSqr = 0;
                double closeSec = 0;
                float index = 0;
                addSeries = true;
                List<double> closeSecSeriesCurrent = new List<double>();

                foreach (double ttlSec in dealsPoints[day].Keys)
                {
                    if (dealsPoints[day][ttlSec][0] < 0)
                    {
                        index += dealsPoints[day][ttlSec][1];
                    }
                    else
                    {
                        if (index >= 2)
                        {
                            cumulativesSeries[symbol][agrEx][counter - 1][0] = index;
                            cumulativesSeries[symbol][agrEx][counter - 1][1] = cumulative;
                            cumulativesSeries[symbol][agrEx][counter - 1][2] = cumulative2;
                            cumulativesSeries[symbol][agrEx][counter - 1][3] = cumulative2 / index;
                            cumulativesSeries[symbol][agrEx][counter - 1][4] = cumulativeSqr;
                            cumulativesSeries[symbol][agrEx][counter - 1][5] = cumulativeSqr / index;
                            cumulativesSeries[symbol][agrEx][counter - 1][6] = cumulativesSeries[symbol][agrEx][counter - 1][3] -
                                Convert.ToSingle(Math.Sqrt(cumulativesSeries[symbol][agrEx][counter - 1][5] - cumulativesSeries[symbol][agrEx][counter - 1][3] *
                                cumulativesSeries[symbol][agrEx][counter - 1][3]));
                            cumulativesSeries[symbol][agrEx][counter - 1][8] = Convert.ToSingle(closeSec);                           
                            float tempCumulative = 0;
                            for (int i = 0; i < closeSecSeriesCurrent.Count; i++)
                            {
                                secondsSeries[symbol][agrEx][counter - 1].Add(new List<double>(2));
                                Utils.AddEmptyValues(secondsSeries[symbol][agrEx][counter - 1][i], 2);
                                tempCumulative += dealsPoints[day][closeSecSeriesCurrent[i]][0];
                                secondsSeries[symbol][agrEx][counter - 1][i][0] = Convert.ToDouble(closeSecSeriesCurrent[i]);
                                secondsSeries[symbol][agrEx][counter - 1][i][1] = tempCumulative;
                            }
                        }                    
                        index = 0;
                        cumulative = 0;
                        cumulative2 = 0;
                        cumulativeSqr = 0;
                        addSeries = true;
                        closeSec = 0;
                        closeSecSeriesCurrent.Clear();
                    }

                    if (index >= 2 && addSeries)
                    {
                        counter++;
                        addSeries = false;
                        secondsSeries[symbol][agrEx].Add(new List<List<double>>());
                        cumulativesSeries[symbol][agrEx].Add(new List<float>(10));
                        Utils.AddEmptyValues(cumulativesSeries[symbol][agrEx][counter - 1], 10);
                    }

                    if (dealsPoints[day][ttlSec][0] < 0)
                    {
                        cumulative2 += (1 + dealsPoints[day][ttlSec][1]) *
                            dealsPoints[day][ttlSec][0] / 2 + cumulative * dealsPoints[day][ttlSec][1];
                        cumulativeSqr += dealsPoints[day][ttlSec][1] * cumulative * cumulative +
                            dealsPoints[day][ttlSec][0] * (1 + dealsPoints[day][ttlSec][1]) *
                            (cumulative + dealsPoints[day][ttlSec][0] * (2 * dealsPoints[day][ttlSec][1] + 1) /
                            (6 * dealsPoints[day][ttlSec][1]));
                        cumulative += dealsPoints[day][ttlSec][0];
                        closeSec = ttlSec;
                        closeSecSeriesCurrent.Add(closeSec);
                    }
                }

                if (!addSeries)
                {
                    cumulativesSeries[symbol][agrEx][counter - 1][0] = index;
                    cumulativesSeries[symbol][agrEx][counter - 1][1] = cumulative;
                    cumulativesSeries[symbol][agrEx][counter - 1][2] = cumulative2;
                    cumulativesSeries[symbol][agrEx][counter - 1][3] = cumulative2 / index;
                    cumulativesSeries[symbol][agrEx][counter - 1][4] = cumulativeSqr;
                    cumulativesSeries[symbol][agrEx][counter - 1][5] = cumulativeSqr / index;
                    cumulativesSeries[symbol][agrEx][counter - 1][6] = cumulativesSeries[symbol][agrEx][counter - 1][3] -
                        Convert.ToSingle(Math.Sqrt(cumulativesSeries[symbol][agrEx][counter - 1][5] - cumulativesSeries[symbol][agrEx][counter - 1][3] *
                        cumulativesSeries[symbol][agrEx][counter - 1][3]));
                    cumulativesSeries[symbol][agrEx][counter - 1][8] = Convert.ToSingle(closeSec);
                    float tempCumulative = 0;
                    for (int i = 0; i < closeSecSeriesCurrent.Count; i++)
                    {
                        secondsSeries[symbol][agrEx][counter - 1].Add(new List<double>(2));
                        Utils.AddEmptyValues(secondsSeries[symbol][agrEx][counter - 1][i], 2);
                        tempCumulative += dealsPoints[day][closeSecSeriesCurrent[i]][0];
                        secondsSeries[symbol][agrEx][counter - 1][i][0] = Convert.ToDouble(closeSecSeriesCurrent[i]);
                        secondsSeries[symbol][agrEx][counter - 1][i][1] = tempCumulative;
                    }

                }
            }
        }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>> GetCumulativesDD()
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>> cumulativesSeries;
            cumulativesSeries = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>>();
            secondsSeries = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<List<double>>>>>();
            
            foreach (string symbol in DealsPoints.Keys)
            {
                if (!cumulativesSeries.ContainsKey(symbol)) cumulativesSeries[symbol] = new ConcurrentDictionary<string, List<List<float>>>();
                if (!secondsSeries.ContainsKey(symbol)) secondsSeries[symbol] = new ConcurrentDictionary<string, List<List<List<double>>>>();
                
                foreach (string agrEx in DealsPoints[symbol].Keys)
                {
                    if (!cumulativesSeries[symbol].ContainsKey(agrEx)) cumulativesSeries[symbol][agrEx] = new List<List<float>>();
                    if (!secondsSeries[symbol].ContainsKey(agrEx)) secondsSeries[symbol][agrEx] = new List<List<List<double>>>();

                    CalculateCumulativeSeries(ref cumulativesSeries, DealsPoints[symbol][agrEx], symbol, agrEx);
                }
            }
            cumulativesSeries["global"] = new ConcurrentDictionary<string, List<List<float>>>();
            cumulativesSeries["global"]["global"] = new List<List<float>>();
            secondsSeries["global"] = new ConcurrentDictionary<string, List<List<List<double>>>>();
            secondsSeries["global"]["global"] = new List<List<List<double>>>();
            CalculateCumulativeSeries(ref cumulativesSeries, GlobalDealsPoints, "global", "global");
            return cumulativesSeries;
        }


        private int GetCumulativesDelay(string symbol, string agrEx, int maxDD, int countSeries, ref ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>> cumulativesSeries)
        {
            int countSeriesDelay = 0;
            for (int i = 0; i < countSeries; i++)
            {
                int countElementsInSeries = secondsSeries[symbol][agrEx][i].Count;
                if (secondsSeries[symbol][agrEx][i][countElementsInSeries - 1][1] > maxDD)
                {
                    continue;
                }
                else
                {
                    countSeriesDelay++;
                    for (int j = 0; j < countElementsInSeries; j++)
                    {

                        if (maxDD <= secondsSeries[symbol][agrEx][i][j][1])
                        {
                            cumulativesSeries[symbol][agrEx][i][7] = Convert.ToSingle(secondsSeries[symbol][agrEx][i][j][0]);
                        }
                    }
                }
                cumulativesSeries[symbol][agrEx][i][9] = cumulativesSeries[symbol][agrEx][i][8] -
                    cumulativesSeries[symbol][agrEx][i][7];
            }

            return countSeriesDelay;
        }


        private ConcurrentDictionary<string, ConcurrentDictionary<string, List<int>>> CalculateDDandDelay()
        {
            ConcurrentDictionary<string, ConcurrentDictionary<string, List<List<float>>>> cumulativesSeries = GetCumulativesDD();

            ConcurrentDictionary<string, ConcurrentDictionary<string, List<int>>> DDandDelays;
            DDandDelays = new ConcurrentDictionary<string, ConcurrentDictionary<string, List<int>>>();
            float SigmaDDdelay = parameters.SigmaCountForDDdelay;
            int globalSeriesDD = 0;
            int globalSeriesDelay = 0;
            foreach (string symbol in cumulativesSeries.Keys)
            {
                if (!DDandDelays.ContainsKey(symbol)) DDandDelays[symbol] = new ConcurrentDictionary<string, List<int>>();
                foreach (string agrEx in cumulativesSeries[symbol].Keys)
                {
                    if (!DDandDelays[symbol].ContainsKey(agrEx)) DDandDelays[symbol][agrEx] = new List<int>(2);
                    Utils.AddEmptyValues(DDandDelays[symbol][agrEx], 2);
                    float sumCurrentMaxDD = 0;
                    float sumSqrCurrentMaxDD = 0;
                    float sumCurrentDelay = 0;
                    float sumSqrCurrentDelay = 0;
                    int countSeries = cumulativesSeries[symbol][agrEx].Count;
                    globalSeriesDD += countSeries;
                    for (int index = 0; index < countSeries; index++)
                    {
                        sumCurrentMaxDD += cumulativesSeries[symbol][agrEx][index][6];
                        sumSqrCurrentMaxDD += cumulativesSeries[symbol][agrEx][index][6] * cumulativesSeries[symbol][agrEx][index][6];
                    }
                    sumCurrentMaxDD /= countSeries;
                    sumSqrCurrentMaxDD /= countSeries;

                    try
                    {
                        if (!(symbol == "global" && agrEx == "global"))
                        {
                            DDandDelays[symbol][agrEx][0] = Convert.ToInt32(Math.Round(sumCurrentMaxDD - Convert.ToSingle(Math.Sqrt(sumSqrCurrentMaxDD - sumCurrentMaxDD * sumCurrentMaxDD))));
                        }
                        else
                        {
                            DDandDelays[symbol][agrEx][0] = Convert.ToInt32(Math.Round(sumCurrentMaxDD - 3*Convert.ToSingle(Math.Sqrt(sumSqrCurrentMaxDD - sumCurrentMaxDD * sumCurrentMaxDD))));
                        }

                        if (DDandDelays[symbol][agrEx][0] < minDD)
                        {
                            minDD = DDandDelays[symbol][agrEx][0];
                        }
                    }
                    catch
                    {
                        DDandDelays[symbol][agrEx][0] = 0;
                    }

                    int countSeriesDelay;
                    countSeriesDelay = GetCumulativesDelay(symbol, agrEx, DDandDelays[symbol][agrEx][0], countSeries, ref cumulativesSeries);
                    globalSeriesDelay += countSeriesDelay;
                    for (int index = 0; index < countSeries; index++)
                    {
                        sumCurrentDelay += cumulativesSeries[symbol][agrEx][index][9];
                        sumSqrCurrentDelay += cumulativesSeries[symbol][agrEx][index][9] * cumulativesSeries[symbol][agrEx][index][9];              
                    }
                    sumCurrentDelay /= countSeriesDelay;
                    sumSqrCurrentDelay /= countSeriesDelay;
                    try
                    {
                        DDandDelays[symbol][agrEx][1] = Convert.ToInt32(Math.Ceiling(sumCurrentDelay + SigmaDDdelay*Convert.ToSingle(Math.Sqrt(sumSqrCurrentDelay - sumCurrentDelay * sumCurrentDelay))));
                        if (DDandDelays[symbol][agrEx][1] == 0) DDandDelays[symbol][agrEx][1] = 1;
                        if (DDandDelays[symbol][agrEx][0] == minDD)
                        {
                            minDDdelay = DDandDelays[symbol][agrEx][1];
                        }
                    }
                    catch
                    {
                        DDandDelays[symbol][agrEx][1] = 0;
                    }
                }
            }
            return DDandDelays;
        }

        public bool GetXMLmaxDD()
        {
            string dir, settingsPath;
            float volume = GetAverageTradeVolume();
            ConcurrentDictionary<string, ConcurrentDictionary<string, List<int>>> DDandDelays = CalculateDDandDelay();
            dir = Environment.CurrentDirectory;
            settingsPath = currentSettings.Substring(0, currentSettings.IndexOf(".xml")) + "_maxDD.xml";
            FileStream fs = File.Create(settingsPath);
            fs.Close();
            StreamReader sr;
            sr = new StreamReader(currentSettings);
            List<string> source = new List<string>();
            source.AddRange(sr.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
            sr.Close();

            List<string> xmlTxt = new List<string>();

            bool isAggr = true;
            bool isDDtag = false;
            string symbol = "", agrEx = "";
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i].IndexOf("<provider>") != -1)
                {
                    string firstPart = source[i - 1].Substring(0, source[i - 1].IndexOf("<"));
                    xmlTxt.Add(firstPart + "<dd_max>" + Convert.ToString(DDandDelays["global"]["global"][0]) + "</dd_max>");
                    xmlTxt.Add(firstPart + "<dd_delay>" + Convert.ToString(DDandDelays["global"]["global"][1]) + "</dd_delay>");
                    xmlTxt.Add(firstPart + "<dd_max_dol>" + Convert.ToString(Math.Round(DDandDelays["global"]["global"][0] * volume * 0.00001)) + "</dd_max_dol>");
                }                
                
                if (source[i].IndexOf("<feedattributes>") != -1) isDDtag = false;
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
                    agrEx = agrEx.Remove(agrEx.Length - 1);
                }
                if (source[i].IndexOf("<dd_max>") != -1 && isAggr)
                {
                    isDDtag = true;
                    string firstPart = source[i].Substring(0, source[i].IndexOf("<dd_max>") + "<dd_max>".Length);
                    string secondPart = source[i].Substring(source[i].IndexOf("</dd_max>"));
                    if (DDandDelays.ContainsKey(symbol) && DDandDelays[symbol].ContainsKey(agrEx) &&
                        DDandDelays[symbol][agrEx][0] != 0 && DDandDelays[symbol][agrEx][1] > parameters.MinDelayForMaxDD)
                    {                        
                        source[i] = firstPart + Convert.ToString(DDandDelays[symbol][agrEx][0]) + secondPart;
                    }
                    else
                    {
                        source[i] = firstPart + Convert.ToString(minDD) + secondPart;
                    }
                }
                if (source[i].IndexOf("<dd_delay>") != -1 && isAggr)
                {
                    string firstPart = source[i].Substring(0, source[i].IndexOf("<dd_delay>") + "<dd_delay>".Length);
                    string secondPart = source[i].Substring(source[i].IndexOf("</dd_delay>"));
                    if (DDandDelays.ContainsKey(symbol) && DDandDelays[symbol].ContainsKey(agrEx) &&
                        DDandDelays[symbol][agrEx][1] != 0 && DDandDelays[symbol][agrEx][1] > parameters.MinDelayForMaxDD) 
                    {
                        source[i] = firstPart + Convert.ToString(DDandDelays[symbol][agrEx][1]) + secondPart;
                    }
                    else
                    {
                        source[i] = firstPart + Convert.ToString(minDDdelay) + secondPart;
                    }
                }
                if (source[i].IndexOf("<dd_attempts>") != -1 && isAggr)
                {
                    string firstPart = source[i].Substring(0, source[i].IndexOf("<dd_attempts>") + "<dd_attempts>".Length);
                    string secondPart = source[i].Substring(source[i].IndexOf("</dd_attempts>"));
                    source[i] = firstPart + Convert.ToString(parameters.DDattempts) + secondPart;
                }
                if (source[i].IndexOf("</feedattributes>") != -1 && !isDDtag)
                {
                    if (DDandDelays.ContainsKey(symbol) && DDandDelays[symbol].ContainsKey(agrEx) &&
                    DDandDelays[symbol][agrEx][0] != 0 && DDandDelays[symbol][agrEx][1] > parameters.MinDelayForMaxDD)
                    {
                        string firstPart = source[i - 1].Substring(0, source[i - 1].IndexOf("<"));
                        xmlTxt.Add(firstPart + "<dd_max>" + Convert.ToString(DDandDelays[symbol][agrEx][0]) + "</dd_max>");
                        xmlTxt.Add(firstPart + "<dd_delay>" + Convert.ToString(DDandDelays[symbol][agrEx][1]) + "</dd_delay>");
                        xmlTxt.Add(firstPart + "<dd_attempts>" + Convert.ToString(parameters.DDattempts) + "</dd_attempts>");
                    }               
                    else
                    {
                        string firstPart = source[i - 1].Substring(0, source[i - 1].IndexOf("<"));
                        xmlTxt.Add(firstPart + "<dd_max>" + Convert.ToString(minDD) + "</dd_max>");
                        xmlTxt.Add(firstPart + "<dd_delay>" + Convert.ToString(minDDdelay) + "</dd_delay>");
                        xmlTxt.Add(firstPart + "<dd_attempts>" + Convert.ToString(parameters.DDattempts) + "</dd_attempts>");
                    }
                }


                xmlTxt.Add(source[i]);
            }
            xmlTxt.SaveToFile(settingsPath);
            return true;
        }
    
    

    }
}
