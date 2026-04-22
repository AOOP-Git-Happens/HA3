using FlightTracker.ViewModels;
using FlightTracker.Services;
using FluentAssertions;
using LiveChartsCore.SkiaSharpView;
using Xunit;
using System;
using System.Linq;

namespace FlightTracker.Tests;

public class AnalyticsViewModelTests
{
    [Fact]
    public void AnalyticsViewModel_SelectedDateSetToNull_ClearsCharts()
    {
        // Arrange
        var service = new FlightAndAirportService();
        var viewModel = new AnalyticsViewModel(service);
        viewModel.SelectedDate = DateTimeOffset.Now;

        // Act
        viewModel.SelectedDate = null;

        // Assert
        viewModel.Series.Should().BeEmpty();
        viewModel.RouteSeries.Should().BeEmpty();
        viewModel.RouteXAxes.Should().BeEmpty();
        viewModel.BusiestAirportSeries.Should().BeEmpty();
        viewModel.BusiestAirportXAxes.Should().BeEmpty();
    }

    [Fact]
    public void AnalyticsViewModel_SelectedDate_BuildsAirlineSeriesFromFlights()
    {
        //Arrange
        var service = new FlightAndAirportService();
        var testDate = service.Flights.First().ScheduledDeparture.Date;

        var expectedAirlines = service.Flights
        .Where(f => f.ScheduledDeparture.Date == testDate)
        .GroupBy(f => f.AirlineName)
        .Select(g => new { Name = g.Key, Count = g.Count() })
        .OrderBy(x => x.Name)
        .ToList();

        var viewModel = new AnalyticsViewModel(service);

        // Act
        viewModel.SelectedDate = testDate;

        //Assert
        var actualAirlines = viewModel.Series
        .Cast<PieSeries<int>>()
        .Select(s => new { Name = s.Name, Count = s.Values.Cast<int>().Single() })
        .OrderBy(x => x.Name)
        .ToList();

        actualAirlines.Should().BeEquivalentTo(expectedAirlines);
    }

    [Fact]
    public void AnalyticsViewModel_SelectedDate_BuildsTopFiveRoutesCorrectly()
    {
        // Arrange
        var service = new FlightAndAirportService();
        var testDate = service.Flights.First().ScheduledDeparture.Date;

        var expectedRoutes = service.Flights
            .Where(f => f.ScheduledDeparture.Date == testDate)
            .GroupBy(f => f.DepartureAirport + " ➔ " + f.ArrivalAirport)
            .Select(g => new { RouteName = g.Key, FlightCount = g.Count() })
            .OrderByDescending(x => x.FlightCount)
            .Take(5)
            .ToList();

        var viewModel = new AnalyticsViewModel(service);

        // Act
        viewModel.SelectedDate = new DateTimeOffset(testDate);

        // Assert
        viewModel.RouteSeries.Should().HaveCount(1);
        viewModel.RouteXAxes.Should().HaveCount(1);

        var routeSeries = viewModel.RouteSeries[0] as ColumnSeries<int>;
        routeSeries.Should().NotBeNull();

        var actualCounts = ExtractValues(routeSeries!).ToList();
        var actualLabels = viewModel.RouteXAxes[0].Labels?.ToList() ?? new List<string>();

        actualLabels.Should().Equal(expectedRoutes.Select(x => x.RouteName));
        actualCounts.Should().Equal(expectedRoutes.Select(x => x.FlightCount));
    }

    [Fact]
    public void AnalyticsViewModel_SelectedDate_BuildsTopFiveDepartureAirportsCorrectly()
    {
        // Arrange
        var service = new FlightAndAirportService();
        var testDate = service.Flights.First().ScheduledDeparture.Date;

        var expectedAirports = service.Flights
            .Where(f => f.ScheduledDeparture.Date == testDate)
            .GroupBy(f => f.DepartureAirport)
            .Select(g => new { AirportCode = g.Key, FlightCount = g.Count() })
            .OrderByDescending(x => x.FlightCount)
            .Take(5)
            .ToList();

        var viewModel = new AnalyticsViewModel(service);

        // Act
        viewModel.SelectedDate = new DateTimeOffset(testDate);

        // Assert
        viewModel.BusiestAirportSeries.Should().HaveCount(1);
        viewModel.BusiestAirportXAxes.Should().HaveCount(1);

        var airportSeries = viewModel.BusiestAirportSeries[0] as ColumnSeries<int>;
        airportSeries.Should().NotBeNull();

        var actualCounts = ExtractValues(airportSeries!).ToList();
        var actualLabels = viewModel.BusiestAirportXAxes[0].Labels?.ToList() ?? new List<string>();

        actualLabels.Should().Equal(expectedAirports.Select(x => x.AirportCode));
        actualCounts.Should().Equal(expectedAirports.Select(x => x.FlightCount));
    }

    private static IEnumerable<int> ExtractValues(ColumnSeries<int> series)
    {
        if (series.Values is null)
            return Enumerable.Empty<int>();

        return series.Values.Cast<int>();
    }
}