using System.Collections.Generic;
using System.Drawing;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Describe cellular automaton. 
	/// </summary>
	public interface IAutomaton
	{
		/// <summary>
		/// Size of cellular automaton.
		/// </summary>
		Size Size { get; }

		/// <summary>
		/// Return count of living cells.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Series of cells count depending on their age.
		/// </summary>
		IEnumerable<int> AgeSeries { get; }

		/// <summary>
		/// Method sets initial cells to automaton.
		/// </summary>
		/// <param name="newPoints">Set of new cells that are added to automaton.</param>
		/// <param name="statistics">Statistics with count of changed cells</param>
		void SetPoints(IEnumerable<Point> newPoints, ref Statistics statistics);

		/// <summary>
		/// Method update current cells.
		/// </summary>
		/// <param name="addedPoints">Set of new cells that are added to automaton.</param>
		/// <param name="removedPoints">Set of cells that are killed in automaton.</param>
		/// <param name="statistics">Statistics with count of changed cells</param>
		void UpdatePoints(IEnumerable<Point> addedPoints, IEnumerable<Point> removedPoints, ref Statistics statistics);

		/// <summary>
		/// Method returns current automaton points.
		/// </summary>
		IEnumerable<Point> GetPoints();

		/// <summary>
		/// Clear all cells.
		/// </summary>
		void Clear();

		/// <summary>
		/// Moves cells to the next stage - depends on used model
		/// they will survive, die, or born.
		/// </summary>
		bool Iterate(ref Statistics statistics);

		/// <summary>
		/// Returns (reference to) cell object at specified coordinates.
		/// </summary>
		/// <param name="x">Row.</param>
		/// <param name="y">Column.</param>
		/// <returns></returns>
		ICell GetCell(int x, int y);
	}
}