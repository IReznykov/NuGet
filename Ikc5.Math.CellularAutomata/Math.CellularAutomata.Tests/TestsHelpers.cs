using System.Drawing;
using Ikc5.TypeLibrary.Logging;
using Moq;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public static class TestsHelpers
	{
		public static IAutomaton CreateSpaceship(ICellLifeService cellLifeService = null)
		{
			const int width = 10;
			const int height = 10;

			if (cellLifeService == null)
				cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());
			var pointList = new[]
			{
				// Lightweight spaceship
				new Point(1, 1),
				new Point(4, 1),
				new Point(5, 2),
				new Point(1, 3),
				new Point(5, 3),
				new Point(2, 4),
				new Point(3, 4),
				new Point(4, 4),
				new Point(5, 4),
			};
			var statistics = new Statistics();
			automaton.SetPoints(pointList, ref statistics);

			return automaton;
		}

	}
}
