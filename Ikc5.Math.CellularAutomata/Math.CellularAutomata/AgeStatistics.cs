using System.Collections.Generic;
using System.Linq;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// 
	/// </summary>
	public class AgeStatistics
	{
		private readonly IDictionary<int, int> _ages = new Dictionary<int, int>(100);

		public int this[int index]
		{
			get { return (_ages.ContainsKey(index)) ? _ages[index] : 0; }
			set
			{
				_ages[index] = value;
				if (value == 0)
					_ages.Remove(index);
			}
		}

		public IEnumerable<int> GetSeries()
		{
			var maxIndex = _ages.Count == 0 ? -1 : _ages.Keys.Max();
			for (var index = 0; index <= maxIndex; index++)
			{
				yield return this[index];
			}
		}

		public void Borned()
		{
			--this[0];
			++this[1];
		}

		public void Survive(int age)
		{
			--this[age];
			++this[age + 1];
		}

		public void Died(int age)
		{
			--this[age];
			++this[0];
		}

		public void Init(int count)
		{
			_ages.Clear();
			_ages[0] = count;
		}
	}
}
