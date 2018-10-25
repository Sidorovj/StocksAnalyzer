using System.Collections.Generic;
using System.Threading.Tasks;
using StocksAnalyzer.Core.Interfaces;

namespace StockCore.Interfaces
{
	public interface IStockLoader
	{
		/// <summary>
		/// Загрузить список всех акций
		/// </summary>
		/// <param name="lbl">Nullable, для сообщения информации</param>
		/// <param name="bar">Nullable, для отображения прогресса</param>
		/// <param name="onlyCheckForTinkoff">Все акции загружать не надо, только проверить, какие есть в тинькофф</param>
		/// <returns></returns>
		Task GetStocksList(IReportText lbl = null, IReportProgress bar = null, bool onlyCheckForTinkoff = false);

		/// <summary>
		/// Загрузить значения коэффициентов для акции
		/// </summary>
		/// <param name="st">Акция</param>
		Task GetStockData(StocksAnalyzer.Data.Stock st);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="stockList">Список акций, для которого получить коэффициенты</param>
		/// <param name="lbl">Nullable, для сообщения информации</param>
		/// <param name="bar">Nullable, для отображения прогресса</param>
		Task LoadStocksData(IEnumerable<StocksAnalyzer.Data.Stock> stockList, IReportText lbl = null, IReportProgress bar = null);
	}
}
