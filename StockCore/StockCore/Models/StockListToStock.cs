using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Data;

namespace StocksAnalyzer.Data
{
	public class StockListToStock
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ForeignKey("StockListNameId")]
		public StockList StockListName { get; set; }

		[ForeignKey("StockId")]
		public Stock Stock { get; set; }

		public ICollection<CoefHasValueCount> CoefHasValueCountList { get; set; }
	}
}