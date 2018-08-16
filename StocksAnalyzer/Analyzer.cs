using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace StocksAnalyzer
{
    class Rating
    {
        public int LocalRating { get; set; }
        public int AllStocksRating { get; set; }
        /// <summary>
        /// Среднее положение среди всех акций
        /// </summary>
        public int AverageNumber { get; set; }

    }
    static class Analyzer
    {
        /// <summary>
        /// TODO
        /// </summary>
        public static Dictionary<string, Dictionary<Stock, Rating>> Rating = new Dictionary<string, Dictionary<Stock, Rating>>();

        #region Funcstions:public

        /// <summary>
        /// Анализирует показатели акций, превращая их в одно число, используя метрики
        /// </summary>
        /// <param name="list">Список акций</param>
        /// <param name="label">Для отображения прогресса</param>
        public static void Analyze(List<Stock> list, Label label)
        {
            StreamWriter sWrite = new StreamWriter($"Analyzed_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv", true, Encoding.UTF8);
            string data = "Название акции;Market;";
            using (var streamCoefs = new StreamReader("analystSettings.csv"))
            {
                string[] coefsName = streamCoefs.ReadLine()?.Split(';');
                List<string> coefs = new List<string>();
                Dictionary<string, Dictionary<string, double>> funcs =
                    new Dictionary<string, Dictionary<string, double>>();

                Debug.Assert(coefsName != null, "coefsName == null");
                foreach (var coef in coefsName)
                    if (coef != "")
                        coefs.Add(coef);
                while (!streamCoefs.EndOfStream)
                {
                    coefsName = streamCoefs.ReadLine()?.Split(';');
                    Debug.Assert(coefsName != null, "coefsName == null");
                    if (coefsName.Length == 0 || coefsName[0] == "")
                        continue;
                    funcs.Add(coefsName[0], new Dictionary<string, double>());
                    Dictionary<string, double> coefVals;
                    funcs.TryGetValue(coefsName[0], out coefVals);
                    for (var i = 1; i < coefsName.Length; i++)
                        if (Math.Abs(coefsName[i].ParseCoefStrToDouble()) > MainClass.Tolerance)
                            coefVals?.Add(coefs[i - 1], coefsName[i].ParseCoefStrToDouble());
                }
                foreach (var str in funcs.Keys)
                    data += str + ';';
                data += "1;";
                foreach (var str in coefs)
                    data += str + ';';
                sWrite.WriteLine(data);
                data = "";

                int counter = 0, length = list.Count;
                foreach (Stock st in list)
                {
                    Dictionary<string, double> values = new Dictionary<string, double>();
                    AddAllStockData(values, st);
                    data += st.Name.Replace(';', '.') + ';' + st.Market.Location.ToString() + ';';
                    foreach (string func in funcs.Keys)
                    {
                        double res = 0;
                        if ((st.Market.Location == StockMarketLocation.Russia && func.Contains("USA")) ||
                            (st.Market.Location == StockMarketLocation.Usa && func.Contains("Rus")))
                        {
                            data += ';';
                            continue;
                        }
                        foreach (string param in funcs[func].Keys)
                        {
                            res += values[param] * funcs[func][param];
                        }
                        if (func == "MainPE")
                            st.MainPe = res;
                        if (func == "Main")
                            st.Main = res;
                        if (func == "MainAll")
                            st.MainAll = res;
                        data += res.ToString(CultureInfo.InvariantCulture) + ';';
                    }
                    data += ';';
                    foreach (var param in values.Keys)
                        data += values[param].ToString(CultureInfo.InvariantCulture) + ';';
                    sWrite.WriteLine(data);
                    data = "";
                    counter++;
                    var executed = counter;
                    label.BeginInvoke((MethodInvoker) (delegate
                    {
                        label.Text = $@"Обработано {executed} / {length}.";
                    }));
                }

                sWrite.Write(data);
                sWrite.Close();
                SetRatings(list);
            }
        }

        #endregion

        #region Functions:private

        /// <summary>
        /// Вычисляет число в заданной степени, если число меньше 0, вернет отрицательный число
        /// </summary>
        /// <param name="d">Число, от которого взять корень</param>
        /// <param name="pow">Степень</param>
        /// <returns>Итоговое число</returns>
        private static double SignedSqr(double d, double pow = 0.5)
        {
            return d >= 0 ? Math.Pow(d, pow) : -Math.Pow(-d, pow);
        }

        private static double Pe(this double coef, bool b = false)
        {
            if (Math.Abs(coef) < MainClass.Tolerance)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / SignedSqr(coef);
        }
        private static double Peg(this double coef, bool b = false)
        {
            if (Math.Abs(coef) < MainClass.Tolerance)
                return 0;
            if (b)
                return 5.0 / coef;
            return 1.0 / SignedSqr(coef, 1.0 / 3);
        }
        private static double EvEbitda(this double coef, bool b = false)
        {
            if (Math.Abs(coef) < MainClass.Tolerance)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / SignedSqr(coef);
        }
        private static double RetOnAss(this double coef)
        {
            return SignedSqr(coef / 500);
        }
        private static double BVpShare(this double coef)
        {
            return SignedSqr(coef / 300);
        }
        private static double LvpMc(this double coef)
        {
            return SignedSqr(coef * 5);
        }
        private static double Eps(this double coef)
        {
            return SignedSqr(coef) / 2;
        }
        private static double Roe(this double coef)
        {
            return SignedSqr(coef / 50);
        }
        private static double UrgLiq(this double coef)
        {
            return Math.Abs(coef) < MainClass.Tolerance ? 0 : 1 - (Math.Abs(coef - 1.9));
        }
        private static double CurrLiq(this double coef)
        {

            return Math.Abs(coef) < MainClass.Tolerance ? 0 : 1 - (Math.Abs(coef - 2.5));
        }
        private static double DebtEbitda(this double coef, bool b = false)
        {
            if (Math.Abs(coef) < MainClass.Tolerance)
                return 0;
            if (b)
                return 5.0 / coef;
            return 0.8 / SignedSqr(coef);
        }
        private static double MarketCap(this double coef)
        {
            return Math.Abs(coef) < MainClass.Tolerance ? 0 : coef < 5000 * 1000 ? -1 : coef < 1000 * 1000 * 1000 ? 0 : 1;
        }
        private static double Ps(this double coef)
        {
            return Math.Abs(coef) < MainClass.Tolerance ? 0 : coef < 0 ? SignedSqr(coef) : coef > 1 ? 1 - SignedSqr(coef) : 1 - coef * coef;
        }
        private static double Pbv(this double coef)
        {
            return Math.Abs(coef) < MainClass.Tolerance ? 0 : coef < 0 ? SignedSqr(coef) : coef > 1 ? 1 - SignedSqr(coef) : 1 - coef * coef;
        }
        private static double SqrFromPercent(this double coef)
        {
            return SignedSqr(coef / 100);
        }
        private static void AddAllStockData(Dictionary<string, double> values, Stock st)
        {
            values.Add("PE", st.PriceToEquity.Pe());
            values.Add("PS", st.PriceToSales.Ps());
            values.Add("PBV", st.PriceToBook.Pbv());
            values.Add("ROE", st.Roe.Roe());
            values.Add("EPS", st.Eps.Eps());
            values.Add("QEG", st.Qeg.SqrFromPercent());
            values.Add("PRM", st.ProfitMarg.SqrFromPercent());
            values.Add("OPM", st.OperMarg.SqrFromPercent());
            if (st.Market.Location == StockMarketLocation.Usa)
                values.Add("GRP", (Math.Abs(st.MarketCap) < MainClass.Tolerance ? 0 : st.ProfitMarg / st.MarketCap).SqrFromPercent());
            else if (st.Market.Location == StockMarketLocation.Russia)
                values.Add("GRP", st.GrossProfit.SqrFromPercent());

            values.Add("GRP5", st.GrossProfit5Ya.SqrFromPercent());
            values.Add("PRM5", st.ProfitMarg5Ya.SqrFromPercent());
            values.Add("OPM5", st.OperMarg5Ya.SqrFromPercent());
            values.Add("PRC", st.ProfitCoef.SqrFromPercent());
            values.Add("PRC5", st.ProfitCoef5Ya.SqrFromPercent());
            values.Add("PRpS", st.ProfitOn12MToAnalogYearAgo.SqrFromPercent());
            values.Add("PGS", st.GrowProfitPerShare5Y.SqrFromPercent());
            values.Add("GCC", st.CapExpenseGrow5Y.SqrFromPercent());
            values.Add("URL", st.UrgentLiquidityCoef.UrgLiq());
            values.Add("CUL", st.CurrentLiquidityCoef.CurrLiq());

            values.Add("EVEB", st.EVtoEbitda.EvEbitda());
            values.Add("DBEB", st.DebtToEbitda.DebtEbitda());
            values.Add("MC", st.MarketCap.MarketCap());
            values.Add("PEG", st.Peg.Peg());
            values.Add("RoA", st.RetOnAssets.RetOnAss());
            values.Add("BVpS", st.BookValPerShare.BVpShare());
            values.Add("LFMC", (Math.Abs(st.MarketCap) < MainClass.Tolerance ? 0 : st.LeveredFreeCashFlow / st.MarketCap).LvpMc());

        }

        private static void SetRatings(List<Stock> list)
        {
            foreach (var stock in list)
            {
                var rate = 1;
                foreach (var anotherStock in list)
                    if (stock.MainPe < anotherStock.MainPe)
                        rate++;
                stock.RateMainPe = rate;
                rate = 1;
                foreach (var anotherStock in list)
                    if (stock.Main < anotherStock.Main)
                        rate++;
                stock.RateMain = rate;
                rate = 1;
                foreach (var anotherStock in list)
                    if (stock.MainAll < anotherStock.MainAll)
                        rate++;
                stock.RateMainAll = rate;
            }
        }
        #endregion
    }
}
