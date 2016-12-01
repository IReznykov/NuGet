using System.Drawing;

namespace Ikc5.Math.CellularAutomata
{
	/// <summary>
	/// Synonym for complex Cells object and some useful additions.
	/// Cells are stored in an array.
	/// </summary>
	public class CellSet
	{
		private ICell[,] _cells;

		public CellSet()
		{
		}

		/// <summary>
		/// Access operator by two coordinates.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public ICell this[int x, int y]
		{
			get { return _cells[GetXIndex(x), GetYIndex(y)]; }
			set { _cells[GetXIndex(x), GetYIndex(y)] = value; }
		}

		private int GetXIndex(int x)
		{
			if (_cells.LongLength == 0)
				return 0;
			var count = _cells.GetLength(0);
			return GetIndex(x, count);
		}

		private int GetYIndex(int y)
		{
			if (_cells.LongLength == 0)
				return 0;
			var count = _cells.GetLength(1);
			return GetIndex(y, count);
		}

		/// <summary>
		/// Checks coordinates whether it in the range,
		/// and correct it if necessary.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private static int GetIndex(int pos, int count)
		{
			if (0 <= pos && pos < count)
				return pos;
			else if (pos > 0)
				return pos % count;
			else
				return (pos + ((-pos / count) + 1) * count) % count;
		}

		/// <summary>
		/// Initialization of _cells.
		/// </summary>
		/// <param name="size"></param>
		public void Init(Size size)
		{
			_cells = new ICell[size.Width, size.Height];

			for (var x = 0; x < size.Width; x++)
				for (var y = 0; y < size.Height; y++)
				{
					_cells[x, y] = new Cell();
				}
		}
	}
}
