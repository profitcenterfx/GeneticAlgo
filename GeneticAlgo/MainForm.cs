using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.IO;
using System.Xml.Serialization;

namespace GeneticAlgo
{
    public partial class MainForm : Form
    {
        public Parameters Parameters { get; private set; }

        public GeneticAlgo Algo { get; private set; }


        private DateTime startDateTime;

        public TimeSpan ElapsedTime { get; set; }

        public TimeSpan EsitmatedTime { get; set; }

        public TimeSpan ProjectedTotalTime { get; set; }

        public MainForm()
        {
            InitializeComponent();
            Parameters = new Parameters();
            Algo = new GeneticAlgo(Parameters, this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BindParameters();
            UpdateControlsAccesibility();
        }

        private void BindParameters()
        {
            tbPopulation.DataBindings.Add(new Binding("Text", Parameters, "Population"));
            tbSelectionPower1.DataBindings.Add(new Binding("Text", Parameters, "SelectionPower1"));
            tbSelectionPower2.DataBindings.Add(new Binding("Text", Parameters, "SelectionPower2"));
            tbFrequencyOfOccurrenceNullIndividual.DataBindings.Add(new Binding("Text", Parameters, "FrequencyOfOccurrenceNullIndividual"));
            tbCountOfGenerations.DataBindings.Add(new Binding("Text", Parameters, "CountOfGenerations"));
            tbVariableNumberOfSlots.DataBindings.Add(new Binding("Text", Parameters, "VariableNumberOfSlots"));
            tbMinNumberOfTrades.DataBindings.Add(new Binding("Text", Parameters, "MinNumberOfTrades"));
            tbMinNumberOfTradesAfterPreopting.DataBindings.Add(new Binding("Text", Parameters, "MinNumberOfTradesAfterPreopting"));
            tbMutationCoef1.DataBindings.Add(new Binding("Text", Parameters, "MutationCoef1"));
            tbMutationCoef2.DataBindings.Add(new Binding("Text", Parameters, "MutationCoef2"));
            tbMutationCoef3.DataBindings.Add(new Binding("Text", Parameters, "MutationCoef3"));
            tbMutationCoef4.DataBindings.Add(new Binding("Text", Parameters, "MutationCoef4"));
            tbNumberOfTheBestIndividuals.DataBindings.Add(new Binding("Text", Parameters, "NumberOfTheBestIndividuals"));
            tbCountOfGenerationsWithTheBestInd.DataBindings.Add(new Binding("Text", Parameters, "CountOfGenerationsWithTheBestInd"));
            tbCoefOfStochastickSelect.DataBindings.Add(new Binding("Text", Parameters, "CoefOfStochastickSelect"));
            tbPercentArbiTradeForAdvancePre_opting.DataBindings.Add(new Binding("Text", Parameters, "ArbPercentForAdvancePreopting"));
            tbPercentOfOptLog.DataBindings.Add(new Binding("Text", Parameters, "PercentOfOptLog"));
            tbPercentOfCheckLog.DataBindings.Add(new Binding("Text", Parameters, "PercentOfCheckLog"));
            tbDatePriority.DataBindings.Add(new Binding("Text", Parameters, "DatePriority"));
            tbDaysForSetVolumes.DataBindings.Add(new Binding("Text", Parameters, "DaysForSetVolumes"));
            tbMaxDealVolume.DataBindings.Add(new Binding("Text", Parameters, "MaxDealVolume"));
            tbMaxDealVolume322.DataBindings.Add(new Binding("Text", Parameters, "MaxDealVolume322"));
            tbMinDealVolume322.DataBindings.Add(new Binding("Text", Parameters, "MinDealVolume322"));
            tbMinPE.DataBindings.Add(new Binding("Text", Parameters, "MinPE"));
            tbDaysForVolChange322.DataBindings.Add(new Binding("Text", Parameters, "DaysForVolChange322"));
            tbDealsForVolChange322.DataBindings.Add(new Binding("Text", Parameters, "DealsForVolChange322"));
            tbEpsilon.DataBindings.Add(new Binding("Text", Parameters, "Epsilon"));
            tbLambda.DataBindings.Add(new Binding("Text", Parameters, "Lambda"));
            tbImportanceDealPercent.DataBindings.Add(new Binding("Text", Parameters, "ImportanceDealPercent"));
            tbImportanceDealCount.DataBindings.Add(new Binding("Text", Parameters, "ImportanceDealCount"));
            tbZeroDaysForDecrVol.DataBindings.Add(new Binding("Text", Parameters, "ZeroDaysForDecrVol"));
            tbDDattempts.DataBindings.Add(new Binding("Text", Parameters, "DDattempts"));
            tbMinDelayForMaxDD.DataBindings.Add(new Binding("Text", Parameters, "MinDelayForMaxDD"));
            tbSigmaCountForDDdelay.DataBindings.Add(new Binding("Text", Parameters, "SigmaCountForDDdelay"));
            cbCurrentSettingsFile.DataBindings.Add(new Binding("Checked", Parameters, "CurrentSettingsFile"));
            cbNetPointsProfitDay.DataBindings.Add(new Binding("Checked", Parameters, "NetPointsProfitDay"));
            cbUseTradeLogDateTimeFilter.DataBindings.Add(new Binding("Checked", Parameters, "UseTradeLogDateTimeFilter"));
            dtpTradeLogFilterStartDateTime.DataBindings.Add(new Binding("Value", Parameters, "TradeLogFilterStartDateTime"));
            dtpTradeLogFilterEndDateTime.DataBindings.Add(new Binding("Value", Parameters, "TradeLogFilterEndDateTime"));
            dtpVolumeSetFirstDate.DataBindings.Add(new Binding("Value", Parameters, "SetVolumeStartDateTime"));
            dtpVolumeSetLastDate.DataBindings.Add(new Binding("Value", Parameters, "SetVolumeEndDateTime"));
            cbTurnOffMutation.DataBindings.Add(new Binding("Checked", Parameters, "TurnOffMutation"));
            cbUsedBTlog.DataBindings.Add(new Binding("Checked", Parameters, "BTlog"));
            cbProfitLimit.DataBindings.Add(new Binding("Checked", Parameters, "ProfitLimit"));
            cbUseImportanceDealForVolumeSet.DataBindings.Add(new Binding("Checked", Parameters, "UseImportanceDeals"));
            cbAllFilters.DataBindings.Add(new Binding("Checked", Parameters, "UseAllFilters"));
            rbPortfolio.DataBindings.Add(new Binding("Checked", Parameters, "UsePortfolio"));
            rb322.DataBindings.Add(new Binding("Checked", Parameters, "Use322"));
            cbUseOpslipInFF.DataBindings.Add(new Binding("Checked", Parameters, "UseOpslipInFF"));
            nudMaxConcurrency.DataBindings.Add(new Binding("Value", Parameters, "MaxConcurrency"));
            cbUseOpenCL.DataBindings.Add(new Binding("Checked", Parameters, "UseOpenCLDevice"));
            comboOpenCLDevice.DataBindings.Add(new Binding("Enabled", cbUseOpenCL, "Checked"));

            var devices = new List<Cloo.ComputeDevice>();
            foreach (var platform in Cloo.ComputePlatform.Platforms)
                devices.AddRange(platform.Devices);

            comboOpenCLDevice.DataSource = devices;
            comboOpenCLDevice.DisplayMember = "Name";
        }

        // сборка результатов из нескольких сетов
        private void btnXMLParsing_Click(object sender, EventArgs e)
        {
            string currentSettings = "";
            List<string> countSymbol = new List<string>();
            List<string> countSymbol_ = new List<string>();
            List<string> countAgrExAll = new List<string>();
            List<string> countAgrEx = new List<string>();

            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory+"/Recomends/");

            // Display the names of the directories.
            foreach (DirectoryInfo dri in di.GetDirectories())
            {
                if (dri.Name != "log")
                {
                    countSymbol_.Add(dri.Name);
                    countSymbol.Add(dri.Name.Replace('_', '/'));
                    DirectoryInfo fi = new DirectoryInfo(Environment.CurrentDirectory + "/Recomends/"+dri.Name);
                    foreach (var fri in fi.GetFiles())
                    {
                        countAgrExAll.Add(fri.Name.Replace(".txt",""));
                    }
                }
            }
            countAgrEx = countAgrExAll.Distinct().ToList();    

            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Current settings file (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                currentSettings = openFileDialog.FileName;
                                    XmlSerializer xml = new XmlSerializer(typeof(Settings));

                        StreamReader file = new StreamReader(currentSettings);
                        GeneticAlgo.Settings = (Settings)xml.Deserialize(file); 
            }          
            Algo.XMLcopyDefTrivial(countSymbol, countAgrEx, currentSettings);
        }

        private void Optimize()
        {
            Invoke(new Action(() =>
            {
                LockControls();
                timer.Start();
                ElapsedTime = new TimeSpan(0);
                EsitmatedTime = new TimeSpan(0);
                startDateTime = DateTime.Now;
            }));
            Algo.StartOptimization();
        }

        public void LockControls()
        {
            foreach (Control c in Controls)
            {
                c.Enabled = false;
            }
            progressBar.Enabled = true;
            lbElapsedTime.Enabled = true;
            lbElapsedTimeValue.Enabled = true;
            lbEstimatedTime.Enabled = true;
            lbEstimatedTimeValue.Enabled = true;
        }

        public void UnlockControls()
        {
            foreach (Control c in Controls)
            {
                c.Enabled = true;
            }
            UpdateControlsAccesibility();
        }

        private void btnOptimization_Click(object sender, EventArgs e)
        {
            if (Algo.SelectLogFile())
            {
                if (Algo.SelectSettingsFile())
                {
                    Task.Factory.StartNew(Optimize);
                }
                else
                {
                    MessageBox.Show("Please select settings filename!");
                }
            }
            else
            {
                MessageBox.Show("Please select log filename!");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            tbFrequencyOfOccurrenceNullIndividual.Enabled = !tbFrequencyOfOccurrenceNullIndividual.Enabled;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ElapsedTime = DateTime.Now - startDateTime;
            lbElapsedTimeValue.Text = ElapsedTime.ToString("hh\\:mm\\:ss");
            if (EsitmatedTime.Ticks != 0)
            {
                lbEstimatedTimeValue.Text = EsitmatedTime.ToString("hh\\:mm\\:ss");
                lbProjectedTotalTimeValue.Text = ProjectedTotalTime.ToString("hh\\:mm\\:ss");
            }
        }

        private void cbUseTradeLogDateTimeFilter_CheckedChanged(object sender, EventArgs e)
        {
            UpdateControlsAccesibility();
        }

        private void UpdateControlsAccesibility()
        {
            dtpTradeLogFilterStartDateTime.Enabled = cbUseTradeLogDateTimeFilter.Checked;
            dtpTradeLogFilterEndDateTime.Enabled = cbUseTradeLogDateTimeFilter.Checked;
        }

        private void btSetVolumes_Click(object sender, EventArgs e)
        {           
            if (Algo.SelectLogFile())
            {
                if (Algo.SelectSettingsFile())
                {
                    Parameters.isVolumeSet = true;
                    TradeInfoCollection trades;
                    string logFilename = Algo.logFilename;
                    string currentSettingsFile = Algo.currentSettings;
                    string dir;
                    dir = Environment.CurrentDirectory;
                    Algo.CopyStepsAndCountDecimalDigits();
                    Filters OptimizationSettings = Filters.LoadOptimizationSettings("OptimizationSettings.xml");
                    trades = new TradeInfoCollection(logFilename, Parameters, Algo.Steps, OptimizationSettings.ParametersIndexes, OptimizationSettings);

                    int daysCount;
                    daysCount = trades.allDates.Count < Parameters.DaysForSetVolumes ? trades.allDates.Count : Parameters.DaysForSetVolumes;
                    VolumesSetter volume;
                    volume = new VolumesSetter(Parameters);
                    ConcurrentDictionary<string, ConcurrentDictionary<string, int>> Volumes;
                    Volumes = new ConcurrentDictionary<string, ConcurrentDictionary<string, int>>();
                    if (Parameters.Use322)
                    {
                        Volumes = volume.CalculateVolumes322(trades.DaysProfitExpectancy);
                    }
                    if (Parameters.UsePortfolio)
                    {
                        var averagePoints = volume.CalculateAveragePointsPerDay(trades.DaysPoints, daysCount);
                        var CovariationMatrix = volume.GetCovariationMatrix(trades.DaysPoints, averagePoints, daysCount);
                        Volumes = volume.GetVolumes(CovariationMatrix, averagePoints);
                    }
                    volume.GetXMLwithVolumes(currentSettingsFile, Volumes);
                    MessageBox.Show("Volumes are calculated!");
                }
                else
                {
                    MessageBox.Show("Please select settings filename!");
                }
            }
            else
            {
                MessageBox.Show("Please select log filename!");
            }
        }

        private void btSetMaxDD_Click(object sender, EventArgs e)
        {
            if (Algo.SelectLogFile())
            {
                if (Algo.SelectSettingsFile())
                {
                    Parameters.isMaxDDSet = true;
                    TradeInfoCollection trades;
                    string logFilename = Algo.logFilename;
                    string currentSettingsFile = Algo.currentSettings;
                    string dir;
                    dir = Environment.CurrentDirectory;
                    Algo.CopyStepsAndCountDecimalDigits();

                    Filters OptimizationSettings = Filters.LoadOptimizationSettings("OptimizationSettings.xml");
                    trades = new TradeInfoCollection(logFilename, Parameters, Algo.Steps, OptimizationSettings.ParametersIndexes, OptimizationSettings);
                    MaxDDSetter ddset = new MaxDDSetter(Parameters, trades.DealsPoints, currentSettingsFile, trades.DealsPointsGlobal, trades.DealsPointsPercent, trades.totalDealsCount);
                    if (ddset.GetXMLmaxDD())
                    {
                        MessageBox.Show("MaxDD and delays are calculated!");
                    }
                    else
                    {
                        MessageBox.Show("There are some problems with MaxDD and delays calculating!");
                    }

                }
                else
                {
                    MessageBox.Show("Please select settings filename!");
                }
            }
            else
            {
                MessageBox.Show("Please select log filename!");
            }
        }

        private void comboOpenCLDevice_SelectedValueChanged(object sender, EventArgs e)
        {
            Parameters.OpenCLDevice = comboOpenCLDevice.SelectedValue as Cloo.ComputeDevice;
        }

    }
}
