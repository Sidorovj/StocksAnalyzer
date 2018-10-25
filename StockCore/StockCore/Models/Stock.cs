using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using StockCore.Data;

namespace StocksAnalyzer.Data
{
	public class Stock
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public DateTime ModifiedOn { get; set; } = DateTime.Now;

		[ForeignKey("MarketId")]
		public StockMarket Market { get; set; }

		public string LinkToGetInfo { get; set; }

		[Required]
		public string Name { get; set; }

		public string Symbol { get; set; }

		[Required, Range(0, 1e10)]
		public double Price { get; set; }

		public bool? IsOnTinkoff { get; set; }

		public ICollection<CoefficientValue> CoefficientValues { get; set; }
		public ICollection<MetricValue> MetricValues { get; set; }



		public async Task UnderstandIsItOnTinkoff()
		{
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

			if (jsonReponse?["payload"]?["total"] != null && (int)jsonReponse["payload"]["total"] >= 0)
				for (var j = 0; j < (int)jsonReponse["payload"]?["values"].Count(); j++)
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