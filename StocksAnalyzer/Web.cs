using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace StocksAnalyzer
{

    static class Web
    {
        static public string ExchangeRatesUrl { get { return "http://api.fixer.io/latest?base=RUB"; } }
        static public string GetStocksListUrl_Russia1 { get { return "https://ru.investing.com/equities/russia"; } }
        static public string GetStocksListUrl_Russia2 { get { return "http://stocks.investfunds.ru/quotes/main/?&start={num}#beginf"; } }
        
        static public string GetStocksListUrl_USA_nyse { get { return "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nyse&render=download"; } }
        static public string GetStocksListUrl_USA_nasdaq { get { return "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download"; } }
        //static public string getStocksListUrl_London { get { return "http://www.nasdaq.com/screening/companies-by-name.aspx?letter=0&exchange=nasdaq&render=download"; } }
        static public string GetStockDataUrl_USA { get { return "https://finance.yahoo.com/quote/{}/key-statistics?p="; } }
        static public string GetStockDataUrl_Russia { get { return "https://ru.investing.com/equities/"; } }
        

        public static string POST(string url, NameValueCollection values)
        {
            string resp = "";
            using (var client = new WebClient())
            {
                //var values = new NameValueCollection();
                //values["thing1"] = "hello";
                //values["thing2"] = "world";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E)");
                client.Encoding = Encoding.UTF8;
                byte[] response = null;
                    response = client.UploadValues(url, values);
                if (response != null)
                    resp = Encoding.Default.GetString(response);
            }
            return resp;
        }

        public static Task<string> GETtask(string url)
        {
            return Task.Run(() =>
            {
                return GET(url);
            });

        }
        public static string GET(string url)
        {
            string _resp;
            using (var _client = new WebClient())
            {
                _client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Win64; x64; Trident/4.0; Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1) ; .NET CLR 2.0.50727; SLCC2; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; Tablet PC 2.0; .NET4.0C; .NET4.0E)");
                _client.Encoding = Encoding.UTF8;
                try
                {
                    _resp = _client.DownloadString(url);
                }
                catch(Exception er)
                {
                    MainClass.WriteLog(er.Message);
                    _resp = "";
                }
            }
            return _resp;
        }
        public static string ReadDownloadedFile(string url)
        {
            string _fileName = "usa_Stocks.dat";
            using (var client = new WebClient())
            {
                client.DownloadFile(url, _fileName);
            }
            string _text = File.ReadAllText(_fileName);
            return _text;
        }
    }
}
