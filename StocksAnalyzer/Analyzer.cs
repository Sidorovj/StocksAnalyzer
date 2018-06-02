using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StocksAnalyzer
{
    class Rating
    {
        public int rate { get; set; }
        public int rateAll { get; set; }
        public int avNum { get; set; }

    }
    static class Analyzer
    {
        delegate double func(Stock st);
        static public Dictionary<string, Dictionary<Stock, Rating>> rating = new Dictionary<string, Dictionary<Stock, Rating>>();
        private static double msqrt(double d, double pow=0.5)
        {
            return d >= 0 ? Math.Pow(d,pow) : -Math.Pow(-d,pow);
        }

        private static double PE(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / msqrt(coef);
        }
        private static double PEG(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 1.0 / msqrt(coef, 1.0/3);
        }
        private static double EVEbitda(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / msqrt(coef);
        }
        private static double RetOnAss(this double coef, bool b = false)
        {
            return msqrt(coef / 500);
        }
        private static double BVpShare(this double coef, bool b = false)
        {
            return msqrt(coef / 300);
        }
        private static double LVpMC(this double coef, bool b = false)
        {
            return msqrt(coef * 5);
        }
        private static double EPS(this double coef, bool b = false)
        {
            return msqrt(coef) / 2;
        }
        private static double ROE(this double coef, bool b = false)
        {
            return msqrt(coef / 50);
        }
        private static double UrgLiq(this double coef, bool b = false)
        {
            return coef == 0 ?0: 1-(Math.Abs(coef - 1.9));
        }
        private static double CurrLiq(this double coef, bool b = false)
        {

            return coef==0?0: 1-(Math.Abs(coef - 2.5));
        }
        private static double DebtEbitda(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 0.8 / msqrt(coef);
        }
        private static double MarketCap(this double coef, bool b = false)
        {
            return coef == 0 ? 0: coef < 5000000 ? -1 : coef < 1000000000 ? 0 : 1;
        }
        private static double PS(this double coef)
        {
            return coef == 0 ? 0: coef<0? msqrt(coef): coef > 1 ? 1 - msqrt(coef) : 1 - coef * coef;
        }
        private static double PBV(this double coef)
        {
            return coef==0?0: coef < 0 ? msqrt(coef ):  coef > 1 ? 1 - msqrt(coef) : 1 - coef * coef;
        }
        private static double fromPerc(this double coef)
        {
            return msqrt(coef / 100);
        }
        private static void addAllStockData(Dictionary<string, double> values, Stock st)
        {
            values.Add("PE", st.PriceToEquity.PE());
            values.Add("PS", st.PriceToSales.PS());
            values.Add("PBV", st.PriceToBook.PBV());
            values.Add("ROE", st.ROE.ROE());
            values.Add("EPS", st.EPS.EPS());
            values.Add("QEG", st.QEG.fromPerc());
            values.Add("PRM", st.ProfitMarg.fromPerc());
            values.Add("OPM", st.OperMarg.fromPerc());
            if (st.Market.Location == StockMarketLocation.USA)
                values.Add("GRP", (st.MarketCap==0?0: st.ProfitMarg / st.MarketCap).fromPerc());
            else if (st.Market.Location == StockMarketLocation.Russia)
                values.Add("GRP", st.GrossProfit.fromPerc());
            
            values.Add("GRP5", st.GrossProfit5ya.fromPerc());
            values.Add("PRM5", st.ProfitMarg5ya.fromPerc());
            values.Add("OPM5", st.OperMarg5ya.fromPerc());
            values.Add("PRC", st.ProfitCoef.fromPerc());
            values.Add("PRC5", st.ProfitCoef5ya.fromPerc());
            values.Add("PRpS", st.ProfitOn12mToAnalogYearAgo.fromPerc());
            values.Add("PGS", st.GrowProfitPerShare5y.fromPerc());
            values.Add("GCC", st.CapExpenseGrow5y.fromPerc());
            values.Add("URL", st.UrgentLiquidityCoef.UrgLiq());
            values.Add("CUL", st.CurrentLiquidityCoef.CurrLiq());

            values.Add("EVEB", st.EVtoEBITDA.EVEbitda());
            values.Add("DBEB", st.DebtToEBITDA.DebtEbitda());
            values.Add("MC", st.MarketCap.MarketCap());
            values.Add("PEG", st.PEG.PEG());
            values.Add("RoA", st.RetOnAssets.RetOnAss());
            values.Add("BVpS", st.BookValPerShare.BVpShare());
            values.Add("LFMC", (st.MarketCap==0?0:st.LeveredFreeCashFlow/st.MarketCap).LVpMC());

        }
        public static void analyze(List<Stock> list, Label lbl)
        {
            StreamWriter sr = new StreamWriter("Analyze_" + DateTime.Now.ToString().Replace('.', '-').Replace(' ', '_').Replace(':', '-') + ".csv", true, Encoding.UTF8);
            string data = "Название акции;Market;";
            StreamReader set = new StreamReader("analystSettings.csv");
            string[] s1 = set.ReadLine().Split(';');
            List<string> coefs = new List<string>();
            Dictionary<string, double> coefVals = new Dictionary<string, double>();
            Dictionary<string, Dictionary<string, double>> funcs = new Dictionary<string, Dictionary<string, double>>();
            foreach (var coef in s1)
                if (coef != "")
                    coefs.Add(coef);
            while (!set.EndOfStream)
            {
                s1 = set.ReadLine().Split(';');
                if (s1.Length == 0 || s1[0] == "")
                    continue;
                funcs.Add(s1[0], new Dictionary<string, double>());
                funcs.TryGetValue(s1[0], out coefVals);
                for (var i = 1; i < s1.Length; i++)
                    if (s1[i].getDoubleNum() != 0)
                        coefVals.Add(coefs[i - 1], s1[i].getDoubleNum());
            }
            foreach (var str in funcs.Keys)
                data += str + ';';
            data += "1;";
            foreach (var str in coefs)
                data += str + ';';
            sr.WriteLine(data);
            data ="";

            int counter = 0, length = list.Count;
            foreach (Stock st in list)
            {
                Dictionary<string, double> values = new Dictionary<string, double>();
                addAllStockData(values, st);
                data += st.Name.Replace(';','.') + ';' + st.Market.Location.ToString() + ';';
                foreach (string func in funcs.Keys)
                {
                    double res = 0;
                    if ((st.Market.Location == StockMarketLocation.Russia && func.Contains("USA")) || (st.Market.Location == StockMarketLocation.USA && func.Contains("Rus")))
                    {
                        data += ';';
                        continue;
                    }
                    foreach (string param in funcs[func].Keys)
                    {
                            res += values[param] * funcs[func][param];
                    }
                    if (func == "MainPE")
                        st.MainPE = res;
                    if (func == "Main")
                        st.Main = res;
                    if (func == "MainAll")
                        st.MainAll = res;
                    data += res.ToString() + ';';
                }
                data += ';';
                foreach (var param in values.Keys)
                    data += values[param].ToString() + ';';
                sr.WriteLine(data);
                data ="";
                counter++;
                lbl.BeginInvoke((MethodInvoker)(delegate { lbl.Text = "Обработано " + counter.ToString() + " / " + length + "."; }));
            }

            sr.Write(data);
            sr.Close();
            setRatings(list);
        }

        private static void setRatings(List<Stock> list)
        {
            foreach (var st in list)
            {
                var rate = 1;
                foreach (var st1 in list)
                    if (st.MainPE < st1.MainPE)
                        rate++;
                st.RateMainPE = rate;
                rate = 1;
                foreach (var st1 in list)
                    if (st.Main < st1.Main)
                        rate++;
                st.RateMain = rate;
                rate = 1;
                foreach (var st1 in list)
                    if (st.MainAll < st1.MainAll)
                        rate++;
                st.RateMainAll = rate;
            }
        }
    }
}
