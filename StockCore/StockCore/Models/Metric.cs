using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Interfaces;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer.Data
{
	public sealed class Metric : IFactor
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Fill metric name")]
		public string Name { get; set; }
		public string Label { get; set; }
		public string Tooltip { get; set; }

		public ICollection<CoefWeightInMetric> CoefWeights { get; set; }

		/// <param name="input">Строка формата Name-Label-Tooltip</param>
		public Metric(string input)
		{
			var splitted = input.Split('-');
			if (splitted.Length != 3)
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