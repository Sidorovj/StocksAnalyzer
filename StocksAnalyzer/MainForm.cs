using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

namespace StocksAnalyzer
{

    public partial class MainForm : Form
    {
        private readonly List<Stock> m_mRussianStocks = new List<Stock>();
        private readonly List<Stock> m_mUsaStocks = new List<Stock>();
        private readonly List<Stock> m_mLondonStocks = new List<Stock>();
        private readonly List<Stock> m_mBestStocks = new List<Stock>();
        private readonly List<Stock> m_mTinkoffStocks = new List<Stock>();
        private readonly List<string> m_mBestStocksNames = new List<string>();
        private List<Stock> m_mSelectedList;
        private Stock m_mSelectedStock;

        public MainForm()
        {
            InitializeComponent();

            InitializeMainClass();

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
            MainClass.LoadStockListFromFile();
            LoadStockListsOnInit();
            m_mSelectedList = m_mTinkoffStocks;
        }

        #region Methods:public

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try { }
            finally
            {
                MainClass.WriteStockListToFile();
            }

            try { }
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
            m_mSelectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
            if (m_mSelectedStock == null)
                return;
            checkBoxIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
                await MainClass.GetStockData(m_mSelectedStock);
            if (m_mSelectedStock.Market.Location == StockMarketLocation.Usa)
            {
                linkLabel1.Text = Web.GetStockDataUrlUsa.Replace("{}", m_mSelectedStock.Symbol) + m_mSelectedStock.Symbol;
                panelUSACoefs.Visible = true;
                panelRussiaCoefs.Visible = false;
            }
            else if (m_mSelectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(m_mSelectedStock.Name))
                    linkLabel1.Text = Web.GetStockDataUrlRussia + MainClass.NamesToSymbolsRus[m_mSelectedStock.Name];
                panelUSACoefs.Visible = false;
                panelRussiaCoefs.Visible = true;
            }
            // Получили

            labelStockName.Text = m_mSelectedStock.Name;
            checkBoxIsStarred.Checked = m_mSelectedStock.IsStarred;
            if (m_mSelectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxStockPrice.Text = m_mSelectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPriceUSD.Text = (m_mSelectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            else if (m_mSelectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxStockPriceUSD.Text = m_mSelectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPrice.Text = (m_mSelectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            textBoxStockSymbol.Text = m_mSelectedStock.Symbol;
            textBoxStockLastUpdated.Text = m_mSelectedStock.LastUpdate.ToString(CultureInfo.InvariantCulture);
            textBoxPE.Text = m_mSelectedStock.PriceToEquity.ToCuteStr();
            textBoxPS.Text = m_mSelectedStock.PriceToSales.ToCuteStr();
            textBoxPBV.Text = m_mSelectedStock.PriceToBook.ToCuteStr();
            textBoxEVEBITDA.Text = m_mSelectedStock.EVtoEbitda.ToCuteStr();
            textBoxDebtEBITDA.Text = m_mSelectedStock.DebtToEbitda.ToCuteStr();
            textBoxROE.Text = m_mSelectedStock.Roe.ToCuteStr();
            textBoxEPS.Text = m_mSelectedStock.Eps.ToCuteStr();
            if (m_mSelectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxQEG.Text = m_mSelectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = m_mSelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = m_mSelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = m_mSelectedStock.GrossProfit.ToCuteStr();
                textBox5YValProfit.Text = m_mSelectedStock.GrossProfit5Ya.ToCuteStr();
                textBoxProfitCoef.Text = m_mSelectedStock.ProfitCoef.ToCuteStr();
                textBox5YProfitCoef.Text = m_mSelectedStock.ProfitCoef5Ya.ToCuteStr();
                textBoxProfitPerShare.Text = m_mSelectedStock.ProfitOn12MToAnalogYearAgo.ToCuteStr();
                textBoxGrowthPS5Y.Text = m_mSelectedStock.GrowProfitPerShare5Y.ToCuteStr();
                textBoxGrowthCapCosts.Text = m_mSelectedStock.CapExpenseGrow5Y.ToCuteStr();
                textBoxProfitMargin5Y.Text = m_mSelectedStock.ProfitMarg5Ya.ToCuteStr();
                textBoxOperMargin5Y.Text = m_mSelectedStock.OperMarg5Ya.ToCuteStr();
                textBoxQuickLiquidity.Text = m_mSelectedStock.UrgentLiquidityCoef.ToCuteStr();
                textBoxCurrLiquidity.Text = m_mSelectedStock.CurrentLiquidityCoef.ToCuteStr();
            }
            else if (m_mSelectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxEVEBITDA.Text = m_mSelectedStock.EVtoEbitda.ToCuteStr();
                textBoxMarketCap.Text = m_mSelectedStock.MarketCap.ToCuteStr();
                textBoxQEG.Text = m_mSelectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = m_mSelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = m_mSelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = m_mSelectedStock.GrossProfit.ToCuteStr();
                textBoxEntVal.Text = m_mSelectedStock.Ev.ToCuteStr();
                textBoxPEG.Text = m_mSelectedStock.Peg.ToCuteStr();
                textBoxEVRev.Text = m_mSelectedStock.EvRev.ToCuteStr();
                textBoxRetOnAssets.Text = m_mSelectedStock.RetOnAssets.ToCuteStr();
                textBoxRevenue.Text = m_mSelectedStock.Revenue.ToCuteStr();
                textBoxRevPerShape.Text = m_mSelectedStock.RevPerShare.ToCuteStr();
                textBoxEBITDA.Text = m_mSelectedStock.Ebitda.ToCuteStr();
                textBoxTotCash.Text = m_mSelectedStock.TotalCash.ToCuteStr();
                textBoxTotCashPShape.Text = m_mSelectedStock.TotalCashPerShare.ToCuteStr();
                textBoxTotDebt.Text = m_mSelectedStock.TotalDebt.ToCuteStr();
                textBoxBookValPShape.Text = m_mSelectedStock.BookValPerShare.ToCuteStr();
                textBoxOperCashFlow.Text = m_mSelectedStock.OperatingCashFlow.ToCuteStr();
                textBoxLeveredFreeCF.Text = m_mSelectedStock.LeveredFreeCashFlow.ToCuteStr();
                textBoxSharesCount.Text = m_mSelectedStock.TotalShares.ToCuteStr();
                textBoxDebtEBITDA.Text = m_mSelectedStock.DebtToEbitda.ToCuteStr();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            labelMainPE.Text = @"MainPE: " + m_mSelectedStock.RateMainPe;
            labelMain.Text = @"Main: " + m_mSelectedStock.RateMain;
            labelMainAll.Text = @"MainAll: " + m_mSelectedStock.RateMainAll;

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

            await Task.Delay(2000);
            labelDone.Visible = false;
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
            m_mTinkoffStocks.Clear();
            m_mRussianStocks.Clear();
            m_mUsaStocks.Clear();
            m_mLondonStocks.Clear();
            m_mBestStocksNames.Clear();
            foreach (var st in m_mBestStocks)
                m_mBestStocksNames.Add(st.Name);
            m_mBestStocks.Clear();

            Stopwatch stopwa = Stopwatch.StartNew();
            MainClass.Stocks.Clear();

            try
            {
                await MainClass.GetStocksList(labelRemainingTime, progressBar);
            }
            finally
            {
                MainClass.WriteStockListToFile();
            }
            FillStockLists();
            stopwa.Stop();

            MainClass.WriteLog("Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с");
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
                if (m_mBestStocksNames.Contains(selectedStock.Name))
                    selectedStock.IsStarred = true;
                ReferStock(selectedStock);
                if (SelectedStockIsInSelectedList(selectedStock))
                    comboBoxStocks.Items.Add(selectedStock.FullName);
            }
            labelStockCount.Text = @"Общее кол-во: " + comboBoxStocks.Items.Count;
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
                    comboBoxStocks.Items.Add(selectedStock.FullName);
            }
            labelStockCount.Text = @"Общее кол-во: " + comboBoxStocks.Items.Count;
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
            if (st.IsStarred && !m_mBestStocks.Contains(st))
                m_mBestStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                m_mRussianStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Usa)
                m_mUsaStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                m_mLondonStocks.Add(st);
            if (st.IsOnTinkoff)
                m_mTinkoffStocks.Add(st);
        }
        private void LoadNewListInComboBox(IEnumerable<Stock> list)
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
                LoadNewListInComboBox(m_mBestStocks);
            m_mSelectedList = m_mBestStocks;
        }

        private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllStocks.Checked)
                LoadNewListInComboBox(MainClass.Stocks);
            m_mSelectedList = MainClass.Stocks;
        }

        private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRusStocks.Checked)
                LoadNewListInComboBox(m_mRussianStocks);
            m_mSelectedList = m_mRussianStocks;
        }

        private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSAStocks.Checked)
                LoadNewListInComboBox(m_mUsaStocks);
            m_mSelectedList = m_mUsaStocks;
        }

        private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLondonStocks.Checked)
                LoadNewListInComboBox(m_mLondonStocks);
        }

        private void comboBoxStocks_DropDown(object sender, EventArgs e)
        {
        }

        private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (m_mSelectedStock != null && m_mSelectedStock.IsStarred != checkBoxIsStarred.Checked)
            {
                MainClass.GetStock(true, m_mSelectedStock.FullName).IsStarred = m_mSelectedStock.IsStarred = checkBoxIsStarred.Checked;
                if (m_mSelectedStock.IsStarred)
                {
                    m_mBestStocks.Add(m_mSelectedStock);
                }
                else
                {
                    m_mBestStocks.Remove(m_mSelectedStock);
                }
                if (radioButtonStarred.Checked)
                    LoadNewListInComboBox(m_mBestStocks);
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

        private async void ButtonLoadStockMultiplicators_Click(object sender, EventArgs e)
        {
            if (m_mSelectedList == null)
            {
                m_mSelectedList = m_mBestStocks;
                if (radioButtonAllStocks.Checked)
                    m_mSelectedList = MainClass.Stocks;
                if (radioButtonRusStocks.Checked)
                    m_mSelectedList = m_mRussianStocks;
                if (radioButtonUSAStocks.Checked)
                    m_mSelectedList = m_mUsaStocks;
                if (radioButtonLondonStocks.Checked)
                    m_mSelectedList = m_mLondonStocks;
                if (radioButtonFromTinkoff.Checked)
                    m_mSelectedList = m_mTinkoffStocks;
            }

            SetButtonsMode(false);
            buttonOpenReport.Enabled = false;

            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                await MainClass.LoadStocksData(m_mSelectedList, labelRemainingTime, progressBar);
            }
            finally
            {
                MainClass.WriteStockListToFile();
            }
            stopwatch.Stop();
            MainClass.WriteLog($"Операция заняла {stopwatch.Elapsed.TotalSeconds:F0} с");
            MainClass.MakeReportAndSaveToFile(m_mSelectedList);

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
            // TODO: переделать сортировку
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
                SortList(0, m_mSelectedList);
        }

        private void RadioButtonMain_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMain.Checked)
                SortList(1, m_mSelectedList);

        }

        private void RadioButtonMainAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainAll.Checked)
                SortList(2, m_mSelectedList);
        }

        private void RadioButtonFromTinkoff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFromTinkoff.Checked)
            {
                LoadNewListInComboBox(m_mTinkoffStocks);
                m_mSelectedList = m_mTinkoffStocks;
            }
        }

        #endregion

        private async void buttonCkechTinkoff_Click(object sender, EventArgs e)
        {
            await MainClass.GetStocksList(labelRemainingTime, progressBar, false);
        }
    }
}
