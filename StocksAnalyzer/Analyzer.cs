using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace StocksAnalyzer
{
	static class Analyzer
	{
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
		/// <param name="list">Список акций</param>
		public static void Analyze(List<Stock> list)
		{
			string data = "Название акции;Market;";
			using (var sWrite = new StreamWriter(OutputFileName, true, Encoding.UTF8))
			{
				// Write the metrics names
				foreach (var str in Coefficient.MetricsList)
					data += str + ';';
				data += ";";
				foreach (var str in Coefficient.CoefficientList.Select(c => c.Name))
					data += str + ';';
				sWrite.WriteLine(data);

				foreach (Stock st in list)
				{

					data = $"{st.Name.Replace(';', '.')};{st.Market.Location.ToString()};";

					foreach (var coef in Coefficient.CoefficientList)
					{
						st.NormalizedCoefficientsValues[coef] = coef.Normalize(st[coef]);
					}
					foreach (var metric in Coefficient.MetricsList)
					{
						st.MetricsValues[metric] = 0;
						foreach (var coef in Coefficient.CoefficientList)
						{
							st.MetricsValues[metric] += (st.NormalizedCoefficientsValues[coef] ?? 0) *
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

			SetRatings(list);
		}

		#endregion

		#region Ratings and positions

		private static void SetRatings(List<Stock> list)
		{
			foreach (var coef in Coefficient.CoefficientList)
			{

				Stock.CoefHasValueCount[coef] = 0;
			}

			foreach (var stock in list)
			{
				stock.PositionInMetricAndCoef.Clear();
				foreach (var metric in stock.MetricsValues)
				{
					var rate = 1;
					foreach (var anotherStock in list)
						if (metric.Value <= anotherStock.MetricsValues[metric.Key])
							rate++;
					stock.PositionInMetricAndCoef[metric.Key] = rate;
				}
				foreach (var coef in stock.NormalizedCoefficientsValues)
				{
					int? rate = 1;
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

					stock.PositionInMetricAndCoef[coef.Key.Name] = rate;
				}

				SetAveragePosition(stock, list.Count);
			}
		}

		private static void SetAveragePosition(Stock st, int maxVal)
		{
			int sumAll = 0, totalAll = 0;
			int sumMetr = 0, totalMetr = 0;
			int sumCoefs = 0, totalCoefs = 0;
			foreach (var metric in st.PositionInMetricAndCoef)
			{
				if (metric.Value.HasValue)
				{
					sumAll += metric.Value.Value;
					totalAll++;
					if (Coefficient.MetricsList.Contains(metric.Key))
					{
						sumMetr += metric.Value.Value;
						totalMetr++;
					}
					else
					{
						sumCoefs += metric.Value.Value;
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
