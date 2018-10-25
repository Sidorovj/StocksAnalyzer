using System;
using Microsoft.EntityFrameworkCore;
using StockCore.Helpers;
using StocksAnalyzer;
using StocksAnalyzer.Data;

namespace AdministratingCore
{
	class Program
	{
		static void Main(string[] args)
		{
			ApplicationContext ctx = new ApplicationContext(null, new MainClass());

			if (true)
			{
				ctx.StockMarketLocation.SeedEnumValues<StockMarketLocation, StockMarketLocationEnum>(@enum => @enum);
				ctx.SaveChanges();
			}
			else
			{
				var location = ctx.StockMarketLocation.Find(0);
			}
			//ctx.ExecuteCommand();

			Console.ReadLine();
		}
	}
}
