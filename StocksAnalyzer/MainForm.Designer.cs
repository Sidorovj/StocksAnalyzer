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
            this.panelRussiaCoefs.SuspendLayout();
            this.panelUSACoefs.SuspendLayout();
            this.panelCommon.SuspendLayout();
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
            this.panelMain.Controls.Add(this.panelCommon);
            this.panelMain.Controls.Add(this.linkLabel1);
            this.panelMain.Controls.Add(this.checkBoxIsStarred);
            this.panelMain.Controls.Add(this.labelStockName);
            this.panelMain.Location = new System.Drawing.Point(551, 65);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1139, 884);
            this.panelMain.TabIndex = 42;
            // 
            // panelRussiaCoefs
            // 
            this.panelRussiaCoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRussiaCoefs.Controls.Add(this.labelRussiaPanel);
            this.panelRussiaCoefs.Location = new System.Drawing.Point(891, 26);
            this.panelRussiaCoefs.Margin = new System.Windows.Forms.Padding(4);
            this.panelRussiaCoefs.Name = "panelRussiaCoefs";
            this.panelRussiaCoefs.Size = new System.Drawing.Size(286, 457);
            this.panelRussiaCoefs.TabIndex = 58;
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
            // panelUSACoefs
            // 
            this.panelUSACoefs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelUSACoefs.Controls.Add(this.labelUSAPanel);
            this.panelUSACoefs.Location = new System.Drawing.Point(619, 69);
            this.panelUSACoefs.Margin = new System.Windows.Forms.Padding(4);
            this.panelUSACoefs.Name = "panelUSACoefs";
            this.panelUSACoefs.Size = new System.Drawing.Size(482, 787);
            this.panelUSACoefs.TabIndex = 57;
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
            this.panelCommon.Location = new System.Drawing.Point(24, 69);
            this.panelCommon.Margin = new System.Windows.Forms.Padding(4);
            this.panelCommon.Name = "panelCommon";
            this.panelCommon.Size = new System.Drawing.Size(466, 787);
            this.panelCommon.TabIndex = 56;
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
            this.panelRussiaCoefs.ResumeLayout(false);
            this.panelRussiaCoefs.PerformLayout();
            this.panelUSACoefs.ResumeLayout(false);
            this.panelUSACoefs.PerformLayout();
            this.panelCommon.ResumeLayout(false);
            this.panelCommon.PerformLayout();
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

