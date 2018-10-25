using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockCore.Interfaces;
using StocksAnalyzer.Data;

namespace StocksAnalyzer
{
	internal class Analyzer : IStockAnalyze
	{
		private static readonly int s_fromPercent = 5;
		private static readonly int s_toPercent = 100 - s_fromPercent;
		
		#region Main function

		/// <summary>
		/// Анализирует показатели акций, превращая их в одно число, используя метрики
		/// </summary>
		/// <param name="listName">Название списка</param>
		/// <param name="stocks">Список акций</param>
		public void Analyze(StockListNamesEnum listName, IEnumerable<Data.Stock> stocks, DbSet<Data.Coefficient> coefsList)
		{
			var coefsCount = coefsList.Count();
			var stockList = stocks.ToList();
			Dictionary<Data.Coefficient, double> leftBorders = new Dictionary<Data.Coefficient, double>(coefsCount);
			Dictionary<Data.Coefficient, double> rightBorders = new Dictionary<Data.Coefficient, double>(coefsCount);
			foreach (var coef in coefsList)
			{
				var borders = TakeNthElemValue(stockList, coef);
				leftBorders[coef] = borders.Item1;
				rightBorders[coef] = borders.Item2;
			}
			
			foreach (var st in stockList)
			{

				foreach (var metric in CoefficientOld.MetricsList)
				{
					st.MetricsValues[metric] = 0;
					foreach (var coef in CoefficientOld.CoefficientList)
					{
						var compactVal = Compact(st.NormalizedCoefficientsValues[coef], leftBorders[coef],
							rightBorders[coef]);
						st.MetricsValues[metric] += CoefficientOld.SignedSqr(compactVal) *
													coef.MetricWeight[metric];
					}
				}
			}

			SetRatings(listName, stocks);
		}

		/// <summary>
		/// Analyze all lists
		/// </summary>
		public void AnalyzeAll(IEnumerable<StockList> stockLists)
		{
			IEnumerable<StockListToStock> sss;
			sss.First().
			foreach (var list in stockLists)
				Analyze(null,list);
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
		private static (double, double) TakeNthElemValue(IEnumerable<Data.Stock> stocks, Data.Coefficient coef)
		{
			var query = (from st in stocks
				let norm = st.CoefficientValues.First(val => val.Coefficient.Equals(coef)).NormalizedValue
				where norm != null
				orderby norm.Value
				select norm.Value).ToList();
			if (query.Count == 0)
				return (0, 0);
			return (query[(int) Math.Round((double) s_fromPercent*query.Count/100)], query.Take((int) Math.Round(
				(double) s_toPercent*query.Count/100)).Last());
		}

		#endregion

		#region Ratings and positions

		private static void SetRatings(StockListNamesEnum listName, List<StockOld> list)
		{
			foreach (var coef in CoefficientOld.CoefficientList)
			{
				StockOld.CoefHasValueCount[listName][coef] = 0;
			}

			Parallel.ForEach(list, stock =>
			{
				var newRatings = new Dictionary<IFactor, int?>();
				stock.ListToRatings[listName] = newRatings;
				foreach (var metric in stock.MetricsValues)
				{
					var rate = 0;
					foreach (var anotherStock in list)
						if (metric.Value <= anotherStock.MetricsValues[metric.Key])
							rate++;
					newRatings.Add(metric.Key, rate);
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
						StockOld.CoefHasValueCount[listName][coef.Key]++;
						foreach (var anotherStock in list)
							if (anotherStock.NormalizedCoefficientsValues[coef.Key].HasValue &&
								coef.Value <= anotherStock.NormalizedCoefficientsValues[coef.Key])
							{
								rate++;
							}
					}

					newRatings.Add(coef.Key, rate);
				}

				SetAveragePosition(listName, stock, list.Count);

			});
		}

		private static void SetAveragePosition(StockListNamesEnum listName, StockOld st, int maxVal)
		{
			int sumAll = 0, totalAll = 0;
			int sumMetr = 0, totalMetr = 0;
			int sumCoefs = 0, totalCoefs = 0;
			foreach (var rating in st.ListToRatings[listName])
			{
				if (rating.Value != null)
				{
					sumAll += rating.Value.Value;
					totalAll++;
					if (CoefficientOld.MetricsList.Contains(rating.Key))
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
				st.AveragePositionAll[listName] = maxVal;
			else
				st.AveragePositionAll[listName] = (double)sumAll / totalAll;
			if (totalMetr == 0)
				st.AveragePositionMetric[listName] = maxVal;
			else
				st.AveragePositionMetric[listName] = (double)sumMetr / totalMetr;
			if (totalCoefs == 0)
				st.AveragePositionNormalizedCoefs[listName] = maxVal;
			else
				st.AveragePositionNormalizedCoefs[listName] = (double)sumCoefs / totalCoefs;
		}
		#endregion
	}
}
