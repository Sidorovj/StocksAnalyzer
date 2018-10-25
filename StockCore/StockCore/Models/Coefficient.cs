using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using org.mariuszgromada.math.mxparser;
using StockCore.Interfaces;
using StocksAnalyzer.Helpers;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer.Data
{
	public sealed class Coefficient : IFactor
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Fill coef name")]
		public string Name { get; }
		public string Label { get; private set; }
		public string Tooltip { get; private set; }

		/// <summary>
		/// Справочная инфа (какое значение лучше, какое хуже)
		/// </summary>
		public string HelpDescription { get; private set; }


		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public string SearchInHTML_Rus { get; private set; }
		public string SearchInHTMLAppendix_Rus { get; private set; }
		public string SearchInHTML_USA { get; private set; }
		public bool IsCommon { get; private set; }
		public bool IsRus { get; private set; }
		public bool IsUSA { get; private set; }
		public string AnalyzerFormula { get; set; }
		public string CalculateFormula { get; set; }


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
			var formula = CalculateFormula;
			foreach (var coef in coefValues.Keys.Where(c => CalculateFormula.Contains($"${{{c}}}")))
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
			if (string.IsNullOrEmpty(AnalyzerFormula))
				return value;
			var formula = AnalyzerFormula.Replace("{val}", value.Value.ToString(CultureInfo.InvariantCulture));
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

	}
}