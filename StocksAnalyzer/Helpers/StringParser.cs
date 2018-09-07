using System;
using System.Globalization;
namespace StocksAnalyzer.Helpers
{
	static class StringParser
	{
		/// <summary>
		/// Например для sin(3.0) вернет 3.0
		/// </summary>
		/// <returns>Текстовое значение в скобках</returns>
		public static string GetNumInBracketsForFunction(string where, string functName)
		{
			var index = where.IndexOf(functName, StringComparison.Ordinal);
			string funct = where.Substring(index,
				where.IndexOf(")", index + 1, StringComparison.Ordinal) - index + 1);
			var scobeIndex = funct.IndexOf("(", StringComparison.Ordinal) + 1;
			return funct.Substring(scobeIndex,
				funct.IndexOf(")", StringComparison.Ordinal) - scobeIndex);
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
												  stringValue.IndexOf(".", StringComparison.Ordinal) ? "." : ",", "");
			}

			stringValue = stringValue.Replace(",", ".");
			if (Double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
				return result * coefficient;
			if (stringValue == "n/a" || stringValue == "-" || stringValue == "N/A" || stringValue == "")
				return null;
			throw new Exception($"Не удается распарсить строку {stringValue}");
		}

		public static string ToCuteStr(this double? num)
		{
			if (!num.HasValue)
				return "";
			return num.Value.ToCuteStr();
		}

		/// <summary>
		/// Преобразовать в короткую/красивую строку
		/// </summary>
		/// <param name="num">Число</param>
		/// <returns>Строку</returns>
		public static string ToCuteStr(this double num)
		{
			string str = "";
			if (Math.Abs(num) > 1000.0 * 1000 * 1000 * 1000) // Триллион
				str = (num / 1000 / 1000 / 1000).ToString("F2") + " T";
			else if (Math.Abs(num) > 1000 * 1000 * 1000) // Миллиард
				str = (num / 1000 / 1000 / 1000).ToString("F2") + " B";
			else if (Math.Abs(num) > 1000 * 1000) // Миллион
				str = (num / 1000 / 1000).ToString("F2") + " M";
			else if (Math.Abs(num) > Const.Tolerance)
				str = num.ToString("F2");
			return str;
		}
	}
}
