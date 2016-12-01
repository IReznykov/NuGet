using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class CellSetTests
	{
		[Fact]
		public void CellSet_Should_Initialize()
		{
			const int width = 4;
			const int height = 7;

			var cellSet = new CellSet();
			cellSet.Init(new Size(width, height));

			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					((Cell)cellSet[x, y]).State = (x % 3 == 0) && (y % 2 == 0);
				}

			// checks
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					var cell = cellSet[x, y];
					if ((x % 3 == 0) && (y % 2 == 0))
						cell.Should().NotBeNull().And.BeLivingCell();
					else
						cell.Should().NotBeNull().And.BeDeadCell();
				}
		}

		[Fact]
		public void CellSet_Should_ReturnCellsInCyclic()
		{
			const int width = 11;
			const int height = 7;

			var cellSet = new CellSet();
			cellSet.Init(new Size(width, height));
			var pointList = new List<Point>();

			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					if (!((x % 2 == 0) && (y % 3 == 0)))
						continue;

					((Cell)cellSet[x, y]).State = true;
					pointList.AddRange(new[]
										{
											new Point(x, y),
											new Point(x, y + height),
											new Point(x, y + 2*height),
											new Point(x, y - height),
											new Point(x, y - 2*height),

											new Point(x + width, y),
											new Point(x + width, y + height),
											new Point(x + width, y + 2*height),
											new Point(x + width, y - height),
											new Point(x + width, y - 2*height),

											new Point(x + 2*width, y),
											new Point(x + 2*width, y + height),
											new Point(x + 2*width, y + 2*height),
											new Point(x + 2*width, y - height),
											new Point(x + 2*width, y - 2*height),

											new Point(x - width, y),
											new Point(x - width, y + height),
											new Point(x - width, y + 2*height),
											new Point(x - width, y - height),
											new Point(x - width, y - 2*height),

											new Point(x - 2*width, y),
											new Point(x - 2*width, y + height),
											new Point(x - 2*width, y + 2*height),
											new Point(x - 2*width, y - height),
											new Point(x - 2*width, y - 2*height),

					});
				}

			// checks
			for (var x = -width; x < 2 * width; x++)
				for (var y = -height; y < 2 * height; y++)
				{
					var cell = cellSet[x, y];
					if (pointList.Contains(new Point(x, y)))
						cell.Should().NotBeNull().And.BeLivingCell();
					else
					{
						cell.State.Should().BeFalse();
						//cell.Should().NotBeNull().And.BeDeadCell();
					}
				}
		}

	}
}
