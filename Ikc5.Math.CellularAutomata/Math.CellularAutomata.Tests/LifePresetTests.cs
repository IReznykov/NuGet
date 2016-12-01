using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class LifePresetTests
	{
		private readonly IEnumerable<int> _bornValues = new List<int>(new[] { 2, 3 });

		private readonly IEnumerable<int> _surviveValues = new List<int>(new[] { 2, 3, 4, 7 });

		[Fact]
		public void LifePreset_Should_BeInitialize()
		{
			var lifePreset = new LifePreset(_bornValues, _surviveValues);

			lifePreset.Should().NotBeNull();
		}

		[Fact]
		public void LifePreset_Should_IgnoreWrongBornCounts()
		{
			ILifePreset lifePreset = null;
			var exception = Record.Exception(() => lifePreset = new LifePreset(new[] { -1, -2, 2, 3, 8, 9, 15 }, _surviveValues));

			lifePreset.Should().NotBeNull();
			exception.Should().BeNull();

			for (var pos = -10; pos < 20; pos++)
			{
				var actualResult = lifePreset.Born(pos);
				var expectedResult = _bornValues.Contains(pos) || pos == 8;

				actualResult.Should().Be(expectedResult);
			}
		}

		[Fact]
		public void LifePreset_Should_IgnoreWrongSurviveCounts()
		{
			ILifePreset lifePreset = null;
			var exception = Record.Exception(() => lifePreset = new LifePreset(_bornValues, new[] { -1, -2, 2, 3, 4, 7, 9, 15 }));

			lifePreset.Should().NotBeNull();
			exception.Should().BeNull();

			for (var pos = -10; pos < 20; pos++)
			{
				var actualResult = lifePreset.Survive(pos);
				var expectedResult = _surviveValues.Contains(pos) || pos == 7;

				actualResult.Should().Be(expectedResult);
			}
		}

		[Fact]
		public void LifePreset_ShouldThrow_ExceptionOnNullBorn()
		{
			ILifePreset lifePreset = null;
			//var message = $"Exception of type 'System.ArgumentNullException' was thrown.{Environment.NewLine}Parameter name: {"bornValues"}";
			var message = $"Value cannot be null.{Environment.NewLine}Parameter name: {"bornValues"}";
			var exception = Record.Exception(() => lifePreset = new LifePreset(null, _surviveValues));

			lifePreset.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

		[Fact]
		public void LifePreset_ShouldThrow_ExceptionOnNullSurvive()
		{
			ILifePreset lifePreset = null;
			//var message = $"Exception of type 'System.ArgumentNullException' was thrown.{Environment.NewLine}Parameter name: {"surviveValues"}";
			var message = $"Value cannot be null.{Environment.NewLine}Parameter name: {"surviveValues"}";
			var exception = Record.Exception(() => lifePreset = new LifePreset(_bornValues, null));

			lifePreset.Should().BeNull();
			exception.Should().NotBeNull();
			exception.Message.Should().Be(message);
		}

		[Fact]
		public void LifePreset_Should_CorrectlyBornCells()
		{
			var lifePreset = new LifePreset(_bornValues, _surviveValues);

			lifePreset.Should().NotBeNull();
			for (var pos = 0; pos < 10; pos++)
			{
				var actualResult = lifePreset.Born(pos);
				var expectedResult = _bornValues.Contains(pos);

				actualResult.Should().Be(expectedResult);

				actualResult = lifePreset.Survive(pos);
				expectedResult = _surviveValues.Contains(pos);

				actualResult.Should().Be(expectedResult);
			}
		}

	}
}
