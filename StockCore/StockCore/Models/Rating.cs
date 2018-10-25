using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class Rating
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int Position { get; set; }

		[ForeignKey("StockId")]
		public StocksAnalyzer.Data.Stock Stock { get; set; }

		[ForeignKey("MetricId")]
		public Metric Metric { get; set; }

		[ForeignKey("CoefId")]
		public Coefficient Coefficient { get; set; }

		[ForeignKey("StockListId")]
		public StockList StockList { get; set; }
	}
}