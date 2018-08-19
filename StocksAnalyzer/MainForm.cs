using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace StocksAnalyzer
{

    public partial class MainForm : Form
    {
        private readonly List<Stock> _russianStocks = new List<Stock>();
        private readonly List<Stock> _usaStocks = new List<Stock>();
        private readonly List<Stock> _londonStocks = new List<Stock>();
        private readonly List<Stock> _bestStocks = new List<Stock>();
        private readonly List<Stock> _tinkoffStocks = new List<Stock>();
        private readonly List<string> _bestStocksNames = new List<string>();
        private List<Stock> _selectedList;
        private Stock _selectedStock;

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
                _selectedList = _tinkoffStocks;
            }).Start();
        }

        #region Methods:public
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainClass.WriteStockListToFile();
        }

        /// <summary>
        /// Заполнить форму данными
        /// </summary>
        /// <param name="getNewInfo">Надо ли загрузить новую информацию об акции с интернета</param>
        private void FillStockForm(bool getNewInfo = true)
        {
            if (comboBoxStocks.Text == "" || !comboBoxStocks.Items.Contains(comboBoxStocks.Text))
                return;
            _selectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
            if (_selectedStock == null)
                return;
            checkBoxIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
                MainClass.GetStockData(_selectedStock);
            if (_selectedStock.Market.Location == StockMarketLocation.Usa)
            {
                linkLabel1.Text = Web.GetStockDataUrlUsa.Replace("{}", _selectedStock.Symbol) + _selectedStock.Symbol;
                panelUSACoefs.Visible = true;
                panelRussiaCoefs.Visible = false;
            }
            else if (_selectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(_selectedStock.Name))
                    linkLabel1.Text = Web.GetStockDataUrlRussia + MainClass.NamesToSymbolsRus[_selectedStock.Name];
                panelUSACoefs.Visible = false;
                panelRussiaCoefs.Visible = true;
            }
            // Получили

            labelStockName.Text = _selectedStock.Name;
            checkBoxIsStarred.Checked = _selectedStock.IsStarred;
            if (_selectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxStockPrice.Text = _selectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPriceUSD.Text = (_selectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            else if (_selectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxStockPriceUSD.Text = _selectedStock.Price.ToString(CultureInfo.InvariantCulture);
                textBoxStockPrice.Text = (_selectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
            }
            textBoxStockSymbol.Text = _selectedStock.Symbol;
            textBoxStockLastUpdated.Text = _selectedStock.LastUpdate.ToString(CultureInfo.InvariantCulture);
            textBoxPE.Text = _selectedStock.PriceToEquity.ToCuteStr();
            textBoxPS.Text = _selectedStock.PriceToSales.ToCuteStr();
            textBoxPBV.Text = _selectedStock.PriceToBook.ToCuteStr();
            textBoxEVEBITDA.Text = _selectedStock.EVtoEbitda.ToCuteStr();
            textBoxDebtEBITDA.Text = _selectedStock.DebtToEbitda.ToCuteStr();
            textBoxROE.Text = _selectedStock.Roe.ToCuteStr();
            textBoxEPS.Text = _selectedStock.Eps.ToCuteStr();
            if (_selectedStock.Market.Currency == StockMarketCurrency.Rub)
            {
                textBoxQEG.Text = _selectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = _selectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = _selectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = _selectedStock.GrossProfit.ToCuteStr();
                textBox5YValProfit.Text = _selectedStock.GrossProfit5Ya.ToCuteStr();
                textBoxProfitCoef.Text = _selectedStock.ProfitCoef.ToCuteStr();
                textBox5YProfitCoef.Text = _selectedStock.ProfitCoef5Ya.ToCuteStr();
                textBoxProfitPerShare.Text = _selectedStock.ProfitOn12MToAnalogYearAgo.ToCuteStr();
                textBoxGrowthPS5Y.Text = _selectedStock.GrowProfitPerShare5Y.ToCuteStr();
                textBoxGrowthCapCosts.Text = _selectedStock.CapExpenseGrow5Y.ToCuteStr();
                textBoxProfitMargin5Y.Text = _selectedStock.ProfitMarg5Ya.ToCuteStr();
                textBoxOperMargin5Y.Text = _selectedStock.OperMarg5Ya.ToCuteStr();
                textBoxQuickLiquidity.Text = _selectedStock.UrgentLiquidityCoef.ToCuteStr();
                textBoxCurrLiquidity.Text = _selectedStock.CurrentLiquidityCoef.ToCuteStr();
            }
            else if (_selectedStock.Market.Currency == StockMarketCurrency.Usd)
            {
                textBoxEVEBITDA.Text = _selectedStock.EVtoEbitda.ToCuteStr();
                textBoxMarketCap.Text = _selectedStock.MarketCap.ToCuteStr();
                textBoxQEG.Text = _selectedStock.Qeg.ToCuteStr();
                textBoxProfitMargin.Text = _selectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = _selectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = _selectedStock.GrossProfit.ToCuteStr();
                textBoxEntVal.Text = _selectedStock.Ev.ToCuteStr();
                textBoxPEG.Text = _selectedStock.Peg.ToCuteStr();
                textBoxEVRev.Text = _selectedStock.EvRev.ToCuteStr();
                textBoxRetOnAssets.Text = _selectedStock.RetOnAssets.ToCuteStr();
                textBoxRevenue.Text = _selectedStock.Revenue.ToCuteStr();
                textBoxRevPerShape.Text = _selectedStock.RevPerShare.ToCuteStr();
                textBoxEBITDA.Text = _selectedStock.Ebitda.ToCuteStr();
                textBoxTotCash.Text = _selectedStock.TotalCash.ToCuteStr();
                textBoxTotCashPShape.Text = _selectedStock.TotalCashPerShare.ToCuteStr();
                textBoxTotDebt.Text = _selectedStock.TotalDebt.ToCuteStr();
                textBoxBookValPShape.Text = _selectedStock.BookValPerShare.ToCuteStr();
                textBoxOperCashFlow.Text = _selectedStock.OperatingCashFlow.ToCuteStr();
                textBoxLeveredFreeCF.Text = _selectedStock.LeveredFreeCashFlow.ToCuteStr();
                textBoxSharesCount.Text = _selectedStock.TotalShares.ToCuteStr();
                textBoxDebtEBITDA.Text = _selectedStock.DebtToEbitda.ToCuteStr();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            labelMainPE.Text = @"MainPE: " + _selectedStock.RateMainPe;
            labelMain.Text = @"Main: " + _selectedStock.RateMain;
            labelMainAll.Text = @"MainAll: " + _selectedStock.RateMainAll;

        }

        private async void ButtonGetInfo_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            SetButtonsMode(false);
            stopwatch.Start();
            FillStockForm();
            stopwatch.Stop();

            MainClass.WriteLog("Запрос занял " + stopwatch.Elapsed.TotalMilliseconds.ToString("F0") + " мс");
            SetButtonsMode(true);
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                labelDone.BeginInvoke((MethodInvoker)(delegate { labelDone.Visible = false; }));
            });
        }

        private void ComboBoxStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedStock = null;
            //checkIsStarred.Enabled = false;
            FillStockForm(false);
            //else comboBoxStocks.DropDownHeight = 0;
        }

        private void ComboBoxStocks_TextChanged(object sender, EventArgs e)
        {
            //if (!comboBoxStocks.Items.Contains(comboBoxStocks.Text) && comboBoxStocks.Items.Count > 0 && !comboBoxStocks.DroppedDown)
            //    comboBoxStocks.DroppedDown = true;
        }

        private async void ButtonLoadAllStocksClick(object sender, EventArgs e)
        {
            //webBrowser1.ScriptErrorsSuppressed = true;
            //webBrowser1.Url =new Uri("http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download");

            //Task tr = new Task(() =>
            //{
            //    Thread.Sleep(3000);
            //    //SendKeys.SendWait("{Enter}");
            //});
            //tr.Start();

            comboBoxStocks.Items.Clear();
            labelRemainingTime.Text = @"Время загрузки порядка 15 с.";
            SetButtonsMode(false);
            comboBoxStocks.Text = "";
            _tinkoffStocks.Clear();
            _russianStocks.Clear();
            _usaStocks.Clear();
            _londonStocks.Clear();
            _bestStocksNames.Clear();
            foreach (var st in _bestStocks)
                _bestStocksNames.Add(st.Name);
            _bestStocks.Clear();

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
                if (_bestStocksNames.Contains(selectedStock.Name))
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
            if (st.IsStarred && !_bestStocks.Contains(st))
                _bestStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                _russianStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Usa)
                _usaStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                _londonStocks.Add(st);
            if (st.IsOnTinkoff)
                _tinkoffStocks.Add(st);
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
                LoadNewListInComboBox(_bestStocks);
            _selectedList = _bestStocks;
        }

        private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllStocks.Checked)
                LoadNewListInComboBox(MainClass.Stocks);
            _selectedList = MainClass.Stocks;
        }

        private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRusStocks.Checked)
                LoadNewListInComboBox(_russianStocks);
            _selectedList = _russianStocks;
        }

        private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSAStocks.Checked)
                LoadNewListInComboBox(_usaStocks);
            _selectedList = _usaStocks;
        }

        private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLondonStocks.Checked)
                LoadNewListInComboBox(_londonStocks);
        }

        private void comboBoxStocks_DropDown(object sender, EventArgs e)
        {
            //if (comboboxselectedStocks.items.count == 0)
            //    comboboxselectedStocks.all
            //    comboboxselectedStocks.droppeddown = false;
        }

        private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (_selectedStock != null && _selectedStock.IsStarred != checkBoxIsStarred.Checked)
            {
                MainClass.GetStock(true, _selectedStock.FullName).IsStarred = _selectedStock.IsStarred = checkBoxIsStarred.Checked;
                if (_selectedStock.IsStarred)
                {
                    _bestStocks.Add(_selectedStock);
                }
                else
                {
                    _bestStocks.Remove(_selectedStock);
                }
                if (radioButtonStarred.Checked)
                    LoadNewListInComboBox(_bestStocks);
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
            if (_selectedList == null)
            {
                _selectedList = _bestStocks;
                if (radioButtonAllStocks.Checked)
                    _selectedList = MainClass.Stocks;
                if (radioButtonRusStocks.Checked)
                    _selectedList = _russianStocks;
                if (radioButtonUSAStocks.Checked)
                    _selectedList = _usaStocks;
                if (radioButtonLondonStocks.Checked)
                    _selectedList = _londonStocks;
                if (radioButtonFromTinkoff.Checked)
                    _selectedList = _tinkoffStocks;
            }

            SetButtonsMode(false);
            buttonOpenReport.Enabled = false;
            //if (radioButton1.Checked)
            //   MainClass.loadStocksData(best);
            //MainClass.loadStocksData(selList);

            await Task.Run(
                () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    try
                    {
                        MainClass.LoadStocksData(_selectedList, labelRemainingTime, progressBar);
                    }
                    finally
                    {
                        MainClass.WriteStockListToFile();
                    }
                    stopwatch.Stop();
                    MainClass.WriteLog($"Операция заняла {stopwatch.Elapsed.TotalSeconds:F0} с");
                    MainClass.MakeReportAndSaveToFile(_selectedList);
                });
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
                SortList(0, _selectedList);
        }

        private void RadioButtonMain_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMain.Checked)
                SortList(1, _selectedList);

        }

        private void RadioButtonMainAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainAll.Checked)
                SortList(2, _selectedList);
        }

        private void RadioButtonFromTinkoff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFromTinkoff.Checked)
            {
                LoadNewListInComboBox(_tinkoffStocks);
                _selectedList = _tinkoffStocks;
            }
        }

        #endregion

        private async void buttonCkechTinkoff_Click(object sender, EventArgs e)
        {
            await MainClass.GetStocksList(labelRemainingTime, progressBar, false);
        }
    }
}
