using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	/// <summary>
	/// Tests change values of the cell in several threads.
	/// </summary>
	public class CellAsyncTests
	{
		[Fact]
		public async void Cell_Should_BeTestedAsync()
		{
			var cell = new Cell();

			await Task.Run(() =>
			{
				for (var pos = 0; pos < 5; pos++)
				{
					cell.State = true;
					Thread.Sleep(5);
				}
			});

			cell.State.Should().BeTrue();
			cell.VertSum.Should().Be(1);
		}

		[Fact]
		public void Cell_Should_CorrectlyLiveAndDieInTwoThreads()
		{
#if PUBLISH
			const int loops = 20;
#else
			const int loops = 200;
#endif
			var cell = new Cell();
			var tasks = new Task[]
			{
				Task.Run(() =>
				{
					for (var pos = 0; pos < 200; pos++)
					{
						cell.State = true;
						Thread.Sleep(7);
					}
				}),
				Task.Run(() =>
				{
					for (var pos = 0; pos < loops; pos++)
					{
						cell.State = false;
						Thread.Sleep(11);
					}
				}),
			};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));

			Assert.NotNull(exception);
			Assert.Null(exception.Result);
		}

		[Fact]
		public void Cell_Should_CorrectlyLiveAndDieAndChangeVertSumInTwoThreads()
		{
#if PUBLISH
			const int loops = 100;
#else
			const int loops = 20000;
#endif
			var cell = new Cell();
			var tasks = new Task[]
			{
				Task.Run(() =>
				{
					for (var pos = 0; pos < loops; pos++)
					{
						cell.State = (pos % 2 == 0);
					}
				}),
				Task.Run(() =>
				{
					for (var pos = 0; pos < loops; pos++)
					{
						cell.AddVertSum((short)(pos % 2 == 0 ? 1 : -1));
					}
				}),
			};

			var exception = Record.ExceptionAsync(async () => await Task.WhenAll(tasks));

			Assert.NotNull(exception);
			Assert.Null(exception.Result);
		}

	}
}