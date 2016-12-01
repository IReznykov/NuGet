using System;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class NeumannCellLifeServiceTests
	{
		[Fact]
		public void CellLifeService_Should_BeInitialize()
		{
			var lifeCellService = new NeumannCellLifeService(Mock.Of<ILifePreset>());

			lifeCellService.Should().NotBeNull();
		}

		[Fact]
		public void CellLifeService_ShouldThrow_ExceptionOnNullLifePreset()
		{
			ICellLifeService lifeCellService = null;
			ILifePreset lifePreset = null;
			//var message = $"Exception of type 'System.ArgumentNullException' was thrown.{Environment.NewLine}Parameter name: {nameof(lifePreset)}";
			var message = $"Value cannot be null.{Environment.NewLine}Parameter name: {nameof(lifePreset)}";
			var exception = Record.Exception(() => lifeCellService = new NeumannCellLifeService(null));

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
			{
				var leftCell = Mock.Of<ICell>(cell => cell.State == leftState && cell.VertSum == (leftState ? 1 : 0));

				foreach (var rightState in new[] { false, true })
				{
					var rightCell = Mock.Of<ICell>(cell => cell.State == rightState && cell.VertSum == (rightState ? 1 : 0));

					for (var bornCount = 0; bornCount <= 4; bornCount++)
						for (var surviveCount = 0; surviveCount <= 4; surviveCount++)
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
									var lifeCellService = new NeumannCellLifeService(lifePreset);
									lifeCellService.Iterate(leftCell, currentCell, rightCell);

									// asserts
									lifeCellService.Should().NotBeNull();
									currentCell.NextState.Should().Be(expectedNextState);
								}
						}
				}
			}
		}
	}
}
