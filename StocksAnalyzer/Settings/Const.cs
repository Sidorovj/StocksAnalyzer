
namespace StocksAnalyzer
{
	class Const
	{
		public static string SettingsDirName { get; } = @"Settings";
		public static string AnalysisDirName { get; } = @"Analysis";
		public static string HistoryDirName { get; } = @"History";
		public static string ReportDirName { get; } = @"Reports";
	    public const string StockListFilePath = "stockList.dat";
	    public static string CoefficientsSettings => "Coefficients.csv";
    }
}
