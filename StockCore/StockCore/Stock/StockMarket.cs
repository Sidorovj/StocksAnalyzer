using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{

	/// <summary>
	/// Представляет собой рынок, на котором обращаются акции
	/// </summary>
	[Serializable]
	public sealed class StockMarket
	{
		public StockMarketLocation Location { get; }
		public StockMarketCurrency Currency { get; }

		private static readonly double s_exchangeRateRubToUsd; // Переделать в словарь
		private static readonly double s_exchangeRateRubToEur;

		public StockMarket(StockMarketLocation loc, StockMarketCurrency curr)
		{
			Location = loc;
			Currency = curr;
		}

		static StockMarket()
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
