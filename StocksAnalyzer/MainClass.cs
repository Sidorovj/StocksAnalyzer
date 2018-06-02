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
    /// переделать распарс в initializeCurrencies()
    /// Разбить функии в классах на регионы
    /// Добавить лондонскую биржу
    /// Сделать ввод данных по аналайзеру через эксель - вводим веса коэфициентов и названия функций
    /// 


    delegate void MainDel();
    static class MainClass
    {
        private static List<Stock> _stocks = new List<Stock>();
        private static string report = "";
        public static string reportFileName = "";
        private static string[] listToLogInReport = new string[] {"PriceToEquity","PriceToSales","PriceToBook", "ROE", "EPS", "QEG", "ProfitMargin","OperatingMargin","GrossProfit" };

        private const string stockListFilePath = "stockList.dat";

        private static void getRussianStocks()
        {
            int i = 0;
            while (i<=270)
            {
                string htmlCode = Web.GETs(Web.getStocksListUrl_Russia2.Replace("{num}", i.ToString()));
                htmlCode = htmlCode.Substring(htmlCode.IndexOf("<tr class=\"tblr-head\""));
                htmlCode = htmlCode.Substring(0, htmlCode.IndexOf("</table"));
                string tdClass = "<a";
                string title = ">";
                while (htmlCode.Contains(tdClass))
                {
                    string name = "";
                    double price = 0;
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(tdClass) + tdClass.Length); 
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(title) + title.Length);
                    name = htmlCode.Substring(0, htmlCode.IndexOf("<"));
                    if (name == "% за день")
                    {
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf("</tr>"));
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf(tdClass) + tdClass.Length);
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf(title) + title.Length);
                        name = htmlCode.Substring(0, htmlCode.IndexOf("<"));
                    }

                    for (var j = 0; j < 6; j++)
                        htmlCode = htmlCode.Substring(htmlCode.IndexOf("</td>") + 6);
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                    price = htmlCode.Substring(0, htmlCode.IndexOf("</td>")).getDoubleNum();
                    if (price>0 && getStock(false, name)==null)
                        Stocks.Add(new Stock(name, price, new StockMarket(StockMarketLocation.Russia, StockMarketCurrency.RUB)));
                }
                i += 30;
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
        private static void getUSAStocks()
        {
            string[] htmlCode = (Web.DownloadFile(Web.getStocksListUrl_USA_nasdaq).Replace("\",\"", "|").Replace("\"", "")+ Web.DownloadFile(Web.getStocksListUrl_USA_nyse).Replace("\",\"", "|").Replace("\"", "")).Split('\n');
            //htmlCode.Concat(Web.DownloadFile(Web.getStocksListUrl_USA_nyse).Replace("\",\"", "|").Replace("\"", "").Split('\n'));
            foreach (string s in htmlCode)
            {
                if (s.StartsWith("Symbol"))
                    continue;
                var parameters = s.Split('|');
                if (parameters.Length < 2)
                    continue;
                string name = parameters[1], symb = parameters[0];
                double price = parameters[2].getDoubleNum();
                if (price > 0 && getStock(false, name) == null)
                    Stocks.Add(new Stock(name, price, new StockMarket(StockMarketLocation.USA, StockMarketCurrency.USD), symb));
            }
        }
        private static void getLondonStocks()
        {

        }
        public static Stock getStock(bool compFullName, string Name)
        {
            foreach (var st in Stocks)
                if ((compFullName && st.FullName == Name) || (!compFullName && st.Name == Name))
                    return st;
            return null;
        }

        public static double getDoubleNum(this string s)
        {
            // Формат строки: "USD":0.001432
            if (s.IndexOf(":")>0)
                s = s.Substring(s.IndexOf(':') + 1);
            while (s.EndsWith("}") || s.EndsWith("%"))
                s = s.Substring(0, s.Length - 1);
            double res = -1, coef = 1;
            if (s.EndsWith("M"))
            {
                coef = 1000000;
                s = s.Substring(0, s.Length - 1);
            }
            else if (s.EndsWith("B"))
            {
                coef = 1000000000;
                s = s.Substring(0, s.Length - 1);
            }
            if (double.TryParse(s, out res))
                return res*coef;
            if (s.Contains(','))
                s = s.Replace(',', '.');
            else
                s = s.Replace('.', ',');
            if (double.TryParse(s, out res))
                return res*coef;
            return res;
        }
        public static void loadStockListFromFile(string path = stockListFilePath)
        {
            if (!File.Exists(path))
                return; 
            List<Stock> Temp = null;
            Serializer ser = new Serializer(path, Temp);
            Temp = (List<Stock>)ser.Deserialize();
            //foreach (var st in Temp)
            //    st.IsStarred = false;
            ser.Close();
            MainClass.Stocks = Temp;
        }
        public static void writeStockListToFile(string path = stockListFilePath)
        {
            Serializer ser = new Serializer(path, MainClass.Stocks);
            ser.Serialize();
            ser.Close();
        }
        public static void makeReport(List<Stock> lst)
        {
            report += '\n';
            foreach (string param in listToLogInReport)
            {
                string helpSt = "";
                int noRes = 0;
                foreach (var st in lst)
                {
                    if (st[param] == 0)
                    {
                        helpSt += st.Name + ';';
                        noRes++;
                    }
                }
                report += param + ';' +"Заполнен в "+ (lst.Count-noRes).ToString() + '/' + lst.Count.ToString() + ';' + helpSt + '\n';
            }
            reportFileName = "Report_" + DateTime.Now.ToString().Replace('.','-').Replace(' ','_').Replace(':', '-') + ".csv";
            StreamWriter sr = new StreamWriter(reportFileName, true, Encoding.UTF8);
            sr.Write(report);
            sr.Close();
        }
        public static void writeLog(params string[] text)
        {
            string fullText = "";
            foreach (var s in text)
                fullText += DateTime.Now.Hour.ToString("D2") + ':' + DateTime.Now.Minute.ToString("D2") + ':' + DateTime.Now.Second.ToString("D2") + "  " + s + '\n';
            Program.myForm.richTextBoxLog.BeginInvoke((MethodInvoker)(delegate { Program.myForm.richTextBoxLog.Text = fullText + Program.myForm.richTextBoxLog.Text; }));
        }
        public static string loadStocksData(List<Stock> lst, Label lbl, ProgressBar bar)
        {
            var i = 0;
            report = "Не удалось загрузить акции:;";
            var count = lst.Count;
            var countStr = lst.Count.ToString();
            Stopwatch stwatch = new Stopwatch();
            stwatch.Start();
            foreach (var st in lst)
            {
                //Stock sto = lst == Stocks ? st : getStock(false, st.Name);
                getStockData(st);
                i++;
                
                double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)i / count) - 1) / 60.0;
                mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;
                lbl.BeginInvoke((MethodInvoker)(delegate { lbl.Text = "Обработано " + i.ToString() + " / " + countStr + ". Расчетное время: " +(mins>=1?Math.Floor(mins).ToString()+" мин ":"")+ (Math.Floor((mins-Math.Floor(mins))*100)).ToString() + " с"; }));
                bar.BeginInvoke((MethodInvoker)(delegate { bar.Value = i * 100 / count; })); 
            }
            lbl.BeginInvoke((MethodInvoker)(delegate { lbl.Text = "Готово."; }));
            stwatch.Stop();
            return "";
        }

        private static double gettingYahooData(string multipl, ref string htmlCode)
        {
            string temp = htmlCode.Substring(htmlCode.IndexOf(">"+multipl+"</span>"));
            temp = temp.Substring(temp.IndexOf("<td class")); temp = temp.Substring(temp.IndexOf(">") + 1); temp = temp.Substring(0, temp.IndexOf("</"));
            if (temp.IndexOf(">") > 0)
                temp = temp.Substring(temp.IndexOf(">") + 1);
            return temp.getDoubleNum();
        }
        private static double gettingInvestingComData(string mult, ref string htmlCode, string appendix="")
        {
            string sp = "<span class=\"\">";
            string temp = htmlCode.Substring(htmlCode.IndexOf(sp + mult));
            if (appendix != "")
                temp = temp.Substring(temp.IndexOf(appendix + "</i>"));
            temp = temp.Substring(temp.IndexOf("<td>") + 4);
            return temp.Substring(0, temp.IndexOf("</td")).getDoubleNum();
        }
        public static string ToSTR(this double num)
        {
            string str = "";
            if (Math.Abs(num) > 1000000000)// Миллиард
                str = (num / 1000000000).ToString("F2") + " B";
            else if (Math.Abs(num) > 1000000)// Миллион
                str = (num / 1000000).ToString("F2") + " M";
            else if (num!=0)
                str = num.ToString("F2");
            return str;
        }
        public static void getStockData(Stock st)
        {
            string name = st.Name;
            st = getStock(false, st.Name);
            if (st == null)
            {
                writeLog("Не удалось найти акцию в getStockData: " + name);
                return;
            }

            try
            {
                if (st.Market.Location == StockMarketLocation.USA)
                {
                    string htmlCode = Web.GETs(Web.getStockDataUrl_USA.Replace("{}", st.Symbol) + st.Symbol);
                    st.PriceToEquity = gettingYahooData("Trailing P/E", ref htmlCode);
                    st.PriceToSales = gettingYahooData("Price/Sales", ref htmlCode);
                    st.PriceToBook = gettingYahooData("Price/Book", ref htmlCode);
                    st.EPS = gettingYahooData("Diluted EPS", ref htmlCode);
                    st.ROE = gettingYahooData("Return on Equity", ref htmlCode);

                    st.EVtoEBITDA = gettingYahooData("Enterprise Value/EBITDA", ref htmlCode);
                    st.MarketCap = gettingYahooData("Market Cap (intraday)", ref htmlCode);
                    st.QEG = gettingYahooData("Quarterly Revenue Growth", ref htmlCode);
                    st.ProfitMarg = gettingYahooData("Profit Margin", ref htmlCode);
                    st.OperMarg = gettingYahooData("Operating Margin", ref htmlCode);
                    st.GrossProfit = gettingYahooData("Gross Profit", ref htmlCode);
                    st.EV = gettingYahooData("Enterprise Value", ref htmlCode);
                    st.PEG = gettingYahooData("PEG Ratio (5 yr expected)", ref htmlCode);
                    st.EVRev = gettingYahooData("Enterprise Value/Revenue", ref htmlCode);
                    st.RetOnAssets = gettingYahooData("Return on Assets", ref htmlCode);
                    st.Revenue = gettingYahooData("Revenue", ref htmlCode);
                    st.RevPerShare = gettingYahooData("Revenue Per Share", ref htmlCode);
                    st.EBITDA = gettingYahooData("EBITDA", ref htmlCode);
                    st.TotalCash = gettingYahooData("Total Cash", ref htmlCode);
                    st.TotalCashPerShare = gettingYahooData("Total Cash Per Share", ref htmlCode);
                    st.TotalDebt = gettingYahooData("Total Debt", ref htmlCode);
                    st.BookValPerShare = gettingYahooData("Book Value Per Share", ref htmlCode);
                    st.OperatingCashFlow = gettingYahooData("Operating Cash Flow", ref htmlCode);
                    st.LeveredFreeCashFlow = gettingYahooData("Levered Free Cash Flow", ref htmlCode);
                    st.TotalShares = gettingYahooData("Shares Outstanding", ref htmlCode);
                    if (st.EBITDA!=0)
                        st.DebtToEBITDA = st.TotalDebt/st.EBITDA;

                    //htmlCode = htmlCode.Substring(htmlCode.IndexOf("data-reactid=\"35\"")); // react-id каждый раз разный
                    //htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                    //st.Price = htmlCode.Substring(0, htmlCode.IndexOf("<")).getDoubleNum();
                    st.LastUpdate = DateTime.Now;
                }
                else if (st.Market.Location == StockMarketLocation.Russia)
                {
                    if (!namesToSymbolsRus.ContainsKey(st.Name))
                    {
                        writeLog("Нет ссылки для получения инфы для " + st.Name);
                        return;
                    }
                    string htmlCode = Web.GETs(Web.getStockDataUrl_Russia + namesToSymbolsRus[st.Name]);
                    st.PriceToEquity = gettingInvestingComData("Коэффициент цена/прибыль", ref htmlCode);
                    st.PriceToSales = gettingInvestingComData("Коэффициент цена/объем продаж", ref htmlCode);
                    st.PriceToBook = gettingInvestingComData("Коэффициент цена/балансовая стоимость", ref htmlCode);
                    st.EPS = gettingInvestingComData("Базовая прибыль на акцию", ref htmlCode);
                    st.ROE = gettingInvestingComData("Прибыль на инвестиции", ref htmlCode);

                    st.QEG = gettingInvestingComData("Прибыль на акцию за последний квартал к квартальной год назад", ref htmlCode);
                    st.ProfitMarg = gettingInvestingComData("Маржа прибыли до налогообложения ", ref htmlCode, "TTM");
                    st.OperMarg = gettingInvestingComData("Операционная маржа", ref htmlCode, "TTM");
                    st.GrossProfit = gettingInvestingComData("Валовая прибыль", ref htmlCode, "TTM");
                    st.GrossProfit5ya =gettingInvestingComData("Валовая прибыль", ref htmlCode, "5YA");
                    st.ProfitCoef = gettingInvestingComData("Коэффициент прибыльности", ref htmlCode, "TTM");
                    st.ProfitCoef5ya = gettingInvestingComData("Коэффициент прибыльности", ref htmlCode, "5YA");
                    st.ProfitOn12mToAnalogYearAgo = gettingInvestingComData("Прибыль на акцию за последние 12 месяцев к аналогичному периоду год назад", ref htmlCode);
                    st.GrowProfitPerShare5y = gettingInvestingComData("Рост прибыли на акцию за 5 лет", ref htmlCode);
                    st.CapExpenseGrow5y = gettingInvestingComData("Рост капитальных расходов за последние 5 лет", ref htmlCode);
                    st.ProfitMarg5ya = gettingInvestingComData("Маржа прибыли до налогообложения ", ref htmlCode, "5YA");
                    st.OperMarg5ya = gettingInvestingComData("Операционная маржа", ref htmlCode, "5YA");
                    st.UrgentLiquidityCoef = gettingInvestingComData("Коэффициент срочной ликвидности", ref htmlCode);
                    st.CurrentLiquidityCoef = gettingInvestingComData("Коэффициент текущей ликвидности", ref htmlCode);

                    htmlCode = htmlCode.Substring(htmlCode.IndexOf("id=\"last_last\""));
                    htmlCode = htmlCode.Substring(htmlCode.IndexOf(">") + 1);
                    st.Price = htmlCode.Substring(0, htmlCode.IndexOf("<")).getDoubleNum();
                    st.LastUpdate = DateTime.Now;
                }
            }
            catch (Exception er)
            {
                writeLog("Не удалось получить инфу по " + st.Name + ": " + er.Message);
                report += st.Name + ';';
            }

        }
        public static Dictionary<string,string> NamesToSymbolsRus
        {
            get { return namesToSymbolsRus; }
        }
        public async static void Initialize()
        {
            StockMarket.initializeCurrencies();
            await Task.Run(() =>
            {
                Thread.Sleep(2000);
                writeLog(new string[] { "USD: " + StockMarket.getExchangeRates(StockMarketCurrency.USD).ToString("F2"), "EUR: " + StockMarket.getExchangeRates(StockMarketCurrency.EUR).ToString("F2") });
            });
            fillDict();
        }
        public static void getStocksList()
        {
            getRussianStocks();
            getUSAStocks();
            getLondonStocks();
            for (var i = 0; i < Stocks.Count; i++)
                for (var j = 0; j < Stocks.Count - i - 1; j++)
                    if (string.Compare( Stocks[j].Name, Stocks[j+1].Name)>0)
                    {
                        var st = Stocks[j];
                        Stocks[j] = Stocks[j + 1];
                        Stocks[j + 1] = st;
                    }
        }
        public static List<Stock> Stocks
        {
            get { return _stocks; }
            set { _stocks = value; }
        }

        private static Dictionary<string, string> namesToSymbolsRus = new Dictionary<string, string>();
        private static void fillDict()
        {
            namesToSymbolsRus.Add("Polymetal International, ао", "polymetal-ratios?cid=44465");
            namesToSymbolsRus.Add("КИВИ ПиЭлСи, ДР", "qiwi-plc-ratios?cid=960754");
            namesToSymbolsRus.Add("Лента Лтд, ДР", "lenta-ltd-ratios?cid=962408");
            namesToSymbolsRus.Add("ЯНДЕКС, ао", "yandex-ratios?cid=102063");
            //namesToSymbolsRus.Add("Белуга Групп, ао", "");
            //namesToSymbolsRus.Add("Владимирская ЭСК, ап", "-ratios");
            //namesToSymbolsRus.Add("Новороссийский комбинат хлебопродуктов, ао", "-ratios");
            //namesToSymbolsRus.Add("РН-Западная Сибирь, ао", "-ratios");
            //namesToSymbolsRus.Add("ТЗА, ао", "-ratios");
            //namesToSymbolsRus.Add("ТНС энерго Нижний Новгород, ао", "-ratios");
            //namesToSymbolsRus.Add("ЮжУрал-АСКО, ао", "-ratios");

            namesToSymbolsRus.Add("GTL, ао", "gtl-oao-ratios");
            namesToSymbolsRus.Add("Абрау-Дюрсо, ао", "abrau-durso-oao-ratios");
            namesToSymbolsRus.Add("Авангард АКБ, ао", "akb-avangard-oao-ratios");
            namesToSymbolsRus.Add("АВТОВАЗ, ао", "avtovaz-ratios");
            namesToSymbolsRus.Add("АВТОВАЗ, ап", "avtovaz-(pref)-ratios");
            namesToSymbolsRus.Add("Акрон, ао", "akron_rts-ratios");
            namesToSymbolsRus.Add("АЛРОСА, ао", "alrosa-ao-ratios");
            namesToSymbolsRus.Add("АЛРОСА-Нюрба, ао", "alrosa-nyurba-ratios");
            namesToSymbolsRus.Add("Аптечная сеть 36.6, ао", "apteka-36-6_rts-ratios");
            namesToSymbolsRus.Add("Армада, ао", "armada-ratios");
            namesToSymbolsRus.Add("Астраханская ЭСК, ао", "astrakhan-power-sale-comp-ratios");
            namesToSymbolsRus.Add("АФК Система, ао", "afk-sistema_rts-ratios");
            namesToSymbolsRus.Add("Ашинский метзавод, ао", "ashinskiy-metallurgical-works-ratios");
            namesToSymbolsRus.Add("Аэрофлот, ао", "aeroflot-ratios");
            namesToSymbolsRus.Add("Банк Возрождение, ао", "vozrozhdenie_rts-ratios");
            namesToSymbolsRus.Add("Банк Возрождение, ап", "bank-vozrozhdeniye-pao-ratios");
            namesToSymbolsRus.Add("Банк Кузнецкий, ао", "bank-kuznetskiy-oao-ratios");
            namesToSymbolsRus.Add("АКБ Приморье, ао", "akb-primorye-oao-ratios");
            namesToSymbolsRus.Add("Банк Санкт-Петербург, ао", "bank-st-petersbr_rts-ratios");
            namesToSymbolsRus.Add("БАНК УРАЛСИБ, ао", "bank-uralsib-ratios");
            namesToSymbolsRus.Add("Банк ФК Открытие, ао", "nomos-bank-ratios");
            namesToSymbolsRus.Add("Башинформсвязь, ао", "bashinformsvyaz-ratios");
            namesToSymbolsRus.Add("Башинформсвязь, ап", "bashinformsvyaz-(pref)-ratios");
            namesToSymbolsRus.Add("Башнефть, ао", "bashneft_rts-ratios");
            namesToSymbolsRus.Add("Башнефть, ап", "bashneft-(pref)-ratios");
            namesToSymbolsRus.Add("Белон, ао", "belon_rts-ratios");
            namesToSymbolsRus.Add("Бест Эффортс Банк (бывш. АЛОР БАНК), ао", "alor-bank-oao-ratios");
            namesToSymbolsRus.Add("Бурятзолото, ао", "buryatzoloto-ratios");
            namesToSymbolsRus.Add("Варьеганнефтегаз, ао", "varyeganneftegaz-ratios");
            namesToSymbolsRus.Add("Варьеганнефтегаз, ап", "varyeganneftegaz-(pref)-ratios");
            namesToSymbolsRus.Add("Владимирская ЭСК, ао", "vladimirenergosbyt-oao-ratios");
            namesToSymbolsRus.Add("Волгоградская ЭСК, ао", "volgogradenergosbyt-ratios");
            namesToSymbolsRus.Add("Волгоградская ЭСК, ап", "volgogradenergosbyt-(pref)-ratios");
            namesToSymbolsRus.Add("ВСМПО-АВИСМА, ао", "vsmpo-avisma-crp_rts-ratios");
            namesToSymbolsRus.Add("ВТБ, ао", "vtb_rts-ratios");
            namesToSymbolsRus.Add("ВТОРРЕСУРСЫ, ао", "vtorresursy-oao-ratios");
            namesToSymbolsRus.Add("ВХЗ, ао", "vladimirskiy-khimicheskiy-ratios");
            namesToSymbolsRus.Add("Выборгский судостроительный завод, ао", "vyborgskiy-sudostroitelnyi-zavod-ratios");
            namesToSymbolsRus.Add("Выборгский судостроительный завод, ап", "vyborgskiy-sudostroitelnyi-pao-ratios");
            namesToSymbolsRus.Add("ГАЗ, ао", "gaz-auto-plant-ratios");
            namesToSymbolsRus.Add("ГАЗ, ап", "gaz-auto-plant-(pref)-ratios");
            namesToSymbolsRus.Add("Газпром газораспределение Ростов-на-Дону, ао", "gazprom-gazoraspredeleniye-ratios");
            namesToSymbolsRus.Add("Газпром нефть, ао", "gazprom-neft_rts-ratios");
            namesToSymbolsRus.Add("Газпром, ао", "gazprom_rts-ratios");
            namesToSymbolsRus.Add("Галс-Девелопмент (бывш. Система-Галс), ао", "gals-development-ratios");
            namesToSymbolsRus.Add("ГМК Норильский никель, ао", "gmk-noril-nickel_rts-ratios");
            namesToSymbolsRus.Add("Городские Инновационные Технологии, ао", "gorodskiye-innovatsionnyye-tekhnolo-ratios");
            namesToSymbolsRus.Add("Группа Компаний ПИК, ао", "pik_rts-ratios");
            namesToSymbolsRus.Add("Группа Компаний Роллман, ао", "gk-rollman-oao-ratios");
            namesToSymbolsRus.Add("Группа Компаний Роллман, ап", "gk-rollman-oao-pref-ratios");
            namesToSymbolsRus.Add("Группа ЛСР (ПАО), ао", "lsr-group_rts-ratios");
            namesToSymbolsRus.Add("Группа Черкизово, ао", "gruppa-cherkizovo-ratios");
            namesToSymbolsRus.Add("Дагестанская ЭСК, ао", "dagestan-sb-ratios");
            namesToSymbolsRus.Add("ДВМП (FESCO), ао", "dvmp-oao-ratios");
            namesToSymbolsRus.Add("Детский мир, ао", "detskiy-mir-pao-ratios");
            namesToSymbolsRus.Add("Дикси Групп, ао", "dixy-group_rts-ratios");
            namesToSymbolsRus.Add("Диод, ао", "diod-oao-ratios");
            namesToSymbolsRus.Add("Донской завод радиодеталей, ао", "donskoy-zavod-radiodetaley-oao-ratios");
            namesToSymbolsRus.Add("Донской завод радиодеталей, ап", "donskoy-zavod-radiodetaley-oao-pref-ratios");
            namesToSymbolsRus.Add("Дорогобуж, ао", "dorogobuzh-ratios");
            namesToSymbolsRus.Add("ДЭК, ао", "dec-ratios");
            namesToSymbolsRus.Add("Европлан, ао", "yevroplan-pao-ratios");
            namesToSymbolsRus.Add("Единые Техно Системы, ао", "yedinye-tekhno-sistemy-pao-ratios");
            namesToSymbolsRus.Add("Звезда, ао", "zvezda-ratios");
            namesToSymbolsRus.Add("ЗИЛ, ао", "amo-zil-ratios");
            namesToSymbolsRus.Add("ЗМЗ, ао", "zmz-oao-ratios");
            namesToSymbolsRus.Add("ЗМЗ, ап", "zavolzhskiy-motornyi-zavod-oao-ratios");
            namesToSymbolsRus.Add("Ижсталь, ао", "izhstal-ratios");
            namesToSymbolsRus.Add("Ижсталь, ап", "izhstal-(pref)-ratios");
            namesToSymbolsRus.Add("ИК РУСС-ИНВЕСТ, ао", "ic-russ-invest-ratios");
            namesToSymbolsRus.Add("Инвест-Девелопмент, ао", "invest-development-pao-ratios");
            namesToSymbolsRus.Add("Институт Стволовых Клеток Человека, ао", "human-stem-cells-institute-ratios");
            namesToSymbolsRus.Add("ИНТЕР РАО, ао", "inter-rao-ees_mm-ratios");
            namesToSymbolsRus.Add("ИРКУТ, ао", "irkut-corp-ratios");
            namesToSymbolsRus.Add("Иркутскэнерго, ао", "irkutskenergo-ratios");
            namesToSymbolsRus.Add("Казаньоргсинтез, ао", "kazanorgsintez-ratios");
            namesToSymbolsRus.Add("Казаньоргсинтез, ап", "organicheskiy-sintez-kpao-ratios");
            namesToSymbolsRus.Add("Калужская СК, ао", "kaluga-power-sale-comp-ratios");
            namesToSymbolsRus.Add("КАМАЗ, ао", "kamaz-ratios");
            namesToSymbolsRus.Add("Камчатскэнерго, ао", "kamchatskenergo-ratios");
            namesToSymbolsRus.Add("Камчатскэнерго, ап", "kamchatskenergo-(pref)-ratios");
            namesToSymbolsRus.Add("Квадра (ТГК-4), ао", "quadra---power-generation-ratios");
            namesToSymbolsRus.Add("Квадра (ТГК-4), ап", "quadra---power-generation-(pref)-ratios");
            namesToSymbolsRus.Add("Ковровский механический завод, ао", "kovrovskiy-mekhanicheskiy-ratios");
            namesToSymbolsRus.Add("Компания М.видео, ао", "mvideo_rts-ratios");
            namesToSymbolsRus.Add("Коршуновский ГОК, ао", "korshynov-mining-plant-ratios");
            namesToSymbolsRus.Add("Костромская СК, ао", "kostroma-retail-company-ratios");
            namesToSymbolsRus.Add("Костромская СК, ап", "kostroma-retail-company-(pref)-ratios");
            namesToSymbolsRus.Add("Красноярскэнергосбыт, ао", "krasnoyarskenergosbyt-ratios");
            namesToSymbolsRus.Add("Красноярскэнергосбыт, ап", "krasnoyarskenergosbyt-(pref)-ratios");
            namesToSymbolsRus.Add("Красный котельщик, ап", "krasny-kotelshchik-(pref)-ratios");
            namesToSymbolsRus.Add("Красный Октябрь, ао", "krasnyj-octyabr-co.-ratios");
            namesToSymbolsRus.Add("Красный Октябрь, ап", "krasnyj-octyabr-co.-(pref)-ratios");
            namesToSymbolsRus.Add("Кубаньэнерго, ао", "kubanenergo-oao-ratios");
            namesToSymbolsRus.Add("Кубаньэнергосбыт, ао", "kubanenergosbyt-oao-ratios");
            namesToSymbolsRus.Add("Кузбасская Топливная Компания, ао", "kuzbasskaya-toplivnaya-ratios");
            namesToSymbolsRus.Add("Куйбышевазот, ао", "kuibyshevazot-ratios");
            namesToSymbolsRus.Add("Куйбышевазот, ап", "kuibyshevazot-(pref)-ratios");
            namesToSymbolsRus.Add("Курганская генерирующая компания, ао", "kurganskaya-generiruyushchaya-komp-ratios");
            namesToSymbolsRus.Add("Курганская генерирующая компания, ап", "kurganskaya-generiruyushchaya-pref-ratios");
            namesToSymbolsRus.Add("Левенгук, ао", "levenguk-oao-ratios");
            namesToSymbolsRus.Add("Лензолото, ао", "lenzoloto-oao-ratios");
            namesToSymbolsRus.Add("Лензолото, ап", "lenzoloto-oao-pref-ratios");
            namesToSymbolsRus.Add("Ленэнерго, ао", "lenenergo-ratios");
            namesToSymbolsRus.Add("Ленэнерго, ап", "lenenergo-(pref)-ratios");
            namesToSymbolsRus.Add("Липецкая ЭСК, ао", "lipetsk-power-sale-comp-ratios");
            namesToSymbolsRus.Add("Лукойл, ао", "lukoil_rts-ratios");
            namesToSymbolsRus.Add("Магаданэнерго, ао", "magadanenergo-ratios");
            namesToSymbolsRus.Add("Магаданэнерго, ап", "magadanenergo-(pref)-ratios");
            namesToSymbolsRus.Add("Магнит, ао", "magnit_rts-ratios");
            namesToSymbolsRus.Add("МГТС, ао", "moscow-city-telephone-network-ratios");
            namesToSymbolsRus.Add("МГТС, ап", "mgts-(pref)-ratios");
            namesToSymbolsRus.Add("МегаФон, ао", "megafon-oao-ratios");
            namesToSymbolsRus.Add("Медиа группа Война и Мир, ао", "media-gruppa-voyna-i-mir-oao-ratios");
            namesToSymbolsRus.Add("Медиахолдинг (ранее О2ТВ), ао", "o2-tv-ratios");
            namesToSymbolsRus.Add("Мечел, ао", "sg-mechel_rts-ratios");
            namesToSymbolsRus.Add("Мечел, ап", "mechel-(pref)-ratios");
            namesToSymbolsRus.Add("ММК, ао", "mmk_rts-ratios");
            namesToSymbolsRus.Add("Мордовская ЭСК, ао", "mordovskaya-energosbytovaya-ratios");
            namesToSymbolsRus.Add("Московская Биржа, ао", "moskovskaya-birzha-oao-ratios");
            namesToSymbolsRus.Add("Московский Кредитный банк, ао", "moskovskiy-kreditnyi-bank-oao-ratios");
            namesToSymbolsRus.Add("Мособлбанк, ао", "mosoblbank-ratios");
            namesToSymbolsRus.Add("Мостотрест, ао", "mostotrest_rts-ratios");
            namesToSymbolsRus.Add("Мосэнерго, ао", "mosenergo_rts-ratios");
            namesToSymbolsRus.Add("Мотовилихинские заводы, ао", "motovilicha-plants-ratios");
            namesToSymbolsRus.Add("МОЭСК, ао", "mos-obl-sb_rts-ratios");
            namesToSymbolsRus.Add("МРСК Волги, ао", "mrsk-volgi-ratios");
            namesToSymbolsRus.Add("МРСК Северного Кавказа, ао", "mrsk-severnogo-kavkaza-ratios");
            namesToSymbolsRus.Add("МРСК Северо-Запада, ао", "mrsk-severo-zapada-ratios");
            namesToSymbolsRus.Add("МРСК Сибири, ао", "mrsk-sibiri-ratios");
            namesToSymbolsRus.Add("МРСК Урала, ао", "mrsk-urala-ao-ratios");
            namesToSymbolsRus.Add("МРСК Центра и Приволжья, ао", "mrsk-cip-ratios");
            namesToSymbolsRus.Add("МРСК Центра, ао", "mrsk-centra-ratios");
            namesToSymbolsRus.Add("МРСК Юга, ао", "mrsk-yuga-ratios");
            namesToSymbolsRus.Add("МТС, ао", "mts_rts-ratios");
            namesToSymbolsRus.Add("Мультисистема, ао", "multisistema-oao-ratios");
            namesToSymbolsRus.Add("Мурманская ТЭЦ, ао", "murmanskaya-tets-pao-ratios");
            namesToSymbolsRus.Add("Мурманская ТЭЦ, ап", "murmanskaya-tets-pao-pref-ratios");
            namesToSymbolsRus.Add("Наука-Связь, ао", "nauka-svyaz-ratios");
            namesToSymbolsRus.Add("Нефтекамский автозавод, ао", "nefaz-ratios");
            namesToSymbolsRus.Add("Нижнекамскшина, ао", "nizhnekamskshina-ratios");
            namesToSymbolsRus.Add("НКНХ, ао", "nizhnekamskneftekhim-ratios");
            namesToSymbolsRus.Add("НКНХ, ап", "nizhnekamskneftekhim-(pref)-ratios");
            namesToSymbolsRus.Add("НЛМК, ао", "nlmk_rts-ratios");
            namesToSymbolsRus.Add("НОВАТЭК, ао", "novatek_rts-ratios");
            namesToSymbolsRus.Add("Новороссийский морской торговый порт, ао", "nmtp_rts-ratios");
            namesToSymbolsRus.Add("НПК ОВК, ао", "npk-ovk-pao-ratios");
            namesToSymbolsRus.Add("НПО Наука, ао", "nauka-ratios");
            namesToSymbolsRus.Add("НПФ Будущее, ао", "fg-budushcheye-pao-ratios");
            namesToSymbolsRus.Add("Объединенная авиастроительная корпорация, ао", "united-aircraft-corporation-ratios");
            namesToSymbolsRus.Add("ОГК-2, ао", "ogk-2_rts-ratios");
            namesToSymbolsRus.Add("ОКС, ао", "obyedinennye-kreditnye-sist-ratios");
            namesToSymbolsRus.Add("ОМЗ, ап", "omz-(pref)-ratios");
            namesToSymbolsRus.Add("ОМПК, ао", "ostankinskiy-myasopererabatyva-ratios");
            namesToSymbolsRus.Add("Омскшина, ао", "omskshina-oao-ratios");
            namesToSymbolsRus.Add("ОПИН (Открытые инвестиции), ао", "otkrytye-investitsii-oao-ratios");
            namesToSymbolsRus.Add("Отисифарм, ао", "otcpharm-pao-ratios");
            namesToSymbolsRus.Add("Павловский автобус, ао", "pavlovskiy-avtobus-oao-ratios");
            namesToSymbolsRus.Add("Пермэнергосбыт (бывш. Пермская ЭСК), ао", "perm-sb-ratios");
            namesToSymbolsRus.Add("Пермэнергосбыт (бывш. Пермская ЭСК), ап", "perm'-energosbyt-pref-ratios");
            namesToSymbolsRus.Add("Плазмек, ао", "plazmek-oao-ratios");
            namesToSymbolsRus.Add("Полюс (бывш. Полюс Золото), ао", "polyus-zoloto_rts-ratios");
            namesToSymbolsRus.Add("Промсвязьбанк, ао", "promsvyazbank-pao-ratios");
            namesToSymbolsRus.Add("ПРОТЕК, ао", "protek_rts-ratios");
            namesToSymbolsRus.Add("Распадская, ао", "raspadskaya-ratios");
            namesToSymbolsRus.Add("РБК (ранее РБК-ТВ Москва), ао", "rbk-tv-moskva-ratios");
            namesToSymbolsRus.Add("РКК ЭНЕРГИЯ, ао", "rsc-energia-ratios");
            namesToSymbolsRus.Add("РОС АГРО ПЛС, ДР", "ros-agro-plc-ratios");
            namesToSymbolsRus.Add("РОСБАНК, ао", "rosbank-ratios");
            namesToSymbolsRus.Add("Росгосстрах, ао", "rosgosstrakh-oao-ratios");
            namesToSymbolsRus.Add("РосДорБанк, ао", "rosdorbank-pao-ratios");
            namesToSymbolsRus.Add("Росинтер Ресторантс Холдинг, ао", "rosinter-restaurants-holding-ratios");
            namesToSymbolsRus.Add("Роснефть, ао", "rosneft_rts-ratios");
            namesToSymbolsRus.Add("Россети, ао", "rosseti-ao-ratios");
            namesToSymbolsRus.Add("Россети, ап", "rosseti-ap-(pref)-ratios");
            namesToSymbolsRus.Add("Ростелеком, ао", "rostelecom-ratios");
            namesToSymbolsRus.Add("Ростелеком, ап", "rostelecom-(pref)-ratios");
            namesToSymbolsRus.Add("РУСАЛ Плс, ао", "united-company-rusal-plc%60-ratios");
            namesToSymbolsRus.Add("РУСАЛ Плс, ДР", "united-co-rusal-ratios");
            namesToSymbolsRus.Add("РусГидро, ао", "gidroogk-011d-ratios");
            namesToSymbolsRus.Add("Русгрэйн Холдинг, ао", "rusgrain-holding-oao-ratios");
            namesToSymbolsRus.Add("Русолово, ао", "rusolovo-oao-ratios");
            namesToSymbolsRus.Add("Русполимет, ао", "ruspolimet-ratios");
            namesToSymbolsRus.Add("Русская Аквакультура (бывш. Русское море), ао", "russian-sea-group-ratios");
            namesToSymbolsRus.Add("РуссНефть, ао", "ruspetro-ratios");
            namesToSymbolsRus.Add("Рязанская ЭСК, ао", "ryazan-sb-ratios");
            namesToSymbolsRus.Add("Самараэнерго, ао", "samaraenergo-ratios");
            namesToSymbolsRus.Add("Самараэнерго, ап", "samaraenergo(pref)-ratios");
            namesToSymbolsRus.Add("Саратовский НПЗ, ао", "saratov-oil-refenery-ratios");
            namesToSymbolsRus.Add("Саратовский НПЗ, ап", "saratov-oil-refenery-(pref)-ratios");
            namesToSymbolsRus.Add("Саратовэнерго, ао", "saratovenergo-ratios");
            namesToSymbolsRus.Add("Саратовэнерго, ап", "saratovenergo-(pref)-ratios");
            namesToSymbolsRus.Add("Сахалинэнерго, ао", "sakhalinenergo-oao-ratios");
            namesToSymbolsRus.Add("Сбербанк России, ао", "sberbank_rts-ratios");
            namesToSymbolsRus.Add("Сбербанк России, ап", "sberbank-p_rts-ratios");
            namesToSymbolsRus.Add("Северсталь, ао", "severstal_rts-ratios");
            namesToSymbolsRus.Add("Селигдар, ао", "seligdar-ratios");
            namesToSymbolsRus.Add("Селигдар, ап", "seligdar-pao-ratios");
            namesToSymbolsRus.Add("СЗП, ао", "north-western-shipping-comp-ratios");
            namesToSymbolsRus.Add("Сибирский гостинец, ао", "sibirskiy-gostinets-pao-ratios");
            namesToSymbolsRus.Add("Славнефть-Мегионнефтегаз, ао", "slavneft-megionneftegaz-ratios");
            namesToSymbolsRus.Add("Славнефть-Мегионнефтегаз, ап", "slavneft-megionneftegaz-(pref)-ratios");
            namesToSymbolsRus.Add("Славнефть-ЯНОС, ао", "slavneft-ratios");
            namesToSymbolsRus.Add("Славнефть-ЯНОС, ап", "slavneft-(pref)-ratios");
            namesToSymbolsRus.Add("СМЗ, ао", "solikamskiy-magniyevyi-zavod-ratios");
            namesToSymbolsRus.Add("СОЛЛЕРС, ао", "sollers-ratios");
            namesToSymbolsRus.Add("Ставропольэнергосбыт, ао", "stavropolenergosbyt-ratios");
            namesToSymbolsRus.Add("Ставропольэнергосбыт, ап", "stavropolenergosbyt-(pref)-ratios");
            namesToSymbolsRus.Add("Сургутнефтегаз, ао", "surgutneftegas_rts-ratios");
            namesToSymbolsRus.Add("Сургутнефтегаз, ап", "surgutneftegas-p_rts-ratios");
            namesToSymbolsRus.Add("Тамбовская ЭСК, ао", "tambov-power-sale-ratios");
            namesToSymbolsRus.Add("Тамбовская ЭСК, ап", "tambov-power-sale-(pref)-ratios");
            namesToSymbolsRus.Add("ТАНТАЛ, ао", "tantal-ratios");
            namesToSymbolsRus.Add("Татнефть, ао", "tatneft_rts-ratios");
            namesToSymbolsRus.Add("Татнефть, ап", "tatneft-p_rts-ratios");
            namesToSymbolsRus.Add("Таттелеком, ао", "tattelecom-ratios");
            namesToSymbolsRus.Add("ТГК-1, ао", "tgk-1-ratios");
            namesToSymbolsRus.Add("ТГК-14, ао", "tgc-14-ratios");
            namesToSymbolsRus.Add("ТГК-2, ао", "tgk-2-ratios");
            namesToSymbolsRus.Add("ТГК-2, ап", "tgk-2-(pref)-ratios");
            namesToSymbolsRus.Add("ТКЗ, ао", "taganrogskiy-kombaynovyi-zavod-oao-ratios");
            namesToSymbolsRus.Add("ТМК, ао", "tmk-ratios");
            namesToSymbolsRus.Add("ТНС энерго Воронеж, ао", "voronezh-sb-ratios");
            namesToSymbolsRus.Add("ТНС энерго Воронеж, ап", "voronezh-sb-(pref)-ratios");
            namesToSymbolsRus.Add("ТНС энерго Марий Эл, ао", "marienergosbyt-ratios");
            namesToSymbolsRus.Add("ТНС энерго Марий Эл, ап", "marienergosbyt-(pref)-ratios");
            namesToSymbolsRus.Add("ТНС энерго Нижний Новгород, ап", "tns-energo-nizhniy-novgorod-pao-ratios");
            namesToSymbolsRus.Add("ТНС энерго Ростов-на-Дону, ао", "rostov-sb-ratios");
            namesToSymbolsRus.Add("ТНС энерго Ростов-на-Дону, ап", "rostov-sb-(pref)-ratios");
            namesToSymbolsRus.Add("ТНС энерго Ярославль, ао", "yask-ratios");
            namesToSymbolsRus.Add("ТНС энерго Ярославль, ап", "tns-energo-nizhniy-novgorod-pao-ratios");
            namesToSymbolsRus.Add("ТНС энерго, ао", "gk-tns-energo-pao-ratios");
            namesToSymbolsRus.Add("Томская РК, ао", "tomsk-distribution-ratios");
            namesToSymbolsRus.Add("Томская РК, ап", "tomsk-distribution-(pref)-ratios");
            namesToSymbolsRus.Add("ТРАНСАЭРО, ао", "ak-transaero-oao-ratios");
            namesToSymbolsRus.Add("ТрансКонтейнер, ао", "transcontainer-ratios");
            namesToSymbolsRus.Add("Транснефть, ап", "transneft-p_rts-ratios");
            namesToSymbolsRus.Add("Тучковский комбинат строительных материалов, ао", "tuchkovskiy-kombinat-stroiteln-ratios");
            namesToSymbolsRus.Add("УК Арсагера, ао", "arsagera-ratios");
            namesToSymbolsRus.Add("Уралкалий, ао", "uralkaliy_rts-ratios");
            namesToSymbolsRus.Add("Уральская кузница, ао", "uralskaya-kuznitsa-oao-ratios");
            namesToSymbolsRus.Add("Фармсинтез, ао", "pharmsynthez-ratios");
            namesToSymbolsRus.Add("ФосАгро, ао", "phosagro-ratios");
            namesToSymbolsRus.Add("ФСК ЕЭС, ао", "fsk-ees_rts-ratios");
            namesToSymbolsRus.Add("Химпром, ао", "khimprom-ratios");
            namesToSymbolsRus.Add("Химпром, ап", "khimprom-(pref)-ratios");
            namesToSymbolsRus.Add("Центральный телеграф, ао", "central-telegraph-ratios");
            namesToSymbolsRus.Add("Центральный телеграф, ап", "central-telegraph-(pref)-ratios");
            namesToSymbolsRus.Add("ЦМТ, ао", "cmt-ratios");
            namesToSymbolsRus.Add("ЦМТ, ап", "cmt-(pref)-ratios");
            namesToSymbolsRus.Add("Челябинский цинковый завод, ао", "chelabinskyi-cinkovyi-zavod-ratios");
            namesToSymbolsRus.Add("Челябэнергосбыт, ао", "chelyabenergosbyt-ratios");
            namesToSymbolsRus.Add("Челябэнергосбыт, ап", "chelyab-energosbyt-ap-ratios");
            namesToSymbolsRus.Add("ЧЗПСН-Профнастил, ао", "chzpsn-profnastil-oao-ratios");
            namesToSymbolsRus.Add("ЧКПЗ, ао", "chelyabinskiy-kuznechno-presso-ratios");
            namesToSymbolsRus.Add("ЧМК, ао", "chmk-ratios");
            namesToSymbolsRus.Add("ЧТПЗ, ао", "chtpz-ratios");
            namesToSymbolsRus.Add("Электроцинк, ао", "electrozinc-ratios");
            namesToSymbolsRus.Add("Энел Россия (бывш. Энел ОГК-5), ао", "ogk-5-ratios");
            namesToSymbolsRus.Add("Южно-Уральский никелевый комбинат, ао", "kombinat-yuzhuralnikel'-oao-ratios");
            namesToSymbolsRus.Add("Южный Кузбасс, ао", "south-kuzbass-ratios");
            namesToSymbolsRus.Add("Юнипро (бывш. Э.ОН Россия), ао", "e.on-russia-ratios");
            namesToSymbolsRus.Add("ЮТэйр, ао", "utair-aviakompaniya-oao-ratios");
            namesToSymbolsRus.Add("Якутскэнерго, ао", "yakutskenergo-ratios");
            namesToSymbolsRus.Add("Якутскэнерго, ап", "yakutskenergo-(pref)-ratios");
            namesToSymbolsRus.Add("ЯТЭК (бывш. Якутгазпром), ао", "yatek-ratios");
        }
    }
}
