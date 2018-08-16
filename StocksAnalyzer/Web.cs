using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace StocksAnalyzer
{

    static class Web
    {
        public static string ExchangeRatesUrl => "http://api.fixer.io/latest?base=RUB";
        public static string GetStocksListUrlRussia1 => "https://ru.investing.com/equities/russia";
        public static string GetStocksListUrlRussia2 => "http://stocks.investfunds.ru/quotes/main/?&start={num}#beginf";

        public static string GetStocksListUrlUsaNyse => "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nyse&render=download";

        public static string GetStocksListUrlUsaNasdaq => "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download";

        //static public string getStocksListUrl_London { get { return "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download"; } }
        public static string GetStockDataUrlUsa => "https://finance.yahoo.com/quote/{}/key-statistics?p=";

        public static string GetStockDataUrlRussia => "https://ru.investing.com/equities/";


        public static string Post(string url, NameValueCollection values)
        {
            string resp = "";
            using (var client = new WebClient())
            {
                //var values = new NameValueCollection();
                //values["thing1"] = "hello";
                //values["thing2"] = "world";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E)");
                client.Encoding = Encoding.UTF8;
                var response = client.UploadValues(url, values);
                if (response != null)
                    resp = Encoding.Default.GetString(response);
            }
            return resp;
        }

        public static Task<string> GeTtask(string url)
        {
            return Task.Run(() => Get(url));

        }
        public static string Get(string url)
        {
            string resp;
            using (var client = new WebClient())
            {
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E)");
                client.Encoding = Encoding.UTF8;
                try
                {
                    resp = client.DownloadString(url);
                }
                catch(Exception er)
                {
                    MainClass.WriteLog(er.Message);
                    resp = "";
                }
            }
            return resp;
        }
        public static string ReadDownloadedFile(string url)
        {
            string fileName = "usa_Stocks.dat";
            using (var client = new WebClient())
            {
                client.DownloadFile(url, fileName);
            }
            string text = File.ReadAllText(fileName);
            return text;
        }
    }
}
