using FlightTracker.ViewModels;
using FluentAssertions;
using Xunit;
using System;

namespace FlightTracker.Tests;

public class AnalyticsViewModelTests
{
    [Fact]
    public void AnalyticsViewModel_SelectedDateSetToNull_ClearsCharts()
    {
        // Arrange
        var viewModel = new AnalyticsViewModel();
        viewModel.SelectedDate = DateTimeOffset.Now;

        // Act
        viewModel.SelectedDate = null;

        // Assert
        viewModel.Series.Should().BeEmpty();
        viewModel.RouteSeries.Should().BeEmpty();
        viewModel.BusiestAirportSeries.Should().BeEmpty();
    }
}