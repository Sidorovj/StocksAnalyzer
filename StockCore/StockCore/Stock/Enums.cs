using System;

namespace StocksAnalyzer
{
	/// <summary>
	/// На каком рынке торгуется акция
	/// </summary>
	[Flags]
	public enum StockMarketLocationEnum
	{
		Russia = 0,
		Usa = 10,
		London = 20
	}

	/// <summary>
	/// Валюта, в которой торгуются акции
	/// </summary>
	[Flags]
	public enum StockMarketCurrencyEnum
	{
		Rub = 0,
		Usd = 10,
		Eur = 20
	}

	//[Flags]
	//public enum StockListNamesEnum
	//{
	//	All,
	//	Tinkoff,
	//	Usa,
	//	Rus,
	//	Starred
	//}


}
