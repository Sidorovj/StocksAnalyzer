using System;
using StocksAnalyzer.WinForms;
using StocksAnalyzer.Core.Interfaces;

namespace StocksAnalyzer
{

	static class LogWriter
	{
		private static readonly IReportText s_reporter = new FormsLoggerReporter();
		/// <summary>
		/// Записать лог в текстБокс на форме
		/// </summary>
		/// <param name="text">Строки лога</param>
		/// <param name="writeLogToFile">Записывать лог</param>
		public static void WriteLog(string text, bool writeLogToFile = true)
		{
			if (writeLogToFile)
				Logger.Log.Info(text);
			text = $"{DateTime.Now:HH-mm-ss}:  {text} {Environment.NewLine}";
			s_reporter.Text = text;
		}

		public static void WriteLog(Exception ex)
		{
			Logger.Log.Error(ex);
			WriteLog($"Ошибка в {ex.TargetSite.Name}: {ex.Message.Substring(0, ex.Message.Length > 40 ? 40 : ex.Message.Length)}", false);
		}
	}

}
