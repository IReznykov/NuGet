using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class MooreCellLifeServiceTests
	{
		[Fact]
		public void CellLifeService_Should_BeInitialize()
		{
			var lifeCellService = new MooreCellLifeService(Mock.Of<ILifePreset>());

			lifeCellService.Should().NotBeNull();
		}

		[Fact]
		public void CellLifeService_ShouldThrow_ExceptionOnNullLifePreset()
		{
			ICellLifeService lifeCellService = null;
			ILifePreset lifePreset = null;
			var message = $"Value cannot be null.{Environment.NewLine}Parameter name: {nameof(lifePreset)}";
			//var message = $"Exception of type 'System.ArgumentNullException' was thrown.{Environment.NewLine}Parameter name: {nameof(lifePreset)}";
			var exception = Record.Exception(() => lifeCellService = new MooreCellLifeService(lifePreset));

			lifeCellService.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

#if !PUBLISH
       [Fact]
#endif
		public void CellLifeService_ShouldCorrectly_CalculatesNeighbors()
		{
			foreach (var leftState in new[] { false, true })
				for (var leftVertSum = 0; leftVertSum <= 2; leftVertSum++)
				{
					var leftCell = Mock.Of<ICell>(cell => cell.State == leftState && cell.VertSum == leftVertSum + (leftState ? 1 : 0));

					foreach (var rightState in new[] { false, true })
						for (var rightVertSum = 0; rightVertSum <= 2; rightVertSum++)
						{
							var rightCell = Mock.Of<ICell>(cell => cell.State == rightState && cell.VertSum == rightVertSum + (rightState ? 1 : 0));

							for (var bornCount = 0; bornCount <= 8; bornCount++)
								for (var surviveCount = 0; surviveCount <= 8; surviveCount++)
								{
									var lifePreset = Mock.Of<ILifePreset>();
									Mock.Get(lifePreset).Setup(preset => preset.Born(It.Is<int>(value => value == bornCount))).Returns(true);
									Mock.Get(lifePreset).Setup(preset => preset.Survive(It.Is<int>(value => value == surviveCount))).Returns(true);

									foreach (var currentState in new[] { false, true })
										for (var currentVertSum = 0; currentVertSum <= 2; currentVertSum++)
										{
											var currentCell = Mock.Of<ICell>(cell => cell.State == currentState && cell.VertSum == currentVertSum + (currentState ? 1 : 0));
											bool? expectedNextState = false;
											if (currentState)
											{
												expectedNextState = (leftCell.VertSum + currentCell.VertSum + rightCell.VertSum - 1) == surviveCount;
											}
											else
											{
												expectedNextState = (leftCell.VertSum + currentCell.VertSum + rightCell.VertSum) == bornCount;
											}
											if (expectedNextState.Value == currentState)
												expectedNextState = null;

											// execution
											var lifeCellService = new MooreCellLifeService(lifePreset);
											lifeCellService.Iterate(leftCell, currentCell, rightCell);

											// asserts
											lifeCellService.Should().NotBeNull();
											//currentCell.NextState.Should().HaveValue().And.Be(expectedNextState);
											currentCell.NextState.Should().Be(expectedNextState);
										}
								}
						}
				}
		}

	}
}
