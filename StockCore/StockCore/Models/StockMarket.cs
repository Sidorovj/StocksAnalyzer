using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer.Data
{
	public class StockMarket
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[ForeignKey("LocationId")]
		public StockMarketLocation Location { get; set; }
		[ForeignKey("CurrencyId")]
		public StockMarketCurrency Currency { get; set; }


		//TODO: peredelat v dictionary
		private static double s_exchangeRateRubToUsd; 
		private static double s_exchangeRateRubToEur;

		public StockMarket(StockMarketLocationEnum loc, StockMarketCurrencyEnum curr)
		{
			Location = loc;
			Currency = curr;
		}

		static StockMarket()
		{
			RefreshExchangeRates();
		}

		public static void RefreshExchangeRates()
		{
			string response = Web.Get(Web.ExchangeRatesUrl).Result;
			JObject rates = JObject.Parse(response);
			s_exchangeRateRubToEur = rates["rates"]["RUB"].Value<double>();
			s_exchangeRateRubToUsd = s_exchangeRateRubToEur / rates["rates"]["USD"].Value<double>();
		}

		/// <summary>
		/// Получить текущий курс обмена рубля К валюте
		/// </summary>
		/// <param name="toCurr">Какая валюта</param>
		/// <returns>Курс обмена</returns>
		public static double GetExchangeRates(StockMarketCurrencyEnum toCurr)
		{
			switch (toCurr)
			{
				case StockMarketCurrencyEnum.Usd:
					return s_exchangeRateRubToUsd;
				case StockMarketCurrencyEnum.Eur:
					return s_exchangeRateRubToEur;
			}
			throw new KeyNotFoundException();
		}

	}
}