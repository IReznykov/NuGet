using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ikc5.Math.CellularAutomata.Tests
{
	public class AgeStatisticsTests
	{
		[Fact]
		public void AgeStatistics_Should_Initialize()
		{
			var ageStatistics = new AgeStatistics();
			ageStatistics.Should().NotBeNull();

			// series
			var series = ageStatistics.GetSeries();
			series.Should().NotBeNull();
			series.Count().Should().Be(0);
		}
	}

}
