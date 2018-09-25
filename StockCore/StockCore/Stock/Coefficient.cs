using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using org.mariuszgromada.math.mxparser;
using StocksAnalyzer.Helpers;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer
{
	[Serializable]
	public sealed class Coefficient
	{
		/// <summary>
		/// Список всех коэффициентов из файла с настройками
		/// </summary>
		public static List<Coefficient> CoefficientList { get; } = new List<Coefficient>();
		/// <summary>
		/// Список всех метрик
		/// </summary>
		public static List<string> MetricsList { get; } = new List<string>();


		/// <summary>
		/// Значение веса коэффициентов для соответствующих метрик
		/// </summary>
		public Dictionary<string, double> MetricWeight { get; } = new Dictionary<string, double>();

		public string Name { get; }
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


		private string m_analyzerFormula;
		private string m_calculateFormula;

		private Coefficient(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public bool Equals(Coefficient coef)
		{
			return Name == coef.Name;
		}

		public override bool Equals(object coef)
		{
			if (coef is Coefficient coefficient)
			{
				return Equals(coefficient);
			}
			return false;
		}

		/// <summary>
		/// Рассчет значения коэффициента 
		/// </summary>
		/// <param name="coefValues">Словарь имя параметра-значение</param>
		/// <returns>Значение коэффициента</returns>
		public double? CalculateCoef(Dictionary<Coefficient, double?> coefValues)
		{
			var formula = m_calculateFormula;
			foreach (var coef in coefValues.Keys.Where(c => m_calculateFormula.Contains($"${{{c}}}")))
			{
				if (!coefValues[coef].HasValue)
					return null;
				formula = formula.Replace($"${{{coef}}}",
					coefValues[coef].Value.ToString(CultureInfo.InvariantCulture));
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
			if (!value.HasValue)
				return null;
			if (string.IsNullOrEmpty(m_analyzerFormula))
				return value;
			var formula = m_analyzerFormula.Replace("{val}", value.Value.ToString(CultureInfo.InvariantCulture));
			string customFunc = "ssqrt";
			while (formula.Contains(customFunc))
			{
				string valStr = StringParser.GetNumInBracketsForFunction(formula, customFunc);
				double? val = valStr.ParseCoefStrToDouble();
				formula = formula.Replace($"{customFunc}({valStr})", $"({SignedSqr(val ?? 0).ToString(CultureInfo.InvariantCulture)})");
			}
			return ParseAlgebraicFormula(formula);
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
				Logger.Log.Warn($"Formula is {formula}", ex);
				return null;
			}
		}

		/// <summary>
		/// Вычисляет число в заданной степени, если число меньше 0, вернет отрицательный число
		/// </summary>
		/// <param name="d">Число, от которого взять корень</param>
		/// <param name="pow">Степень</param>
		/// <returns>Итоговое число</returns>
		internal static double SignedSqr(double d, double pow = 0.5)
		{
			return d >= 0 ? Math.Pow(d, pow) : -Math.Pow(-d, pow);
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
			int nameIndex = columnNumToName.FirstOrDefault(c => c.Value.Item1 == "Name").Key;
			Coefficient coef = new Coefficient(data[nameIndex]);
			for (var i = 0; i < columnNumToName.Count; i++)
			{
				if (string.IsNullOrEmpty(columnNumToName[i].Item1))
					continue;

				string value = data[i];
				// if is metric
				if (columnNumToName[i].Item2)
				{
					coef.MetricWeight[columnNumToName[i].Item1] = value.ParseCoefStrToDouble() ?? 0;
				}
				else
				{
					switch (columnNumToName[i].Item1)
					{
						case "Name":
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