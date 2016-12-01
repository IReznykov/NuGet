using System.Drawing;

namespace Ikc5.Math.CellularAutomata
{
	public static class SizeExtensions
	{
		public static string ToString(this Size size)
		{
			return $"({size.Width}, {size.Height})";
		}

		public static bool Contains(this Size size, Point point)
		{
			return (0 <= point.X && point.X < size.Width) && (0 <= point.Y && point.Y < size.Height);
		}

		public static bool Inside(this Size size, Point point)
		{
			return (1 <= point.X && point.X < size.Width - 1) && (0 < point.Y && point.Y < size.Height - 1);
		}
	}
}
