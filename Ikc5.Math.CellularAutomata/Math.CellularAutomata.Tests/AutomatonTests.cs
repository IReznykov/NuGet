using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using Ikc5.TypeLibrary.Logging;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class AutomatonTests
	{
		[Fact]
		public void Automaton_Should_Initialize()
		{
			const int width = 5;
			const int height = 8;

			var cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());

			// checks
			automaton.Should().NotBeNull();
			automaton.Size.Should().NotBeNull();
			automaton.Size.Width.Should().Be(width);
			automaton.Size.Height.Should().Be(height);

			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					var cell = automaton.GetCell(x, y);
					cell.Should().NotBeNull().And.BeDeadCell();
				}
		}

		[Theory]
		[InlineData(4)]
		[InlineData(3)]
		[InlineData(2)]
		[InlineData(1)]
		[InlineData(0)]
		[InlineData(-1)]
		public void Automaton_Should_IncreaseWidth(int width)
		{
			const int height = 7;

			var cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());

			// checks
			automaton.Should().NotBeNull();
			automaton.Size.Should().NotBeNull();
			automaton.Size.Width.Should().Be(5);
			automaton.Size.Height.Should().Be(height);
		}

		[Theory]
		[InlineData(4)]
		[InlineData(3)]
		[InlineData(2)]
		[InlineData(1)]
		[InlineData(0)]
		[InlineData(-1)]
		public void Automaton_Should_IncreaseHeight(int height)
		{
			const int width = 8;

			var cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());

			// checks
			automaton.Should().NotBeNull();
			automaton.Size.Should().NotBeNull();
			automaton.Size.Width.Should().Be(width);
			automaton.Size.Height.Should().Be(5);
		}

		[Fact]
		public void Automaton_Should_ReturnCellsInCyclic()
		{
			const int width = 8;
			const int height = 5;

			var cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());

			// checks
			for (var x = -width; x < 2 * width; x++)
				for (var y = -height; y < 2 * height; y++)
				{
					var cell = automaton.GetCell(x, y);
					cell.Should().NotBeNull().And.BeDeadCell();
				}
		}

		//[Theory]
		//[InlineData(3)]
		//[InlineData(2)]
		//[InlineData(1)]
		//[InlineData(0)]
		//[InlineData(-1)]
		public void Automaton_ShouldThrow_ExceptionOnSmallWidth(int width)
		{
			const int height = 7;
			var message = $"Width of automaton should be more than 3{Environment.NewLine}Parameter name: {nameof(Automaton.Size)}{Environment.NewLine}Actual value was {width}.";

			var cellLifeService = Mock.Of<ICellLifeService>();
			IAutomaton automaton = null;

			var exception = Record.Exception(() => automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>()));

			// checks
			automaton.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

		//[Theory]
		//[InlineData(3)]
		//[InlineData(2)]
		//[InlineData(1)]
		//[InlineData(0)]
		//[InlineData(-1)]
		public void Automaton_ShouldThrow_ExceptionOnSmallHeight(int height)
		{
			const int width = 7;
			var message = $"Height of automaton should be more than 3{Environment.NewLine}Parameter name: {nameof(Automaton.Size)}{Environment.NewLine}Actual value was {height}.";

			var cellLifeService = Mock.Of<ICellLifeService>();
			IAutomaton automaton = null;

			var exception = Record.Exception(() => automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>()));

			// checks
			automaton.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

		[Fact]
		public void Automaton_ShouldThrow_ExceptionOnNullService()
		{
			const int width = 4;
			const int height = 7;

			ICellLifeService cellLifeService = null;
			//var message = $"Exception of type 'System.ArgumentNullException' was thrown.{Environment.NewLine}Parameter name: {nameof(cellLifeService)}";
			var message = $"Value cannot be null.{Environment.NewLine}Parameter name: {nameof(cellLifeService)}";
			IAutomaton automaton = null;

			var exception = Record.Exception(() => automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>()));

			// checks
			automaton.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

		[Fact]
		public void Automaton_Should_ClearAllCells()
		{
			const int width = 4;
			const int height = 7;

			var cellLifeService = Mock.Of<ICellLifeService>();
			var automaton = new Automaton(width, height, cellLifeService, Mock.Of<ILogger>());

			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					((Cell)automaton.GetCell(x, y)).State = (x % 3 == 0) && (y % 2 == 0);
				}

			automaton.Clear();

			// checks
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					var cell = automaton.GetCell(x, y);
					cell.Should().NotBeNull().And.BeDeadCell();
				}
		}

		[Fact]
		public void Automaton_Should_SetPoint()
		{
			const int width = 10;
			const int height = 10;

			var cellLifeService = Mock.Of<ICellLifeService>();
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
			var vertSumCollection = new Dictionary<Point, short>
			{
				[new Point(1, 0)] = 1,
				[new Point(1, 1)] = 1,
				[new Point(1, 2)] = 2,
				[new Point(1, 3)] = 1,
				[new Point(1, 4)] = 1,
				[new Point(2, 3)] = 1,
				[new Point(2, 4)] = 1,
				[new Point(2, 5)] = 1,
				[new Point(3, 3)] = 1,
				[new Point(3, 4)] = 1,
				[new Point(3, 5)] = 1,
				[new Point(4, 0)] = 1,
				[new Point(4, 1)] = 1,
				[new Point(4, 2)] = 1,
				[new Point(4, 3)] = 1,
				[new Point(4, 4)] = 1,
				[new Point(4, 5)] = 1,
				[new Point(5, 0)] = 0,
				[new Point(5, 1)] = 1,
				[new Point(5, 2)] = 2,
				[new Point(5, 3)] = 3,
				[new Point(5, 4)] = 2,
				[new Point(5, 5)] = 1,
			};

			var statistics = new Statistics();
			automaton.SetPoints(pointList, ref statistics);

			// checks
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					var point = new Point(x, y);
					var cell = automaton.GetCell(x, y);
					cell.Should().NotBeNull();

					if (pointList.Contains(point))
						cell.State.Should().BeTrue();
					else
						cell.State.Should().BeFalse();

					if (vertSumCollection.ContainsKey(point))
						cell.VertSum.Should().Be(vertSumCollection[point]);
					else
						cell.VertSum.Should().Be(0);
				}
		}

		[Fact]
		public void Automaton_Should_ClearAfterSetPoint()
		{
			const int width = 10;
			const int height = 10;

			var cellLifeService = Mock.Of<ICellLifeService>();
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
			automaton.Clear();

			// checks
			for (var x = 0; x < width; x++)
				for (var y = 0; y < height; y++)
				{
					var cell = automaton.GetCell(x, y);
					cell.Should().NotBeNull().And.BeDeadCell();
				}
		}

		[Fact]
		public void Automaton_Should_CheckStatusForEachCell()
		{
			var cellLifeService = new Mock<ICellLifeService>();
			cellLifeService.
				Setup(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>()));

			var automaton = TestsHelpers.CreateSpaceship(cellLifeService.Object);
			var expectedCount = automaton.Size.Width * automaton.Size.Height;
			var statistics = new Statistics();
			var result = automaton.Iterate(ref statistics);

			result.Should().Be(true);  // there is no iterations, no changes
			cellLifeService.Verify(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>()), Times.Exactly(expectedCount));
			statistics.Borned.Should().Be(0);
			statistics.Died.Should().Be(0);
		}

		[Fact]
		public void Automaton_Should_ChangeAllCells()
		{
			// service inverts state of the cell, and put thread to sleep for 30 ms
			// 10*10 cells give total 3s of delay -> second task should be rejected
			var cellLifeService = new Mock<ICellLifeService>();
			cellLifeService.
				Setup(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>())).
				Callback((ICell left, ICell current, ICell right) => current.NextState = !current.State);

			var automaton = TestsHelpers.CreateSpaceship(cellLifeService.Object);
			var expectedCount = automaton.Size.Width * automaton.Size.Height;
			var cellCount = automaton.Count;

			var statistics = new Statistics();
			var result = automaton.Iterate(ref statistics);

			result.Should().Be(true);
			cellLifeService.Verify(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>()), Times.Exactly(expectedCount));
			statistics.Borned.Should().Be(expectedCount - cellCount);
			statistics.Died.Should().Be(-cellCount);
		}

		[Fact]
		public void Automaton_Should_ChangeLiveCells()
		{
			// service inverts state of the cell, and put thread to sleep for 30 ms
			// 10*10 cells give total 3s of delay -> second task should be rejected
			var cellLifeService = new Mock<ICellLifeService>();
			cellLifeService.
				Setup(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>())).
				Callback((ICell left, ICell current, ICell right) => current.NextState = (current.State ? (bool?)false : null));

			var automaton = TestsHelpers.CreateSpaceship(cellLifeService.Object);
			var expectedCount = automaton.Size.Width * automaton.Size.Height;
			var liveCount = automaton.GetPoints().Count();

			var statistics = new Statistics();
			var result = automaton.Iterate(ref statistics);

			result.Should().Be(true);
			cellLifeService.Verify(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>()), Times.Exactly(expectedCount));
			statistics.Borned.Should().Be(0);
			statistics.Died.Should().Be(-liveCount);
		}

	}
}
