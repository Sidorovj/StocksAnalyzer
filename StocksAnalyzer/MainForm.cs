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
        private List<Stock> russian = new List<Stock>();
        private List<Stock> russian2 = new List<Stock>();
        private List<Stock> usa = new List<Stock>();
        private List<Stock> london = new List<Stock>();
        private List<Stock> Best = new List<Stock>();
        private List<Stock> tinkoff = new List<Stock>();
        private List<string> bestNames = new List<string>();
        private List<Stock> other = new List<Stock>();
        private List<Stock> selectedList;
        private Stock selectedStock;
        public MainForm()
        {
            InitializeComponent();
            Best = new List<Stock>();
            MainClass.LoadStockListFromFile();
            fillStockListsOnInit();
            this.FormClosing += Form1_FormClosing;

            ToolTip t = new ToolTip();
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
            selectedList = MainClass.Stocks;
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainClass.WriteStockListToFile();
        }
        private void fillStockForm(bool getNewInfo = true)
        {
            if (comboBoxStocks.Text == "" || !comboBoxStocks.Items.Contains(comboBoxStocks.Text))
                return;
            selectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
            if (selectedStock == null)
                return;
            checkIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
                MainClass.GetStockData(selectedStock);
            if (selectedStock.Market.Location == StockMarketLocation.USA)
            {
                linkLabel1.Text = Web.GetStockDataUrl_USA.Replace("{}", selectedStock.Symbol) + selectedStock.Symbol;
                panelUSACoefs.Visible = true;
                panelRussiaCoefs.Visible = false;
            }
            else if (selectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(selectedStock.Name))
                    linkLabel1.Text = Web.GetStockDataUrl_Russia + MainClass.NamesToSymbolsRus[selectedStock.Name];
                panelUSACoefs.Visible = false;
                panelRussiaCoefs.Visible = true;
            }
            // Получили

            labelStockName.Text = selectedStock.Name;
            checkIsStarred.Checked = selectedStock.IsStarred;
            if (selectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxStockPrice.Text = selectedStock.Price.ToString();
                textBoxStockPriceUSD.Text = (selectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            else
            {
                textBoxStockPriceUSD.Text = selectedStock.Price.ToString();
                textBoxStockPrice.Text = (selectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            textBoxStockSymbol.Text = selectedStock.Symbol;
            textBoxStockLastUpdated.Text = selectedStock.LastUpdate.ToString();
            textBoxPE.Text = selectedStock.PriceToEquity.ToCuteStr();
            textBoxPS.Text = selectedStock.PriceToSales.ToCuteStr();
            textBoxPBV.Text = selectedStock.PriceToBook.ToCuteStr();
            textBoxEVEBITDA.Text = selectedStock.EVtoEBITDA.ToCuteStr();
            textBoxDebtEBITDA.Text = selectedStock.DebtToEBITDA.ToCuteStr();
            textBoxROE.Text = selectedStock.ROE.ToCuteStr();
            textBoxEPS.Text = selectedStock.EPS.ToCuteStr();
            if (selectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxQEG.Text = selectedStock.QEG.ToCuteStr();
                textBoxProfitMargin.Text = selectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = selectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = selectedStock.GrossProfit.ToCuteStr();
                textBox5YValProfit.Text = selectedStock.GrossProfit5ya.ToCuteStr();
                textBoxProfitCoef.Text = selectedStock.ProfitCoef.ToCuteStr();
                textBox5YProfitCoef.Text = selectedStock.ProfitCoef5ya.ToCuteStr();
                textBoxProfitPerShare.Text = selectedStock.ProfitOn12mToAnalogYearAgo.ToCuteStr();
                textBoxGrowthPS5Y.Text = selectedStock.GrowProfitPerShare5y.ToCuteStr();
                textBoxGrowthCapCosts.Text = selectedStock.CapExpenseGrow5y.ToCuteStr();
                textBoxProfitMargin5Y.Text = selectedStock.ProfitMarg5ya.ToCuteStr();
                textBoxOperMargin5Y.Text = selectedStock.OperMarg5ya.ToCuteStr();
                textBoxQuickLiquidity.Text = selectedStock.UrgentLiquidityCoef.ToCuteStr();
                textBoxCurrLiquidity.Text = selectedStock.CurrentLiquidityCoef.ToCuteStr();
            }
            else
            {
                textBoxEVEBITDA.Text = selectedStock.EVtoEBITDA.ToCuteStr();
                textBoxMarketCap.Text = selectedStock.MarketCap.ToCuteStr();
                textBoxQEG.Text = selectedStock.QEG.ToCuteStr();
                textBoxProfitMargin.Text = selectedStock.ProfitMarg.ToCuteStr();
                textBoxOperatingMargin.Text = selectedStock.OperMarg.ToCuteStr();
                textBoxGrossProfit.Text = selectedStock.GrossProfit.ToCuteStr();
                textBoxEntVal.Text = selectedStock.EV.ToCuteStr();
                textBoxPEG.Text = selectedStock.PEG.ToCuteStr();
                textBoxEVRev.Text = selectedStock.EVRev.ToCuteStr();
                textBoxRetOnAssets.Text = selectedStock.RetOnAssets.ToCuteStr();
                textBoxRevenue.Text = selectedStock.Revenue.ToCuteStr();
                textBoxRevPerShape.Text = selectedStock.RevPerShare.ToCuteStr();
                textBoxEBITDA.Text = selectedStock.EBITDA.ToCuteStr();
                textBoxTotCash.Text = selectedStock.TotalCash.ToCuteStr();
                textBoxTotCashPShape.Text = selectedStock.TotalCashPerShare.ToCuteStr();
                textBoxTotDebt.Text = selectedStock.TotalDebt.ToCuteStr();
                textBoxBookValPShape.Text = selectedStock.BookValPerShare.ToCuteStr();
                textBoxOperCashFlow.Text = selectedStock.OperatingCashFlow.ToCuteStr();
                textBoxLeveredFreeCF.Text = selectedStock.LeveredFreeCashFlow.ToCuteStr();
                textBoxSharesCount.Text = selectedStock.TotalShares.ToCuteStr();
                textBoxDebtEBITDA.Text = selectedStock.DebtToEBITDA.ToCuteStr();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            labelMainPE.Text = "MainPE: "+ selectedStock.RateMainPE.ToString();
            labelMain.Text = "Main: "+selectedStock.RateMain.ToString();
            labelMainAll.Text = "MainAll: "+selectedStock.RateMainAll.ToString();

        }
        private async void buttonGetInfo_Click(object sender, EventArgs e)
        {
            Stopwatch stopwa = new Stopwatch();
            setButtonsMode(false);
            stopwa.Start();
            fillStockForm();
            MainClass.WriteLog(new string[] { "Запрос занял " + stopwa.Elapsed.TotalMilliseconds.ToString("F0") + " мс" });
            stopwa.Stop();
            setButtonsMode(true);
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                this.labelDone.BeginInvoke((MethodInvoker)(delegate { labelDone.Visible = false; }));
            });
        }

        private void comboBoxStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            //selectedStock = null;
            //checkIsStarred.Enabled = false;
            fillStockForm(false);
            //else comboBoxStocks.DropDownHeight = 0;
        }

        private void comboBoxStocks_TextChanged(object sender, EventArgs e)
        {
            //if (!comboBoxStocks.Items.Contains(comboBoxStocks.Text) && comboBoxStocks.Items.Count > 0 && !comboBoxStocks.DroppedDown)
            //    comboBoxStocks.DroppedDown = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainClass.WriteStockListToFile();
            MessageBox.Show("Сохранено");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            comboBoxStocks.Items.Clear();
            labelRemainingTime.Text = "Время загрузки порядка 15 с.";
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            setButtonsMode(false);
            comboBoxStocks.Text = "";
            russian.Clear();
            usa.Clear();
            london.Clear();
            bestNames.Clear();
            foreach (var st in Best)
                bestNames.Add(st.Name);
            Best.Clear();
            await Task.Run(() =>
            {
                MainClass.Stocks.Clear();
                MainClass.GetStocksList();
                fillStockLists();
            });
            setButtonsMode(true);
            MainClass.WriteLog(new string[] { "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" });
            stopwa.Stop();
            MessageBox.Show("Список загружен");
        }

        private void fillStockLists()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                if (bestNames.Contains(selectedStock.Name))
                    selectedStock.IsStarred = true;
                referStock(selectedStock);
                if (selectedStockRegime(selectedStock))
                    this.comboBoxStocks.BeginInvoke((MethodInvoker)(delegate { this.comboBoxStocks.Items.Add(selectedStock.FullName); }));
            }
            this.labelStockCount.BeginInvoke((MethodInvoker)(delegate { labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count; }));
        }
        private void fillStockListsOnInit()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                referStock(selectedStock);
                if (selectedStockRegime(selectedStock))
                    this.comboBoxStocks.Items.Add(selectedStock.FullName);
            }
            labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
        }
        private bool selectedStockRegime(Stock st)
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
        private void referStock(Stock st) // Отнесем акцию к определенному списку
        {
            if (st.IsStarred && !Best.Contains(st))
                this.Best.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                this.russian.Add(st);
            if (st.Market.Location == StockMarketLocation.USA)
                this.usa.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                this.london.Add(st);
            if (st.IsOnTinkoff)
                tinkoff.Add(st);
        }
        private void changeList(List<Stock> list)
        {
            this.comboBoxStocks.Items.Clear();
            comboBoxStocks.Text = "";
            foreach (var st in list)
                this.comboBoxStocks.Items.Add(st.FullName);
            comboBoxStocks.DropDownHeight = comboBoxStocks.Items.Count == 0 ? 1 : 300;
            labelStockCount.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStarred.Checked)
                changeList(Best);
            selectedList = Best;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAllStocks.Checked)
                changeList(MainClass.Stocks);
            selectedList = MainClass.Stocks;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRusStocks.Checked)
                changeList(russian);
            selectedList = russian;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSAStocks.Checked)
                changeList(usa);
            selectedList = usa;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLondonStocks.Checked)
                changeList(london);
        }

        private void comboBoxStocks_DropDown(object sender, EventArgs e)
        {
            //if (comboboxselectedStocks.items.count == 0)
            //    comboboxselectedStocks.all
            //    comboboxselectedStocks.droppeddown = false;
        }

        private void checkIsStarred_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedStock != null && selectedStock.IsStarred != checkIsStarred.Checked)
            {
                MainClass.GetStock(true, selectedStock.FullName).IsStarred = selectedStock.IsStarred = checkIsStarred.Checked;
                if (selectedStock.IsStarred)
                {
                    Best.Add(selectedStock);
                }
                else
                {
                    Best.Remove(selectedStock);
                }
                if (radioButtonStarred.Checked)
                    changeList(Best);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MainClass.WriteStockListToFile("History_file_" + DateTime.Now.ToString().Replace('.', '-').Replace(' ', '_').Replace(':', '-') + ".dat");
            MessageBox.Show("Сохранено");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string s = openFileDialog1.FileName;
            if (s != "")
                MainClass.LoadStockListFromFile(s);
            MainClass.WriteLog("Загружен файл " + s);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string s;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var selList = Best;
            setButtonsMode(false); buttonOpenReport.Enabled = false;
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            //if (radioButton1.Checked)
            //   MainClass.loadStocksData(best);
            if (radioButtonAllStocks.Checked)
                selList = MainClass.Stocks;
            if (radioButtonRusStocks.Checked)
                selList = russian;
            if (radioButtonUSAStocks.Checked)
                selList = usa;
            if (radioButtonLondonStocks.Checked)
                selList = london;
            if (radioButtonFromTinkoff.Checked)
                selList = tinkoff;
            //MainClass.loadStocksData(selList);

            await Task.Factory.StartNew<string>(
                                                     () => MainClass.LoadStocksData(selList, this.labelRemainingTime, this.progressBar),
                                                     TaskCreationOptions.LongRunning);

            MainClass.WriteLog(new string[] { "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" });
            stopwa.Stop();
            setButtonsMode(true);
            MainClass.MakeReportAndSaveToFile(selList);
            buttonOpenReport.Enabled = true;
        }
        private void setButtonsMode(bool mode) // true - enabled, false - disabled
        {
           buttonAnalyzeMultiplicators.Enabled= panelMain.Enabled = comboBoxStocks.Enabled = buttonGetInfo.Enabled = buttonLoadAllStocks.Enabled = buttonSaveHistory.Enabled = buttonLoadStocksMultiplicators.Enabled = buttonLoadHistoryFile.Enabled = radioButtonStarred.Enabled = radioButtonAllStocks.Enabled = radioButtonRusStocks.Enabled = radioButtonUSAStocks.Enabled = mode;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (MainClass.ReportFileName != "")
            {
                Process.Start(MainClass.ReportFileName);
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            setButtonsMode(false);
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            await Task.Run(() =>
            {
                Analyzer.Analyze(MainClass.Stocks, this.labelRemainingTime);
            });

            MainClass.WriteLog( "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" );
            stopwa.Stop();
            setButtonsMode(true);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainPE.Checked)
                sortList(0, selectedList);
        }
        private void sortList(int regime, List<Stock> lst)
        {
            this.comboBoxStocks.Items.Clear();
            for (var i = 0; i < lst.Count; i++) 
                for (var j = 0; j < lst.Count - i - 1; j++)
                    if ((regime == 0 && lst[j].RateMainPE > lst[j + 1].RateMainPE)|| (regime == 1 && lst[j].RateMain > lst[j + 1].RateMain)|| (regime == 2 && lst[j].RateMainAll > lst[j + 1].RateMainAll))
                    {
                        var st = lst[j];
                        lst[j] = lst[j + 1];
                        lst[j + 1] = st;
                    }
            foreach (var st in lst)
                this.comboBoxStocks.Items.Add(st.FullName);
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMain.Checked)
                sortList(1, selectedList);

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMainAll.Checked)
                sortList(2, selectedList);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFromTinkoff.Checked)
            {
                changeList(tinkoff);
                selectedList = tinkoff;
            }
        }
    }
}
