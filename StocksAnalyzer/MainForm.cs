using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace StocksAnalyzer
{

    public partial class MainForm : Form
    {
        private List<Stock> RussianStocks = new List<Stock>();
        private List<Stock> UsaStocks = new List<Stock>();
        private List<Stock> LondonStocks = new List<Stock>();
        private List<Stock> BestStocks = new List<Stock>();
        private List<Stock> TinkoffStocks = new List<Stock>();
        private List<string> BestStocksNames = new List<string>();
        private List<Stock> SelectedList;
        private Stock SelectedStock;

        public MainForm()
        {
            InitializeComponent();
            BestStocks = new List<Stock>();
            MainClass.LoadStockListFromFile();
            LoadStockListsOnInit();
            this.FormClosing += Form1_FormClosing;

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
            SelectedList = MainClass.Stocks;
            /*int c1 = 0, c2 = 0;

            foreach (var st in MainClass.Stocks)
            {
                if (st.EV != 0 || st.EVRev != 0 || st.EVtoEBITDA != 0)
                    c1++;
                if (st.PriceToEquity != 0)
                    c2++;
            }
            richTextBoxLog.Text +='\n'+c1.ToString() + ' '+c2.ToString();*/
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
            SelectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
            if (SelectedStock == null)
                return;
            checkBoxIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
                MainClass.GetStockData(SelectedStock);
            if (SelectedStock.Market.Location == StockMarketLocation.USA)
            {
                linkLabel1.Text = Web.GetStockDataUrl_USA.Replace("{}", SelectedStock.Symbol) + SelectedStock.Symbol;
                panelUSACoefs.Visible = true;
                panelRussiaCoefs.Visible = false;
            }
            else if (SelectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(SelectedStock.Name))
                    linkLabel1.Text = Web.GetStockDataUrl_Russia + MainClass.NamesToSymbolsRus[SelectedStock.Name];
                panelUSACoefs.Visible = false;
                panelRussiaCoefs.Visible = true;
            }
            // Получили

            labelStockName.Text = SelectedStock.Name;
            checkBoxIsStarred.Checked = SelectedStock.IsStarred;
            if (SelectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxStockPrice.Text = SelectedStock.Price.ToString();
                textBoxStockPriceUSD.Text = (SelectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            else if (SelectedStock.Market.Currency == StockMarketCurrency.USD)
            {
                textBoxStockPriceUSD.Text = SelectedStock.Price.ToString();
                textBoxStockPrice.Text = (SelectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            textBoxStockSymbol.Text = SelectedStock.Symbol;
            textBoxStockLastUpdated.Text = SelectedStock.LastUpdate.ToString();
            textBoxPE.Text = SelectedStock.PriceToEquity.ToCuteStr();
            textBoxPS.Text = SelectedStock.PriceToSales.ToCuteStr();
            textBoxPBV.Text = SelectedStock.PriceToBook.ToCuteStr();
            textBoxEVEBITDA.Text = SelectedStock.EVtoEBITDA.ToCuteStr();
            textBoxDebtEBITDA.Text = SelectedStock.DebtToEBITDA.ToCuteStr();
            textBoxROE.Text = SelectedStock.ROE.ToCuteStr();
            textBoxEPS.Text = SelectedStock.EPS.ToCuteStr();
            if (SelectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxQEG.Text = SelectedStock.QEG.ToCuteStr();
                textBoxProfitMargin.Text = SelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = SelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = SelectedStock.GrossProfit.ToCuteStr();
                textBox5YValProfit.Text = SelectedStock.GrossProfit5ya.ToCuteStr();
                textBoxProfitCoef.Text = SelectedStock.ProfitCoef.ToCuteStr();
                textBox5YProfitCoef.Text = SelectedStock.ProfitCoef5ya.ToCuteStr();
                textBoxProfitPerShare.Text = SelectedStock.ProfitOn12mToAnalogYearAgo.ToCuteStr();
                textBoxGrowthPS5Y.Text = SelectedStock.GrowProfitPerShare5y.ToCuteStr();
                textBoxGrowthCapCosts.Text = SelectedStock.CapExpenseGrow5y.ToCuteStr();
                textBoxProfitMargin5Y.Text = SelectedStock.ProfitMarg5ya.ToCuteStr();
                textBoxOperMargin5Y.Text = SelectedStock.OperMarg5ya.ToCuteStr();
                textBoxQuickLiquidity.Text = SelectedStock.UrgentLiquidityCoef.ToCuteStr();
                textBoxCurrLiquidity.Text = SelectedStock.CurrentLiquidityCoef.ToCuteStr();
            }
            else if(SelectedStock.Market.Currency == StockMarketCurrency.USD)
            {
                textBoxEVEBITDA.Text = SelectedStock.EVtoEBITDA.ToCuteStr();
                textBoxMarketCap.Text = SelectedStock.MarketCap.ToCuteStr();
                textBoxQEG.Text = SelectedStock.QEG.ToCuteStr();
                textBoxProfitMargin.Text = SelectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = SelectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = SelectedStock.GrossProfit.ToCuteStr();
                textBoxEntVal.Text = SelectedStock.EV.ToCuteStr();
                textBoxPEG.Text = SelectedStock.PEG.ToCuteStr();
                textBoxEVRev.Text = SelectedStock.EVRev.ToCuteStr();
                textBoxRetOnAssets.Text = SelectedStock.RetOnAssets.ToCuteStr();
                textBoxRevenue.Text = SelectedStock.Revenue.ToCuteStr();
                textBoxRevPerShape.Text = SelectedStock.RevPerShare.ToCuteStr();
                textBoxEBITDA.Text = SelectedStock.EBITDA.ToCuteStr();
                textBoxTotCash.Text = SelectedStock.TotalCash.ToCuteStr();
                textBoxTotCashPShape.Text = SelectedStock.TotalCashPerShare.ToCuteStr();
                textBoxTotDebt.Text = SelectedStock.TotalDebt.ToCuteStr();
                textBoxBookValPShape.Text = SelectedStock.BookValPerShare.ToCuteStr();
                textBoxOperCashFlow.Text = SelectedStock.OperatingCashFlow.ToCuteStr();
                textBoxLeveredFreeCF.Text = SelectedStock.LeveredFreeCashFlow.ToCuteStr();
                textBoxSharesCount.Text = SelectedStock.TotalShares.ToCuteStr();
                textBoxDebtEBITDA.Text = SelectedStock.DebtToEBITDA.ToCuteStr();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            labelMainPE.Text = "MainPE: " + SelectedStock.RateMainPE.ToString();
            labelMain.Text = "Main: " + SelectedStock.RateMain.ToString();
            labelMainAll.Text = "MainAll: " + SelectedStock.RateMainAll.ToString();

        }

        private async void ButtonGetInfo_Click(object sender, EventArgs e)
        {
            Stopwatch _stopwatch = new Stopwatch();
            SetButtonsMode(false);
            _stopwatch.Start();
            FillStockForm();
            _stopwatch.Stop();

            MainClass.WriteLog(new string[] { "Запрос занял " + _stopwatch.Elapsed.TotalMilliseconds.ToString("F0") + " мс" });
            SetButtonsMode(true);
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                this.labelDone.BeginInvoke((MethodInvoker)(delegate { labelDone.Visible = false; }));
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
            comboBoxStocks.Items.Clear();
            labelRemainingTime.Text = "Время загрузки порядка 15 с.";
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            SetButtonsMode(false);
            comboBoxStocks.Text = "";
            RussianStocks.Clear();
            UsaStocks.Clear();
            LondonStocks.Clear();
            BestStocksNames.Clear();
            foreach (var st in BestStocks)
                BestStocksNames.Add(st.Name);
            BestStocks.Clear();
            await Task.Run(() =>
            {
                MainClass.Stocks.Clear();
                MainClass.GetStocksList();
                FillStockLists();
            });
            SetButtonsMode(true);
            MainClass.WriteLog(new string[] { "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" });
            stopwa.Stop();
            MessageBox.Show("Список загружен");
        }

        /// <summary>
        /// Заполнить список акций
        /// </summary>
        private void FillStockLists()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                if (BestStocksNames.Contains(selectedStock.Name))
                    selectedStock.IsStarred = true;
                ReferStock(selectedStock);
                if (SelectedStockIsInSelectedList(selectedStock))
                    this.comboBoxStocks.BeginInvoke((MethodInvoker)(delegate { this.comboBoxStocks.Items.Add(selectedStock.FullName); }));
            }
            this.labelStockCount.BeginInvoke((MethodInvoker)(delegate { labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count; }));
        }

        /// <summary>
        /// Загружает список при инициализации
        /// </summary>
        private void LoadStockListsOnInit()
        {
            foreach (var _selectedStock in MainClass.Stocks)
            {
                ReferStock(_selectedStock);
                if (SelectedStockIsInSelectedList(_selectedStock))
                    this.comboBoxStocks.Items.Add(_selectedStock.FullName);
            }
            labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
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
            if (radioButtonUSAStocks.Checked)
                return st.Market.Location == StockMarketLocation.USA;
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
            if (st.IsStarred && !BestStocks.Contains(st))
                this.BestStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                this.RussianStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.USA)
                this.UsaStocks.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                this.LondonStocks.Add(st);
            if (st.IsOnTinkoff)
                TinkoffStocks.Add(st);
        }
        private void LoadNewListInComboBox(List<Stock> list)
        {
            this.comboBoxStocks.Items.Clear();
            comboBoxStocks.Text = "";
            foreach (var st in list)
                this.comboBoxStocks.Items.Add(st.FullName);
            comboBoxStocks.DropDownHeight = comboBoxStocks.Items.Count == 0 ? 1 : 300;
            labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
        }

        private void RadioButtonStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStarred.Checked)
                LoadNewListInComboBox(BestStocks);
            SelectedList = BestStocks;
        }

        private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllStocks.Checked)
                LoadNewListInComboBox(MainClass.Stocks);
            SelectedList = MainClass.Stocks;
        }

        private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRusStocks.Checked)
                LoadNewListInComboBox(RussianStocks);
            SelectedList = RussianStocks;
        }

        private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSAStocks.Checked)
                LoadNewListInComboBox(UsaStocks);
            SelectedList = UsaStocks;
        }

        private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLondonStocks.Checked)
                LoadNewListInComboBox(LondonStocks);
        }

        private void comboBoxStocks_DropDown(object sender, EventArgs e)
        {
            //if (comboboxselectedStocks.items.count == 0)
            //    comboboxselectedStocks.all
            //    comboboxselectedStocks.droppeddown = false;
        }

        private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedStock != null && SelectedStock.IsStarred != checkBoxIsStarred.Checked)
            {
                MainClass.GetStock(true, SelectedStock.FullName).IsStarred = SelectedStock.IsStarred = checkBoxIsStarred.Checked;
                if (SelectedStock.IsStarred)
                {
                    BestStocks.Add(SelectedStock);
                }
                else
                {
                    BestStocks.Remove(SelectedStock);
                }
                if (radioButtonStarred.Checked)
                    LoadNewListInComboBox(BestStocks);
            }
        }

        private void ButtonSaveHistory_Click(object sender, EventArgs e)
        {
            MainClass.WriteStockListToFile($"History_file_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.dat");
            MessageBox.Show("Сохранено");
        }

        private void ButtonLoadHistoryFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string _filePath = openFileDialog1.FileName;
            if (_filePath != "")
                MainClass.LoadStockListFromFile(_filePath);
            MainClass.WriteLog("Загружен файл " + _filePath);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string s;
        }

        private async void ButtonLoadStockMultiplicators_Click(object sender, EventArgs e)
        {
            if (SelectedList == null)
            {
                SelectedList = BestStocks;
                if (radioButtonAllStocks.Checked)
                    SelectedList = MainClass.Stocks;
                if (radioButtonRusStocks.Checked)
                    SelectedList = RussianStocks;
                if (radioButtonUSAStocks.Checked)
                    SelectedList = UsaStocks;
                if (radioButtonLondonStocks.Checked)
                    SelectedList = LondonStocks;
                if (radioButtonFromTinkoff.Checked)
                    SelectedList = TinkoffStocks;
            }

            SetButtonsMode(false);
            buttonOpenReport.Enabled = false;
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            //if (radioButton1.Checked)
            //   MainClass.loadStocksData(best);
            //MainClass.loadStocksData(selList);

            await Task.Factory.StartNew<string>(
                                                     () => MainClass.LoadStocksData(SelectedList, this.labelRemainingTime, this.progressBar),
                                                     TaskCreationOptions.LongRunning);

            _stopwatch.Stop();
            MainClass.WriteLog($"Операция заняла {_stopwatch.Elapsed.TotalSeconds.ToString("F0")} с");
            SetButtonsMode(true);
            MainClass.MakeReportAndSaveToFile(SelectedList);
            buttonOpenReport.Enabled = true;
        }

        /// <summary>
        /// Ставит доступность кнопок
        /// </summary>
        /// <param name="enabled">Доступны</param>
        private void SetButtonsMode(bool enabled)
        {
            buttonAnalyzeMultiplicators.Enabled = panelMain.Enabled = comboBoxStocks.Enabled = 
                buttonGetInfo.Enabled = buttonLoadAllStocks.Enabled = buttonSaveHistory.Enabled = 
                buttonLoadStocksMultiplicators.Enabled = buttonLoadHistoryFile.Enabled = 
                radioButtonStarred.Enabled = radioButtonAllStocks.Enabled = radioButtonRusStocks.Enabled = 
                radioButtonUSAStocks.Enabled = enabled;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {           
            ProcessStartInfo _sInfo = new ProcessStartInfo(linkLabel1.Text);
            Process.Start(_sInfo);
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
            Stopwatch _stopwatch = new Stopwatch();
            _stopwatch.Start();
            await Task.Run(() =>
            {
                Analyzer.Analyze(MainClass.Stocks, this.labelRemainingTime);
            });

            _stopwatch.Stop();
            MainClass.WriteLog($"Операция заняла {_stopwatch.Elapsed.TotalSeconds.ToString("F0")} с");
            SetButtonsMode(true);
        }


        /// <summary>
        /// Сортирует список по указанной метрике
        /// </summary>
        /// <param name="regime">0-MainPE, 1-Main, 2-MainAll</param>
        /// <param name="lst">Список акций</param>
        private void SortList(int regime, List<Stock> lst)
        {
            this.comboBoxStocks.Items.Clear();
            for (var i = 0; i < lst.Count; i++)
                for (var j = 0; j < lst.Count - i - 1; j++)
                    if ((regime == 0 && lst[j].RateMainPE > lst[j + 1].RateMainPE) || (regime == 1 && lst[j].RateMain > lst[j + 1].RateMain) || (regime == 2 && lst[j].RateMainAll > lst[j + 1].RateMainAll))
                    {
                        var st = lst[j];
                        lst[j] = lst[j + 1];
                        lst[j + 1] = st;
                    }
            foreach (var st in lst)
                this.comboBoxStocks.Items.Add(st.FullName);
        }
        private void RadioButtonMainPE_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainPE.Checked)
                SortList(0, SelectedList);
        }

        private void RadioButtonMain_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMain.Checked)
                SortList(1, SelectedList);

        }

        private void RadioButtonMainAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainAll.Checked)
                SortList(2, SelectedList);
        }

        private void RadioButtonFromTinkoff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFromTinkoff.Checked)
            {
                LoadNewListInComboBox(TinkoffStocks);
                SelectedList = TinkoffStocks;
            }
        }

        #endregion
    }
}
