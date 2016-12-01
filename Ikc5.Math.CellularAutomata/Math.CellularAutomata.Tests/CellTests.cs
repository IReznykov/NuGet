using System;
using FluentAssertions;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	/// <summary>
	/// Simple tests for cell's properties.
	/// </summary>
	public class CellTests
	{
		[Fact]
		public void Cell_Should_BeInitializedEmpty()
		{
			var cell = new Cell();

			cell.Should().BeDeadCell();
		}

		[Fact]
		public void Cell_Should_IgnoreAssignStateToFalse()
		{
			var cell = new Cell
			{
				State = false
			};

			cell.Should().BeDeadCell();
		}

		[Fact]
		public void Cell_Should_CorrectlyAssignStateToTrue()
		{
			var cell = new Cell
			{
				State = true
			};

			cell.Should().BeLivingCell();
		}

		[Fact]
		public void Cell_Should_CorrectlyRevertStateToFalse()
		{
			var cell = new Cell
			{
				State = true
			};

			cell.Should().BeLivingCell();

			cell.State = false;
			cell.Should().BeDeadCell();
		}

		[Theory]
		[InlineData(false, 1, 0, 1)]
		[InlineData(false, 1, 1, 2)]
		[InlineData(false, 1, -1, 0)]
		[InlineData(true, 0, -1, 0)]
		[InlineData(true, 1, 0, 2)]
		[InlineData(true, 2, 0, 3)]
		[InlineData(true, 2, -1, 2)]
		[InlineData(true, 2, -3, 0)]
		public void Cell_Should_CorrectlyAssignVertSum(bool state, short initialVertSum, short vertSum, short expectVertSum)
		{
			var cell = new Cell
			{
				VertSum = initialVertSum,
				State = state,
			};

			cell.VertSum += vertSum;

			cell.State.Should().Be(state);
			cell.VertSum.Should().Be(expectVertSum);
		}


		[Theory]
		[InlineData(false, 1, -2)]
		[InlineData(false, 0, -2)]
		[InlineData(true, 0, -2)]
		[InlineData(true, 2, -4)]
		public void Cell_Should_ThrowExceptionOnNegativeVertSum(bool state, short initialVertSum, short vertSum)
		{
			var cell = new Cell
			{
				VertSum = initialVertSum,
				State = state,
			};

			var exception = Record.Exception(() => cell.VertSum += vertSum);

			Assert.NotNull(exception);
			Assert.IsType<ArgumentException>(exception);
			exception.Message.Should().Be($"Vertical count of cells should be non-negative{Environment.NewLine}Parameter name: {nameof(cell.VertSum)}");
		}

		[Theory]
		[InlineData(false, null, false, null, 0, false, 0)]
		[InlineData(true, null, true, null, 1, false, 0)]
		[InlineData(false, false, false, false, 0, false, 0)]
		[InlineData(true, false, true, false, 1, true, -1)]
		[InlineData(false, true, false, true, 0, true, 1)]
		[InlineData(true, true, true, true, 1, false, 0)]
		public void Cell_Should_CorrectlyUpdateNextState(bool state, bool? nextState,
			bool expectState, bool? expectNextState, short expectVertSum, bool expectIsChanged, short expectDelta)
		{
			var cell = new Cell();

			cell.State = state;
			cell.NextState = nextState;

			cell.State.Should().Be(expectState);
			cell.NextState.Should().Be(expectNextState);
			cell.VertSum.Should().Be(expectVertSum);
			cell.IsChanged.Should().Be(expectIsChanged);
			cell.Delta.Should().Be(expectDelta);
		}

		[Theory]
		[InlineData(false, null, false, 0, 0)]
		[InlineData(true, null, true, 1, 2)]
		[InlineData(false, false, false, 0, 0)]
		[InlineData(true, false, false, 0, 0)]
		[InlineData(false, true, true, 1, 1)]
		[InlineData(true, true, true, 1, 2)]
		public void Cell_Should_CorrectlyChangeValuesAfterNewState(bool state, bool? nextState,
			bool expectState, short expectVertSum, short expectedAge)
		{
			var cell = new Cell();

			cell.State = state;
			cell.NextState = nextState;
			cell.ApplyChange();

			cell.State.Should().Be(expectState);
			cell.NextState.Should().Be(null);
			cell.VertSum.Should().Be(expectVertSum);
			cell.IsChanged.Should().Be(false);
			cell.Delta.Should().Be(0);
			cell.Age.Should().Be(expectedAge);
		}
	}
}
