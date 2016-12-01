using System.Collections.Generic;
using System.Linq;
using Ikc5.TypeLibrary;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Contains parameters for born/survive decisions.
	/// </summary>
	public class LifePreset : ILifePreset
	{
		private const int MaxNeighborCount = 9;

		private readonly bool[] _bornValues = new bool[MaxNeighborCount];

		private readonly bool[] _surviveValues = new bool[MaxNeighborCount];

		private readonly IEnumerable<int> _range = Enumerable.Range(0, MaxNeighborCount);

		private LifePreset()
		{ }

		public LifePreset(IEnumerable<int> bornValues, IEnumerable<int> surviveValues)
			: this()
		{
			bornValues.ThrowIfNull(nameof(bornValues));
			surviveValues.ThrowIfNull(nameof(surviveValues));

			foreach (var value in bornValues)
			{
				if (_range.Contains(value))
					_bornValues[value] = true;
			}
			foreach (var value in surviveValues)
			{
				if (_range.Contains(value))
					_surviveValues[value] = true;
			}
		}

		/// <summary>
		/// Returns will cell born if it has exact neighbors count.
		/// </summary>
		/// <param name="neighborCount">Neighbors count</param>
		/// <returns></returns>
		public bool Born(int neighborCount)
		{
			return _range.Contains(neighborCount) && _bornValues[neighborCount];    
		}

		/// <summary>
		/// Returns will cell survive if it has exact neighbors count.
		/// </summary>
		/// <param name="neighborCount">Neighbors count</param>
		/// <returns></returns>
		public bool Survive(int neighborCount)
		{
			return _range.Contains(neighborCount) && _surviveValues[neighborCount];
		}
	}
}
