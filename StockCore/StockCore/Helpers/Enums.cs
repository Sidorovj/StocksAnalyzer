using System;

namespace StocksAnalyzer.Helpers
{
	[Flags]
	public enum SortingModes
	{
		Name,
		Price,
		Metric,
		Coefficeint,
		PositionAll,
		PositionMetric,
		PositionCoef
	}

	[Flags]
	public enum AvailableLanguages
	{
		Russian,
		English
	}
}
