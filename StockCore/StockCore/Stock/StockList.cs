using System;
using System.Collections.Generic;
using StocksAnalyzer;

namespace StockCore.Stock
{
	[Serializable]
	internal class StockListOld
	{
		private Func<IEnumerable<StocksAnalyzer.StockOld>, IEnumerable<StocksAnalyzer.StockOld>> Selector { get; }

		public IEnumerable<StocksAnalyzer.StockOld> List => Selector(MainClass.Stocks);

		internal StockListOld(Func<IEnumerable<StocksAnalyzer.StockOld>, IEnumerable<StocksAnalyzer.StockOld>> selector)
		{
			Selector = selector;
		}
	}
}
