using Ikc5.TypeLibrary;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Service calculates next states of the cells.
	/// Distance by Moore equals count of neighbors at distance 1, up to 8.
	/// </summary>
	public class MooreCellLifeService : ICellLifeService
	{
		public MooreCellLifeService(ILifePreset lifePreset)
		{
			lifePreset.ThrowIfNull(nameof(lifePreset));
			LifePreset = lifePreset;
		}

		public ILifePreset LifePreset { get; set; }

		#region ICellLifeService

		/// <summary>
		/// Set next state of the cell - depends on used model
		/// they will survive, die, or born.
		/// Next state is obtained value only when state should be changed. It is ok
		/// if iterate algorithm works in synchronous way. If iterate algorithm works in
		/// parallel mode and need to check whether particular cell was considered, NextState
		/// should be assigned to bool value.
		/// </summary>
		public void Iterate(ICell left, ICell current, ICell right)
		{
			var neighborCount = left.VertSum + current.VertSum + right.VertSum;
			var nextState = current.State ?
								LifePreset.Survive(neighborCount - 1) :
								LifePreset.Born(neighborCount);

			current.NextState = (nextState != current.State) ? (bool?)nextState : null;
		}

		#endregion
	}
}
