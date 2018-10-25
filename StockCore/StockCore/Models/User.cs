using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StockCore.Interfaces;

// ReSharper disable InconsistentNaming

namespace StocksAnalyzer.Data
{
	public sealed class User
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Fill user name")]
		public string Name { get; set; }
	}
}