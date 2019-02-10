using System;
using System.Collections.Generic;
using StocksAnalyzer;

namespace StockCore.Stock
{
	[Serializable]
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

		public override string ToString()
		{
			return Name.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is StockList m)
				return Equals(m);
			return false;
		}

		public bool Equals(StockList m)
		{
			return m.Name == Name;
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

	}
}
