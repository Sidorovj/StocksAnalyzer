using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{

	/// <summary>
	/// Представляет собой рынок, на котором обращаются акции
	/// </summary>
	[Serializable]
	internal class StockMarket
	{
		public StockMarketLocation Location { get; }
		public StockMarketCurrency Currency { get; }

		private static double s_exchangeRateRubToUsd; // Переделать в словарь
		private static double s_exchangeRateRubToEur;

		public StockMarket(StockMarketLocation loc, StockMarketCurrency curr)
		{
			Location = loc;
			Currency = curr;
		}


		/// <summary>
		/// Получение текущих курсов обмена
		/// </summary>
		public static async Task InitializeCurrencies()
		{
			string response = await Web.Get(Web.ExchangeRatesUrl).ConfigureAwait(false);
			JObject rates = JObject.Parse(response);
			s_exchangeRateRubToEur = rates["rates"]["RUB"].Value<double>();
			s_exchangeRateRubToUsd = s_exchangeRateRubToEur / rates["rates"]["USD"].Value<double>();
		}

		/// <summary>
		/// Получить текущий курс обмена рубля К валюте
		/// </summary>
		/// <param name="toCurr">Какая валюта</param>
		/// <returns>Курс обмена</returns>
		public static double GetExchangeRates(StockMarketCurrency toCurr)
		{
			switch (toCurr)
			{
				case StockMarketCurrency.Usd:
					return s_exchangeRateRubToUsd;
				case StockMarketCurrency.Eur:
					return s_exchangeRateRubToEur;
			}
			throw new KeyNotFoundException();
		}
	}

}
