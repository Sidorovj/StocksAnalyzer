using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace StocksAnalyzer
{

	public partial class MainForm : Form
	{
		private readonly List<Stock> m_russianStocks = new List<Stock>();
		private readonly List<Stock> m_usaStocks = new List<Stock>();
		private readonly List<Stock> m_londonStocks = new List<Stock>();
		private readonly List<Stock> m_bestStocks = new List<Stock>();
		private readonly List<Stock> m_tinkoffStocks = new List<Stock>();
		private readonly List<string> m_bestStocksNames = new List<string>();
		private List<Stock> m_selectedList;
		private Stock m_selectedStock;

		public MainForm()
		{
			InitializeComponent();
			webBrowser1.Hide();

			InitializeMainClass();

			FormClosing += Form1_FormClosing;

			//comboBoxStocks.DataBindings.Add("Items", m_selectedList, "");
			//comboBoxStocks.DataBindings.Add("Name", m_selectedStock, "Name");

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
			LoadStockListsToViewOnInit();
			m_selectedList = m_tinkoffStocks;
		}

		#region Methods

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
			m_selectedStock = MainClass.GetStock(true, comboBoxStocks.Text);
			if (m_selectedStock == null)
				return;
			checkBoxIsStarred.Enabled = true;

			// Получим инфу об акции
			if (getNewInfo)
				await MainClass.GetStockData(m_selectedStock);
			if (m_selectedStock.Market.Location == StockMarketLocation.Usa)
			{
				linkLabel1.Text = string.Format(Web.GetStockDataUrlUsa, m_selectedStock.Symbol);
				panelUSACoefs.Visible = true;
				panelRussiaCoefs.Visible = false;
			}
			else if (m_selectedStock.Market.Location == StockMarketLocation.Russia)
			{
				if (MainClass.NamesToSymbolsRus.ContainsKey(m_selectedStock.Name))
					linkLabel1.Text = Web.GetStockDataUrlRussia + MainClass.NamesToSymbolsRus[m_selectedStock.Name];
				panelUSACoefs.Visible = false;
				panelRussiaCoefs.Visible = true;
			}
			// Получили

			labelStockName.Text = m_selectedStock.Name;
			checkBoxIsStarred.Checked = m_selectedStock.IsStarred;
			if (m_selectedStock.Market.Currency == StockMarketCurrency.Rub)
			{
				textBoxStockPrice.Text = m_selectedStock.Price.ToString(CultureInfo.InvariantCulture);
				textBoxStockPriceUSD.Text = (m_selectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
			}
			else if (m_selectedStock.Market.Currency == StockMarketCurrency.Usd)
			{
				textBoxStockPriceUSD.Text = m_selectedStock.Price.ToString(CultureInfo.InvariantCulture);
				textBoxStockPrice.Text = (m_selectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
			}
			textBoxStockSymbol.Text = m_selectedStock.Symbol;
			textBoxStockLastUpdated.Text = m_selectedStock.LastUpdate.ToString(CultureInfo.InvariantCulture);
			textBoxPE.Text = m_selectedStock.PriceToEquity.ToCuteStr();
			textBoxPS.Text = m_selectedStock.PriceToSales.ToCuteStr();
			textBoxPBV.Text = m_selectedStock.PriceToBook.ToCuteStr();
			textBoxEVEBITDA.Text = m_selectedStock.EVtoEbitda.ToCuteStr();
			textBoxDebtEBITDA.Text = m_selectedStock.DebtToEbitda.ToCuteStr();
			textBoxROE.Text = m_selectedStock.Roe.ToCuteStr();
			textBoxEPS.Text = m_selectedStock.Eps.ToCuteStr();
			if (m_selectedStock.Market.Currency == StockMarketCurrency.Rub)
			{
				textBoxQEG.Text = m_selectedStock.Qeg.ToCuteStr();
				textBoxProfitMargin.Text = m_selectedStock.ProfitMarg.ToCuteStr();
				textBoxOperatingMargin.Text = m_selectedStock.OperMarg.ToCuteStr();
				textBoxGrossProfit.Text = m_selectedStock.GrossProfit.ToCuteStr();
				textBox5YValProfit.Text = m_selectedStock.GrossProfit5Ya.ToCuteStr();
				textBoxProfitCoef.Text = m_selectedStock.ProfitCoef.ToCuteStr();
				textBox5YProfitCoef.Text = m_selectedStock.ProfitCoef5Ya.ToCuteStr();
				textBoxProfitPerShare.Text = m_selectedStock.ProfitOn12MToAnalogYearAgo.ToCuteStr();
				textBoxGrowthPS5Y.Text = m_selectedStock.GrowProfitPerShare5Y.ToCuteStr();
				textBoxGrowthCapCosts.Text = m_selectedStock.CapExpenseGrow5Y.ToCuteStr();
				textBoxProfitMargin5Y.Text = m_selectedStock.ProfitMarg5Ya.ToCuteStr();
				textBoxOperMargin5Y.Text = m_selectedStock.OperMarg5Ya.ToCuteStr();
				textBoxQuickLiquidity.Text = m_selectedStock.UrgentLiquidityCoef.ToCuteStr();
				textBoxCurrLiquidity.Text = m_selectedStock.CurrentLiquidityCoef.ToCuteStr();
			}
			else if (m_selectedStock.Market.Currency == StockMarketCurrency.Usd)
			{
				textBoxEVEBITDA.Text = m_selectedStock.EVtoEbitda.ToCuteStr();
				textBoxMarketCap.Text = m_selectedStock.MarketCap.ToCuteStr();
				textBoxQEG.Text = m_selectedStock.Qeg.ToCuteStr();
				textBoxProfitMargin.Text = m_selectedStock.ProfitMarg.ToCuteStr();
				textBoxOperatingMargin.Text = m_selectedStock.OperMarg.ToCuteStr();
				textBoxGrossProfit.Text = m_selectedStock.GrossProfit.ToCuteStr();
				textBoxEntVal.Text = m_selectedStock.Ev.ToCuteStr();
				textBoxPEG.Text = m_selectedStock.Peg.ToCuteStr();
				textBoxEVRev.Text = m_selectedStock.EvRev.ToCuteStr();
				textBoxRetOnAssets.Text = m_selectedStock.RetOnAssets.ToCuteStr();
				textBoxRevenue.Text = m_selectedStock.Revenue.ToCuteStr();
				textBoxRevPerShape.Text = m_selectedStock.RevPerShare.ToCuteStr();
				textBoxEBITDA.Text = m_selectedStock.Ebitda.ToCuteStr();
				textBoxTotCash.Text = m_selectedStock.TotalCash.ToCuteStr();
				textBoxTotCashPShape.Text = m_selectedStock.TotalCashPerShare.ToCuteStr();
				textBoxTotDebt.Text = m_selectedStock.TotalDebt.ToCuteStr();
				textBoxBookValPShape.Text = m_selectedStock.BookValPerShare.ToCuteStr();
				textBoxOperCashFlow.Text = m_selectedStock.OperatingCashFlow.ToCuteStr();
				textBoxLeveredFreeCF.Text = m_selectedStock.LeveredFreeCashFlow.ToCuteStr();
				textBoxSharesCount.Text = m_selectedStock.TotalShares.ToCuteStr();
				textBoxDebtEBITDA.Text = m_selectedStock.DebtToEbitda.ToCuteStr();
			}
			if (getNewInfo)
				labelDone.Visible = true;
			labelMainPE.Text = @"MainPE: " + m_selectedStock.RateMainPe;
			labelMain.Text = @"Main: " + m_selectedStock.RateMain;
			labelMainAll.Text = @"MainAll: " + m_selectedStock.RateMainAll;

		}

		private async void ButtonGetInfo_Click(object sender, EventArgs e)
		{
			Stopwatch stopwatch = new Stopwatch();
			SetButtonsMode(false);
			stopwatch.Start();
			try
			{
				await FillStockForm();
			}
			catch (Exception ex)
			{
				MainClass.WriteLog(ex);
			}

			stopwatch.Stop();

			MainClass.WriteLog($"Загрузка инфы для акции {m_selectedStock} заняла {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
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
			m_tinkoffStocks.Clear();
			m_russianStocks.Clear();
			m_usaStocks.Clear();
			m_londonStocks.Clear();
			m_bestStocksNames.Clear();
			foreach (var st in m_bestStocks)
				m_bestStocksNames.Add(st.Name);
			m_bestStocks.Clear();

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

			MainClass.WriteLog($"Загрузка списка акций заняла {stopwa.Elapsed.TotalSeconds:F0} с");
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
				if (m_bestStocksNames.Contains(selectedStock.Name))
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
		private void LoadStockListsToViewOnInit()
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
			if (st.IsStarred && !m_bestStocks.Contains(st))
				m_bestStocks.Add(st);
			if (st.Market.Location == StockMarketLocation.Russia)
				m_russianStocks.Add(st);
			if (st.Market.Location == StockMarketLocation.Usa)
				m_usaStocks.Add(st);
			if (st.Market.Location == StockMarketLocation.London)
				m_londonStocks.Add(st);
			if (st.IsOnTinkoff)
				m_tinkoffStocks.Add(st);
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
				LoadNewListInComboBox(m_bestStocks);
			m_selectedList = m_bestStocks;
		}

		private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonAllStocks.Checked)
				LoadNewListInComboBox(MainClass.Stocks);
			m_selectedList = MainClass.Stocks;
		}

		private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonRusStocks.Checked)
				LoadNewListInComboBox(m_russianStocks);
			m_selectedList = m_russianStocks;
		}

		private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonUSAStocks.Checked)
				LoadNewListInComboBox(m_usaStocks);
			m_selectedList = m_usaStocks;
		}

		private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonLondonStocks.Checked)
				LoadNewListInComboBox(m_londonStocks);
		}

		private void ComboBoxStocks_DropDown(object sender, EventArgs e)
		{
		}

		private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
		{
			if (m_selectedStock != null && m_selectedStock.IsStarred != checkBoxIsStarred.Checked)
			{
				MainClass.GetStock(true, m_selectedStock.FullName).IsStarred = m_selectedStock.IsStarred = checkBoxIsStarred.Checked;
				if (m_selectedStock.IsStarred)
				{
					m_bestStocks.Add(m_selectedStock);
				}
				else
				{
					m_bestStocks.Remove(m_selectedStock);
				}
				if (radioButtonStarred.Checked)
					LoadNewListInComboBox(m_bestStocks);
			}
		}

		private void ButtonSaveHistory_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(Const.HistoryDirName))
				Directory.CreateDirectory(Const.HistoryDirName);
			MainClass.WriteStockListToFile($"{Const.HistoryDirName}/History_file_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.dat");
			MessageBox.Show(@"Сохранено");
		}

		private void ButtonLoadHistoryFile_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			string filePath = openFileDialog1.FileName;
			if (filePath != "" && filePath != nameof(openFileDialog1))
			{
				MainClass.LoadStockListFromFile(filePath);
				MainClass.WriteLog("Загружен файл " + filePath);
			}
		}

		private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
		{

		}

		private async void ButtonLoadStockMultiplicators_Click(object sender, EventArgs e)
		{
			if (m_selectedList == null)
			{
				m_selectedList = m_bestStocks;
				if (radioButtonAllStocks.Checked)
					m_selectedList = MainClass.Stocks;
				if (radioButtonRusStocks.Checked)
					m_selectedList = m_russianStocks;
				if (radioButtonUSAStocks.Checked)
					m_selectedList = m_usaStocks;
				if (radioButtonLondonStocks.Checked)
					m_selectedList = m_londonStocks;
				if (radioButtonFromTinkoff.Checked)
					m_selectedList = m_tinkoffStocks;
			}

			SetButtonsMode(false);
			buttonOpenReport.Enabled = false;

			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				await MainClass.LoadStocksData(m_selectedList, labelRemainingTime, progressBar);
			}
			finally
			{
				MainClass.WriteStockListToFile();
			}
			stopwatch.Stop();
			MainClass.WriteLog($"Загрузка мультипл. из инета заняла {stopwatch.Elapsed.TotalSeconds:F0} с");
			MainClass.MakeReportAndSaveToFile(m_selectedList);

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
			MainClass.WriteLog($"Анализ мультипликаторов занял {stopwatch.Elapsed.TotalSeconds:F0} с");
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
			lst.Sort((s1, s2) =>
			{
				switch (regime)
				{
					case 0:
						return s1.RateMainPe - s2.RateMainPe;
					case 1:
						return s1.RateMain - s2.RateMain;
					case 2:
						return s1.RateMainAll - s2.RateMainAll;
				}
				throw new NotSupportedException("In sort list");
			});
			foreach (var st in lst)
				comboBoxStocks.Items.Add(st.FullName);
		}
		private void RadioButtonMainPE_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonMainPE.Checked)
				SortList(0, m_selectedList);
		}

		private void RadioButtonMain_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonMain.Checked)
				SortList(1, m_selectedList);

		}

		private void RadioButtonMainAll_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonMainAll.Checked)
				SortList(2, m_selectedList);
		}

		private void RadioButtonFromTinkoff_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonFromTinkoff.Checked)
			{
				LoadNewListInComboBox(m_tinkoffStocks);
				m_selectedList = m_tinkoffStocks;
			}
		}

		#endregion

		private async void ButtonCkechTinkoff_Click(object sender, EventArgs e)
		{
			await MainClass.GetStocksList(labelRemainingTime, progressBar, false);
		}
	}
}
