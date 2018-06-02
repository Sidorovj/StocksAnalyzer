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
        public int localRating { get; set; }
        public int allStocksRating { get; set; }
        /// <summary>
        /// Среднее положение среди всех акций
        /// </summary>
        public int averageNumber { get; set; }

    }
    static class Analyzer
    {
        /// <summary>
        /// TODO
        /// </summary>
        static public Dictionary<string, Dictionary<Stock, Rating>> rating = new Dictionary<string, Dictionary<Stock, Rating>>();

        #region Funcstions:public

        /// <summary>
        /// Анализирует показатели акций, превращая их в одно число, используя метрики
        /// </summary>
        /// <param name="list">Список акций</param>
        /// <param name="label">Для отображения прогресса</param>
        public static void Analyze(List<Stock> list, Label label)
        {
            StreamWriter _sWrite = new StreamWriter($"Analyzed_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.csv", true, Encoding.UTF8);
            string _data = "Название акции;Market;";
            StreamReader _streamCoefs = new StreamReader("analystSettings.csv");
            string[] _coefsName = _streamCoefs.ReadLine().Split(';');
            List<string> _coefs = new List<string>();
            Dictionary<string, double> _coefVals = new Dictionary<string, double>();
            Dictionary<string, Dictionary<string, double>> _funcs = new Dictionary<string, Dictionary<string, double>>();

            foreach (var _coef in _coefsName)
                if (_coef != "")
                    _coefs.Add(_coef);
            while (!_streamCoefs.EndOfStream)
            {
                _coefsName = _streamCoefs.ReadLine().Split(';');
                if (_coefsName.Length == 0 || _coefsName[0] == "")
                    continue;
                _funcs.Add(_coefsName[0], new Dictionary<string, double>());
                _funcs.TryGetValue(_coefsName[0], out _coefVals);
                for (var i = 1; i < _coefsName.Length; i++)
                    if (_coefsName[i].ParseCoefStrToDouble() != 0)
                        _coefVals.Add(_coefs[i - 1], _coefsName[i].ParseCoefStrToDouble());
            }
            foreach (var str in _funcs.Keys)
                _data += str + ';';
            _data += "1;";
            foreach (var str in _coefs)
                _data += str + ';';
            _sWrite.WriteLine(_data);
            _data = "";

            int counter = 0, length = list.Count;
            foreach (Stock st in list)
            {
                Dictionary<string, double> values = new Dictionary<string, double>();
                AddAllStockData(values, st);
                _data += st.Name.Replace(';', '.') + ';' + st.Market.Location.ToString() + ';';
                foreach (string func in _funcs.Keys)
                {
                    double res = 0;
                    if ((st.Market.Location == StockMarketLocation.Russia && func.Contains("USA")) || (st.Market.Location == StockMarketLocation.USA && func.Contains("Rus")))
                    {
                        _data += ';';
                        continue;
                    }
                    foreach (string param in _funcs[func].Keys)
                    {
                        res += values[param] * _funcs[func][param];
                    }
                    if (func == "MainPE")
                        st.MainPE = res;
                    if (func == "Main")
                        st.Main = res;
                    if (func == "MainAll")
                        st.MainAll = res;
                    _data += res.ToString() + ';';
                }
                _data += ';';
                foreach (var param in values.Keys)
                    _data += values[param].ToString() + ';';
                _sWrite.WriteLine(_data);
                _data = "";
                counter++;
                label.BeginInvoke((MethodInvoker)(delegate { label.Text = "Обработано " + counter.ToString() + " / " + length + "."; }));
            }

            _sWrite.Write(_data);
            _sWrite.Close();
            SetRatings(list);
        }

        #endregion

        #region Functions:private

        /// <summary>
        /// Вычисляет число в заданной степени, если число <0, вернет отрицательный число
        /// </summary>
        /// <param name="d">Число, от которого взять корень</param>
        /// <param name="pow">Степень</param>
        /// <returns>Итоговое число</returns>
        private static double SignedSqr(double d, double pow = 0.5)
        {
            return d >= 0 ? Math.Pow(d, pow) : -Math.Pow(-d, pow);
        }

        private static double PE(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / SignedSqr(coef);
        }
        private static double PEG(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 1.0 / SignedSqr(coef, 1.0 / 3);
        }
        private static double EVEbitda(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 2.0 / SignedSqr(coef);
        }
        private static double RetOnAss(this double coef, bool b = false)
        {
            return SignedSqr(coef / 500);
        }
        private static double BVpShare(this double coef, bool b = false)
        {
            return SignedSqr(coef / 300);
        }
        private static double LVpMC(this double coef, bool b = false)
        {
            return SignedSqr(coef * 5);
        }
        private static double EPS(this double coef, bool b = false)
        {
            return SignedSqr(coef) / 2;
        }
        private static double ROE(this double coef, bool b = false)
        {
            return SignedSqr(coef / 50);
        }
        private static double UrgLiq(this double coef, bool b = false)
        {
            return coef == 0 ? 0 : 1 - (Math.Abs(coef - 1.9));
        }
        private static double CurrLiq(this double coef, bool b = false)
        {

            return coef == 0 ? 0 : 1 - (Math.Abs(coef - 2.5));
        }
        private static double DebtEbitda(this double coef, bool b = false)
        {
            if (coef == 0)
                return 0;
            if (b)
                return 5.0 / coef;
            return 0.8 / SignedSqr(coef);
        }
        private static double MarketCap(this double coef, bool b = false)
        {
            return coef == 0 ? 0 : coef < 5000 * 1000 ? -1 : coef < 1000 * 1000 * 1000 ? 0 : 1;
        }
        private static double PS(this double coef)
        {
            return coef == 0 ? 0 : coef < 0 ? SignedSqr(coef) : coef > 1 ? 1 - SignedSqr(coef) : 1 - coef * coef;
        }
        private static double PBV(this double coef)
        {
            return coef == 0 ? 0 : coef < 0 ? SignedSqr(coef) : coef > 1 ? 1 - SignedSqr(coef) : 1 - coef * coef;
        }
        private static double SqrFromPercent(this double coef)
        {
            return SignedSqr(coef / 100);
        }
        private static void AddAllStockData(Dictionary<string, double> values, Stock st)
        {
            values.Add("PE", st.PriceToEquity.PE());
            values.Add("PS", st.PriceToSales.PS());
            values.Add("PBV", st.PriceToBook.PBV());
            values.Add("ROE", st.ROE.ROE());
            values.Add("EPS", st.EPS.EPS());
            values.Add("QEG", st.QEG.SqrFromPercent());
            values.Add("PRM", st.ProfitMarg.SqrFromPercent());
            values.Add("OPM", st.OperMarg.SqrFromPercent());
            if (st.Market.Location == StockMarketLocation.USA)
                values.Add("GRP", (st.MarketCap == 0 ? 0 : st.ProfitMarg / st.MarketCap).SqrFromPercent());
            else if (st.Market.Location == StockMarketLocation.Russia)
                values.Add("GRP", st.GrossProfit.SqrFromPercent());

            values.Add("GRP5", st.GrossProfit5ya.SqrFromPercent());
            values.Add("PRM5", st.ProfitMarg5ya.SqrFromPercent());
            values.Add("OPM5", st.OperMarg5ya.SqrFromPercent());
            values.Add("PRC", st.ProfitCoef.SqrFromPercent());
            values.Add("PRC5", st.ProfitCoef5ya.SqrFromPercent());
            values.Add("PRpS", st.ProfitOn12mToAnalogYearAgo.SqrFromPercent());
            values.Add("PGS", st.GrowProfitPerShare5y.SqrFromPercent());
            values.Add("GCC", st.CapExpenseGrow5y.SqrFromPercent());
            values.Add("URL", st.UrgentLiquidityCoef.UrgLiq());
            values.Add("CUL", st.CurrentLiquidityCoef.CurrLiq());

            values.Add("EVEB", st.EVtoEBITDA.EVEbitda());
            values.Add("DBEB", st.DebtToEBITDA.DebtEbitda());
            values.Add("MC", st.MarketCap.MarketCap());
            values.Add("PEG", st.PEG.PEG());
            values.Add("RoA", st.RetOnAssets.RetOnAss());
            values.Add("BVpS", st.BookValPerShare.BVpShare());
            values.Add("LFMC", (st.MarketCap == 0 ? 0 : st.LeveredFreeCashFlow / st.MarketCap).LVpMC());

        }

        private static void SetRatings(List<Stock> list)
        {
            foreach (var _stock in list)
            {
                var rate = 1;
                foreach (var _anotherStock in list)
                    if (_stock.MainPE < _anotherStock.MainPE)
                        rate++;
                _stock.RateMainPE = rate;
                rate = 1;
                foreach (var _anotherStock in list)
                    if (_stock.Main < _anotherStock.Main)
                        rate++;
                _stock.RateMain = rate;
                rate = 1;
                foreach (var _anotherStock in list)
                    if (_stock.MainAll < _anotherStock.MainAll)
                        rate++;
                _stock.RateMainAll = rate;
            }
        }
        #endregion
    }
}
