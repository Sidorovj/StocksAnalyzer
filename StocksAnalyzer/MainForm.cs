using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Globalization;

namespace StocksAnalyzer
{

    public partial class MainForm : Form
    {
        private readonly List<Stock> _mRussianStocks = new List<Stock>();
        private readonly List<Stock> _mUsaStocks = new List<Stock>();
        private readonly List<Stock> _mLondonStocks = new List<Stock>();
        private readonly List<Stock> _mBestStocks = new List<Stock>();
        private readonly List<Stock> _mTinkoffStocks = new List<Stock>();
        private readonly List<string> _mBestStocksNames = new List<string>();
        private List<Stock> _mSelectedList;
        private Stock _mSelectedStock;

        public MainForm()
        {
            InitializeComponent();

            Task.Run(() => InitializeMainClass());

            FormClosing += Form1_FormClosing;

            webBrowser1.Hide();

            ToolTip t = new ToolTip();
            //Common
            t.SetToolTip(labelSymbol, "Тикет");
            t.SetToolTip(labelPE, "Цена/прибыль");
            t.SetToolTip(labelPS, "Цена/объем продаж");
            t.SetToolTip(labelPBV, "Цена/балансовая стоимость");
            t.SetToolTip(labelROE, "Прибыль на инвестиции");
            t.SetToolTip(labelEPS, "Прибыль на акцию");
            t.SetToolTip(labelQEG, "Прибыль на акцию за последний квартал к квартальной год назад");
            t.SetToolTip(labelProfitMargin, "Рентабельность (маржа прибыли до налогооблажения)");
            t.SetToolTip(labelOperMargin, "Маржа операционной прибыли");
            t.SetToolTip(labelGrossProfit, "Валовая прибыль");

            //USA
            t.SetToolTip(labelMarketCap, "Рыночная капитализация");
            t.SetToolTip(labelEntVal, "Справедливая стоимость компании = Market Cap + debt - cash");
            t.SetToolTip(labelPEG, "P/E деленная на скорость роста прибыли");
            t.SetToolTip(labelEVRev, "Стоим. предпр./доход");
            t.SetToolTip(labelRetOnAssests, "Доходность/рентабельность активов");
            t.SetToolTip(labelRevenue, "Доход/прибыль");
            t.SetToolTip(labelRevenuePerShape, "Доход на акцию");
            t.SetToolTip(labelEBITDA, "Прибыль компании до выплаты процентов, налогов и амортизации");
            t.SetToolTip(labelBookValPShape, "Балансовая стоимость на акцию");
            t.SetToolTip(labelLeveredFreeCF, "Свободный денежный поток");
            t.SetToolTip(labelOperCashFlow, "Операционный денежный поток");
            t.SetToolTip(labelSharesCount, "Кол-во акций в обращении");

            //Russia
            t.SetToolTip(labelProfitPerShare, "Прибыль на акцию за последние 12 месяцев к аналогичному периоду год назад");
            panelUSACoefs.Visible = panelRussiaCoefs.Visible = false;
            panelRussiaCoefs.Top = panelUSACoefs.Top;
            panelRussiaCoefs.Left = panelUSACoefs.Left;
            panelRussiaCoefs.Width = panelUSACoefs.Width;
        }

        private void InitializeMainClass()
        {
            MainClass.Initialize();
            new Thread(() =>
            {
                MainClass.LoadStockListFromFile();
                LoadStockListsOnInit();
                _mSelectedList = _mTinkoffStocks;
            }).Start();
        }

        #region Methods:public

	    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
	    {
		    try{}
		    finally
		    {
			    MainClass.WriteStockListToFile();
		    }

		    try{}
		    finally
		    {
			    NLog.LogManager.Shutdown();
		    }
	    }

	    /// <summary>
        /// Заполнить форму данными
        /// </summary>
        /// <param name="getNewInfo">Надо ли загрузить новую информацию об акции с интернета</param>
        private async Task FillStockForm(bool getNewInfo = true)
        {
            if (comboBoxStocks.Text == "" || !comboBoxStocks.Items.Contains(comboBoxStocks.Text))
                return;
            _mSelectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
            if (_mSelectedStock == null)
                return;
            checkBoxIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
               await MainClass.GetStockData(_mSelectedStock);
            if (_mSelectedStock.Market.Location == StockMarketLocation.Usa)
            {
                linkLabel1.Text = Web.GetStockDataUrlUsa.Replace("{}", _mSelectedStock.Symbol) + _mSelectedStock.Symbol;
                panelUSACoefs.Visible = true;
                panelRussiaCoefs.Visible = false;
            }
            else if (_mSelectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(_mSelectedStock.Name))
                    linkLabel1.Text = Web.GetStockDataUrlRussia + MainClass.NamesToSymbolsRus[_mSelectedStock.Name];
                panelUSACoefs.Visible = false;
                panelRussiaCoefs.Visible = true;
            }
            // Получили

            labelStockName.Text = _mSelectedStock.Name;
            checkBoxIsStarred.Checked = _mSelectedStock.IsStarred;
            if (_mSelectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxStockPrice.Text = _mSelectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPriceUSD.Text = (_mSelectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            else if (_mSelectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxStockPriceUSD.Text = _mSelectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPrice.Text = (_mSelectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            textBoxStockSymbol.Text = _mSelectedStock.Symbol;
            textBoxStockLastUpdated.Text = _mSelectedStock.LastUpdate.ToString(CultureInfo.InvariantCulture);
            textBoxPE.Text = _mSelectedStock.PriceToEquity.ToCuteStr();
            textBoxPS.Text = _mSelectedStock.PriceToSales.ToCuteStr();
            textBoxPBV.Text = _mSelectedStock.PriceToBook.ToCuteStr();
            textBoxEVEBITDA.Text = _mSelectedStock.EVtoEbitda.ToCuteStr();
            textBoxDebtEBITDA.Text = _mSelectedStock.DebtToEbitda.ToCuteStr();
            textBoxROE.Text = _mSelectedStock.Roe.ToCuteStr();
            textBoxEPS.Text = _mSelectedStock.Eps.ToCuteStr();
            if (_mSelectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxQEG.Text = _mSelectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = _mSelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = _mSelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = _mSelectedStock.GrossProfit.ToCuteStr();
                textBox5YValProfit.Text = _mSelectedStock.GrossProfit5Ya.ToCuteStr();
                textBoxProfitCoef.Text = _mSelectedStock.ProfitCoef.ToCuteStr();
                textBox5YProfitCoef.Text = _mSelectedStock.ProfitCoef5Ya.ToCuteStr();
                textBoxProfitPerShare.Text = _mSelectedStock.ProfitOn12MToAnalogYearAgo.ToCuteStr();
                textBoxGrowthPS5Y.Text = _mSelectedStock.GrowProfitPerShare5Y.ToCuteStr();
                textBoxGrowthCapCosts.Text = _mSelectedStock.CapExpenseGrow5Y.ToCuteStr();
                textBoxProfitMargin5Y.Text = _mSelectedStock.ProfitMarg5Ya.ToCuteStr();
                textBoxOperMargin5Y.Text = _mSelectedStock.OperMarg5Ya.ToCuteStr();
                textBoxQuickLiquidity.Text = _mSelectedStock.UrgentLiquidityCoef.ToCuteStr();
                textBoxCurrLiquidity.Text = _mSelectedStock.CurrentLiquidityCoef.ToCuteStr();
            }
            else if (_mSelectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxEVEBITDA.Text = _mSelectedStock.EVtoEbitda.ToCuteStr();
                textBoxMarketCap.Text = _mSelectedStock.MarketCap.ToCuteStr();
                textBoxQEG.Text = _mSelectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = _mSelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = _mSelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = _mSelectedStock.GrossProfit.ToCuteStr();
                textBoxEntVal.Text = _mSelectedStock.Ev.ToCuteStr();
                textBoxPEG.Text = _mSelectedStock.Peg.ToCuteStr();
                textBoxEVRev.Text = _mSelectedStock.EvRev.ToCuteStr();
                textBoxRetOnAssets.Text = _mSelectedStock.RetOnAssets.ToCuteStr();
                textBoxRevenue.Text = _mSelectedStock.Revenue.ToCuteStr();
                textBoxRevPerShape.Text = _mSelectedStock.RevPerShare.ToCuteStr();
                textBoxEBITDA.Text = _mSelectedStock.Ebitda.ToCuteStr();
                textBoxTotCash.Text = _mSelectedStock.TotalCash.ToCuteStr();
                textBoxTotCashPShape.Text = _mSelectedStock.TotalCashPerShare.ToCuteStr();
                textBoxTotDebt.Text = _mSelectedStock.TotalDebt.ToCuteStr();
                textBoxBookValPShape.Text = _mSelectedStock.BookValPerShare.ToCuteStr();
                textBoxOperCashFlow.Text = _mSelectedStock.OperatingCashFlow.ToCuteStr();
                textBoxLeveredFreeCF.Text = _mSelectedStock.LeveredFreeCashFlow.ToCuteStr();
                textBoxSharesCount.Text = _mSelectedStock.TotalShares.ToCuteStr();
                textBoxDebtEBITDA.Text = _mSelectedStock.DebtToEbitda.ToCuteStr();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            labelMainPE.Text = @"MainPE: " + _mSelectedStock.RateMainPe;
            labelMain.Text = @"Main: " + _mSelectedStock.RateMain;
            labelMainAll.Text = @"MainAll: " + _mSelectedStock.RateMainAll;

        }

        private async void ButtonGetInfo_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            SetButtonsMode(false);
            stopwatch.Start();
            await FillStockForm();
            stopwatch.Stop();

            MainClass.WriteLog("Запрос занял " + stopwatch.Elapsed.TotalMilliseconds.ToString("F0") + " мс");
            SetButtonsMode(true);
#pragma warning disable 4014
	        Task.Factory.StartNew(async () =>
#pragma warning restore 4014
	        {
		        await Task.Delay(2000);
                labelDone.BeginInvoke((MethodInvoker)(delegate { labelDone.Visible = false; }));
            });
        }

        private async void ComboBoxStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
           await FillStockForm(false);
        }

        private void ComboBoxStocks_TextChanged(object sender, EventArgs e)
        {
        }

        private async void ButtonLoadAllStocksClick(object sender, EventArgs e)
        {
            comboBoxStocks.Items.Clear();
            labelRemainingTime.Text = @"Время загрузки порядка 15 с.";
            SetButtonsMode(false);
            comboBoxStocks.Text = "";
            _mTinkoffStocks.Clear();
            _mRussianStocks.Clear();
            _mUsaStocks.Clear();
            _mLondonStocks.Clear();
            _mBestStocksNames.Clear();
            foreach (var st in _mBestStocks)
                _mBestStocksNames.Add(st.Name);
            _mBestStocks.Clear();

            await Task.Run(() =>
               {
                   Stopwatch stopwa = Stopwatch.StartNew();
                   MainClass.Stocks.Clear();

                   try
                   {
                       MainClass.GetStocksList(labelRemainingTime, progressBar).Wait();
                   }
                   finally
                   {
                       MainClass.WriteStockListToFile();
                   }
                   FillStockLists();
                   stopwa.Stop();
                   MainClass.WriteLog("Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с");
               });
            SetButtonsMode(true);
            MessageBox.Show(@"Список загружен");
        }

        /// <summary>
        /// Заполнить список акций
        /// </summary>
        private void FillStockLists()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                if (_mBestStocksNames.Contains(selectedStock.Name))
                    selectedStock.IsStarred = true;
                ReferStock(selectedStock);
                if (SelectedStockIsInSelectedList(selectedStock))
                    comboBoxStocks.BeginInvoke((MethodInvoker)delegate { comboBoxStocks.Items.Add(selectedStock.FullName); });
            }
            labelStockCount.BeginInvoke((MethodInvoker)delegate { labelStockCount.Text = @"Общее кол-во: " + comboBoxStocks.Items.Count; });
        }

        /// <summary>
        /// Загружает список при инициализации
        /// </summary>
        private void LoadStockListsOnInit()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                ReferStock(selectedStock);
                if (SelectedStockIsInSelectedList(selectedStock))
                    comboBoxStocks.BeginInvoke((MethodInvoker)delegate { comboBoxStocks.Items.Add(selectedStock.FullName); });
            }
            labelStockCount.BeginInvoke((MethodInvoker)delegate { labelStockCount.Text = @"Общее кол-во: " + comboBoxStocks.Items.Count; });
        }

        /// <summary>
        /// Относится ли акция к выбранному списку
        /// </summary>
        /// <param name="st">Акция</param>
        private bool SelectedStockIsInSelectedList(Stock st)
        {
            if (radioButtonStarred.Checked)
                return st.IsStarred;
            if (radioButtonAllStocks.Checked)
                return true;
            if (radioButtonRusStocks.Checked)
                return st.Market.Location == StockMarketLocation.Russia;
            if (radioButtonFromTinkoff.Checked)
                return st.IsOnTinkoff;
            if (radioButtonUSAStocks.Checked)
                return st.Market.Location == StockMarketLocation.Usa;
            if (radioButtonLondonStocks.Checked)
                return st.Market.Location == StockMarketLocation.London;
            return false;
        }

        /// <summary>
        /// Отнесем акцию к определенному списку
        /// </summary>
        /// <param name="st">Акция</param>
        private void ReferStock(Stock st)
        {
            if (st.IsStarred && !_mBestStocks.Contains(st))
                _mBestStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                _mRussianStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Usa)
                _mUsaStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                _mLondonStocks.Add(st);
            if (st.IsOnTinkoff)
                _mTinkoffStocks.Add(st);
        }
        private void LoadNewListInComboBox(List<Stock> list)
        {
            comboBoxStocks.Items.Clear();
            comboBoxStocks.Text = "";
            foreach (var st in list)
                comboBoxStocks.Items.Add(st.FullName);
            comboBoxStocks.DropDownHeight = comboBoxStocks.Items.Count == 0 ? 1 : 300;
            labelStockCount.Text = @"Общее кол-во: " + comboBoxStocks.Items.Count;
        }

        private void RadioButtonStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStarred.Checked)
                LoadNewListInComboBox(_mBestStocks);
            _mSelectedList = _mBestStocks;
        }

        private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllStocks.Checked)
                LoadNewListInComboBox(MainClass.Stocks);
            _mSelectedList = MainClass.Stocks;
        }

        private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRusStocks.Checked)
                LoadNewListInComboBox(_mRussianStocks);
            _mSelectedList = _mRussianStocks;
        }

        private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSAStocks.Checked)
                LoadNewListInComboBox(_mUsaStocks);
            _mSelectedList = _mUsaStocks;
        }

        private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLondonStocks.Checked)
                LoadNewListInComboBox(_mLondonStocks);
        }

        private void comboBoxStocks_DropDown(object sender, EventArgs e)
        {
            //if (comboboxselectedStocks.items.count == 0)
            //    comboboxselectedStocks.all
            //    comboboxselectedStocks.droppeddown = false;
        }

        private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (_mSelectedStock != null && _mSelectedStock.IsStarred != checkBoxIsStarred.Checked)
            {
                MainClass.GetStock(true, _mSelectedStock.FullName).IsStarred = _mSelectedStock.IsStarred = checkBoxIsStarred.Checked;
                if (_mSelectedStock.IsStarred)
                {
                    _mBestStocks.Add(_mSelectedStock);
                }
                else
                {
                    _mBestStocks.Remove(_mSelectedStock);
                }
                if (radioButtonStarred.Checked)
                    LoadNewListInComboBox(_mBestStocks);
            }
        }

        private void ButtonSaveHistory_Click(object sender, EventArgs e)
        {
            MainClass.WriteStockListToFile($"History_file_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.dat");
            MessageBox.Show(@"Сохранено");
        }

        private void ButtonLoadHistoryFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string filePath = openFileDialog1.FileName;
            if (filePath != "")
                MainClass.LoadStockListFromFile(filePath);
            MainClass.WriteLog("Загружен файл " + filePath);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void ButtonLoadStockMultiplicators_Click(object sender, EventArgs e)
        {
            if (_mSelectedList == null)
            {
                _mSelectedList = _mBestStocks;
                if (radioButtonAllStocks.Checked)
                    _mSelectedList = MainClass.Stocks;
                if (radioButtonRusStocks.Checked)
                    _mSelectedList = _mRussianStocks;
                if (radioButtonUSAStocks.Checked)
                    _mSelectedList = _mUsaStocks;
                if (radioButtonLondonStocks.Checked)
                    _mSelectedList = _mLondonStocks;
                if (radioButtonFromTinkoff.Checked)
                    _mSelectedList = _mTinkoffStocks;
            }

            SetButtonsMode(false);
            buttonOpenReport.Enabled = false;

	        Stopwatch stopwatch = Stopwatch.StartNew();
	        try
	        {
		        MainClass.LoadStocksData(_mSelectedList, labelRemainingTime, progressBar);
	        }
	        finally
	        {
		        MainClass.WriteStockListToFile();
	        }
	        stopwatch.Stop();
	        MainClass.WriteLog($"Операция заняла {stopwatch.Elapsed.TotalSeconds:F0} с");
	        MainClass.MakeReportAndSaveToFile(_mSelectedList);

            SetButtonsMode(true);
            buttonOpenReport.Enabled = true;
        }

        /// <summary>
        /// Ставит доступность кнопок
        /// </summary>
        /// <param name="enabled">Доступны</param>
        private void SetButtonsMode(bool enabled)
        {
            buttonAnalyzeMultiplicators.Enabled = buttonCkechTinkoff.Enabled = panelMain.Enabled = comboBoxStocks.Enabled =
                buttonGetInfo.Enabled = buttonLoadAllStocks.Enabled = buttonSaveHistory.Enabled =
                buttonLoadStocksMultiplicators.Enabled = buttonLoadHistoryFile.Enabled =
                radioButtonStarred.Enabled = radioButtonAllStocks.Enabled = radioButtonRusStocks.Enabled =
                radioButtonUSAStocks.Enabled = radioButtonFromTinkoff.Enabled = enabled;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo(linkLabel1.Text);
            Process.Start(sInfo);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxRetOnAssets_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonOpenReport_Click(object sender, EventArgs e)
        {
            if (MainClass.ReportFileName != "")
            {
                Process.Start(MainClass.ReportFileName);
            }
        }

        private async void ButtonAnalyzeMultiplicators_Click(object sender, EventArgs e)
        {
            SetButtonsMode(false);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Run(() =>
            {
                Analyzer.Analyze(MainClass.Stocks, labelRemainingTime);
            });

            stopwatch.Stop();
            MainClass.WriteLog($"Операция заняла {stopwatch.Elapsed.TotalSeconds:F0} с");
            SetButtonsMode(true);
        }


        /// <summary>
        /// Сортирует список по указанной метрике
        /// </summary>
        /// <param name="regime">0-MainPE, 1-Main, 2-MainAll</param>
        /// <param name="lst">Список акций</param>
        private void SortList(int regime, List<Stock> lst)
        {
            comboBoxStocks.Items.Clear();
            for (var i = 0; i < lst.Count; i++)
                for (var j = 0; j < lst.Count - i - 1; j++)
                    if ((regime == 0 && lst[j].RateMainPe > lst[j + 1].RateMainPe) || (regime == 1 && lst[j].RateMain > lst[j + 1].RateMain) || (regime == 2 && lst[j].RateMainAll > lst[j + 1].RateMainAll))
                    {
                        var st = lst[j];
                        lst[j] = lst[j + 1];
                        lst[j + 1] = st;
                    }
            foreach (var st in lst)
                comboBoxStocks.Items.Add(st.FullName);
        }
        private void RadioButtonMainPE_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainPE.Checked)
                SortList(0, _mSelectedList);
        }

        private void RadioButtonMain_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMain.Checked)
                SortList(1, _mSelectedList);

        }

        private void RadioButtonMainAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainAll.Checked)
                SortList(2, _mSelectedList);
        }

        private void RadioButtonFromTinkoff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFromTinkoff.Checked)
            {
                LoadNewListInComboBox(_mTinkoffStocks);
                _mSelectedList = _mTinkoffStocks;
            }
        }

        #endregion

        private async void buttonCkechTinkoff_Click(object sender, EventArgs e)
        {
            await MainClass.GetStocksList(labelRemainingTime, progressBar, false);
        }
    }
}
