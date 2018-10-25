using System.Threading;

namespace StockCore.Helpers
{
	public class GlobalHelper
	{
		public static string CurrentCulture => Thread.CurrentThread.CurrentUICulture.Name;

		public static string DefaultCulture => "ru-RU";
	}
}
