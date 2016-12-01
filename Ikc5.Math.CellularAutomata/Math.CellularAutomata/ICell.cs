using System.ComponentModel;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Interface describes the cell of cellular automaton.
	/// </summary>
	public interface ICell : INotifyPropertyChanged
	{
		#region States

		/// <summary>
		/// Current state of the cell - Living/Dead.
		/// </summary>
		bool State { get; }

		/// <summary>
		/// Count of living cells among the current one and two nearest neighbors
		/// - above and below.
		/// </summary>
		short VertSum { get; }	// set; }

		/// <summary>
		/// Update count of living cells around current one.
		/// </summary>
		void AddVertSum(short delta);

		/// <summary>
		/// Age of the cell, so how many iteration it is living.
		/// </summary>
		short Age { get; }

		#endregion

		#region Buffered data during iteration

		/// <summary>
		/// Next state of the cell, used as buffered value during iteration.
		/// </summary>
		bool? NextState { get; set; }

		/// <summary>
		/// TRUE if next state and current state are different, otherwise are equal.
		/// </summary>
		bool IsChanged { get; }

		/// <summary>
		/// Integer equivalence of the difference betwee next state and current state:
		/// 0 - if they are the same;
		/// 1 - if cell will born;
		/// -1 - if cell wil die.
		/// </summary>
		short Delta { get; }

		/// <summary>
		/// Cell goes to next state.
		/// </summary>
		void ApplyChange();

		/// <summary>
		/// Methods initiate cell by default values.
		/// </summary>
		void Clear();

		#endregion
	}
}