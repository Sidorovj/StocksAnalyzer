using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class AveragePosition
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("StockId")]
		[Required]
		public StocksAnalyzer.Data.Stock Stock { get; set; }

		[ForeignKey("StockListId")]
		public StockList StockList { get; set; }

		public int Position { get; set; }

		public bool? ForAll { get; set; }
		public bool? ForMetric { get; set; }
		public bool? ForCoefficient { get; set; }
	}
}