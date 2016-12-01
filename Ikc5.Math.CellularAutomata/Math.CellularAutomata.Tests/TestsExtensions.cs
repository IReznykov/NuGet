using FluentAssertions;
using FluentAssertions.Primitives;
using Moq;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public static class TestsExtensions
	{
		public static AndConstraint<TAssertions> Stub<TSubject, TAssertions>(
			this TAssertions assertion,
			string because = "") where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
		{
			return new AndConstraint<TAssertions>(assertion);
		}

		public static AndConstraint<ObjectAssertions> BeDeadCell(
			this ObjectAssertions assertion,
			string because = "")
		{
			var cell = assertion.Subject as ICell;

			Assert.NotNull(cell);

			cell.Should().NotBeNull(because);
			cell.State.Should().BeFalse(because);
			cell.NextState.Should().Be(null, because);
			cell.VertSum.Should().Be(0, because);
			cell.IsChanged.Should().BeFalse(because);
			cell.Delta.Should().Be(0, because);

			return new AndConstraint<ObjectAssertions>(assertion);
		}

		public static AndConstraint<ObjectAssertions> BeDeadCellNearOther(
			this ObjectAssertions assertion,
			string because = "")
		{
			var cell = assertion.Subject as ICell;

			Assert.NotNull(cell);

			cell.Should().NotBeNull(because);
			cell.State.Should().BeFalse(because);
			cell.NextState.Should().Be(null, because);
			//cell.VertSum.Should().Be(0, because);
			cell.IsChanged.Should().BeFalse(because);
			cell.Delta.Should().Be(0, because);

			return new AndConstraint<ObjectAssertions>(assertion);
		}

		public static AndConstraint<ObjectAssertions> BeLivingCell(
			this ObjectAssertions assertion,
			string because = "")
		{
			var cell = assertion.Subject as ICell;

			Assert.NotNull(cell);

			cell.Should().NotBeNull(because);
			cell.State.Should().BeTrue(because);
			cell.NextState.Should().Be(null, because);
			cell.VertSum.Should().Be(1, because);
			cell.IsChanged.Should().BeFalse(because);
			cell.Delta.Should().Be(0, because);

			return new AndConstraint<ObjectAssertions>(assertion);
		}

		public static AndConstraint<ObjectAssertions> BeLivingCellNearOther(
			this ObjectAssertions assertion,
			string because = "")
		{
			var cell = assertion.Subject as ICell;

			Assert.NotNull(cell);

			cell.Should().NotBeNull(because);
			cell.State.Should().BeTrue(because);
			cell.NextState.Should().Be(null, because);
			//cell.VertSum.Should().Be(1, because);
			cell.IsChanged.Should().BeFalse(because);
			cell.Delta.Should().Be(0, because);

			return new AndConstraint<ObjectAssertions>(assertion);
		}

		public static Mock<T> GetMock<T>(this T mocked) where T : class
		{
			return Mock.Get(mocked);
		}

	}
}
