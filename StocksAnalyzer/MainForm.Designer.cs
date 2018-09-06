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
			this.labelDone = new System.Windows.Forms.Label();
			this.labelPriceUSD = new System.Windows.Forms.Label();
			this.textBoxStockPriceUSD = new System.Windows.Forms.TextBox();
			this.buttonSaveHistory = new System.Windows.Forms.Button();
			this.buttonLoadHistoryFile = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.panelMain = new System.Windows.Forms.Panel();
			this.panelMetric = new System.Windows.Forms.Panel();
			this.labelMetric = new System.Windows.Forms.Label();
			this.textBoxRatingCoefs = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxRatingMetrics = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxRatingAll = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.panelRussiaCoefs = new System.Windows.Forms.Panel();
			this.labelRussiaPanel = new System.Windows.Forms.Label();
			this.panelUSACoefs = new System.Windows.Forms.Panel();
			this.labelUSAPanel = new System.Windows.Forms.Label();
			this.panelCommon = new System.Windows.Forms.Panel();
			this.labelSymbol = new System.Windows.Forms.Label();
			this.labelCommonCoefs = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.buttonOpenReport = new System.Windows.Forms.Button();
			this.buttonAnalyzeMultiplicators = new System.Windows.Forms.Button();
			this.radioButtonFromTinkoff = new System.Windows.Forms.RadioButton();
			this.buttonGetTinkoffStocks = new System.Windows.Forms.Button();
			this.buttonCkechTinkoff = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.panelMain.SuspendLayout();
			this.panelMetric.SuspendLayout();
			this.panelRussiaCoefs.SuspendLayout();
			this.panelUSACoefs.SuspendLayout();
			this.panelCommon.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelRemainingTime
			// 
			this.labelRemainingTime.AutoSize = true;
			this.labelRemainingTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelRemainingTime.Location = new System.Drawing.Point(10, 274);
			this.labelRemainingTime.Name = "labelRemainingTime";
			this.labelRemainingTime.Size = new System.Drawing.Size(208, 16);
			this.labelRemainingTime.TabIndex = 1;
			this.labelRemainingTime.Text = "Расчетное оставшееся время: ";
			// 
			// buttonGetInfo
			// 
			this.buttonGetInfo.Location = new System.Drawing.Point(238, 32);
			this.buttonGetInfo.Name = "buttonGetInfo";
			this.buttonGetInfo.Size = new System.Drawing.Size(92, 23);
			this.buttonGetInfo.TabIndex = 2;
			this.buttonGetInfo.Text = "Получить инфо";
			this.buttonGetInfo.UseVisualStyleBackColor = true;
			this.buttonGetInfo.Click += new System.EventHandler(this.ButtonGetInfo_Click);
			// 
			// labelSelStock
			// 
			this.labelSelStock.AutoSize = true;
			this.labelSelStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelSelStock.Location = new System.Drawing.Point(12, 15);
			this.labelSelStock.Name = "labelSelStock";
			this.labelSelStock.Size = new System.Drawing.Size(105, 15);
			this.labelSelStock.TabIndex = 4;
			this.labelSelStock.Text = "Выберите акцию";
			// 
			// comboBoxStocks
			// 
			this.comboBoxStocks.FormattingEnabled = true;
			this.comboBoxStocks.Location = new System.Drawing.Point(15, 33);
			this.comboBoxStocks.Name = "comboBoxStocks";
			this.comboBoxStocks.Size = new System.Drawing.Size(216, 21);
			this.comboBoxStocks.TabIndex = 3;
			this.comboBoxStocks.DropDown += new System.EventHandler(this.ComboBoxStocks_DropDown);
			this.comboBoxStocks.SelectedIndexChanged += new System.EventHandler(this.ComboBoxStocks_SelectedIndexChanged);
			this.comboBoxStocks.TextChanged += new System.EventHandler(this.ComboBoxStocks_TextChanged);
			// 
			// richTextBoxLog
			// 
			this.richTextBoxLog.Location = new System.Drawing.Point(15, 326);
			this.richTextBoxLog.Name = "richTextBoxLog";
			this.richTextBoxLog.Size = new System.Drawing.Size(319, 446);
			this.richTextBoxLog.TabIndex = 5;
			this.richTextBoxLog.Text = "";
			// 
			// radioButtonStarred
			// 
			this.radioButtonStarred.AutoSize = true;
			this.radioButtonStarred.Location = new System.Drawing.Point(15, 71);
			this.radioButtonStarred.Name = "radioButtonStarred";
			this.radioButtonStarred.Size = new System.Drawing.Size(83, 17);
			this.radioButtonStarred.TabIndex = 6;
			this.radioButtonStarred.Text = "Избранные";
			this.radioButtonStarred.UseVisualStyleBackColor = true;
			this.radioButtonStarred.CheckedChanged += new System.EventHandler(this.RadioButtonStarred_CheckedChanged);
			// 
			// radioButtonAllStocks
			// 
			this.radioButtonAllStocks.AutoSize = true;
			this.radioButtonAllStocks.Location = new System.Drawing.Point(15, 105);
			this.radioButtonAllStocks.Name = "radioButtonAllStocks";
			this.radioButtonAllStocks.Size = new System.Drawing.Size(44, 17);
			this.radioButtonAllStocks.TabIndex = 7;
			this.radioButtonAllStocks.Text = "Все";
			this.radioButtonAllStocks.UseVisualStyleBackColor = true;
			this.radioButtonAllStocks.CheckedChanged += new System.EventHandler(this.RadioButtonAllStocks_CheckedChanged);
			// 
			// radioButtonRusStocks
			// 
			this.radioButtonRusStocks.AutoSize = true;
			this.radioButtonRusStocks.Location = new System.Drawing.Point(15, 122);
			this.radioButtonRusStocks.Name = "radioButtonRusStocks";
			this.radioButtonRusStocks.Size = new System.Drawing.Size(67, 17);
			this.radioButtonRusStocks.TabIndex = 8;
			this.radioButtonRusStocks.Text = "Русские";
			this.radioButtonRusStocks.UseVisualStyleBackColor = true;
			this.radioButtonRusStocks.CheckedChanged += new System.EventHandler(this.RadioButtonRus_CheckedChanged);
			// 
			// radioButtonUSAStocks
			// 
			this.radioButtonUSAStocks.AutoSize = true;
			this.radioButtonUSAStocks.Location = new System.Drawing.Point(15, 140);
			this.radioButtonUSAStocks.Name = "radioButtonUSAStocks";
			this.radioButtonUSAStocks.Size = new System.Drawing.Size(47, 17);
			this.radioButtonUSAStocks.TabIndex = 9;
			this.radioButtonUSAStocks.Text = "USA";
			this.radioButtonUSAStocks.UseVisualStyleBackColor = true;
			this.radioButtonUSAStocks.CheckedChanged += new System.EventHandler(this.RadioButtonUSA_CheckedChanged);
			// 
			// radioButtonLondonStocks
			// 
			this.radioButtonLondonStocks.AutoSize = true;
			this.radioButtonLondonStocks.Enabled = false;
			this.radioButtonLondonStocks.Location = new System.Drawing.Point(15, 157);
			this.radioButtonLondonStocks.Name = "radioButtonLondonStocks";
			this.radioButtonLondonStocks.Size = new System.Drawing.Size(61, 17);
			this.radioButtonLondonStocks.TabIndex = 10;
			this.radioButtonLondonStocks.Text = "London";
			this.radioButtonLondonStocks.UseVisualStyleBackColor = true;
			this.radioButtonLondonStocks.CheckedChanged += new System.EventHandler(this.RadioButtonLondon_CheckedChanged);
			// 
			// buttonLoadAllStocks
			// 
			this.buttonLoadAllStocks.Location = new System.Drawing.Point(15, 191);
			this.buttonLoadAllStocks.Name = "buttonLoadAllStocks";
			this.buttonLoadAllStocks.Size = new System.Drawing.Size(159, 23);
			this.buttonLoadAllStocks.TabIndex = 11;
			this.buttonLoadAllStocks.Text = "Загрузить список акций";
			this.buttonLoadAllStocks.UseVisualStyleBackColor = true;
			this.buttonLoadAllStocks.Click += new System.EventHandler(this.ButtonLoadAllStocksClick);
			// 
			// buttonLoadStocksMultiplicators
			// 
			this.buttonLoadStocksMultiplicators.Location = new System.Drawing.Point(15, 220);
			this.buttonLoadStocksMultiplicators.Name = "buttonLoadStocksMultiplicators";
			this.buttonLoadStocksMultiplicators.Size = new System.Drawing.Size(159, 23);
			this.buttonLoadStocksMultiplicators.TabIndex = 12;
			this.buttonLoadStocksMultiplicators.Text = "Загрузить инфо для списка";
			this.buttonLoadStocksMultiplicators.UseVisualStyleBackColor = true;
			this.buttonLoadStocksMultiplicators.Click += new System.EventHandler(this.ButtonLoadStockMultiplicators_Click);
			// 
			// labelStockCount
			// 
			this.labelStockCount.AutoSize = true;
			this.labelStockCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelStockCount.Location = new System.Drawing.Point(142, 72);
			this.labelStockCount.Name = "labelStockCount";
			this.labelStockCount.Size = new System.Drawing.Size(80, 15);
			this.labelStockCount.TabIndex = 14;
			this.labelStockCount.Text = "Общее кол-во";
			// 
			// labelStockName
			// 
			this.labelStockName.AutoSize = true;
			this.labelStockName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelStockName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelStockName.Location = new System.Drawing.Point(102, 32);
			this.labelStockName.Name = "labelStockName";
			this.labelStockName.Size = new System.Drawing.Size(53, 18);
			this.labelStockName.TabIndex = 15;
			this.labelStockName.Text = "label2";
			// 
			// textBoxStockSymbol
			// 
			this.textBoxStockSymbol.BackColor = System.Drawing.SystemColors.ControlLight;
			this.textBoxStockSymbol.Location = new System.Drawing.Point(191, 11);
			this.textBoxStockSymbol.Name = "textBoxStockSymbol";
			this.textBoxStockSymbol.Size = new System.Drawing.Size(68, 20);
			this.textBoxStockSymbol.TabIndex = 16;
			this.textBoxStockSymbol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(863, 432);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 13);
			this.label2.TabIndex = 17;
			this.label2.Text = "Symbol";
			this.label2.Visible = false;
			// 
			// labelLastUpdateTime
			// 
			this.labelLastUpdateTime.AutoSize = true;
			this.labelLastUpdateTime.Location = new System.Drawing.Point(18, 56);
			this.labelLastUpdateTime.Name = "labelLastUpdateTime";
			this.labelLastUpdateTime.Size = new System.Drawing.Size(103, 13);
			this.labelLastUpdateTime.TabIndex = 19;
			this.labelLastUpdateTime.Text = "Время обновления";
			// 
			// textBoxStockLastUpdated
			// 
			this.textBoxStockLastUpdated.Location = new System.Drawing.Point(120, 53);
			this.textBoxStockLastUpdated.Name = "textBoxStockLastUpdated";
			this.textBoxStockLastUpdated.Size = new System.Drawing.Size(120, 20);
			this.textBoxStockLastUpdated.TabIndex = 18;
			// 
			// checkBoxIsStarred
			// 
			this.checkBoxIsStarred.AutoSize = true;
			this.checkBoxIsStarred.Enabled = false;
			this.checkBoxIsStarred.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.checkBoxIsStarred.Location = new System.Drawing.Point(21, 30);
			this.checkBoxIsStarred.Name = "checkBoxIsStarred";
			this.checkBoxIsStarred.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.checkBoxIsStarred.Size = new System.Drawing.Size(71, 20);
			this.checkBoxIsStarred.TabIndex = 20;
			this.checkBoxIsStarred.Text = "Starred";
			this.checkBoxIsStarred.UseVisualStyleBackColor = true;
			this.checkBoxIsStarred.CheckedChanged += new System.EventHandler(this.CheckBoxIsStarred_CheckedChanged);
			// 
			// labelPriceRub
			// 
			this.labelPriceRub.AutoSize = true;
			this.labelPriceRub.Location = new System.Drawing.Point(114, 37);
			this.labelPriceRub.Name = "labelPriceRub";
			this.labelPriceRub.Size = new System.Drawing.Size(55, 13);
			this.labelPriceRub.TabIndex = 22;
			this.labelPriceRub.Text = "Price (rub)";
			// 
			// textBoxStockPrice
			// 
			this.textBoxStockPrice.Location = new System.Drawing.Point(108, 57);
			this.textBoxStockPrice.Name = "textBoxStockPrice";
			this.textBoxStockPrice.Size = new System.Drawing.Size(68, 20);
			this.textBoxStockPrice.TabIndex = 21;
			this.textBoxStockPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// labelDone
			// 
			this.labelDone.AutoSize = true;
			this.labelDone.BackColor = System.Drawing.Color.Lime;
			this.labelDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelDone.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.labelDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelDone.Location = new System.Drawing.Point(277, 58);
			this.labelDone.Name = "labelDone";
			this.labelDone.Size = new System.Drawing.Size(59, 22);
			this.labelDone.TabIndex = 37;
			this.labelDone.Text = "Done.";
			this.labelDone.Visible = false;
			// 
			// labelPriceUSD
			// 
			this.labelPriceUSD.AutoSize = true;
			this.labelPriceUSD.Location = new System.Drawing.Point(197, 37);
			this.labelPriceUSD.Name = "labelPriceUSD";
			this.labelPriceUSD.Size = new System.Drawing.Size(57, 13);
			this.labelPriceUSD.TabIndex = 39;
			this.labelPriceUSD.Text = "Price (usd)";
			// 
			// textBoxStockPriceUSD
			// 
			this.textBoxStockPriceUSD.Location = new System.Drawing.Point(191, 57);
			this.textBoxStockPriceUSD.Name = "textBoxStockPriceUSD";
			this.textBoxStockPriceUSD.Size = new System.Drawing.Size(68, 20);
			this.textBoxStockPriceUSD.TabIndex = 38;
			this.textBoxStockPriceUSD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// buttonSaveHistory
			// 
			this.buttonSaveHistory.Location = new System.Drawing.Point(142, 93);
			this.buttonSaveHistory.Name = "buttonSaveHistory";
			this.buttonSaveHistory.Size = new System.Drawing.Size(159, 23);
			this.buttonSaveHistory.TabIndex = 40;
			this.buttonSaveHistory.Text = "Сохранить в историю";
			this.buttonSaveHistory.UseVisualStyleBackColor = true;
			this.buttonSaveHistory.Click += new System.EventHandler(this.ButtonSaveHistory_Click);
			// 
			// buttonLoadHistoryFile
			// 
			this.buttonLoadHistoryFile.Location = new System.Drawing.Point(142, 122);
			this.buttonLoadHistoryFile.Name = "buttonLoadHistoryFile";
			this.buttonLoadHistoryFile.Size = new System.Drawing.Size(159, 23);
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
			this.panelMain.Controls.Add(this.panelMetric);
			this.panelMain.Controls.Add(this.textBoxRatingCoefs);
			this.panelMain.Controls.Add(this.label5);
			this.panelMain.Controls.Add(this.textBoxRatingMetrics);
			this.panelMain.Controls.Add(this.label4);
			this.panelMain.Controls.Add(this.textBoxRatingAll);
			this.panelMain.Controls.Add(this.label3);
			this.panelMain.Controls.Add(this.panelRussiaCoefs);
			this.panelMain.Controls.Add(this.panelUSACoefs);
			this.panelMain.Controls.Add(this.panelCommon);
			this.panelMain.Controls.Add(this.linkLabel1);
			this.panelMain.Controls.Add(this.checkBoxIsStarred);
			this.panelMain.Controls.Add(this.labelStockName);
			this.panelMain.Controls.Add(this.textBoxStockLastUpdated);
			this.panelMain.Controls.Add(this.labelLastUpdateTime);
			this.panelMain.Location = new System.Drawing.Point(339, 11);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(1090, 740);
			this.panelMain.TabIndex = 42;
			// 
			// panelMetric
			// 
			this.panelMetric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelMetric.Controls.Add(this.labelMetric);
			this.panelMetric.Location = new System.Drawing.Point(820, 128);
			this.panelMetric.Name = "panelMetric";
			this.panelMetric.Size = new System.Drawing.Size(265, 607);
			this.panelMetric.TabIndex = 59;
			// 
			// labelMetric
			// 
			this.labelMetric.AutoSize = true;
			this.labelMetric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelMetric.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelMetric.Location = new System.Drawing.Point(-2, -1);
			this.labelMetric.Name = "labelMetric";
			this.labelMetric.Size = new System.Drawing.Size(74, 18);
			this.labelMetric.TabIndex = 57;
			this.labelMetric.Text = "Метрики";
			// 
			// textBoxRatingCoefs
			// 
			this.textBoxRatingCoefs.Location = new System.Drawing.Point(638, 102);
			this.textBoxRatingCoefs.Name = "textBoxRatingCoefs";
			this.textBoxRatingCoefs.Size = new System.Drawing.Size(90, 20);
			this.textBoxRatingCoefs.TabIndex = 63;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(470, 105);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(136, 13);
			this.label5.TabIndex = 64;
			this.label5.Text = "Ср. рейтинг (коэф. норм.)";
			// 
			// textBoxRatingMetrics
			// 
			this.textBoxRatingMetrics.Location = new System.Drawing.Point(638, 76);
			this.textBoxRatingMetrics.Name = "textBoxRatingMetrics";
			this.textBoxRatingMetrics.Size = new System.Drawing.Size(90, 20);
			this.textBoxRatingMetrics.TabIndex = 61;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(470, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(118, 13);
			this.label4.TabIndex = 62;
			this.label4.Text = "Ср. рейтинг (метрики)";
			// 
			// textBoxRatingAll
			// 
			this.textBoxRatingAll.Location = new System.Drawing.Point(638, 50);
			this.textBoxRatingAll.Name = "textBoxRatingAll";
			this.textBoxRatingAll.Size = new System.Drawing.Size(90, 20);
			this.textBoxRatingAll.TabIndex = 59;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(470, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(158, 13);
			this.label3.TabIndex = 60;
			this.label3.Text = "Ср. рейтинг (метрики+коэфы)";
			// 
			// panelRussiaCoefs
			// 
			this.panelRussiaCoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelRussiaCoefs.Controls.Add(this.labelRussiaPanel);
			this.panelRussiaCoefs.Location = new System.Drawing.Point(658, 126);
			this.panelRussiaCoefs.Name = "panelRussiaCoefs";
			this.panelRussiaCoefs.Size = new System.Drawing.Size(135, 152);
			this.panelRussiaCoefs.TabIndex = 58;
			// 
			// labelRussiaPanel
			// 
			this.labelRussiaPanel.AutoSize = true;
			this.labelRussiaPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelRussiaPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelRussiaPanel.Location = new System.Drawing.Point(-1, -1);
			this.labelRussiaPanel.Name = "labelRussiaPanel";
			this.labelRussiaPanel.Size = new System.Drawing.Size(58, 18);
			this.labelRussiaPanel.TabIndex = 60;
			this.labelRussiaPanel.Text = "Russia";
			// 
			// panelUSACoefs
			// 
			this.panelUSACoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelUSACoefs.Controls.Add(this.labelUSAPanel);
			this.panelUSACoefs.Location = new System.Drawing.Point(421, 128);
			this.panelUSACoefs.Name = "panelUSACoefs";
			this.panelUSACoefs.Size = new System.Drawing.Size(394, 607);
			this.panelUSACoefs.TabIndex = 57;
			// 
			// labelUSAPanel
			// 
			this.labelUSAPanel.AutoSize = true;
			this.labelUSAPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelUSAPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelUSAPanel.Location = new System.Drawing.Point(-1, -1);
			this.labelUSAPanel.Name = "labelUSAPanel";
			this.labelUSAPanel.Size = new System.Drawing.Size(41, 18);
			this.labelUSAPanel.TabIndex = 59;
			this.labelUSAPanel.Text = "USA";
			// 
			// panelCommon
			// 
			this.panelCommon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelCommon.Controls.Add(this.labelSymbol);
			this.panelCommon.Controls.Add(this.labelCommonCoefs);
			this.panelCommon.Controls.Add(this.labelPriceUSD);
			this.panelCommon.Controls.Add(this.textBoxStockPriceUSD);
			this.panelCommon.Controls.Add(this.labelPriceRub);
			this.panelCommon.Controls.Add(this.textBoxStockPrice);
			this.panelCommon.Controls.Add(this.textBoxStockSymbol);
			this.panelCommon.Location = new System.Drawing.Point(21, 79);
			this.panelCommon.Name = "panelCommon";
			this.panelCommon.Size = new System.Drawing.Size(394, 656);
			this.panelCommon.TabIndex = 56;
			// 
			// labelSymbol
			// 
			this.labelSymbol.AutoSize = true;
			this.labelSymbol.Location = new System.Drawing.Point(144, 13);
			this.labelSymbol.Name = "labelSymbol";
			this.labelSymbol.Size = new System.Drawing.Size(41, 13);
			this.labelSymbol.TabIndex = 58;
			this.labelSymbol.Text = "Symbol";
			// 
			// labelCommonCoefs
			// 
			this.labelCommonCoefs.AutoSize = true;
			this.labelCommonCoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelCommonCoefs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelCommonCoefs.Location = new System.Drawing.Point(-2, -1);
			this.labelCommonCoefs.Name = "labelCommonCoefs";
			this.labelCommonCoefs.Size = new System.Drawing.Size(58, 18);
			this.labelCommonCoefs.TabIndex = 57;
			this.labelCommonCoefs.Text = "Общие";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(29, 9);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(0, 13);
			this.linkLabel1.TabIndex = 41;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(15, 298);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(318, 23);
			this.progressBar.TabIndex = 43;
			// 
			// buttonOpenReport
			// 
			this.buttonOpenReport.Enabled = false;
			this.buttonOpenReport.Location = new System.Drawing.Point(191, 220);
			this.buttonOpenReport.Name = "buttonOpenReport";
			this.buttonOpenReport.Size = new System.Drawing.Size(110, 23);
			this.buttonOpenReport.TabIndex = 44;
			this.buttonOpenReport.Text = "Открыть отчет";
			this.buttonOpenReport.UseVisualStyleBackColor = true;
			this.buttonOpenReport.Click += new System.EventHandler(this.ButtonOpenReport_Click);
			// 
			// buttonAnalyzeMultiplicators
			// 
			this.buttonAnalyzeMultiplicators.Location = new System.Drawing.Point(15, 249);
			this.buttonAnalyzeMultiplicators.Name = "buttonAnalyzeMultiplicators";
			this.buttonAnalyzeMultiplicators.Size = new System.Drawing.Size(159, 23);
			this.buttonAnalyzeMultiplicators.TabIndex = 45;
			this.buttonAnalyzeMultiplicators.Text = "Проанализировать все";
			this.buttonAnalyzeMultiplicators.UseVisualStyleBackColor = true;
			this.buttonAnalyzeMultiplicators.Click += new System.EventHandler(this.ButtonAnalyzeMultiplicators_Click);
			// 
			// radioButtonFromTinkoff
			// 
			this.radioButtonFromTinkoff.AutoSize = true;
			this.radioButtonFromTinkoff.Checked = true;
			this.radioButtonFromTinkoff.Location = new System.Drawing.Point(15, 88);
			this.radioButtonFromTinkoff.Name = "radioButtonFromTinkoff";
			this.radioButtonFromTinkoff.Size = new System.Drawing.Size(100, 17);
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
			this.buttonGetTinkoffStocks.Location = new System.Drawing.Point(142, 157);
			this.buttonGetTinkoffStocks.Name = "buttonGetTinkoffStocks";
			this.buttonGetTinkoffStocks.Size = new System.Drawing.Size(176, 23);
			this.buttonGetTinkoffStocks.TabIndex = 55;
			this.buttonGetTinkoffStocks.Text = "Получить список с сайта Tinkoff";
			this.buttonGetTinkoffStocks.UseVisualStyleBackColor = false;
			// 
			// buttonCkechTinkoff
			// 
			this.buttonCkechTinkoff.Location = new System.Drawing.Point(180, 191);
			this.buttonCkechTinkoff.Name = "buttonCkechTinkoff";
			this.buttonCkechTinkoff.Size = new System.Drawing.Size(153, 23);
			this.buttonCkechTinkoff.TabIndex = 57;
			this.buttonCkechTinkoff.Text = "Догрузить список тинькофф";
			this.buttonCkechTinkoff.UseVisualStyleBackColor = true;
			this.buttonCkechTinkoff.Click += new System.EventHandler(this.ButtonCkechTinkoff_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(340, 759);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(401, 13);
			this.label1.TabIndex = 65;
			this.label1.Text = "hint: Нормализованные значения коэф-ов можно найти в файле анализатора";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1438, 783);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.buttonCkechTinkoff);
			this.Controls.Add(this.buttonGetTinkoffStocks);
			this.Controls.Add(this.radioButtonFromTinkoff);
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
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "Stocks Analyzer";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.panelMetric.ResumeLayout(false);
			this.panelMetric.PerformLayout();
			this.panelRussiaCoefs.ResumeLayout(false);
			this.panelRussiaCoefs.PerformLayout();
			this.panelUSACoefs.ResumeLayout(false);
			this.panelUSACoefs.PerformLayout();
			this.panelCommon.ResumeLayout(false);
			this.panelCommon.PerformLayout();
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
        private System.Windows.Forms.Panel panelCommon;
        private System.Windows.Forms.Label labelCommonCoefs;
        private System.Windows.Forms.Label labelSymbol;
        private System.Windows.Forms.Panel panelRussiaCoefs;
        private System.Windows.Forms.Label labelRussiaPanel;
        private System.Windows.Forms.Panel panelUSACoefs;
        private System.Windows.Forms.Label labelUSAPanel;
        private System.Windows.Forms.Button buttonOpenReport;
        private System.Windows.Forms.Button buttonAnalyzeMultiplicators;
        private System.Windows.Forms.RadioButton radioButtonFromTinkoff;
        private System.Windows.Forms.Button buttonGetTinkoffStocks;
        private System.Windows.Forms.Button buttonCkechTinkoff;
		private System.Windows.Forms.TextBox textBoxRatingCoefs;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxRatingMetrics;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxRatingAll;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panelMetric;
		private System.Windows.Forms.Label labelMetric;
		private System.Windows.Forms.Label label1;
	}
}

