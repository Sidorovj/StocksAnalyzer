using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using StocksAnalyzer.Helpers;

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
		// ReSharper disable once CollectionNeverQueried.Local
		private Dictionary<string, Label> CoefsLabels { get; } = new Dictionary<string, Label>(Coefficient.CoefficientList.Count);
		private Dictionary<string, TextBox> CoefsTextboxes { get; } = new Dictionary<string, TextBox>(Coefficient.CoefficientList.Count);
		private Dictionary<string, TextBox> MetricsTextboxes { get; } = new Dictionary<string, TextBox>(Coefficient.MetricsList.Count);

		public MainForm()
		{
			InitializeComponent();
			panelRussiaCoefs.Top = panelUSACoefs.Top;
			panelRussiaCoefs.Left = panelUSACoefs.Left;
			panelRussiaCoefs.Width = panelUSACoefs.Width;

			ResetStockForm();

			InitializeMainClass();

			FormClosing += Form1_FormClosing;

			ToolTip t = new ToolTip();
			var yStep = 25;
			var xPadding = 190;
			var positionCommon = new Point(textBoxStockPriceUSD.Location.X, textBoxStockPriceUSD.Location.Y + yStep);
			var positionUsa = new Point(labelUSAPanel.Location.X + xPadding, labelUSAPanel.Location.Y + yStep);
			var positionRus = new Point(labelRussiaPanel.Location.X + xPadding, labelRussiaPanel.Location.Y + yStep);
			foreach (var coef in Coefficient.CoefficientList)
			{
				Point position;
				if (coef.IsCommon)
				{
					position = positionCommon;
					positionCommon.Y += yStep;
				}
				else if (coef.IsUSA)
				{
					position = positionUsa;
					positionUsa.Y += yStep;
				}
				else
				{
					position = positionRus;
					positionRus.Y += yStep;
				}
				CoefsTextboxes[coef.Name] = CreateTextbox(coef.Name, coef.IsUSA, coef.IsRus, position);

				var label = CreateLabel(coef.Name, coef.Label, coef.IsUSA, coef.IsRus, position);
				CoefsLabels[coef.Name] = label;
				if (!string.IsNullOrEmpty(coef.Tooltip))
					t.SetToolTip(label, coef.Tooltip);

				if (!string.IsNullOrEmpty(coef.HelpDescription))
					CreateHelpLabel($"{coef.Name}_help", coef.HelpDescription, CoefsTextboxes[coef.Name]);
			}

			var positionMetric = new Point(labelMetric.Location.X + xPadding/2, labelMetric.Location.Y + 5);
			foreach (var metric in Coefficient.MetricsList)
			{
				positionMetric.Y += yStep;
				CreateLabel($"label_{metric}", metric, positionMetric, labelMetric);
				MetricsTextboxes[metric] = CreateTextbox(metric, positionMetric, labelMetric);
			}

			t.SetToolTip(labelSymbol, "Тикет");
		}


		private Label CreateLabel(string name, string text, bool isUsa, bool isRus, Point position)
		{
			Label lbl = CreateLabel(name, text, position, null);
			AddControlToPanel(lbl, isUsa, isRus);
			lbl.Location = new Point(position.X - lbl.Size.Width - 14, position.Y + 3);
			return lbl;
		}
		private Label CreateLabel(string name, string text, Point position, Control ctr)
		{
			Label lbl = new Label
			{
				AutoSize = true,
				Margin = new Padding(4, 0, 4, 0),
				Name = $"label{name}",
				Text = text
			};// maybe add TabIndex?
			if (ctr != null)
			{
				ctr.Parent.Controls.Add(lbl);
				lbl.Location = new Point(position.X - lbl.Size.Width - 14, position.Y + 3);
			}

			return lbl;
		}

		private void CreateHelpLabel(string name, string text, Control ctr)
		{
			Label lbl = new Label
			{
				AutoSize = true,
				Margin = new Padding(4, 0, 4, 0),
				BorderStyle = BorderStyle.Fixed3D,
				Name = $"label{name}",
				Text = @"?"
			};
			lbl.Font = new Font(lbl.Font.FontFamily, lbl.Font.Size + 1);
			ctr.Parent.Controls.Add(lbl);
			lbl.BringToFront();
			lbl.Location = new Point(ctr.Location.X + ctr.Width + 12, ctr.Location.Y);
			Label tb = null;
			lbl.MouseEnter += (o, args) =>
			{
				var sender = o as Label ?? throw new ArgumentNullException();
				int x = sender.Parent.Parent.Location.X + sender.Parent.Location.X + sender.Location.X;
				int y = sender.Parent.Parent.Location.Y + sender.Parent.Location.Y + sender.Location.Y;
				if (tb == null)
				{
					tb = new Label
					{
						Margin = new Padding(4),
						Name = $"textBox{name}",
						BorderStyle = BorderStyle.Fixed3D,
						Text = text,
						AutoSize = true

					};
					tb.Font = new Font(tb.Font.FontFamily, tb.Font.Size + 1);
					tb.Location = new Point(x - tb.Width / 2, y - lbl.Height - 5);
					Controls.Add(tb);
					tb.BringToFront();
				}
				else
				{
					tb.Visible = true;
					tb.BringToFront();
				}

			};
			lbl.MouseLeave += (o, args) => { tb.Visible = false; };
		}

		private TextBox CreateTextbox(string name, Point position, Control ctr)
		{
			TextBox tb = CreateSimpleTextbox(name, position);
			ctr.Parent.Controls.Add(tb);
			return tb;
		}
		private TextBox CreateTextbox(string name, bool isUsa, bool isRus, Point position)
		{
			TextBox tb = CreateSimpleTextbox(name, position);
			AddControlToPanel(tb, isUsa, isRus);
			return tb;
		}

		private TextBox CreateSimpleTextbox(string name, Point position)
		{
			return new TextBox
			{
				Location = position,
				Margin = new Padding(4),
				Name = $"textBox{name}",
				Size = new Size(100, 22)
			};
		}

		private void AddControlToPanel(Control c, bool isUsa, bool isRus)
		{
			if (!isRus && !isUsa)
				panelCommon.Controls.Add(c);
			else if (isRus)
				panelRussiaCoefs.Controls.Add(c);
			else panelUSACoefs.Controls.Add(c);
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

		private void ResetStockForm(bool exceptCoefs = false)
		{
			textBoxRatingAll.Text = "";
			textBoxRatingCoefs.Text = "";
			textBoxRatingMetrics.Text = "";
			if (exceptCoefs)
				return;
			checkBoxIsStarred.Enabled = false;
			panelMain.Visible = panelUSACoefs.Visible = panelCommon.Visible = panelRussiaCoefs.Visible = false;
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
			{
				ResetStockForm();
				return;
			}
			panelMain.Visible = panelCommon.Visible = true;
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
			foreach (var coef in Coefficient.CoefficientList)
			{
				CoefsTextboxes[coef.Name].Text = m_selectedStock[coef].ToCuteStr();
			}

			textBoxRatingAll.Text = m_selectedStock.AveragePositionAll.ToString(CultureInfo.InvariantCulture);
			textBoxRatingCoefs.Text = m_selectedStock.AveragePositionNormalizedCoefs.ToString(CultureInfo.InvariantCulture);
			textBoxRatingMetrics.Text = m_selectedStock.AveragePositionMetric.ToString(CultureInfo.InvariantCulture);
			if (getNewInfo)
				labelDone.Visible = true;

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
			Stock.AllStocksInListAnalyzed = false;
			ResetStockForm(true);
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
				Analyzer.Analyze(m_selectedList);
			});

			stopwatch.Stop();
			await FillStockForm(false);
			MainClass.WriteLog($"Анализ мультипликаторов занял {stopwatch.Elapsed.TotalSeconds:F0} с");

			SetButtonsMode(true);
			Stock.AllStocksInListAnalyzed = true;
		}


		/// <summary>
		/// Сортирует список по указанной метрике
		/// </summary>
		/// <param name="regime">0-MainPE, 1-Main, 2-MainAll</param>
		/// <param name="lst">Список акций</param>
		/// <param name="coef">Коэффициент</param>
		/// <param name="metricName">Название метрики</param>
		private void SortList(SortingModes regime, List<Stock> lst, Coefficient coef = null, string metricName = "")
		{
			if (!Stock.AllStocksInListAnalyzed)
				ButtonAnalyzeMultiplicators_Click(null, null);
			comboBoxStocks.Items.Clear();
			lst.Sort((s1, s2) =>
			{
				switch (regime)
				{
					case SortingModes.PositionAll:
						return s1.AveragePositionAll > s2.AveragePositionAll ? 1 :
							Math.Abs(s1.AveragePositionAll - s2.AveragePositionAll) < MainClass.Tolerance ? 0 : -1;
					case SortingModes.PositionMetric:
						return s1.AveragePositionMetric > s2.AveragePositionMetric ? 1 :
							Math.Abs(s1.AveragePositionMetric - s2.AveragePositionMetric) < MainClass.Tolerance ? 0 : -1;
					case SortingModes.PositionCoef:
						return s1.AveragePositionNormalizedCoefs > s2.AveragePositionNormalizedCoefs ? 1 :
							Math.Abs(s1.AveragePositionNormalizedCoefs - s2.AveragePositionNormalizedCoefs) <
							MainClass.Tolerance ? 0 : -1;
					case SortingModes.Coefficeint:
						if (coef == null)
							throw new ArgumentNullException(nameof(coef));
						var v1 = s1.NormalizedCoefficientsValues[coef];
						var v2 = s2.NormalizedCoefficientsValues[coef];
						if (v1.HasValue && v2.HasValue)
							return v1 > v2 ? 1 : Math.Abs(v1.Value - v2.Value) < MainClass.Tolerance ? 0 : -1;
						if (v1.HasValue && !v2.HasValue)
							return 1;
						if (!v1.HasValue && v2.HasValue)
							return -1;
						return 0;
					case SortingModes.Metric:
						var m1 = s1.MetricsValues[metricName];
						var m2 = s2.MetricsValues[metricName];
						return m1 > m2 ? 1 : Math.Abs(m1 - m2) < MainClass.Tolerance ? 0 : -1;
				}
				throw new NotSupportedException("In sort list");
			});
			foreach (var st in lst)
				comboBoxStocks.Items.Add(st.FullName);
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
