using FlightTracker.ViewModels;
using FlightTracker.Services;
using FluentAssertions;
using Xunit;
using System.Linq;

namespace FlightTracker.Tests;

public class InfoFlightViewModelTests
{
    [Fact]
    public void InfoFlightViewModel_Initialization_PopulatesCollections()
    {
        // Arrange & Act
        var service = new FlightAndAirportService(); 
        var viewModel = new InfoFlightViewModel(service);

        // Assert
        viewModel.Airports.Should().NotBeEmpty();
        viewModel.Flights.Should().NotBeEmpty();
    }

    [Fact]
    public void InfoFlightViewModel_ClearSelectionCommand_ResetsState()
    {
        // Arrange
        var service = new FlightAndAirportService(); 
        var viewModel = new InfoFlightViewModel(service);
        viewModel.SelectedAirport = viewModel.Airports.First();
        viewModel.SearchText = "Test";

        // Act
        viewModel.ClearSelectionCommand.Execute(null);

        // Assert
        viewModel.SelectedAirport.Should().BeNull();
        viewModel.SearchText.Should().BeEmpty();
    }

    [Fact]
    public void InfoFlightViewModel_FilterFlightsLanded_FiltersByAirportAndStatus()
    {
        // Arrange
        var service = new FlightAndAirportService();
        var airport = service.Airports.First(a =>
            service.Flights.Any(f => f.DepartureAirport == a.IataCode && f.Status == "Landed"));

        var expectedFlights = service.Flights
            .Where(f => f.DepartureAirport == airport.IataCode && f.Status == "Landed")
            .ToList();

        var viewModel = new InfoFlightViewModel(service);
        viewModel.SelectedAirport = airport;

        // Act
        viewModel.FilterFlightsLandedCommand.Execute(null);

        // Assert
        viewModel.FilteredFlights.Should().HaveCount(expectedFlights.Count);
        viewModel.FilteredFlights.Should().OnlyContain(f =>
            f.DepartureAirport == airport.IataCode && f.Status == "Landed");
    }
}