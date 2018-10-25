using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StockCore.Interfaces;
using StockCore.Stock;

namespace StocksAnalyzer
{

	/// <summary>
	/// Акция и ее характеристики
	/// </summary>
	[Serializable]
	internal class StockOld
	{
		/// <summary>
		/// Связь коэффициента и количества акций, в которых он заполнен
		/// </summary>
		public static Dictionary<StockListNamesEnum, Dictionary<CoefficientOld, int>> CoefHasValueCount { get; } =
			new Dictionary<StockListNamesEnum, Dictionary<CoefficientOld, int>>();

		public static bool AllStocksAnalyzed = false;

		private static readonly Dictionary<string, string> s_namesToSymbolsRus = new Dictionary<string, string>();


		public bool IsStarred { get; set; }
		public string LinkToGetInfo => s_namesToSymbolsRus.ContainsKey(Name) ? s_namesToSymbolsRus[Name] : null;

		public DateTime LastUpdate { get; set; }
		public StockMarket Market { get; }

		[Key]
		// ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
		// Needs to have setter cause of entity framework DB migration
		public string Name { get; private set; }

		public string Symbol { get; }
		public string FullName => $"{Name} [{Market.Location}]";
		public bool IsOnTinkoff { get; private set; }
		public bool TinkoffScanned { get; private set; }
		public double Price { get; set; }

		/// <summary>
		/// Имя коэффициента к его значению
		/// </summary>
		public Dictionary<CoefficientOld, double?> CoefficientsValues { get; } =
			new Dictionary<CoefficientOld, double?>(CoefficientOld.CoefficientList.Count);

		public Dictionary<CoefficientOld, double?> NormalizedCoefficientsValues { get; } =
			new Dictionary<CoefficientOld, double?>(CoefficientOld.CoefficientList.Count);

		/// <summary>
		/// Название метрики к ее значению
		/// </summary>
		public Dictionary<MetricOld, double> MetricsValues { get; } = new Dictionary<MetricOld, double>();

		public ConcurrentDictionary<StockListNamesEnum, Dictionary<IFactor, int?>> ListToRatings { get; } =
			new ConcurrentDictionary<StockListNamesEnum, Dictionary<IFactor, int?>>();

		public ConcurrentDictionary<StockListNamesEnum, double> AveragePositionAll;
		public ConcurrentDictionary<StockListNamesEnum, double> AveragePositionMetric;
		public ConcurrentDictionary<StockListNamesEnum, double> AveragePositionNormalizedCoefs;


		static StockOld()
		{
			using (var fs = new FileStream($"{Const.SettingsDirName}/NamesToSymbols.csv", FileMode.Open))
			{
				using (var sr = new StreamReader(fs))
				{
					var lines = sr.ReadToEnd().Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
					foreach (var line in lines)
					{
						var splitted = line.Split(';');
						if (splitted.Length < 2)
							throw new NotSupportedException($"Wrong line {line}, fs.Position={fs.Position}");
						var name = splitted[0];
						var key = splitted[1];
						s_namesToSymbolsRus[name] = key;
					}
				}
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public void CalculateCoef(CoefficientOld coef)
		{
			this[coef] = coef.CalculateCoef(CoefficientsValues);
		}


		public double? this[CoefficientOld coef]
		{
			get => CoefficientsValues[coef];
			set => CoefficientsValues[coef] = value;
		}

		public StockOld(string name, double price, StockMarket mar, string symb = "")
		{
			Name = name;
			Price = price;
			Market = mar;
			Symbol = symb;
			LastUpdate = DateTime.Now;
			IsOnTinkoff = TinkoffScanned = mar.Location == StockMarketLocationEnum.Russia;
			foreach (var coef in CoefficientOld.CoefficientList)
			{
				CoefficientsValues[coef] = null;
			}
		}

		private StockOld()
		{
		}

		public async Task UnderstandIsItOnTinkoff()
		{
			if (TinkoffScanned)
				return;

			string nameToSearch = "";
			string[] suffixArray =
			{
				"", "ао", "ап", "деп.", "расп.", "inc", "inc.", "corp", "corp.", "ltd", "ltd.", "corporation",
				"incorporated", "plc", "group", "company"
			};
			foreach (var s in Name.ToLower().Split(' '))
			{
				if (!suffixArray.Contains(s))
				{
					if (s != "and" && s != "&")
						nameToSearch += s.Replace(",", "") + ' ';
				}
				else
					break;

				if (s.EndsWith(","))
					break;
			}

			JObject jsonReponse;

			while (true)
			{
				var urlFilter = nameToSearch.Split(' ')[0];
				var respStr =
					await Web.Get(
						"https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" +
						urlFilter);
				jsonReponse = JObject.Parse(respStr);
				if (jsonReponse?["status"]?.Value<string>() != "Error")
					break;
				if (jsonReponse["payload"]?["code"]?.Value<string>() == "RequestRateLimitExceeded")
				{
					await Task.Delay(60 * 1000);
				}
				else
				{
					throw new Exception($"{nameof(nameToSearch)}={nameToSearch}; {nameof(respStr)}={respStr}");
				}
			}

			if (jsonReponse?["payload"]?["total"] != null && (int) jsonReponse["payload"]["total"] >= 0)
				for (var j = 0; j < (int) jsonReponse["payload"]?["values"].Count(); j++)
				{
					var symbol = jsonReponse["payload"]?["values"]?[j]?["symbol"];
					if (NameOrDescriptionContains(
						nameToSearch,
						(string) symbol?["showName"],
						(string) symbol?["description"],
						(string) symbol?["fullDescription"]))
					{
						IsOnTinkoff = true;
						break;
					}

				}

			TinkoffScanned = true;
		}


		private bool NameOrDescriptionContains(string searchStr, params string[] arr)
		{
			var query = from s in arr.Where(s => !string.IsNullOrEmpty(s))
				let str = s.ToLower()
				where s != null && searchStr.Split(' ').All(str.Contains)
				select 0;
			return query.Any();
		}
	}
}
