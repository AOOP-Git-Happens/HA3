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
}