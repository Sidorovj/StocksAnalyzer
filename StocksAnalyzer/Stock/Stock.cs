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
		public double Price { get; set; }

	    private Dictionary<string, double?> CoefficientsValues { get; } = new Dictionary<string, double?>(Coefficient.CoefficientList.Count);

		#region Metrics
		public double MainPe { get; set; }
        public double Main { get; set; }
        public double MainAll { get; set; }
        public int RateMainPe { get; set; }
        public int RateMain { get; set; }
        public int RateMainAll { get; set; }
        #endregion
		

        public override string ToString()
        {
            return Name;
        }

        //TODO:make it private
        public double? this[string ind]
        {
            get => CoefficientsValues[ind];
            set => CoefficientsValues[ind] = value;
        }

        public double? this[Coefficient coef]
        {
            get => this[coef.Name];
            set => this[coef.Name] = value;
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
                CoefficientsValues[coef.Name] = null;

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
