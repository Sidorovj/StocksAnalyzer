using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class CoefficientValue
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("StockId")]
		public StocksAnalyzer.Data.Stock Stock { get; set; }

		[ForeignKey("CoefId")]
		public Coefficient Coefficient { get; set; }

		public double? Value
		{
			get => m_value;
			set
			{
				m_value = value;
				NormalizedValue = Coefficient.Normalize(value);
			}
		}

		public double? NormalizedValue { get; private set; }

		[NotMapped] private double? m_value;
	}
}