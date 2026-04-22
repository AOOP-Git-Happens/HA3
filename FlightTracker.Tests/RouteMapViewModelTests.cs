using FlightTracker.ViewModels;
using FlightTracker.Services;
using FluentAssertions;
using Xunit;
using System.Linq;

namespace FlightTracker.Tests;

public class RouteMapViewModelTests
{
    [Fact]
    public void RouteMapViewModel_ClearSelectionCommand_ResetsState()
    {
        // Arrange
        var service = new FlightAndAirportService(); 
        var viewModel = new RouteMapViewModel(service);
        viewModel.SelectedAirport = viewModel.AllAirports.First();
        viewModel.SearchText = "Test";

        // Act
        viewModel.ClearSelectionCommand.Execute(null);

        // Assert
        viewModel.SelectedAirport.Should().BeNull();
        viewModel.SearchText.Should().BeEmpty();
    }

    [Fact]
    public void RouteMapViewModel_SelectedAirportChanged_FiresMapRefreshEvent()
    {
        // Arrange
        var service = new FlightAndAirportService(); 
        var viewModel = new RouteMapViewModel(service);
        bool eventFired = false;
        
        viewModel.MapRequestRefresh += () => eventFired = true;

        // Act
        viewModel.SelectedAirport = viewModel.AllAirports.First();

        // Assert
        eventFired.Should().BeTrue();
    }

    [Fact]
    public void RouteMapViewModel_SelectedAirport_UpdatesDistinctDestinationAirports()
    {
        // Arrange
        var service = new FlightAndAirportService();
        var airport = service.Airports.First(a =>
            service.Flights.Any(f => f.DepartureAirport == a.IataCode));

        var expectedDestinationCodes = service.Flights
            .Where(f => f.DepartureAirport == airport.IataCode)
            .Select(f => f.ArrivalAirport)
            .Distinct()
            .Where(code => code != airport.IataCode)
            .Join(
                service.Airports,
                code => code,
                a => a.IataCode,
                (code, a) => code)
            .OrderBy(code => code)
            .ToList();

        var viewModel = new RouteMapViewModel(service);

        // Act
        viewModel.SelectedAirport = airport;

        // Assert
        var actualDestinationCodes = viewModel.DestinationAirports
            .Select(a => a.IataCode)
            .OrderBy(code => code)
            .ToList();

        actualDestinationCodes.Should().Equal(expectedDestinationCodes);
    }
}