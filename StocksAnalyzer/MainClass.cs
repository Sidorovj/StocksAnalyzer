﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;


namespace StocksAnalyzer
{

    internal static class MainClass
    {
        public const double Tolerance = 1e-7;

        public static string ReportFileName { get; private set; } = "";
        public static Dictionary<string, string> NamesToSymbolsRus { get; } = new Dictionary<string, string>();
        public static List<Stock> Stocks { get; set; } = new List<Stock>();

        private static string s_report = "";

        private static int s_doneEventsCount;

        private static readonly object s_rusStockLoaderLocker = new object();


        #region Methods:public

        public static Stock GetStock(bool compareFullName, string name)
        {
            return Stocks.FirstOrDefault(st => compareFullName && st.FullName == name || !compareFullName && st.Name == name);
        }

        /// <summary>
        /// Преобразует строку вида "USD":0.001432 / "RUB":2B в double
        /// </summary>
        /// <param name="stringValue">Формат строки: "USD":0.001432</param>
        /// <returns></returns>
        public static double? ParseCoefStrToDouble(this string stringValue)
        {
            if (stringValue.IndexOf(":", StringComparison.Ordinal) > 0)
                stringValue = stringValue.Substring(stringValue.IndexOf(':') + 1);
            while (stringValue.EndsWith("}") || stringValue.EndsWith("%"))
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            double coefficient = 1;
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
            else if (stringValue.EndsWith("k"))
            {
                coefficient = 1000;
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            else if (stringValue.EndsWith("T"))
            {
                coefficient = 1000.0 * 1000 * 1000 * 1000;
                stringValue = stringValue.Substring(0, stringValue.Length - 1);
            }
            if (stringValue.Contains(",") && stringValue.Contains("."))
            {
                stringValue = stringValue.Replace(stringValue.IndexOf(",", StringComparison.Ordinal) <
                                                  stringValue.IndexOf(".", StringComparison.Ordinal) ? "," : ".", "");
            }
            if (double.TryParse(stringValue, out var result))
                return result * coefficient;
            stringValue = stringValue.Contains(',') ? stringValue.Replace(',', '.') : stringValue.Replace('.', ',');
            if (double.TryParse(stringValue, out result))
                return result * coefficient;
            if (stringValue == "n/a" || stringValue == "-" || stringValue == "N/A" || stringValue == "")
                return null;
            throw new Exception($"Не удается распарсить строку {stringValue}");
        }

        /// <summary>
        /// Загружает список из файла
        /// </summary>
        /// <param name="path"></param>
        public static void LoadStockListFromFile(string path = Const.StockListFilePath)
        {
            string fullPath = $"{Const.HistoryDirName}/{path}";
            if (!Directory.Exists(Const.HistoryDirName) || !File.Exists(fullPath))
            {
                WriteLog(@"Не могу найти файл для десериализации");
                return;
            }

            Serializer ser = new Serializer(fullPath);
            Stocks = (List<Stock>)ser.Deserialize() ?? new List<Stock>();
        }

        /// <summary>
        /// Сериализует список акций в файл
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public static void WriteStockListToFile(string path = Const.StockListFilePath)
        {
            if (!Directory.Exists(Const.HistoryDirName))
                Directory.CreateDirectory(Const.HistoryDirName);
            Serializer ser = new Serializer($"{Const.HistoryDirName}/{path}");
            ser.Serialize(Stocks);
        }

        /// <summary>
        /// Составить отчет по списку акций и записать в файл
        /// </summary>
        /// <param name="stockLst">Список акций</param>
        public static void MakeReportAndSaveToFile(List<Stock> stockLst)
        {
            s_report += '\n';
            foreach (var coef in Coefficient.CoefficientList)
            {
                var helpSt = "";
                var numRes = 0;
                foreach (var st in stockLst)
                {
                    if (Math.Abs(st[coef] ?? 0) < Tolerance)
                    {
                        helpSt += st.Name + ';';
                        numRes++;
                    }
                }
                s_report += $"{coef};Заполнен в {stockLst.Count - numRes}/{stockLst.Count};{helpSt}\r\n";
            }

            if (!Directory.Exists(Const.ReportDirName))
                Directory.CreateDirectory(Const.ReportDirName);
            ReportFileName = $"{Const.ReportDirName}/Report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
            using (var sr = new StreamWriter(ReportFileName, true, Encoding.UTF8))
            {
                sr.Write(s_report);
            }
        }

        /// <summary>
        /// Записать лог в текстБокс на форме
        /// </summary>
        /// <param name="text">Строки лога</param>
        /// <param name="writeLogToFile">Записывать лог</param>
        public static async void WriteLog(string text, bool writeLogToFile = true)
        {
            if (writeLogToFile)
                Logger.Log.Info(text);
            while (Program.MyForm == null)
                await Task.Delay(100);

            text = $"{DateTime.Now:HH-mm-ss}:  {text} {Environment.NewLine}";
            if (Program.MyForm.richTextBoxLog.InvokeRequired)
                Program.MyForm.richTextBoxLog.BeginInvoke(
                    (MethodInvoker)delegate
                   {
                       Program.MyForm.richTextBoxLog.Text = text + Environment.NewLine + Program.MyForm.richTextBoxLog.Text;
                   });
            else
            {
                Program.MyForm.richTextBoxLog.Text = text + Environment.NewLine + Program.MyForm.richTextBoxLog.Text;
            }
        }

        public static void WriteLog(Exception ex)
        {
            Logger.Log.Error(ex);
            WriteLog($"Ошибка в {ex.TargetSite.Name}: {ex.Message.Substring(0, ex.Message.Length > 40 ? 40 : ex.Message.Length)}", false);
        }

        /// <summary>
        /// Загрузить данные по акциям
        /// </summary>
        /// <param name="lst">Список акций</param>
        /// <param name="lbl">Лейбл с формы</param>
        /// <param name="bar">Прогресс-бар</param>
        /// <returns></returns>
        public static async Task LoadStocksData(List<Stock> lst, Label lbl, ProgressBar bar)
        {
            s_report = "Не удалось загрузить акции:;";
            int count = lst.Count, doneEvents = 0;
            Task[] tasks = new Task[count];

            Stopwatch stwatch = Stopwatch.StartNew();

            for (int i = 0; i < lst.Count; i++)
            {
                var i1 = i;
                tasks[i] = Task.Run(async () =>
                {
                    await GetStockData(lst[i1]);
                    Interlocked.Increment(ref doneEvents);
                    if (i1 % 10 != 0)
                        return;

                    double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)doneEvents / count) - 1) / 60.0;
                    mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;
                    lbl.BeginInvoke((MethodInvoker)delegate
                    {
                        lbl.Text =
                            $@"Обработано {doneEvents} / {count}. Расчетное время: {
                                    (mins >= 1 ? Math.Floor(mins) + " мин " : "")
                                }{Math.Floor((mins - Math.Floor(mins)) * 100)} с";
                    });
                    bar.BeginInvoke((MethodInvoker)delegate { bar.Value = doneEvents * 100 / count; });
                });
            }
            await Task.WhenAll(tasks);

            stwatch.Stop();
            bar.Value = 100;
            lbl.Text = @"Готово.";
        }

        /// <summary>
        /// Преобразовать в короткую/красивую строку
        /// </summary>
        /// <param name="doub">Число</param>
        /// <returns>Строку</returns>
        public static string ToCuteStr(this double? doub)
        {
            if (!doub.HasValue || doub.Value < -Tolerance)
                return "";
            double num = doub.Value;
            string str = "";
            if (Math.Abs(num) > 1000.0 * 1000 * 1000 * 1000) // Триллион
                str = (num / 1000 / 1000 / 1000).ToString("F2") + " T";
            else if (Math.Abs(num) > 1000 * 1000 * 1000) // Миллиард
                str = (num / 1000 / 1000 / 1000).ToString("F2") + " B";
            else if (Math.Abs(num) > 1000 * 1000) // Миллион
                str = (num / 1000 / 1000).ToString("F2") + " M";
            else if (Math.Abs(num) > Tolerance)
                str = num.ToString("F2");
            return str;
        }

        /// <summary>
        /// Получить данные по акции из интернета
        /// </summary>
        /// <param name="st">Акция</param>
        public static async Task GetStockData(Stock st)
        {
            string stockName = st.Name, htmlCode = "";
            st = GetStock(false, st.Name);
            if (st == null)
            {
                WriteLog($"Не удалось найти акцию в getStockData: {stockName}");
                return;
            }

            try
            {
                if (st.Market.Location == StockMarketLocation.Usa)
                {
                    htmlCode = await Web.Get(string.Format(Web.GetStockDataUrlUsa, st.Symbol));
                    if (htmlCode.IndexOf(">Trailing P/E</span>", StringComparison.Ordinal) < 0)
                    {
                        Logger.Log.Warn($"На сайте (USA) нет данных для {st.Symbol}");
                        return;
                    }

                    foreach (var coef in Coefficient.CoefficientList)
                    {
                        if (coef.IsUSA || coef.IsCommon)
                        {
                            if (!string.IsNullOrEmpty(coef.SearchInHTML_USA))
                                st[coef] = GettingYahooData(coef.SearchInHTML_USA, ref htmlCode);
                            else
                                st[coef] = coef.CalculateCoef(st.CoefficientsValues);
                        }
                    }

                    st.CalculateCoefs();
                    //if (Math.Abs(st["Ebitda"] ?? 0) > Tolerance)
                    //	st["DebtToEbitda"] = st["TotalDebt"] / st["Ebitda"];

                    st.LastUpdate = DateTime.Now;
                }
                else if (st.Market.Location == StockMarketLocation.Russia)
                {
                    if (!NamesToSymbolsRus.ContainsKey(st.Name))
                    {
                        WriteLog($"Нет ссылки для получения инфы для {st}");
                        return;
                    }

                    lock (s_rusStockLoaderLocker)
                    {
                        htmlCode = Web.Get(Web.GetStockDataUrlRussia + NamesToSymbolsRus[st.Name]).Result;
                        if (htmlCode.IndexOf("<span class=\"\">Коэффициент цена/прибыль", StringComparison.Ordinal) < 0)
                        {
                            Logger.Log.Warn($"На сайте (RUS) нет данных для {st.Symbol}");
                            return;
                        }

                        foreach (var coef in Coefficient.CoefficientList)
                        {
                            if (coef.IsCommon || coef.IsRus)
                                st[coef] = GettingInvestingComData(coef.SearchInHTML_Rus, ref htmlCode, coef.SearchInHTMLAppendix_Rus);
                        }

                        st.LastUpdate = DateTime.Now;
                    }
                }
            }
            catch (Exception er)
            {
                Logger.Log.Error(
                    $"Не удалось получить инфу по {st.Name}: {er.Message}\r\n{htmlCode}\r\n{er.StackTrace}");
                s_report += st.Name + ';';
            }

        }

        public static async void Initialize()
        {
            LoadStockListFromFile();
            try
            {
                await StockMarket.InitializeCurrencies();
                WriteLog("USD: " + StockMarket.GetExchangeRates(StockMarketCurrency.Usd).ToString("F2") +
                         "\r\nEUR: " + StockMarket.GetExchangeRates(StockMarketCurrency.Eur).ToString("F2"));
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            FillDict();
        }

        /// <summary>
        /// Загрузить список всех акций
        /// </summary>
        public static async Task GetStocksList(Label lbl, ProgressBar bar, bool loadAllStocksAgain = true)
        {
            if (loadAllStocksAgain)
            {
                var getRus = Task.Run(GetRussianStocks);
                var getUsa = Task.Run((Action)GetUsaStocks);

                await Task.WhenAll(getRus, getUsa);

                CheckForRepeatsAndSort();
            }

            int count = Stocks.Count;
            s_doneEventsCount = 0;

            Stopwatch stwatch = Stopwatch.StartNew();
            var tinkoffCheck = Task.Run(async () => await CheckAllForTinkoff(count));

            while (true)
            {
                double mins = stwatch.Elapsed.TotalSeconds * (1.0 / ((double)s_doneEventsCount / count) - 1) / 60.0;
                mins = Math.Floor(mins) + (mins - Math.Floor(mins)) * 0.6;

                lbl.BeginInvoke((MethodInvoker)delegate
                {
                    lbl.Text =
                        $@"Обработано {s_doneEventsCount} / {count}. Расчетное время: {
                                (mins >= 1 ? Math.Floor(mins) + " мин " : "")
                            }{Math.Floor((mins - Math.Floor(mins)) * 100)} с";
                });
                bar.BeginInvoke((MethodInvoker)delegate { bar.Value = s_doneEventsCount * 100 / count; });
                if (tinkoffCheck.IsCompleted)
                {
                    break;
                }
                await Task.Delay(5 * 1000);
                if (tinkoffCheck.IsCanceled || tinkoffCheck.IsFaulted)
                    break;
            }
        }


        #endregion

        #region Methods:private

        private static void CheckForRepeatsAndSort()
        {
            var temp = Stocks;
            Stocks = new List<Stock>(temp.Count / 3);
            foreach (var st in temp)
            {
                if (GetStock(false, st.Name) == null)
                    Stocks.Add(st);
            }
            //Отсортируем по алфавиту
            Stocks.Sort((s1, s2) => string.CompareOrdinal(s1.Name, s2.Name));
        }

        private static async Task CheckAllForTinkoff(int count)
        {
            List<Task> tinkoffTask = new List<Task>(3500);
            foreach (var stock in Stocks)
            {
                tinkoffTask.Add(stock.UnderstandIsItOnTinkoff());
            }

            while (s_doneEventsCount < count)
            {
                var task = await Task.WhenAny(tinkoffTask);
                tinkoffTask.Remove(task);
                Interlocked.Increment(ref s_doneEventsCount);
            }

        }

        /// <summary>
        /// Загрузить в Stocks акции с рус. биржы
        /// </summary>
        private static async Task GetRussianStocks()
        {
            int start = 0, end = 100;

            string htmlCode = await Web.Get(string.Format(Web.GetStocksListUrlRussia, start, end));
            JObject resp = JObject.Parse(htmlCode);
            end = resp["payload"]?["total"]?.Value<int>() + 1 ?? -1;
            if (end < 0)
                throw new Exception("Неверный ответ в GetRussianStocks");
            ExecuteTinkoffResponse(resp);

            int taskCount = (int)Math.Ceiling((double)(end - 100) / 100);
            Task[] tasks = new Task[taskCount];
            for (var i = 0; i < taskCount; i++)
            {
                var i1 = i;
                tasks[i] = Task.Run(async () =>
               {
                   start = (i1 + 1) * 100;
                   htmlCode = await Web.Get(string.Format(Web.GetStocksListUrlRussia, start, (i1 + 2) * 100 < end ? (i1 + 2) * 100 : end));

                   ExecuteTinkoffResponse(JObject.Parse(htmlCode));
               });
            }
            await Task.WhenAll(tasks);

        }

        private static void ExecuteTinkoffResponse(JObject resp)
        {
            foreach (var st in (JArray)resp["payload"]["values"])
            {
                var currencyToken = st["prices"]["buy"] ?? st["prices"]["close"] ?? st["prices"]["last"];
                if (currencyToken == null)
                {
                    Logger.Log.Warn($"Cannot resolve prices in {nameof(ExecuteTinkoffResponse)} for {nameof(st)}={st}");
                    continue;
                }
                if (currencyToken["currency"].Value<string>() != "RUB")
                    continue;

                var newStock = new Stock(st["symbol"]["showName"].Value<string>(), currencyToken["value"].Value<double>(),
                    new StockMarket(StockMarketLocation.Russia, StockMarketCurrency.Rub));
                Stocks.Add(newStock);
            }
        }

        /// <summary>
        /// Загрузить в Stocks акции с амер. биржы
        /// </summary>
        private static void GetUsaStocks()
        {
            string[] htmlCode =
                (File.ReadAllText(@"C:/temp/companylist.csv") + File.ReadAllText(@"C:/temp/companylist1.csv"))
                .Replace("\",\"", "|")
                .Replace("\"", "")
                .Split('\n');
            // Данные код перестал работать, т.к. файл не удается скачать(((

            //(Web.ReadDownloadedFile(Web.GetStocksListUrlUsaNasdaq) +
            // Web.ReadDownloadedFile(Web.GetStocksListUrlUsaNyse)).Replace("\"", "")
            //    .Replace("\",\"", "|")
            //    .Split('\n');

            foreach (var s in htmlCode)
            {
                if (s.StartsWith("Symbol"))
                    continue;
                var parameters = s.Split('|');
                if (parameters.Length < 2)
                    continue;
                string name = parameters[1], symb = parameters[0];
                double? price = parameters[2].ParseCoefStrToDouble();
                if (price.HasValue && price.Value > 0)
                {
                    var newStock = new Stock(name, price.Value, new StockMarket(StockMarketLocation.Usa, StockMarketCurrency.Usd), symb);
                    Stocks.Add(newStock);
                }
            }
        }


        /// <summary>
        /// Получает значение из разметки html сайта yahoo
        /// </summary>
        /// <param name="multiplicator">Название мультипликатора</param>
        /// <param name="htmlCode">Код</param>
        /// <returns>Значение</returns>
        private static double? GettingYahooData(string multiplicator, ref string htmlCode)
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
        private static double? GettingInvestingComData(string multiplicator, ref string htmlCode, string appendix = "")
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
            using (var fs = new FileStream($"{Const.SettingsDirName}/NamesToSymbols.csv", FileMode.Open))
            {
                using (var sr = new StreamReader(fs))
                {
                    var lines = sr.ReadToEnd().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        var splitted = line.Split(';');
                        if (splitted.Length < 2)
                            throw new NotSupportedException($"Wrong line {line}, fs.Position={fs.Position}");
                        var name = splitted[0];
                        var key = splitted[1];
                        NamesToSymbolsRus[name] = key;
                    }
                }
            }
        }

        #endregion
    }
}
