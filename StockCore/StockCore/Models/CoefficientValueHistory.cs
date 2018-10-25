using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class CoefficientValueHistory
	{
		public int Id { get; set; }

		[ForeignKey("StockId")]
		public StocksAnalyzer.Data.Stock Stock { get; set; }

		[ForeignKey("CoefId")]
		public Coefficient Coefficient { get; set; }

		public double? Value { get; set; }

		public double? NormalizedValue { get; set; }
	}
}