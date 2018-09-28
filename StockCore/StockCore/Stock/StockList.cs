using System;
using System.Collections.Generic;
using StocksAnalyzer;

namespace StockCore.Stock
{
	public class StockList
	{
		public StockListNamesEnum Name;

		private Func<IEnumerable<StocksAnalyzer.Stock>, IEnumerable<StocksAnalyzer.Stock>> Selector { get; }

		public IEnumerable<StocksAnalyzer.Stock> StList => Selector(MainClass.Stocks);

		internal StockList(StockListNamesEnum name,
			Func<IEnumerable<StocksAnalyzer.Stock>, IEnumerable<StocksAnalyzer.Stock>> selector)
		{
			Name = name;
			Selector = selector;
		}
	}
}
