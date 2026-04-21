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
}