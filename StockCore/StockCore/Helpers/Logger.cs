using NLog;

namespace StocksAnalyzer
{
	public static class Logger
	{
		public static  NLog.Logger Log { get; }

		static Logger()
		{
			Log = LogManager.GetCurrentClassLogger();
		}
	}
}
