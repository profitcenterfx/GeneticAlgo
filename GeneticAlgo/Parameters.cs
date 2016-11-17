using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgo
{
    public class Parameters
    {
        public int Population { get; set; }

        public float MinAdvance { get; set; }

        public float MinAdvanceGlobal { get; set; }

        public int CountOfVariable { get; set; }

        public float SelectionPower1 { get; set; }

        public float SelectionPower2 { get; set; }

        public float SigmaCountForDDdelay { get; set; }

        public float FrequencyOfOccurrenceNullIndividual { get; set; }

        public int CountOfGenerations { get; set; }

        public int VariableNumberOfSlots { get; set; }

        public int MinNumberOfTrades { get; set; }

        public int MinNumberOfTradesAfterPreopting { get; set; }

        public float MutationCoef1 { get; set; }

        public int MutationCoef2 { get; set; }

        public float MutationCoef3 { get; set; }

        public float MutationCoef4 { get; set; }

        public int NumberOfTheBestIndividuals { get; set; }

        public int DaysForSetVolumes { get; set; }

        public int MaxDealVolume { get; set; }

        public int MinDealVolume322 { get; set; }

        public int MaxDealVolume322 { get; set; }

        public int ZeroDaysForDecrVol { get; set; }

        public int CountOfGenerationsWithTheBestInd { get; set; }

        public float CoefOfStochastickSelect { get; set; }

        public float ArbPercentForAdvancePreopting { get; set; }

        public float PercentOfOptLog { get; set; }

        public float PercentOfCheckLog { get; set; }

        public float Epsilon { get; set; }

        public float Lambda { get; set; }

        public float ImportanceDealPercent { get; set; }

        public int ImportanceDealCount { get; set; }

        public bool CurrentSettingsFile { get; set; }

        public bool UsePortfolio { get; set; }

        public bool Use322 { get; set; }

        public bool isVolumeSet { get; set; }

        public bool isMaxDDSet { get; set; }

        public bool UseImportanceDeals { get; set; }

        public double mass_multiplier { get; set; }

        public double accel_multiplier { get; set; }

        public bool UseTradeLogDateTimeFilter { get; set; }

        public bool UseAllFilters { get; set; }
        public bool BTlog { get; set; }

        public bool UseOpslipInFF { get; set; }
        public int daysCount { get; set; }

        public float MinPE { get; set; }

        public int DaysForVolChange322 { get; set; }

        public int DealsForVolChange322 { get; set; }

        public int DatePriority { get; set; }

        public int MinDelayForMaxDD { get; set; }

        public int DDattempts { get; set; }

        public int OpenTrendIndex { get; set; }

        public bool NetPointsProfitDay { get; set; }

        private DateTime tradeLogFilterStartDateTime;

        public DateTime TradeLogFilterStartDateTime
        {
            get { return tradeLogFilterStartDateTime; }
            set { tradeLogFilterStartDateTime = new DateTime(value.Ticks, DateTimeKind.Local); }
        }

        private DateTime tradeLogFilterEndDateTime;

        public DateTime TradeLogFilterEndDateTime
        {
            get { return tradeLogFilterEndDateTime; }
            set { tradeLogFilterEndDateTime = new DateTime(value.Ticks, DateTimeKind.Local); }
        }

        private DateTime setVolumeStartDateTime;

        public DateTime SetVolumeStartDateTime
        {
            get { return setVolumeStartDateTime; }
            set { setVolumeStartDateTime = new DateTime(value.Ticks, DateTimeKind.Local); }
        }

        private DateTime setVolumeEndDateTime;
        public DateTime SetVolumeEndDateTime
        {
            get { return setVolumeEndDateTime; }
            set { setVolumeEndDateTime = new DateTime(value.Ticks, DateTimeKind.Local); }
        }

        public bool TurnOffMutation { get; set; }

        public bool ProfitLimit { get; set; }

        public int MaxConcurrency { get; set; }

        public bool UseOpenCLDevice { get; set; }

        public Cloo.ComputeDevice OpenCLDevice { get; set; }

        public int MinTradesForUseOpenCL { get; set; }

        public Parameters()
        {
            Population = 50;
            SelectionPower1 = 10;
            SelectionPower2 = 1;
            FrequencyOfOccurrenceNullIndividual = 1000;
            CountOfGenerations = 10000;
            VariableNumberOfSlots = 2;
            MinNumberOfTrades = 10;
            MinNumberOfTradesAfterPreopting = 10;
            MutationCoef1 = 1;
            MutationCoef2 = 5;
            MutationCoef3 = 1;
            DatePriority = 2;
            DaysForSetVolumes = 10;
            MutationCoef4 = 2F;
            NumberOfTheBestIndividuals = 20;
            CountOfGenerationsWithTheBestInd = 50;
            CoefOfStochastickSelect = 0.5F;
            ArbPercentForAdvancePreopting = 1.01F;
            CurrentSettingsFile = true;
            UseTradeLogDateTimeFilter = false;
            TradeLogFilterStartDateTime = DateTime.UtcNow;
            TradeLogFilterEndDateTime = DateTime.UtcNow;
            SetVolumeStartDateTime = DateTime.UtcNow;
            SetVolumeEndDateTime = DateTime.UtcNow;
            TurnOffMutation = false;
            BTlog = false;
            ProfitLimit = false;
            UseAllFilters = true;
            UseOpslipInFF = false;
            isVolumeSet = false;
            isMaxDDSet = false;
            mass_multiplier = 0.001;
            accel_multiplier = 10000;
            PercentOfOptLog = 1.0f;
            PercentOfCheckLog = 0.0f;
            MaxConcurrency = Environment.ProcessorCount;
            MaxDealVolume = 150000;
            Epsilon = 0.001f;
            Lambda = 0.0001f;
            Use322 = true;
            UsePortfolio = false;
            UseImportanceDeals = true;
            MinDealVolume322 = 1000;
            MaxDealVolume322 = 100000;
            MinDelayForMaxDD = 60;
            MinPE = 0f;
            DaysForVolChange322 = 2;
            DealsForVolChange322 = 3;
            ImportanceDealPercent = 0;
            ImportanceDealCount = 3;
            DDattempts = 3;
            UseOpenCLDevice = false;
            SigmaCountForDDdelay = 0;
            ZeroDaysForDecrVol = 5;
            NetPointsProfitDay = true;
            UseOpenCLDevice = false;//true;
            MinTradesForUseOpenCL = 500;
        }
    }
}
