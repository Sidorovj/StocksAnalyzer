using System;
using StockCore.Interfaces;

namespace StockCore.Stock
{
	[Serializable]
	internal class MetricOld : IFactor
	{
		public string Name { get; }
		public string Label { get; }
		public string Tooltip { get; }

		/// <param name="input">Строка формата Name=Label=Tooltip</param>
		internal MetricOld(string input)
		{
			var splitted = input.Split('-');
			if (splitted.Length !=3)
				throw new ArgumentException($"Неверно заполнен файл настроек Coefficients, input={input}", nameof(input));
			Name = splitted[0];
			Label = splitted[1];
			Tooltip = splitted[2];
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (obj is MetricOld m)
				return Equals(m);
			return false;
		}

		public bool Equals(MetricOld m)
		{
			return m.Name == Name;
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
