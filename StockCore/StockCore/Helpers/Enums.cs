using System;

namespace StocksAnalyzer.Helpers
{
	[Flags]
	public enum SortingModesEnum
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
	public enum AvailableLanguagesEnum
	{
		Russian,
		English
	}
}
