using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StocksAnalyzer.Data;

namespace StockCore.Data
{
	public class CoefHasValueCount
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("StockListId")]
		public StockList StockList { get; set; }

		[ForeignKey("CoefficientId")]
		public Coefficient Coefficient { get; set; }

		public int Count { get; set; }
	}
}
