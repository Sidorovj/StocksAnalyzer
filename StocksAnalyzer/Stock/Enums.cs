using System;

namespace StocksAnalyzer
{
	/// <summary>
	/// На каком рынке торгуется акция
	/// </summary>
	[Flags]
	enum StockMarketLocation
	{
		Russia = 0,
		Usa = 10,
		London = 20
	}

	/// <summary>
	/// Валюта, в которой торгуются акции
	/// </summary>
	[Flags]
	enum StockMarketCurrency 
	{
		Rub = 0,
		Usd = 10,
		Eur = 20
	}
    
    
}
