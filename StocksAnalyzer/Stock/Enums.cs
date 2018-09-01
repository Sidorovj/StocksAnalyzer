using System;

namespace StocksAnalyzer
{
	[Flags]
	enum StockMarketLocation
	{
		Russia = 0,
		Usa = 10,
		London = 20
	}

	[Flags]
	enum StockMarketCurrency //валюта, в которой торгуются акции
	{
		Rub = 0,
		Usd = 10,
		Eur = 20
	}
    
    
}
