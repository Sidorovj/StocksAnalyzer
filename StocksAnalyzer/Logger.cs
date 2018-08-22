using System;
using NLog;

namespace StocksAnalyzer
{
	internal static class Logger
	{
		public static  NLog.Logger Log { get; }

		static Logger()
		{
			Log = LogManager.GetCurrentClassLogger();
		}
	}
}
