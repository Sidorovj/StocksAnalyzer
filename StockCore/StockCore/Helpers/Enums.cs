using System;

namespace StocksAnalyzer.Helpers
{
	[Flags]
	public enum SortingModes
	{
		Metric,
		Coefficeint,
		PositionAll,
		PositionMetric,
		PositionCoef
	}
}
