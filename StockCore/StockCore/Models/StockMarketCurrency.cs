using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Helpers;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer.Data
{
	public sealed class StockMarketCurrency
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int Id { get; set; }

		[Required, MaxLength(100)]
		public string Name { get; set; }

		[MaxLength(100)]
		public string Description { get; set; }

		public static implicit operator StockMarketCurrency(StockMarketCurrencyEnum @enum) => new StockMarketCurrency(@enum);

		public static implicit operator StockMarketCurrencyEnum(StockMarketCurrency faculty) =>
			(StockMarketCurrencyEnum) faculty.Id;

		private StockMarketCurrency(StockMarketCurrencyEnum @enum)
		{
			Id = (int)@enum;
			Name = @enum.ToString();
			Description = @enum.GetEnumDescription();
		}

		private StockMarketCurrency()
		{
		} //For EF
	}
}