using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace StocksAnalyzer
{

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
        public bool IsOnTinkoff { get; private set; }
        public bool TinkoffScanned { get; private set; }


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
        public Dictionary<Coefficient, double> CoefficientsValues { get; } = new Dictionary<Coefficient, double>(Coefficient.CoefficientList.Count);
        //      public double PriceToEquity { get; set; }
        //public double PriceToSales { get; set; }
        //public double PriceToBook { get; set; }
        //public double EVtoEbitda { get; set; }
        //public double DebtToEbitda { get; set; }
        //public double Roe { get; set; }
        //public double Eps { get; set; }
        //#endregion

        //#region Other properties
        //public double Qeg { get; set; }
        //public double ProfitMarg { get; set; }
        //public double ProfitMarg5Ya { get; set; }
        //public double OperMarg { get; set; }
        //public double OperMarg5Ya { get; set; }
        //public double GrossProfit { get; set; }
        //public double GrossProfit5Ya { get; set; }
        //public double MarketCap { get; set; }
        //public double Ev { get; set; }
        //public double Peg { get; set; }
        //public double EvRev { get; set; }
        //public double RetOnAssets { get; set; }
        //public double Revenue { get; set; }
        //public double RevPerShare { get; set; }
        //public double Ebitda { get; set; }
        //public double TotalCash { get; set; }
        //public double TotalCashPerShare { get; set; }
        //public double TotalDebt { get; set; }
        //public double BookValPerShare { get; set; }
        //public double OperatingCashFlow { get; set; }
        //public double LeveredFreeCashFlow { get; set; }
        //public double TotalShares { get; set; }
        //public double ProfitCoef { get; set; }
        //public double ProfitCoef5Ya { get; set; }
        //public double ProfitOn12MToAnalogYearAgo { get; set; }
        //public double GrowProfitPerShare5Y { get; set; }
        //public double CapExpenseGrow5Y { get; set; }
        //public double UrgentLiquidityCoef { get; set; }
        //public double CurrentLiquidityCoef { get; set; }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public double this[string ind]
        {
            get
            {
                var coef = Coefficient.CoefficientList.FirstOrDefault(c => c.Name == ind) ?? throw new ArgumentNullException($"Нету такого коэф. с именем = {ind}");
                return CoefficientsValues[coef];
            }
            set
            {
                var coef = Coefficient.CoefficientList.FirstOrDefault(c => c.Name == ind) ?? throw new ArgumentNullException($"Нету такого коэф. с именем = {ind}");
                CoefficientsValues[coef] = value;
            }
        }

        public double this[Coefficient coef]
        {
            get => CoefficientsValues[coef];
            set => CoefficientsValues[coef] = value;
        }

        public Stock(string name, double price, StockMarket mar, string symb = "")
        {
            Name = name;
            Price = price;
            Market = mar;
            Symbol = symb;
            LastUpdate = DateTime.Now;
            IsOnTinkoff = TinkoffScanned = mar.Location == StockMarketLocation.Russia;
            foreach (var coef in Coefficient.CoefficientList)
            {
                CoefficientsValues[coef] = -1;

            }
        }

        public async Task UnderstandIsItOnTinkoff()
        {
            if (TinkoffScanned)
                return;

            string nameToSearch = "";
            string[] suffixArray = { "", "ао", "ап", "деп.", "расп.", "inc", "inc.", "corp", "corp.", "ltd", "ltd.", "corporation", "incorporated", "plc", "group", "company" };
            foreach (var s in Name.ToLower().Split(' '))
            {
                if (!suffixArray.Contains(s))
                {
                    if (s != "and" && s != "&")
                        nameToSearch += s.Replace(",", "") + ' ';
                }
                else
                    break;
                if (s.EndsWith(","))
                    break;
            }

            JObject jsonReponse;

            while (true)
            {
                var urlFilter = nameToSearch.Split(' ')[0];
                var respStr = await Web.Get("https://api.tinkoff.ru/trading/stocks/list?country=All&sortType=ByName&orderType=Asc&start=0&end=20&filter=" + urlFilter);
                jsonReponse = JObject.Parse(respStr);
                if (jsonReponse?["status"]?.Value<string>() != "Error")
                    break;
                if (jsonReponse["payload"]?["code"]?.Value<string>() == "RequestRateLimitExceeded")
                {
                    await Task.Delay(60 * 1000);

                    //именно спим, чтобы блокировать остальные вызовы
                    //Thread.Sleep(60 * 1000);
                }
                else
                {
                    throw new Exception($"{nameof(nameToSearch)}={nameToSearch}; {nameof(respStr)}={respStr}");
                }
            }

            if (jsonReponse?["payload"]?["total"] != null && (int)jsonReponse["payload"]["total"] >= 0)
                for (var j = 0; j < (int)jsonReponse["payload"]["total"]; j++)
                {
                    var symbol = jsonReponse["payload"]?["values"]?[j]?["symbol"];
                    if (NameOrDescriptionContains(
                        nameToSearch,
                        (string)symbol?["showName"],
                        (string)symbol?["description"],
                        (string)symbol?["fullDescription"]))
                    {
                        IsOnTinkoff = true;
                        break;
                    }
                }
            TinkoffScanned = true;
        }


        private bool NameOrDescriptionContains(string searchStr, params string[] arr)
        {
            var elems = searchStr.Split(' ');
            foreach (var str in arr)
            {
                if (str == null)
                    continue;
                bool cont = false;
                foreach (var elem in elems)
                {
                    if (!str.ToLower().Contains(elem))
                    {
                        cont = true;
                        break;
                    }
                }
                if (cont)
                    continue;
                return true;
            }
            return false;
        }
    }
}
