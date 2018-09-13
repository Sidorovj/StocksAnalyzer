
namespace StocksAnalyzer.Core.Interfaces
{
	/// <summary>
	/// Пишет справочную информацию о текущей стадии операции во View
	/// </summary>
	public interface IReportText
	{
		string Text { get; set; }
	}
}
