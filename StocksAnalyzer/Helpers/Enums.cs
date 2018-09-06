using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksAnalyzer.Helpers
{
	[Flags]
	enum SortingModes
	{
		Metric,
		Coefficeint,
		PositionAll,
		PositionMetric,
		PositionCoef
	}
}
