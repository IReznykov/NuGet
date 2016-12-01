using System.Collections.Generic;
using System.Drawing;
using FluentAssertions;
using Ikc5.TypeLibrary.Logging;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class AutomatonIntegrationTests
	{

		[Fact]
		public void Automaton_Should_ChangeLightingInMooreAndLife()
		{
			const int width = 10;
			const int height = 10;

			var cellLifeService = new MooreCellLifeService(KnownLifePresets.Life);
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());
			var pointList = new List<Point>(
				new[]{
					// Lighting
					new Point(4, 3),
					new Point(4, 4),
					new Point(4, 5),
				});
			var secondPointList = new List<Point>(
				new[]{
					new Point(3, 4),
					new Point(4, 4),
					new Point(5, 4),
				});

			var statistics = new Statistics();
			automaton.SetPoints(pointList, ref statistics);

			for (var iteration = 0; iteration < 10; iteration++)
			{
				// iteration
				//       X
				// from  X  to XXX and backward
				//       X
				//
				automaton.Iterate(ref statistics);

				// checks
				statistics.Borned.Should().Be(2);
				statistics.Died.Should().Be(-2);
				automaton.Count.Should().Be(3);

				for (var x = 0; x < width; x++)
					for (var y = 0; y < height; y++)
					{
						var cell = automaton.GetCell(x, y);
						var point = new Point(x, y);

						if (iteration % 2 == 0 && secondPointList.Contains(point))
						{
							cell.Should().BeLivingCellNearOther();
							if (x == y)
							{
								cell.Age.Should().Be((short) (iteration + 2));
							}
							else
							{
								cell.Age.Should().Be(1);
							}
						}
						else if (iteration % 2 == 1 && pointList.Contains(point))
						{
							cell.Should().BeLivingCellNearOther();
							if (x == y)
							{
								cell.Age.Should().Be((short)(iteration + 2));
							}
							else
							{
								cell.Age.Should().Be(1);
							}
						}
						else
							cell.Should().NotBeNull().And.BeDeadCellNearOther();
					}
			}
		}

		//[Fact]
		//public void Automaton_Should_ChangeLightingInMooreAndLife()
		//{
		//	const int width = 10;
		//	const int height = 10;

		//	var cellLifeService = new MooreCellLifeService(KnownLifePresets.Life);
		//	var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());
		//	var pointList = new[]
		//	{
		//		// Lightweight spaceship
		//		new Point(1, 1),
		//		new Point(4, 1),
		//		new Point(5, 2),
		//		new Point(1, 3),
		//		new Point(5, 3),
		//		new Point(2, 4),
		//		new Point(3, 4),
		//		new Point(4, 4),
		//		new Point(5, 4),
		//	};
		//	automaton.SetPoints(pointList);

		//	automaton.Clear();

		//	// checks
		//	for (var x = 0; x < width; x++)
		//		for (var y = 0; y < height; y++)
		//		{
		//			var cell = automaton.GetCell(x, y);
		//			cell.Should().NotBeNull().And.BeDeadCell();
		//		}
		//}

	}
}
