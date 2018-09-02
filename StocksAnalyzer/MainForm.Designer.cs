namespace StocksAnalyzer
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelRemainingTime = new System.Windows.Forms.Label();
            this.buttonGetInfo = new System.Windows.Forms.Button();
            this.labelSelStock = new System.Windows.Forms.Label();
            this.comboBoxStocks = new System.Windows.Forms.ComboBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.radioButtonStarred = new System.Windows.Forms.RadioButton();
            this.radioButtonAllStocks = new System.Windows.Forms.RadioButton();
            this.radioButtonRusStocks = new System.Windows.Forms.RadioButton();
            this.radioButtonUSAStocks = new System.Windows.Forms.RadioButton();
            this.radioButtonLondonStocks = new System.Windows.Forms.RadioButton();
            this.buttonLoadAllStocks = new System.Windows.Forms.Button();
            this.buttonLoadStocksMultiplicators = new System.Windows.Forms.Button();
            this.labelStockCount = new System.Windows.Forms.Label();
            this.labelStockName = new System.Windows.Forms.Label();
            this.textBoxStockSymbol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelLastUpdateTime = new System.Windows.Forms.Label();
            this.textBoxStockLastUpdated = new System.Windows.Forms.TextBox();
            this.checkBoxIsStarred = new System.Windows.Forms.CheckBox();
            this.labelPriceRub = new System.Windows.Forms.Label();
            this.textBoxStockPrice = new System.Windows.Forms.TextBox();
            this.labelPE = new System.Windows.Forms.Label();
            this.textBoxPE = new System.Windows.Forms.TextBox();
            this.labelPS = new System.Windows.Forms.Label();
            this.textBoxPS = new System.Windows.Forms.TextBox();
            this.labelPBV = new System.Windows.Forms.Label();
            this.textBoxPBV = new System.Windows.Forms.TextBox();
            this.labelEVEBITDA = new System.Windows.Forms.Label();
            this.textBoxEVEBITDA = new System.Windows.Forms.TextBox();
            this.labelDebtEBITDA = new System.Windows.Forms.Label();
            this.textBoxDebtEBITDA = new System.Windows.Forms.TextBox();
            this.labelROE = new System.Windows.Forms.Label();
            this.textBoxROE = new System.Windows.Forms.TextBox();
            this.labelEPS = new System.Windows.Forms.Label();
            this.textBoxEPS = new System.Windows.Forms.TextBox();
            this.labelDone = new System.Windows.Forms.Label();
            this.labelPriceUSD = new System.Windows.Forms.Label();
            this.textBoxStockPriceUSD = new System.Windows.Forms.TextBox();
            this.buttonSaveHistory = new System.Windows.Forms.Button();
            this.buttonLoadHistoryFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelUSACoefs = new System.Windows.Forms.Panel();
            this.labelSharesCount = new System.Windows.Forms.Label();
            this.textBoxSharesCount = new System.Windows.Forms.TextBox();
            this.labelLeveredFreeCF = new System.Windows.Forms.Label();
            this.textBoxLeveredFreeCF = new System.Windows.Forms.TextBox();
            this.labelOperCashFlow = new System.Windows.Forms.Label();
            this.textBoxOperCashFlow = new System.Windows.Forms.TextBox();
            this.labelBookValPShape = new System.Windows.Forms.Label();
            this.textBoxBookValPShape = new System.Windows.Forms.TextBox();
            this.labelTotDebt = new System.Windows.Forms.Label();
            this.textBoxTotDebt = new System.Windows.Forms.TextBox();
            this.labelTotCashPShape = new System.Windows.Forms.Label();
            this.textBoxTotCashPShape = new System.Windows.Forms.TextBox();
            this.labelTotCash = new System.Windows.Forms.Label();
            this.textBoxTotCash = new System.Windows.Forms.TextBox();
            this.labelEBITDA = new System.Windows.Forms.Label();
            this.textBoxEBITDA = new System.Windows.Forms.TextBox();
            this.labelRevenuePerShape = new System.Windows.Forms.Label();
            this.textBoxRevPerShape = new System.Windows.Forms.TextBox();
            this.labelRevenue = new System.Windows.Forms.Label();
            this.textBoxRevenue = new System.Windows.Forms.TextBox();
            this.labelRetOnAssests = new System.Windows.Forms.Label();
            this.textBoxRetOnAssets = new System.Windows.Forms.TextBox();
            this.labelEVRev = new System.Windows.Forms.Label();
            this.textBoxEVRev = new System.Windows.Forms.TextBox();
            this.labelUSAPanel = new System.Windows.Forms.Label();
            this.labelPEG = new System.Windows.Forms.Label();
            this.textBoxPEG = new System.Windows.Forms.TextBox();
            this.labelEntVal = new System.Windows.Forms.Label();
            this.textBoxEntVal = new System.Windows.Forms.TextBox();
            this.labelMarketCap = new System.Windows.Forms.Label();
            this.textBoxMarketCap = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelGrossPerc = new System.Windows.Forms.Label();
            this.labelSymbol = new System.Windows.Forms.Label();
            this.labelCommonCoefs = new System.Windows.Forms.Label();
            this.labelGrossProfit = new System.Windows.Forms.Label();
            this.textBoxGrossProfit = new System.Windows.Forms.TextBox();
            this.labelOperMargin = new System.Windows.Forms.Label();
            this.textBoxOperatingMargin = new System.Windows.Forms.TextBox();
            this.labelProfitMargin = new System.Windows.Forms.Label();
            this.textBoxProfitMargin = new System.Windows.Forms.TextBox();
            this.labelQEG = new System.Windows.Forms.Label();
            this.textBoxQEG = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panelRussiaCoefs = new System.Windows.Forms.Panel();
            this.labelCurrLiqudity = new System.Windows.Forms.Label();
            this.textBoxCurrLiquidity = new System.Windows.Forms.TextBox();
            this.labelQuickLiquidity = new System.Windows.Forms.Label();
            this.textBoxQuickLiquidity = new System.Windows.Forms.TextBox();
            this.labelOperMargin5Y = new System.Windows.Forms.Label();
            this.textBoxOperMargin5Y = new System.Windows.Forms.TextBox();
            this.labelProfitMargin5Y = new System.Windows.Forms.Label();
            this.textBoxProfitMargin5Y = new System.Windows.Forms.TextBox();
            this.labelGrowthCapCosts = new System.Windows.Forms.Label();
            this.textBoxGrowthCapCosts = new System.Windows.Forms.TextBox();
            this.labelGrowthPS5Y = new System.Windows.Forms.Label();
            this.textBoxGrowthPS5Y = new System.Windows.Forms.TextBox();
            this.labelProfitPerShare = new System.Windows.Forms.Label();
            this.textBoxProfitPerShare = new System.Windows.Forms.TextBox();
            this.label5YProfitCoef = new System.Windows.Forms.Label();
            this.textBox5YProfitCoef = new System.Windows.Forms.TextBox();
            this.labelProfitCoef = new System.Windows.Forms.Label();
            this.textBoxProfitCoef = new System.Windows.Forms.TextBox();
            this.label5YValProfit = new System.Windows.Forms.Label();
            this.textBox5YValProfit = new System.Windows.Forms.TextBox();
            this.labelRussiaPanel = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonOpenReport = new System.Windows.Forms.Button();
            this.buttonAnalyzeMultiplicators = new System.Windows.Forms.Button();
            this.labelMainPE = new System.Windows.Forms.Label();
            this.labelMain = new System.Windows.Forms.Label();
            this.labelMainAll = new System.Windows.Forms.Label();
            this.labelSortingMode = new System.Windows.Forms.Label();
            this.radioButtonMainPE = new System.Windows.Forms.RadioButton();
            this.radioButtonMain = new System.Windows.Forms.RadioButton();
            this.radioButtonMainAll = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.radioButtonFromTinkoff = new System.Windows.Forms.RadioButton();
            this.buttonGetTinkoffStocks = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.buttonCkechTinkoff = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.panelUSACoefs.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelRussiaCoefs.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelRemainingTime
            // 
            this.labelRemainingTime.AutoSize = true;
            this.labelRemainingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRemainingTime.Location = new System.Drawing.Point(13, 337);
            this.labelRemainingTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRemainingTime.Name = "labelRemainingTime";
            this.labelRemainingTime.Size = new System.Drawing.Size(273, 20);
            this.labelRemainingTime.TabIndex = 1;
            this.labelRemainingTime.Text = "Расчетное оставшееся время: ";
            // 
            // buttonGetInfo
            // 
            this.buttonGetInfo.Location = new System.Drawing.Point(317, 39);
            this.buttonGetInfo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGetInfo.Name = "buttonGetInfo";
            this.buttonGetInfo.Size = new System.Drawing.Size(123, 28);
            this.buttonGetInfo.TabIndex = 2;
            this.buttonGetInfo.Text = "Получить инфо";
            this.buttonGetInfo.UseVisualStyleBackColor = true;
            this.buttonGetInfo.Click += new System.EventHandler(this.ButtonGetInfo_Click);
            // 
            // labelSelStock
            // 
            this.labelSelStock.AutoSize = true;
            this.labelSelStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSelStock.Location = new System.Drawing.Point(16, 18);
            this.labelSelStock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSelStock.Name = "labelSelStock";
            this.labelSelStock.Size = new System.Drawing.Size(124, 18);
            this.labelSelStock.TabIndex = 4;
            this.labelSelStock.Text = "Выберите акцию";
            // 
            // comboBoxStocks
            // 
            this.comboBoxStocks.FormattingEnabled = true;
            this.comboBoxStocks.Location = new System.Drawing.Point(20, 41);
            this.comboBoxStocks.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxStocks.Name = "comboBoxStocks";
            this.comboBoxStocks.Size = new System.Drawing.Size(287, 24);
            this.comboBoxStocks.TabIndex = 3;
            this.comboBoxStocks.DropDown += new System.EventHandler(this.ComboBoxStocks_DropDown);
            this.comboBoxStocks.SelectedIndexChanged += new System.EventHandler(this.ComboBoxStocks_SelectedIndexChanged);
            this.comboBoxStocks.TextChanged += new System.EventHandler(this.ComboBoxStocks_TextChanged);
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(20, 401);
            this.richTextBoxLog.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(505, 548);
            this.richTextBoxLog.TabIndex = 5;
            this.richTextBoxLog.Text = "";
            // 
            // radioButtonStarred
            // 
            this.radioButtonStarred.AutoSize = true;
            this.radioButtonStarred.Location = new System.Drawing.Point(20, 87);
            this.radioButtonStarred.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonStarred.Name = "radioButtonStarred";
            this.radioButtonStarred.Size = new System.Drawing.Size(104, 21);
            this.radioButtonStarred.TabIndex = 6;
            this.radioButtonStarred.Text = "Избранные";
            this.radioButtonStarred.UseVisualStyleBackColor = true;
            this.radioButtonStarred.CheckedChanged += new System.EventHandler(this.RadioButtonStarred_CheckedChanged);
            // 
            // radioButtonAllStocks
            // 
            this.radioButtonAllStocks.AutoSize = true;
            this.radioButtonAllStocks.Location = new System.Drawing.Point(20, 129);
            this.radioButtonAllStocks.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonAllStocks.Name = "radioButtonAllStocks";
            this.radioButtonAllStocks.Size = new System.Drawing.Size(53, 21);
            this.radioButtonAllStocks.TabIndex = 7;
            this.radioButtonAllStocks.Text = "Все";
            this.radioButtonAllStocks.UseVisualStyleBackColor = true;
            this.radioButtonAllStocks.CheckedChanged += new System.EventHandler(this.RadioButtonAllStocks_CheckedChanged);
            // 
            // radioButtonRusStocks
            // 
            this.radioButtonRusStocks.AutoSize = true;
            this.radioButtonRusStocks.Location = new System.Drawing.Point(20, 150);
            this.radioButtonRusStocks.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonRusStocks.Name = "radioButtonRusStocks";
            this.radioButtonRusStocks.Size = new System.Drawing.Size(82, 21);
            this.radioButtonRusStocks.TabIndex = 8;
            this.radioButtonRusStocks.Text = "Русские";
            this.radioButtonRusStocks.UseVisualStyleBackColor = true;
            this.radioButtonRusStocks.CheckedChanged += new System.EventHandler(this.RadioButtonRus_CheckedChanged);
            // 
            // radioButtonUSAStocks
            // 
            this.radioButtonUSAStocks.AutoSize = true;
            this.radioButtonUSAStocks.Location = new System.Drawing.Point(20, 172);
            this.radioButtonUSAStocks.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonUSAStocks.Name = "radioButtonUSAStocks";
            this.radioButtonUSAStocks.Size = new System.Drawing.Size(57, 21);
            this.radioButtonUSAStocks.TabIndex = 9;
            this.radioButtonUSAStocks.Text = "USA";
            this.radioButtonUSAStocks.UseVisualStyleBackColor = true;
            this.radioButtonUSAStocks.CheckedChanged += new System.EventHandler(this.RadioButtonUSA_CheckedChanged);
            // 
            // radioButtonLondonStocks
            // 
            this.radioButtonLondonStocks.AutoSize = true;
            this.radioButtonLondonStocks.Enabled = false;
            this.radioButtonLondonStocks.Location = new System.Drawing.Point(20, 193);
            this.radioButtonLondonStocks.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonLondonStocks.Name = "radioButtonLondonStocks";
            this.radioButtonLondonStocks.Size = new System.Drawing.Size(77, 21);
            this.radioButtonLondonStocks.TabIndex = 10;
            this.radioButtonLondonStocks.Text = "London";
            this.radioButtonLondonStocks.UseVisualStyleBackColor = true;
            this.radioButtonLondonStocks.CheckedChanged += new System.EventHandler(this.RadioButtonLondon_CheckedChanged);
            // 
            // buttonLoadAllStocks
            // 
            this.buttonLoadAllStocks.Location = new System.Drawing.Point(20, 235);
            this.buttonLoadAllStocks.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLoadAllStocks.Name = "buttonLoadAllStocks";
            this.buttonLoadAllStocks.Size = new System.Drawing.Size(212, 28);
            this.buttonLoadAllStocks.TabIndex = 11;
            this.buttonLoadAllStocks.Text = "Загрузить список акций";
            this.buttonLoadAllStocks.UseVisualStyleBackColor = true;
            this.buttonLoadAllStocks.Click += new System.EventHandler(this.ButtonLoadAllStocksClick);
            // 
            // buttonLoadStocksMultiplicators
            // 
            this.buttonLoadStocksMultiplicators.Location = new System.Drawing.Point(20, 271);
            this.buttonLoadStocksMultiplicators.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLoadStocksMultiplicators.Name = "buttonLoadStocksMultiplicators";
            this.buttonLoadStocksMultiplicators.Size = new System.Drawing.Size(212, 28);
            this.buttonLoadStocksMultiplicators.TabIndex = 12;
            this.buttonLoadStocksMultiplicators.Text = "Загрузить инфо для списка";
            this.buttonLoadStocksMultiplicators.UseVisualStyleBackColor = true;
            this.buttonLoadStocksMultiplicators.Click += new System.EventHandler(this.ButtonLoadStockMultiplicators_Click);
            // 
            // labelStockCount
            // 
            this.labelStockCount.AutoSize = true;
            this.labelStockCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStockCount.Location = new System.Drawing.Point(189, 89);
            this.labelStockCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStockCount.Name = "labelStockCount";
            this.labelStockCount.Size = new System.Drawing.Size(103, 19);
            this.labelStockCount.TabIndex = 14;
            this.labelStockCount.Text = "Общее кол-во";
            // 
            // labelStockName
            // 
            this.labelStockName.AutoSize = true;
            this.labelStockName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStockName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelStockName.Location = new System.Drawing.Point(136, 39);
            this.labelStockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStockName.Name = "labelStockName";
            this.labelStockName.Size = new System.Drawing.Size(61, 22);
            this.labelStockName.TabIndex = 15;
            this.labelStockName.Text = "label2";
            // 
            // textBoxStockSymbol
            // 
            this.textBoxStockSymbol.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textBoxStockSymbol.Location = new System.Drawing.Point(255, 13);
            this.textBoxStockSymbol.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxStockSymbol.Name = "textBoxStockSymbol";
            this.textBoxStockSymbol.Size = new System.Drawing.Size(89, 22);
            this.textBoxStockSymbol.TabIndex = 16;
            this.textBoxStockSymbol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1151, 532);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 17);
            this.label2.TabIndex = 17;
            this.label2.Text = "Symbol";
            this.label2.Visible = false;
            // 
            // labelLastUpdateTime
            // 
            this.labelLastUpdateTime.AutoSize = true;
            this.labelLastUpdateTime.Location = new System.Drawing.Point(548, 37);
            this.labelLastUpdateTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLastUpdateTime.Name = "labelLastUpdateTime";
            this.labelLastUpdateTime.Size = new System.Drawing.Size(91, 17);
            this.labelLastUpdateTime.TabIndex = 19;
            this.labelLastUpdateTime.Text = "Last updated";
            // 
            // textBoxStockLastUpdated
            // 
            this.textBoxStockLastUpdated.Location = new System.Drawing.Point(648, 33);
            this.textBoxStockLastUpdated.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxStockLastUpdated.Name = "textBoxStockLastUpdated";
            this.textBoxStockLastUpdated.Size = new System.Drawing.Size(159, 22);
            this.textBoxStockLastUpdated.TabIndex = 18;
            // 
            // checkBoxIsStarred
            // 
            this.checkBoxIsStarred.AutoSize = true;
            this.checkBoxIsStarred.Enabled = false;
            this.checkBoxIsStarred.Location = new System.Drawing.Point(37, 41);
            this.checkBoxIsStarred.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxIsStarred.Name = "checkBoxIsStarred";
            this.checkBoxIsStarred.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxIsStarred.Size = new System.Drawing.Size(77, 21);
            this.checkBoxIsStarred.TabIndex = 20;
            this.checkBoxIsStarred.Text = "Starred";
            this.checkBoxIsStarred.UseVisualStyleBackColor = true;
            this.checkBoxIsStarred.CheckedChanged += new System.EventHandler(this.CheckBoxIsStarred_CheckedChanged);
            // 
            // labelPriceRub
            // 
            this.labelPriceRub.AutoSize = true;
            this.labelPriceRub.Location = new System.Drawing.Point(152, 46);
            this.labelPriceRub.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPriceRub.Name = "labelPriceRub";
            this.labelPriceRub.Size = new System.Drawing.Size(75, 17);
            this.labelPriceRub.TabIndex = 22;
            this.labelPriceRub.Text = "Price (rub)";
            // 
            // textBoxStockPrice
            // 
            this.textBoxStockPrice.Location = new System.Drawing.Point(144, 70);
            this.textBoxStockPrice.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxStockPrice.Name = "textBoxStockPrice";
            this.textBoxStockPrice.Size = new System.Drawing.Size(89, 22);
            this.textBoxStockPrice.TabIndex = 21;
            this.textBoxStockPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelPE
            // 
            this.labelPE.AutoSize = true;
            this.labelPE.Location = new System.Drawing.Point(107, 111);
            this.labelPE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPE.Name = "labelPE";
            this.labelPE.Size = new System.Drawing.Size(30, 17);
            this.labelPE.TabIndex = 24;
            this.labelPE.Text = "P/E";
            // 
            // textBoxPE
            // 
            this.textBoxPE.Location = new System.Drawing.Point(163, 107);
            this.textBoxPE.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPE.Name = "textBoxPE";
            this.textBoxPE.Size = new System.Drawing.Size(89, 22);
            this.textBoxPE.TabIndex = 23;
            // 
            // labelPS
            // 
            this.labelPS.AutoSize = true;
            this.labelPS.Location = new System.Drawing.Point(108, 146);
            this.labelPS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPS.Name = "labelPS";
            this.labelPS.Size = new System.Drawing.Size(30, 17);
            this.labelPS.TabIndex = 26;
            this.labelPS.Text = "P/S";
            // 
            // textBoxPS
            // 
            this.textBoxPS.Location = new System.Drawing.Point(163, 143);
            this.textBoxPS.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPS.Name = "textBoxPS";
            this.textBoxPS.Size = new System.Drawing.Size(89, 22);
            this.textBoxPS.TabIndex = 25;
            // 
            // labelPBV
            // 
            this.labelPBV.AutoSize = true;
            this.labelPBV.Location = new System.Drawing.Point(100, 182);
            this.labelPBV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPBV.Name = "labelPBV";
            this.labelPBV.Size = new System.Drawing.Size(39, 17);
            this.labelPBV.TabIndex = 28;
            this.labelPBV.Text = "P/BV";
            // 
            // textBoxPBV
            // 
            this.textBoxPBV.Location = new System.Drawing.Point(163, 178);
            this.textBoxPBV.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPBV.Name = "textBoxPBV";
            this.textBoxPBV.Size = new System.Drawing.Size(89, 22);
            this.textBoxPBV.TabIndex = 27;
            // 
            // labelEVEBITDA
            // 
            this.labelEVEBITDA.AutoSize = true;
            this.labelEVEBITDA.Location = new System.Drawing.Point(71, 12);
            this.labelEVEBITDA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEVEBITDA.Name = "labelEVEBITDA";
            this.labelEVEBITDA.Size = new System.Drawing.Size(79, 17);
            this.labelEVEBITDA.TabIndex = 30;
            this.labelEVEBITDA.Text = "EV/EBITDA";
            // 
            // textBoxEVEBITDA
            // 
            this.textBoxEVEBITDA.Location = new System.Drawing.Point(179, 9);
            this.textBoxEVEBITDA.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEVEBITDA.Name = "textBoxEVEBITDA";
            this.textBoxEVEBITDA.Size = new System.Drawing.Size(89, 22);
            this.textBoxEVEBITDA.TabIndex = 29;
            // 
            // labelDebtEBITDA
            // 
            this.labelDebtEBITDA.AutoSize = true;
            this.labelDebtEBITDA.Location = new System.Drawing.Point(59, 38);
            this.labelDebtEBITDA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDebtEBITDA.Name = "labelDebtEBITDA";
            this.labelDebtEBITDA.Size = new System.Drawing.Size(91, 17);
            this.labelDebtEBITDA.TabIndex = 32;
            this.labelDebtEBITDA.Text = "Debt/EBITDA";
            // 
            // textBoxDebtEBITDA
            // 
            this.textBoxDebtEBITDA.Location = new System.Drawing.Point(179, 34);
            this.textBoxDebtEBITDA.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxDebtEBITDA.Name = "textBoxDebtEBITDA";
            this.textBoxDebtEBITDA.Size = new System.Drawing.Size(89, 22);
            this.textBoxDebtEBITDA.TabIndex = 31;
            // 
            // labelROE
            // 
            this.labelROE.AutoSize = true;
            this.labelROE.Location = new System.Drawing.Point(84, 218);
            this.labelROE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelROE.Name = "labelROE";
            this.labelROE.Size = new System.Drawing.Size(58, 17);
            this.labelROE.TabIndex = 34;
            this.labelROE.Text = "ROE, %";
            // 
            // textBoxROE
            // 
            this.textBoxROE.Location = new System.Drawing.Point(163, 214);
            this.textBoxROE.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxROE.Name = "textBoxROE";
            this.textBoxROE.Size = new System.Drawing.Size(89, 22);
            this.textBoxROE.TabIndex = 33;
            // 
            // labelEPS
            // 
            this.labelEPS.AutoSize = true;
            this.labelEPS.Location = new System.Drawing.Point(104, 254);
            this.labelEPS.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEPS.Name = "labelEPS";
            this.labelEPS.Size = new System.Drawing.Size(35, 17);
            this.labelEPS.TabIndex = 36;
            this.labelEPS.Text = "EPS";
            // 
            // textBoxEPS
            // 
            this.textBoxEPS.Location = new System.Drawing.Point(163, 250);
            this.textBoxEPS.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEPS.Name = "textBoxEPS";
            this.textBoxEPS.Size = new System.Drawing.Size(89, 22);
            this.textBoxEPS.TabIndex = 35;
            // 
            // labelDone
            // 
            this.labelDone.AutoSize = true;
            this.labelDone.BackColor = System.Drawing.Color.Lime;
            this.labelDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.labelDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelDone.Location = new System.Drawing.Point(448, 41);
            this.labelDone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDone.Name = "labelDone";
            this.labelDone.Size = new System.Drawing.Size(71, 27);
            this.labelDone.TabIndex = 37;
            this.labelDone.Text = "Done.";
            this.labelDone.Visible = false;
            // 
            // labelPriceUSD
            // 
            this.labelPriceUSD.AutoSize = true;
            this.labelPriceUSD.Location = new System.Drawing.Point(263, 46);
            this.labelPriceUSD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPriceUSD.Name = "labelPriceUSD";
            this.labelPriceUSD.Size = new System.Drawing.Size(77, 17);
            this.labelPriceUSD.TabIndex = 39;
            this.labelPriceUSD.Text = "Price (usd)";
            // 
            // textBoxStockPriceUSD
            // 
            this.textBoxStockPriceUSD.Location = new System.Drawing.Point(255, 70);
            this.textBoxStockPriceUSD.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxStockPriceUSD.Name = "textBoxStockPriceUSD";
            this.textBoxStockPriceUSD.Size = new System.Drawing.Size(89, 22);
            this.textBoxStockPriceUSD.TabIndex = 38;
            this.textBoxStockPriceUSD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonSaveHistory
            // 
            this.buttonSaveHistory.Location = new System.Drawing.Point(189, 114);
            this.buttonSaveHistory.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSaveHistory.Name = "buttonSaveHistory";
            this.buttonSaveHistory.Size = new System.Drawing.Size(212, 28);
            this.buttonSaveHistory.TabIndex = 40;
            this.buttonSaveHistory.Text = "Сохранить в историю";
            this.buttonSaveHistory.UseVisualStyleBackColor = true;
            this.buttonSaveHistory.Click += new System.EventHandler(this.ButtonSaveHistory_Click);
            // 
            // buttonLoadHistoryFile
            // 
            this.buttonLoadHistoryFile.Location = new System.Drawing.Point(189, 150);
            this.buttonLoadHistoryFile.Margin = new System.Windows.Forms.Padding(4);
            this.buttonLoadHistoryFile.Name = "buttonLoadHistoryFile";
            this.buttonLoadHistoryFile.Size = new System.Drawing.Size(212, 28);
            this.buttonLoadHistoryFile.TabIndex = 41;
            this.buttonLoadHistoryFile.Text = "Загрузить файл истории";
            this.buttonLoadHistoryFile.UseVisualStyleBackColor = true;
            this.buttonLoadHistoryFile.Click += new System.EventHandler(this.ButtonLoadHistoryFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.OpenFileDialog1_FileOk);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.panelRussiaCoefs);
            this.panelMain.Controls.Add(this.panelUSACoefs);
            this.panelMain.Controls.Add(this.panel2);
            this.panelMain.Controls.Add(this.linkLabel1);
            this.panelMain.Controls.Add(this.checkBoxIsStarred);
            this.panelMain.Controls.Add(this.labelStockName);
            this.panelMain.Location = new System.Drawing.Point(551, 65);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1139, 884);
            this.panelMain.TabIndex = 42;
            // 
            // panelUSACoefs
            // 
            this.panelUSACoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUSACoefs.Controls.Add(this.labelSharesCount);
            this.panelUSACoefs.Controls.Add(this.textBoxSharesCount);
            this.panelUSACoefs.Controls.Add(this.labelLeveredFreeCF);
            this.panelUSACoefs.Controls.Add(this.textBoxLeveredFreeCF);
            this.panelUSACoefs.Controls.Add(this.labelOperCashFlow);
            this.panelUSACoefs.Controls.Add(this.textBoxOperCashFlow);
            this.panelUSACoefs.Controls.Add(this.labelBookValPShape);
            this.panelUSACoefs.Controls.Add(this.textBoxBookValPShape);
            this.panelUSACoefs.Controls.Add(this.labelTotDebt);
            this.panelUSACoefs.Controls.Add(this.textBoxTotDebt);
            this.panelUSACoefs.Controls.Add(this.labelTotCashPShape);
            this.panelUSACoefs.Controls.Add(this.textBoxTotCashPShape);
            this.panelUSACoefs.Controls.Add(this.labelTotCash);
            this.panelUSACoefs.Controls.Add(this.textBoxTotCash);
            this.panelUSACoefs.Controls.Add(this.labelEBITDA);
            this.panelUSACoefs.Controls.Add(this.textBoxEBITDA);
            this.panelUSACoefs.Controls.Add(this.labelRevenuePerShape);
            this.panelUSACoefs.Controls.Add(this.textBoxRevPerShape);
            this.panelUSACoefs.Controls.Add(this.labelRevenue);
            this.panelUSACoefs.Controls.Add(this.textBoxRevenue);
            this.panelUSACoefs.Controls.Add(this.labelRetOnAssests);
            this.panelUSACoefs.Controls.Add(this.textBoxRetOnAssets);
            this.panelUSACoefs.Controls.Add(this.labelEVRev);
            this.panelUSACoefs.Controls.Add(this.textBoxEVRev);
            this.panelUSACoefs.Controls.Add(this.labelUSAPanel);
            this.panelUSACoefs.Controls.Add(this.labelPEG);
            this.panelUSACoefs.Controls.Add(this.textBoxPEG);
            this.panelUSACoefs.Controls.Add(this.labelEntVal);
            this.panelUSACoefs.Controls.Add(this.textBoxEntVal);
            this.panelUSACoefs.Controls.Add(this.labelMarketCap);
            this.panelUSACoefs.Controls.Add(this.textBoxMarketCap);
            this.panelUSACoefs.Controls.Add(this.labelDebtEBITDA);
            this.panelUSACoefs.Controls.Add(this.textBoxDebtEBITDA);
            this.panelUSACoefs.Controls.Add(this.labelEVEBITDA);
            this.panelUSACoefs.Controls.Add(this.textBoxEVEBITDA);
            this.panelUSACoefs.Location = new System.Drawing.Point(619, 69);
            this.panelUSACoefs.Margin = new System.Windows.Forms.Padding(4);
            this.panelUSACoefs.Name = "panelUSACoefs";
            this.panelUSACoefs.Size = new System.Drawing.Size(394, 567);
            this.panelUSACoefs.TabIndex = 57;
            // 
            // labelSharesCount
            // 
            this.labelSharesCount.AutoSize = true;
            this.labelSharesCount.Location = new System.Drawing.Point(27, 426);
            this.labelSharesCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSharesCount.Name = "labelSharesCount";
            this.labelSharesCount.Size = new System.Drawing.Size(131, 17);
            this.labelSharesCount.TabIndex = 83;
            this.labelSharesCount.Text = "Shares outstanding";
            // 
            // textBoxSharesCount
            // 
            this.textBoxSharesCount.Location = new System.Drawing.Point(179, 422);
            this.textBoxSharesCount.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxSharesCount.Name = "textBoxSharesCount";
            this.textBoxSharesCount.Size = new System.Drawing.Size(89, 22);
            this.textBoxSharesCount.TabIndex = 82;
            // 
            // labelLeveredFreeCF
            // 
            this.labelLeveredFreeCF.AutoSize = true;
            this.labelLeveredFreeCF.Location = new System.Drawing.Point(4, 400);
            this.labelLeveredFreeCF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLeveredFreeCF.Name = "labelLeveredFreeCF";
            this.labelLeveredFreeCF.Size = new System.Drawing.Size(151, 17);
            this.labelLeveredFreeCF.TabIndex = 81;
            this.labelLeveredFreeCF.Text = "Levered free cash flow";
            // 
            // textBoxLeveredFreeCF
            // 
            this.textBoxLeveredFreeCF.Location = new System.Drawing.Point(179, 396);
            this.textBoxLeveredFreeCF.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxLeveredFreeCF.Name = "textBoxLeveredFreeCF";
            this.textBoxLeveredFreeCF.Size = new System.Drawing.Size(89, 22);
            this.textBoxLeveredFreeCF.TabIndex = 80;
            // 
            // labelOperCashFlow
            // 
            this.labelOperCashFlow.AutoSize = true;
            this.labelOperCashFlow.Location = new System.Drawing.Point(23, 374);
            this.labelOperCashFlow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOperCashFlow.Name = "labelOperCashFlow";
            this.labelOperCashFlow.Size = new System.Drawing.Size(133, 17);
            this.labelOperCashFlow.TabIndex = 79;
            this.labelOperCashFlow.Text = "Operating cash flow";
            // 
            // textBoxOperCashFlow
            // 
            this.textBoxOperCashFlow.Location = new System.Drawing.Point(179, 370);
            this.textBoxOperCashFlow.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOperCashFlow.Name = "textBoxOperCashFlow";
            this.textBoxOperCashFlow.Size = new System.Drawing.Size(89, 22);
            this.textBoxOperCashFlow.TabIndex = 78;
            // 
            // labelBookValPShape
            // 
            this.labelBookValPShape.AutoSize = true;
            this.labelBookValPShape.Location = new System.Drawing.Point(7, 348);
            this.labelBookValPShape.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBookValPShape.Name = "labelBookValPShape";
            this.labelBookValPShape.Size = new System.Drawing.Size(152, 17);
            this.labelBookValPShape.TabIndex = 77;
            this.labelBookValPShape.Text = "Book val p share (mrq)";
            // 
            // textBoxBookValPShape
            // 
            this.textBoxBookValPShape.Location = new System.Drawing.Point(179, 345);
            this.textBoxBookValPShape.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxBookValPShape.Name = "textBoxBookValPShape";
            this.textBoxBookValPShape.Size = new System.Drawing.Size(89, 22);
            this.textBoxBookValPShape.TabIndex = 76;
            // 
            // labelTotDebt
            // 
            this.labelTotDebt.AutoSize = true;
            this.labelTotDebt.Location = new System.Drawing.Point(49, 325);
            this.labelTotDebt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotDebt.Name = "labelTotDebt";
            this.labelTotDebt.Size = new System.Drawing.Size(110, 17);
            this.labelTotDebt.TabIndex = 75;
            this.labelTotDebt.Text = "Total debt (mrq)";
            // 
            // textBoxTotDebt
            // 
            this.textBoxTotDebt.Location = new System.Drawing.Point(179, 319);
            this.textBoxTotDebt.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTotDebt.Name = "textBoxTotDebt";
            this.textBoxTotDebt.Size = new System.Drawing.Size(89, 22);
            this.textBoxTotDebt.TabIndex = 74;
            // 
            // labelTotCashPShape
            // 
            this.labelTotCashPShape.AutoSize = true;
            this.labelTotCashPShape.Location = new System.Drawing.Point(5, 297);
            this.labelTotCashPShape.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotCashPShape.Name = "labelTotCashPShape";
            this.labelTotCashPShape.Size = new System.Drawing.Size(152, 17);
            this.labelTotCashPShape.TabIndex = 73;
            this.labelTotCashPShape.Text = "Total cash/share (mrq)";
            // 
            // textBoxTotCashPShape
            // 
            this.textBoxTotCashPShape.Location = new System.Drawing.Point(179, 293);
            this.textBoxTotCashPShape.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTotCashPShape.Name = "textBoxTotCashPShape";
            this.textBoxTotCashPShape.Size = new System.Drawing.Size(89, 22);
            this.textBoxTotCashPShape.TabIndex = 72;
            // 
            // labelTotCash
            // 
            this.labelTotCash.AutoSize = true;
            this.labelTotCash.Location = new System.Drawing.Point(47, 271);
            this.labelTotCash.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTotCash.Name = "labelTotCash";
            this.labelTotCash.Size = new System.Drawing.Size(112, 17);
            this.labelTotCash.TabIndex = 71;
            this.labelTotCash.Text = "Total cash (mrq)";
            // 
            // textBoxTotCash
            // 
            this.textBoxTotCash.Location = new System.Drawing.Point(179, 267);
            this.textBoxTotCash.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxTotCash.Name = "textBoxTotCash";
            this.textBoxTotCash.Size = new System.Drawing.Size(89, 22);
            this.textBoxTotCash.TabIndex = 70;
            // 
            // labelEBITDA
            // 
            this.labelEBITDA.AutoSize = true;
            this.labelEBITDA.Location = new System.Drawing.Point(96, 245);
            this.labelEBITDA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEBITDA.Name = "labelEBITDA";
            this.labelEBITDA.Size = new System.Drawing.Size(57, 17);
            this.labelEBITDA.TabIndex = 69;
            this.labelEBITDA.Text = "EBITDA";
            // 
            // textBoxEBITDA
            // 
            this.textBoxEBITDA.Location = new System.Drawing.Point(179, 241);
            this.textBoxEBITDA.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEBITDA.Name = "textBoxEBITDA";
            this.textBoxEBITDA.Size = new System.Drawing.Size(89, 22);
            this.textBoxEBITDA.TabIndex = 68;
            // 
            // labelRevenuePerShape
            // 
            this.labelRevenuePerShape.AutoSize = true;
            this.labelRevenuePerShape.Location = new System.Drawing.Point(27, 219);
            this.labelRevenuePerShape.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRevenuePerShape.Name = "labelRevenuePerShape";
            this.labelRevenuePerShape.Size = new System.Drawing.Size(130, 17);
            this.labelRevenuePerShape.TabIndex = 67;
            this.labelRevenuePerShape.Text = "Revenue per share";
            // 
            // textBoxRevPerShape
            // 
            this.textBoxRevPerShape.Location = new System.Drawing.Point(179, 215);
            this.textBoxRevPerShape.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRevPerShape.Name = "textBoxRevPerShape";
            this.textBoxRevPerShape.Size = new System.Drawing.Size(89, 22);
            this.textBoxRevPerShape.TabIndex = 66;
            // 
            // labelRevenue
            // 
            this.labelRevenue.AutoSize = true;
            this.labelRevenue.Location = new System.Drawing.Point(89, 193);
            this.labelRevenue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRevenue.Name = "labelRevenue";
            this.labelRevenue.Size = new System.Drawing.Size(65, 17);
            this.labelRevenue.TabIndex = 65;
            this.labelRevenue.Text = "Revenue";
            // 
            // textBoxRevenue
            // 
            this.textBoxRevenue.Location = new System.Drawing.Point(179, 190);
            this.textBoxRevenue.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRevenue.Name = "textBoxRevenue";
            this.textBoxRevenue.Size = new System.Drawing.Size(89, 22);
            this.textBoxRevenue.TabIndex = 64;
            // 
            // labelRetOnAssests
            // 
            this.labelRetOnAssests.AutoSize = true;
            this.labelRetOnAssests.Location = new System.Drawing.Point(23, 167);
            this.labelRetOnAssests.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRetOnAssests.Name = "labelRetOnAssests";
            this.labelRetOnAssests.Size = new System.Drawing.Size(136, 17);
            this.labelRetOnAssests.TabIndex = 63;
            this.labelRetOnAssests.Text = "Return on assets, %";
            // 
            // textBoxRetOnAssets
            // 
            this.textBoxRetOnAssets.Location = new System.Drawing.Point(179, 164);
            this.textBoxRetOnAssets.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxRetOnAssets.Name = "textBoxRetOnAssets";
            this.textBoxRetOnAssets.Size = new System.Drawing.Size(89, 22);
            this.textBoxRetOnAssets.TabIndex = 62;
            // 
            // labelEVRev
            // 
            this.labelEVRev.AutoSize = true;
            this.labelEVRev.Location = new System.Drawing.Point(96, 142);
            this.labelEVRev.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEVRev.Name = "labelEVRev";
            this.labelEVRev.Size = new System.Drawing.Size(55, 17);
            this.labelEVRev.TabIndex = 61;
            this.labelEVRev.Text = "EV/Rev";
            // 
            // textBoxEVRev
            // 
            this.textBoxEVRev.Location = new System.Drawing.Point(179, 138);
            this.textBoxEVRev.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEVRev.Name = "textBoxEVRev";
            this.textBoxEVRev.Size = new System.Drawing.Size(89, 22);
            this.textBoxEVRev.TabIndex = 60;
            // 
            // labelUSAPanel
            // 
            this.labelUSAPanel.AutoSize = true;
            this.labelUSAPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelUSAPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelUSAPanel.Location = new System.Drawing.Point(-1, -1);
            this.labelUSAPanel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelUSAPanel.Name = "labelUSAPanel";
            this.labelUSAPanel.Size = new System.Drawing.Size(48, 22);
            this.labelUSAPanel.TabIndex = 59;
            this.labelUSAPanel.Text = "USA";
            // 
            // labelPEG
            // 
            this.labelPEG.AutoSize = true;
            this.labelPEG.Location = new System.Drawing.Point(-4, 116);
            this.labelPEG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPEG.Name = "labelPEG";
            this.labelPEG.Size = new System.Drawing.Size(164, 17);
            this.labelPEG.TabIndex = 55;
            this.labelPEG.Text = "PEG ratio (5yr expected)";
            // 
            // textBoxPEG
            // 
            this.textBoxPEG.Location = new System.Drawing.Point(179, 112);
            this.textBoxPEG.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxPEG.Name = "textBoxPEG";
            this.textBoxPEG.Size = new System.Drawing.Size(89, 22);
            this.textBoxPEG.TabIndex = 54;
            // 
            // labelEntVal
            // 
            this.labelEntVal.AutoSize = true;
            this.labelEntVal.Location = new System.Drawing.Point(47, 90);
            this.labelEntVal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEntVal.Name = "labelEntVal";
            this.labelEntVal.Size = new System.Drawing.Size(111, 17);
            this.labelEntVal.TabIndex = 53;
            this.labelEntVal.Text = "Enterprise value";
            // 
            // textBoxEntVal
            // 
            this.textBoxEntVal.Location = new System.Drawing.Point(179, 86);
            this.textBoxEntVal.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxEntVal.Name = "textBoxEntVal";
            this.textBoxEntVal.Size = new System.Drawing.Size(89, 22);
            this.textBoxEntVal.TabIndex = 52;
            // 
            // labelMarketCap
            // 
            this.labelMarketCap.AutoSize = true;
            this.labelMarketCap.Location = new System.Drawing.Point(76, 64);
            this.labelMarketCap.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMarketCap.Name = "labelMarketCap";
            this.labelMarketCap.Size = new System.Drawing.Size(78, 17);
            this.labelMarketCap.TabIndex = 51;
            this.labelMarketCap.Text = "Market cap";
            // 
            // textBoxMarketCap
            // 
            this.textBoxMarketCap.Location = new System.Drawing.Point(179, 60);
            this.textBoxMarketCap.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxMarketCap.Name = "textBoxMarketCap";
            this.textBoxMarketCap.Size = new System.Drawing.Size(89, 22);
            this.textBoxMarketCap.TabIndex = 50;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.labelGrossPerc);
            this.panel2.Controls.Add(this.labelSymbol);
            this.panel2.Controls.Add(this.labelCommonCoefs);
            this.panel2.Controls.Add(this.labelGrossProfit);
            this.panel2.Controls.Add(this.textBoxGrossProfit);
            this.panel2.Controls.Add(this.labelOperMargin);
            this.panel2.Controls.Add(this.textBoxOperatingMargin);
            this.panel2.Controls.Add(this.labelProfitMargin);
            this.panel2.Controls.Add(this.textBoxProfitMargin);
            this.panel2.Controls.Add(this.labelQEG);
            this.panel2.Controls.Add(this.textBoxQEG);
            this.panel2.Controls.Add(this.labelPriceUSD);
            this.panel2.Controls.Add(this.textBoxStockPriceUSD);
            this.panel2.Controls.Add(this.labelEPS);
            this.panel2.Controls.Add(this.textBoxEPS);
            this.panel2.Controls.Add(this.labelROE);
            this.panel2.Controls.Add(this.textBoxROE);
            this.panel2.Controls.Add(this.labelPBV);
            this.panel2.Controls.Add(this.textBoxPBV);
            this.panel2.Controls.Add(this.labelPS);
            this.panel2.Controls.Add(this.textBoxPS);
            this.panel2.Controls.Add(this.labelPE);
            this.panel2.Controls.Add(this.textBoxPE);
            this.panel2.Controls.Add(this.labelPriceRub);
            this.panel2.Controls.Add(this.textBoxStockPrice);
            this.panel2.Controls.Add(this.textBoxStockSymbol);
            this.panel2.Location = new System.Drawing.Point(24, 69);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(449, 567);
            this.panel2.TabIndex = 56;
            // 
            // labelGrossPerc
            // 
            this.labelGrossPerc.AutoSize = true;
            this.labelGrossPerc.Location = new System.Drawing.Point(53, 412);
            this.labelGrossPerc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGrossPerc.Name = "labelGrossPerc";
            this.labelGrossPerc.Size = new System.Drawing.Size(95, 17);
            this.labelGrossPerc.TabIndex = 59;
            this.labelGrossPerc.Text = "(для рус в %)";
            // 
            // labelSymbol
            // 
            this.labelSymbol.AutoSize = true;
            this.labelSymbol.Location = new System.Drawing.Point(192, 16);
            this.labelSymbol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSymbol.Name = "labelSymbol";
            this.labelSymbol.Size = new System.Drawing.Size(54, 17);
            this.labelSymbol.TabIndex = 58;
            this.labelSymbol.Text = "Symbol";
            // 
            // labelCommonCoefs
            // 
            this.labelCommonCoefs.AutoSize = true;
            this.labelCommonCoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCommonCoefs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelCommonCoefs.Location = new System.Drawing.Point(-3, -1);
            this.labelCommonCoefs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCommonCoefs.Name = "labelCommonCoefs";
            this.labelCommonCoefs.Size = new System.Drawing.Size(73, 22);
            this.labelCommonCoefs.TabIndex = 57;
            this.labelCommonCoefs.Text = "Общие";
            // 
            // labelGrossProfit
            // 
            this.labelGrossProfit.AutoSize = true;
            this.labelGrossProfit.Location = new System.Drawing.Point(61, 396);
            this.labelGrossProfit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGrossProfit.Name = "labelGrossProfit";
            this.labelGrossProfit.Size = new System.Drawing.Size(82, 17);
            this.labelGrossProfit.TabIndex = 49;
            this.labelGrossProfit.Text = "Gross profit";
            // 
            // textBoxGrossProfit
            // 
            this.textBoxGrossProfit.Location = new System.Drawing.Point(163, 393);
            this.textBoxGrossProfit.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGrossProfit.Name = "textBoxGrossProfit";
            this.textBoxGrossProfit.Size = new System.Drawing.Size(89, 22);
            this.textBoxGrossProfit.TabIndex = 48;
            // 
            // labelOperMargin
            // 
            this.labelOperMargin.AutoSize = true;
            this.labelOperMargin.Location = new System.Drawing.Point(7, 361);
            this.labelOperMargin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOperMargin.Name = "labelOperMargin";
            this.labelOperMargin.Size = new System.Drawing.Size(138, 17);
            this.labelOperMargin.TabIndex = 47;
            this.labelOperMargin.Text = "Operating margin, %";
            // 
            // textBoxOperatingMargin
            // 
            this.textBoxOperatingMargin.Location = new System.Drawing.Point(163, 357);
            this.textBoxOperatingMargin.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOperatingMargin.Name = "textBoxOperatingMargin";
            this.textBoxOperatingMargin.Size = new System.Drawing.Size(89, 22);
            this.textBoxOperatingMargin.TabIndex = 46;
            // 
            // labelProfitMargin
            // 
            this.labelProfitMargin.AutoSize = true;
            this.labelProfitMargin.Location = new System.Drawing.Point(36, 325);
            this.labelProfitMargin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProfitMargin.Name = "labelProfitMargin";
            this.labelProfitMargin.Size = new System.Drawing.Size(108, 17);
            this.labelProfitMargin.TabIndex = 45;
            this.labelProfitMargin.Text = "Profit margin, %";
            // 
            // textBoxProfitMargin
            // 
            this.textBoxProfitMargin.Location = new System.Drawing.Point(163, 321);
            this.textBoxProfitMargin.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProfitMargin.Name = "textBoxProfitMargin";
            this.textBoxProfitMargin.Size = new System.Drawing.Size(89, 22);
            this.textBoxProfitMargin.TabIndex = 44;
            // 
            // labelQEG
            // 
            this.labelQEG.AutoSize = true;
            this.labelQEG.Location = new System.Drawing.Point(-3, 289);
            this.labelQEG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelQEG.Name = "labelQEG";
            this.labelQEG.Size = new System.Drawing.Size(149, 17);
            this.labelQEG.TabIndex = 43;
            this.labelQEG.Text = "Qua. Ear. Gr. (yoy), %";
            // 
            // textBoxQEG
            // 
            this.textBoxQEG.Location = new System.Drawing.Point(163, 286);
            this.textBoxQEG.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxQEG.Name = "textBoxQEG";
            this.textBoxQEG.Size = new System.Drawing.Size(89, 22);
            this.textBoxQEG.TabIndex = 42;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(39, 11);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 17);
            this.linkLabel1.TabIndex = 41;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // panelRussiaCoefs
            // 
            this.panelRussiaCoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRussiaCoefs.Controls.Add(this.labelCurrLiqudity);
            this.panelRussiaCoefs.Controls.Add(this.textBoxCurrLiquidity);
            this.panelRussiaCoefs.Controls.Add(this.labelQuickLiquidity);
            this.panelRussiaCoefs.Controls.Add(this.textBoxQuickLiquidity);
            this.panelRussiaCoefs.Controls.Add(this.labelOperMargin5Y);
            this.panelRussiaCoefs.Controls.Add(this.textBoxOperMargin5Y);
            this.panelRussiaCoefs.Controls.Add(this.labelProfitMargin5Y);
            this.panelRussiaCoefs.Controls.Add(this.textBoxProfitMargin5Y);
            this.panelRussiaCoefs.Controls.Add(this.labelGrowthCapCosts);
            this.panelRussiaCoefs.Controls.Add(this.textBoxGrowthCapCosts);
            this.panelRussiaCoefs.Controls.Add(this.labelGrowthPS5Y);
            this.panelRussiaCoefs.Controls.Add(this.textBoxGrowthPS5Y);
            this.panelRussiaCoefs.Controls.Add(this.labelProfitPerShare);
            this.panelRussiaCoefs.Controls.Add(this.textBoxProfitPerShare);
            this.panelRussiaCoefs.Controls.Add(this.label5YProfitCoef);
            this.panelRussiaCoefs.Controls.Add(this.textBox5YProfitCoef);
            this.panelRussiaCoefs.Controls.Add(this.labelProfitCoef);
            this.panelRussiaCoefs.Controls.Add(this.textBoxProfitCoef);
            this.panelRussiaCoefs.Controls.Add(this.label5YValProfit);
            this.panelRussiaCoefs.Controls.Add(this.textBox5YValProfit);
            this.panelRussiaCoefs.Controls.Add(this.labelRussiaPanel);
            this.panelRussiaCoefs.Location = new System.Drawing.Point(944, 21);
            this.panelRussiaCoefs.Margin = new System.Windows.Forms.Padding(4);
            this.panelRussiaCoefs.Name = "panelRussiaCoefs";
            this.panelRussiaCoefs.Size = new System.Drawing.Size(286, 457);
            this.panelRussiaCoefs.TabIndex = 58;
            // 
            // labelCurrLiqudity
            // 
            this.labelCurrLiqudity.AutoSize = true;
            this.labelCurrLiqudity.Location = new System.Drawing.Point(7, 315);
            this.labelCurrLiqudity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCurrLiqudity.Name = "labelCurrLiqudity";
            this.labelCurrLiqudity.Size = new System.Drawing.Size(165, 17);
            this.labelCurrLiqudity.TabIndex = 89;
            this.labelCurrLiqudity.Text = "Коэф. тек. ливкидности";
            // 
            // textBoxCurrLiquidity
            // 
            this.textBoxCurrLiquidity.Location = new System.Drawing.Point(185, 311);
            this.textBoxCurrLiquidity.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCurrLiquidity.Name = "textBoxCurrLiquidity";
            this.textBoxCurrLiquidity.Size = new System.Drawing.Size(89, 22);
            this.textBoxCurrLiquidity.TabIndex = 88;
            // 
            // labelQuickLiquidity
            // 
            this.labelQuickLiquidity.AutoSize = true;
            this.labelQuickLiquidity.Location = new System.Drawing.Point(27, 284);
            this.labelQuickLiquidity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelQuickLiquidity.Name = "labelQuickLiquidity";
            this.labelQuickLiquidity.Size = new System.Drawing.Size(148, 17);
            this.labelQuickLiquidity.TabIndex = 87;
            this.labelQuickLiquidity.Text = "Коэф. срочн. ликв-ти";
            // 
            // textBoxQuickLiquidity
            // 
            this.textBoxQuickLiquidity.Location = new System.Drawing.Point(185, 281);
            this.textBoxQuickLiquidity.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxQuickLiquidity.Name = "textBoxQuickLiquidity";
            this.textBoxQuickLiquidity.Size = new System.Drawing.Size(89, 22);
            this.textBoxQuickLiquidity.TabIndex = 86;
            // 
            // labelOperMargin5Y
            // 
            this.labelOperMargin5Y.AutoSize = true;
            this.labelOperMargin5Y.Location = new System.Drawing.Point(31, 254);
            this.labelOperMargin5Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelOperMargin5Y.Name = "labelOperMargin5Y";
            this.labelOperMargin5Y.Size = new System.Drawing.Size(143, 17);
            this.labelOperMargin5Y.TabIndex = 85;
            this.labelOperMargin5Y.Text = "Опер маржа, 5YA, %";
            // 
            // textBoxOperMargin5Y
            // 
            this.textBoxOperMargin5Y.Location = new System.Drawing.Point(185, 250);
            this.textBoxOperMargin5Y.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxOperMargin5Y.Name = "textBoxOperMargin5Y";
            this.textBoxOperMargin5Y.Size = new System.Drawing.Size(89, 22);
            this.textBoxOperMargin5Y.TabIndex = 84;
            // 
            // labelProfitMargin5Y
            // 
            this.labelProfitMargin5Y.AutoSize = true;
            this.labelProfitMargin5Y.Location = new System.Drawing.Point(28, 223);
            this.labelProfitMargin5Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProfitMargin5Y.Name = "labelProfitMargin5Y";
            this.labelProfitMargin5Y.Size = new System.Drawing.Size(146, 17);
            this.labelProfitMargin5Y.TabIndex = 83;
            this.labelProfitMargin5Y.Text = "Маржа приб., 5YA, %";
            // 
            // textBoxProfitMargin5Y
            // 
            this.textBoxProfitMargin5Y.Location = new System.Drawing.Point(185, 219);
            this.textBoxProfitMargin5Y.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProfitMargin5Y.Name = "textBoxProfitMargin5Y";
            this.textBoxProfitMargin5Y.Size = new System.Drawing.Size(89, 22);
            this.textBoxProfitMargin5Y.TabIndex = 82;
            // 
            // labelGrowthCapCosts
            // 
            this.labelGrowthCapCosts.AutoSize = true;
            this.labelGrowthCapCosts.Location = new System.Drawing.Point(21, 192);
            this.labelGrowthCapCosts.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGrowthCapCosts.Name = "labelGrowthCapCosts";
            this.labelGrowthCapCosts.Size = new System.Drawing.Size(152, 17);
            this.labelGrowthCapCosts.TabIndex = 81;
            this.labelGrowthCapCosts.Text = "Рост кап. расх., 5Y, %";
            // 
            // textBoxGrowthCapCosts
            // 
            this.textBoxGrowthCapCosts.Location = new System.Drawing.Point(185, 188);
            this.textBoxGrowthCapCosts.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGrowthCapCosts.Name = "textBoxGrowthCapCosts";
            this.textBoxGrowthCapCosts.Size = new System.Drawing.Size(89, 22);
            this.textBoxGrowthCapCosts.TabIndex = 80;
            // 
            // labelGrowthPS5Y
            // 
            this.labelGrowthPS5Y.AutoSize = true;
            this.labelGrowthPS5Y.Location = new System.Drawing.Point(17, 161);
            this.labelGrowthPS5Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGrowthPS5Y.Name = "labelGrowthPS5Y";
            this.labelGrowthPS5Y.Size = new System.Drawing.Size(155, 17);
            this.labelGrowthPS5Y.TabIndex = 79;
            this.labelGrowthPS5Y.Text = "Рост приб./акц., 5Y, %";
            // 
            // textBoxGrowthPS5Y
            // 
            this.textBoxGrowthPS5Y.Location = new System.Drawing.Point(185, 158);
            this.textBoxGrowthPS5Y.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxGrowthPS5Y.Name = "textBoxGrowthPS5Y";
            this.textBoxGrowthPS5Y.Size = new System.Drawing.Size(89, 22);
            this.textBoxGrowthPS5Y.TabIndex = 78;
            // 
            // labelProfitPerShare
            // 
            this.labelProfitPerShare.AutoSize = true;
            this.labelProfitPerShare.Location = new System.Drawing.Point(0, 130);
            this.labelProfitPerShare.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProfitPerShare.Name = "labelProfitPerShare";
            this.labelProfitPerShare.Size = new System.Drawing.Size(173, 17);
            this.labelProfitPerShare.TabIndex = 77;
            this.labelProfitPerShare.Text = "Приб. на акц. за 12 м., %";
            // 
            // textBoxProfitPerShare
            // 
            this.textBoxProfitPerShare.Location = new System.Drawing.Point(185, 127);
            this.textBoxProfitPerShare.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProfitPerShare.Name = "textBoxProfitPerShare";
            this.textBoxProfitPerShare.Size = new System.Drawing.Size(89, 22);
            this.textBoxProfitPerShare.TabIndex = 76;
            // 
            // label5YProfitCoef
            // 
            this.label5YProfitCoef.AutoSize = true;
            this.label5YProfitCoef.Location = new System.Drawing.Point(39, 100);
            this.label5YProfitCoef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5YProfitCoef.Name = "label5YProfitCoef";
            this.label5YProfitCoef.Size = new System.Drawing.Size(137, 17);
            this.label5YProfitCoef.TabIndex = 75;
            this.label5YProfitCoef.Text = "Коэф. приб. 5YA, %";
            // 
            // textBox5YProfitCoef
            // 
            this.textBox5YProfitCoef.Location = new System.Drawing.Point(185, 96);
            this.textBox5YProfitCoef.Margin = new System.Windows.Forms.Padding(4);
            this.textBox5YProfitCoef.Name = "textBox5YProfitCoef";
            this.textBox5YProfitCoef.Size = new System.Drawing.Size(89, 22);
            this.textBox5YProfitCoef.TabIndex = 74;
            // 
            // labelProfitCoef
            // 
            this.labelProfitCoef.AutoSize = true;
            this.labelProfitCoef.Location = new System.Drawing.Point(8, 69);
            this.labelProfitCoef.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelProfitCoef.Name = "labelProfitCoef";
            this.labelProfitCoef.Size = new System.Drawing.Size(166, 17);
            this.labelProfitCoef.TabIndex = 73;
            this.labelProfitCoef.Text = "Коэф. прибыльности, %";
            // 
            // textBoxProfitCoef
            // 
            this.textBoxProfitCoef.Location = new System.Drawing.Point(185, 65);
            this.textBoxProfitCoef.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxProfitCoef.Name = "textBoxProfitCoef";
            this.textBoxProfitCoef.Size = new System.Drawing.Size(89, 22);
            this.textBoxProfitCoef.TabIndex = 72;
            // 
            // label5YValProfit
            // 
            this.label5YValProfit.AutoSize = true;
            this.label5YValProfit.Location = new System.Drawing.Point(-1, 38);
            this.label5YValProfit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5YValProfit.Name = "label5YValProfit";
            this.label5YValProfit.Size = new System.Drawing.Size(175, 17);
            this.label5YValProfit.TabIndex = 71;
            this.label5YValProfit.Text = "Валовая прибыль 5YA, %";
            // 
            // textBox5YValProfit
            // 
            this.textBox5YValProfit.Location = new System.Drawing.Point(185, 34);
            this.textBox5YValProfit.Margin = new System.Windows.Forms.Padding(4);
            this.textBox5YValProfit.Name = "textBox5YValProfit";
            this.textBox5YValProfit.Size = new System.Drawing.Size(89, 22);
            this.textBox5YValProfit.TabIndex = 70;
            // 
            // labelRussiaPanel
            // 
            this.labelRussiaPanel.AutoSize = true;
            this.labelRussiaPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRussiaPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelRussiaPanel.Location = new System.Drawing.Point(-1, -1);
            this.labelRussiaPanel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRussiaPanel.Name = "labelRussiaPanel";
            this.labelRussiaPanel.Size = new System.Drawing.Size(69, 22);
            this.labelRussiaPanel.TabIndex = 60;
            this.labelRussiaPanel.Text = "Russia";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(20, 367);
            this.progressBar.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(507, 28);
            this.progressBar.TabIndex = 43;
            // 
            // buttonOpenReport
            // 
            this.buttonOpenReport.Enabled = false;
            this.buttonOpenReport.Location = new System.Drawing.Point(255, 271);
            this.buttonOpenReport.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOpenReport.Name = "buttonOpenReport";
            this.buttonOpenReport.Size = new System.Drawing.Size(147, 28);
            this.buttonOpenReport.TabIndex = 44;
            this.buttonOpenReport.Text = "Открыть отчет";
            this.buttonOpenReport.UseVisualStyleBackColor = true;
            this.buttonOpenReport.Click += new System.EventHandler(this.ButtonOpenReport_Click);
            // 
            // buttonAnalyzeMultiplicators
            // 
            this.buttonAnalyzeMultiplicators.Location = new System.Drawing.Point(20, 306);
            this.buttonAnalyzeMultiplicators.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAnalyzeMultiplicators.Name = "buttonAnalyzeMultiplicators";
            this.buttonAnalyzeMultiplicators.Size = new System.Drawing.Size(212, 28);
            this.buttonAnalyzeMultiplicators.TabIndex = 45;
            this.buttonAnalyzeMultiplicators.Text = "Проанализировать все";
            this.buttonAnalyzeMultiplicators.UseVisualStyleBackColor = true;
            this.buttonAnalyzeMultiplicators.Click += new System.EventHandler(this.ButtonAnalyzeMultiplicators_Click);
            // 
            // labelMainPE
            // 
            this.labelMainPE.AutoSize = true;
            this.labelMainPE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMainPE.Location = new System.Drawing.Point(816, 37);
            this.labelMainPE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMainPE.Name = "labelMainPE";
            this.labelMainPE.Size = new System.Drawing.Size(66, 19);
            this.labelMainPE.TabIndex = 46;
            this.labelMainPE.Text = "MainPE: ";
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMain.Location = new System.Drawing.Point(941, 37);
            this.labelMain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(48, 19);
            this.labelMain.TabIndex = 47;
            this.labelMain.Text = "Main: ";
            // 
            // labelMainAll
            // 
            this.labelMainAll.AutoSize = true;
            this.labelMainAll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMainAll.Location = new System.Drawing.Point(1063, 37);
            this.labelMainAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMainAll.Name = "labelMainAll";
            this.labelMainAll.Size = new System.Drawing.Size(63, 19);
            this.labelMainAll.TabIndex = 48;
            this.labelMainAll.Text = "MainAll: ";
            // 
            // labelSortingMode
            // 
            this.labelSortingMode.AutoSize = true;
            this.labelSortingMode.Location = new System.Drawing.Point(12, 4);
            this.labelSortingMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSortingMode.Name = "labelSortingMode";
            this.labelSortingMode.Size = new System.Drawing.Size(86, 17);
            this.labelSortingMode.TabIndex = 49;
            this.labelSortingMode.Text = "Сортировка";
            // 
            // radioButtonMainPE
            // 
            this.radioButtonMainPE.AutoSize = true;
            this.radioButtonMainPE.Location = new System.Drawing.Point(127, 4);
            this.radioButtonMainPE.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonMainPE.Name = "radioButtonMainPE";
            this.radioButtonMainPE.Size = new System.Drawing.Size(17, 16);
            this.radioButtonMainPE.TabIndex = 50;
            this.radioButtonMainPE.TabStop = true;
            this.radioButtonMainPE.UseVisualStyleBackColor = true;
            this.radioButtonMainPE.CheckedChanged += new System.EventHandler(this.RadioButtonMainPE_CheckedChanged);
            // 
            // radioButtonMain
            // 
            this.radioButtonMain.AutoSize = true;
            this.radioButtonMain.Location = new System.Drawing.Point(249, 4);
            this.radioButtonMain.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonMain.Name = "radioButtonMain";
            this.radioButtonMain.Size = new System.Drawing.Size(17, 16);
            this.radioButtonMain.TabIndex = 51;
            this.radioButtonMain.TabStop = true;
            this.radioButtonMain.UseVisualStyleBackColor = true;
            this.radioButtonMain.CheckedChanged += new System.EventHandler(this.RadioButtonMain_CheckedChanged);
            // 
            // radioButtonMainAll
            // 
            this.radioButtonMainAll.AutoSize = true;
            this.radioButtonMainAll.Location = new System.Drawing.Point(377, 4);
            this.radioButtonMainAll.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonMainAll.Name = "radioButtonMainAll";
            this.radioButtonMainAll.Size = new System.Drawing.Size(17, 16);
            this.radioButtonMainAll.TabIndex = 52;
            this.radioButtonMainAll.TabStop = true;
            this.radioButtonMainAll.UseVisualStyleBackColor = true;
            this.radioButtonMainAll.CheckedChanged += new System.EventHandler(this.RadioButtonMainAll_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.radioButtonMainAll);
            this.panel5.Controls.Add(this.radioButtonMain);
            this.panel5.Controls.Add(this.radioButtonMainPE);
            this.panel5.Controls.Add(this.labelSortingMode);
            this.panel5.Location = new System.Drawing.Point(705, 6);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(449, 27);
            this.panel5.TabIndex = 53;
            // 
            // radioButtonFromTinkoff
            // 
            this.radioButtonFromTinkoff.AutoSize = true;
            this.radioButtonFromTinkoff.Checked = true;
            this.radioButtonFromTinkoff.Location = new System.Drawing.Point(20, 108);
            this.radioButtonFromTinkoff.Margin = new System.Windows.Forms.Padding(4);
            this.radioButtonFromTinkoff.Name = "radioButtonFromTinkoff";
            this.radioButtonFromTinkoff.Size = new System.Drawing.Size(127, 21);
            this.radioButtonFromTinkoff.TabIndex = 54;
            this.radioButtonFromTinkoff.TabStop = true;
            this.radioButtonFromTinkoff.Text = "С сайта Tinkoff";
            this.radioButtonFromTinkoff.UseVisualStyleBackColor = true;
            this.radioButtonFromTinkoff.CheckedChanged += new System.EventHandler(this.RadioButtonFromTinkoff_CheckedChanged);
            // 
            // buttonGetTinkoffStocks
            // 
            this.buttonGetTinkoffStocks.BackColor = System.Drawing.SystemColors.Control;
            this.buttonGetTinkoffStocks.Enabled = false;
            this.buttonGetTinkoffStocks.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonGetTinkoffStocks.Location = new System.Drawing.Point(189, 193);
            this.buttonGetTinkoffStocks.Margin = new System.Windows.Forms.Padding(4);
            this.buttonGetTinkoffStocks.Name = "buttonGetTinkoffStocks";
            this.buttonGetTinkoffStocks.Size = new System.Drawing.Size(235, 28);
            this.buttonGetTinkoffStocks.TabIndex = 55;
            this.buttonGetTinkoffStocks.Text = "Получить список с сайта Tinkoff";
            this.buttonGetTinkoffStocks.UseVisualStyleBackColor = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(447, 87);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(72, 54);
            this.webBrowser1.TabIndex = 56;
            // 
            // buttonCkechTinkoff
            // 
            this.buttonCkechTinkoff.Location = new System.Drawing.Point(254, 235);
            this.buttonCkechTinkoff.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCkechTinkoff.Name = "buttonCkechTinkoff";
            this.buttonCkechTinkoff.Size = new System.Drawing.Size(204, 28);
            this.buttonCkechTinkoff.TabIndex = 57;
            this.buttonCkechTinkoff.Text = "Догрузить список тинькофф";
            this.buttonCkechTinkoff.UseVisualStyleBackColor = true;
            this.buttonCkechTinkoff.Click += new System.EventHandler(this.ButtonCkechTinkoff_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1712, 962);
            this.Controls.Add(this.buttonCkechTinkoff);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.buttonGetTinkoffStocks);
            this.Controls.Add(this.radioButtonFromTinkoff);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.labelMainAll);
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.labelMainPE);
            this.Controls.Add(this.buttonAnalyzeMultiplicators);
            this.Controls.Add(this.buttonOpenReport);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.buttonLoadHistoryFile);
            this.Controls.Add(this.buttonSaveHistory);
            this.Controls.Add(this.labelDone);
            this.Controls.Add(this.labelStockCount);
            this.Controls.Add(this.buttonLoadStocksMultiplicators);
            this.Controls.Add(this.buttonLoadAllStocks);
            this.Controls.Add(this.radioButtonLondonStocks);
            this.Controls.Add(this.radioButtonUSAStocks);
            this.Controls.Add(this.radioButtonRusStocks);
            this.Controls.Add(this.radioButtonAllStocks);
            this.Controls.Add(this.radioButtonStarred);
            this.Controls.Add(this.richTextBoxLog);
            this.Controls.Add(this.labelSelStock);
            this.Controls.Add(this.comboBoxStocks);
            this.Controls.Add(this.buttonGetInfo);
            this.Controls.Add(this.labelRemainingTime);
            this.Controls.Add(this.textBoxStockLastUpdated);
            this.Controls.Add(this.labelLastUpdateTime);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Stocks Analyzer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelUSACoefs.ResumeLayout(false);
            this.panelUSACoefs.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panelRussiaCoefs.ResumeLayout(false);
            this.panelRussiaCoefs.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelRemainingTime;
        private System.Windows.Forms.Button buttonGetInfo;
        private System.Windows.Forms.Label labelSelStock;
        private System.Windows.Forms.ComboBox comboBoxStocks;
        private System.Windows.Forms.RadioButton radioButtonStarred;
        private System.Windows.Forms.RadioButton radioButtonAllStocks;
        private System.Windows.Forms.RadioButton radioButtonRusStocks;
        private System.Windows.Forms.RadioButton radioButtonUSAStocks;
        private System.Windows.Forms.RadioButton radioButtonLondonStocks;
        private System.Windows.Forms.Button buttonLoadAllStocks;
        private System.Windows.Forms.Button buttonLoadStocksMultiplicators;
        private System.Windows.Forms.Label labelStockCount;
        private System.Windows.Forms.Label labelStockName;
        private System.Windows.Forms.TextBox textBoxStockSymbol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelLastUpdateTime;
        private System.Windows.Forms.TextBox textBoxStockLastUpdated;
        private System.Windows.Forms.CheckBox checkBoxIsStarred;
        private System.Windows.Forms.Label labelPriceRub;
        private System.Windows.Forms.TextBox textBoxStockPrice;
        private System.Windows.Forms.Label labelPE;
        private System.Windows.Forms.TextBox textBoxPE;
        private System.Windows.Forms.Label labelPS;
        private System.Windows.Forms.TextBox textBoxPS;
        private System.Windows.Forms.Label labelPBV;
        private System.Windows.Forms.TextBox textBoxPBV;
        private System.Windows.Forms.Label labelEVEBITDA;
        private System.Windows.Forms.TextBox textBoxEVEBITDA;
        private System.Windows.Forms.Label labelDebtEBITDA;
        private System.Windows.Forms.TextBox textBoxDebtEBITDA;
        private System.Windows.Forms.Label labelROE;
        private System.Windows.Forms.TextBox textBoxROE;
        private System.Windows.Forms.Label labelEPS;
        private System.Windows.Forms.TextBox textBoxEPS;
        private System.Windows.Forms.Label labelDone;
        private System.Windows.Forms.Label labelPriceUSD;
        private System.Windows.Forms.TextBox textBoxStockPriceUSD;
        private System.Windows.Forms.Button buttonSaveHistory;
        public System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonLoadHistoryFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label labelPEG;
        private System.Windows.Forms.TextBox textBoxPEG;
        private System.Windows.Forms.Label labelEntVal;
        private System.Windows.Forms.TextBox textBoxEntVal;
        private System.Windows.Forms.Label labelMarketCap;
        private System.Windows.Forms.TextBox textBoxMarketCap;
        private System.Windows.Forms.Label labelGrossProfit;
        private System.Windows.Forms.TextBox textBoxGrossProfit;
        private System.Windows.Forms.Label labelOperMargin;
        private System.Windows.Forms.TextBox textBoxOperatingMargin;
        private System.Windows.Forms.Label labelProfitMargin;
        private System.Windows.Forms.TextBox textBoxProfitMargin;
        private System.Windows.Forms.Label labelQEG;
        private System.Windows.Forms.TextBox textBoxQEG;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelCommonCoefs;
        private System.Windows.Forms.Label labelSymbol;
        private System.Windows.Forms.Panel panelRussiaCoefs;
        private System.Windows.Forms.Label labelRussiaPanel;
        private System.Windows.Forms.Panel panelUSACoefs;
        private System.Windows.Forms.Label labelUSAPanel;
        private System.Windows.Forms.Label labelSharesCount;
        private System.Windows.Forms.TextBox textBoxSharesCount;
        private System.Windows.Forms.Label labelLeveredFreeCF;
        private System.Windows.Forms.TextBox textBoxLeveredFreeCF;
        private System.Windows.Forms.Label labelOperCashFlow;
        private System.Windows.Forms.TextBox textBoxOperCashFlow;
        private System.Windows.Forms.Label labelBookValPShape;
        private System.Windows.Forms.TextBox textBoxBookValPShape;
        private System.Windows.Forms.Label labelTotDebt;
        private System.Windows.Forms.TextBox textBoxTotDebt;
        private System.Windows.Forms.Label labelTotCashPShape;
        private System.Windows.Forms.TextBox textBoxTotCashPShape;
        private System.Windows.Forms.Label labelTotCash;
        private System.Windows.Forms.TextBox textBoxTotCash;
        private System.Windows.Forms.Label labelEBITDA;
        private System.Windows.Forms.TextBox textBoxEBITDA;
        private System.Windows.Forms.Label labelRevenuePerShape;
        private System.Windows.Forms.TextBox textBoxRevPerShape;
        private System.Windows.Forms.Label labelRevenue;
        private System.Windows.Forms.TextBox textBoxRevenue;
        private System.Windows.Forms.Label labelRetOnAssests;
        private System.Windows.Forms.TextBox textBoxRetOnAssets;
        private System.Windows.Forms.Label labelEVRev;
        private System.Windows.Forms.TextBox textBoxEVRev;
        private System.Windows.Forms.Label labelCurrLiqudity;
        private System.Windows.Forms.TextBox textBoxCurrLiquidity;
        private System.Windows.Forms.Label labelQuickLiquidity;
        private System.Windows.Forms.TextBox textBoxQuickLiquidity;
        private System.Windows.Forms.Label labelOperMargin5Y;
        private System.Windows.Forms.TextBox textBoxOperMargin5Y;
        private System.Windows.Forms.Label labelProfitMargin5Y;
        private System.Windows.Forms.TextBox textBoxProfitMargin5Y;
        private System.Windows.Forms.Label labelGrowthCapCosts;
        private System.Windows.Forms.TextBox textBoxGrowthCapCosts;
        private System.Windows.Forms.Label labelGrowthPS5Y;
        private System.Windows.Forms.TextBox textBoxGrowthPS5Y;
        private System.Windows.Forms.Label labelProfitPerShare;
        private System.Windows.Forms.TextBox textBoxProfitPerShare;
        private System.Windows.Forms.Label label5YProfitCoef;
        private System.Windows.Forms.TextBox textBox5YProfitCoef;
        private System.Windows.Forms.Label labelProfitCoef;
        private System.Windows.Forms.TextBox textBoxProfitCoef;
        private System.Windows.Forms.Label label5YValProfit;
        private System.Windows.Forms.TextBox textBox5YValProfit;
        private System.Windows.Forms.Label labelGrossPerc;
        private System.Windows.Forms.Button buttonOpenReport;
        private System.Windows.Forms.Button buttonAnalyzeMultiplicators;
        private System.Windows.Forms.Label labelMainPE;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Label labelMainAll;
        private System.Windows.Forms.Label labelSortingMode;
        private System.Windows.Forms.RadioButton radioButtonMainPE;
        private System.Windows.Forms.RadioButton radioButtonMain;
        private System.Windows.Forms.RadioButton radioButtonMainAll;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton radioButtonFromTinkoff;
        private System.Windows.Forms.Button buttonGetTinkoffStocks;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button buttonCkechTinkoff;
    }
}

