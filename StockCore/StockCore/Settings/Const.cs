
using System;
using System.IO;

namespace StocksAnalyzer
{
	public class Const
	{
		public const double Tolerance = 1e-7;
		public static string SettingsDirName { get; } = @"Settings";
		public static string AnalysisDirName { get; } = @"Analysis";
		public static string ToRestoreDirName { get; } = @"_ToRestoreState";

		public const string StockListFilePath = "stockList.dat";
	    public static string CoefficientsSettings => "Coefficients.csv";
		public static string ReportFileName 
		{
			get
			{
				if (!Directory.Exists(ReportDirName))
					Directory.CreateDirectory(ReportDirName);
				return $"{ReportDirName}/Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
			}
		}

		
		private static string ReportDirName { get; } = @"Reports";
	}
}
