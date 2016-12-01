using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class AutomatonAsyncTests
	{
		[Fact]
		public void Automaton_Should_AllowSequentialCallOfIterateFromOneTask()
		{
#if PUBLISH
			const int loops = 50;
#else
			const int loops = 1000;
#endif

			var automaton = TestsHelpers.CreateSpaceship();
			var statistics = new Statistics();

			var tasks = new[]
			{
				Task.Run(() =>
				{
					for (var pos = 0; pos < loops; pos++)
					{
						automaton.Iterate(ref statistics);
						Thread.Sleep(10);
					}
				})
			};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));

			Assert.NotNull(exception);
			Assert.Null(exception.Result);
		}

#if !PUBLISH
		[Fact]
#endif
		public void Automaton_Should_RejectSecondCallOfIterate()
		{
			// service inverts state of the cell, and put thread to sleep for 50 ms
			// 10*10 cells give total 5s of delay -> second task should be rejected
			var cellLifeService = new Mock<ICellLifeService>();
			cellLifeService.
				Setup(service => service.Iterate(It.IsAny<ICell>(), It.IsAny<ICell>(), It.IsAny<ICell>())).
				Callback((ICell cell1, ICell cell2, ICell cell3) =>
							{
								Thread.Sleep(50);
								cell2.NextState = !cell2.State;
							});

			var automaton = TestsHelpers.CreateSpaceship(cellLifeService.Object);
			var statistics1 = new Statistics();
			var statistics2 = new Statistics();
			var tasks = new[]
			{
				Task.Run(() =>
				{
					return automaton.Iterate(ref statistics1) ? 1 : 0;
				}),
				Task.Run(() =>
				{
					var pos = 0;
					for (; pos < 100; pos++)
					{
						Thread.Sleep(10);
						if (automaton.Iterate(ref statistics2))
							break;
					}
					return pos;
				})
			};

			// use int as returned value in order to catch how many
			// requests from second task were rejected
			int[] result = null;
			var exception = Record.ExceptionAsync(async () => result = await Task.WhenAll(tasks));

			Assert.NotNull(exception);
			Assert.Null(exception.Result);

			result.Should().NotBeNull();
			result.Length.Should().Be(2);
			result[0].Should().Be(1);
			result[1].Should().Be(100);
		}

	}
}
