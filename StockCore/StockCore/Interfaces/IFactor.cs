

namespace StockCore.Interfaces
{
	/// <summary>
	/// Коэффициент
	/// </summary>
	public interface IFactor
	{
		string Name { get; }
		string Label { get; }
		string Tooltip { get; }
	}
}