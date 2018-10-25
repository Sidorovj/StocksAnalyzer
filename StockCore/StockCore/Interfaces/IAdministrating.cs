using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksAnalyzer.Core.Interfaces;

namespace StockCore.Interfaces
{
	public interface IAdministrating
	{
		/// <summary>
		/// Загрузить новые акции
		/// </summary>
		/// <returns></returns>
		Task CreateNewStocks(bool deleteOld = false);

		void CreateCoefficientsAndMetrics(bool deleteOld = false);
	}
}
