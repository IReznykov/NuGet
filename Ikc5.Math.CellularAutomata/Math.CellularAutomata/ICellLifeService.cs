namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Service calculates next states of the cells. Different
	/// implementations use different geometric distance.
	/// </summary>
	public interface ICellLifeService
	{
		/// <summary>
		/// Contains parameters for born/survive decisions.
		/// </summary>
		ILifePreset LifePreset { get; set; }

		/// <summary>
		/// Set next state of the cell - depends on used model
		/// they will survive, die, or born.
		/// </summary>
		void Iterate(ICell left, ICell current, ICell right);
	}
}
