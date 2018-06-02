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

    /// <summary>
    /// Представляет собой рынок, на котором обращаются акции
    /// </summary>
    [Serializable]
    class StockMarket
    {
        public StockMarketLocation Location { get; private set; }
        public StockMarketCurrency Currency        { get; private set; }
        private static double exchangeRate_RubToUsd; // Переделать в словарь
        private static double exchangeRate_RubToEur;
        
        public StockMarket(StockMarketLocation loc, StockMarketCurrency curr)
        {
            Location = loc;
            Currency = curr;
            //getExchangeRates();
        }

        /// <summary>
        /// Получение текущих курсов обмена
        /// </summary>
        public async static void InitializeCurrencies()
        {
            string _response = await Web.GETtask(Web.ExchangeRatesUrl);
            var _currency = _response.Split(',');
            // Переделать распарсивание в JSON
            for (int i = 0; i < _currency.Length; i++)
            {
                if (_currency[i].StartsWith("\"USD"))
                {
                    exchangeRate_RubToUsd = 1.0 / _currency[i].Substring(_currency[i].IndexOf(':') + 1).ParseCoefStrToDouble();
                }
                else if (_currency[i].StartsWith("\"EUR"))
                {
                    exchangeRate_RubToEur = 1.0 / _currency[i].Substring(_currency[i].IndexOf(':') + 1).ParseCoefStrToDouble();
                }

            }
        }

        /// <summary>
        /// Получить текущий курс обмена рубля К валюте
        /// </summary>
        /// <param name="toCurr">Какая валюта</param>
        /// <returns>Курс обмена</returns>
        public static double GetExchangeRates(StockMarketCurrency toCurr) // Найдем курс рубля к запрошенной валюте
        {
            switch (toCurr)
            {
                case StockMarketCurrency.USD:
                    return exchangeRate_RubToUsd;
                case StockMarketCurrency.EUR:
                    return exchangeRate_RubToEur;
            }
            throw new KeyNotFoundException();
        }
    }


    /// <summary>
    /// Акция и ее характеристики
    /// </summary>
    [Serializable]
    class Stock
    {
        public bool IsStarred { get; set; }
        public DateTime LastUpdate { get; set; }
        public StockMarket Market { get; private set; }
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public string FullName => $"{Name} [{Market.Location}]";
        public bool IsOnTinkoff { get; private set; }

        #region Metrics
        public double MainPE { get; set; }
        public double Main { get; set; }
        public double MainAll { get; set; }
        public int RateMainPE { get; set; }
        public int RateMain { get; set; }
        public int RateMainAll { get; set; }
        #endregion

        #region Main properties
        public double Price { get; set; }
        public double PriceToEquity { get; set; }
        public double PriceToSales { get; set; }
        public double PriceToBook { get; set; }
        public double EVtoEBITDA { get; set; }
        public double DebtToEBITDA { get; set; }
        public double ROE { get; set; }
        public double EPS { get; set; }
        #endregion

        #region Other properties
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
        #endregion


        public double this[string ind]
        {
            get
            {
                switch (ind)
                {
                    case "PriceToEquity":
                        return PriceToEquity;
                    case "PriceToSales":
                        return PriceToSales;
                    case "PriceToBook":
                        return PriceToBook;
                    case "ROE":
                        return ROE;
                    case "EPS":
                        return EPS;
                    case "QEG":
                        return QEG;
                    case "ProfitMargin":
                        return ProfitMarg;
                    case "OperatingMargin":
                        return OperMarg;
                    case "GrossProfit":
                        return GrossProfit;
                }
                throw new ArgumentException("Не могу найти параметр", ind);
            }
        }
        

        public Stock(string name, double price, StockMarket mar, string symb = "")
        {
            Name = name;
            Price = price;
            Market = mar;
            Symbol = symb;
            LastUpdate = DateTime.Now;
            IsOnTinkoff = false;

            // ЗДЕСЬ ЗАПРОС К ТИНЬКОФ
            string _nameToSearch = "";
            var _splitted = name.Split(' ');
            for (var i = 0; i < _splitted.Length - 1; i++)
            {
                _nameToSearch += _splitted[i].Replace(",", "");
            }
            string _respStr = Web.GET("https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" + _nameToSearch);
            JObject jsonReponse = JObject.Parse(_respStr);
            _nameToSearch = _nameToSearch.ToLower();
            for (var j = 0; j < (int)jsonReponse["payload"]["total"]; j++)
            {
                string _descr = !string.IsNullOrEmpty(((string)jsonReponse["payload"]["values"][0]["symbol"]["showName"]))
                    ? (string)jsonReponse["payload"]["values"][0]["symbol"]["showName"]
                    : (string)jsonReponse["payload"]["values"][0]["symbol"]["description"];
                if (_descr.ToLower().Contains( _nameToSearch))
                {
                    IsOnTinkoff = true;
                    break;
                }
            }
            //IsOnTinkoff = ((int) jsonReponse["payload"]["total"] > 0) && _respStr.Contains(_nameToSearch);
        }
    }
}
