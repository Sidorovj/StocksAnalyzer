using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using StockCore.Interfaces;
using StockCore.Stock;

namespace StocksAnalyzer
{
	internal static class Analyzer
	{
		private static readonly int s_fromPercent = 5;
		private static readonly int s_toPercent = 100 - s_fromPercent;

		private static string OutputFileName
		{
			get
			{
				if (!Directory.Exists(Const.AnalysisDirName))
					Directory.CreateDirectory(Const.AnalysisDirName);
				return $"{Const.AnalysisDirName}/Analyzed_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";
			}
		}

		#region Main function

		/// <summary>
		/// Анализирует показатели акций, превращая их в одно число, используя метрики
		/// </summary>
		/// <param name="stList">Список акций</param>
		public static void Analyze(StockList stList)
		{
			var list = stList.StList.ToList();
			foreach (var st in list)
			{
				foreach (var coef in Coefficient.CoefficientList)
				{
					st.NormalizedCoefficientsValues[coef] = coef.Normalize(st[coef]);
				}
			}

			Dictionary<Coefficient, double> leftBorders = new Dictionary<Coefficient, double>(Coefficient.CoefficientList.Count);
			Dictionary<Coefficient, double> rightBorders = new Dictionary<Coefficient, double>(Coefficient.CoefficientList.Count);
			foreach (var coef in Coefficient.CoefficientList)
			{
				var borders = TakeNthElemValue(list, coef);
				leftBorders[coef] = borders.Item1;
				rightBorders[coef] = borders.Item2;
			}

			string data = "Название акции;Market;";
			using (var sWrite = new StreamWriter(OutputFileName, true, Encoding.UTF8))
			{
				// Write the metrics names
				foreach (var str in Coefficient.MetricsList)
					data += str.ToString() + ';';
				data += ";";
				foreach (var str in Coefficient.CoefficientList.Select(c => c.Name))
					data += str + ';';
				sWrite.WriteLine(data);

				foreach (Stock st in list)
				{

					data = $"{st.Name.Replace(';', '.')};{st.Market.Location.ToString()};";

					foreach (var metric in Coefficient.MetricsList)
					{
						st.MetricsValues[metric] = 0;
						foreach (var coef in Coefficient.CoefficientList)
						{
							var compactVal = Compact(st.NormalizedCoefficientsValues[coef], leftBorders[coef],
								rightBorders[coef]);
							st.MetricsValues[metric] += Coefficient.SignedSqr(compactVal) *
														coef.MetricWeight[metric];
						}
						data += st.MetricsValues[metric].ToString(CultureInfo.InvariantCulture) + ';';
					}

					data += ';';
					foreach (var param in st.NormalizedCoefficientsValues.Keys)
						data += st.NormalizedCoefficientsValues[param].ToString() + ';';
					sWrite.WriteLine(data);
				}
			}

			SetRatings(stList, list);
		}

		#endregion

		#region Compacting functions

		private static double Compact(double? value, double leftBorder, double rightBorder)
		{
			if (value == null)
				return 0;
			if (Math.Abs(rightBorder - leftBorder) < Const.Tolerance)
				return value.Value;
			return 2 * (value.Value - leftBorder) / (rightBorder - leftBorder) - 1;
		}

		[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
		private static (double, double) TakeNthElemValue(List<Stock> list, Coefficient coef)
		{
			var query = (from st in list
						 let norm = st.NormalizedCoefficientsValues[coef]
						 where norm != null
						 orderby norm.Value
						 select norm.Value).ToList();
			if (query.Count == 0)
				return (0, 0);
			return (query[(int)Math.Round((double)s_fromPercent * query.Count / 100)], query.Take((int)Math.Round(
				(double)s_toPercent * query.Count / 100)).Last());
		}

		#endregion

		#region Ratings and positions

		private static void SetRatings(StockList stockList, List<Stock> list)
		{

			foreach (var coef in Coefficient.CoefficientList)
			{
				Stock.CoefHasValueCount[coef] = 0;
			}

			foreach (var stock in list)
			{
				if (stock.ListToRatings.ContainsKey(stockList))
					stock.ListToRatings[stockList].Clear();
				else
					stock.ListToRatings[stockList] = new Dictionary<IFactor, int?>();
				foreach (var metric in stock.MetricsValues)
				{
					var rate = 0;
					foreach (var anotherStock in list)
						if (metric.Value <= anotherStock.MetricsValues[metric.Key])
							rate++;
					stock.ListToRatings[stockList].Add(metric.Key, rate);
				}
				foreach (var coef in stock.NormalizedCoefficientsValues)
				{
					int? rate = 0;
					if (coef.Value == null)
					{
						rate = null;
					}
					else
					{
						Stock.CoefHasValueCount[coef.Key]++;
						foreach (var anotherStock in list)
							if (anotherStock.NormalizedCoefficientsValues[coef.Key].HasValue &&
								coef.Value <= anotherStock.NormalizedCoefficientsValues[coef.Key])
							{
								rate++;
							}
					}

					stock.ListToRatings[stockList].Add(coef.Key, rate);
				}

				SetAveragePosition(stockList, stock, list.Count);
			}
		}

		private static void SetAveragePosition(StockList stockList, Stock st, int maxVal)
		{
			int sumAll = 0, totalAll = 0;
			int sumMetr = 0, totalMetr = 0;
			int sumCoefs = 0, totalCoefs = 0;
			foreach (var rating in st.ListToRatings[stockList])
			{
				if (rating.Value != null)
				{
					sumAll += rating.Value.Value;
					totalAll++;
					if (Coefficient.MetricsList.Contains(rating.Key))
					{
						sumMetr += rating.Value.Value;
						totalMetr++;
					}
					else
					{
						sumCoefs += rating.Value.Value;
						totalCoefs++;
					}
				}
			}

			if (totalAll == 0)
				st.AveragePositionAll = maxVal;
			else
				st.AveragePositionAll = (double)sumAll / totalAll;
			if (totalMetr == 0)
				st.AveragePositionMetric = maxVal;
			else
				st.AveragePositionMetric = (double)sumMetr / totalMetr;
			if (totalCoefs == 0)
				st.AveragePositionNormalizedCoefs = maxVal;
			else
				st.AveragePositionNormalizedCoefs = (double)sumCoefs / totalCoefs;
		}
		#endregion
	}
}
