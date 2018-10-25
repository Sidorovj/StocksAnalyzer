using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class MetricValue
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("StockId")]
		public StocksAnalyzer.Data.Stock Stock { get; set; }

		[ForeignKey("CoefId")]
		public Metric Metric { get; set; }

		public double? Value { get; set; }
	}
}