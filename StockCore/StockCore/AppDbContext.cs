using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StocksAnalyzer.Data;

namespace StocksAnalyzerWebApp.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public DbSet<Stock> Stocks { get; set; }
		public DbSet<StockList> StockLists { get; set; }
		public DbSet<StockListToStock> StockListToStock { get; set; }
		

	}
}
