using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StockCore.Interfaces;
using StockCore.Stock;
using StocksAnalyzer.Core.Interfaces;
using StocksAnalyzer.Helpers;

namespace StocksAnalyzer
{

	/// <summary>
	/// Must be a singleton
	/// </summary>
	internal class MainClass : IStockLoader, IAdministrating
	{
		private static string s_reportFileName;
		private static int s_doneEventsCount;
		private static readonly object s_rusStockLoaderLocker = new object();

		public static AvailableLanguagesEnum Language = AvailableLanguagesEnum.Russian;

		internal static ReadOnlyDictionary<StockListNamesEnum, StockListOld> PossibleStockList { get; } =
			new ReadOnlyDictionary<StockListNamesEnum, StockListOld>(new Dictionary<StockListNamesEnum, StockListOld>
			{
				[StockListNamesEnum.All] = new StockListOld(list => list),
				[StockListNamesEnum.Tinkoff] = new StockListOld(list => list.Where(s => s.IsOnTinkoff)),
				[StockListNamesEnum.Rus] = new StockListOld(
					list => list.Where(s => s.Market.Location == StockMarketLocationEnum.Russia)
				),
				[StockListNamesEnum.Usa] = new StockListOld(
					list => list.Where(s => s.Market.Location == StockMarketLocationEnum.Usa)
				),
				[StockListNamesEnum.Starred] = new StockListOld(list => list.Where(s => s.IsStarred))
			});

		internal static List<StockOld> Stocks { get; private set; } = new List<StockOld>();
		
		#region Methods:public


		public static void OpenReportIfExists()
		{
			if (s_reportFileName != "")
			{
				Process.Start(s_reportFileName);
			}
		}

		///// <summary>
		///// Загружает список из файла
		///// </summary>
		///// <param name="path"></param>
		//public static void LoadStockListFromFile(string path = Const.StockListFilePath)
		//{
		//	string fullPath = $"{Const.ToRestoreDirName}/{path}";
		//	if (!Directory.Exists(Const.ToRestoreDirName) || !File.Exists(fullPath))
		//	{
		//		Logger.Log.Error(@"Не могу найти файл для десериализации");
		//		return;
		//	}

		//	Serializer ser = new Serializer(fullPath);
		//	Stocks = (List<Stock>)ser.Deserialize() ?? new List<Stock>();

		//	foreach (var kpv in PossibleStockList)
		//	{
		//		Stock.CoefHasValueCount.Add(kpv.Key, new Dictionary<Coefficient, int>(Coefficient.CoefficientList.Count));
		//		foreach (var coef in Coefficient.CoefficientList)
		//		{
		//			Stock.CoefHasValueCount[kpv.Key][coef] = (from st in Stocks
		//													  where st.NormalizedCoefficientsValues.ContainsKey(coef) &&
		//															st.NormalizedCoefficientsValues[coef].HasValue
		//													  select 0).Count();
		//		}
		//	}
		//	Stock.AllStocksAnalyzed = Stock.CoefHasValueCount.Any(k => k.Value.Any(num => num.Value > 0));
		//}

		///// <summary>
		///// Сериализует список акций в файл
		///// </summary>
		///// <param name="path">Путь к файлу</param>
		//public static void WriteStockListToFile(string path = Const.StockListFilePath)
		//{
		//	if (!Directory.Exists(Const.ToRestoreDirName))
		//		Directory.CreateDirectory(Const.ToRestoreDirName);
		//	Serializer ser = new Serializer($"{Const.ToRestoreDirName}/{path}");
		//	ser.Serialize(Stocks);
		//}


		/// <summary>
		/// Загрузить данные по акциям
		/// </summary>
		/// <param name="stockList">Список акций</param>
		/// <param name="lbl">Лейбл с формы</param>
		/// <param name="bar">Прогресс-бар</param>
		/// <returns></returns>
		public async Task LoadStocksData(IEnumerable<Data.Stock> stockList, IReportText lbl, IReportProgress bar)
		{
			//var lst = stockList.ToList();
			//int count = lst.Count, doneEvents = 0;
			//var report = new StringBuilder();
			//Task[] tasks = new Task[count];

			//Stopwatch stwatch = Stopwatch.StartNew();

			//for (int i = 0; i < lst.Count; i++)
			//{
			//	var i1 = i;
			//	tasks[i] = Task.Run(async () =>
			//	{
			//		try
			//		{
			//			await GetStockData(lst[i1]);
			//		}
			//		catch (Exception er)
			//		{
			//			Logger.Log.Error(
			//				$"Не удалось получить инфу по {lst[i1].Name}: {er.Message}\r\n{er.StackTrace}");
			//			report.Append($"{lst[i1].Name};");
			//		}

			//		Interlocked.Increment(ref doneEvents);
			//		if (i1 % 10 != 0)
			//			return;

			//		if (lbl != null)
			//		{
			//			double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)doneEvents / count) - 1) / 60.0;
			//			mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;
			//			lbl.Text = $@"Обработано {doneEvents} / {count}. Расчетное время: {
			//					(mins >= 1 ? Math.Floor(mins) + " мин " : "")
			//				}{Math.Floor((mins - Math.Floor(mins)) * 100)} с";
			//		}
			//		if (bar != null)
			//			bar.Value = doneEvents * 100 / count;
			//	});
			//}
			//await Task.WhenAll(tasks);

			//stwatch.Stop();
			//if (bar != null)
			//	bar.Value = 100;
			//if (lbl != null)
			//	lbl.Text = @"Готово.";
			//MakeReportAndSaveToFile(lst, report.ToString());
		}

		/// <summary>
		/// Получить данные по акции из интернета
		/// </summary>
		/// <param name="st">Акция</param>
		public async Task GetStockData(Data.Stock st)
		{
			//string htmlCode;
			//if (st.Market.Location == StockMarketLocationEnum.Usa)
			//{
			//	htmlCode = await Web.Get(string.Format(Web.GetStockDataUrlUsa, st.Symbol));
			//	if (htmlCode.IndexOf(">Trailing P/E</span>", StringComparison.Ordinal) < 0)
			//	{
			//		Logger.Log.Warn($"На сайте (USA) нет данных для {st.Symbol}");
			//		return;
			//	}

			//	foreach (var coef in Coefficient.CoefficientList)
			//	{
			//		if (coef.IsUSA || coef.IsCommon)
			//		{
			//			if (!string.IsNullOrEmpty(coef.SearchInHTML_USA))
			//				st[coef] = GettingYahooData(coef.SearchInHTML_USA, ref htmlCode);
			//			else
			//				st.CalculateCoef(coef);
			//		}
			//	}

			//	st.LastUpdate = DateTime.Now;
			//}
			//else if (st.Market.Location == StockMarketLocationEnum.Russia)
			//{
			//	if (string.IsNullOrEmpty(st.LinkToGetInfo))
			//	{
			//		Logger.Log.Error($"Нет ссылки для получения инфы для {st}");
			//		return;
			//	}

			//	lock (s_rusStockLoaderLocker)
			//	{
			//		htmlCode = Web.Get(Web.GetStockDataUrlRussia + st.LinkToGetInfo).Result;
			//		if (htmlCode.IndexOf("<span class=\"\">Коэффициент цена/прибыль", StringComparison.Ordinal) < 0)
			//		{
			//			Logger.Log.Warn($"На сайте (RUS) нет данных для {st.Symbol}");
			//			return;
			//		}

			//		foreach (var coef in Coefficient.CoefficientList)
			//		{
			//			if (coef.IsCommon || coef.IsRus)
			//				st[coef] = GettingInvestingComData(coef.SearchInHTML_Rus, ref htmlCode,
			//					coef.SearchInHTMLAppendix_Rus);
			//		}

			//		st.LastUpdate = DateTime.Now;
			//	}
			//}

		}


		/// <summary>
		/// Загрузить список всех акций
		/// </summary>
		public async Task GetStocksList(IReportText lbl, IReportProgress bar, bool onlyCheckForTinkoff = false)
		{
			Stocks.Clear();
			try
			{
				if (!onlyCheckForTinkoff)
				{
					var getRus = Task.Run(GetRussianStocks);
					var getUsa = Task.Run((Action)GetUsaStocks);

					await Task.WhenAll(getRus, getUsa);

					CheckForRepeatsAndSort();
				}

				int count = Stocks.Count;
				s_doneEventsCount = 0;

				Stopwatch stwatch = Stopwatch.StartNew();
				var tinkoffCheck = Task.Run(async () => await CheckAllForTinkoff(count));

				while (true)
				{
					double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)s_doneEventsCount / count) - 1) / 60.0;
					mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;

					if (lbl != null)
						lbl.Text =
							$@"Обработано {s_doneEventsCount} / {count}. Расчетное время: {
									(mins >= 1 ? Math.Floor(mins) + " мин " : "")
								}{Math.Floor((mins - Math.Floor(mins)) * 100)} с";
					if (bar != null)
						bar.Value = s_doneEventsCount * 100 / count;
					if (tinkoffCheck.IsCompleted)
					{
						break;
					}

					await Task.Delay(5 * 1000);
					if (tinkoffCheck.IsCanceled || tinkoffCheck.IsFaulted)
						break;
				}
			}
			catch (Exception ex)
			{
				Logger.Log.Error(ex);
				throw;
			}
		}

		#endregion



		#region Methods:private

		/// <summary>
		/// Составить отчет по списку акций и записать в файл
		/// </summary>
		/// <param name="stockLst">Список акций</param>
		/// <param name="failedStocks">Акции, который не удалось загрузить</param>
		private static void MakeReportAndSaveToFile(List<StockOld> stockLst, string failedStocks)
		{
			StringBuilder report = new StringBuilder();
			report.Append($"Не удалось загрузить акции:;\n{failedStocks}");
			foreach (var coef in CoefficientOld.CoefficientList)
			{
				var failedLst = stockLst.Where(st => Math.Abs(st[coef] ?? 0) < Const.Tolerance).ToList();
				var helpSt = string.Join(";", failedLst);
				var numRes = failedLst.Count;
				report.Append($"{coef};Заполнен в {stockLst.Count - numRes}/{stockLst.Count};{helpSt}\r\n");
			}

			s_reportFileName = Const.ReportFileName;
			using (var sr = new StreamWriter(s_reportFileName, true, Encoding.UTF8))
			{
				sr.Write(report.ToString());
			}
		}

		private static void CheckForRepeatsAndSort()
		{
			var temp = Stocks;
			Stocks = new List<StockOld>(temp.Count / 3);
			foreach (var stock in temp)
			{
				if (temp.FirstOrDefault(st => st.Name == stock.Name) == null)
					Stocks.Add(stock);
			}
			//Отсортируем по алфавиту
			Stocks.Sort((s1, s2) => string.CompareOrdinal(s1.Name, s2.Name));
		}

		private static async Task CheckAllForTinkoff(int count)
		{
			List<Task> tinkoffTask = new List<Task>(3500);
			foreach (var stock in Stocks)
			{
				tinkoffTask.Add(stock.UnderstandIsItOnTinkoff());
			}

			while (s_doneEventsCount < count)
			{
				var task = await Task.WhenAny(tinkoffTask);
				tinkoffTask.Remove(task);
				if (task.IsFaulted && task.Exception != null)
				{
					Logger.Log.Error(task.Exception.InnerExceptions);
				}
				Interlocked.Increment(ref s_doneEventsCount);
			}
		}

		/// <summary>
		/// Загрузить в Stocks акции с рус. биржы
		/// </summary>
		private static async Task GetRussianStocks()
		{
			int start = 0, end = 100;

			string htmlCode = await Web.Get(string.Format(Web.GetStocksListUrlRussia, start, end));
			JObject resp = JObject.Parse(htmlCode);
			end = resp["payload"]?["total"]?.Value<int>() + 1 ?? -1;
			if (end < 0)
				throw new Exception("Неверный ответ в GetRussianStocks");
			ExecuteTinkoffResponse(resp);

			int taskCount = (int)Math.Ceiling((double)(end - 100) / 100);
			Task[] tasks = new Task[taskCount];
			for (var i = 0; i < taskCount; i++)
			{
				var i1 = i;
				tasks[i] = Task.Run(async () =>
			   {
				   start = (i1 + 1) * 100;
				   htmlCode = await Web.Get(string.Format(Web.GetStocksListUrlRussia, start, (i1 + 2) * 100 < end ? (i1 + 2) * 100 : end));

				   ExecuteTinkoffResponse(JObject.Parse(htmlCode));
			   });
			}
			await Task.WhenAll(tasks);

		}

		private static void ExecuteTinkoffResponse(JObject resp)
		{
			foreach (var st in (JArray)resp["payload"]["values"])
			{
				var currencyToken = st["prices"]["buy"] ?? st["prices"]["close"] ?? st["prices"]["last"];
				if (currencyToken == null)
				{
					Logger.Log.Warn($"Cannot resolve prices in {nameof(ExecuteTinkoffResponse)} for {nameof(st)}={st}");
					continue;
				}
				if (currencyToken["currency"].Value<string>() != "RUB")
					continue;

				var newStock = new StockOld(st["symbol"]["showName"].Value<string>(), currencyToken["value"].Value<double>(),
					new StockMarket(StockMarketLocationEnum.Russia, StockMarketCurrencyEnum.Rub));
				Stocks.Add(newStock);
			}
		}

		/// <summary>
		/// Загрузить в Stocks акции с амер. биржы
		/// </summary>
		private static void GetUsaStocks()
		{
			string[] htmlCode =
				(File.ReadAllText(@"C:/temp/companylist.csv") + File.ReadAllText(@"C:/temp/companylist1.csv"))
				.Replace("\",\"", "|")
				.Replace("\"", "")
				.Split('\n');

			foreach (var s in htmlCode)
			{
				if (s.StartsWith("Symbol"))
					continue;
				var parameters = s.Split('|');
				if (parameters.Length < 2)
					continue;
				string name = parameters[1], symb = parameters[0];
				double? price = parameters[2].ParseCoefStrToDouble();
				if (price.HasValue && price.Value > 0)
				{
					var newStock = new StockOld(name, price.Value, new StockMarket(StockMarketLocationEnum.Usa, StockMarketCurrencyEnum.Usd), symb);
					Stocks.Add(newStock);
				}
			}
		}


		/// <summary>
		/// Получает значение из разметки html сайта yahoo
		/// </summary>
		/// <param name="multiplicator">Название мультипликатора</param>
		/// <param name="htmlCode">Код</param>
		/// <returns>Значение</returns>
		private static double? GettingYahooData(string multiplicator, ref string htmlCode)
		{
			string temp = htmlCode.Substring(htmlCode.IndexOf(">" + multiplicator + "</span>", StringComparison.Ordinal));
			temp = temp.Substring(temp.IndexOf("<td class", StringComparison.Ordinal));
			temp = temp.Substring(temp.IndexOf(">", StringComparison.Ordinal) + 1);
			temp = temp.Substring(0, temp.IndexOf("</", StringComparison.Ordinal));
			if (temp.IndexOf(">", StringComparison.Ordinal) > 0)
				temp = temp.Substring(temp.IndexOf(">", StringComparison.Ordinal) + 1);
			return temp.ParseCoefStrToDouble();
		}

		/// <summary>
		/// Получает значение из разметки html сайта yahoo
		/// </summary>
		/// <param name="multiplicator">Название мультипликатора</param>
		/// <param name="htmlCode">Код html</param>
		/// <param name="appendix"></param>
		/// <returns>Значение</returns>
		private static double? GettingInvestingComData(string multiplicator, ref string htmlCode, string appendix = "")
		{
			string sp = "<span class=\"\">";
			string temp = htmlCode.Substring(htmlCode.IndexOf(sp + multiplicator, StringComparison.Ordinal));
			if (appendix != "")
				temp = temp.Substring(temp.IndexOf(appendix + "</i>", StringComparison.Ordinal));
			temp = temp.Substring(temp.IndexOf("<td>", StringComparison.Ordinal) + 4);
			return temp.Substring(0, temp.IndexOf("</td", StringComparison.Ordinal)).ParseCoefStrToDouble();
		}


		#endregion

		public Task CreateNewStocks(bool deleteOld = false)
		{
			throw new NotImplementedException();
		}

		public void CreateCoefficientsAndMetrics(bool deleteOld = false)
		{
			throw new NotImplementedException();
		}
	}
}
