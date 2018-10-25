using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Helpers;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer.Data
{
	public sealed class StockMarketLocation
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(100)]
		public string Description { get; set; }

		public static implicit operator StockMarketLocation(StockMarketLocationEnum @enum) => new StockMarketLocation(@enum);

		public static implicit operator StockMarketLocationEnum(StockMarketLocation faculty) => (StockMarketLocationEnum)faculty.Id;

		private StockMarketLocation(StockMarketLocationEnum @enum)
		{
			Id = (int)@enum;
			Name = @enum.ToString();
			Description = @enum.GetEnumDescription();
		}

		private StockMarketLocation()
		{
		} //For EF
	}
}