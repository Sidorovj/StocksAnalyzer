using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{
    enum StockMarketLocation
    {
        Russia=0,
        USA=10,
        London=20,
        Other=30
    }
    enum StockMarketCurrency //валюта, в которой торгуются акции
    {
        RUB=0,
        USD=10,
        EUR=20
    }

    [Serializable]
    class StockMarket
    {
        private StockMarketLocation _location;
        private StockMarketCurrency _currency;
        private static double _RubToUsd; // Переделать в словарь
        private static double _RubToEur;
        public StockMarket(StockMarketLocation loc, StockMarketCurrency curr)
        {
            _location = loc;
            _currency = curr;
            //getExchangeRates();
        }
        public async static void initializeCurrencies()
        {
            string resp = await Web.GET(Web.exchangeRatesUrl);
            var curr = resp.Split(',');
            // Переделать распарсивание в JSON
            for (int i = 0; i < curr.Length; i++)
            {
                if (curr[i].StartsWith("\"USD"))
                {
                    _RubToUsd = 1.0 / curr[i].Substring(curr[i].IndexOf(':') + 1).getDoubleNum();
                }
                else if (curr[i].StartsWith("\"EUR"))
                {
                    _RubToEur = 1.0 / curr[i].Substring(curr[i].IndexOf(':') + 1).getDoubleNum();
                }

            }
        }
        public static double getExchangeRates(StockMarketCurrency toCurr = StockMarketCurrency.USD) // Найдем курс рубля к запрошенной валюте
        {
            switch (toCurr)
            {
                case StockMarketCurrency.USD:
                    return _RubToUsd;
                case StockMarketCurrency.EUR:
                    return _RubToEur;
            }
            return -1;
        }
        public StockMarketLocation Location
        {
            get { return _location; }
            set { _location = value; }
        }
        public StockMarketCurrency Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
    }

    [Serializable]
    class Stock
    {
        private double _price;
        private double _priceToEquity;
        private double _priceToSales;
        private double _priceToBook;
        private double _EVtoEBITDA;
        private double _debtToEBITDA; // Долг к EBITDA
        private double _ROE;
        private double _EPS;
        public double MainPE { get; set; }
        public double Main { get; set; }
        public double MainAll { get; set; }
        public int RateMainPE { get; set; }
        public int RateMain { get; set; }
        public int RateMainAll { get; set; }

        public double QEG { get; set; }
        public double ProfitMarg { get; set; }
        public double ProfitMarg5ya { get; set; }
        public double OperMarg { get; set; }
        public double OperMarg5ya { get; set; }
        public double GrossProfit { get; set; }
        public double GrossProfit5ya { get; set; }
        public double MarketCap { get; set; }
        public double EV { get; set; }
        public double PEG { get; set; }
        public double EVRev { get; set; }
        public double RetOnAssets { get; set; }
        public double Revenue { get; set; }
        public double RevPerShare { get; set; }
        public double EBITDA { get; set; }
        public double TotalCash { get; set; }
        public double TotalCashPerShare { get; set; }
        public double TotalDebt { get; set; }
        public double BookValPerShare { get; set; }
        public double OperatingCashFlow { get; set; }
        public double LeveredFreeCashFlow { get; set; }
        public double TotalShares { get; set; }
        public double ProfitCoef { get; set; }
        public double ProfitCoef5ya { get; set; }
        public double ProfitOn12mToAnalogYearAgo { get; set; }
        public double GrowProfitPerShare5y { get; set; }
        public double CapExpenseGrow5y { get; set; }
        public double UrgentLiquidityCoef { get; set; }
        public double CurrentLiquidityCoef { get; set; }
        public double this[string ind]
        {
            get
            {
                switch (ind)
                {
                    case "PriceToEquity":
                        return PriceToEquity;
                    case "PriceToSales":
                        return _priceToSales;
                    case "PriceToBook":
                        return _priceToBook;
                    case "ROE":
                        return _ROE;
                    case "EPS":
                        return _EPS;
                    case "QEG":
                        return QEG;
                    case "ProfitMargin":
                        return ProfitMarg;
                    case "OperatingMargin":
                        return OperMarg;
                    case "GrossProfit":
                        return GrossProfit;
                }
                return -1;
            }
        }

        private StockMarket _market;
        private string _name;
        private string _symbol;
        private DateTime _lastUpdate;
        private bool _isStarred;
        public bool isOnTinkoff { get; private set; }

        public Stock(string name, double price, StockMarket mar, string symb = "")
        {
            Name = name;
            Price = price;
            Market = mar;
            Symbol = symb;
            LastUpdate = DateTime.Now;

            // ЗДЕСЬ ЗАПРОС К ТНЬКОФ
            string nameToSearch = "";
            var splitted = name.Split(' ');
            for (var i = 0; i < splitted.Length - 1; i++)
            {
                nameToSearch += splitted[i].Replace(",", "");
            }
            string respStr = Web.GETs("https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" + nameToSearch);
            JObject resp = JObject.Parse(respStr);
            bool isTin = false;
            nameToSearch = nameToSearch.ToLower();
            for (var j = 0; j < (int)resp["payload"]["total"]; j++)
            {
                string descr = !string.IsNullOrEmpty(((string)resp["payload"]["values"][0]["symbol"]["showName"])) ? (string)resp["payload"]["values"][0]["symbol"]["showName"] : (string)resp["payload"]["values"][0]["symbol"]["description"];
                if (descr.ToLower().Contains( nameToSearch))
                {
                    isTin = true;
                    break;
                }
            }
            isOnTinkoff = isTin;//((int) resp["payload"]["total"] > 0) && respStr.Contains(nameToSearch);
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        public string FullName
        {
            get
            {
                string tmp = "[Rus]";
                if (Market.Location == StockMarketLocation.USA)
                    tmp = "[USA]";
                if (Market.Location == StockMarketLocation.London)
                    tmp = "[Lon]";
                return Name + ' '+ tmp;
            }
        }
        public bool IsStarred
        {
            get { return _isStarred; }
            set { _isStarred = value; }
        }
        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            set { _lastUpdate = value; }
        }
        public double Price
        {
            get { return _price; }
            set { if (value > 0) _price = value; }
        }
        public double PriceToEquity
        {
            get { return _priceToEquity; }
            set { _priceToEquity = value; }
        }
        public double PriceToSales
        {
            get { return _priceToSales; }
            set {  _priceToSales = value; }
        }
        public double PriceToBook
        {
            get { return _priceToBook; }
            set { _priceToBook = value; }
        }
        public double EVtoEBITDA
        {
            get { return _EVtoEBITDA; }
            set { _EVtoEBITDA = value; }
        }
        public double DebtToEBITDA
        {
            get { return _debtToEBITDA; }
            set { _debtToEBITDA = value; }
        }
        public double ROE
        {
            get { return _ROE; }
            set { _ROE = value; }
        }
        public double EPS
        {
            get { return _EPS; }
            set { _EPS = value; }
        }
        public StockMarket Market
        {
            get { return _market; }
            set { _market = value; }
        }
    }
}
