using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksAnalyzerWebApp.Data;

namespace StockCore
{
	public class StockRepository
	{
		private readonly ApplicationDbContext m_ctx;

		public StockRepository(ApplicationDbContext ctx)
		{
			m_ctx = ctx;
		}

		public async Task<int> Save()
		{
			return await m_ctx.SaveChangesAsync();
		}
	}
}
