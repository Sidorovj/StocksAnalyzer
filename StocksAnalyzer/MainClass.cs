using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace StocksAnalyzer
{
    /// TODO:
    /// переделать распарс в InitializeCurrencies()
    /// Добавить лондонскую биржу
    /// 
    internal static class MainClass
    {
        public const double Tolerance = 1e-7;

        public static string ReportFileName { get; private set; } = "";
        public static Dictionary<string, string> NamesToSymbolsRus { get; } = new Dictionary<string, string>();
        public static List<Stock> Stocks { get; set; } = new List<Stock>();

        private static string _report = "";

        private static readonly string[] ListToLogInReport =
        {
            "PriceToEquity", "PriceToSales", "PriceToBook", "ROE", "EPS", "QEG", "ProfitMargin", "OperatingMargin",
            "GrossProfit"
        };

        private const string StockListFilePath = "stockList.dat";

        #region Methods:public

        public static Stock GetStock(bool compareFullName, string name)
        {
            // TODO: 
            foreach (var st in Stocks)
                if ((compareFullName && st.FullName == name) || (!compareFullName && st.Name == name))
                    return st;
            return null;
        }

        /// <summary>
        /// Преобразует строку вида "USD":0.001432 / "RUB":2B в double
        /// </summary>
        /// <param name="stringValue">Формат строки: "USD":0.001432</param>
        /// <returns></returns>
        public static double ParseCoefStrToDouble(this string stringValue)
        {
            if (stringValue.IndexOf(":", StringComparison.Ordinal) > 0)
                stringValue = stringValue.Substring(stringValue.IndexOf(':') + 1);
            while (stringValue.EndsWith("}") || stringValue.EndsWith("%"))
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            double result, coefficient = 1;
            if (stringValue.EndsWith("M"))
            {
                coefficient = 1000 * 1000;
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            else if (stringValue.EndsWith("B"))
            {
                coefficient = 1000 * 1000 * 1000;
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            if (double.TryParse(stringValue, out result))
                return result * coefficient;
            stringValue = stringValue.Contains(',') ? stringValue.Replace(',', '.') : stringValue.Replace('.', ',');
            if (double.TryParse(stringValue, out result))
                return result * coefficient;
            throw new Exception($"Не удается распарсить строку {stringValue}");
        }

        /// <summary>
        /// Загружает список из файла
        /// </summary>
        /// <param name="path"></param>
        public static void LoadStockListFromFile(string path = StockListFilePath)
        {
            if (!File.Exists(path))
                return;
            Serializer ser = new Serializer(path);
            var temp = (List<Stock>)ser.Deserialize();

            // TODO: чекнуть, что все ок
            Stocks = temp.Where(st => st.Market != null).ToList();
        }

        /// <summary>
        /// Сериализует список акций в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public static void WriteStockListToFile(string path = StockListFilePath)
        {
            Serializer ser = new Serializer(path);
            ser.Serialize(Stocks);
        }

        /// <summary>
        /// Составить отчет по списку акций и записать в файл
        /// </summary>
        /// <param name="stockLst">Список акций</param>
        public static void MakeReportAndSaveToFile(List<Stock> stockLst)
        {
            _report += '\n';
            foreach (string param in ListToLogInReport)
            {
                string helpSt = "";
                int numRes = 0;
                foreach (var st in stockLst)
                {
                    if (Math.Abs(st[param]) < Tolerance)
                    {
                        helpSt += st.Name + ';';
                        numRes++;
                    }
                }
                _report += $"{param};Заполнен в {stockLst.Count - numRes}/{stockLst.Count};{helpSt}\r\n";
            }
            ReportFileName = $"Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            StreamWriter sr = new StreamWriter(ReportFileName, true, Encoding.UTF8);
            sr.Write(_report);
            sr.Close();
        }

        /// <summary>
        /// Записать лог в текстБокс на форме
        /// </summary>
        /// <param name="text">Строки лога</param>
        public static void WriteLog(params string[] text)
        {
            string fullText = "";
            foreach (var s in text)
                fullText += $"{DateTime.Now:HH:mm:ss}  {s}\r\n";
            Program.MyForm.richTextBoxLog.BeginInvoke(
                (MethodInvoker)delegate
               {
                   Program.MyForm.richTextBoxLog.Text = fullText + Program.MyForm.richTextBoxLog.Text;
               });
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog($"[Error]: {ex.Message}");
        }

        /// <summary>
        /// Загрузить данные по акциям
        /// </summary>
        /// <param name="lst">Список акций</param>
        /// <param name="lbl">Лейбл с формы</param>
        /// <param name="bar">Прогресс-бар</param>
        /// <returns></returns>
        public static string LoadStocksData(List<Stock> lst, Label lbl, ProgressBar bar)
        {
            var i = 0;
            _report = "Не удалось загрузить акции:;";
            var count = lst.Count;
            Stopwatch stwatch = new Stopwatch();
            stwatch.Start();
            Parallel.ForEach(lst, st =>
            {
                GetStockData(st);
                i++;

                double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)i / count) - 1) / 60.0;
                mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;
                var i1 = i;
                lbl.BeginInvoke((MethodInvoker)delegate
                {
                    lbl.Text =
                        $@"Обработано {i1} / {count}. Расчетное время: {
                                (mins >= 1 ? Math.Floor(mins) + " мин " : "")
                            }{Math.Floor((mins - Math.Floor(mins)) * 100)} с";
                });
                bar.BeginInvoke((MethodInvoker)delegate { bar.Value = i1 * 100 / count; });
            });

            lbl.BeginInvoke((MethodInvoker)delegate { lbl.Text = @"Готово."; });
            stwatch.Stop();
            return "";
        }

        /// <summary>
        /// Преобразовать в короткую/красивую строку
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Строку</returns>
        public static string ToCuteStr(this double num)
        {
            string str = "";
            if (Math.Abs(num) > 1000 * 1000 * 1000) // Миллиард
                str = (num / 1000 * 1000 * 1000).ToString("F2") + " B";
            else if (Math.Abs(num) > 1000 * 1000) // Миллион
                str = (num / 1000 * 1000).ToString("F2") + " M";
            else if (Math.Abs(num) > Tolerance)
                str = num.ToString("F2");
            return str;
        }

        /// <summary>
        /// Получить данные по акции из интернета
        /// </summary>
        /// <param name="st">Акция</param>
        public static void GetStockData(Stock st)
        {
            string stockName = st.Name;
            st = GetStock(false, st.Name);
            if (st == null)
            {
                WriteLog("Не удалось найти акцию в getStockData: " + stockName);
                return;
            }

            try
            {
                if (st.Market.Location == StockMarketLocation.Usa)
                {
                    string htmlCode = Web.Get(Web.GetStockDataUrlUsa.Replace("{}", st.Symbol) + st.Symbol);
                    st.PriceToEquity = GettingYahooData("Trailing P/E", ref htmlCode);
                    st.PriceToSales = GettingYahooData("Price/Sales", ref htmlCode);
                    st.PriceToBook = GettingYahooData("Price/Book", ref htmlCode);
                    st.Eps = GettingYahooData("Diluted EPS", ref htmlCode);
                    st.Roe = GettingYahooData("Return on Equity", ref htmlCode);

                    st.EVtoEbitda = GettingYahooData("Enterprise Value/EBITDA", ref htmlCode);
                    st.MarketCap = GettingYahooData("Market Cap (intraday)", ref htmlCode);
                    st.Qeg = GettingYahooData("Quarterly Revenue Growth", ref htmlCode);
                    st.ProfitMarg = GettingYahooData("Profit Margin", ref htmlCode);
                    st.OperMarg = GettingYahooData("Operating Margin", ref htmlCode);
                    st.GrossProfit = GettingYahooData("Gross Profit", ref htmlCode);
                    st.Ev = GettingYahooData("Enterprise Value", ref htmlCode);
                    st.Peg = GettingYahooData("PEG Ratio (5 yr expected)", ref htmlCode);
                    st.EvRev = GettingYahooData("Enterprise Value/Revenue", ref htmlCode);
                    st.RetOnAssets = GettingYahooData("Return on Assets", ref htmlCode);
                    st.Revenue = GettingYahooData("Revenue", ref htmlCode);
                    st.RevPerShare = GettingYahooData("Revenue Per Share", ref htmlCode);
                    st.Ebitda = GettingYahooData("EBITDA", ref htmlCode);
                    st.TotalCash = GettingYahooData("Total Cash", ref htmlCode);
                    st.TotalCashPerShare = GettingYahooData("Total Cash Per Share", ref htmlCode);
                    st.TotalDebt = GettingYahooData("Total Debt", ref htmlCode);
                    st.BookValPerShare = GettingYahooData("Book Value Per Share", ref htmlCode);
                    st.OperatingCashFlow = GettingYahooData("Operating Cash Flow", ref htmlCode);
                    st.LeveredFreeCashFlow = GettingYahooData("Levered Free Cash Flow", ref htmlCode);
                    st.TotalShares = GettingYahooData("Shares Outstanding", ref htmlCode);
                    if (Math.Abs(st.Ebitda) > Tolerance)
                        st.DebtToEbitda = st.TotalDebt / st.Ebitda;

                    //htmlCode = htmlCode.Substring(htmlCode.IndexOf("data-reactid=\"35\"")); // react-id каждый раз разный
                    //htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                    //st.Price = htmlCode.Substring(0, htmlCode.IndexOf("<")).getDoubleNum();
                    st.LastUpdate = DateTime.Now;
                }
                else if (st.Market.Location == StockMarketLocation.Russia)
                {
                    if (!NamesToSymbolsRus.ContainsKey(st.Name))
                    {
                        WriteLog("Нет ссылки для получения инфы для " + st.Name);
                        return;
                    }
                    string htmlCode = Web.Get(Web.GetStockDataUrlRussia + NamesToSymbolsRus[st.Name]);
                    st.PriceToEquity = GettingInvestingComData("Коэффициент цена/прибыль", ref htmlCode);
                    st.PriceToSales = GettingInvestingComData("Коэффициент цена/объем продаж", ref htmlCode);
                    st.PriceToBook = GettingInvestingComData("Коэффициент цена/балансовая стоимость", ref htmlCode);
                    st.Eps = GettingInvestingComData("Базовая прибыль на акцию", ref htmlCode);
                    st.Roe = GettingInvestingComData("Прибыль на инвестиции", ref htmlCode);

                    st.Qeg = GettingInvestingComData("Прибыль на акцию за последний квартал к квартальной год назад",
                        ref htmlCode);
                    st.ProfitMarg = GettingInvestingComData("Маржа прибыли до налогообложения ", ref htmlCode, "TTM");
                    st.OperMarg = GettingInvestingComData("Операционная маржа", ref htmlCode, "TTM");
                    st.GrossProfit = GettingInvestingComData("Валовая прибыль", ref htmlCode, "TTM");
                    st.GrossProfit5Ya = GettingInvestingComData("Валовая прибыль", ref htmlCode, "5YA");
                    st.ProfitCoef = GettingInvestingComData("Коэффициент прибыльности", ref htmlCode, "TTM");
                    st.ProfitCoef5Ya = GettingInvestingComData("Коэффициент прибыльности", ref htmlCode, "5YA");
                    st.ProfitOn12MToAnalogYearAgo =
                        GettingInvestingComData(
                            "Прибыль на акцию за последние 12 месяцев к аналогичному периоду год назад", ref htmlCode);
                    st.GrowProfitPerShare5Y = GettingInvestingComData("Рост прибыли на акцию за 5 лет", ref htmlCode);
                    st.CapExpenseGrow5Y =
                        GettingInvestingComData("Рост капитальных расходов за последние 5 лет", ref htmlCode);
                    st.ProfitMarg5Ya =
                        GettingInvestingComData("Маржа прибыли до налогообложения ", ref htmlCode, "5YA");
                    st.OperMarg5Ya = GettingInvestingComData("Операционная маржа", ref htmlCode, "5YA");
                    st.UrgentLiquidityCoef = GettingInvestingComData("Коэффициент срочной ликвидности", ref htmlCode);
                    st.CurrentLiquidityCoef = GettingInvestingComData("Коэффициент текущей ликвидности", ref htmlCode);

                    htmlCode = htmlCode.Substring(htmlCode.IndexOf("id=\"last_last\"", StringComparison.Ordinal));
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(">", StringComparison.Ordinal) + 1);
                    st.Price = htmlCode.Substring(0, htmlCode.IndexOf("<", StringComparison.Ordinal))
                        .ParseCoefStrToDouble();
                    st.LastUpdate = DateTime.Now;
                }
            }
            catch (Exception er)
            {
                WriteLog("Не удалось получить инфу по " + st.Name + ": " + er.Message);
                _report += st.Name + ';';
            }

        }

        public static async void Initialize()
        {
            try
            {
                StockMarket.InitializeCurrencies();
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                WriteLog("USD: " + StockMarket.GetExchangeRates(StockMarketCurrency.Usd).ToString("F2"),
                    "EUR: " + StockMarket.GetExchangeRates(StockMarketCurrency.Eur).ToString("F2"));
            });
            FillDict();
        }

        /// <summary>
        /// Загрузить список всех акций
        /// </summary>
        public static void GetStocksList()
        {
            var getRus = Task.Factory.StartNew(() => Parallel.For(0, 10, GetRussianStocks));
            var getUsa = Task.Factory.StartNew(GetUsaStocks);

            Task.WaitAll(getRus, getUsa);
            Stocks = Stocks.Distinct().ToList();
            //Отсортируем по алфавиту
            for (var i = 0; i < Stocks.Count; i++)
                for (var j = 0; j < Stocks.Count - i - 1; j++)
                    if (string.CompareOrdinal(Stocks[j].Name, Stocks[j + 1].Name) > 0)
                    {
                        var st = Stocks[j];
                        Stocks[j] = Stocks[j + 1];
                        Stocks[j + 1] = st;
                    }
        }

        #endregion

        #region Methods:private
        

        /// <summary>
        /// Загрузить в Stocks акции с рус. биржы
        /// </summary>
        private static void GetRussianStocks(int page)
        {
            page *= 30;

            string htmlCode = Web.Get(Web.GetStocksListUrlRussia2.Replace("{num}", page.ToString()));

            htmlCode = htmlCode.Substring(htmlCode.IndexOf("<tr class=\"tblr-head\"", StringComparison.Ordinal));
            htmlCode = htmlCode.Substring(0, htmlCode.IndexOf("</table", StringComparison.Ordinal));
            string tdClass = "<a";
            string title = ">";
            while (htmlCode.Contains(tdClass))
            {
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(tdClass, StringComparison.Ordinal) + tdClass.Length);
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(title, StringComparison.Ordinal) + title.Length);
                if (string.IsNullOrWhiteSpace(htmlCode))
                    break;
                var name = htmlCode.Substring(0, htmlCode.IndexOf("<", StringComparison.Ordinal));
                if (name == "% за день")
                {
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf("</tr>", StringComparison.Ordinal));
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(tdClass, StringComparison.Ordinal) + tdClass.Length);
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(title, StringComparison.Ordinal) + title.Length);
                    if (string.IsNullOrWhiteSpace(htmlCode))
                        break;
                    name = htmlCode.Substring(0, htmlCode.IndexOf("<", StringComparison.Ordinal));
                }

                for (var j = 0; j < 6; j++)
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf("</td>", StringComparison.Ordinal) + 6);
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(">", StringComparison.Ordinal) + 1);
                var price = htmlCode.StartsWith(@"&mdash;")
                    ? -1
                    : htmlCode.Substring(0, htmlCode.IndexOf("</td>", StringComparison.Ordinal)).ParseCoefStrToDouble();
                if (price > 0)
                    Stocks.Add(new Stock(name, price,
                        new StockMarket(StockMarketLocation.Russia, StockMarketCurrency.Rub)));
            }


            /*
             * Получение 50 основных акций
             * 
             * 
            string htmlCode =  Web.GETs(Web.getStocksListUrl_Russia1);
            string tdClass = "<td class=\"bold left noWrap elp plusIconTd\">";
            string title = "title=\"";
            while (htmlCode.Contains(tdClass))
            {
                string name = "";
                double price = 0;
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(tdClass) + tdClass.Length); // получили строку до тега <a>
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(title) + title.Length);
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                name = htmlCode.Substring(0, htmlCode.IndexOf("<"));
                htmlCode = htmlCode.Substring(htmlCode.IndexOf("<td "));
                htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                price = htmlCode.Substring(0, htmlCode.IndexOf("</td>")).Replace(".","").getDoubleNum();
                Stocks.Add(new Stock(name, price, new StockMarket(StockMarketLocation.Russia, StockMarketCurrency.RUB)));
            }//*/
        }

        /// <summary>
        /// Загрузить в Stocks акции с амер. биржы
        /// </summary>
        private static void GetUsaStocks()
        {
            string[] htmlCode = (Web.ReadDownloadedFile(Web.GetStocksListUrlUsaNasdaq).Replace("\",\"", "|").Replace("\"", "") + Web.ReadDownloadedFile(Web.GetStocksListUrlUsaNyse).Replace("\",\"", "|").Replace("\"", "")).Split('\n');
            //htmlCode.Concat(Web.DownloadFile(Web.getStocksListUrl_USA_nyse).Replace("\",\"", "|").Replace("\"", "").Split('\n'));
            foreach (string s in htmlCode)
            {
                if (s.StartsWith("Symbol"))
                    continue;
                var parameters = s.Split('|');
                if (parameters.Length < 2)
                    continue;
                string name = parameters[1], symb = parameters[0];
                double price = parameters[2].ParseCoefStrToDouble();
                if (price > 0 && GetStock(false, name) == null)
                    Stocks.Add(new Stock(name, price, new StockMarket(StockMarketLocation.Usa, StockMarketCurrency.Usd), symb));
            }
        }


        /// <summary>
        /// Получает значение из разметки html сайта yahoo
        /// </summary>
        /// <param name="multiplicator">Название мультипликатора</param>
        /// <param name="htmlCode">Код</param>
        /// <returns>Значение</returns>
        private static double GettingYahooData(string multiplicator, ref string htmlCode)
        {
            string temp = htmlCode.Substring(htmlCode.IndexOf(">" + multiplicator + "</span>", StringComparison.Ordinal));
            temp = temp.Substring(temp.IndexOf("<td class", StringComparison.Ordinal));
            temp = temp.Substring(temp.IndexOf(">", StringComparison.Ordinal) + 1);
            temp = temp.Substring(0, temp.IndexOf("</", StringComparison.Ordinal));
            if (temp.IndexOf(">", StringComparison.Ordinal) > 0)
                temp = temp.Substring(temp.IndexOf(">", StringComparison.Ordinal) + 1);
            return temp.ParseCoefStrToDouble();
        }

        /// <summary>
        /// Получает значение из разметки html сайта yahoo
        /// </summary>
        /// <param name="multiplicator">Название мультипликатора</param>
        /// <param name="htmlCode">Код html</param>
        /// <param name="appendix"></param>
        /// <returns>Значение</returns>
        private static double GettingInvestingComData(string multiplicator, ref string htmlCode, string appendix = "")
        {
            string sp = "<span class=\"\">";
            string temp = htmlCode.Substring(htmlCode.IndexOf(sp + multiplicator, StringComparison.Ordinal));
            if (appendix != "")
                temp = temp.Substring(temp.IndexOf(appendix + "</i>", StringComparison.Ordinal));
            temp = temp.Substring(temp.IndexOf("<td>", StringComparison.Ordinal) + 4);
            return temp.Substring(0, temp.IndexOf("</td", StringComparison.Ordinal)).ParseCoefStrToDouble();
        }

        /// <summary>
        /// Заполнить словарь списком акций, которые надо обработать
        /// </summary>
        private static void FillDict()
        {
            NamesToSymbolsRus.Add("Polymetal International, ао", "polymetal-ratios?cid=44465");
            NamesToSymbolsRus.Add("КИВИ ПиЭлСи, ДР", "qiwi-plc-ratios?cid=960754");
            NamesToSymbolsRus.Add("Лента Лтд, ДР", "lenta-ltd-ratios?cid=962408");
            NamesToSymbolsRus.Add("ЯНДЕКС, ао", "yandex-ratios?cid=102063");
            //namesToSymbolsRus.Add("Белуга Групп, ао", "");
            //namesToSymbolsRus.Add("Владимирская ЭСК, ап", "-ratios");
            //namesToSymbolsRus.Add("Новороссийский комбинат хлебопродуктов, ао", "-ratios");
            //namesToSymbolsRus.Add("РН-Западная Сибирь, ао", "-ratios");
            //namesToSymbolsRus.Add("ТЗА, ао", "-ratios");
            //namesToSymbolsRus.Add("ТНС энерго Нижний Новгород, ао", "-ratios");
            //namesToSymbolsRus.Add("ЮжУрал-АСКО, ао", "-ratios");

            NamesToSymbolsRus.Add("GTL, ао", "gtl-oao-ratios");
            NamesToSymbolsRus.Add("Абрау-Дюрсо, ао", "abrau-durso-oao-ratios");
            NamesToSymbolsRus.Add("Авангард АКБ, ао", "akb-avangard-oao-ratios");
            NamesToSymbolsRus.Add("АВТОВАЗ, ао", "avtovaz-ratios");
            NamesToSymbolsRus.Add("АВТОВАЗ, ап", "avtovaz-(pref)-ratios");
            NamesToSymbolsRus.Add("Акрон, ао", "akron_rts-ratios");
            NamesToSymbolsRus.Add("АЛРОСА, ао", "alrosa-ao-ratios");
            NamesToSymbolsRus.Add("АЛРОСА-Нюрба, ао", "alrosa-nyurba-ratios");
            NamesToSymbolsRus.Add("Аптечная сеть 36.6, ао", "apteka-36-6_rts-ratios");
            NamesToSymbolsRus.Add("Армада, ао", "armada-ratios");
            NamesToSymbolsRus.Add("Астраханская ЭСК, ао", "astrakhan-power-sale-comp-ratios");
            NamesToSymbolsRus.Add("АФК Система, ао", "afk-sistema_rts-ratios");
            NamesToSymbolsRus.Add("Ашинский метзавод, ао", "ashinskiy-metallurgical-works-ratios");
            NamesToSymbolsRus.Add("Аэрофлот, ао", "aeroflot-ratios");
            NamesToSymbolsRus.Add("Банк Возрождение, ао", "vozrozhdenie_rts-ratios");
            NamesToSymbolsRus.Add("Банк Возрождение, ап", "bank-vozrozhdeniye-pao-ratios");
            NamesToSymbolsRus.Add("Банк Кузнецкий, ао", "bank-kuznetskiy-oao-ratios");
            NamesToSymbolsRus.Add("АКБ Приморье, ао", "akb-primorye-oao-ratios");
            NamesToSymbolsRus.Add("Банк Санкт-Петербург, ао", "bank-st-petersbr_rts-ratios");
            NamesToSymbolsRus.Add("БАНК УРАЛСИБ, ао", "bank-uralsib-ratios");
            NamesToSymbolsRus.Add("Банк ФК Открытие, ао", "nomos-bank-ratios");
            NamesToSymbolsRus.Add("Башинформсвязь, ао", "bashinformsvyaz-ratios");
            NamesToSymbolsRus.Add("Башинформсвязь, ап", "bashinformsvyaz-(pref)-ratios");
            NamesToSymbolsRus.Add("Башнефть, ао", "bashneft_rts-ratios");
            NamesToSymbolsRus.Add("Башнефть, ап", "bashneft-(pref)-ratios");
            NamesToSymbolsRus.Add("Белон, ао", "belon_rts-ratios");
            NamesToSymbolsRus.Add("Бест Эффортс Банк (бывш. АЛОР БАНК), ао", "alor-bank-oao-ratios");
            NamesToSymbolsRus.Add("Бурятзолото, ао", "buryatzoloto-ratios");
            NamesToSymbolsRus.Add("Варьеганнефтегаз, ао", "varyeganneftegaz-ratios");
            NamesToSymbolsRus.Add("Варьеганнефтегаз, ап", "varyeganneftegaz-(pref)-ratios");
            NamesToSymbolsRus.Add("Владимирская ЭСК, ао", "vladimirenergosbyt-oao-ratios");
            NamesToSymbolsRus.Add("Волгоградская ЭСК, ао", "volgogradenergosbyt-ratios");
            NamesToSymbolsRus.Add("Волгоградская ЭСК, ап", "volgogradenergosbyt-(pref)-ratios");
            NamesToSymbolsRus.Add("ВСМПО-АВИСМА, ао", "vsmpo-avisma-crp_rts-ratios");
            NamesToSymbolsRus.Add("ВТБ, ао", "vtb_rts-ratios");
            NamesToSymbolsRus.Add("ВТОРРЕСУРСЫ, ао", "vtorresursy-oao-ratios");
            NamesToSymbolsRus.Add("ВХЗ, ао", "vladimirskiy-khimicheskiy-ratios");
            NamesToSymbolsRus.Add("Выборгский судостроительный завод, ао", "vyborgskiy-sudostroitelnyi-zavod-ratios");
            NamesToSymbolsRus.Add("Выборгский судостроительный завод, ап", "vyborgskiy-sudostroitelnyi-pao-ratios");
            NamesToSymbolsRus.Add("ГАЗ, ао", "gaz-auto-plant-ratios");
            NamesToSymbolsRus.Add("ГАЗ, ап", "gaz-auto-plant-(pref)-ratios");
            NamesToSymbolsRus.Add("Газпром газораспределение Ростов-на-Дону, ао", "gazprom-gazoraspredeleniye-ratios");
            NamesToSymbolsRus.Add("Газпром нефть, ао", "gazprom-neft_rts-ratios");
            NamesToSymbolsRus.Add("Газпром, ао", "gazprom_rts-ratios");
            NamesToSymbolsRus.Add("Галс-Девелопмент (бывш. Система-Галс), ао", "gals-development-ratios");
            NamesToSymbolsRus.Add("ГМК Норильский никель, ао", "gmk-noril-nickel_rts-ratios");
            NamesToSymbolsRus.Add("Городские Инновационные Технологии, ао", "gorodskiye-innovatsionnyye-tekhnolo-ratios");
            NamesToSymbolsRus.Add("Группа Компаний ПИК, ао", "pik_rts-ratios");
            NamesToSymbolsRus.Add("Группа Компаний Роллман, ао", "gk-rollman-oao-ratios");
            NamesToSymbolsRus.Add("Группа Компаний Роллман, ап", "gk-rollman-oao-pref-ratios");
            NamesToSymbolsRus.Add("Группа ЛСР (ПАО), ао", "lsr-group_rts-ratios");
            NamesToSymbolsRus.Add("Группа Черкизово, ао", "gruppa-cherkizovo-ratios");
            NamesToSymbolsRus.Add("Дагестанская ЭСК, ао", "dagestan-sb-ratios");
            NamesToSymbolsRus.Add("ДВМП (FESCO), ао", "dvmp-oao-ratios");
            NamesToSymbolsRus.Add("Детский мир, ао", "detskiy-mir-pao-ratios");
            NamesToSymbolsRus.Add("Дикси Групп, ао", "dixy-group_rts-ratios");
            NamesToSymbolsRus.Add("Диод, ао", "diod-oao-ratios");
            NamesToSymbolsRus.Add("Донской завод радиодеталей, ао", "donskoy-zavod-radiodetaley-oao-ratios");
            NamesToSymbolsRus.Add("Донской завод радиодеталей, ап", "donskoy-zavod-radiodetaley-oao-pref-ratios");
            NamesToSymbolsRus.Add("Дорогобуж, ао", "dorogobuzh-ratios");
            NamesToSymbolsRus.Add("ДЭК, ао", "dec-ratios");
            NamesToSymbolsRus.Add("Европлан, ао", "yevroplan-pao-ratios");
            NamesToSymbolsRus.Add("Единые Техно Системы, ао", "yedinye-tekhno-sistemy-pao-ratios");
            NamesToSymbolsRus.Add("Звезда, ао", "zvezda-ratios");
            NamesToSymbolsRus.Add("ЗИЛ, ао", "amo-zil-ratios");
            NamesToSymbolsRus.Add("ЗМЗ, ао", "zmz-oao-ratios");
            NamesToSymbolsRus.Add("ЗМЗ, ап", "zavolzhskiy-motornyi-zavod-oao-ratios");
            NamesToSymbolsRus.Add("Ижсталь, ао", "izhstal-ratios");
            NamesToSymbolsRus.Add("Ижсталь, ап", "izhstal-(pref)-ratios");
            NamesToSymbolsRus.Add("ИК РУСС-ИНВЕСТ, ао", "ic-russ-invest-ratios");
            NamesToSymbolsRus.Add("Инвест-Девелопмент, ао", "invest-development-pao-ratios");
            NamesToSymbolsRus.Add("Институт Стволовых Клеток Человека, ао", "human-stem-cells-institute-ratios");
            NamesToSymbolsRus.Add("ИНТЕР РАО, ао", "inter-rao-ees_mm-ratios");
            NamesToSymbolsRus.Add("ИРКУТ, ао", "irkut-corp-ratios");
            NamesToSymbolsRus.Add("Иркутскэнерго, ао", "irkutskenergo-ratios");
            NamesToSymbolsRus.Add("Казаньоргсинтез, ао", "kazanorgsintez-ratios");
            NamesToSymbolsRus.Add("Казаньоргсинтез, ап", "organicheskiy-sintez-kpao-ratios");
            NamesToSymbolsRus.Add("Калужская СК, ао", "kaluga-power-sale-comp-ratios");
            NamesToSymbolsRus.Add("КАМАЗ, ао", "kamaz-ratios");
            NamesToSymbolsRus.Add("Камчатскэнерго, ао", "kamchatskenergo-ratios");
            NamesToSymbolsRus.Add("Камчатскэнерго, ап", "kamchatskenergo-(pref)-ratios");
            NamesToSymbolsRus.Add("Квадра (ТГК-4), ао", "quadra---power-generation-ratios");
            NamesToSymbolsRus.Add("Квадра (ТГК-4), ап", "quadra---power-generation-(pref)-ratios");
            NamesToSymbolsRus.Add("Ковровский механический завод, ао", "kovrovskiy-mekhanicheskiy-ratios");
            NamesToSymbolsRus.Add("Компания М.видео, ао", "mvideo_rts-ratios");
            NamesToSymbolsRus.Add("Коршуновский ГОК, ао", "korshynov-mining-plant-ratios");
            NamesToSymbolsRus.Add("Костромская СК, ао", "kostroma-retail-company-ratios");
            NamesToSymbolsRus.Add("Костромская СК, ап", "kostroma-retail-company-(pref)-ratios");
            NamesToSymbolsRus.Add("Красноярскэнергосбыт, ао", "krasnoyarskenergosbyt-ratios");
            NamesToSymbolsRus.Add("Красноярскэнергосбыт, ап", "krasnoyarskenergosbyt-(pref)-ratios");
            NamesToSymbolsRus.Add("Красный котельщик, ап", "krasny-kotelshchik-(pref)-ratios");
            NamesToSymbolsRus.Add("Красный Октябрь, ао", "krasnyj-octyabr-co.-ratios");
            NamesToSymbolsRus.Add("Красный Октябрь, ап", "krasnyj-octyabr-co.-(pref)-ratios");
            NamesToSymbolsRus.Add("Кубаньэнерго, ао", "kubanenergo-oao-ratios");
            NamesToSymbolsRus.Add("Кубаньэнергосбыт, ао", "kubanenergosbyt-oao-ratios");
            NamesToSymbolsRus.Add("Кузбасская Топливная Компания, ао", "kuzbasskaya-toplivnaya-ratios");
            NamesToSymbolsRus.Add("Куйбышевазот, ао", "kuibyshevazot-ratios");
            NamesToSymbolsRus.Add("Куйбышевазот, ап", "kuibyshevazot-(pref)-ratios");
            NamesToSymbolsRus.Add("Курганская генерирующая компания, ао", "kurganskaya-generiruyushchaya-komp-ratios");
            NamesToSymbolsRus.Add("Курганская генерирующая компания, ап", "kurganskaya-generiruyushchaya-pref-ratios");
            NamesToSymbolsRus.Add("Левенгук, ао", "levenguk-oao-ratios");
            NamesToSymbolsRus.Add("Лензолото, ао", "lenzoloto-oao-ratios");
            NamesToSymbolsRus.Add("Лензолото, ап", "lenzoloto-oao-pref-ratios");
            NamesToSymbolsRus.Add("Ленэнерго, ао", "lenenergo-ratios");
            NamesToSymbolsRus.Add("Ленэнерго, ап", "lenenergo-(pref)-ratios");
            NamesToSymbolsRus.Add("Липецкая ЭСК, ао", "lipetsk-power-sale-comp-ratios");
            NamesToSymbolsRus.Add("Лукойл, ао", "lukoil_rts-ratios");
            NamesToSymbolsRus.Add("Магаданэнерго, ао", "magadanenergo-ratios");
            NamesToSymbolsRus.Add("Магаданэнерго, ап", "magadanenergo-(pref)-ratios");
            NamesToSymbolsRus.Add("Магнит, ао", "magnit_rts-ratios");
            NamesToSymbolsRus.Add("МГТС, ао", "moscow-city-telephone-network-ratios");
            NamesToSymbolsRus.Add("МГТС, ап", "mgts-(pref)-ratios");
            NamesToSymbolsRus.Add("МегаФон, ао", "megafon-oao-ratios");
            NamesToSymbolsRus.Add("Медиа группа Война и Мир, ао", "media-gruppa-voyna-i-mir-oao-ratios");
            NamesToSymbolsRus.Add("Медиахолдинг (ранее О2ТВ), ао", "o2-tv-ratios");
            NamesToSymbolsRus.Add("Мечел, ао", "sg-mechel_rts-ratios");
            NamesToSymbolsRus.Add("Мечел, ап", "mechel-(pref)-ratios");
            NamesToSymbolsRus.Add("ММК, ао", "mmk_rts-ratios");
            NamesToSymbolsRus.Add("Мордовская ЭСК, ао", "mordovskaya-energosbytovaya-ratios");
            NamesToSymbolsRus.Add("Московская Биржа, ао", "moskovskaya-birzha-oao-ratios");
            NamesToSymbolsRus.Add("Московский Кредитный банк, ао", "moskovskiy-kreditnyi-bank-oao-ratios");
            NamesToSymbolsRus.Add("Мособлбанк, ао", "mosoblbank-ratios");
            NamesToSymbolsRus.Add("Мостотрест, ао", "mostotrest_rts-ratios");
            NamesToSymbolsRus.Add("Мосэнерго, ао", "mosenergo_rts-ratios");
            NamesToSymbolsRus.Add("Мотовилихинские заводы, ао", "motovilicha-plants-ratios");
            NamesToSymbolsRus.Add("МОЭСК, ао", "mos-obl-sb_rts-ratios");
            NamesToSymbolsRus.Add("МРСК Волги, ао", "mrsk-volgi-ratios");
            NamesToSymbolsRus.Add("МРСК Северного Кавказа, ао", "mrsk-severnogo-kavkaza-ratios");
            NamesToSymbolsRus.Add("МРСК Северо-Запада, ао", "mrsk-severo-zapada-ratios");
            NamesToSymbolsRus.Add("МРСК Сибири, ао", "mrsk-sibiri-ratios");
            NamesToSymbolsRus.Add("МРСК Урала, ао", "mrsk-urala-ao-ratios");
            NamesToSymbolsRus.Add("МРСК Центра и Приволжья, ао", "mrsk-cip-ratios");
            NamesToSymbolsRus.Add("МРСК Центра, ао", "mrsk-centra-ratios");
            NamesToSymbolsRus.Add("МРСК Юга, ао", "mrsk-yuga-ratios");
            NamesToSymbolsRus.Add("МТС, ао", "mts_rts-ratios");
            NamesToSymbolsRus.Add("Мультисистема, ао", "multisistema-oao-ratios");
            NamesToSymbolsRus.Add("Мурманская ТЭЦ, ао", "murmanskaya-tets-pao-ratios");
            NamesToSymbolsRus.Add("Мурманская ТЭЦ, ап", "murmanskaya-tets-pao-pref-ratios");
            NamesToSymbolsRus.Add("Наука-Связь, ао", "nauka-svyaz-ratios");
            NamesToSymbolsRus.Add("Нефтекамский автозавод, ао", "nefaz-ratios");
            NamesToSymbolsRus.Add("Нижнекамскшина, ао", "nizhnekamskshina-ratios");
            NamesToSymbolsRus.Add("НКНХ, ао", "nizhnekamskneftekhim-ratios");
            NamesToSymbolsRus.Add("НКНХ, ап", "nizhnekamskneftekhim-(pref)-ratios");
            NamesToSymbolsRus.Add("НЛМК, ао", "nlmk_rts-ratios");
            NamesToSymbolsRus.Add("НОВАТЭК, ао", "novatek_rts-ratios");
            NamesToSymbolsRus.Add("Новороссийский морской торговый порт, ао", "nmtp_rts-ratios");
            NamesToSymbolsRus.Add("НПК ОВК, ао", "npk-ovk-pao-ratios");
            NamesToSymbolsRus.Add("НПО Наука, ао", "nauka-ratios");
            NamesToSymbolsRus.Add("НПФ Будущее, ао", "fg-budushcheye-pao-ratios");
            NamesToSymbolsRus.Add("Объединенная авиастроительная корпорация, ао", "united-aircraft-corporation-ratios");
            NamesToSymbolsRus.Add("ОГК-2, ао", "ogk-2_rts-ratios");
            NamesToSymbolsRus.Add("ОКС, ао", "obyedinennye-kreditnye-sist-ratios");
            NamesToSymbolsRus.Add("ОМЗ, ап", "omz-(pref)-ratios");
            NamesToSymbolsRus.Add("ОМПК, ао", "ostankinskiy-myasopererabatyva-ratios");
            NamesToSymbolsRus.Add("Омскшина, ао", "omskshina-oao-ratios");
            NamesToSymbolsRus.Add("ОПИН (Открытые инвестиции), ао", "otkrytye-investitsii-oao-ratios");
            NamesToSymbolsRus.Add("Отисифарм, ао", "otcpharm-pao-ratios");
            NamesToSymbolsRus.Add("Павловский автобус, ао", "pavlovskiy-avtobus-oao-ratios");
            NamesToSymbolsRus.Add("Пермэнергосбыт (бывш. Пермская ЭСК), ао", "perm-sb-ratios");
            NamesToSymbolsRus.Add("Пермэнергосбыт (бывш. Пермская ЭСК), ап", "perm'-energosbyt-pref-ratios");
            NamesToSymbolsRus.Add("Плазмек, ао", "plazmek-oao-ratios");
            NamesToSymbolsRus.Add("Полюс (бывш. Полюс Золото), ао", "polyus-zoloto_rts-ratios");
            NamesToSymbolsRus.Add("Промсвязьбанк, ао", "promsvyazbank-pao-ratios");
            NamesToSymbolsRus.Add("ПРОТЕК, ао", "protek_rts-ratios");
            NamesToSymbolsRus.Add("Распадская, ао", "raspadskaya-ratios");
            NamesToSymbolsRus.Add("РБК (ранее РБК-ТВ Москва), ао", "rbk-tv-moskva-ratios");
            NamesToSymbolsRus.Add("РКК ЭНЕРГИЯ, ао", "rsc-energia-ratios");
            NamesToSymbolsRus.Add("РОС АГРО ПЛС, ДР", "ros-agro-plc-ratios");
            NamesToSymbolsRus.Add("РОСБАНК, ао", "rosbank-ratios");
            NamesToSymbolsRus.Add("Росгосстрах, ао", "rosgosstrakh-oao-ratios");
            NamesToSymbolsRus.Add("РосДорБанк, ао", "rosdorbank-pao-ratios");
            NamesToSymbolsRus.Add("Росинтер Ресторантс Холдинг, ао", "rosinter-restaurants-holding-ratios");
            NamesToSymbolsRus.Add("Роснефть, ао", "rosneft_rts-ratios");
            NamesToSymbolsRus.Add("Россети, ао", "rosseti-ao-ratios");
            NamesToSymbolsRus.Add("Россети, ап", "rosseti-ap-(pref)-ratios");
            NamesToSymbolsRus.Add("Ростелеком, ао", "rostelecom-ratios");
            NamesToSymbolsRus.Add("Ростелеком, ап", "rostelecom-(pref)-ratios");
            NamesToSymbolsRus.Add("РУСАЛ Плс, ао", "united-company-rusal-plc%60-ratios");
            NamesToSymbolsRus.Add("РУСАЛ Плс, ДР", "united-co-rusal-ratios");
            NamesToSymbolsRus.Add("РусГидро, ао", "gidroogk-011d-ratios");
            NamesToSymbolsRus.Add("Русгрэйн Холдинг, ао", "rusgrain-holding-oao-ratios");
            NamesToSymbolsRus.Add("Русолово, ао", "rusolovo-oao-ratios");
            NamesToSymbolsRus.Add("Русполимет, ао", "ruspolimet-ratios");
            NamesToSymbolsRus.Add("Русская Аквакультура (бывш. Русское море), ао", "russian-sea-group-ratios");
            NamesToSymbolsRus.Add("РуссНефть, ао", "ruspetro-ratios");
            NamesToSymbolsRus.Add("Рязанская ЭСК, ао", "ryazan-sb-ratios");
            NamesToSymbolsRus.Add("Самараэнерго, ао", "samaraenergo-ratios");
            NamesToSymbolsRus.Add("Самараэнерго, ап", "samaraenergo(pref)-ratios");
            NamesToSymbolsRus.Add("Саратовский НПЗ, ао", "saratov-oil-refenery-ratios");
            NamesToSymbolsRus.Add("Саратовский НПЗ, ап", "saratov-oil-refenery-(pref)-ratios");
            NamesToSymbolsRus.Add("Саратовэнерго, ао", "saratovenergo-ratios");
            NamesToSymbolsRus.Add("Саратовэнерго, ап", "saratovenergo-(pref)-ratios");
            NamesToSymbolsRus.Add("Сахалинэнерго, ао", "sakhalinenergo-oao-ratios");
            NamesToSymbolsRus.Add("Сбербанк России, ао", "sberbank_rts-ratios");
            NamesToSymbolsRus.Add("Сбербанк России, ап", "sberbank-p_rts-ratios");
            NamesToSymbolsRus.Add("Северсталь, ао", "severstal_rts-ratios");
            NamesToSymbolsRus.Add("Селигдар, ао", "seligdar-ratios");
            NamesToSymbolsRus.Add("Селигдар, ап", "seligdar-pao-ratios");
            NamesToSymbolsRus.Add("СЗП, ао", "north-western-shipping-comp-ratios");
            NamesToSymbolsRus.Add("Сибирский гостинец, ао", "sibirskiy-gostinets-pao-ratios");
            NamesToSymbolsRus.Add("Славнефть-Мегионнефтегаз, ао", "slavneft-megionneftegaz-ratios");
            NamesToSymbolsRus.Add("Славнефть-Мегионнефтегаз, ап", "slavneft-megionneftegaz-(pref)-ratios");
            NamesToSymbolsRus.Add("Славнефть-ЯНОС, ао", "slavneft-ratios");
            NamesToSymbolsRus.Add("Славнефть-ЯНОС, ап", "slavneft-(pref)-ratios");
            NamesToSymbolsRus.Add("СМЗ, ао", "solikamskiy-magniyevyi-zavod-ratios");
            NamesToSymbolsRus.Add("СОЛЛЕРС, ао", "sollers-ratios");
            NamesToSymbolsRus.Add("Ставропольэнергосбыт, ао", "stavropolenergosbyt-ratios");
            NamesToSymbolsRus.Add("Ставропольэнергосбыт, ап", "stavropolenergosbyt-(pref)-ratios");
            NamesToSymbolsRus.Add("Сургутнефтегаз, ао", "surgutneftegas_rts-ratios");
            NamesToSymbolsRus.Add("Сургутнефтегаз, ап", "surgutneftegas-p_rts-ratios");
            NamesToSymbolsRus.Add("Тамбовская ЭСК, ао", "tambov-power-sale-ratios");
            NamesToSymbolsRus.Add("Тамбовская ЭСК, ап", "tambov-power-sale-(pref)-ratios");
            NamesToSymbolsRus.Add("ТАНТАЛ, ао", "tantal-ratios");
            NamesToSymbolsRus.Add("Татнефть, ао", "tatneft_rts-ratios");
            NamesToSymbolsRus.Add("Татнефть, ап", "tatneft-p_rts-ratios");
            NamesToSymbolsRus.Add("Таттелеком, ао", "tattelecom-ratios");
            NamesToSymbolsRus.Add("ТГК-1, ао", "tgk-1-ratios");
            NamesToSymbolsRus.Add("ТГК-14, ао", "tgc-14-ratios");
            NamesToSymbolsRus.Add("ТГК-2, ао", "tgk-2-ratios");
            NamesToSymbolsRus.Add("ТГК-2, ап", "tgk-2-(pref)-ratios");
            NamesToSymbolsRus.Add("ТКЗ, ао", "taganrogskiy-kombaynovyi-zavod-oao-ratios");
            NamesToSymbolsRus.Add("ТМК, ао", "tmk-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Воронеж, ао", "voronezh-sb-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Воронеж, ап", "voronezh-sb-(pref)-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Марий Эл, ао", "marienergosbyt-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Марий Эл, ап", "marienergosbyt-(pref)-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Нижний Новгород, ап", "tns-energo-nizhniy-novgorod-pao-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Ростов-на-Дону, ао", "rostov-sb-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Ростов-на-Дону, ап", "rostov-sb-(pref)-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Ярославль, ао", "yask-ratios");
            NamesToSymbolsRus.Add("ТНС энерго Ярославль, ап", "tns-energo-nizhniy-novgorod-pao-ratios");
            NamesToSymbolsRus.Add("ТНС энерго, ао", "gk-tns-energo-pao-ratios");
            NamesToSymbolsRus.Add("Томская РК, ао", "tomsk-distribution-ratios");
            NamesToSymbolsRus.Add("Томская РК, ап", "tomsk-distribution-(pref)-ratios");
            NamesToSymbolsRus.Add("ТРАНСАЭРО, ао", "ak-transaero-oao-ratios");
            NamesToSymbolsRus.Add("ТрансКонтейнер, ао", "transcontainer-ratios");
            NamesToSymbolsRus.Add("Транснефть, ап", "transneft-p_rts-ratios");
            NamesToSymbolsRus.Add("Тучковский комбинат строительных материалов, ао", "tuchkovskiy-kombinat-stroiteln-ratios");
            NamesToSymbolsRus.Add("УК Арсагера, ао", "arsagera-ratios");
            NamesToSymbolsRus.Add("Уралкалий, ао", "uralkaliy_rts-ratios");
            NamesToSymbolsRus.Add("Уральская кузница, ао", "uralskaya-kuznitsa-oao-ratios");
            NamesToSymbolsRus.Add("Фармсинтез, ао", "pharmsynthez-ratios");
            NamesToSymbolsRus.Add("ФосАгро, ао", "phosagro-ratios");
            NamesToSymbolsRus.Add("ФСК ЕЭС, ао", "fsk-ees_rts-ratios");
            NamesToSymbolsRus.Add("Химпром, ао", "khimprom-ratios");
            NamesToSymbolsRus.Add("Химпром, ап", "khimprom-(pref)-ratios");
            NamesToSymbolsRus.Add("Центральный телеграф, ао", "central-telegraph-ratios");
            NamesToSymbolsRus.Add("Центральный телеграф, ап", "central-telegraph-(pref)-ratios");
            NamesToSymbolsRus.Add("ЦМТ, ао", "cmt-ratios");
            NamesToSymbolsRus.Add("ЦМТ, ап", "cmt-(pref)-ratios");
            NamesToSymbolsRus.Add("Челябинский цинковый завод, ао", "chelabinskyi-cinkovyi-zavod-ratios");
            NamesToSymbolsRus.Add("Челябэнергосбыт, ао", "chelyabenergosbyt-ratios");
            NamesToSymbolsRus.Add("Челябэнергосбыт, ап", "chelyab-energosbyt-ap-ratios");
            NamesToSymbolsRus.Add("ЧЗПСН-Профнастил, ао", "chzpsn-profnastil-oao-ratios");
            NamesToSymbolsRus.Add("ЧКПЗ, ао", "chelyabinskiy-kuznechno-presso-ratios");
            NamesToSymbolsRus.Add("ЧМК, ао", "chmk-ratios");
            NamesToSymbolsRus.Add("ЧТПЗ, ао", "chtpz-ratios");
            NamesToSymbolsRus.Add("Электроцинк, ао", "electrozinc-ratios");
            NamesToSymbolsRus.Add("Энел Россия (бывш. Энел ОГК-5), ао", "ogk-5-ratios");
            NamesToSymbolsRus.Add("Южно-Уральский никелевый комбинат, ао", "kombinat-yuzhuralnikel'-oao-ratios");
            NamesToSymbolsRus.Add("Южный Кузбасс, ао", "south-kuzbass-ratios");
            NamesToSymbolsRus.Add("Юнипро (бывш. Э.ОН Россия), ао", "e.on-russia-ratios");
            NamesToSymbolsRus.Add("ЮТэйр, ао", "utair-aviakompaniya-oao-ratios");
            NamesToSymbolsRus.Add("Якутскэнерго, ао", "yakutskenergo-ratios");
            NamesToSymbolsRus.Add("Якутскэнерго, ап", "yakutskenergo-(pref)-ratios");
            NamesToSymbolsRus.Add("ЯТЭК (бывш. Якутгазпром), ао", "yatek-ratios");
        }

        #endregion
    }
}
