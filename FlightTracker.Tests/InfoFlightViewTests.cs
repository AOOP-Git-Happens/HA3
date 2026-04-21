using FlightTracker.ViewModels;
using FlightTracker.Services; // Fixes Errors 3 & 4
using FluentAssertions;
using Xunit;
using System.Linq;

namespace FlightTracker.Tests;

public class InfoFlightViewModelTests
{
    [Fact]
    public void InfoFlightViewModel_ClearSelection_SetsNull()
    {
        // Arrange
        var testService = new FlightAndAirportService(); 
        var viewModel = new InfoFlightViewModel(testService);

        // Act
        viewModel.ClearSelectionCommand.Execute(null);

        // Assert
        viewModel.SelectedAirport.Should().BeNull();
        viewModel.SearchText.Should().BeEmpty();
    }
}