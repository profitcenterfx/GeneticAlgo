using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace GeneticAlgo
{
    public class GeneticAlgo
    {
        private int[] ind = new int[0];

        public float[] Steps { get; private set; }

        public int[] CountDecimal { get; private set; }

        public MainForm MainForm { get; private set; }

        private int taskCount = 0;

        private int tasksRest = 0;

        public int PercentOfCompletedTasks { get; private set; }

        char char9 = Convert.ToChar(9);

        public string logFilename;

        public string currentSettings;

        private int openSlipIndex = -1;

        public static Settings Settings { get; set; }

        public Parameters Parameters { get; private set; }

        public SymbolExchangeFilteringSettings FilteringSettings { get; private set; }

        public Filters OptimizationSettings { get; private set; }

        public GeneticAlgo(Parameters parameters, MainForm form)
        {
            Parameters = parameters;
            FilteringSettings = new SymbolExchangeFilteringSettings("SymbolExchangeFilter.xml");
            OptimizationSettings = Filters.LoadOptimizationSettings("OptimizationSettings.xml");
            MainForm = form;
            PercentOfCompletedTasks = 0;
        }

        public bool SelectLogFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = Environment.CurrentDirectory;
            openDialog.Filter = "Excel statistic file (.csv)|*.csv";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                logFilename = openDialog.FileName;
                return true;
            }
            return false;
        }

        public Dictionary<string, List<double>> GetCurrentSettings(string symbol, string feed)
        {
            Dictionary<string, List<double>> dic = new Dictionary<string, List<double>>();
            dic.Add("indent", new List<double> {Settings.aggregator[symbol][feed].indent,double.NaN});
            dic.Add("advance", new List<double> {Settings.aggregator[symbol][feed].advance,Settings.aggregator[symbol][feed].maxadvance});
            dic.Add("agrspread", new List<double> {Settings.aggregator[symbol][feed].minagrspread,Settings.aggregator[symbol][feed].maxagrspread});
            dic.Add("agrexspread", new List<double> {Settings.aggregator[symbol][feed].minagrexspread,Settings.aggregator[symbol][feed].maxagrexspread});
            dic.Add("agrturn", new List<double> {Settings.aggregator[symbol][feed].minagrturn,Settings.aggregator[symbol][feed].maxagrturn});
            dic.Add("feedturn", new List<double> {Settings.aggregator[symbol][feed].minfeedturn,Settings.aggregator[symbol][feed].maxfeedturn});
            dic.Add("div", new List<double> {Settings.aggregator[symbol][feed].mindiv,Settings.aggregator[symbol][feed].maxdiv});
            dic.Add("agrstdev", new List<double> {Settings.aggregator[symbol][feed].minagrstdev,Settings.aggregator[symbol][feed].maxagrstdev});
            dic.Add("feedstdev", new List<double> {Settings.aggregator[symbol][feed].minfeedstdev,Settings.aggregator[symbol][feed].maxfeedstdev});   
            dic.Add("agrsize", new List<double> {Settings.aggregator[symbol][feed].minagrsize,Settings.aggregator[symbol][feed].maxagrsize});
            dic.Add("feedsize", new List<double> { Settings.aggregator[symbol][feed].minfeedsize, Settings.aggregator[symbol][feed].maxfeedsize});
            dic.Add("agravespread", new List<double> { Settings.aggregator[symbol][feed].minagravespread, Settings.aggregator[symbol][feed].maxagravespread });
            dic.Add("feedavespread", new List<double> { Settings.aggregator[symbol][feed].minfeedavespread, Settings.aggregator[symbol][feed].maxfeedavespread });
            dic.Add("opentrend", new List<double> { Settings.aggregator[symbol][feed].minopentrend, Settings.aggregator[symbol][feed].maxopentrend });
            dic.Add("powerfeed", new List<double> { Settings.aggregator[symbol][feed].minpowerfeed, Settings.aggregator[symbol][feed].maxpowerfeed });
            dic.Add("poweragr", new List<double> { Settings.aggregator[symbol][feed].minpoweragr, Settings.aggregator[symbol][feed].maxpoweragr });
            dic.Add("powerbbo", new List<double> { Settings.aggregator[symbol][feed].minpowerbbo, Settings.aggregator[symbol][feed].maxpowerbbo });
            dic.Add("powersum", new List<double> { Settings.aggregator[symbol][feed].minpowersum, Settings.aggregator[symbol][feed].maxpowersum });
            dic.Add("accelfeed", new List<double> { Settings.aggregator[symbol][feed].minaccelfeed, Settings.aggregator[symbol][feed].maxaccelfeed });
            dic.Add("accelagr", new List<double> { Settings.aggregator[symbol][feed].minaccelagr, Settings.aggregator[symbol][feed].maxaccelagr });
            dic.Add("accelbbo", new List<double> { Settings.aggregator[symbol][feed].minaccelbbo, Settings.aggregator[symbol][feed].maxaccelbbo });
            dic.Add("accelsum", new List<double> { Settings.aggregator[symbol][feed].minaccelsum, Settings.aggregator[symbol][feed].maxaccelsum });
            dic.Add("massfeed", new List<double> { Settings.aggregator[symbol][feed].minmassfeed, Settings.aggregator[symbol][feed].maxmassfeed });
            dic.Add("massagr", new List<double> { Settings.aggregator[symbol][feed].minmassagr, Settings.aggregator[symbol][feed].maxmassagr });
            dic.Add("massbbo", new List<double> { Settings.aggregator[symbol][feed].minmassbbo, Settings.aggregator[symbol][feed].maxmassbbo });
            dic.Add("masssum", new List<double> { Settings.aggregator[symbol][feed].minmasssum, Settings.aggregator[symbol][feed].maxmasssum });
            dic.Add("anglefeed", new List<double> { Settings.aggregator[symbol][feed].minanglefeed, Settings.aggregator[symbol][feed].maxanglefeed });
            dic.Add("angleagr", new List<double> { Settings.aggregator[symbol][feed].minangleagr, Settings.aggregator[symbol][feed].maxangleagr });
            dic.Add("anglebbo", new List<double> { Settings.aggregator[symbol][feed].minanglebbo, Settings.aggregator[symbol][feed].maxanglebbo });
            dic.Add("anglesum", new List<double> { Settings.aggregator[symbol][feed].minanglesum, Settings.aggregator[symbol][feed].maxanglesum });
            dic.Add("sigpriceage", new List<double> { Settings.aggregator[symbol][feed].minsigpriceage, Settings.aggregator[symbol][feed].maxsigpriceage });
            dic.Add("feedpriceage", new List<double> { Settings.aggregator[symbol][feed].minfeedpriceage, Settings.aggregator[symbol][feed].maxfeedpriceage });
            return dic;
        }

        public bool SelectSettingsFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog = new OpenFileDialog();
            FileInfo fInfo = new FileInfo(logFilename);
            openDialog.InitialDirectory = fInfo.Directory.FullName;
            openDialog.Filter = "Current settings file (.xml)|*.xml";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                currentSettings = openDialog.FileName;
                XmlSerializer xml = new XmlSerializer(typeof(Settings));
                try
                {
                    StreamReader file = new StreamReader(currentSettings);
                    Settings = (Settings)xml.Deserialize(file);                      

                    if (Settings.symbols == null)
                    {
                        MessageBox.Show("Bad config file : not all fields filled");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("An error occured when reading settings file: " + e.Message);
                    return false;
                }
                return true;
            }
            return false;
        }

        public void XMLcopyDefTrivial(List<string> symbols, List<string> exchanges, string currentSettings)
        {
            string dir, q;
            List<string> filters;
            int i, j, k, SymInd, AEInd;
            List<List<string>> filterVar;
            List<List<List<string>>> copySet;

            dir = Environment.CurrentDirectory;
            string timeNow = DateTime.Now.ToShortDateString().Replace('/', '.');
            var settingsPath = currentSettings.Substring(0, currentSettings.IndexOf(".xml")) + timeNow + ".xml";

            using (var stream = File.Create(settingsPath))
            { }

            var source = new List<string>();
            var xmlTxt = new StringBuilder();

            using (var sr = new StreamReader(currentSettings))
                source.AddRange(sr.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            copySet = new List<List<List<string>>>();
            for (i = 0; i < exchanges.Count; i++)
            {
                List<List<string>> tmp = new List<List<string>>();
                for (j = 0; j < 5; j++)
                {
                    tmp.Add(new List<string>(new string[symbols.Count]));
                }
                copySet.Add(tmp);
            }

            i = 0;
            while (source[i].IndexOf("<aggregator>") == -1)
            {
                xmlTxt.AppendLine(source[i]);
                i++;
            }

            i++;

            SymInd = -1;
            AEInd = -1;

            while (i < source.Count)
            {
                if (source[i].IndexOf("<aggregator>") != -1) { break; }

                if (source[i].IndexOf("<symbol name=\"") != -1)
                {
                    for (j = 0; j < symbols.Count; j++)
                    {
                        if (symbols[j] == source[i].Substring(source[i].IndexOf("<symbol name=\"") + "<symbol name=\"".Length,
                            source[i].IndexOf("\">") - (source[i].IndexOf("<symbol name=\"") + "<symbol name=\"".Length)))
                        {
                            SymInd = j;
                            break;
                        }
                    }
                }

                if (source[i].IndexOf("<exchange name=\"") != -1)
                {
                    AEInd = -1;
                    for (j = 0; j < exchanges.Count; j++)
                    {
                        if (exchanges[j] == source[i].Substring(source[i].IndexOf("<exchange name=\"") + "<exchange name=\"".Length,
                            source[i].IndexOf("\">") - (source[i].IndexOf("<exchange name=\"") + "<exchange name=\"".Length)))
                        {
                            AEInd = j;
                            break;
                        }
                    }
                }

                if (SymInd != -1)
                {

                    if (source[i].IndexOf("<max_positions>") != -1 && AEInd != -1)
                        copySet[AEInd][0][SymInd] =
                            source[i].Substring(source[i].IndexOf("<max_positions>") + "<max_positions>".Length,
                                source[i].IndexOf("</max_positions>") -
                                (source[i].IndexOf("<max_positions>") + "<max_positions>".Length));

                    if (source[i].IndexOf("<reverse>") != -1 && AEInd != -1)
                        copySet[AEInd][1][SymInd] =
                            source[i].Substring(source[i].IndexOf("<reverse>") + "<reverse>".Length,
                                source[i].IndexOf("</reverse>") - (source[i].IndexOf("<reverse>") + "<reverse>".Length));

                    if (source[i].IndexOf("<volume>") != -1 && AEInd != -1)
                        copySet[AEInd][2][SymInd] =
                            source[i].Substring(source[i].IndexOf("<volume>") + "<volume>".Length,
                                source[i].IndexOf("</volume>") - (source[i].IndexOf("<volume>") + "<volume>".Length));

                    if (source[i].IndexOf("<string>") != -1 && AEInd != -1)
                        copySet[AEInd][3][SymInd] =
                            source[i].Substring(source[i].IndexOf("<string>") + "<string>".Length,
                                source[i].IndexOf("</string>") - (source[i].IndexOf("<string>") + "<string>".Length));

                    if (source[i].IndexOf("<treshhold>") != -1 && AEInd != -1)
                        copySet[AEInd][4][SymInd] =
                            source[i].Substring(source[i].IndexOf("<treshhold>") + "<treshhold>".Length,
                                source[i].IndexOf("</treshhold>") - (source[i].IndexOf("<treshhold>") + "<treshhold>".Length));                
                }

                i++;
            }

            SymInd = 0;
            AEInd = 0;

            xmlTxt.AppendLine("  " + "<aggregator>");

            for (var symbolIndex = 0; symbolIndex < symbols.Count; symbolIndex++)
            {
                if (!Settings.aggregator.ContainsKey(symbols[symbolIndex]))
                    continue;

                xmlTxt.AppendLine("    " + "<symbol name=\"" + symbols[symbolIndex] + "\">");
                xmlTxt.AppendLine("      " + "<exchanges>");

                for (var exchangeIndex = 0; exchangeIndex < exchanges.Count; exchangeIndex++)
                {
                    if (!FilteringSettings[symbols[symbolIndex].Replace('/', '_'), exchanges[exchangeIndex]])
                        continue;

                    if (!Settings.aggregator[symbols[symbolIndex]].ContainsKey(exchanges[exchangeIndex]))
                        continue;

                    filters = new List<string>();
                    filters.LoadFromFile(dir + "/Recomends/" + symbols[symbolIndex].Replace('/', '_') + "/" + exchanges[exchangeIndex] + ".txt");
                    if (filters.Count == 0)
                        continue;

                    var limits = new List<List<string>>();
                    for (i = 0; i < OptimizationSettings.DefaultSettings.Count; i++)
                    {
                        string[] arr = 
                        {
                            OptimizationSettings.DefaultSettings[i].Name, OptimizationSettings.DefaultSettings[i].MinValue, OptimizationSettings.DefaultSettings[i].MaxValue 
                        };
                        limits.Add(new List<string>(arr));
                    }

                    if (Parameters.CurrentSettingsFile)
                    {
                        var dic = GetCurrentSettings(symbols[symbolIndex], exchanges[exchangeIndex]);
                        
                        for (int index = 0; index < limits.Count; index++)
                        {
                            if (limits[index][0] != "indent")
                            {
                                if (Convert.ToDouble(limits[index][1]) < dic[limits[index][0]][0])
                                    limits[index][1] = Convert.ToString(dic[limits[index][0]][0]);
                                if (Convert.ToDouble(limits[index][2]) > dic[limits[index][0]][1])
                                    limits[index][2] = Convert.ToString(dic[limits[index][0]][1]);
                            }
                            else if (Convert.ToDouble(limits[index][1]) > dic[limits[index][0]][0])
                                limits[index][1] = Convert.ToString(dic[limits[index][0]][0]);

                            dic.Remove(limits[index][0]);
                        }
                        
                        foreach (KeyValuePair<string, List<double>> kvp in dic)
                        {
                            if (!kvp.Value[0].Equals(double.NaN) || !kvp.Value[1].Equals(double.NaN))
                            {
                                limits.Add(new List<string>(new string[3]));
                                limits[limits.Count - 1][0] = kvp.Key;
                                if (!double.IsNaN(kvp.Value[0]))
                                    limits[limits.Count - 1][1] = Convert.ToString(kvp.Value[0]);
                                else
                                    limits[limits.Count - 1][1] = "NaN";
                                if (!double.IsNaN(kvp.Value[1]))
                                    limits[limits.Count - 1][2] = Convert.ToString(kvp.Value[1]);
                                else
                                    limits[limits.Count - 1][2] = "NaN";
                            }
                        }
                    }
                 
                    xmlTxt.AppendLine("        " + "<exchange name=\"" + exchanges[exchangeIndex] + "\">");
                    xmlTxt.AppendLine("          " + "<feedattributes>");
                    xmlTxt.AppendLine("            " + "<max_positions>" + copySet[exchangeIndex][0][symbolIndex] + "</max_positions>");
                    xmlTxt.AppendLine("            " + "<reverse>" + copySet[exchangeIndex][1][symbolIndex] + "</reverse>");
                    xmlTxt.AppendLine("            " + "<volume>" + copySet[exchangeIndex][2][symbolIndex] + "</volume>");
                    xmlTxt.AppendLine("            " + "<treshhold>" + copySet[exchangeIndex][4][symbolIndex] + "</treshhold>");

                    if (filters.Count == 0)
                    {
                        for (i = 0; i < limits.Count; i++)
                        {
                            if (limits[i][0] == "indent")
                            {
                                if (limits[i][1] != "NaN") 
                                xmlTxt.AppendLine("            " + "<" + limits[i][0] + ">" + limits[i][1] + "</" + limits[i][0] + ">");
                            }
                            else
                            {
                                if (limits[i][0] == "advance")
                                {
                                    if (limits[i][1] != "NaN") 
                                    xmlTxt.AppendLine("            " + "<" + limits[i][0] + ">" + limits[i][1] + "</" + limits[i][0] + ">");
                                    if (limits[i][2] != "NaN") 
                                    xmlTxt.AppendLine("            " + "<max" + limits[i][0] + ">" + limits[i][2] + "</max" + limits[i][0] + ">");
                                }
                                else
                                {
                                    if (limits[i][1] != "NaN")
                                    xmlTxt.AppendLine("            " + "<min" + limits[i][0] + ">" + limits[i][1] + "</min" + limits[i][0] + ">");
                                    if (limits[i][2] != "NaN")
                                    xmlTxt.AppendLine("            " + "<max" + limits[i][0] + ">" + limits[i][2] + "</max" + limits[i][0] + ">");
                                }
                            }
                        }
                    }
                    else
                    {
                        filterVar = new List<List<string>>();
                        filterVar.FillWithEmptyValues(filters.Count - 67, 3);

                        for (i = 67; i < filters.Count; i++)
                        {
                            q = "";
                            k = 0;
                            for (j = 0; j < filters[i].Length; j++)
                            {
                                if (filters[i][j] == 9 || filters[i][j] == 13)
                                {
                                    filterVar[i - 67][k] = q;
                                    k++;
                                    q = "";
                                }
                                else if (filters[i][j] == ',')
                                {
                                    q = q + ".";
                                }
                                else
                                {
                                    q = q + filters[i][j];
                                }
                            }

                            filterVar[i - 67][k] = q;
                        }

                        for (int varIndex = 0; varIndex < filterVar.Count; varIndex++)
                        {
                            double val;
                            if (double.TryParse(filterVar[varIndex][0], out val))
                                filterVar[varIndex][0] = val.ToString();
                        }

                        for (i = 0; i < filterVar.Count; i++)
                        {
                            int filterIndex=-1;
                            for (j = 0; j < limits.Count; j++)
                            {
                                if (limits[j][0] == filterVar[i][2]) filterIndex = j;
                            }
                            if (filterVar[i][0] != "xxx")
                            {
                                if (filterIndex!=-1 && limits[filterIndex][1] != "NaN")
                                {
                                    if (filterVar[i][2] == "indent" && Convert.ToDouble(filterVar[i][0]) > Convert.ToDouble(limits[filterIndex][1]))
                                        filterVar[i][0] = limits[filterIndex][1];
                                }
                                if ((filterVar[i][2] == "indent") || (filterVar[i][2] == "advance"))
                                {
                                    xmlTxt.AppendLine("            " + "<" + filterVar[i][2] + ">" + filterVar[i][0] + "</" + filterVar[i][2] + ">");
                                }
                                else
                                {
                                    xmlTxt.AppendLine("            " + "<min" + filterVar[i][2] + ">" + filterVar[i][0] + "</min" + filterVar[i][2] + ">");
                                }

                            }
                            else
                            {
                                if ((filterVar[i][2] == "indent") || (filterVar[i][2] == "advance"))
                                {
                                    if (filterIndex != -1 && limits[filterIndex][1] != "NaN" && limits[filterIndex][1] != null)
                                        xmlTxt.AppendLine("            " + "<" + filterVar[i][2] + ">" + limits[filterIndex][1] + "</" + filterVar[i][2] + ">");
                                }
                                else
                                {
                                    if (filterIndex != -1 && limits[filterIndex][1] != "NaN" && limits[filterIndex][1] != null)
                                        xmlTxt.AppendLine("            " + "<min" + limits[filterIndex][0] + ">" + limits[filterIndex][1] + "</min" + limits[filterIndex][0] + ">");
                                }
                            }
                       
                            if (filterVar[i][1] != "xxx")
                            {
                                if (filterIndex != -1 && limits[filterIndex][2] != "NaN" && limits[filterIndex][2] != null)
                                {
                                    if (Convert.ToDouble(filterVar[i][1]) > Convert.ToDouble(limits[filterIndex][2]))
                                        filterVar[i][1] = limits[filterIndex][2];
                                }
                                xmlTxt.AppendLine("            " + "<max" + filterVar[i][2] + ">" + filterVar[i][1] + "</max" + filterVar[i][2] + ">");
                            }
                            else
                            {
                                if ((filterIndex != -1) && (limits[filterIndex][0] != "indent") && (limits[filterIndex][2] != "NaN") && (limits[filterIndex][2] != null))
                                    xmlTxt.AppendLine("            " + "<max" + limits[filterIndex][0] + ">" + limits[filterIndex][2] + "</max" + limits[filterIndex][0] + ">");
                            }
                        }
                    }
                    xmlTxt.AppendLine("          " + "</feedattributes>");
                    xmlTxt.AppendLine("        " + "</exchange>");
                    limits.Clear();
                }

                xmlTxt.AppendLine("      " + "</exchanges>");
                xmlTxt.AppendLine("    " + "</symbol>");
            }


            xmlTxt.AppendLine("  </aggregator>");

            for (i = 0; i < source.Count; i++)
            {
                if (source[i].IndexOf("<filter />") != -1)
                    break;
            }

            while (i < source.Count)
            {
                xmlTxt.AppendLine(source[i]);
                i++;
            }

            using (var writer = new StreamWriter(settingsPath))
                writer.Write(xmlTxt.ToString());
        }

        public int[][] initialPositionOpt(int k)
        {
            int i, j, m, n, counter;
            int[,] basic;
            int[][] result;

            // разобраться - возможно параметр таки только целый бывает
            float l = Parameters.VariableNumberOfSlots;

            basic = new int[(int)GeneMath.Round((float)(l + (l - 1) * l / 2)), 2];

            counter = 0;

            for (i = 0; i < l; i++)
            {
                for (j = 0; j < l - i; j++)
                {
                    basic[counter, 0] = i;
                    basic[counter, 1] = j;
                    counter++;
                }
            }

            int size = 0;
            for (i = 0; i < k; i++)
            {
                counter = 0;
                int jMax = (int)GeneMath.Round(GeneMath.Exp(GeneMath.Log(l + (l - 1) * l / 2) * i));
                for (j = 1; j <= jMax; j++)
                {
                    int mMax = (int)GeneMath.Round((float)(l + (l - 1) * l / 2)) - 1;
                    for (m = 0; m <= mMax; m++)
                    {
                        int nMax = (int)(GeneMath.Round(GeneMath.Exp(GeneMath.Log(l + (l - 1) * l / 2) * (k - 1 - i))) - 1);
                        for (n = 0; n <= nMax; n++)
                        {
                            counter++;
                        }
                    }
                }
                size = size > counter ? size : counter;
            }
            result = new int[size][];

            for (i = 0; i < k; i++)
            {
                int jMax = (int)GeneMath.Round(GeneMath.Exp(GeneMath.Log(l + (l - 1) * l / 2) * i));
                counter = 0;
                for (j = 1; j <= jMax; j++)
                {
                    int mMax = (int)GeneMath.Round((float)(l + (l - 1) * l / 2)) - 1;
                    for (m = 0; m <= mMax; m++)
                    {
                        int basicM0 = basic[m, 0];
                        int basicM1 = basic[m, 1];
                        int nMax = ((int)GeneMath.Round(GeneMath.Exp(GeneMath.Log(l + (l - 1) * l / 2) * (k - 1 - i)))) - 1;
                        for (n = 0; n <= nMax; n++)
                        {
                            if (i == 0)
                            {
                                result[counter] = new int[2];
                                result[counter][0] = basicM0;
                                result[counter][1] = basicM1;
                            }
                            else
                            {
                                int[] tmp = result[counter];
                                result[counter] = new int[tmp.Length + 2];
                                tmp.CopyTo(result[counter], 0);
                                result[counter][tmp.Length] = basicM0;
                                result[counter][tmp.Length + 1] = basicM1;
                            }
                            counter++;
                        }
                    }
                }
            }

            return result;
        }

        public int CalcInflectiveNumber()
        {
            return OptimizationSettings.filters.Count(filter => filter.FirstPopulation) + 2;
        }

        public void CopyStepsAndCountDecimalDigits()
        {
            var filtersCount = OptimizationSettings.filters.Count;
            Steps = new float[filtersCount];
            CountDecimal = new int[filtersCount];
            for (int i = 0; i < filtersCount; i++)
            {
                Steps[i] = (float)OptimizationSettings.filters[i].Step;
                CountDecimal[i] = Utils.GetDecimalDigitsCount(OptimizationSettings.filters[i].Step);
            }
        }
        
        class SymbolExchangeTradeCount
        {
            public bool IsFatFeed;

            public string Symbol;

            public string Symbol_;

            public string Exchange;

            public int Count;
        }

        struct ThreadType
        {
            public Thread Thread;

            public SymbolExchangeTradeCount Info;
        }

        int tradesCompleted = 0;

        private int[][] initialPos;

        private int inflectiveNumber;

        private object starterLockObject = new object();

        private TradeInfoCollection trades;

        private void OptimizerStarter(object feedObject)
        {
            var feed = (SymbolExchangeTradeCount)feedObject;

            var opt = new OptimizerTrivialBitDriven(Parameters, feed.Symbol_, feed.Exchange, initialPos, OptimizationSettings, inflectiveNumber);

            lock (starterLockObject)
            {
                opt.Steps = Steps;
                opt.CountDecimal = CountDecimal;
                List<TradeInfo> Trades = new List<TradeInfo>();
                List<TradeInfo> TradesCheck = new List<TradeInfo>();
                List<TradeInfo> TradesPrediction = new List<TradeInfo>();
                trades.GetTradesListFiltered(feed.Symbol, feed.Exchange, out Trades, out TradesCheck, out TradesPrediction);
                opt.SetTrades(Trades.ToArray());
                opt.SetTradesCheck(TradesCheck.ToArray());
                opt.SetTradesPrediction(TradesPrediction.ToArray());
                var tradesFull = trades.GetTradesList(feed.Symbol, feed.Exchange).ToArray();
                opt.SetTradesFull(tradesFull);
                opt.ParametersRoundDigits = trades.ParametersRoundDigits;
                // if trade count is smaller then set - skip current task
                if (tradesFull.Length < Parameters.MinNumberOfTrades)
                {
                    tradesCompleted += tradesFull.Length;
                    return;
                }

                opt.minOpSl = Parameters.MinAdvance;
                opt.minOpSlGlobal = Parameters.MinAdvanceGlobal;
            }

            int result = 0;

            result = opt.OptimizeSymbol();

            lock (starterLockObject)
            {
                tradesCompleted += result;
            }
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }

        public void StartOptimization()
        {
            CopyStepsAndCountDecimalDigits();

            var dir = Environment.CurrentDirectory;

            // load/parse/filter trade data
            trades = new TradeInfoCollection(logFilename, Parameters, Steps, OptimizationSettings.ParametersIndexes, OptimizationSettings);

            var symbols = new List<string>(trades.Trades.Keys);
            var exchanges = trades.GetArgExchanges();

            Directory.CreateDirectory(dir + "/Recomends");
            Directory.CreateDirectory(dir + "/Recomends/log");

            // число изменяемых параметров
            inflectiveNumber = CalcInflectiveNumber();
            // Calculate initial position
            initialPos = initialPositionOpt(inflectiveNumber);

            Queue<KeyValuePair<SymbolExchangeTradeCount, Thread>> taskQueueFat = new Queue<KeyValuePair<SymbolExchangeTradeCount, Thread>>();
            Queue<KeyValuePair<SymbolExchangeTradeCount, Thread>> taskQueueFit = new Queue<KeyValuePair<SymbolExchangeTradeCount, Thread>>();
            Dictionary<int, ThreadType> threadPool = new Dictionary<int, ThreadType>();

            PercentOfCompletedTasks = 0;

            List<SymbolExchangeTradeCount> symExCounts = new List<SymbolExchangeTradeCount>();

            foreach (KeyValuePair<string, ConcurrentDictionary<string, List<TradeInfo>>> sympair in trades.Trades)
            {
                foreach (KeyValuePair<string, List<TradeInfo>> expair in sympair.Value)
                    symExCounts.Add(new SymbolExchangeTradeCount()
                    {
                        Symbol = sympair.Key,
                        Symbol_ = sympair.Key.Replace('/', '_'),
                        Exchange = expair.Key,
                        Count = expair.Value.Count
                    });
            }

            var orderedFeedsForSplit = symExCounts.OrderByDescending(feed => feed.Count).ToList();
            int tradesTotal = orderedFeedsForSplit.Sum(feed => feed.Count);

            if (Parameters.UseOpenCLDevice)
            {
                var pointer = 0;
                var currentPercent = 0.0;

                for(int i = 0; i < orderedFeedsForSplit.Count; i++)
                {
                    currentPercent += (orderedFeedsForSplit[i].Count / (float)tradesTotal) * 100;
                    if(currentPercent >= 50.0F)
                    {
                        pointer = i;
                        break;
                    }
                }

                Parameters.MinTradesForUseOpenCL = orderedFeedsForSplit[pointer].Count;
            }

            var orderedFeedsFat = new List<SymbolExchangeTradeCount>();
            var orderedFeedsFit = new List<SymbolExchangeTradeCount>();

            if (Parameters.UseOpenCLDevice)
            {
                var fat = orderedFeedsForSplit.Where(feed => feed.Count >= Parameters.MinTradesForUseOpenCL).ToList();
                var fit = orderedFeedsForSplit.Where(feed => feed.Count < Parameters.MinTradesForUseOpenCL).OrderBy(feed => feed.Count).ToList();
                fat.ForEach(feed => { orderedFeedsFat.Add(feed); feed.IsFatFeed = true; });
                fit.ForEach(feed => { orderedFeedsFit.Add(feed); feed.IsFatFeed = false; });
            }
            else
            {
                orderedFeedsForSplit.ForEach(feed => orderedFeedsFat.Add(feed));
            }

            tradesCompleted = 0;

            foreach (var feed in orderedFeedsFat)
            {
                string symbol_ = feed.Symbol.Replace('/', '_');
                // check for skipping symbol/exchange pair
                if (!FilteringSettings[feed.Symbol, feed.Exchange])
                {
                    continue;
                }

                Thread t = new Thread(new ParameterizedThreadStart(OptimizerStarter));
                t.Priority = ThreadPriority.Highest;
                taskQueueFat.Enqueue(new KeyValuePair<SymbolExchangeTradeCount, Thread>(feed, t));
                taskCount++;
                tasksRest++;
            }

            foreach (var feed in orderedFeedsFit)
            {
                string symbol_ = feed.Symbol.Replace('/', '_');
                // check for skipping symbol/exchange pair
                if (!FilteringSettings[feed.Symbol, feed.Exchange])
                {
                    continue;
                }

                Thread t = new Thread(new ParameterizedThreadStart(OptimizerStarter));
                t.Priority = ThreadPriority.Highest;
                taskQueueFit.Enqueue(new KeyValuePair<SymbolExchangeTradeCount, Thread>(feed, t));
                taskCount++;
                tasksRest++;
            }

            try
            {
                if (Parameters.UseOpenCLDevice)
                {
                    OpenCLService.Device = Parameters.OpenCLDevice;
                    OpenCLService.Platform = OpenCLService.Device.Platform;
                    OpenCLService.IsRunning = false;
                    OpenCLService.InitOpenCLService();
                    OpenCLService.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            var index = 0;
            for (int i = 0; i < (Parameters.UseOpenCLDevice ? Parameters.MaxConcurrency / 2 : Parameters.MaxConcurrency); i++)
            {
                if (taskQueueFat.Count > 0)
                {
                    KeyValuePair<SymbolExchangeTradeCount, Thread> pair = taskQueueFat.Dequeue();
                    threadPool.Add(index, new ThreadType() { Thread = pair.Value, Info = pair.Key });
                    pair.Value.Start(pair.Key);
                    index++;
                }
                if (taskQueueFit.Count > 0)
                {
                    KeyValuePair<SymbolExchangeTradeCount, Thread> pair = taskQueueFit.Dequeue();
                    threadPool.Add(index, new ThreadType() { Thread = pair.Value, Info = pair.Key });
                    pair.Value.Start(pair.Key);
                    index++;
                }
            }

            while (threadPool.Count > 0)
            {
                Thread.Sleep(2000);
                try
                {
                    var fatCount = 0;
                    var fitCount = 0;
                    foreach (var feed in threadPool)
                    {
                        fatCount += feed.Value.Info.IsFatFeed ? 1 : 0;
                        fitCount += feed.Value.Info.IsFatFeed ? 0 : 1;
                    }

                    var addFit = fatCount >= fitCount ? true : false;

                    for (int i = 0; i < Parameters.MaxConcurrency; i++)
                    {
                        if (i >= threadPool.Count)
                            break;
                        int key = threadPool.Keys.ToArray()[i];
                        Thread curThread = threadPool[key].Thread;
                        if (curThread == null)
                            continue;
                        if (curThread.ThreadState == System.Threading.ThreadState.Stopped)
                        {
                            MainForm.progressBar.Invoke(new Action(() =>
                            {
                                PercentOfCompletedTasks = (int)(((double)(tradesCompleted) / (double)tradesTotal) * 100);
                                MainForm.progressBar.Value = PercentOfCompletedTasks;
                                MainForm.EsitmatedTime = new TimeSpan((long)(((double)MainForm.ElapsedTime.Ticks / (double)(tradesCompleted)) * (double)(tradesTotal - tradesCompleted)));
                                MainForm.ProjectedTotalTime = MainForm.EsitmatedTime + MainForm.ElapsedTime;
                                Application.DoEvents();
                            }));

                            Queue<KeyValuePair<SymbolExchangeTradeCount, Thread>> taskQueue;
                            taskQueue = addFit ? taskQueueFit : taskQueueFat;

                            if (taskQueue.Count > 0)
                            {
                                KeyValuePair<SymbolExchangeTradeCount, Thread> pair = taskQueue.Dequeue();
                                threadPool[key] = new ThreadType() { Thread = pair.Value, Info = pair.Key };
                                threadPool[key].Thread.Start(pair.Key);
                            }
                            else if (taskQueueFat.Count == 0 && taskQueueFit.Count == 0)
                            {
                                threadPool.Remove(key);
                                break;
                            }
                            else
                            {
                                taskQueue = addFit ? taskQueueFat : taskQueueFit;
                                KeyValuePair<SymbolExchangeTradeCount, Thread> pair = taskQueue.Dequeue();
                                threadPool[key] = new ThreadType() { Thread = pair.Value, Info = pair.Key };
                                threadPool[key].Thread.Start(pair.Key);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if (Parameters.UseOpenCLDevice)
                OpenCLService.Stop();

            try
            {
                XMLcopyDefTrivial(symbols, exchanges, currentSettings);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MainForm.Invoke(new Action(() =>
            {
                MainForm.UnlockControls();
                MainForm.timer.Stop();
                MainForm.lbEstimatedTimeValue.Text = "00:00:00";
                MainForm.progressBar.Value = 100;
                MessageBox.Show("Optimization completed!");
            }));
        }
    }
}
