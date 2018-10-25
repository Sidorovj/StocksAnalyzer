using System;
using StockCore.Interfaces;

namespace StockCore.Stock
{
	[Serializable]
	public class Metric : IFactor
	{
		public string Name { get; }
		public string Label { get; }
		public string Tooltip { get; }

		/// <param name="input">Строка формата Name=Label=Tooltip</param>
		internal Metric(string input)
		{
			var splitted = input.Split('-');
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
			if (obj is Metric m)
				return Equals(m);
			return false;
		}

		public bool Equals(Metric m)
		{
			return m.Name == Name;
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
