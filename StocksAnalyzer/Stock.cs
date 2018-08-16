using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{
    enum StockMarketLocation
    {
        Russia=0,
        Usa=10,
        London=20,
        Other=30
    }
    enum StockMarketCurrency //валюта, в которой торгуются акции
    {
        Rub=0,
        Usd=10,
        Eur=20
    }

    /// <summary>
    /// Представляет собой рынок, на котором обращаются акции
    /// </summary>
    [Serializable]
    class StockMarket
    {
        public StockMarketLocation Location { get; }
        public StockMarketCurrency Currency        { get; }
        private static double _exchangeRateRubToUsd; // Переделать в словарь
        private static double _exchangeRateRubToEur;
        
        public StockMarket(StockMarketLocation loc, StockMarketCurrency curr)
        {
            Location = loc;
            Currency = curr;
            //getExchangeRates();
        }

        /// <summary>
        /// Получение текущих курсов обмена
        /// </summary>
        public static async void InitializeCurrencies()
        {
            string response = await Web.GeTtask(Web.ExchangeRatesUrl);
            var currency = response.Split(',');
            // Переделать распарсивание в JSON
            for (int i = 0; i < currency.Length; i++)
            {
                if (currency[i].StartsWith("\"USD"))
                {
                    _exchangeRateRubToUsd = 1.0 / currency[i].Substring(currency[i].IndexOf(':') + 1).ParseCoefStrToDouble();
                }
                else if (currency[i].StartsWith("\"EUR"))
                {
                    _exchangeRateRubToEur = 1.0 / currency[i].Substring(currency[i].IndexOf(':') + 1).ParseCoefStrToDouble();
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
                case StockMarketCurrency.Usd:
                    return _exchangeRateRubToUsd;
                case StockMarketCurrency.Eur:
                    return _exchangeRateRubToEur;
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
        public StockMarket Market { get; }
        public string Name { get; }
        public string Symbol { get; }
        public string FullName => $"{Name} [{Market.Location}]";
        public bool IsOnTinkoff { get; }

        #region Metrics
        public double MainPe { get; set; }
        public double Main { get; set; }
        public double MainAll { get; set; }
        public int RateMainPe { get; set; }
        public int RateMain { get; set; }
        public int RateMainAll { get; set; }
        #endregion

        #region Main properties
        public double Price { get; set; }
        public double PriceToEquity { get; set; }
        public double PriceToSales { get; set; }
        public double PriceToBook { get; set; }
        public double EVtoEbitda { get; set; }
        public double DebtToEbitda { get; set; }
        public double Roe { get; set; }
        public double Eps { get; set; }
        #endregion

        #region Other properties
        public double Qeg { get; set; }
        public double ProfitMarg { get; set; }
        public double ProfitMarg5Ya { get; set; }
        public double OperMarg { get; set; }
        public double OperMarg5Ya { get; set; }
        public double GrossProfit { get; set; }
        public double GrossProfit5Ya { get; set; }
        public double MarketCap { get; set; }
        public double Ev { get; set; }
        public double Peg { get; set; }
        public double EvRev { get; set; }
        public double RetOnAssets { get; set; }
        public double Revenue { get; set; }
        public double RevPerShare { get; set; }
        public double Ebitda { get; set; }
        public double TotalCash { get; set; }
        public double TotalCashPerShare { get; set; }
        public double TotalDebt { get; set; }
        public double BookValPerShare { get; set; }
        public double OperatingCashFlow { get; set; }
        public double LeveredFreeCashFlow { get; set; }
        public double TotalShares { get; set; }
        public double ProfitCoef { get; set; }
        public double ProfitCoef5Ya { get; set; }
        public double ProfitOn12MToAnalogYearAgo { get; set; }
        public double GrowProfitPerShare5Y { get; set; }
        public double CapExpenseGrow5Y { get; set; }
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
                        return Roe;
                    case "EPS":
                        return Eps;
                    case "QEG":
                        return Qeg;
                    case "ProfitMargin":
                        return ProfitMarg;
                    case "OperatingMargin":
                        return OperMarg;
                    case "GrossProfit":
                        return GrossProfit;
                }
                throw new ArgumentException(@"Не могу найти параметр", ind);
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
            string nameToSearch = "";
            var splitted = name.Split(' ');
            for (var i = 0; i < splitted.Length - 1; i++)
            {
                nameToSearch += splitted[i].Replace(",", "");
            }
            string respStr = Web.Get("https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" + nameToSearch);
            JObject jsonReponse = JObject.Parse(respStr);
            nameToSearch = nameToSearch.ToLower();
            for (var j = 0; j < (int)jsonReponse["payload"]["total"]; j++)
            {
                string descr = !string.IsNullOrEmpty(((string)jsonReponse["payload"]["values"][0]["symbol"]["showName"]))
                    ? (string)jsonReponse["payload"]["values"][0]["symbol"]["showName"]
                    : (string)jsonReponse["payload"]["values"][0]["symbol"]["description"];
                if (descr.ToLower().Contains( nameToSearch))
                {
                    IsOnTinkoff = true;
                    break;
                }
            }
            //IsOnTinkoff = ((int) jsonReponse["payload"]["total"] > 0) && _respStr.Contains(_nameToSearch);
        }
    }
}
