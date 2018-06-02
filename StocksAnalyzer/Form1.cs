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

    public partial class Form1 : Form
    {
        private List<Stock> russian = new List<Stock>();
        private List<Stock> usa = new List<Stock>();
        private List<Stock> london = new List<Stock>();
        private List<Stock> best = new List<Stock>();
        private List<Stock> tinkoff = new List<Stock>();
        private List<string> bestNames = new List<string>();
        private List<Stock> other = new List<Stock>();
        private List<Stock> selectedList;
        private Stock selectedStock;
        public Form1()
        {
            InitializeComponent();
            MainClass.loadStockListFromFile();
            fillStockListsOnInit();
            this.FormClosing += Form1_FormClosing;

            ToolTip t = new ToolTip();
            t.SetToolTip(label5, "Тикет");
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
            t.SetToolTip(label15, "Прибыль на акцию за последние 12 месяцев к аналогичному периоду год назад");
            panel3.Visible = panel4.Visible = false;
            panel4.Top = panel3.Top;
            panel4.Left = panel3.Left;
            panel4.Width = panel3.Width;
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
            MainClass.writeStockListToFile();
        }
        private void fillStockForm(bool getNewInfo = true)
        {
            if (comboBoxStocks.Text == "" || !comboBoxStocks.Items.Contains(comboBoxStocks.Text))
                return;
            selectedStock = MainClass.getStock(true, comboBoxStocks.Text);
            if (selectedStock == null)
                return;
            checkIsStarred.Enabled = true;

            // Получим инфу об акции
            if (getNewInfo)
                MainClass.getStockData(selectedStock);
            if (selectedStock.Market.Location == StockMarketLocation.USA)
            {
                linkLabel1.Text = Web.getStockDataUrl_USA.Replace("{}", selectedStock.Symbol) + selectedStock.Symbol;
                panel3.Visible = true;
                panel4.Visible = false;
            }
            else if (selectedStock.Market.Location == StockMarketLocation.Russia)
            {
                if (MainClass.NamesToSymbolsRus.ContainsKey(selectedStock.Name))
                    linkLabel1.Text = Web.getStockDataUrl_Russia + MainClass.NamesToSymbolsRus[selectedStock.Name];
                panel3.Visible = false;
                panel4.Visible = true;
            }
            // Получили

            labelStockName.Text = selectedStock.Name;
            checkIsStarred.Checked = selectedStock.IsStarred;
            if (selectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxStockPrice.Text = selectedStock.Price.ToString();
                textBoxStockPriceUSD.Text = (selectedStock.Price / StockMarket.getExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            else
            {
                textBoxStockPriceUSD.Text = selectedStock.Price.ToString();
                textBoxStockPrice.Text = (selectedStock.Price * StockMarket.getExchangeRates(StockMarketCurrency.USD)).ToString("F2");
            }
            textBoxStockSymbol.Text = selectedStock.Symbol;
            textBoxStockLastUpdated.Text = selectedStock.LastUpdate.ToString();
            textBoxPE.Text = selectedStock.PriceToEquity.ToSTR();
            textBoxPS.Text = selectedStock.PriceToSales.ToSTR();
            textBoxPBV.Text = selectedStock.PriceToBook.ToSTR();
            textBoxEVEBITDA.Text = selectedStock.EVtoEBITDA.ToSTR();
            textBoxDebtEBITDA.Text = selectedStock.DebtToEBITDA.ToSTR();
            textBoxROE.Text = selectedStock.ROE.ToSTR();
            textBoxEPS.Text = selectedStock.EPS.ToSTR();
            if (selectedStock.Market.Currency == StockMarketCurrency.RUB)
            {
                textBoxQEG.Text = selectedStock.QEG.ToSTR();
                textBoxProfitMargin.Text = selectedStock.ProfitMarg.ToSTR();
                textBoxOperatingMargin.Text = selectedStock.OperMarg.ToSTR();
                textBoxGrossProfit.Text = selectedStock.GrossProfit.ToSTR();
                textBox12.Text = selectedStock.GrossProfit5ya.ToSTR();
                textBox11.Text = selectedStock.ProfitCoef.ToSTR();
                textBox10.Text = selectedStock.ProfitCoef5ya.ToSTR();
                textBox9.Text = selectedStock.ProfitOn12mToAnalogYearAgo.ToSTR();
                textBox8.Text = selectedStock.GrowProfitPerShare5y.ToSTR();
                textBox7.Text = selectedStock.CapExpenseGrow5y.ToSTR();
                textBox6.Text = selectedStock.ProfitMarg5ya.ToSTR();
                textBox5.Text = selectedStock.OperMarg5ya.ToSTR();
                textBox3.Text = selectedStock.UrgentLiquidityCoef.ToSTR();
                textBox2.Text = selectedStock.CurrentLiquidityCoef.ToSTR();
            }
            else
            {
                textBoxEVEBITDA.Text = selectedStock.EVtoEBITDA.ToSTR();
                textBoxMarketCap.Text = selectedStock.MarketCap.ToSTR();
                textBoxQEG.Text = selectedStock.QEG.ToSTR();
                textBoxProfitMargin.Text = selectedStock.ProfitMarg.ToSTR();
                textBoxOperatingMargin.Text = selectedStock.OperMarg.ToSTR();
                textBoxGrossProfit.Text = selectedStock.GrossProfit.ToSTR();
                textBoxEntVal.Text = selectedStock.EV.ToSTR();
                textBoxPEG.Text = selectedStock.PEG.ToSTR();
                textBoxEVRev.Text = selectedStock.EVRev.ToSTR();
                textBoxRetOnAssets.Text = selectedStock.RetOnAssets.ToSTR();
                textBoxRevenue.Text = selectedStock.Revenue.ToSTR();
                textBoxRevPerShape.Text = selectedStock.RevPerShare.ToSTR();
                textBoxEBITDA.Text = selectedStock.EBITDA.ToSTR();
                textBoxTotCash.Text = selectedStock.TotalCash.ToSTR();
                textBoxTotCashPShape.Text = selectedStock.TotalCashPerShare.ToSTR();
                textBoxTotDebt.Text = selectedStock.TotalDebt.ToSTR();
                textBoxBookValPShape.Text = selectedStock.BookValPerShare.ToSTR();
                textBoxOperCashFlow.Text = selectedStock.OperatingCashFlow.ToSTR();
                textBoxLeveredFreeCF.Text = selectedStock.LeveredFreeCashFlow.ToSTR();
                textBoxSharesCount.Text = selectedStock.TotalShares.ToSTR();
                textBoxDebtEBITDA.Text = selectedStock.DebtToEBITDA.ToSTR();
            }
            if (getNewInfo)
                labelDone.Visible = true;
            label21.Text = "MainPE: "+ selectedStock.RateMainPE.ToString();
            label22.Text = "Main: "+selectedStock.RateMain.ToString();
            label23.Text = "MainAll: "+selectedStock.RateMainAll.ToString();

        }
        private async void buttonGetInfo_Click(object sender, EventArgs e)
        {
            Stopwatch stopwa = new Stopwatch();
            setButtonsMode(false);
            stopwa.Start();
            fillStockForm();
            MainClass.writeLog(new string[] { "Запрос занял " + stopwa.Elapsed.TotalMilliseconds.ToString("F0") + " мс" });
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
            MainClass.writeStockListToFile();
            MessageBox.Show("Сохранено");
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            comboBoxStocks.Items.Clear();
            labelSelectParam.Text = "Время загрузки порядка 15 с.";
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            setButtonsMode(false);
            comboBoxStocks.Text = "";
            russian.Clear();
            usa.Clear();
            london.Clear();
            bestNames.Clear();
            foreach (var st in best)
                bestNames.Add(st.Name);
            best.Clear();
            await Task.Run(() =>
            {
                MainClass.Stocks.Clear();
                MainClass.getStocksList();
                fillStockLists();
            });
            setButtonsMode(true);
            MainClass.writeLog(new string[] { "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" });
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
            this.label1.BeginInvoke((MethodInvoker)(delegate { label1.Text = "Общее кол-во: " + comboBoxStocks.Items.Count; }));
        }
        private void fillStockListsOnInit()
        {
            foreach (var selectedStock in MainClass.Stocks)
            {
                referStock(selectedStock);
                if (selectedStockRegime(selectedStock))
                    this.comboBoxStocks.Items.Add(selectedStock.FullName);
            }
            label1.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
        }
        private bool selectedStockRegime(Stock st)
        {
            if (radioButton1.Checked)
                return st.IsStarred;
            if (radioButton2.Checked)
                return true;
            if (radioButton3.Checked)
                return st.Market.Location == StockMarketLocation.Russia;
            if (radioButton4.Checked)
                return st.Market.Location == StockMarketLocation.USA;
            if (radioButton5.Checked)
                return st.Market.Location == StockMarketLocation.London;
            return false;
        }
        private void referStock(Stock st) // Отнесем акцию к определенному списку
        {
            if (st.IsStarred && !best.Contains(st))
                this.best.Add(st);
            if (st.Market.Location == StockMarketLocation.Russia)
                this.russian.Add(st);
            if (st.Market.Location == StockMarketLocation.USA)
                this.usa.Add(st);
            if (st.Market.Location == StockMarketLocation.London)
                this.london.Add(st);
            if (st.isOnTinkoff)
                tinkoff.Add(st);
        }
        private void changeList(List<Stock> list)
        {
            this.comboBoxStocks.Items.Clear();
            comboBoxStocks.Text = "";
            foreach (var st in list)
                this.comboBoxStocks.Items.Add(st.FullName);
            comboBoxStocks.DropDownHeight = comboBoxStocks.Items.Count == 0 ? 1 : 300;
            label1.Text = "Общее кол-во: " + comboBoxStocks.Items.Count;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                changeList(best);
            selectedList = best;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                changeList(MainClass.Stocks);
            selectedList = MainClass.Stocks;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                changeList(russian);
            selectedList = russian;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                changeList(usa);
            selectedList = usa;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
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
                MainClass.getStock(true, selectedStock.FullName).IsStarred = selectedStock.IsStarred = checkIsStarred.Checked;
                if (selectedStock.IsStarred)
                {
                    best.Add(selectedStock);
                }
                else
                {
                    best.Remove(selectedStock);
                }
                if (radioButton1.Checked)
                    changeList(best);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MainClass.writeStockListToFile("History_file_" + DateTime.Now.ToString().Replace('.', '-').Replace(' ', '_').Replace(':', '-') + ".dat");
            MessageBox.Show("Сохранено");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string s = openFileDialog1.FileName;
            if (s != "")
                MainClass.loadStockListFromFile(s);
            MainClass.writeLog("Загружен файл " + s);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string s;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var selList = best;
            MainClass.reportFileName = "";
            setButtonsMode(false); button3.Enabled = false;
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            //if (radioButton1.Checked)
            //   MainClass.loadStocksData(best);
            if (radioButton2.Checked)
                selList = MainClass.Stocks;
            if (radioButton3.Checked)
                selList = russian;
            if (radioButton4.Checked)
                selList = usa;
            if (radioButton5.Checked)
                selList = london;
            if (radioButton9.Checked)
                selList = tinkoff;
            //MainClass.loadStocksData(selList);

            await Task.Factory.StartNew<string>(
                                                     () => MainClass.loadStocksData(selList, this.labelSelectParam, this.progressBar1),
                                                     TaskCreationOptions.LongRunning);

            MainClass.writeLog(new string[] { "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" });
            stopwa.Stop();
            setButtonsMode(true);
            MainClass.makeReport(selList);
            button3.Enabled = true;
        }
        private void setButtonsMode(bool mode) // true - enabled, false - disabled
        {
           button5.Enabled= panel1.Enabled = comboBoxStocks.Enabled = buttonGetInfo.Enabled = buttonLoadAllStocks.Enabled = button1.Enabled = button2.Enabled = button4.Enabled = radioButton1.Enabled = radioButton2.Enabled = radioButton3.Enabled = radioButton4.Enabled = mode;
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
            if (MainClass.reportFileName != "")
            {
                Process.Start(MainClass.reportFileName);
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            setButtonsMode(false);
            Stopwatch stopwa = new Stopwatch();
            stopwa.Start();
            await Task.Run(() =>
            {
                Analyzer.analyze(MainClass.Stocks, this.labelSelectParam);
            });

            MainClass.writeLog( "Операция заняла " + stopwa.Elapsed.TotalSeconds.ToString("F0") + " с" );
            stopwa.Stop();
            setButtonsMode(true);
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
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
            if (radioButton7.Checked)
                sortList(1, selectedList);

        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
                sortList(2, selectedList);
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
            {
                changeList(tinkoff);
                selectedList = tinkoff;
            }
        }
    }
}
