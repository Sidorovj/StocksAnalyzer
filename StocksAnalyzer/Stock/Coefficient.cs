using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using org.mariuszgromada.math.mxparser;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer
{
	[Serializable]
	internal sealed class Coefficient
	{
		/// <summary>
		/// Список всех коэффициентов из файла с настройками
		/// </summary>
		public static List<Coefficient> CoefficientList { get; } = new List<Coefficient>();
		/// <summary>
		/// Список всех метрик
		/// </summary>
		public static List<string> MetricsList { get; } = new List<string>();


		public string Name { get; private set; }
		public string Label { get; private set; }
		public string Tooltip { get; private set; }

		/// <summary>
		/// Справочная инфа (какое значение лучше, какое хуже)
		/// </summary>
		public string HelpDescription { get; private set; }

		public string SearchInHTML_Rus { get; private set; }
		public string SearchInHTMLAppendix_Rus { get; private set; }
		public string SearchInHTML_USA { get; private set; }
		public bool IsCommon { get; private set; }
		public bool IsRus { get; private set; }
		public bool IsUSA { get; private set; }

		/// <summary>
		/// Значение коэффициентов для соответствующих метрик
		/// </summary>
		public Dictionary<string, double> MetricCoefs { get; } = new Dictionary<string, double>();



		private string m_analyzerFormula { get; set; }
		private string m_calculateFormula { get; set; }

		private Coefficient()
		{

		}

		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Рассчет значения коэффициента 
		/// </summary>
		/// <param name="coefValues">Словарь имя параметра-значение</param>
		/// <returns>Значение коэффициента</returns>
		public double? CalculateCoef(Dictionary<string, double?> coefValues)
		{
			var formula = m_calculateFormula;
			foreach (var coef in coefValues.Keys)
			{
				string searchStr = $"${{{coef}}}";
				if (m_calculateFormula.Contains(searchStr))
					formula = formula.Replace(searchStr, (coefValues[coef] ?? 0).ToString(CultureInfo.InvariantCulture));
			}
			return ParseAlgebraicFormula(formula);
		}

		/// <summary>
		/// Нормализует значение коэффициента согласно формуле из настроек
		/// </summary>
		/// <param name="value">Значение коэффициента</param>
		/// <returns>Нормализованное значение</returns>
		public double? Normalize(double? value)
		{
			// TODO: http://mathparser.org/mxparser-math-collection/unary-functions/
			// Заполнить соответствующие формулы в эксельнике
			var formula = m_analyzerFormula;
			return 0;
		}


		private double? ParseAlgebraicFormula(string formula)
		{
			try
			{
				Expression form = new Expression(formula);
				return form.calculate();
			}
			catch (DivideByZeroException ex)
			{
				Logger.Log.Warn(ex);
				return null;
			}
		}

		#region Static (loader)

		static Coefficient()
		{
			using (var reader = new StreamReader($"{Const.SettingsDirName}/{Const.CoefficientsSettings}"))
			{
				Dictionary<int, (string, bool)> columnNumToName = null;
				while (!reader.EndOfStream)
				{
					string[] data = reader.ReadLine()?.Split(';');
					if (data?[0] == "Name")
					{
						columnNumToName = new Dictionary<int, (string, bool)>(data.Length);
						bool isMetric = false;
						for (var i = 0; i < data.Length; i++)
						{
						    if (data[i] == "#Metrics")
						    {
						        isMetric = true;
						        columnNumToName.Add(i, ("", false));
                            }
						    else
							{
								columnNumToName.Add(i, (data[i], isMetric));
								if (isMetric)
									MetricsList.Add(data[i]);
							}
						}
					}
					else if (!string.IsNullOrEmpty(data?[0]))
					{
						CoefficientList.Add(ParseCoefficient(data, columnNumToName) ??
											throw new ArgumentNullException(
												$"Method {nameof(ParseCoefficient)} return null"));
					}
				}
			}
		}

		private static Coefficient ParseCoefficient(string[] data, Dictionary<int, (string, bool)> columnNumToName)
		{
			if (data == null || columnNumToName == null)
				throw new ArgumentNullException(data == null ? nameof(data) : nameof(columnNumToName));
			Coefficient coef = new Coefficient();
			for (var i = 0; i < columnNumToName.Count; i++)
			{
			    if (string.IsNullOrEmpty(columnNumToName[i].Item1))
			        continue;

			    string value = data[i];
				if (columnNumToName[i].Item2)
				{
					coef.MetricCoefs[columnNumToName[i].Item1] = value.ParseCoefStrToDouble() ?? 0;
				}
				else
				{
					switch (columnNumToName[i].Item1)
					{
						case "Name":
							coef.Name = value;
							break;
						case "Label":
							coef.Label = value;
							break;
						case "Tooltip":
							coef.Tooltip = value;
							break;
						case "SearchInHTML_Rus":
							coef.SearchInHTML_Rus = value;
							break;
						case "SearchInHTMLAppendix_Rus":
							coef.SearchInHTMLAppendix_Rus = value;
							break;
						case "SearchInHTML_USA":
							coef.SearchInHTML_USA = value;
							break;
						case "CalculateFormula":
							coef.m_calculateFormula = value;
							break;
						case "AnalyzerFormula":
							coef.m_analyzerFormula = value;
							break;
						case "HelpDescription":
							coef.HelpDescription = value;
							break;
						case "IsCommon":
							coef.IsCommon = value == "1";
							break;
						case "IsRus":
							coef.IsRus = value == "1";
							break;
						case "IsUSA":
							coef.IsUSA = value == "1";
							break;
						default:
							throw new NotSupportedException(
								$@"Ошибка при чтении файла коэффициентов. {columnNumToName[i]}");
					}
				}
			}

			return coef;
		}

		#endregion
	}
}