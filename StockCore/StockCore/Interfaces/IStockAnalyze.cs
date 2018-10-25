using System.Collections.Generic;
using StocksAnalyzer;

namespace StockCore.Interfaces
{
	public interface IStockAnalyze
	{
		void Analyze(StockListNamesEnum listName, List<StocksAnalyzer.Data.Stock> list);

		void AnalyzeAll();
	}
}
