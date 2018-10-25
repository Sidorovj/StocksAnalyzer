using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Data;

namespace StocksAnalyzer.Data
{
	public class StockList
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[ForeignKey("MarketId")]
		public StockMarket StockMarket { get; set; }

		public bool? TakeOnTinkoff { get; set; }

		public bool? IsStarred { get; set; }

		[ForeignKey("UserId")]
		public User User { get; set; }

		public ICollection<Rating> Ratings { get; set; }

		public ICollection<AveragePosition> AveragePositions { get; set; }

		[NotMapped] public ICollection<Stock> Stocks => StockListToStock 
	}
}