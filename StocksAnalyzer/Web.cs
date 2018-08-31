using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace StocksAnalyzer
{
	static class Web
	{
		private static readonly CookieContainer s_cookContainer = new CookieContainer();

		public static string ExchangeRatesUrl => @"http://data.fixer.io/api/latest?access_key=d7b80760e664065395dc2db532327183&symbols=RUB,USD";

		public static string GetStocksListUrlRussia =>
			@"https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start={0}&end={1}";

		[Obsolete]
		public static string GetStocksListUrlRussia1 => @"https://ru.investing.com/equities/russia";
		[Obsolete]
		public static string GetStocksListUrlRussia2 => @"http://stocks.investfunds.ru/quotes/main/?&start={num}#beginf";

		public static string GetStocksListUrlUsaNyse => @"http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nyse&render=download";

		public static string GetStocksListUrlUsaNasdaq => @"http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download";

		//static public string getStocksListUrl_London { get { return "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download"; } }
		public static string GetStockDataUrlUsa => @"https://finance.yahoo.com/quote/{}/key-statistics?p=";

		public static string GetStockDataUrlRussia => @"https://ru.investing.com/equities/";
		public static int MaxTriesCount => 3;


		public static async Task<string> Get(string url, int triesCount = 0)
		{
			var webReq = WebRequest.CreateHttp(url);
			webReq.CookieContainer = s_cookContainer;
			webReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.103 YaBrowser/18.7.0.2695 Yowser/2.5 Safari/537.36";
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)await webReq.GetResponseAsync())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException($"Response stream is null, url={url}")))
				{
					try
					{
						return await reader.ReadToEndAsync();
					}
					catch (Exception ex)
					{
						Logger.Log.Error($"Requested url: {url}\r\nError: {ex.Message}");
						throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is WebException wex)
					using (var stream = wex.Response?.GetResponseStream())
					{
						if (stream != null)
							Logger.Log.Error($"Requested url: {url}\r\n{nameof(triesCount)}={triesCount}\r\nResponseStream: {new StreamReader(stream).ReadToEnd()}");
					}
				else
				{
					Logger.Log.Error($"Requested url: {url}\r\n{nameof(triesCount)}={triesCount}\r\nMessage: {ex.Message}");
				}

				if (triesCount < MaxTriesCount)
					return await Get(url, triesCount + 1);
				throw;
			}
		}

		public static string ReadDownloadedFile(string url)
		{
			string fileName = "usa_Stocks.dat";
			using (var client = new WebClient())
			{
				client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.3396.103 YaBrowser/18.7.0.2695 Yowser/2.5 Safari/537.36");
				client.Encoding = Encoding.UTF8;
				client.DownloadFile(url, fileName);
			}
			string text = File.ReadAllText(fileName);
			return text;
		}
	}
}
