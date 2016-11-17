namespace GeneticAlgo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbPopulation = new System.Windows.Forms.TextBox();
            this.labelPopulation = new System.Windows.Forms.Label();
            this.tbSelectionPower1 = new System.Windows.Forms.TextBox();
            this.tbSelectionPower2 = new System.Windows.Forms.TextBox();
            this.tbCountOfGenerations = new System.Windows.Forms.TextBox();
            this.tbMinNumberOfTrades = new System.Windows.Forms.TextBox();
            this.tbMutationCoef1 = new System.Windows.Forms.TextBox();
            this.tbMutationCoef2 = new System.Windows.Forms.TextBox();
            this.tbMutationCoef3 = new System.Windows.Forms.TextBox();
            this.tbMutationCoef4 = new System.Windows.Forms.TextBox();
            this.tbVariableNumberOfSlots = new System.Windows.Forms.TextBox();
            this.tbFrequencyOfOccurrenceNullIndividual = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbCurrentSettingsFile = new System.Windows.Forms.CheckBox();
            this.labelSelectionPower = new System.Windows.Forms.Label();
            this.labelFrequencyOfOccurrenceNullIndividual = new System.Windows.Forms.Label();
            this.labelVariableNumberOfSlots = new System.Windows.Forms.Label();
            this.labelCountOfGenerations = new System.Windows.Forms.Label();
            this.labelMinNumberOfTrades = new System.Windows.Forms.Label();
            this.labelMutationCoef = new System.Windows.Forms.Label();
            this.tbNumberOfTheBestIndividuals = new System.Windows.Forms.TextBox();
            this.tbCountOfGenerationsWithTheBestInd = new System.Windows.Forms.TextBox();
            this.tbCoefOfStochastickSelect = new System.Windows.Forms.TextBox();
            this.labelNumberOfTheBestIndividuals = new System.Windows.Forms.Label();
            this.labelCountOfGenerationsWithTheBestInd = new System.Windows.Forms.Label();
            this.labelCoefOfStochastickSelect = new System.Windows.Forms.Label();
            this.btnOptimization = new System.Windows.Forms.Button();
            this.btnXMLParsing = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.lbElapsedTime = new System.Windows.Forms.Label();
            this.lbElapsedTimeValue = new System.Windows.Forms.Label();
            this.lbEstimatedTime = new System.Windows.Forms.Label();
            this.lbEstimatedTimeValue = new System.Windows.Forms.Label();
            this.dtpTradeLogFilterStartDateTime = new System.Windows.Forms.DateTimePicker();
            this.dtpTradeLogFilterEndDateTime = new System.Windows.Forms.DateTimePicker();
            this.lbTradeLogFilterStartDateTime = new System.Windows.Forms.Label();
            this.lbTradeLogFilterEndDateTime = new System.Windows.Forms.Label();
            this.cbUseTradeLogDateTimeFilter = new System.Windows.Forms.CheckBox();
            this.cbTurnOffMutation = new System.Windows.Forms.CheckBox();
            this.labelPercentArbiTradeForAdvancePre_opting = new System.Windows.Forms.Label();
            this.tbPercentArbiTradeForAdvancePre_opting = new System.Windows.Forms.TextBox();
            this.tbMinNumberOfTradesAfterPreopting = new System.Windows.Forms.TextBox();
            this.cbUsedBTlog = new System.Windows.Forms.CheckBox();
            this.cbProfitLimit = new System.Windows.Forms.CheckBox();
            this.tbPercentOfOptLog = new System.Windows.Forms.TextBox();
            this.tbPercentOfCheckLog = new System.Windows.Forms.TextBox();
            this.cbAllFilters = new System.Windows.Forms.CheckBox();
            this.cbUseOpslipInFF = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbProjectedTotalTimeValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMaxConcurrency = new System.Windows.Forms.NumericUpDown();
            this.tbDatePriority = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDaysForSetVolumes = new System.Windows.Forms.TextBox();
            this.tbMaxDealVolume = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btSetVolumes = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbEpsilon = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbLambda = new System.Windows.Forms.TextBox();
            this.dtpVolumeSetFirstDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb322 = new System.Windows.Forms.RadioButton();
            this.rbPortfolio = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.tbMinDealVolume322 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbMaxDealVolume322 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbMinPE = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbDaysForVolChange322 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbDealsForVolChange322 = new System.Windows.Forms.TextBox();
            this.dtpVolumeSetLastDate = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbImportanceDealPercent = new System.Windows.Forms.TextBox();
            this.btSetMaxDD = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tbMinDelayForMaxDD = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.tbDDattempts = new System.Windows.Forms.TextBox();
            this.cbUseImportanceDealForVolumeSet = new System.Windows.Forms.CheckBox();
            this.comboOpenCLDevice = new System.Windows.Forms.ComboBox();
            this.cbUseOpenCL = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbSigmaCountForDDdelay = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbImportanceDealCount = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tbZeroDaysForDecrVol = new System.Windows.Forms.TextBox();
            this.cbNetPointsProfitDay = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxConcurrency)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbPopulation
            // 
            this.tbPopulation.Location = new System.Drawing.Point(155, 12);
            this.tbPopulation.Name = "tbPopulation";
            this.tbPopulation.Size = new System.Drawing.Size(60, 20);
            this.tbPopulation.TabIndex = 0;
            // 
            // labelPopulation
            // 
            this.labelPopulation.AutoSize = true;
            this.labelPopulation.Location = new System.Drawing.Point(12, 15);
            this.labelPopulation.Name = "labelPopulation";
            this.labelPopulation.Size = new System.Drawing.Size(57, 13);
            this.labelPopulation.TabIndex = 11;
            this.labelPopulation.Text = "Population";
            // 
            // tbSelectionPower1
            // 
            this.tbSelectionPower1.Location = new System.Drawing.Point(431, 12);
            this.tbSelectionPower1.Name = "tbSelectionPower1";
            this.tbSelectionPower1.Size = new System.Drawing.Size(60, 20);
            this.tbSelectionPower1.TabIndex = 21;
            // 
            // tbSelectionPower2
            // 
            this.tbSelectionPower2.Location = new System.Drawing.Point(497, 12);
            this.tbSelectionPower2.Name = "tbSelectionPower2";
            this.tbSelectionPower2.Size = new System.Drawing.Size(60, 20);
            this.tbSelectionPower2.TabIndex = 22;
            // 
            // tbCountOfGenerations
            // 
            this.tbCountOfGenerations.Location = new System.Drawing.Point(431, 63);
            this.tbCountOfGenerations.Name = "tbCountOfGenerations";
            this.tbCountOfGenerations.Size = new System.Drawing.Size(60, 20);
            this.tbCountOfGenerations.TabIndex = 23;
            // 
            // tbMinNumberOfTrades
            // 
            this.tbMinNumberOfTrades.Location = new System.Drawing.Point(431, 89);
            this.tbMinNumberOfTrades.Name = "tbMinNumberOfTrades";
            this.tbMinNumberOfTrades.Size = new System.Drawing.Size(60, 20);
            this.tbMinNumberOfTrades.TabIndex = 24;
            // 
            // tbMutationCoef1
            // 
            this.tbMutationCoef1.Location = new System.Drawing.Point(431, 115);
            this.tbMutationCoef1.Name = "tbMutationCoef1";
            this.tbMutationCoef1.Size = new System.Drawing.Size(60, 20);
            this.tbMutationCoef1.TabIndex = 25;
            // 
            // tbMutationCoef2
            // 
            this.tbMutationCoef2.Location = new System.Drawing.Point(497, 115);
            this.tbMutationCoef2.Name = "tbMutationCoef2";
            this.tbMutationCoef2.Size = new System.Drawing.Size(60, 20);
            this.tbMutationCoef2.TabIndex = 26;
            // 
            // tbMutationCoef3
            // 
            this.tbMutationCoef3.Location = new System.Drawing.Point(563, 115);
            this.tbMutationCoef3.Name = "tbMutationCoef3";
            this.tbMutationCoef3.Size = new System.Drawing.Size(60, 20);
            this.tbMutationCoef3.TabIndex = 27;
            // 
            // tbMutationCoef4
            // 
            this.tbMutationCoef4.Location = new System.Drawing.Point(629, 115);
            this.tbMutationCoef4.Name = "tbMutationCoef4";
            this.tbMutationCoef4.Size = new System.Drawing.Size(60, 20);
            this.tbMutationCoef4.TabIndex = 28;
            // 
            // tbVariableNumberOfSlots
            // 
            this.tbVariableNumberOfSlots.Location = new System.Drawing.Point(629, 63);
            this.tbVariableNumberOfSlots.Name = "tbVariableNumberOfSlots";
            this.tbVariableNumberOfSlots.Size = new System.Drawing.Size(60, 20);
            this.tbVariableNumberOfSlots.TabIndex = 29;
            // 
            // tbFrequencyOfOccurrenceNullIndividual
            // 
            this.tbFrequencyOfOccurrenceNullIndividual.Location = new System.Drawing.Point(563, 37);
            this.tbFrequencyOfOccurrenceNullIndividual.Name = "tbFrequencyOfOccurrenceNullIndividual";
            this.tbFrequencyOfOccurrenceNullIndividual.Size = new System.Drawing.Size(60, 20);
            this.tbFrequencyOfOccurrenceNullIndividual.TabIndex = 30;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(629, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 31;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbCurrentSettingsFile
            // 
            this.cbCurrentSettingsFile.AutoSize = true;
            this.cbCurrentSettingsFile.Checked = true;
            this.cbCurrentSettingsFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCurrentSettingsFile.Location = new System.Drawing.Point(431, 144);
            this.cbCurrentSettingsFile.Name = "cbCurrentSettingsFile";
            this.cbCurrentSettingsFile.Size = new System.Drawing.Size(210, 17);
            this.cbCurrentSettingsFile.TabIndex = 32;
            this.cbCurrentSettingsFile.Text = "Apply default filters from the settings file";
            this.cbCurrentSettingsFile.UseVisualStyleBackColor = true;
            // 
            // labelSelectionPower
            // 
            this.labelSelectionPower.AutoSize = true;
            this.labelSelectionPower.Location = new System.Drawing.Point(342, 15);
            this.labelSelectionPower.Name = "labelSelectionPower";
            this.labelSelectionPower.Size = new System.Drawing.Size(83, 13);
            this.labelSelectionPower.TabIndex = 33;
            this.labelSelectionPower.Text = "Selection power";
            // 
            // labelFrequencyOfOccurrenceNullIndividual
            // 
            this.labelFrequencyOfOccurrenceNullIndividual.AutoSize = true;
            this.labelFrequencyOfOccurrenceNullIndividual.Location = new System.Drawing.Point(319, 39);
            this.labelFrequencyOfOccurrenceNullIndividual.Name = "labelFrequencyOfOccurrenceNullIndividual";
            this.labelFrequencyOfOccurrenceNullIndividual.Size = new System.Drawing.Size(238, 13);
            this.labelFrequencyOfOccurrenceNullIndividual.TabIndex = 34;
            this.labelFrequencyOfOccurrenceNullIndividual.Text = "Frequency of occurrence \"null\" individual, like 1/";
            // 
            // labelVariableNumberOfSlots
            // 
            this.labelVariableNumberOfSlots.AutoSize = true;
            this.labelVariableNumberOfSlots.Location = new System.Drawing.Point(504, 66);
            this.labelVariableNumberOfSlots.Name = "labelVariableNumberOfSlots";
            this.labelVariableNumberOfSlots.Size = new System.Drawing.Size(119, 13);
            this.labelVariableNumberOfSlots.TabIndex = 35;
            this.labelVariableNumberOfSlots.Text = "Variable number of slots";
            // 
            // labelCountOfGenerations
            // 
            this.labelCountOfGenerations.AutoSize = true;
            this.labelCountOfGenerations.Location = new System.Drawing.Point(320, 66);
            this.labelCountOfGenerations.Name = "labelCountOfGenerations";
            this.labelCountOfGenerations.Size = new System.Drawing.Size(105, 13);
            this.labelCountOfGenerations.TabIndex = 36;
            this.labelCountOfGenerations.Text = "Count of generations";
            // 
            // labelMinNumberOfTrades
            // 
            this.labelMinNumberOfTrades.AutoSize = true;
            this.labelMinNumberOfTrades.Location = new System.Drawing.Point(320, 92);
            this.labelMinNumberOfTrades.Name = "labelMinNumberOfTrades";
            this.labelMinNumberOfTrades.Size = new System.Drawing.Size(106, 13);
            this.labelMinNumberOfTrades.TabIndex = 37;
            this.labelMinNumberOfTrades.Text = "Min number of trades";
            // 
            // labelMutationCoef
            // 
            this.labelMutationCoef.AutoSize = true;
            this.labelMutationCoef.Location = new System.Drawing.Point(351, 118);
            this.labelMutationCoef.Name = "labelMutationCoef";
            this.labelMutationCoef.Size = new System.Drawing.Size(75, 13);
            this.labelMutationCoef.TabIndex = 38;
            this.labelMutationCoef.Text = "Mutation coef.";
            // 
            // tbNumberOfTheBestIndividuals
            // 
            this.tbNumberOfTheBestIndividuals.Location = new System.Drawing.Point(431, 171);
            this.tbNumberOfTheBestIndividuals.Name = "tbNumberOfTheBestIndividuals";
            this.tbNumberOfTheBestIndividuals.Size = new System.Drawing.Size(60, 20);
            this.tbNumberOfTheBestIndividuals.TabIndex = 39;
            // 
            // tbCountOfGenerationsWithTheBestInd
            // 
            this.tbCountOfGenerationsWithTheBestInd.Location = new System.Drawing.Point(431, 197);
            this.tbCountOfGenerationsWithTheBestInd.Name = "tbCountOfGenerationsWithTheBestInd";
            this.tbCountOfGenerationsWithTheBestInd.Size = new System.Drawing.Size(60, 20);
            this.tbCountOfGenerationsWithTheBestInd.TabIndex = 40;
            // 
            // tbCoefOfStochastickSelect
            // 
            this.tbCoefOfStochastickSelect.Location = new System.Drawing.Point(629, 171);
            this.tbCoefOfStochastickSelect.Name = "tbCoefOfStochastickSelect";
            this.tbCoefOfStochastickSelect.Size = new System.Drawing.Size(60, 20);
            this.tbCoefOfStochastickSelect.TabIndex = 41;
            // 
            // labelNumberOfTheBestIndividuals
            // 
            this.labelNumberOfTheBestIndividuals.AutoSize = true;
            this.labelNumberOfTheBestIndividuals.Location = new System.Drawing.Point(277, 174);
            this.labelNumberOfTheBestIndividuals.Name = "labelNumberOfTheBestIndividuals";
            this.labelNumberOfTheBestIndividuals.Size = new System.Drawing.Size(149, 13);
            this.labelNumberOfTheBestIndividuals.TabIndex = 42;
            this.labelNumberOfTheBestIndividuals.Text = "Number of the best individuals";
            // 
            // labelCountOfGenerationsWithTheBestInd
            // 
            this.labelCountOfGenerationsWithTheBestInd.AutoSize = true;
            this.labelCountOfGenerationsWithTheBestInd.Location = new System.Drawing.Point(238, 200);
            this.labelCountOfGenerationsWithTheBestInd.Name = "labelCountOfGenerationsWithTheBestInd";
            this.labelCountOfGenerationsWithTheBestInd.Size = new System.Drawing.Size(188, 13);
            this.labelCountOfGenerationsWithTheBestInd.TabIndex = 43;
            this.labelCountOfGenerationsWithTheBestInd.Text = "Count of generations with the best ind.";
            // 
            // labelCoefOfStochastickSelect
            // 
            this.labelCoefOfStochastickSelect.AutoSize = true;
            this.labelCoefOfStochastickSelect.Location = new System.Drawing.Point(494, 174);
            this.labelCoefOfStochastickSelect.Name = "labelCoefOfStochastickSelect";
            this.labelCoefOfStochastickSelect.Size = new System.Drawing.Size(132, 13);
            this.labelCoefOfStochastickSelect.TabIndex = 44;
            this.labelCoefOfStochastickSelect.Text = "Coef. of stochastick select";
            // 
            // btnOptimization
            // 
            this.btnOptimization.Location = new System.Drawing.Point(199, 409);
            this.btnOptimization.Name = "btnOptimization";
            this.btnOptimization.Size = new System.Drawing.Size(75, 21);
            this.btnOptimization.TabIndex = 45;
            this.btnOptimization.Text = "Optimization";
            this.btnOptimization.UseVisualStyleBackColor = true;
            this.btnOptimization.Click += new System.EventHandler(this.btnOptimization_Click);
            // 
            // btnXMLParsing
            // 
            this.btnXMLParsing.Location = new System.Drawing.Point(280, 409);
            this.btnXMLParsing.Name = "btnXMLParsing";
            this.btnXMLParsing.Size = new System.Drawing.Size(75, 21);
            this.btnXMLParsing.TabIndex = 46;
            this.btnXMLParsing.Text = "XML-parsing";
            this.btnXMLParsing.UseVisualStyleBackColor = true;
            this.btnXMLParsing.Click += new System.EventHandler(this.btnXMLParsing_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 297);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(674, 21);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 49;
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // lbElapsedTime
            // 
            this.lbElapsedTime.AutoSize = true;
            this.lbElapsedTime.Location = new System.Drawing.Point(25, 330);
            this.lbElapsedTime.Name = "lbElapsedTime";
            this.lbElapsedTime.Size = new System.Drawing.Size(70, 13);
            this.lbElapsedTime.TabIndex = 50;
            this.lbElapsedTime.Text = "Elapsed time:";
            // 
            // lbElapsedTimeValue
            // 
            this.lbElapsedTimeValue.AutoSize = true;
            this.lbElapsedTimeValue.Location = new System.Drawing.Point(98, 330);
            this.lbElapsedTimeValue.Name = "lbElapsedTimeValue";
            this.lbElapsedTimeValue.Size = new System.Drawing.Size(49, 13);
            this.lbElapsedTimeValue.TabIndex = 51;
            this.lbElapsedTimeValue.Text = "00:00:00";
            // 
            // lbEstimatedTime
            // 
            this.lbEstimatedTime.AutoSize = true;
            this.lbEstimatedTime.Location = new System.Drawing.Point(153, 330);
            this.lbEstimatedTime.Name = "lbEstimatedTime";
            this.lbEstimatedTime.Size = new System.Drawing.Size(78, 13);
            this.lbEstimatedTime.TabIndex = 52;
            this.lbEstimatedTime.Text = "Estimated time:";
            // 
            // lbEstimatedTimeValue
            // 
            this.lbEstimatedTimeValue.AutoSize = true;
            this.lbEstimatedTimeValue.Location = new System.Drawing.Point(231, 330);
            this.lbEstimatedTimeValue.Name = "lbEstimatedTimeValue";
            this.lbEstimatedTimeValue.Size = new System.Drawing.Size(49, 13);
            this.lbEstimatedTimeValue.TabIndex = 53;
            this.lbEstimatedTimeValue.Text = "00:00:00";
            // 
            // dtpTradeLogFilterStartDateTime
            // 
            this.dtpTradeLogFilterStartDateTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dtpTradeLogFilterStartDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTradeLogFilterStartDateTime.Location = new System.Drawing.Point(212, 257);
            this.dtpTradeLogFilterStartDateTime.Name = "dtpTradeLogFilterStartDateTime";
            this.dtpTradeLogFilterStartDateTime.Size = new System.Drawing.Size(200, 20);
            this.dtpTradeLogFilterStartDateTime.TabIndex = 54;
            // 
            // dtpTradeLogFilterEndDateTime
            // 
            this.dtpTradeLogFilterEndDateTime.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.dtpTradeLogFilterEndDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTradeLogFilterEndDateTime.Location = new System.Drawing.Point(444, 257);
            this.dtpTradeLogFilterEndDateTime.Name = "dtpTradeLogFilterEndDateTime";
            this.dtpTradeLogFilterEndDateTime.Size = new System.Drawing.Size(200, 20);
            this.dtpTradeLogFilterEndDateTime.TabIndex = 55;
            // 
            // lbTradeLogFilterStartDateTime
            // 
            this.lbTradeLogFilterStartDateTime.AutoSize = true;
            this.lbTradeLogFilterStartDateTime.Location = new System.Drawing.Point(180, 260);
            this.lbTradeLogFilterStartDateTime.Name = "lbTradeLogFilterStartDateTime";
            this.lbTradeLogFilterStartDateTime.Size = new System.Drawing.Size(30, 13);
            this.lbTradeLogFilterStartDateTime.TabIndex = 56;
            this.lbTradeLogFilterStartDateTime.Text = "from:";
            // 
            // lbTradeLogFilterEndDateTime
            // 
            this.lbTradeLogFilterEndDateTime.AutoSize = true;
            this.lbTradeLogFilterEndDateTime.Location = new System.Drawing.Point(418, 260);
            this.lbTradeLogFilterEndDateTime.Name = "lbTradeLogFilterEndDateTime";
            this.lbTradeLogFilterEndDateTime.Size = new System.Drawing.Size(19, 13);
            this.lbTradeLogFilterEndDateTime.TabIndex = 57;
            this.lbTradeLogFilterEndDateTime.Text = "to:";
            // 
            // cbUseTradeLogDateTimeFilter
            // 
            this.cbUseTradeLogDateTimeFilter.AutoSize = true;
            this.cbUseTradeLogDateTimeFilter.Location = new System.Drawing.Point(15, 260);
            this.cbUseTradeLogDateTimeFilter.Name = "cbUseTradeLogDateTimeFilter";
            this.cbUseTradeLogDateTimeFilter.Size = new System.Drawing.Size(159, 17);
            this.cbUseTradeLogDateTimeFilter.TabIndex = 58;
            this.cbUseTradeLogDateTimeFilter.Text = "Use trade log date/time filter";
            this.cbUseTradeLogDateTimeFilter.UseVisualStyleBackColor = true;
            this.cbUseTradeLogDateTimeFilter.CheckedChanged += new System.EventHandler(this.cbUseTradeLogDateTimeFilter_CheckedChanged);
            // 
            // cbTurnOffMutation
            // 
            this.cbTurnOffMutation.AutoSize = true;
            this.cbTurnOffMutation.Location = new System.Drawing.Point(241, 228);
            this.cbTurnOffMutation.Name = "cbTurnOffMutation";
            this.cbTurnOffMutation.Size = new System.Drawing.Size(106, 17);
            this.cbTurnOffMutation.TabIndex = 59;
            this.cbTurnOffMutation.Text = "Turn mutation off";
            this.cbTurnOffMutation.UseVisualStyleBackColor = true;
            // 
            // labelPercentArbiTradeForAdvancePre_opting
            // 
            this.labelPercentArbiTradeForAdvancePre_opting.AutoSize = true;
            this.labelPercentArbiTradeForAdvancePre_opting.Location = new System.Drawing.Point(12, 37);
            this.labelPercentArbiTradeForAdvancePre_opting.Name = "labelPercentArbiTradeForAdvancePre_opting";
            this.labelPercentArbiTradeForAdvancePre_opting.Size = new System.Drawing.Size(122, 26);
            this.labelPercentArbiTradeForAdvancePre_opting.TabIndex = 63;
            this.labelPercentArbiTradeForAdvancePre_opting.Text = "% arbi-trade for advance\r\npre-opting";
            // 
            // tbPercentArbiTradeForAdvancePre_opting
            // 
            this.tbPercentArbiTradeForAdvancePre_opting.Location = new System.Drawing.Point(155, 39);
            this.tbPercentArbiTradeForAdvancePre_opting.Name = "tbPercentArbiTradeForAdvancePre_opting";
            this.tbPercentArbiTradeForAdvancePre_opting.Size = new System.Drawing.Size(60, 20);
            this.tbPercentArbiTradeForAdvancePre_opting.TabIndex = 62;
            // 
            // tbMinNumberOfTradesAfterPreopting
            // 
            this.tbMinNumberOfTradesAfterPreopting.Location = new System.Drawing.Point(497, 89);
            this.tbMinNumberOfTradesAfterPreopting.Name = "tbMinNumberOfTradesAfterPreopting";
            this.tbMinNumberOfTradesAfterPreopting.Size = new System.Drawing.Size(60, 20);
            this.tbMinNumberOfTradesAfterPreopting.TabIndex = 64;
            // 
            // cbUsedBTlog
            // 
            this.cbUsedBTlog.AutoSize = true;
            this.cbUsedBTlog.Location = new System.Drawing.Point(421, 223);
            this.cbUsedBTlog.Name = "cbUsedBTlog";
            this.cbUsedBTlog.Size = new System.Drawing.Size(85, 17);
            this.cbUsedBTlog.TabIndex = 65;
            this.cbUsedBTlog.Text = "Used BT log";
            this.cbUsedBTlog.UseVisualStyleBackColor = true;
            // 
            // cbProfitLimit
            // 
            this.cbProfitLimit.AutoSize = true;
            this.cbProfitLimit.Location = new System.Drawing.Point(515, 223);
            this.cbProfitLimit.Name = "cbProfitLimit";
            this.cbProfitLimit.Size = new System.Drawing.Size(113, 17);
            this.cbProfitLimit.TabIndex = 66;
            this.cbProfitLimit.Text = "Profit limit (50 pips)";
            this.cbProfitLimit.UseVisualStyleBackColor = true;
            // 
            // tbPercentOfOptLog
            // 
            this.tbPercentOfOptLog.Location = new System.Drawing.Point(497, 197);
            this.tbPercentOfOptLog.Name = "tbPercentOfOptLog";
            this.tbPercentOfOptLog.Size = new System.Drawing.Size(60, 20);
            this.tbPercentOfOptLog.TabIndex = 68;
            // 
            // tbPercentOfCheckLog
            // 
            this.tbPercentOfCheckLog.Location = new System.Drawing.Point(563, 196);
            this.tbPercentOfCheckLog.Name = "tbPercentOfCheckLog";
            this.tbPercentOfCheckLog.Size = new System.Drawing.Size(60, 20);
            this.tbPercentOfCheckLog.TabIndex = 69;
            // 
            // cbAllFilters
            // 
            this.cbAllFilters.AutoSize = true;
            this.cbAllFilters.Location = new System.Drawing.Point(563, 329);
            this.cbAllFilters.Name = "cbAllFilters";
            this.cbAllFilters.Size = new System.Drawing.Size(63, 17);
            this.cbAllFilters.TabIndex = 70;
            this.cbAllFilters.Text = "all filters";
            this.cbAllFilters.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbAllFilters.UseVisualStyleBackColor = true;
            // 
            // cbUseOpslipInFF
            // 
            this.cbUseOpslipInFF.AutoSize = true;
            this.cbUseOpslipInFF.Location = new System.Drawing.Point(452, 329);
            this.cbUseOpslipInFF.Name = "cbUseOpslipInFF";
            this.cbUseOpslipInFF.Size = new System.Drawing.Size(105, 17);
            this.cbUseOpslipInFF.TabIndex = 71;
            this.cbUseOpslipInFF.Text = "Use OpSlip in FF";
            this.cbUseOpslipInFF.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cbUseOpslipInFF.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(286, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 72;
            this.label1.Text = "Projected total time:";
            // 
            // lbProjectedTotalTimeValue
            // 
            this.lbProjectedTotalTimeValue.AutoSize = true;
            this.lbProjectedTotalTimeValue.Location = new System.Drawing.Point(389, 330);
            this.lbProjectedTotalTimeValue.Name = "lbProjectedTotalTimeValue";
            this.lbProjectedTotalTimeValue.Size = new System.Drawing.Size(49, 13);
            this.lbProjectedTotalTimeValue.TabIndex = 73;
            this.lbProjectedTotalTimeValue.Text = "00:00:00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 413);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 75;
            this.label2.Text = "Max. concurrency:";
            // 
            // nudMaxConcurrency
            // 
            this.nudMaxConcurrency.Location = new System.Drawing.Point(115, 411);
            this.nudMaxConcurrency.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudMaxConcurrency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxConcurrency.Name = "nudMaxConcurrency";
            this.nudMaxConcurrency.Size = new System.Drawing.Size(78, 20);
            this.nudMaxConcurrency.TabIndex = 76;
            this.nudMaxConcurrency.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tbDatePriority
            // 
            this.tbDatePriority.Location = new System.Drawing.Point(629, 407);
            this.tbDatePriority.Name = "tbDatePriority";
            this.tbDatePriority.Size = new System.Drawing.Size(60, 20);
            this.tbDatePriority.TabIndex = 77;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(532, 407);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Days priority coef. ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 383);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 13);
            this.label4.TabIndex = 80;
            this.label4.Text = "Days count for setting volumes";
            // 
            // tbDaysForSetVolumes
            // 
            this.tbDaysForSetVolumes.Location = new System.Drawing.Point(168, 379);
            this.tbDaysForSetVolumes.Name = "tbDaysForSetVolumes";
            this.tbDaysForSetVolumes.Size = new System.Drawing.Size(60, 20);
            this.tbDaysForSetVolumes.TabIndex = 79;
            // 
            // tbMaxDealVolume
            // 
            this.tbMaxDealVolume.Location = new System.Drawing.Point(338, 379);
            this.tbMaxDealVolume.Name = "tbMaxDealVolume";
            this.tbMaxDealVolume.Size = new System.Drawing.Size(60, 20);
            this.tbMaxDealVolume.TabIndex = 81;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 383);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 82;
            this.label5.Text = "Max volume of deal";
            // 
            // btSetVolumes
            // 
            this.btSetVolumes.Location = new System.Drawing.Point(361, 409);
            this.btSetVolumes.Name = "btSetVolumes";
            this.btSetVolumes.Size = new System.Drawing.Size(75, 21);
            this.btSetVolumes.TabIndex = 83;
            this.btSetVolumes.Text = "Set volumes";
            this.btSetVolumes.UseVisualStyleBackColor = true;
            this.btSetVolumes.Click += new System.EventHandler(this.btSetVolumes_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(411, 383);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 85;
            this.label6.Text = "Epsilon";
            // 
            // tbEpsilon
            // 
            this.tbEpsilon.Location = new System.Drawing.Point(455, 379);
            this.tbEpsilon.Name = "tbEpsilon";
            this.tbEpsilon.Size = new System.Drawing.Size(60, 20);
            this.tbEpsilon.TabIndex = 84;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(521, 383);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 87;
            this.label7.Text = "Lambda";
            // 
            // tbLambda
            // 
            this.tbLambda.Location = new System.Drawing.Point(568, 379);
            this.tbLambda.Name = "tbLambda";
            this.tbLambda.Size = new System.Drawing.Size(60, 20);
            this.tbLambda.TabIndex = 86;
            // 
            // dtpVolumeSetFirstDate
            // 
            this.dtpVolumeSetFirstDate.CustomFormat = "MM/dd/yyyy";
            this.dtpVolumeSetFirstDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVolumeSetFirstDate.Location = new System.Drawing.Point(200, 482);
            this.dtpVolumeSetFirstDate.Name = "dtpVolumeSetFirstDate";
            this.dtpVolumeSetFirstDate.Size = new System.Drawing.Size(79, 20);
            this.dtpVolumeSetFirstDate.TabIndex = 88;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb322);
            this.groupBox2.Controls.Add(this.rbPortfolio);
            this.groupBox2.Location = new System.Drawing.Point(20, 437);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 39);
            this.groupBox2.TabIndex = 89;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose set volumes method";
            // 
            // rb322
            // 
            this.rb322.AutoSize = true;
            this.rb322.Location = new System.Drawing.Point(95, 16);
            this.rb322.Name = "rb322";
            this.rb322.Size = new System.Drawing.Size(55, 17);
            this.rb322.TabIndex = 1;
            this.rb322.TabStop = true;
            this.rb322.Text = "3+2+2";
            this.rb322.UseVisualStyleBackColor = true;
            // 
            // rbPortfolio
            // 
            this.rbPortfolio.AutoSize = true;
            this.rbPortfolio.Location = new System.Drawing.Point(3, 16);
            this.rbPortfolio.Name = "rbPortfolio";
            this.rbPortfolio.Size = new System.Drawing.Size(62, 17);
            this.rbPortfolio.TabIndex = 0;
            this.rbPortfolio.TabStop = true;
            this.rbPortfolio.Text = "portfolio";
            this.rbPortfolio.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(224, 455);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 91;
            this.label8.Text = "Min volume of deal";
            // 
            // tbMinDealVolume322
            // 
            this.tbMinDealVolume322.Location = new System.Drawing.Point(322, 450);
            this.tbMinDealVolume322.Name = "tbMinDealVolume322";
            this.tbMinDealVolume322.Size = new System.Drawing.Size(60, 20);
            this.tbMinDealVolume322.TabIndex = 90;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(389, 455);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 13);
            this.label9.TabIndex = 93;
            this.label9.Text = "Max volume of deal";
            // 
            // tbMaxDealVolume322
            // 
            this.tbMaxDealVolume322.Location = new System.Drawing.Point(491, 450);
            this.tbMaxDealVolume322.Name = "tbMaxDealVolume322";
            this.tbMaxDealVolume322.Size = new System.Drawing.Size(60, 20);
            this.tbMaxDealVolume322.TabIndex = 92;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(304, 483);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 13);
            this.label11.TabIndex = 95;
            this.label11.Text = "Min PE for grossing Vol";
            // 
            // tbMinPE
            // 
            this.tbMinPE.Location = new System.Drawing.Point(421, 478);
            this.tbMinPE.Name = "tbMinPE";
            this.tbMinPE.Size = new System.Drawing.Size(45, 20);
            this.tbMinPE.TabIndex = 94;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(500, 535);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(141, 13);
            this.label12.TabIndex = 99;
            this.label12.Text = "Days count for changing Vol";
            // 
            // tbDaysForVolChange322
            // 
            this.tbDaysForVolChange322.Location = new System.Drawing.Point(649, 531);
            this.tbDaysForVolChange322.Name = "tbDaysForVolChange322";
            this.tbDaysForVolChange322.Size = new System.Drawing.Size(45, 20);
            this.tbDaysForVolChange322.TabIndex = 98;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(299, 538);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 13);
            this.label13.TabIndex = 101;
            this.label13.Text = "Deals count for changing Vol";
            // 
            // tbDealsForVolChange322
            // 
            this.tbDealsForVolChange322.Location = new System.Drawing.Point(450, 534);
            this.tbDealsForVolChange322.Name = "tbDealsForVolChange322";
            this.tbDealsForVolChange322.Size = new System.Drawing.Size(45, 20);
            this.tbDealsForVolChange322.TabIndex = 100;
            // 
            // dtpVolumeSetLastDate
            // 
            this.dtpVolumeSetLastDate.CustomFormat = "MM/dd/yyyy";
            this.dtpVolumeSetLastDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVolumeSetLastDate.Location = new System.Drawing.Point(199, 506);
            this.dtpVolumeSetLastDate.Name = "dtpVolumeSetLastDate";
            this.dtpVolumeSetLastDate.Size = new System.Drawing.Size(79, 20);
            this.dtpVolumeSetLastDate.TabIndex = 102;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 500);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(166, 13);
            this.label14.TabIndex = 103;
            this.label14.Text = "Dates for setting volumes/maxDD";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(503, 482);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(94, 13);
            this.label16.TabIndex = 106;
            this.label16.Text = "Importance deal %";
            // 
            // tbImportanceDealPercent
            // 
            this.tbImportanceDealPercent.Location = new System.Drawing.Point(599, 477);
            this.tbImportanceDealPercent.Name = "tbImportanceDealPercent";
            this.tbImportanceDealPercent.Size = new System.Drawing.Size(45, 20);
            this.tbImportanceDealPercent.TabIndex = 105;
            // 
            // btSetMaxDD
            // 
            this.btSetMaxDD.Location = new System.Drawing.Point(442, 409);
            this.btSetMaxDD.Name = "btSetMaxDD";
            this.btSetMaxDD.Size = new System.Drawing.Size(75, 21);
            this.btSetMaxDD.TabIndex = 107;
            this.btSetMaxDD.Text = "Set max DD";
            this.btSetMaxDD.UseVisualStyleBackColor = true;
            this.btSetMaxDD.Click += new System.EventHandler(this.btSetMaxDD_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(312, 509);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 13);
            this.label10.TabIndex = 97;
            this.label10.Text = "Max PE for rising Vol";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(180, 509);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 13);
            this.label15.TabIndex = 108;
            this.label15.Text = "to:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(172, 485);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 109;
            this.label17.Text = "from:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 538);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(122, 13);
            this.label18.TabIndex = 111;
            this.label18.Text = "Min delay for set maxDD";
            // 
            // tbMinDelayForMaxDD
            // 
            this.tbMinDelayForMaxDD.Location = new System.Drawing.Point(137, 534);
            this.tbMinDelayForMaxDD.Name = "tbMinDelayForMaxDD";
            this.tbMinDelayForMaxDD.Size = new System.Drawing.Size(45, 20);
            this.tbMinDelayForMaxDD.TabIndex = 110;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(183, 538);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(66, 13);
            this.label19.TabIndex = 113;
            this.label19.Text = "DD attempts";
            // 
            // tbDDattempts
            // 
            this.tbDDattempts.Location = new System.Drawing.Point(250, 534);
            this.tbDDattempts.Name = "tbDDattempts";
            this.tbDDattempts.Size = new System.Drawing.Size(45, 20);
            this.tbDDattempts.TabIndex = 112;
            // 
            // cbUseImportanceDealForVolumeSet
            // 
            this.cbUseImportanceDealForVolumeSet.AutoSize = true;
            this.cbUseImportanceDealForVolumeSet.Location = new System.Drawing.Point(556, 451);
            this.cbUseImportanceDealForVolumeSet.Name = "cbUseImportanceDealForVolumeSet";
            this.cbUseImportanceDealForVolumeSet.Size = new System.Drawing.Size(138, 17);
            this.cbUseImportanceDealForVolumeSet.TabIndex = 114;
            this.cbUseImportanceDealForVolumeSet.Text = "Apply importance deals ";
            this.cbUseImportanceDealForVolumeSet.UseVisualStyleBackColor = true;
            // 
            // comboOpenCLDevice
            // 
            this.comboOpenCLDevice.FormattingEnabled = true;
            this.comboOpenCLDevice.Location = new System.Drawing.Point(168, 347);
            this.comboOpenCLDevice.Name = "comboOpenCLDevice";
            this.comboOpenCLDevice.Size = new System.Drawing.Size(209, 21);
            this.comboOpenCLDevice.TabIndex = 115;
            this.comboOpenCLDevice.SelectedValueChanged += new System.EventHandler(this.comboOpenCLDevice_SelectedValueChanged);
            // 
            // cbUseOpenCL
            // 
            this.cbUseOpenCL.AutoSize = true;
            this.cbUseOpenCL.Location = new System.Drawing.Point(28, 350);
            this.cbUseOpenCL.Name = "cbUseOpenCL";
            this.cbUseOpenCL.Size = new System.Drawing.Size(125, 17);
            this.cbUseOpenCL.TabIndex = 116;
            this.cbUseOpenCL.Text = "Use OpenCL device:";
            this.cbUseOpenCL.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(11, 562);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(182, 13);
            this.label20.TabIndex = 116;
            this.label20.Text = "Sigma count for DD delay calculating";
            // 
            // tbSigmaCountForDDdelay
            // 
            this.tbSigmaCountForDDdelay.Location = new System.Drawing.Point(199, 559);
            this.tbSigmaCountForDDdelay.Name = "tbSigmaCountForDDdelay";
            this.tbSigmaCountForDDdelay.Size = new System.Drawing.Size(45, 20);
            this.tbSigmaCountForDDdelay.TabIndex = 115;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(483, 509);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(113, 13);
            this.label21.TabIndex = 118;
            this.label21.Text = "Importance deal count";
            // 
            // tbImportanceDealCount
            // 
            this.tbImportanceDealCount.Location = new System.Drawing.Point(599, 504);
            this.tbImportanceDealCount.Name = "tbImportanceDealCount";
            this.tbImportanceDealCount.Size = new System.Drawing.Size(45, 20);
            this.tbImportanceDealCount.TabIndex = 117;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(468, 559);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(174, 13);
            this.label22.TabIndex = 120;
            this.label22.Text = "\"Zero\" days count for decrease Vol";
            // 
            // tbZeroDaysForDecrVol
            // 
            this.tbZeroDaysForDecrVol.Location = new System.Drawing.Point(649, 555);
            this.tbZeroDaysForDecrVol.Name = "tbZeroDaysForDecrVol";
            this.tbZeroDaysForDecrVol.Size = new System.Drawing.Size(45, 20);
            this.tbZeroDaysForDecrVol.TabIndex = 119;
            // 
            // cbNetPointsProfitDay
            // 
            this.cbNetPointsProfitDay.AutoSize = true;
            this.cbNetPointsProfitDay.Location = new System.Drawing.Point(248, 575);
            this.cbNetPointsProfitDay.Name = "cbNetPointsProfitDay";
            this.cbNetPointsProfitDay.Size = new System.Drawing.Size(269, 17);
            this.cbNetPointsProfitDay.TabIndex = 121;
            this.cbNetPointsProfitDay.Text = "Use net points like a positive days definition method";
            this.cbNetPointsProfitDay.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 600);
            this.Controls.Add(this.cbNetPointsProfitDay);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.tbZeroDaysForDecrVol);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.tbImportanceDealCount);
            this.Controls.Add(this.cbUseOpenCL);
            this.Controls.Add(this.comboOpenCLDevice);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.tbSigmaCountForDDdelay);
            this.Controls.Add(this.cbUseImportanceDealForVolumeSet);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.tbDDattempts);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.tbMinDelayForMaxDD);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btSetMaxDD);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbImportanceDealPercent);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dtpVolumeSetLastDate);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbDealsForVolChange322);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbDaysForVolChange322);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbMinPE);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbMaxDealVolume322);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbMinDealVolume322);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dtpVolumeSetFirstDate);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbLambda);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbEpsilon);
            this.Controls.Add(this.btSetVolumes);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbMaxDealVolume);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbDaysForSetVolumes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDatePriority);
            this.Controls.Add(this.nudMaxConcurrency);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbProjectedTotalTimeValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbUseOpslipInFF);
            this.Controls.Add(this.cbAllFilters);
            this.Controls.Add(this.tbPercentOfCheckLog);
            this.Controls.Add(this.tbPercentOfOptLog);
            this.Controls.Add(this.cbProfitLimit);
            this.Controls.Add(this.cbUsedBTlog);
            this.Controls.Add(this.tbMinNumberOfTradesAfterPreopting);
            this.Controls.Add(this.labelPercentArbiTradeForAdvancePre_opting);
            this.Controls.Add(this.tbPercentArbiTradeForAdvancePre_opting);
            this.Controls.Add(this.cbTurnOffMutation);
            this.Controls.Add(this.cbUseTradeLogDateTimeFilter);
            this.Controls.Add(this.lbTradeLogFilterEndDateTime);
            this.Controls.Add(this.lbTradeLogFilterStartDateTime);
            this.Controls.Add(this.dtpTradeLogFilterEndDateTime);
            this.Controls.Add(this.dtpTradeLogFilterStartDateTime);
            this.Controls.Add(this.lbEstimatedTimeValue);
            this.Controls.Add(this.lbEstimatedTime);
            this.Controls.Add(this.lbElapsedTimeValue);
            this.Controls.Add(this.lbElapsedTime);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnXMLParsing);
            this.Controls.Add(this.btnOptimization);
            this.Controls.Add(this.labelCoefOfStochastickSelect);
            this.Controls.Add(this.labelCountOfGenerationsWithTheBestInd);
            this.Controls.Add(this.labelNumberOfTheBestIndividuals);
            this.Controls.Add(this.tbCoefOfStochastickSelect);
            this.Controls.Add(this.tbCountOfGenerationsWithTheBestInd);
            this.Controls.Add(this.tbNumberOfTheBestIndividuals);
            this.Controls.Add(this.labelMutationCoef);
            this.Controls.Add(this.labelMinNumberOfTrades);
            this.Controls.Add(this.labelCountOfGenerations);
            this.Controls.Add(this.labelVariableNumberOfSlots);
            this.Controls.Add(this.labelFrequencyOfOccurrenceNullIndividual);
            this.Controls.Add(this.labelSelectionPower);
            this.Controls.Add(this.cbCurrentSettingsFile);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.tbFrequencyOfOccurrenceNullIndividual);
            this.Controls.Add(this.tbVariableNumberOfSlots);
            this.Controls.Add(this.tbMutationCoef4);
            this.Controls.Add(this.tbMutationCoef3);
            this.Controls.Add(this.tbMutationCoef2);
            this.Controls.Add(this.tbMutationCoef1);
            this.Controls.Add(this.tbMinNumberOfTrades);
            this.Controls.Add(this.tbCountOfGenerations);
            this.Controls.Add(this.tbSelectionPower2);
            this.Controls.Add(this.tbSelectionPower1);
            this.Controls.Add(this.labelPopulation);
            this.Controls.Add(this.tbPopulation);
            this.Name = "MainForm";
            this.Text = "Genetic algo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxConcurrency)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPopulation;
        private System.Windows.Forms.Label labelPopulation;
        private System.Windows.Forms.TextBox tbSelectionPower1;
        private System.Windows.Forms.TextBox tbSelectionPower2;
        private System.Windows.Forms.TextBox tbCountOfGenerations;
        private System.Windows.Forms.TextBox tbMinNumberOfTrades;
        private System.Windows.Forms.TextBox tbMutationCoef1;
        private System.Windows.Forms.TextBox tbMutationCoef2;
        private System.Windows.Forms.TextBox tbMutationCoef3;
        private System.Windows.Forms.TextBox tbMutationCoef4;
        private System.Windows.Forms.TextBox tbVariableNumberOfSlots;
        private System.Windows.Forms.TextBox tbFrequencyOfOccurrenceNullIndividual;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox cbCurrentSettingsFile;
        private System.Windows.Forms.Label labelSelectionPower;
        private System.Windows.Forms.Label labelFrequencyOfOccurrenceNullIndividual;
        private System.Windows.Forms.Label labelVariableNumberOfSlots;
        private System.Windows.Forms.Label labelCountOfGenerations;
        private System.Windows.Forms.Label labelMinNumberOfTrades;
        private System.Windows.Forms.Label labelMutationCoef;
        private System.Windows.Forms.TextBox tbNumberOfTheBestIndividuals;
        private System.Windows.Forms.TextBox tbCountOfGenerationsWithTheBestInd;
        private System.Windows.Forms.TextBox tbCoefOfStochastickSelect;
        private System.Windows.Forms.Label labelNumberOfTheBestIndividuals;
        private System.Windows.Forms.Label labelCountOfGenerationsWithTheBestInd;
        private System.Windows.Forms.Label labelCoefOfStochastickSelect;
        private System.Windows.Forms.Button btnOptimization;
        private System.Windows.Forms.Button btnXMLParsing;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        internal System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lbElapsedTime;
        private System.Windows.Forms.Label lbElapsedTimeValue;
        internal System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lbEstimatedTime;
        internal System.Windows.Forms.Label lbEstimatedTimeValue;
        private System.Windows.Forms.DateTimePicker dtpTradeLogFilterStartDateTime;
        private System.Windows.Forms.DateTimePicker dtpTradeLogFilterEndDateTime;
        private System.Windows.Forms.Label lbTradeLogFilterStartDateTime;
        private System.Windows.Forms.Label lbTradeLogFilterEndDateTime;
        private System.Windows.Forms.CheckBox cbUseTradeLogDateTimeFilter;
        private System.Windows.Forms.CheckBox cbTurnOffMutation;
        private System.Windows.Forms.Label labelPercentArbiTradeForAdvancePre_opting;
        private System.Windows.Forms.TextBox tbPercentArbiTradeForAdvancePre_opting;
        private System.Windows.Forms.TextBox tbMinNumberOfTradesAfterPreopting;
        private System.Windows.Forms.CheckBox cbUsedBTlog;
        private System.Windows.Forms.CheckBox cbProfitLimit;
        private System.Windows.Forms.TextBox tbPercentOfOptLog;
        private System.Windows.Forms.TextBox tbPercentOfCheckLog;
        private System.Windows.Forms.CheckBox cbAllFilters;
        private System.Windows.Forms.CheckBox cbUseOpslipInFF;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lbProjectedTotalTimeValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMaxConcurrency;
        private System.Windows.Forms.TextBox tbDatePriority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDaysForSetVolumes;
        private System.Windows.Forms.TextBox tbMaxDealVolume;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btSetVolumes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbEpsilon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbLambda;
        private System.Windows.Forms.DateTimePicker dtpVolumeSetFirstDate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb322;
        private System.Windows.Forms.RadioButton rbPortfolio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbMinDealVolume322;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbMaxDealVolume322;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbMinPE;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbDaysForVolChange322;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbDealsForVolChange322;
        private System.Windows.Forms.DateTimePicker dtpVolumeSetLastDate;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbImportanceDealPercent;
        private System.Windows.Forms.Button btSetMaxDD;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox tbMinDelayForMaxDD;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbDDattempts;
        private System.Windows.Forms.CheckBox cbUseImportanceDealForVolumeSet;
        private System.Windows.Forms.ComboBox comboOpenCLDevice;
        private System.Windows.Forms.CheckBox cbUseOpenCL;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbSigmaCountForDDdelay;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbImportanceDealCount;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tbZeroDaysForDecrVol;
        private System.Windows.Forms.CheckBox cbNetPointsProfitDay;
    }
}

