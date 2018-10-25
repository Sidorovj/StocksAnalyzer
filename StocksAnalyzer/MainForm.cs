using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using StockCore.Interfaces;
using StockCore.Stock;
using StocksAnalyzer.Helpers;
using StocksAnalyzer.WinForms;

namespace StocksAnalyzer
{

	public partial class MainForm : Form
	{
		public static string ViewSettingsFile { get; } = @"ViewSettings.dat";

		private readonly List<string> m_bestStocksNames = new List<string>();
		private StockList m_selectedList = GetList(StockListNamesEnum.Tinkoff);
		private Stock m_selectedStock;

		// ReSharper disable once CollectionNeverQueried.Local
		private Dictionary<Coefficient, Label> CoefsLabels { get; } =
			new Dictionary<Coefficient, Label>(Coefficient.CoefficientList.Count);

		private Dictionary<Coefficient, TextBox> CoefsTextboxes { get; } =
			new Dictionary<Coefficient, TextBox>(Coefficient.CoefficientList.Count);

		private Dictionary<Metric, TextBox> MetricsTextboxes { get; } =
			new Dictionary<Metric, TextBox>(Coefficient.MetricsList.Count);

		private List<Button> SortButtons { get; } =
			new List<Button>(Coefficient.MetricsList.Count + Coefficient.CoefficientList.Count);

		private Dictionary<IFactor, Label> PositionLabels { get; } =
			new Dictionary<IFactor, Label>(Coefficient.MetricsList.Count + Coefficient.CoefficientList.Count);

		public MainForm()
		{
			InitializeComponent();
			panelRussiaCoefs.Top = panelUSACoefs.Top;
			panelRussiaCoefs.Left = panelUSACoefs.Left;
			panelRussiaCoefs.Width = panelUSACoefs.Width;
			panelRussiaCoefs.Height = panelUSACoefs.Height;

			ResetStockForm();

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

				CoefsTextboxes[coef] = CreateTextbox(coef.Name, coef.IsUSA, coef.IsRus, position);

				var label = CreateLabel(coef.Name, coef.Label, coef.IsUSA, coef.IsRus, position);
				CoefsLabels[coef] = label;
				if (!string.IsNullOrEmpty(coef.Tooltip))
					t.SetToolTip(label, coef.Tooltip);

				if (!string.IsNullOrEmpty(coef.HelpDescription))
					CreateHelpLabel($"{coef.Name}_help", coef.HelpDescription, CoefsTextboxes[coef]);
			}


			var positionMetric = new Point(labelMetric.Location.X + xPadding / 2, labelMetric.Location.Y + 5);
			foreach (var metric in Coefficient.MetricsList)
			{
				positionMetric.Y += yStep;
				CreateLabel($"label_{metric}", metric.ToString(), positionMetric, labelMetric);
				MetricsTextboxes[metric] = CreateTextbox(metric.ToString(), positionMetric, labelMetric);
			}

			foreach (var coef in Coefficient.CoefficientList)
			{
				PositionLabels[coef] = CreatePositionLabel(CoefsTextboxes[coef], coef.Name);
				CreateSortButton(PositionLabels[coef], coef.Name, coef);
			}

			foreach (var metric in Coefficient.MetricsList)
			{
				PositionLabels[metric] = CreatePositionLabel(MetricsTextboxes[metric], metric.ToString(), true);
				CreateSortButton(PositionLabels[metric], metric.ToString(), metric);
			}

			// ReSharper disable once RedundantArgumentDefaultValue
			CreateSortButton(textBoxRatingAll, textBoxRatingAll.Name, sortMode: SortingModes.PositionAll);
			CreateSortButton(textBoxRatingCoefs, textBoxRatingCoefs.Name, sortMode: SortingModes.PositionCoef);
			CreateSortButton(textBoxRatingMetrics, textBoxRatingMetrics.Name, sortMode: SortingModes.PositionMetric);

			t.SetToolTip(labelSymbol, "Тикет");

#pragma warning disable 4014
			InitializeMainClass();
#pragma warning restore 4014
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
			}; // maybe add TabIndex?
			if (ctr != null)
			{
				ctr.Parent.Controls.Add(lbl);
				lbl.Location = new Point(position.X - lbl.Size.Width - 14, position.Y + 3);
			}

			return lbl;
		}

		private void CreateSortButton(Control ctr, string name, IFactor factor = null,
			SortingModes sortMode = SortingModes.PositionAll)
		{
			Button btn = new Button
			{
				Text = @"S",
				Size = new Size(15, 20),
				Enabled = Stock.AllStocksInListAnalyzed
			};
			SortButtons.Add(btn);
			ctr.Parent.Controls.Add(btn);
			btn.BringToFront();
			btn.Location = new Point(ctr.Location.X + ctr.Width + (factor == null ? 12 : factor is Metric ? 30 : 40),
				ctr.Location.Y - 2);
			btn.Click += (o, args) =>
			{
				sortMode = factor is Coefficient ? SortingModes.Coefficeint :
					factor is Metric ? SortingModes.Metric : sortMode;
				SortList(sortMode, m_selectedList.StList.ToList(), factor);
			};
			var text = "Sort";

			Label tb = null;
			btn.MouseEnter += (o, args) =>
			{
				var sender = o as Control ?? throw new ArgumentNullException();
				if (tb == null)
				{
					tb = new Label
					{
						Margin = new Padding(4),
						Name = $"textBoxHelp_{name}",
						BorderStyle = BorderStyle.Fixed3D,
						Text = text,
						AutoSize = true
					};
					int x = sender.Location.X;
					int y = sender.Location.Y;
					tb.Font = new Font(tb.Font.FontFamily, tb.Font.Size + 1);
					tb.Location = new Point(x - sender.Width / 2, y - sender.Height + 1);
					btn.Parent.Controls.Add(tb);
					tb.BringToFront();
				}
				else
				{
					tb.Visible = true;
					tb.BringToFront();
				}

			};
			btn.MouseLeave += (o, args) => { tb.Visible = false; };
		}

		private Label CreatePositionLabel(Control ctr, string name, bool isMetric = false)
		{
			Label lbl = new Label
			{
				AutoSize = true,
				Margin = new Padding(4, 0, 4, 0),
				BorderStyle = BorderStyle.Fixed3D,
				Name = $"labelSort{name}",
				Text = @"0"
			};
			ctr.Parent.Controls.Add(lbl);
			lbl.BringToFront();
			lbl.Location = new Point(ctr.Location.X + ctr.Width + (isMetric ? 6 : 25), ctr.Location.Y + 2);
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
			lbl.Location = new Point(ctr.Location.X + ctr.Width + 6, ctr.Location.Y);
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

		private async Task InitializeMainClass()
		{
			await Task.Delay(10);
			LoadStockListsToViewOnInit();
			LogWriter.WriteLog(
				$"Текущие котировки: \r\nUSD: {StockMarket.GetExchangeRates(StockMarketCurrency.Usd):F2}\r\nEUR: {StockMarket.GetExchangeRates(StockMarketCurrency.Eur):F2}");

			string path = $"{Const.ToRestoreDirName}/{ViewSettingsFile}";
			if (!File.Exists(path))
			{
				m_selectedList = GetList(StockListNamesEnum.Tinkoff);
				Logger.Log.Warn($"File {path} does not exists.");
				return;
			}

			Serializer ser = new Serializer(path);
			bool[] checks = ser.Deserialize() as bool[] ?? throw new ArgumentNullException();

			if (m_selectedList == null)
			{
				m_selectedList = GetList(StockListNamesEnum.Starred);
				if (checks[0])
				{
					radioButtonAllStocks.Checked = true;
					m_selectedList = GetList(StockListNamesEnum.All);
				}
				else if (checks[1])
				{
					radioButtonRusStocks.Checked = true;
					m_selectedList = GetList(StockListNamesEnum.Rus);
				}

				else if (checks[2])
				{
					radioButtonUSAStocks.Checked = true;
					m_selectedList = GetList(StockListNamesEnum.Usa);
				}
				else if (checks[3])
				{
					radioButtonLondonStocks.Checked = true;
					m_selectedList = null;
				}
				else if (checks[4])
				{
					radioButtonFromTinkoff.Checked = true;
					m_selectedList = GetList(StockListNamesEnum.Tinkoff);
				}
			}
		}

		#region Methods

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
			}
			finally
			{
				MainClass.WriteStockListToFile();
				string path = $"{Const.ToRestoreDirName}/{ViewSettingsFile}";
				if (!File.Exists(path))
				{
					var fs = File.Create(path);
					fs.Close();
				}

				Serializer ser = new Serializer(path);
				ser.Serialize(new[]
				{
					radioButtonAllStocks.Checked, radioButtonRusStocks.Checked, radioButtonUSAStocks.Checked,
					radioButtonLondonStocks.Checked, radioButtonFromTinkoff.Checked
				});
			}

			try
			{
			}
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
				if (!string.IsNullOrEmpty(m_selectedStock.LinkToGetInfo))
					linkLabel1.Text = Web.GetStockDataUrlRussia + m_selectedStock.LinkToGetInfo;
				panelUSACoefs.Visible = false;
				panelRussiaCoefs.Visible = true;
			}
			// Получили

			labelStockName.Text = m_selectedStock.Name;
			checkBoxIsStarred.Checked = m_selectedStock.IsStarred;
			if (m_selectedStock.Market.Currency == StockMarketCurrency.Rub)
			{
				textBoxStockPrice.Text = m_selectedStock.Price.ToString(CultureInfo.InvariantCulture);
				textBoxStockPriceUSD.Text =
					(m_selectedStock.Price / StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
			}
			else if (m_selectedStock.Market.Currency == StockMarketCurrency.Usd)
			{
				textBoxStockPriceUSD.Text = m_selectedStock.Price.ToString(CultureInfo.InvariantCulture);
				textBoxStockPrice.Text =
					(m_selectedStock.Price * StockMarket.GetExchangeRates(StockMarketCurrency.Usd)).ToString("F2");
			}

			textBoxStockSymbol.Text = m_selectedStock.Symbol;
			textBoxStockLastUpdated.Text = m_selectedStock.LastUpdate.ToString(CultureInfo.InvariantCulture);
			foreach (var coef in Coefficient.CoefficientList)
			{
				CoefsTextboxes[coef].Text = m_selectedStock[coef].ToCuteStr();
				if (m_selectedStock.ListToRatings.ContainsKey(m_selectedList) && m_selectedStock.ListToRatings[m_selectedList].ContainsKey(coef) &&
					Stock.AllStocksInListAnalyzed)
				{
					var pos = m_selectedStock.ListToRatings[m_selectedList][coef];
					PositionLabels[coef].Text = pos != null
						? $@"{100.0 * (Stock.CoefHasValueCount[coef] - pos + 1) / Stock.CoefHasValueCount[coef]:F2}%"
						: @" ";
				}
			}

			var listCount = m_selectedList.StList.Count();
			foreach (var metric in Coefficient.MetricsList)
			{
				if (m_selectedStock.MetricsValues.ContainsKey(metric))
					MetricsTextboxes[metric].Text = m_selectedStock.MetricsValues[metric].ToCuteStr();
				if (m_selectedStock.ListToRatings.ContainsKey(m_selectedList) && m_selectedStock.ListToRatings[m_selectedList].ContainsKey(metric) &&
					Stock.AllStocksInListAnalyzed)
				{
					var pos = m_selectedStock.ListToRatings[m_selectedList][metric];
					PositionLabels[metric].Text = pos != null
						? $@"{100.0 * (listCount - pos + 1) / listCount:F2}%"
						: @" ";
				}
			}

			if (Stock.AllStocksInListAnalyzed)
			{
				textBoxRatingAll.Text = $@"{m_selectedStock.AveragePositionAll:F2}";
				textBoxRatingCoefs.Text = $@"{m_selectedStock.AveragePositionNormalizedCoefs:F2}";
				textBoxRatingMetrics.Text = $@"{m_selectedStock.AveragePositionMetric:F2}";
			}

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
				LogWriter.WriteLog(ex);
			}

			stopwatch.Stop();

			LogWriter.WriteLog(
				$"Загрузка инфы для акции {m_selectedStock} заняла {stopwatch.Elapsed.TotalMilliseconds:F2} мс");
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
			m_selectedList = GetList(StockListNamesEnum.All);
			m_bestStocksNames.Clear();
			foreach (var st in m_selectedList.StList)
				m_bestStocksNames.Add(st.Name);

			Stopwatch stopwa = Stopwatch.StartNew();

			await MainClass.GetStocksList(new FormsTextReporter(labelRemainingTime),
				new FormsProgressReporter(progressBar));
			FillStockLists();
			stopwa.Stop();

			LogWriter.WriteLog($"Загрузка списка акций заняла {stopwa.Elapsed.TotalSeconds:F0} с");
			SetButtonsMode(true);

			MessageBox.Show(@"Список загружен");
		}

		/// <summary>
		/// Заполнить список акций
		/// </summary>
		private void FillStockLists()
		{
			foreach (var selectedStock in m_selectedList.StList)
			{
				if (m_bestStocksNames.Contains(selectedStock.Name))
					selectedStock.IsStarred = true;
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
			foreach (var selectedStock in m_selectedList.StList)
			{
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

		private void LoadNewListInComboBox(IEnumerable<Stock> list)
		{
			Stock.AllStocksInListAnalyzed = false;
			foreach (var btn in SortButtons)
			{
				btn.Enabled = false;
			}

			ResetStockForm(true);

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
			{
				m_selectedList = GetList(StockListNamesEnum.Starred);
				LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private void RadioButtonAllStocks_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonAllStocks.Checked)
			{
				m_selectedList = GetList(StockListNamesEnum.All);
				LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private void RadioButtonRus_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonRusStocks.Checked)
			{
				m_selectedList = GetList(StockListNamesEnum.Rus);
				LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private void RadioButtonUSA_CheckedChanged(object sender, EventArgs e)
		{
			if (radioButtonUSAStocks.Checked)
			{
				m_selectedList = GetList(StockListNamesEnum.Usa);
				LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private void RadioButtonLondon_CheckedChanged(object sender, EventArgs e)
		{
			throw new NotSupportedException();
			//if (radioButtonLondonStocks.Checked)
			//	LoadNewListInComboBox(m_londonStocks);
		}

		private void ComboBoxStocks_DropDown(object sender, EventArgs e)
		{
		}

		private void CheckBoxIsStarred_CheckedChanged(object sender, EventArgs e)
		{
			if (m_selectedStock != null && m_selectedStock.IsStarred != checkBoxIsStarred.Checked)
			{
				MainClass.GetStock(true, m_selectedStock.FullName).IsStarred =
					m_selectedStock.IsStarred = checkBoxIsStarred.Checked;

				if (radioButtonStarred.Checked)
					LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private void ButtonSaveHistory_Click(object sender, EventArgs e)
		{
			if (!Directory.Exists(Const.ToRestoreDirName))
				Directory.CreateDirectory(Const.ToRestoreDirName);
			MainClass.WriteStockListToFile(
				$"{Const.ToRestoreDirName}/History_file_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.dat");
			MessageBox.Show(@"Сохранено");
		}

		private void ButtonLoadHistoryFile_Click(object sender, EventArgs e)
		{
			openFileDialog1.ShowDialog();
			string filePath = openFileDialog1.FileName;
			if (filePath != "" && filePath != nameof(openFileDialog1))
			{
				MainClass.LoadStockListFromFile(filePath);
				LogWriter.WriteLog("Загружен файл " + filePath);
			}
		}

		private void OpenFileDialog1_FileOk(object sender, CancelEventArgs e)
		{

		}

		private async void ButtonLoadStockMultiplicators_Click(object sender, EventArgs e)
		{
			if (m_selectedList == null)
			{
				m_selectedList = GetList(StockListNamesEnum.Starred);
				if (radioButtonAllStocks.Checked)
					m_selectedList = GetList(StockListNamesEnum.All);
				if (radioButtonRusStocks.Checked)
					m_selectedList = GetList(StockListNamesEnum.Rus);
				if (radioButtonUSAStocks.Checked)
					m_selectedList = GetList(StockListNamesEnum.Usa);
				if (radioButtonLondonStocks.Checked)
					m_selectedList = null;
				if (radioButtonFromTinkoff.Checked)
					m_selectedList = GetList(StockListNamesEnum.Tinkoff);
			}

			SetButtonsMode(false);
			buttonOpenReport.Enabled = false;

			Stopwatch stopwatch = Stopwatch.StartNew();
			try
			{
				await MainClass.LoadStocksData(m_selectedList.StList.ToList(),
					new FormsTextReporter(labelRemainingTime),
					new FormsProgressReporter(progressBar));
			}
			finally
			{
				MainClass.WriteStockListToFile();
			}

			stopwatch.Stop();
			LogWriter.WriteLog($"Загрузка мультипл. из инета заняла {stopwatch.Elapsed.TotalSeconds:F0} с");

			SetButtonsMode(true);
			buttonOpenReport.Enabled = true;
		}

		/// <summary>
		/// Ставит доступность кнопок
		/// </summary>
		/// <param name="enabled">Доступны</param>
		private void SetButtonsMode(bool enabled)
		{
			buttonAnalyzeMultiplicators.Enabled = buttonCkechTinkoff.Enabled = panelMain.Enabled =
				comboBoxStocks.Enabled =
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
			MainClass.OpenReportIfExists();
		}

		private async void ButtonAnalyzeMultiplicators_Click(object sender, EventArgs e)
		{
			SetButtonsMode(false);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			await Task.Run(() => { MainClass.Analyze(); });

			stopwatch.Stop();
			Stock.AllStocksInListAnalyzed = true;
			await FillStockForm(false);
			LogWriter.WriteLog($"Анализ мультипликаторов занял {stopwatch.Elapsed.TotalSeconds:F0} с");

			SetButtonsMode(true);
			foreach (var btn in SortButtons)
			{
				btn.Enabled = true;
			}
		}


		/// <summary>
		/// Сортирует список по указанной метрике
		/// </summary>
		/// <param name="regime">0-MainPE, 1-Main, 2-MainAll</param>
		/// <param name="lst">Список акций</param>
		/// <param name="factor">Коэффициент или метрика</param>
		private void SortList(SortingModes regime, List<Stock> lst, IFactor factor)
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
							Math.Abs(s1.AveragePositionAll - s2.AveragePositionAll) < Const.Tolerance ? 0 : -1;
					case SortingModes.PositionMetric:
						return s1.AveragePositionMetric > s2.AveragePositionMetric ? 1 :
							Math.Abs(s1.AveragePositionMetric - s2.AveragePositionMetric) < Const.Tolerance ? 0 : -1;
					case SortingModes.PositionCoef:
						return s1.AveragePositionNormalizedCoefs > s2.AveragePositionNormalizedCoefs ? 1 :
							Math.Abs(s1.AveragePositionNormalizedCoefs - s2.AveragePositionNormalizedCoefs) <
							Const.Tolerance ? 0 : -1;
					case SortingModes.Coefficeint:
						var coef = factor as Coefficient;
						if (coef == null)
							throw new ArgumentNullException(nameof(factor));
						var v1 = s1.NormalizedCoefficientsValues[coef];
						var v2 = s2.NormalizedCoefficientsValues[coef];
						if (v1.HasValue && v2.HasValue)
							return v1 < v2 ? 1 : Math.Abs(v1.Value - v2.Value) < Const.Tolerance ? 0 : -1;
						if (v1.HasValue && !v2.HasValue)
							return -1;
						if (!v1.HasValue && v2.HasValue)
							return 1;
						return 0;
					case SortingModes.Metric:
						var metric = factor as Metric;
						if (metric == null)
							throw new ArgumentNullException(nameof(factor));
						var m1 = s1.MetricsValues[metric];
						var m2 = s2.MetricsValues[metric];
						return m1 < m2 ? 1 : Math.Abs(m1 - m2) < Const.Tolerance ? 0 : -1;
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
				m_selectedList = GetList(StockListNamesEnum.Tinkoff);
				LoadNewListInComboBox(m_selectedList.StList);
			}
		}

		private static StockList GetList(StockListNamesEnum name)
		{
			return MainClass.PossibleStockList.First(l => l.Name == name);
		}

		#endregion

		private async void ButtonCkechTinkoff_Click(object sender, EventArgs e)
		{
			await MainClass.GetStocksList(new FormsTextReporter(labelRemainingTime),
				new FormsProgressReporter(progressBar), false);
		}

	}
}