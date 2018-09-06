using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{

	/// <summary>
	/// Акция и ее характеристики
	/// </summary>
	[Serializable]
	class Stock
	{
		/// <summary>
		/// Связь коэффициента и количества акций, в которых он заполнен
		/// </summary>
		public static Dictionary<Coefficient, int> CoefHasValueCount { get; } = new Dictionary<Coefficient, int>(Coefficient.CoefficientList.Count);

		public static bool AllStocksInListAnalyzed = false;


		public bool IsStarred { get; set; }
		public DateTime LastUpdate { get; set; }
		public StockMarket Market { get; }
		public string Name { get; }
		public string Symbol { get; }
		public string FullName => $"{Name} [{Market.Location}]";
		public bool IsOnTinkoff { get; private set; }
		public bool TinkoffScanned { get; private set; }
		public double Price { get; set; }

		/// <summary>
		/// Имя коэффициента к его значению
		/// </summary>
		private Dictionary<Coefficient, double?> CoefficientsValues { get; } = new Dictionary<Coefficient, double?>(Coefficient.CoefficientList.Count);
		public Dictionary<Coefficient, double?> NormalizedCoefficientsValues { get; } = new Dictionary<Coefficient, double?>(Coefficient.CoefficientList.Count);
		/// <summary>
		/// Название метрики к ее значению
		/// </summary>
		public Dictionary<string, double> MetricsValues { get; } = new Dictionary<string, double>();
		public Dictionary<string, int?> PositionInMetricAndCoef { get; } = new Dictionary<string, int?>();

		public double AveragePositionAll;
		public double AveragePositionMetric;
		public double AveragePositionNormalizedCoefs;

		public override string ToString()
		{
			return Name;
		}

		public void CalculateCoef(Coefficient coef)
		{
			this[coef] = coef.CalculateCoef(CoefficientsValues);
		}
		

		public double? this[Coefficient coef]
		{
			get => CoefficientsValues[coef];
			set => CoefficientsValues[coef] = value;
		}

		public Stock(string name, double price, StockMarket mar, string symb = "")
		{
			Name = name;
			Price = price;
			Market = mar;
			Symbol = symb;
			LastUpdate = DateTime.Now;
			IsOnTinkoff = TinkoffScanned = mar.Location == StockMarketLocation.Russia;
			foreach (var coef in Coefficient.CoefficientList)
			{
				CoefficientsValues[coef] = null;
			}
		}

		public async Task UnderstandIsItOnTinkoff()
		{
			if (TinkoffScanned)
				return;

			string nameToSearch = "";
			string[] suffixArray = { "", "ао", "ап", "деп.", "расп.", "inc", "inc.", "corp", "corp.", "ltd", "ltd.", "corporation", "incorporated", "plc", "group", "company" };
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
				var respStr = await Web.Get("https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" + urlFilter);
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

			if (jsonReponse?["payload"]?["total"] != null && (int)jsonReponse["payload"]["total"] >= 0)
				for (var j = 0; j < (int)jsonReponse["payload"]["total"]; j++)
				{
					var symbol = jsonReponse["payload"]?["values"]?[j]?["symbol"];
					if (NameOrDescriptionContains(
						nameToSearch,
						(string)symbol?["showName"],
						(string)symbol?["description"],
						(string)symbol?["fullDescription"]))
					{
						IsOnTinkoff = true;
						break;
					}
				}
			TinkoffScanned = true;
		}


		private bool NameOrDescriptionContains(string searchStr, params string[] arr)
		{
			var query = from s in arr
				let str = s.ToLower()
				where s != null && searchStr.Split(' ').All(str.Contains)
				select 0;
			return query.Any();
		}
	}
}
