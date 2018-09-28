
namespace StocksAnalyzer.Core.Interfaces
{
	/// <summary>
	/// Шлет прогресс (0-100) во View progress bar
	/// </summary>
	public interface IReportProgress
	{
		int Value { get; set; }
	}
}
