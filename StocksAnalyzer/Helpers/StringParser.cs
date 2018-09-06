using System;

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
	}
}
